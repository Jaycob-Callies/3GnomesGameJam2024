using UnityEngine;
public class DDOL : MonoBehaviour {
    private void Start() {
        //DDOL and making sure that any objects with the same tag get destroyed after the first
        DontDestroyOnLoad(gameObject);
        if (GameObject.FindGameObjectsWithTag("GameManager").Length > 1) {
            Destroy(GameObject.FindGameObjectsWithTag("GameManager")[1]);
        }
    }
}