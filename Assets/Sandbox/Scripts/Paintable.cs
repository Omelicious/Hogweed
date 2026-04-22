using UnityEngine;

public class Paintable : MonoBehaviour
{
    public RenderTexture rTexture;
    public Texture2D paintMap;
    public Material material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<Renderer>().material; // mat instance

        // rTexture = new RenderTexture(material.mainTexture.width, material.mainTexture.height, 0, RenderTextureFormat.R8);
        paintMap = new Texture2D(material.mainTexture.width, material.mainTexture.height, TextureFormat.R8, false);

        paintMap.SetPixelData(new byte [paintMap.width * paintMap.height], 0); // setting black

        paintMap.Apply();

        // if (!rTexture.IsCreated())
            // rTexture.Create();

        material.SetTexture("_PaintMap", paintMap);
    }

    void OnDestroy()
    {
        // if (rTexture.IsCreated())
            // rTexture.Release();
    }
}
