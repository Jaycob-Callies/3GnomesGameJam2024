using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformSpell : MonoBehaviour {

    #region variables
    GameManager GM;

    float[] CastSpeed = new float[6];
    [HideInInspector]
    public float[] CastDelay = new float[6]; //the ammount in the array needs to be 1 Higher than the amount of total spells

    #endregion

    private void Start() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        CastSpeed[1] = 0.75f; //1 Fireball
        CastSpeed[2] = 1.00f; //2 Ice
        CastSpeed[3] = 1.50f; //3 Lightning
        CastSpeed[4] = 1.20f; //4 Goo
        CastSpeed[5] = 0.20f; //5 Star

    }

    private void Update() {
        //This checks the cast time for whatever Spell ID is currently being used in a Slot
        if (CastDelay[GM.CurrentSpells[0]] > -1) {
            CastDelay[GM.CurrentSpells[0]] -= Time.deltaTime;
        }
        if (CastDelay[GM.CurrentSpells[1]] > -1) {
            CastDelay[GM.CurrentSpells[1]] -= Time.deltaTime;
        }
        if (CastDelay[GM.CurrentSpells[2]] > -1) {
            CastDelay[GM.CurrentSpells[2]] -= Time.deltaTime;
        }

        //Updates Stats
        GM.TimePlayed += Time.deltaTime;
    }

    public void CastSpell(int ID) {

        if(ID == 0) {
            Debug.Log("This Should Do Nothing, and you shouldn't see it"); 
        }
        if (ID == 1) {
            //Fireball Attack 
            if (CastDelay[ID] < 0) {


                //Should set the icon in the UiIcons to the icon of the spell

                GM.SpellsCast += 1;
                float Velocity = 6f;
                CastDelay[ID] = CastSpeed[ID];
                GameObject Fireball = Instantiate(GM.SpellObjects[ID], transform.position, transform.rotation);
                Fireball.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
            }
        }
        if (ID == 2)
        {
            //Icecicle Attack


            if (CastDelay[ID] < 0)
            {
                GM.SpellsCast += 1;
                float Velocity = 9f;
                CastDelay[ID] = CastSpeed[ID];
                GameObject Ice = Instantiate(GM.SpellObjects[ID], transform.position, transform.rotation);
                Ice.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
                StartCoroutine(SecondIcicle(Velocity));
            }

        }
        if (ID == 3)
        {
            //Lightning Attack
            if (CastDelay[ID] < 0)
            {
                GM.SpellsCast += 1;
                float Velocity = 12f;
                CastDelay[ID] = CastSpeed[ID];
                GameObject Lightning = Instantiate(GM.SpellObjects[ID], transform.position, transform.rotation);
                Lightning.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
            }
        }
        if (ID == 4)
        {
            //Goo Attack
            if (CastDelay[ID] < 0)
            {
                GM.SpellsCast += 1;
                float Velocity = 6.5f;
                CastDelay[ID] = CastSpeed[ID];
                GameObject Goo = Instantiate(GM.SpellObjects[ID], transform.position, transform.rotation);
                Goo.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
            }
        }
        if (ID == 5) {
            //Star Attack
            if (CastDelay[ID] < 0)
            {
                GM.SpellsCast += 1;
                float Velocity = 10f;
                CastDelay[ID] = CastSpeed[ID];
                GameObject Star = Instantiate(GM.SpellObjects[ID], transform.position, transform.rotation);
                Star.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator SecondIcicle(float Vel) {
        yield return new WaitForSeconds(0.1f);
        GameObject Ice2 = Instantiate(GM.SpellObjects[2], transform.position, transform.rotation);
        Ice2.GetComponent<Rigidbody2D>().AddForce(transform.up * Vel, ForceMode2D.Impulse);
    }
}