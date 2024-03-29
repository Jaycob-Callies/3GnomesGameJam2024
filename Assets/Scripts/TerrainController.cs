using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decoration{
    public float minHeight, maxHeight;
    public Sprite sprite;
}
public class SimplexNoise
{
    /*
     * Adapted from Original Source Code for Simplex
     * https://www.researchgate.net/publication/216813608_Simplex_noise_demystified
    */

    private static float skewingFactor = (Mathf.Sqrt(2.0f + 1.0f) - 1.0f) / 2.0f;
    private static float unskewingFactor = (1.0f - (1.0f / Mathf.Sqrt(2.0f + 1.0f))) / 2.0f;
    private int[] permutation = new int[256];

    private static int[] ORIGINAL_PERMUTATION = new int[256]
	{
		151, 160, 137, 91,  90,  15,  131, 13,  201, 95,  96,  53,  194, 233, 7,  225, 
        140, 36,  103, 30,  69,  142, 8,   99,  37,  240, 21,  10,  23,  190, 6,  148, 
        247, 120, 234, 75,  0,   26,  197, 62,  94,  252, 219, 203, 117, 35,  11,  32,  
        57,  177, 33,  88,  237, 149, 56,  87,  174, 20,  125, 136,	171, 168, 68,  175, 
        74,  165, 71,  134, 139, 48,  27,  166, 77,  146, 158, 231, 83,  111, 229, 122, 
        60,  211, 133, 230, 220, 105, 92,  41,  55,  46,  245, 40,  244, 102, 143, 54,  
        65,  25,  63,  161, 1,   216, 80,  73,  209, 76,  132, 187, 208, 89,  18,  169, 
        200, 196, 135, 130, 116, 188, 159, 86,	164, 100, 109, 198, 173, 186, 3,   64,  
        52,  217, 226, 250, 124, 123, 5,   202, 38,  147, 118, 126, 255, 82,  85,  212, 
        207, 206, 59,  227, 47,  16,  58,  17,  182, 189, 28,  42,  223, 183, 170, 213, 
        119, 248, 152, 2,   44,  154, 163, 70,  221, 153, 101, 155, 167, 43,  172, 9,   
        129, 22,  39,  253,	19,  98,  108, 110, 79,  113, 224, 232, 178, 185, 112, 104, 
        218, 246, 97,  228, 251, 34,  242, 193, 238, 210, 144, 12,  191, 179, 162, 241, 
        81,  51,  145, 235, 249, 14,  239, 107, 49,  192, 214, 31,  181, 199, 106, 157, 
        184, 84,  204, 176, 115, 121, 50,  45,  127, 4,   150, 254, 138, 236, 205, 93,
		222, 114, 67,  29,  24,  72,  243, 141, 128, 195, 78,  66,  215, 61, 156,  180
	};

	private static Vector2[] GRADIENT_2D = new Vector2[9] 
    {
        new Vector2(1f, 1f), new Vector2(1f, 0f), new Vector2(1f, -1f),
        new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(0f, -1f),
        new Vector2(-1f, 1f), new Vector2(-1f, 0f), new Vector2(-1f, -1f)
	};

    public SimplexNoise() {
        ORIGINAL_PERMUTATION.CopyTo(this.permutation, 0);
    }

    private int hash(int i) {
        return permutation[i & 255];
    }

    private int fastFloor(float x)
    {
        return Mathf.FloorToInt(x);
    }

    private float dot(Vector2 gradient2D, float x, float y)
    {
        return gradient2D.x * x + gradient2D.y * y;
    }

    public float signedRawNoise(float xPos, float yPos, float scale = 1f)
	{
		xPos = xPos * scale;
		yPos = yPos * scale;

		float n0, n1, n2;

        // Skew the input space to determine which simplex cell we're in
        float skewedCell = (xPos + yPos) * skewingFactor;
        int xSimplexCell = fastFloor(xPos + skewedCell);
        int ySimplexCell = fastFloor(yPos + skewedCell);

        // Unskew the cell origin back to (x,y) space
        float unskewedCell = (xSimplexCell + ySimplexCell) * unskewingFactor;
        float X0 = xSimplexCell - unskewedCell;
        float Y0 = ySimplexCell - unskewedCell;
        float x0 = xPos - X0; // The x,y distances from the cell origin
        float y0 = yPos - Y0;

        // For the 2D case, the simplex shape is an equilateral triangle.
        // Determine which simplex we are in.
        // Offsets for second (middle) corner of simplex in (i,j) coords
        int i1, j1;
        if (x0 > y0)
        {
            i1 = 1;
            j1 = 0;
            // lower triangle, XY order: (0,0)->(1,0)->(1,1)
        }
        else
        {
            i1 = 0;
            j1 = 1;
        } // upper triangle, YX order: (0,0)->(0,1)->(1,1)

        // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
        // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
        // c = (3-sqrt(3))/6
        // Offsets for middle corner in (x,y) unskewed coords
        float x1 = x0 - i1 + unskewingFactor;
        float y1 = y0 - j1 + unskewingFactor;
        float x2 = x0 - 1.0f + 2.0f * unskewingFactor; // Offsets for last corner in (x,y) unskewed coords
        float y2 = y0 - 1.0f + 2.0f * unskewingFactor;

        // Work out the hashed gradient2D indices of the three simplex corners
        int ii = xSimplexCell & 255;
        int jj = ySimplexCell & 255;

        const int gradientsSize = 9;//GRADIENT_2D.Length;
        int gradientIndex0 = hash(ii + hash(jj)) % gradientsSize;
        int gradientIndex1 = hash(ii + i1 + hash(jj + j1)) % gradientsSize;
        int gradientIndex2 = hash(ii + 1 + hash(jj + 1)) % gradientsSize;

		// Calculate the contribution from the three corners
		n0 = calculateCornerValue(x0, y0, gradientIndex0);
		n1 = calculateCornerValue(x1, y1, gradientIndex1);
		n2 = calculateCornerValue(x2, y2, gradientIndex2);

        // Add contributions from each corner to get the final noise value.
        // The result is scaled to return values in the interval [-1,1].
        return 70.0f * (n0 + n1 + n2);
    }


