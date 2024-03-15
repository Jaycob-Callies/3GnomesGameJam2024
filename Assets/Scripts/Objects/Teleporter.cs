using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Teleporter : MonoBehaviour {
	public enum TeleporterAnimationType { Rotate = 0, Shake = 1 };

	private float TimeOnPad = 0f;
    public float teleporterDelay = 3f;
    private TeleporterController tController = null;
    public TeleporterAnimationType animationType = 0;

	public void Initialize(TeleporterController tc)
	{
        this.tController = tc;
	}
	public void Start() {

	}

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.tag == "Player") {
            TimeOnPad += Time.deltaTime;

            switch (animationType)
            {
                case TeleporterAnimationType.Rotate:
                    Quaternion rotate = new Quaternion();
                    rotate = this.gameObject.transform.rotation;
                    rotate.eulerAngles = rotate.eulerAngles + new Vector3(0f, 0f, Time.fixedDeltaTime * 25f);
					this.gameObject.transform.rotation = rotate;
                    break;
                default:
                    this.gameObject.transform.position = this.gameObject.transform.position + new Vector3(Mathf.Sin(UnityEngine.Time.time * 5f) * 0.005f, 0f, 0f);
                    break;
			}

            if(TimeOnPad >= teleporterDelay) {
                tController.Teleport();
            }
        }
    }
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
            TimeOnPad = 0f;
		}
	}
}