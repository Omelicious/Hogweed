using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;


public class Painter : MonoBehaviour
{
    // i chose to cache the fields to reduce memory allocation work
    [SerializeField] private int brushRadius = 20;
    [SerializeField] private Material circleMat;
    [SerializeField] private Camera camera;
    public RenderGraph renderGraph;
    private RaycastHit hit;

    #region Terrain Specific Fields
    private float pointX;
    private float pointY;
    private int mapX;
    private int mapY;
    private int zeroX;
    private int zeroY;
    private int mapWidth;
    private int mapHeight;
    #endregion
    
    void Start()
    {
        // renderGraph = new RenderGraph("DrawCircle");
    }

    public void PaintTerrain()
    {
        // raycast
        if(!Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 10f, 1 << LayerMask.NameToLayer("Ground")))
            return;
        
        // hit -> terrain, coords
        Terrain terrain = hit.collider.GetComponent<Terrain>();
        GrassSpawner spawner = terrain.GetComponent<GrassSpawner>();

        if (terrain == null)
            return;

        // localize coords for terrain
        pointX = hit.point.x - terrain.transform.position.x;
        pointY = hit.point.z - terrain.transform.position.z;
        
        // make coords relative (percents, 1000 will be 1, 0 will be 0)
        pointX = pointX / 1000; // terrain is 1000 units
        pointY = pointY / 1000; // long and wide

        // localize coords for map
        mapX = (int)(pointX * terrain.terrainData.alphamapWidth);
        mapY = (int)(pointY * terrain.terrainData.alphamapHeight);

        // Debug.Log($"Raycast hit at {hit.point.x}, {hit.point.z} converted to {mapX}, {mapY});");

        // for getting map
        int radius = brushRadius;

        zeroX = mapX - radius;
        zeroY = mapY - radius;
        mapWidth = 2 * radius;
        mapHeight = 2 * radius;

        // cut the map, so it doesn't go over the border
        if (zeroX < 0)
            zeroX = 0;
        if (zeroY < 0)
            zeroY = 0;
        if (zeroX + mapWidth >= terrain.terrainData.alphamapWidth)
            mapWidth -= zeroX + mapWidth - terrain.terrainData.alphamapWidth;
        if (zeroY + mapHeight >= terrain.terrainData.alphamapWidth)
            mapHeight -= zeroY + mapHeight - terrain.terrainData.alphamapHeight;

        // Debug.Log($"float [,,] map = terrain.terrainData.GetAlphamaps({zeroX}, {zeroY}, {mapWidth}, {mapHeight});");

        float [,,] map = terrain.terrainData.GetAlphamaps(zeroX, zeroY, mapWidth, mapHeight);

        radius--; // don't really know why, but it doesn't look the way it supposed to w/o it

        // changing color on terrain (coords)

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int layer = 0; layer < terrain.terrainData.alphamapLayers; layer++)
                {
                        // if out of circle. (mapX - radius - mapX + x) - there is radius, it's just better this way
                    if (((zeroX - mapX + x)*(zeroX - mapX + x) + (zeroY - mapY + y)*(zeroY - mapY + y)) > radius*radius)
                        continue;

                    if (layer != 1)
                    {
                        map[y, x, layer] = 0f;  // why tf GetAlphamaps returns y, x, layer and not x, y, layer?
                        continue;
                    }

                    map[y, x, layer] = 1f;
                }
            }
        }

        terrain.terrainData.SetAlphamaps(zeroX, zeroY, map);

        spawner.SpawnGrass(map, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight, zeroX, zeroY, 10);
    }

    public void PaintWallBlit()
    {
        if (!Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 10f, 1 << LayerMask.NameToLayer("Wall")))
            return;

        Paintable wall = hit.collider.GetComponent<Paintable>();

        if (wall == null)
            return;

        
        //Debug.Log($"Center was at {circleMat.GetVector("_Center")}");
        circleMat.SetVector("Center", hit.textureCoord);
        //Debug.Log($"Center is at {circleMat.GetVector("_Center")}");
        
        //Debug.Log($"Radius was {circleMat.GetFloat("_Radius")}");
        circleMat.SetFloat("Radius", (float)brushRadius / wall.rTexture.height); //circleMat.mainTexture.height
        //Debug.Log($"Radius is {circleMat.GetFloat("_Radius")}");

        // copy - change copy - paste copy - release copy
        // RenderTexture tempRT = RenderTexture.GetTemporary(wall.rTexture.descriptor);
        
        // Graphics.Blit(wall.rTexture, tempRT); // copy A to B

        // Graphics.Blit(wall.rTexture, tempRT, circleMat);
        
        //Graphics.Blit(circleMat.mainTexture, wall.rTexture, circleMat); //should output the circle

        // wall.rTexture = tempRT;
        
        // RenderTexture.ReleaseTemporary(tempRT);

        //wall.material.SetTexture("_PaintMap", wall.rTexture);

        RTHandle rHandle = RTHandles.Alloc(wall.rTexture, false);
        RTHandle tHandle = RTHandles.Alloc(circleMat.mainTexture);
        TextureHandle handle1 = renderGraph.ImportTexture(rHandle);
        TextureHandle handle2 = renderGraph.ImportTexture(tHandle);
        RenderGraphUtils.AddBlitPass(renderGraph, new RenderGraphUtils.BlitMaterialParameters(handle1, handle2, circleMat, 0), "Draw circle", false); //should output the circle
        rHandle.Release();
        tHandle.Release();
    }

    public void PaintWallCPU()
    {
        if (!Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 10f, 1 << LayerMask.NameToLayer("Wall")))
            return;

        Paintable wall = hit.collider.GetComponent<Paintable>();

        if (wall == null)
            return;

        var pixels = wall.paintMap.GetRawTextureData<byte>();

        int radius = brushRadius - 1;

        int centerX = (int)(hit.textureCoord.x * wall.paintMap.width);
        int centerY = (int)(hit.textureCoord.y * wall.paintMap.height);

        for (int y = 0; y < wall.paintMap.height; y++)
        {
            for (int x = 0; x < wall.paintMap.width; x++)
            {
                if (((x - centerX)*(x - centerX) + (y - centerY)*(y - centerY)) > radius*radius)
                     continue;
                    
                pixels[y*wall.paintMap.width + x] = (byte)255;
            }
        }

        wall.paintMap.Apply();
    }
}
