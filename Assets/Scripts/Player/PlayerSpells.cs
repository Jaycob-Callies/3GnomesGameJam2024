using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSpells : MonoBehaviour {

    #region Variables
    GameManager GM;
    public LoseScreen LS;

    public Image[] UiIcons = new Image[3];
    float cooldown = 10f; //maybe if we have extra time in the end we can have difficulties that raise and lower this number
    [HideInInspector]
    public bool[] isCooldown = new bool[3];
    [HideInInspector]
    public bool[] hasSpell = new bool[3];

    PerformSpell Perform;
    #endregion

    void Start() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Perform = GetComponent<PerformSpell>();

        if (GM.CurrentSpells[0] == 0 && GM.CurrentSpells[1] == 0 && GM.CurrentSpells[2] == 0) {
            //this happens when you first hit play so it gives you a spell to start with
            GM.CurrentSpells[0] = GM.GrantSpell();
        }

        #region Setting the UI when you go to the next floor
        if (GM.CurrentSpells[0] != 0) {
            hasSpell[0] = true;
            UiIcons[0].fillAmount = 1; 
        }
        if (GM.CurrentSpells[1] != 0) {
            hasSpell[1] = true;
            UiIcons[1].fillAmount = 1;
        }
        if (GM.CurrentSpells[2] != 0) {
            hasSpell[2] = true;
            UiIcons[2].fillAmount = 1;
        }
        #endregion

    }

    void Update() {

        if(GM.CurrentSpells[0] == 0 && GM.CurrentSpells[1] == 0 && GM.CurrentSpells[2] == 0) { 
            //when u run out of spells this happens
            LS.GameOver(); 
        }

        if(PauseMenu.GameIsPaused == false) {

            #region PressingButtons
            if (Input.GetButton("Fire1") && hasSpell[0] == true) {
                //Mouse1
                int AbilityID = 0;
                UiIcons[AbilityID].fillAmount = 1;
                isCooldown[AbilityID] = true;
                hasSpell[AbilityID] = true;
                Perform.CastSpell(GM.CurrentSpells[AbilityID]); 
            }
            if (Input.GetButton("Fire2") && hasSpell[1] == true && !Input.GetButton("Fire1")) {
                //Mouse2
                int AbilityID = 1;
                UiIcons[AbilityID].fillAmount = 1;
                isCooldown[AbilityID] = true;
                hasSpell[AbilityID] = true;
                Perform.CastSpell(GM.CurrentSpells[AbilityID]); 
            }
            if (Input.GetKey(KeyCode.E) && hasSpell[2] == true && !Input.GetButton("Fire1") && !Input.GetButton("Fire2")) {
                //Keyboard E
                int AbilityID = 2;
                UiIcons[AbilityID].fillAmount = 1;
                isCooldown[AbilityID] = true;
                hasSpell[AbilityID] = true;
                Perform.CastSpell(GM.CurrentSpells[AbilityID]); 
            }
            #endregion 

            #region SpellCoolDownsUI
            if (isCooldown[0])
            {
                UiIcons[0].fillAmount -= 1 / cooldown * Time.deltaTime;
                if (UiIcons[0].fillAmount <= 0)
                {
                    //this is when the Cooldown Runs Out 
                    UiIcons[0].fillAmount = 0;
                    isCooldown[0] = false;
                    hasSpell[0] = false;
                    GM.CurrentSpells[0] = 0;
                }
            }
            if (isCooldown[1]) {
                UiIcons[1].fillAmount -= 1 / cooldown * Time.deltaTime;
                if (UiIcons[1].fillAmount <= 0) {
                    //this is when the Cooldown Runs Out 
                    UiIcons[1].fillAmount = 0;
                    isCooldown[1] = false;
                    hasSpell[1] = false;
                    GM.CurrentSpells[1] = 0;
                }
            }
            if (isCooldown[2]) {
                UiIcons[2].fillAmount -= 1 / cooldown * Time.deltaTime;
                if (UiIcons[2].fillAmount <= 0) {
                    //this is when the Cooldown Runs Out 
                    UiIcons[2].fillAmount = 0;
                    isCooldown[2] = false;
                    hasSpell[2] = false;
                    GM.CurrentSpells[2] = 0;
                }
            }
            #endregion

        }
    }
}