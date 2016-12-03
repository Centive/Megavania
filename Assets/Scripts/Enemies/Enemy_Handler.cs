using UnityEngine;
using System.Collections;

public class Enemy_Handler : MonoBehaviour
{
    public int myHealth = 5;
    
    void Update()
    {
        if(myHealth <= 0)
        {
            //Debug.Log(this.gameObject.name + ": is Dead");
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(this.gameObject.name + " is hit");
        if (col.gameObject.tag == "PlayerHit")
        {
            myHealth--;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log(this.gameObject.name + " is hit");
        if (col.gameObject.tag == "PlayerHit")
        {
            myHealth--;
        }
    }

}
