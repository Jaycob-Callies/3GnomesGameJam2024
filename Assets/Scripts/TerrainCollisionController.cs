using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
//using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using static UnityEditor.FilePathAttribute;

public class TerrainCollisionController : MonoBehaviour
{
    private SimplexNoise noise = new SimplexNoise();
    // vector.x is height cutoff, vector.y is speed at cutoff
    private List<Vector2> cutOffList = new List<Vector2>();
    private float terrainScale = 1f;
    //4 is minimum, increase as needed
    private int proximalAccuracy = 15;
    private static bool checkedOnce = false;
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
		if (cutOffList.Count == 0)
		{
			return 1;
		}
        float height = noise.signedRawNoise(location.x, location.y, terrainScale);
        float percent = 0f;

		for (int i = 1; i < this.cutOffList.Count; i++)
        {
            if (cutOffList[i].x > height)
            {
                percent = (height - cutOffList[i - 1].x) / (cutOffList[i].x - cutOffList[i - 1].x);
                float speed = Mathf.Lerp(cutOffList[i - 1].y, cutOffList[i].y, percent);
				if (speed < 0f || height < -0.5)
                {
					//Debug.Log("At (" + location.x + ", " + location.y + ") Height = " + height + ", Speed = " + speed);
				}
				return speed;
            }
        }
        Debug.Log("No cutoFF found in terrainSpeedAtPoint");
		percent = (height - cutOffList[cutOffList.Count - 1].x) / (cutOffList[cutOffList.Count].x - cutOffList[cutOffList.Count - 1].x);
		return Mathf.Lerp(cutOffList[cutOffList.Count - 1].y, cutOffList[cutOffList.Count].y, percent) * Mathf.Sign(-.5f + Mathf.Sign(cutOffList[cutOffList.Count].y) + Mathf.Sign(cutOffList[cutOffList.Count].y));
	}

    public Vector2 getTerrainVelocity(Vector2 xyLocation, Vector2 xyDirection)
    {
        //return Mathf.Max(terrainSpeedAtPoint(xyLocation), 0.1f) * xyDirection.normalized; 
		// too many bugs right now temporarily ignore
		
		xyDirection.Normalize();

		float proximalDistance = 0.65f;

		string log = "Starting Log for getTerrainVelocity\n";

		bool collisionImminent = false;
        List<float> proximalSpeeds = new List<float>();

		float CloseLeft = -90f * (1f / (proximalAccuracy * 2));
		float CloseRight = 90f * (1f / (proximalAccuracy * 2));
		Vector2 proximalLeft = Quaternion.FromToRotation(Vector3.up, Quaternion.Euler(new Vector3(0f, 0f,CloseLeft)) * Vector3.up) * xyDirection;
		Vector2 proximalRight = Quaternion.FromToRotation(Vector3.up, Quaternion.Euler(new Vector3(0f, 0f, CloseRight)) * Vector3.up) * xyDirection;

		proximalLeft = (xyLocation + (proximalLeft.normalized * proximalDistance));
		proximalRight = (xyLocation + (proximalRight.normalized * proximalDistance));


		float localSpeed = terrainSpeedAtPoint(xyLocation);

		if (localSpeed < 0 || (terrainSpeedAtPoint(proximalLeft) < 0 && terrainSpeedAtPoint(proximalRight) < 0))
		{
			collisionImminent = true;
		}

		//for (int i = 0; i < proximalAccuracy; i++)
		//      {
		//          Vector2 directon = Quaternion.FromToRotation(Vector3.up, Quaternion.Euler(new Vector3(0f, 0f, -90f + (180f * ((float)i / (float)(proximalAccuracy - 1)) ))) * Vector3.up) * xyDirection;
		//          Vector2 proximalPoint = (xyLocation + (directon.normalized * 0.25f));
		//          float speedAtPoint = terrainSpeedAtPoint(proximalPoint);

		//	log += "proximal point " + proximalPoint + " speed is " + speedAtPoint + "\n";

		//	proximalSpeeds.Add(speedAtPoint);
		//          if (speedAtPoint < 0f)
		//          {

		//		log += "collisionImminent\n";
		//		collisionImminent = true;
		//          }
		//      }
		if (!collisionImminent)
        {
			if (!checkedOnce)
			{
				Debug.Log(log);
			}
			checkedOnce = true;
			return xyDirection.normalized * Mathf.Max(localSpeed, 0.1f);// cap slow speed for edge cases;
        }
        else
		{
			for (int i = 2; i < Mathf.RoundToInt(180f / CloseRight); i++)
			{
				proximalLeft = Quaternion.FromToRotation(Vector3.up, Quaternion.Euler(new Vector3(0f, 0f, CloseLeft * i)) * Vector3.up) * xyDirection;
				proximalRight = Quaternion.FromToRotation(Vector3.up, Quaternion.Euler(new Vector3(0f, 0f, CloseRight * i)) * Vector3.up) * xyDirection;
				float speedLeft = terrainSpeedAtPoint(xyLocation + (proximalLeft.normalized * proximalDistance));
				float speedRight = terrainSpeedAtPoint(xyLocation + (proximalRight.normalized * proximalDistance));
				if (speedLeft > 0)
				{
					return proximalLeft * Mathf.Max(speedLeft, localSpeed, 0.1f);
				}
				else if (speedRight > 0)
				{
					return proximalRight * Mathf.Max(speedRight, localSpeed, 0.1f);
				}
			}
			Debug.Log("All Negative around Circle");
			return xyDirection.normalized * Mathf.Max(localSpeed, 0.1f);// cap slow speed for edge cases;
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
            float normalIndex = ((float)(maxDirection + minDirection) / 2f);

			Quaternion fromTo = Quaternion.FromToRotation(Vector3.up, Quaternion.Euler(new Vector3(0f, 0f, 360f * (normalIndex / (float)(proximalAccuracy + 1)))) * Vector3.up);
            Vector3 normal = fromTo * xyDirection;
            Vector3 result = new Vector3(xyDirection.x, xyDirection.y, 0f);
            if ((normalIndex < ((float)proximalAccuracy / 2f) && (minDirection < normalIndex))
            || (normalIndex >= ((float)proximalAccuracy / 2f) && (minDirection > normalIndex))){
				result = Vector3.Project(new Vector3(xyDirection.x, xyDirection.y, 0f), normal);
			} //     Vector3.Project(new Vector3(xyDirection.x,xyDirection.y, 0f), normal);
            result = result.normalized * Mathf.Max(terrainSpeedAtPoint(xyLocation), 0.1f);

            Debug.Log("Collision Imminent expecting " + xyDirection + " got " + result);
            if (!checkedOnce)
            {
				Debug.Log(log);
			}
            checkedOnce = true;
			return new Vector2(result.x, result.y);
		}
        
    }
}
