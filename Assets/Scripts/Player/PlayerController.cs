using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //State machine for handling dash
    private enum DashState
    {
        Ready,
        Dashing,
        Cooldown
    }

    //variables
    public float maxMoveSpeed = 5f;
    public float moveSpeed = 5f;
    public float jumpPower = 5f;
    public float dodgeSpeed = 5f;
    private DashState dashState = DashState.Ready;

    public bool isFacingLeft = false;
    public bool isFalling = false;

    //components
    public Animator myAnimator;
    public Rigidbody2D myRigidbody;

    //prefabs
    public GameObject knifePrefab;

    //Player Handler
    private PlayerHandler pHandler;

    void Start()
    {
        //get components
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        pHandler = GetComponent<PlayerHandler>();

        //init vars
        moveSpeed = maxMoveSpeed;
    }

    void Update()
    {
        //Enable UpdateControls
        UpdateControls();

        //Currently checks if the player's y velocity is decreasing. Play falling animation
        myAnimator.SetFloat("VelocityY", myRigidbody.velocity.y);
    }

    void UpdateControls()
    {
        //Enable animations depending on the state
        myAnimator.SetInteger("State", (int)pHandler.curState);

        //Whenever the player is not flying, turn on the gravity
        if (pHandler.curState != PlayerHandler.Estate.OnFly)
        {
            myRigidbody.gravityScale = 1;
        }

        //Input
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        //Update the controls for each state
        switch (pHandler.curState)
        {
            case PlayerHandler.Estate.None:
                {
                    //Nothing
                    break;
                }
            case PlayerHandler.Estate.OnGround:
                {
                    //Move rigidbody on x-axis
                    myRigidbody.velocity = new Vector2(xInput * moveSpeed, myRigidbody.velocity.y);
                    myAnimator.SetFloat("Speed", Mathf.Abs(myRigidbody.velocity.x));

                    //Init Crouch
                    if (yInput < 0)
                    {
                        pHandler.curState = PlayerHandler.Estate.OnCrouch;
                    }
                    else
                    {
                        pHandler.curState = PlayerHandler.Estate.OnGround;
                    }

                    //Init Jump
                    if (Input.GetKeyDown(KeyCode.K))//if (Input.GetButtonDown("XButton"))
                    {
                        myAnimator.SetTrigger("jump!");
                        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpPower);
                    }

                    //

                    //Melee Attack
                    //Attack only when the animation is NOT playing - !isRegAttack()
                    if (Input.GetKeyDown(KeyCode.J) && !isRegAttack())//if (Input.GetButtonDown("SquareButton") && !isRegAttack())
                    {
                        myAnimator.SetTrigger("regAttack!");
                    }

                    //Shoots a knife projectile
                    if (Input.GetKeyDown(KeyCode.L))//if (Input.GetButtonDown("RightBumper"))
                    {
                        Instantiate(knifePrefab, transform.position, (Quaternion.identity));
                    }
                    break;
                }
            case PlayerHandler.Estate.OnCrouch:
                {
                    myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
                    //Init OnGround
                    if (yInput >= 0)
                    {
                        pHandler.curState = PlayerHandler.Estate.OnGround;
                    }

                    //

                    //Melee Attack
                    //Attack only when the animation is NOT playing - !isCrouchRegAttack()
                    if (Input.GetKeyDown(KeyCode.J) && !isCrouchRegAttack())//if (Input.GetButtonDown("SquareButton") && !isCrouchRegAttack())
                    {
                        myAnimator.SetTrigger("crouchAttack!");
                    }
                    break;
                }
            case PlayerHandler.Estate.OnAir:
                {
                    //Init Fly
                    if (Input.GetKeyDown(KeyCode.K))//if (Input.GetButtonDown("XButton"))
                    {
                        pHandler.curState = PlayerHandler.Estate.OnFly;
                    }

                    //Move rigidbody on x-axis
                    myRigidbody.velocity = new Vector2(xInput * moveSpeed, myRigidbody.velocity.y);

                    //

                    //Melee Attack
                    //Attack only when the animation is NOT playing - !isCrouchRegAttack()
                    if (Input.GetKeyDown(KeyCode.J))//if (Input.GetButtonDown("SquareButton"))
                    {
                        myAnimator.SetTrigger("airRegAttack!");
                    }

                    //Shoots a knife projectile
                    if (Input.GetKeyDown(KeyCode.L))//if (Input.GetButtonDown("RightBumper"))
                    {
                        Instantiate(knifePrefab, transform.position, (Quaternion.identity));
                    }
                    break;
                }
            case PlayerHandler.Estate.OnFly:
                {
                    //Always true when flying
                    myRigidbody.gravityScale = 0;

                    //Init OnAir
                    if (Input.GetKeyUp(KeyCode.K))//if (Input.GetButtonUp("XButton"))
                    {
                        pHandler.curState = PlayerHandler.Estate.OnAir;
                    }
                    
                    //Move
                    //Move rigidbody on x/y axis
                    myRigidbody.velocity = new Vector2(xInput * moveSpeed, yInput * moveSpeed);

                    //Shoots a knife projectile
                    if (Input.GetKeyDown(KeyCode.L))//if (Input.GetButtonDown("RightBumper"))
                    {
                        Instantiate(knifePrefab, transform.position, (Quaternion.identity));
                    }

                    //Use the correct fly animation. This is due to freezing where the player faces
                    myAnimator.SetInteger("xInput", (isFacingLeft) ? (int)xInput * -1 : (int)xInput);
                    break;
                }
        }

        /* Depending on the state the character
         * flips the sprite to correctly face the 
         * direction it's moving.
         */
        FlipSprite();
    }

    /* Depending on the state the character
     * flips the sprite to correctly face the 
     * direction it's moving.
     */
    void FlipSprite()
    {
        switch (pHandler.curState)
        {
            case PlayerHandler.Estate.None:
                {
                    //Nothing
                    break;
                }
            case PlayerHandler.Estate.OnGround:
            case PlayerHandler.Estate.OnCrouch:
            case PlayerHandler.Estate.OnAir:
                {
                    //Input
                    float xInput = Input.GetAxisRaw("Horizontal");

                    Vector3 currentScale = transform.localScale;

                    //Flip the x sprite moving left or right
                    if (xInput < 0)
                    {
                        currentScale.x = -1;
                        transform.localScale = currentScale;
                        isFacingLeft = true;
                    }

                    if (xInput > 0)
                    {
                        currentScale.x = 1;
                        transform.localScale = currentScale;
                        isFacingLeft = false;
                    }
                    break;
                }
            case PlayerHandler.Estate.OnFly:
                {
                    //Dont Flip on fly
                    break;
                }
        }
    }

    ////////////////////////////////////////////////////////////////////////
    //Check if attack animations are playing
    bool isRegAttack()
    {
        //if(myAnimator.GetCurrentAnimatorStateInfo(0).IsName("RegAttack"))
        //{
        //    moveSpeed = 0;
        //}
        //else
        //{
        //    moveSpeed = maxMoveSpeed;
        //}

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
    ////////////////////////////////////////////////////////////////////////

    /*
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
    */ //Quarantined CancelMovements()
}