    private float calculateCornerValue(float x, float y, int gradientIndex)
    {
        float corner = 0f;
        float t = 0.5f - (x * x) - (y * y);
        if (t > 0.0f)
        {
            t *= t;
            corner = t * t * dot(GRADIENT_2D[gradientIndex], x, y);
        }
        return corner;
    }

    public int randomizeSeed()
    {
        int seed = (int)(System.DateTime.UtcNow.Ticks);
        setSeed(seed);
        return seed;
    }

    public void setSeed(int seedNumber)
    {
        Random.InitState(seedNumber);
        ORIGINAL_PERMUTATION.CopyTo(this.permutation, 0);
        ShufflePermutation<int>(ref this.permutation);
    }

    private void ShufflePermutation<T>(ref T[] list)
    {
        int count = list.Length;
        int last = count - 1;
        for(int index = 0; index < last; ++index)
	    {
            int randomIndex = Random.Range(index, count);
            T temp = list[index];
            list[index] = list[randomIndex];
            list[randomIndex] = temp;
	    }
    }

    /**
     * Fractional Brownian Motion is the summation of successive octaves of noise, each with higher frequency and lower amplitude.
     *
     * @param xPos       - noise x value
     * @param yPos       - noise y value
     * @param octaves    - how many layers you are putting together (number of octaves determines how detailed the map will look)
     * @param lacunarity - Lacunarity is what makes the frequency grow as each octave the frequency is multiplied by the lacunarity
     * @param gain       - Gain, also called persistence, is what makes the amplitude shrink (or not shrink). Each octave the amplitude is multiplied by the gain

     * @return           - Fractional Brownian motion for noise value
     */
    float signedFBM(float xPos, float yPos, int octaves, float lacunarity, float gain)
    {
        float sum = 0.0f;
        float frequency = 1.0f;
        float amplitude = 1.0f;
        float maxValue = 0.0f;  // Used for normalizing result between -1.0 and 1.0
        for (int i = 0; i < octaves; ++i)
        {
            sum += signedRawNoise(xPos * frequency, yPos * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= gain;
            frequency *= lacunarity;
        }

        return sum / maxValue;
    }

    float unsignedFBM(float xPos, float yPos, int octaves, float lacunarity, float gain)
    {
        return signedFBM(xPos, yPos, octaves, lacunarity, gain) / 2.0f + 0.5f;
    }
    public int[] getPermutation()
	{
        int[] clonePermutation = new int[256];
		for (int i = 0; i < 256; i++)
		{
            clonePermutation[i] = this.permutation[i];
		}
        return clonePermutation;
	}
}

public class TerrainController : MonoBehaviour
{

    public Texture2D spriteMap = null;
	[Range(-1f, 1f)]
	public List<float> spriteCutoff = new List<float>();
    [Tooltip("Multiplies movement speed, Negative value denotes hard barriers / walls")]
    public List<float> collisionSpeed = new List<float>();
    public int seed = 0;
    public bool randomSeed = true;
    public Shader shader = null;
    public float terrainScale = 0.01f;
    [Range(0f, 1f)]
    public float ditherPercent = 0.5f;
    [Header("Decorations")]
	public List<GameObject> decorations = new List<GameObject>();
	[Range(-1f, 1f)]
	public List<float> decorationMinHeight = new List<float>();
    [Range(-1f, 1f)]
	public List<float> decorationMaxHeight = new List<float>();

