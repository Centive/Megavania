﻿using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour
{
    public enum Estate
    {
        None,//if the player is not grounded
        OnGround,
        OnCrouch,
        OnAir,
        OnFly
    }

    //vars
    public int myHealth = 10;
    public Estate curState = Estate.None;

    //pcontroler
    private PlayerController pController;
    
    void Start ()
    {
        pController = GetComponent<PlayerController>();
	}
	
	void Update ()
    {
	    if(myHealth <= 0)
        {
            Debug.Log(this.gameObject + ": is Dead");
            Destroy(this.gameObject);
        }
	}

    void FixedUpdate()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
        //if (hit.collider != null)
        //{
        //    //float distance = Mathf.Abs(hit.point.y - transform.position.y);
        //    //float heightError = floatHeight - distance;
        //    //float force = liftForce * heightError - rb2D.velocity.y * damping;
        //    //rb2D.AddForce(Vector3.up * force);
        //}
    }

    /*
     * - isGround = true (if) colliding tag "Ground"
     */
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            if (curState != Estate.OnCrouch)
            {
                curState = Estate.OnGround;
            }
        }
    }

    /*
     * - isGround = false (if) exiting collision of tag "Ground"
     */
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            curState = Estate.OnAir;
        }
    }

    /*
     * - Enemy hit check
     */
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyHit")
        {
            myHealth--;
        }

        if (col.gameObject.tag == "Ground")
        {
            //Cancel fly when hits ground
            if (curState == Estate.OnFly)
            {
                curState = Estate.OnGround;
            }
        }
    }
}
