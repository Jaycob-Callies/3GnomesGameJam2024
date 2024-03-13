using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpriteController : MonoBehaviour
{
    public Shader shader = null;
    public List<Texture2D> textures = null;
    private float ditherPercent = 0.5f;
    private float terrainScale = 0.5f;
    private int[] permutation = null;
    private List<Material> materialList = null;
    private List<GameObject> spriteLayers = null;

    public void setDitherChanges(float ditherPercent, List<float> spriteCutoff)
    {
        this.ditherPercent = ditherPercent;
        int spriteCount = spriteLayers.Count;
        for (int spriteIndex = 0; spriteIndex < spriteCount; spriteIndex++)
        {

            float ditherPrev = spriteIndex <= 1 ? -1f : spriteCutoff[spriteIndex - 2];
            float ditherCur = spriteIndex == 0 ? -1f : spriteCutoff[spriteIndex - 1];
            float ditherNext = spriteIndex == (spriteCount - 1) ? 1f : spriteCutoff[spriteIndex];

            float ditherMin = Mathf.Lerp(ditherCur, ditherPrev, ditherPercent * 0.5f);
            float ditherMax = Mathf.Lerp(ditherCur, ditherNext, ditherPercent * 0.5f);


            if (spriteIndex == 0)
            {
                ditherMin = ditherMax = -1.0f;
            }

            Debug.Log("Sprite " + spriteIndex + ": ( " + ditherMin.ToString() + ", " + ditherMax.ToString() + " )");

            spriteLayers[spriteIndex].GetComponent<MeshRenderer>().material.SetFloat("_DitherMax", ditherMax);
            spriteLayers[spriteIndex].GetComponent<MeshRenderer>().material.SetFloat("_DitherMin", ditherMin);
        }
    }

    public void setSimplexChanges(float terrainScale, int[] intPermutation)
    {
        this.terrainScale = terrainScale;

        List<float> floatPermutation = new List<float>();
        for (int pIndex = 0; pIndex < intPermutation.Length; pIndex++)
        {
            floatPermutation.Add((float)intPermutation[pIndex]);
        }
        foreach(GameObject layer in spriteLayers)
        {
            layer.GetComponent<MeshRenderer>().material.SetFloatArray("_Permutation", floatPermutation);
            layer.GetComponent<MeshRenderer>().material.SetFloat("_Scale", terrainScale);
        }
    }

    public void Initialize(Shader initShader, Texture2D initTexture, int[] intPermutation, List<float> spriteCutoff, float ditherPercent, float terrainScale)
    {
        this.ditherPercent = ditherPercent;
        //convert permutation
        List<float> floatPermutation = new List<float>();
        for (int pIndex = 0; pIndex < intPermutation.Length; pIndex++)
		{
            floatPermutation.Add((float)intPermutation[pIndex]);
		}


        List<Material> convertingMaterials = new List<Material>();
        int spriteCount = initTexture.height / initTexture.width;
        textures = new List<Texture2D>();
        spriteLayers = new List<GameObject>();
        for (int spriteIndex = 0; spriteIndex < spriteCount; spriteIndex++)
		{
            Texture2D croppedTexture = new Texture2D(initTexture.width, initTexture.width);
            Color[] colors = initTexture.GetPixels(0, initTexture.width * spriteIndex, initTexture.width, initTexture.width);
            croppedTexture.SetPixels(colors);
            croppedTexture.filterMode = FilterMode.Point;
            //croppedTexture.mip
            croppedTexture.Apply();
            textures.Add(croppedTexture);
            convertingMaterials.Add(new Material(initShader));
            convertingMaterials[convertingMaterials.Count - 1].SetTexture("_MainTex", textures[textures.Count - 1]);

            float ditherPrev = spriteIndex <= 1 ? -1f : spriteCutoff[spriteIndex - 2];
            float ditherCur = spriteIndex == 0 ? -1f : spriteCutoff[spriteIndex - 1];
            float ditherNext = spriteIndex == (spriteCount - 1) ? 1f : spriteCutoff[spriteIndex];

            float ditherMin = Mathf.Lerp(ditherCur, ditherPrev, ditherPercent * 0.5f);
            float ditherMax = Mathf.Lerp(ditherCur, ditherNext, ditherPercent * 0.5f);

            if (spriteIndex == 0)
			{
                ditherMin = ditherMax = -1.0f;
            }

            convertingMaterials[convertingMaterials.Count - 1].SetFloat("_DitherMax", ditherMax);
            convertingMaterials[convertingMaterials.Count - 1].SetFloat("_DitherMin", ditherMin);
            convertingMaterials[convertingMaterials.Count - 1].SetFloat("_Scale", terrainScale);
            convertingMaterials[convertingMaterials.Count - 1].SetFloatArray("_Permutation", floatPermutation);


            spriteLayers.Add(new GameObject());
            spriteLayers[spriteLayers.Count - 1].transform.parent = this.gameObject.transform;
            MeshRenderer tempR = spriteLayers[spriteLayers.Count - 1].AddComponent<MeshRenderer>();
            MeshFilter tempF = spriteLayers[spriteLayers.Count - 1].AddComponent<MeshFilter>();
            spriteLayers[spriteLayers.Count - 1].transform.position = Camera.main.transform.position + Vector3.back;
            spriteLayers[spriteLayers.Count - 1].transform.localScale = Camera.main.orthographicSize * Vector3.one;
            tempR.material = convertingMaterials[convertingMaterials.Count - 1];
            float cameraX = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.farClipPlane - 1.0f )).x - Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.farClipPlane - 1.0f)).x;
            float cameraY = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.farClipPlane - 1.0f )).x - Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.farClipPlane - 1.0f)).x;
            Mesh m = new Mesh();
            m.vertices = new Vector3[4] {
                new Vector3(-cameraX / 2f,-cameraY / 2f,0),
                new Vector3(cameraX / 2f,-cameraY / 2f,0),
                new Vector3(-cameraX / 2f,cameraY / 2f,0),
                new Vector3(cameraX / 2f,cameraY / 2f,0)
            };
            m.triangles = new int[6]
            {
                0,2,1,2,3,1
            };
            m.normals = new Vector3[4]
            {
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward
            };
            m.uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            tempF.mesh = m;

            //tempSP.sprite = Sprite.Create(initTexture, new Rect(0, initTexture.width * spriteIndex, initTexture.width, initTexture.width), Vector2.zero);
        }
        this.shader = initShader;
        this.permutation = new int[256];
        for(int i = 0; i < 256 ; i++)
		{
            this.permutation[i] = intPermutation[i];
		}
    }

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
        for(int spriteIndex = 0; spriteIndex < spriteLayers.Count; spriteIndex++)
		{
            spriteLayers[spriteIndex].transform.position = Camera.main.transform.position + (Vector3.forward * ((Camera.main.farClipPlane / 2f) - (spriteIndex * 1f)));
            //g.transform.position
            //g.transform.position = new Vector3(g.transform.position.x - Camera.main.orthographicSize, g.transform.position.y - Camera.main.orthographicSize, g.transform.position.z) ;
            //g.transform.localScale = Camera.main.orthographicSize * Vector3.one;
        }
    }
}
