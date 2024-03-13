using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour {

    float TimeOnPad;

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.tag == "Player") {
            TimeOnPad += Time.deltaTime;

            GetComponent<Rigidbody2D>().rotation+=1f;

            if(TimeOnPad >= 3) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            }
        }
    }
}