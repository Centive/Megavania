using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //variables
    public float            maxMoveSpeed    = 5;
    public float            moveSpeed       = 5;
    public float            jumpPower       = 5;
    public float            xMovement;         
    public float            yMovement;
    private bool             isGrounded;
    private bool            isCrouching;

    //components
    private Animator        myAnimator;
    private Rigidbody2D     myRigidbody;
    private SpriteRenderer  myRenderer;

    //prefabs

    void Start()
    {
        //get components
        myAnimator      = GetComponent<Animator>();
        myRigidbody     = GetComponent<Rigidbody2D>();
        myRenderer      = GetComponent<SpriteRenderer>();

        //init vars
        moveSpeed = maxMoveSpeed;
    }
    
    void Update()
    {
        ControlIn();
        FlipSprite();


    }
    void FlipSprite()
    {
        if (xMovement < 0)
        {
            myRenderer.flipX = true;
        }
        if (xMovement > 0)
        {
            myRenderer.flipX = false;
        }
    }
    void ControlIn()
    {
        //get xy input
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");
        
        //move left or right
        myRigidbody.velocity = new Vector2(xMovement * moveSpeed, myRigidbody.velocity.y);

        //attack on ground
        if (Input.GetKeyDown(KeyCode.J) && isGrounded)
        {
            myAnimator.SetTrigger("regAttack!");
            
        }
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("RegAttack"))
        {
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = maxMoveSpeed;
        }
        //Air controls
        AirIn();

        //Crounching
        CrouchingIn();

        //set animations
        myAnimator.SetFloat("xVelocity", Mathf.Abs(xMovement));
        myAnimator.SetFloat("yVelocity", myRigidbody.velocity.y);
        myAnimator.SetBool("isGrounded", isGrounded);
        myAnimator.SetBool("isCrouching", isCrouching);
    }

    void AirIn()
    {
        //jump
        if (Input.GetKeyDown(KeyCode.K) && !isCrouching)
        {
            myAnimator.SetTrigger("jump!");
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpPower);
        }
    }
    void CrouchingIn()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            isCrouching = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            isCrouching = false;
        }

        if(isCrouching)
        {
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = maxMoveSpeed;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log("eyy");
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}