	public int decoCount = 0;
    public float decoDistance = 100f;
	//public List<Sprite> decorations = new List<Sprite>();
	//public List<Sprite> decorations = new List<Sprite>();
	[HideInInspector]
    public List<Sprite> spriteList = new List<Sprite>();
    private SimplexNoise noise = new SimplexNoise();
    private TerrainCollisionController terrainCollisionController = null;
	private TerrainSpriteController terrainSpriteController = null;
	private DecorationSpawner decoSpawner = null;

	private bool updatedOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = this.gameObject;
        this.terrainCollisionController = go.AddComponent<TerrainCollisionController>();
		this.terrainSpriteController = go.AddComponent<TerrainSpriteController>();
		this.decoSpawner = go.AddComponent<DecorationSpawner>();

		this.noise = new SimplexNoise();
        this.noise.setSeed(this.seed);

        if (randomSeed)
        {
            this.seed = this.noise.randomizeSeed();
        }

        //Debug.Log("1");
        //Debug.Log(this.shader.name);
        //terrainSpriteController.Initialize(this.shader, spriteList, this.noise.getPermutation());



        //Debug.Log(sp.border);
        //UnityEditor.GridPalette.
    }

    // Update is called once per frame
    void Update()
	{
		if (!updatedOnce)
        {
            //Debug.Log("2");
            //Debug.Log(this.shader.name);
            //Debug.Log("3");
            //for(int i = 0; i< this.noise.getPermutation().Length; i = i + 16)
            //{
            //    string s = "";
            //    for (int j = i; j < i + 16; j++)
            //    {
            //        s = s + this.noise.getPermutation()[j] + ", ";
            //    }
            //    Debug.Log(s);
            //}
            this.spawnDecos();
            terrainSpriteController.Initialize(this.shader, spriteMap, this.noise.getPermutation(), this.spriteCutoff, this.ditherPercent, this.terrainScale);
            terrainCollisionController.Initialize(this.seed, this.spriteCutoff, this.collisionSpeed, this.ditherPercent, this.terrainScale);
			updatedOnce = !updatedOnce;
        }
    }

	private void OnValidate()
	{
        int spriteCount = spriteMap.height / spriteMap.width;
        while(spriteCount != (spriteCutoff.Count + 1))
		{
            if (spriteCount > (spriteCutoff.Count + 1))
            {
                spriteCutoff.Add(spriteCutoff.Count > 0 ? spriteCutoff[spriteCutoff.Count - 1] : 0.5f);
            }
            else // (spriteList.Count < (spriteCutoff.Count + 1))
            {
                spriteCutoff.RemoveAt(spriteCutoff.Count - 1);
            }
        }
        spriteCutoff.Sort();
        if (this.terrainSpriteController != null)
        {
            terrainSpriteController.setDitherChanges(this.ditherPercent, this.spriteCutoff);
            this.noise.setSeed(this.seed);
            terrainSpriteController.setSimplexChanges(this.terrainScale, this.noise.getPermutation());
        }

		while (spriteCount > collisionSpeed.Count)
		{
			collisionSpeed.Add(1f);
		}
		while (spriteCount < collisionSpeed.Count)
		{
			collisionSpeed.RemoveAt(collisionSpeed.Count - 1);
		}


		int decorationCount = decorations.Count;
		while (decorationCount > decorationMaxHeight.Count)
		{
			decorationMaxHeight.Add(1f);
		}
		while (decorationCount < decorationMaxHeight.Count)
		{
			decorationMaxHeight.RemoveAt(decorationMaxHeight.Count - 1);
		}
		while (decorationCount > decorationMinHeight.Count)
		{
			decorationMinHeight.Add(-1f);
		}
		while (decorationCount < decorationMinHeight.Count)
		{
			decorationMinHeight.RemoveAt(decorationMinHeight.Count - 1);
		}
        for (int i = 0; i < decorationCount;i++)
        {
            if (decorationMaxHeight[i] < decorationMinHeight[i]) {
                decorationMaxHeight[i] = decorationMinHeight[i] = (decorationMinHeight[i] + decorationMaxHeight[i] / 2);
            }
        }
	}
	public void spawnDecos()
	{
        int spawned = 0;
        while (spawned < decoCount)
        {
            Vector2 location = Random.insideUnitCircle * this.decoDistance;
            float height = this.noise.signedRawNoise(location.x, location.y, this.terrainScale);
            int randomOffset = Random.Range(0, this.decorations.Count);

            for (int j = 0; j < this.decorations.Count; j++)
            {
                int offsetJ = (j + randomOffset) % this.decorations.Count;
                if (this.decorationMaxHeight[offsetJ] > height && height > this.decorationMinHeight[offsetJ])
                {
                    spawned++;
                    Debug.Log("Spawned " + this.decorations[offsetJ].name + " at height = " + height + " location " + location);
                    Instantiate(this.decorations[offsetJ], new Vector3(location.x, location.y, -1f), Quaternion.identity, this.transform);
                    break;
                }
            }

        }
	}
}
