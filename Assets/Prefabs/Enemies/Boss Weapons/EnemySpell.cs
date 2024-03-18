using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpells : MonoBehaviour
{

    public GameObject[] spells = null;

    // Start is called before the first frame update
    void Start()
    {
        this.CastSpell(Random.Range(1, 7));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CastSpell(int ID)
    {

        if (ID == 0)
        {
            Debug.Log("This Should Do Nothing, and you shouldn't see it");
        }
        if (ID == 1)
        {
            //Fireball Attack 

                //Should set the icon in the UiIcons to the icon of the spell
                float Velocity = 6f;
                GameObject Fireball = Instantiate(spells[ID], transform.position, transform.rotation);
                //Fireball.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
        }
        if (ID == 2)
        {
            //Icecicle Attack

                float Velocity = 9f;
                GameObject Ice = Instantiate(spells[ID], transform.position, transform.rotation);
                //Ice.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
                StartCoroutine(SecondIcicle(Velocity));

        }
        if (ID == 3)
        {
            //Lightning Attack
                float Velocity = 12f;
                GameObject Lightning = Instantiate(spells[ID], transform.position, transform.rotation);
                //Lightning.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
        }
        if (ID == 4)
        {
            //Goo Attack
                float Velocity = 6.5f;
                GameObject Goo = Instantiate(spells[ID], transform.position, transform.rotation);
                //Goo.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
        }
        if (ID == 5)
        {
            //Star Attack
                float Velocity = 10f;
                GameObject Star = Instantiate(spells[ID], transform.position, transform.rotation);
                //Star.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
        }
        if (ID == 6)
        {
            //Bubble Attack

                float Velocity = 4f;
                
                GameObject Bubble1 = Instantiate(spells[ID], transform.position, transform.rotation * Quaternion.Euler(Vector3.forward * 5f));
                //Bubble1.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
                //Bubble1.GetComponent<Rigidbody2D>().AddForce(transform.right, ForceMode2D.Impulse);
                GameObject Bubble2 = Instantiate(spells[ID], transform.position, transform.rotation);
                //Bubble2.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);
                //Bubble2.GetComponent<Rigidbody2D>().AddForce(transform.right * -1, ForceMode2D.Impulse);
                GameObject Bubble3 = Instantiate(spells[ID], transform.position, transform.rotation * Quaternion.Euler(Vector3.forward * -5f));
                //Bubble3.GetComponent<Rigidbody2D>().AddForce(transform.up * Velocity, ForceMode2D.Impulse);

            
        }
    }

    IEnumerator SecondIcicle(float Vel)
    {
        yield return new WaitForSeconds(0.1f);
        GameObject Ice2 = Instantiate(spells[2], transform.position, transform.rotation);
    }

}
