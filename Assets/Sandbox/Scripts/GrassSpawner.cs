using UnityEngine;
using System.Collections.Generic;

public class GrassSpawner : MonoBehaviour
{
    Terrain terrain;
    [SerializeField] Mesh grassMesh;
    [SerializeField] Material grassMat;
    List<Matrix4x4> grassInstances = new List<Matrix4x4>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        terrain = GetComponent<Terrain>();

        float [,,] map = terrain.terrainData.GetAlphamaps(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight); // full alphamap

        if (map == null) Debug.Log("map is null");
        else Debug.Log("map is present");
        if (terrain == null) Debug.Log("terrain is null");
        else Debug.Log("terrain is present");

        Vector3 center = terrain.transform.position + new Vector3(terrain.terrainData.size.x / 2, 0, terrain.terrainData.size.z / 2);
        Debug.Log($"Instancing at {center}...");
        grassInstances.Add(Matrix4x4.TRS(center, Quaternion.identity, Vector3.one * 100f));

        SpawnGrass(map, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight, 0, 0, 100);

        // spawn grass
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 [] matrices = grassInstances.ToArray();

        Graphics.DrawMeshInstanced(grassMesh, 0, grassMat, matrices);
    }

    public void SpawnGrass (float[,,] map, int width, int height, int zeroX, int zeroY, int amount) // spawn grass in the map snapshot
    {
        //  generate random coords within width and height (multiple)
        bool [,] isPlanted = new bool [height, width];
        for (int i = 0; i < amount; i++)
            isPlanted [(int)(Random.value * 100000) / height, (int)(Random.value * 100000)  / width] = true;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //  if [y, x] is 1f and is generated
                if (isPlanted [y, x])
                    Debug.Log($"Planted at {y}, {x}");
            }
        }

        //  for loop y->x->layer (layer is 1)
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //  if [y, x] is 1f and is generated
                if (!isPlanted [y, x])
                    continue;

                if (!(map [y, x, 1] == 1f))
                    continue;
                
                // spawn at x + zeroX, y + zeroY
                
                Debug.Log($"Instancing... {y}y, {x}x");

                Debug.Log($"Trying to add an instance at {(float)(zeroX + x) / terrain.terrainData.alphamapWidth * 1000 + terrain.transform.position.x}, 0, {(float)(zeroY + y) / terrain.terrainData.alphamapWidth * 1000 + terrain.transform.position.z}\nx: {zeroX + x} -> {(float)(zeroX + x) / terrain.terrainData.alphamapWidth} -> {(float)(zeroX + x) / terrain.terrainData.alphamapWidth * 1000} -> {(float)(zeroX + x) / terrain.terrainData.alphamapWidth * 1000 + terrain.transform.position.x}");

                grassInstances.Add(Matrix4x4.TRS(new Vector3(
                (float)(zeroX + x) / terrain.terrainData.alphamapWidth * 1000 + terrain.transform.position.x,
                0,
                (float)(zeroY + y) / terrain.terrainData.alphamapWidth * 1000 + terrain.transform.position.z),
                Quaternion.identity, Vector3.one * 100f)); // translate pos to transform

                Debug.Log($"Added instance at {(float)(zeroX + x) / terrain.terrainData.alphamapWidth * 1000 + terrain.transform.position.x}, 0, {(float)(zeroY + y) / terrain.terrainData.alphamapWidth * 1000 + terrain.transform.position.z}");
            }   
        }

        Debug.Log($"Instance count: {grassInstances.Count}");
        Matrix4x4 [] matrices = grassInstances.ToArray();

        Graphics.DrawMeshInstanced(grassMesh, 0, grassMat, matrices);
    }
}


//  so we store grass positions in marix list

//  we add to the list when draw on terrain
//  convert it to an array
//  then we draw the mesh

//  at the start we have the map and no positions
//  we look at it and add positions to the list
//  list stays while terrain exists
//  new grass -> add to the list.   np

//  problems:
//  1. how much we draw at the start?                                                               - static field?
//  2. positions aren't consistent between different starts                                         - static field?
//  3. not guaranteed to place requested amount of instances (pixel misses)                         - check how much is placed then subtract from requested
//      3.1. not guaranteed to place instances at blank spots somewhat consistently                 - modify map based on previous one (subtract)
//  4. when brush moves the code doesn't consider the existing ones. it'll become too "crowded"     - check how much is placed then subtract from requested
//  5. random only gives small numbers. it's okay when player does the brushing, but not in Start() - use different method?

//  idea for spreading the grass:
//  1. set offset (for example 10 - grass is spawned each 10 units)
//  2. iterate the offsets (10 - proceed only if x % 10 == 0)
//  3. add random offset (0, 10 + randomX, randomY = 1.7, 5.3)