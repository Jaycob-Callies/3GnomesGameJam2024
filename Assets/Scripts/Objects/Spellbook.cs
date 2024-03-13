using UnityEngine;
public class Spellbook : MonoBehaviour {

    GameManager GM;
    PlayerSpells PS;
    //this is attached to the Book object that drops from enemies

    private void Start() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {

            PS = collision.GetComponent<PlayerSpells>();

            //this is where it randomly selects an Spell or at least the id for the spell
            int ran = GM.GrantSpell();
            while (ran == GM.CurrentSpells[0] || ran == GM.CurrentSpells[1] || ran == GM.CurrentSpells[2]) {
                ran = GM.GrantSpell();
            }

            //this checks if the player has empty slots in the correct order and if its empty then it will fill it 
            if (GM.CurrentSpells[0] == 0) {
                GainSpell(0, ran);
            }
            else if(GM.CurrentSpells[1] == 0) {
                GainSpell(1, ran);
            }
            else if(GM.CurrentSpells[2] == 0) {
                GainSpell(2, ran);
            }
            else {
                Debug.Log("Inventory Full");
            }

            GM.ItemsGrabbed += 1;
            Destroy(gameObject);
        }
    } 

    void GainSpell(int Slot, int ran) {
        //this is the line of code that gives you a new spell when you pick up the book
        GM.CurrentSpells[Slot] = ran; //sets the new spell
        PS.UiIcons[Slot].fillAmount = 1; // sets the UI to full
        PS.hasSpell[Slot] = true;//tells the code hey this spot has a spell
        PS.isCooldown[Slot] = true;//starts the cooldown on the UI
        Debug.Log("Obtained New Spell");
    }
}