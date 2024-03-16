using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterController : MonoBehaviour
{

	private float TimeOnPad = 0f;
	public float teleporterDelay = 3f;
	GameManager GM;
	public GameObject teleporter = null;
	public Sprite arrowSprite = null;
	public int killsToNextLevel = 10;
	[Tooltip("Leave null to spawn teleporter on last kill")]
	public GameObject Boss = null;
	public int teleportTo = -1;
	private GameObject Arrow = null;
	private KillKracker KT = null;
	public void Start()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		teleporter.GetComponent<Teleporter>().Initialize(this);
		teleporter.SetActive(false);
		//teleporter.getComponent<Teleporter>().Initialize(this);
		//default teleport to next scene in build order
		if (teleportTo == -1)
		{
			teleportTo = SceneManager.GetActiveScene().buildIndex + 1;
		}
		Arrow = new GameObject();
		SpriteRenderer ArRenderer = Arrow.AddComponent<SpriteRenderer>();
		ArRenderer.sprite = arrowSprite;
		Arrow.name = "TeleporterArrow";
		Arrow.transform.parent = this.gameObject.transform;
		Arrow.transform.localScale = Vector3.one * 2;
		Arrow.SetActive(false);
		if (Boss != null)
		{
			Boss.transform.parent = this.gameObject.transform;
			Boss.SetActive(false);
		}
	}
	public void Update()
	{
		if (Arrow.activeSelf)
		{
			Vector3 ArrowPos = new Vector3(this.teleporter.transform.position.x - Camera.main.transform.position.x, 
				this.teleporter.transform.position.y - Camera.main.transform.position.y, 0f);
			if ((ArrowPos.magnitude) > (Camera.main.orthographicSize / 2f))
			{
				ArrowPos = ArrowPos.normalized * (Camera.main.orthographicSize / 2f);
			}
			Arrow.transform.position = Camera.main.transform.position + ArrowPos + (Vector3.forward * 10f);
			Arrow.transform.rotation = Quaternion.FromToRotation(Vector3.left, ArrowPos);
		}
	}
	public void killedEnemy(Vector3 killLocation)
	{
		killsToNextLevel--;
		if (Boss == null && (killsToNextLevel == 0 || (killsToNextLevel < 0 && !teleporter.activeSelf)))
		{
			//activate teleporter 15 units from last kill
			teleporter.SetActive(true);
			Arrow.SetActive(true);
			teleporter.transform.position = killLocation;// + 3f * Camera.main.orthographicSize * (Quaternion.Euler(0f,0f,Random.RandomRange(0f,360f)) * Vector3.right) ;
			//activeTeleporter = GameObject.;
		}
		else if (killsToNextLevel == 0)
		{
			this.Boss.SetActive(true);
			//spawn off screen
			this.Boss.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f) + 3f 
				* Camera.main.orthographicSize * (Quaternion.Euler(0f, 0f, Random.RandomRange(0f, 360f)) * Vector3.right);
		}
		if (KT == null) {
			KT = GameObject.FindAnyObjectByType<KillKracker>();
		}
		KT.UpdateKill();
	}
	public void killedBoss()
	{
		teleporter.SetActive(true);
		Arrow.SetActive(true);
		teleporter.transform.position = Boss.transform.position;
	}
	public void Teleport()
	{
		GM.LevelReached += 1;
		SceneManager.LoadScene(teleportTo);
	}
}