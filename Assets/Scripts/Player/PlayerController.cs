using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //variables
    public float            maxMoveSpeed    = 5f;
    public float            moveSpeed       = 5f;
    public float            jumpPower       = 5f;
    public float            xMovement;         
    public float            yMovement;
    public bool             isGrounded;
    public bool             isCrouching;
    public bool             isFlying;
    public bool             isAttacking;
    public bool             isFacingLeft    = false;

    //components
    public Animator         myAnimator;
    public Rigidbody2D      myRigidbody;
    public SpriteRenderer   myRenderer;

    //prefabs
    public GameObject       knifePrefab;

    void Start()
    {
        //get components
        myAnimator      = GetComponent<Animator>();
        myRigidbody     = GetComponent<Rigidbody2D>();
        myRenderer      = GetComponent<SpriteRenderer>();

        //init vars
        moveSpeed       = maxMoveSpeed;
        isFlying        = false;
    }
    
    void Update()
    {
        CancelMovements();
        FlyIn();
        GroundIn();
        AirIn();
        CrouchingIn();
        if (!isAirRegAttack() && !isCrouchRegAttack() && !isRegAttack())
        {
            FlipSprite();
        }
    }

    void FlipSprite()
    {
        if (xMovement < 0)
        {
            Vector3 currentScale = transform.localScale;
            currentScale.x = -1;
            transform.localScale = currentScale;
            isFacingLeft = true;
        }
        if (xMovement > 0)
        {
            Vector3 currentScale = transform.localScale;
            currentScale.x = 1;
            transform.localScale = currentScale;
            isFacingLeft = false;
        }
    }      // Flips the gameobject
    void GroundIn()
    {
        //get xy input
        xMovement = (Input.GetAxisRaw("Horizontal") * moveSpeed);
        yMovement = (isFlying) ? (Input.GetAxisRaw("Vertical") * moveSpeed) : myRigidbody.velocity.y;
        
        //move left or right
        myRigidbody.velocity = new Vector2(xMovement, yMovement);

        //attack on ground
        if (Input.GetButtonDown("SquareButton") && isGrounded && !isCrouching && !isRegAttack())
        {
            myAnimator.SetTrigger("regAttack!");
        }

        //knife
        if(Input.GetButtonDown("RightBumper"))
        {
            Instantiate(knifePrefab, transform.position + new Vector3(-0.3f, 0.0f, 0.0f), (Quaternion.identity));
        }

        //set animations
        myAnimator.SetFloat("xVelocity", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        myAnimator.SetFloat("yVelocity", myRigidbody.velocity.y);
        myAnimator.SetBool("isGrounded", isGrounded);
    }        // Controls available in the ground
    void AirIn()
    {
        //jump
        if (Input.GetButtonDown("XButton") && !isCrouching && isGrounded)
        {
            myAnimator.SetTrigger("jump!");
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpPower);
        }
        
        if(Input.GetButtonDown("SquareButton") && !isGrounded)
        {
            myAnimator.SetTrigger("airRegAttack!");
        }
    }           // Controls available in the air
    void CrouchingIn()
    {
        //set if player is crouching
        if(!isFlying)
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                isCrouching = true;
            }
            else
            {
                isCrouching = false;
            }
        }

        if (isCrouching)//actions while crouching
        {
            if (Input.GetButtonDown("SquareButton") && !isCrouchRegAttack())//crouching attack
            {
                myAnimator.SetTrigger("crouchAttack!");
            }
        }
        //set animations
        myAnimator.SetBool("isCrouching", isCrouching);
    }     // Controls available while crouching
    void FlyIn()
    {
        if(!isGrounded)
        {
            if(Input.GetButtonDown("XButton"))
            {
                isFlying = true;
            }
        }
        if(isFlying)
        {
            myRigidbody.isKinematic = true;
            if (Input.GetButtonUp("XButton"))
            {
                isFlying = false;
            }
        }
        else
        {
            myRigidbody.isKinematic = false;
        }
    }           // Controls available for flying
    void CancelMovements()
    {
        if (isRegAttack() || isCrouching)
        {
            //myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = maxMoveSpeed;
        }
    }

    //Check if attack animations are playing
    bool isRegAttack()
    {
        return (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("RegAttack"));
    }
    bool isAirRegAttack()
    {
        return (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Air_RegAttack"));
    }
    bool isCrouchRegAttack()
    {
        return (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Crouch_RegAttack"));
    }

    void OnCollisionStay2D(Collision2D col)
    {
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