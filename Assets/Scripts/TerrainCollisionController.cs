using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TerrainCollisionController : MonoBehaviour
{
    private SimplexNoise noise = new SimplexNoise();
    // vector.x is height cutoff, vector.y is speed at cutoff
    private List<Vector2> cutOffList = new List<Vector2>();
    private float terrainScale = 1f;
    //8 is minimum, increase as needed
    private int proximalAccuracy = 8;
	public void Initialize(int seed, List<float> cutOff, List<float> movementMultiplier, float ditherPercent, float terrainScale)
	{
        noise.setSeed(seed);

		cutOffList.Clear();
		cutOffList.Add(new Vector2(-1f, movementMultiplier[0]));
        for (int cutOffIndex = 1; cutOffIndex < cutOff.Count; cutOffIndex++) 
        {
			float ditherPrev = cutOffIndex <= 1 ? -1f : cutOff[cutOffIndex - 2];
			float ditherCur = cutOff[cutOffIndex - 1];
			float ditherNext = cutOffIndex == (cutOff.Count - 1) ? 1f : cutOff[cutOffIndex];

			float ditherMin = Mathf.Lerp(ditherCur, ditherPrev, ditherPercent * 0.5f);
			float ditherMax = Mathf.Lerp(ditherCur, ditherNext, ditherPercent * 0.5f);

			cutOffList.Add(new Vector2(ditherMin, movementMultiplier[cutOffIndex-1]));
			cutOffList.Add(new Vector2(ditherMax, movementMultiplier[cutOffIndex]));
		}
		cutOffList.Add(new Vector2(1f, movementMultiplier[movementMultiplier.Count - 1]));
        for(int i = 0;i < cutOffList.Count; i++)
        {
            Debug.Log("Cutoff at " + cutOffList[i].x + " is " + cutOffList[i].y);
        }
		this.terrainScale = terrainScale;
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    
    }
    public float terrainSpeedAtPoint(Vector2 location)
    {
        float height = noise.signedRawNoise(location.x * terrainScale, location.y * terrainScale);
        float percent = 0f;

		for (int i = 1; i < this.cutOffList.Count; i++)
        {
            if (cutOffList[i].x > height)
            {
                percent = (height - cutOffList[i - 1].x) / (cutOffList[i].x - cutOffList[i - 1].x);
                float speed = Mathf.Lerp(cutOffList[i - 1].y, cutOffList[i].y, percent);
				//Debug.Log("At (" + location.x + ", " + location.y + ") Height = " + height + ", Speed = " + Mathf.Lerp(cutOffList[i - 1].y, cutOffList[i].y, percent));
				return speed;
            }
        }
        Debug.Log("No cutoFF found in terrainSpeedAtPoint");
		percent = (height - cutOffList[cutOffList.Count - 1].x) / (cutOffList[cutOffList.Count].x - cutOffList[cutOffList.Count - 1].x);
		return Mathf.Lerp(cutOffList[cutOffList.Count - 1].y, cutOffList[cutOffList.Count].y, percent);
	}

    public Vector2 getTerrainVelocity(Vector2 xyLocation, Vector2 xyDirection)
    {
        bool collisionImminent = false;
        List<float> proximalSpeeds = new List<float>();
        for (int i = 0; i < proximalAccuracy; i++)
        {
            Vector2 directon = Quaternion.FromToRotation(Vector3.up, Quaternion.Euler(new Vector3(0f, 0f, 360f * ((float)i / (float)(proximalAccuracy + 1))   )) * Vector3.up) * xyDirection;
            Vector2 proximalPoint = (xyLocation + (directon.normalized * 0.1f)) * this.terrainScale;
            float speedAtPoint = terrainSpeedAtPoint(proximalPoint);

			proximalSpeeds.Add(speedAtPoint);
            if (speedAtPoint < 0)
            {
                collisionImminent = true;
            }
        }
        if (!collisionImminent)
        {
            float h = terrainSpeedAtPoint(xyLocation * this.terrainScale);
			return xyDirection.normalized * Mathf.Max(h, 0.1f);// cap slow speed for edge cases;
        }
        else
		{
			int minDirection = -1;
			int maxDirection = -1;
            for (int i = 0; i < proximalSpeeds.Count; i++)
			{
				if (minDirection == -1 || proximalSpeeds[i] < proximalSpeeds[minDirection])
				{
					minDirection = i;
				}
				if (maxDirection == -1 || proximalSpeeds[i] > proximalSpeeds[maxDirection])
				{
					maxDirection = i;
				}
			}
            Quaternion fromTo = Quaternion.FromToRotation(Vector3.up, Quaternion.Euler(new Vector3(0f, 0f, 360f * (((float)(maxDirection + minDirection) / 2f) / (float)(proximalAccuracy + 1)))) * Vector3.up);
            Vector3 normal = fromTo * xyDirection;
			Vector3 result = Vector3.Project(new Vector3(xyDirection.x,xyDirection.y, 0f), normal) * Mathf.Max(terrainSpeedAtPoint(xyLocation), 0.1f);
            return new Vector2(result.x, result.y);
		}

    }
}
