using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class KillKracker : MonoBehaviour {

    GameManager GM;
    TeleporterController TeleporterController = null;

    public TextMeshProUGUI StatsText;
    public GameObject Image;

	private void FixedUpdate()
	{
		if (TeleporterController == null)
        {
            TeleporterController = FindFirstObjectByType<TeleporterController>(FindObjectsInactive.Include);
            UpdateKill();

		}
	}

	public void UpdateKill()
    {
        if (TeleporterController.killsToNextLevel <= 0)
        {
            Image.transform.localScale = 2.0f * Vector3.one;
            StatsText.text = "";
		}
        else
        {
            Image.transform.localScale = 1.0f * Vector3.one;
			StatsText.text = "" + TeleporterController.killsToNextLevel;
		}
	}
}