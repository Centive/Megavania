using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour
{
    public enum Estate
    {
        None,
        OnGround,
        OnCrouch,
        OnAir,
        OnFly
    }

    //vars
    public int myHealth = 10;
    public Estate curState = Estate.None;
    protected bool isWall = false;

    //player controller script
    private PlayerController pController;
    
    void Start ()
    {
        pController = GetComponent<PlayerController>();
	}
	
	void Update ()
    {
	    if(myHealth <= 0)
        {
            //Debug.Log(this.gameObject + ": is Dead");
            Destroy(this.gameObject);
        }

        ////////////////////////////////////////////////////////////////////////
        /////Testing raycast :: Does not contribute anything in the code
        Vector2 facingDirection = (pController.isFacingLeft) ? -transform.right : transform.right;
        //Debug.DrawRay(transform.position, facingDirection, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, Mathf.Infinity, 9);
        if(hit.collider != null)
        {
            //Debug.Log("pos: " + hit.transform.position);
            //Debug.DrawLine(transform.position, hit.transform.position);
            //if (hit.transform.tag == "Ground")
            //{
            //    Debug.Log(hit.transform.tag);
            //}
            //else
            //{
            //    //Debug.Log("no hit");
            //}
        }
    }

    // - isGround = true (if) colliding tag "Ground" 
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            Collider2D collider = col.collider;
            if (curState != Estate.OnCrouch)
            {
                Vector3 contactPoint = col.contacts[0].point;
                Vector3 center = collider.bounds.center;

                Debug.DrawLine(contactPoint, new Vector2(center.x, (center.y + (collider.bounds.size.y / 2))));
                if (contactPoint.y > (center.y + (collider.bounds.size.y / 2)))
                {
                    curState = Estate.OnGround;
                }

                //WARNING
                //else
                //{
                //    curState = Estate.OnAir;
                //}
            }
        }
    }

    /*
     * - isGround = false (if) exiting collision of tag "Ground"
     */
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground" && curState == Estate.OnGround)
        {
            curState = Estate.OnAir;
        }
    }

    /*
     * - Enemy hit check
     */
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyHit" && !pController.isDashing)
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
