using UnityEngine;
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

        //Debug.DrawRay(transform.position, forward, Color.green);
        //
        Vector2 direction = (pController.isFacingLeft) ? Vector2.left : -Vector2.left;
        Debug.DrawRay(transform.position, direction, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
        if (hit.collider.tag == "Ground" && hit.collider != null)
        {
            Debug.Log("hit");
        }
        else
        {
            Debug.Log("no hit");
        }
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
