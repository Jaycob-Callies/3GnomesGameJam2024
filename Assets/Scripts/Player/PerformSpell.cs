using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformSpell : MonoBehaviour {

    #region variables
    GameManager GM;

    float[] CastSpeed = new float[8];
    
    float[] CastDelay = new float[8];

    #endregion

    private void Start() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        CastSpeed[1] = 0.75f; //1 Fireball
        CastSpeed[2] = 1.00f; //2 Ice
        CastSpeed[3] = 1.50f; //3 Lightning
        CastSpeed[4] = 1.20f; //4 Goo
        CastSpeed[5] = 0.30f; //5 Star
        CastSpeed[6] = 1.20f; //6 Bubble
        CastSpeed[7] = 1.20f; //7 Skull

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
        if (ID == 6) {
            //Bubble Attack

            if (CastDelay[ID] < 0) {
                GM.SpellsCast += 1;
                float Velocity = 4f;
                CastDelay[ID] = CastSpeed[ID];

                GameObject Bubble1 = Instantiate(GM.SpellObjects[ID], transform.position, transform.rotation);
                Bubble1.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
                Bubble1.GetComponent<Rigidbody2D>().AddForce(transform.right, ForceMode2D.Impulse);
                GameObject Bubble2 = Instantiate(GM.SpellObjects[ID], transform.position, transform.rotation);
                Bubble2.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
                Bubble2.GetComponent<Rigidbody2D>().AddForce(transform.right * -1, ForceMode2D.Impulse);
                GameObject Bubble3 = Instantiate(GM.SpellObjects[ID], transform.position, transform.rotation);
                Bubble3.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);

            }
        }
        if (ID == 7) {
            //Skull Attack 
            if (CastDelay[ID] < 0) {

                GM.SpellsCast += 1;
                CastDelay[ID] = CastSpeed[ID];

                GameObject Skull = Instantiate(GM.SpellObjects[ID], transform.position, transform.rotation);
                Skull.GetComponent<Rigidbody2D>().AddForce(transform.up * 3, ForceMode2D.Impulse);
                StartCoroutine(SpeedUp(Skull, 6));
            }
        }
    }

    IEnumerator SecondIcicle(float Vel) {
        yield return new WaitForSeconds(0.1f);
        GameObject Ice2 = Instantiate(GM.SpellObjects[2], transform.position, transform.rotation);
        Ice2.GetComponent<Rigidbody2D>().AddForce(transform.up * Vel, ForceMode2D.Impulse);
    }

    IEnumerator SpeedUp(GameObject Skull, float Vel) {
        yield return new WaitForSeconds(0.5f);
        if(Skull != null) {
            Skull.GetComponent<Rigidbody2D>().AddForce(transform.up * Vel, ForceMode2D.Impulse);
        }
    }
}