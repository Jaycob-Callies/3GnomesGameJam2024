Shader "Hidden/NewImageEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelPerTile("Pixels Per Tile", Integer) = 16
        //_Permutation("Simplex Permutation", FloatArray)
        _Scale("Simplex Scale", Float) = 0.01
        _Octaves("Simplex Octaves", Integer) = 1
        _Lacunarity("Simplex Lacunarity", Float) = 2.0
        _Gain("Simplex Gain", Float) = 0.5
        _DitherMain("Min Dither Value (-1,1)", Float) = -1.0
        _DitherMax("Max Dither Value (-1,1)", Float) = 1.0
    }
    SubShader
    {
        // No culling or depth
        Cull Off //ZWrite Off //ZTest Always
        //Tags { "RenderType" = "Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma enable_d3d11_debug_symbols

            #include "UnityCG.cginc"

            int _PixelPerTile;
            sampler2D _MainTex;

            static const int ditherMap[16] = {
                    1,13,4,16,
                    9,5,12,8,
                    3,15,2,14,
                    11,7,10,6
            };

            float _Permutation[256];
            int _Octaves;
            float _Lacunarity;
            float _Gain;
            float _DitherMin;
            float _DitherMax;


            static const float2 gradient2D[9] = {
                float2(1.0, 1.0), float2(1.0, 1.0), float2(1.0, -1.0),
                float2(0.0, -1.0), float2(0.0, 0.0), float2(-1.0, 0.0),
                float2(-1.0, 1.0), float2(-1.0, 0.0), float2(-1.0, -1.0)
            };

            static const float gradientsSize = 9.0;//GRADIENT_2D.Length;

            static float F2 = 0.5 * (sqrt(3.0) - 1.0);//(sqrt(2.0 + 1.0) - 1.0) / 2.0;//F2
            static float G2 = (3.0 - sqrt(3.0)) / 6.0;//(1.0 - (1.0 / sqrt(2.0 + 1.0))) / 2.0;//G2

            int fastFloor(float x) {
                    return x > 0 ? (int)x : (int)x - 1;
            }

            int hash(int i) {
                    return _Permutation[i & 255];
            }

            float posMod(float x, float y) {
                    return x - y * floor(x / y);
            }

            float hashf(float i) {
                    return _Permutation[int(posMod(i,256))];
            }

            float calculateCornerValue(float x, float y, float gradientIndex)
            {
                    float corner = 0.0;
                    float t = 0.5 - (x * x) - (y * y);
                    [flatten] if (t > 0.0)
                    {
                            corner = mul(pow(t, 4), dot(gradient2D[int(gradientIndex)], float2(x, y)));
                    }
                    else {
                            //discard;
                            corner = 0.0;
                    }

                    return corner;
            }

            float signedRawNoise(float xIn, float yIn, float scale = 1.0)
            {
                    xIn *= scale;
                    yIn *= scale;
                    
                    float n0, n1, n2;

                    // Skew the input space to determine which simplex cell we're in
                    float s = (xIn + yIn) * F2;//(-inf,inf)
                    float i = floor(xIn + s);//(-inf,inf) int
                    float j = floor(yIn + s);//(-inf,inf) int

                    // Unskew the cell origin back to (x,y) space
                    float t = (i + j) * G2;//(-inf,inf)
                    float X0 = i - t;//(-inf,inf)
                    float Y0 = j - t;//(-inf,inf)
                    float x0 = xIn - X0; // The x,y distances from the cell origin //(0,1)
                    float y0 = yIn - Y0;//(0,1)

                    // For the 2D case, the simplex shape is an equilateral triangle.
                    // Determine which simplex we are in.
                    // Offsets for second (middle) corner of simplex in (i,j) coords
                    float i1, j1;//(0,1)
                    [flatten] if (x0 > y0)
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
                    float x1 = x0 - i1 + G2;
                    float y1 = y0 - j1 + G2;
                    float x2 = x0 - 1.0 + 2.0 * G2; // Offsets for last corner in (x,y) unskewed coords
                    float y2 = y0 - 1.0 + 2.0 * G2;

                    // Work out the hashed gradient2D indices of the three simplex corners
                    float ii = posMod(i, 256);
                    float jj = posMod(j, 256);

                    float gradientIndex0 = posMod(hashf(ii + hashf(jj)), gradientsSize);
                    float gradientIndex1 = posMod(hashf(ii + i1 + hashf(jj + j1)), gradientsSize);
                    float gradientIndex2 = posMod(hashf(ii + 1.0 + hashf(jj + 1.0)), gradientsSize);

                    // Calculate the contribution from the three corners
                    n0 = calculateCornerValue(x0, y0, gradientIndex0);
                    n1 = calculateCornerValue(x1, y1, gradientIndex1);
                    n2 = calculateCornerValue(x2, y2, gradientIndex2);

                    // Add contributions from each corner to get the final noise value.
                    // The result is scaled to return values in the interval [-1,1].

                    return (35.0 * (n0 + n1 + n2)) + 0.5;

                    return 70.0 * (n0 + n1 + n2);
            }

            float signedFBM(float xPos, float yPos)
            {
                    float sum = 0.0f;
                    float frequency = 1.0f;
                    float amplitude = 1.0f;
                    float maxValue = 0.0f;  // Used for normalizing result between -1.0 and 1.0
                    for (int i = 0; i < _Octaves; ++i)
                    {
                            sum += signedRawNoise(xPos * frequency, yPos * frequency) * amplitude;
                            maxValue += amplitude;
                            amplitude *= _Gain;
                            frequency *= _Lacunarity;
                    }

                    return sum / maxValue;
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : NORMAL;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0)).xyz;
                //o.vertex = mul(unity_ObjectToWorld, o.vertex);
                o.uv = v.uv;
                return o;
            }


            fixed4 frag(v2f i) : SV_Target
            {

                //make sure max > min
                float ditherDirectionCheck = _DitherMax;
                _DitherMax = max(_DitherMin, _DitherMax);
                _DitherMin = min(_DitherMin, ditherDirectionCheck);

                float offset = 0.5f / _PixelPerTile;

                float xPixel = floor(frac(i.worldPos.x) * _PixelPerTile);
                float yPixel = floor(frac(i.worldPos.y) * _PixelPerTile);

                float xTile = floor(i.worldPos.x);
                float yTile = floor(i.worldPos.y);

                //float simplexResult = signedFBM(xTile + (xPixel / (float)_PixelPerTile), yTile + (yPixel / (float)_PixelPerTile));
                //float simplexResult = signedRawNoise(xTile + (xPixel / (float)_PixelPerTile), yTile + (yPixel / (float)_PixelPerTile));
                
                //float simplexResult = signedRawNoise(i.worldPos.x, i.worldPos.y);
                
                float C = signedRawNoise(xTile, yTile, 0.01);

                float2 testF2 = float2(1,0);

                //return fixed4((gradient2D[7].y * 0.5) + 0.5 ,0,0,1);

                return fixed4(C, frac(C + (1.0/3.0)), frac(C + (2.0/3.0)), 1);
                //return fixed4(simplexResult, calculateCornerValue(hashf(xTile), hashf(yTile), 0) / 255, 1, 1);

                //clip(simplexResult < _DitherMin ? -1 : 1);

                int ditherOffset = ditherMap[((uint)xPixel % 4) + (((uint)yPixel % 4) * 4)];

                

                /*
                float xPixel = floor(clamp(mul(frac(i.worldPos.x), (float)_PixelPerTile), 0.1, (float)_PixelPerTile - 0.1));
                float yPixel = floor(clamp(frac(i.worldPos.y) * _PixelPerTile, 0.9, (float)_PixelPerTile - 0.9));
                */

                i.uv.x = offset + (xPixel * rcp(_PixelPerTile));
                i.uv.y = offset + (yPixel * rcp(_PixelPerTile));

                float4 biasCoord = float4(i.uv.x, i.uv.y, 0, 0);

                //float2 tileCoord = float2(
                 //       frac(offset + (round(frac(i.worldPos.x) * _PixelPerTile) / _PixelPerTile)),
                  //      frac(offset + (round(frac(i.worldPos.y) * _PixelPerTile) / _PixelPerTile)));
                //fixed4 col = tex2D(_MainTex, tileCoord);
                fixed4 col = tex2Dlod(_MainTex, biasCoord);
                //fixed4 col = tex2D(_MainTex, i.uv);
                return col;

                // just invert the colors
                //col.rgb = 1 - col.rgb;
                //return fixed4(0, 0, 1, 0);
                return fixed4(frac(i.worldPos.x), frac(i.worldPos.y), frac(i.worldPos.z), 0);
                return fixed4(0, 0, frac(i.vertex.w), 0);
                return col;
            }
            ENDCG
        }
    }
}
