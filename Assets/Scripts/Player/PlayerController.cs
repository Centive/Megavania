using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    //variables
    public float maxMoveSpeed = 5f;
    public float moveSpeed = 5f;
    public float jumpPower = 5f;
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
        //Enable Controls
        Controls();

        myAnimator.SetFloat("VelocityY", myRigidbody.velocity.y);
    }

    void Controls()
    {
        //Enable animations
        myAnimator.SetInteger("State", (int)pHandler.curState);

        //Turn isKinematic to false whenever the player is not flying
        if (pHandler.curState != PlayerHandler.Estate.OnFly)
        {
            myRigidbody.gravityScale = 1;
        }

        //
        switch (pHandler.curState)
        {
            case PlayerHandler.Estate.None:
                {
                    //Nothing
                    break;
                }
            case PlayerHandler.Estate.OnGround:
                {
                    MovementsOnGround();
                    AttacksOnGround();
                    break;
                }
            case PlayerHandler.Estate.OnCrouch:
                {
                    MovementsOnCrouch();
                    AttacksOnCrouch();
                    break;
                }//movOnGround & atks
            case PlayerHandler.Estate.OnAir:
                {
                    MovementsOnAir();
                    AttacksOnAir();
                    break;
                }//movOnAir & atks
            case PlayerHandler.Estate.OnFly:
                {
                    MovementsOnFly();
                    AttacksOnFly();
                    break;
                }//movOnFly & atks
        }

        //
        FlipSprite();
    }

    ////////////////////////////////////////////////////////////////////////
    void MovementsOnGround()
    {
        //Input
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

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
        if (Input.GetButtonDown("XButton"))
        {
            myAnimator.SetTrigger("jump!");
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpPower);
        }

    }
    void MovementsOnCrouch()
    {
        float yInput = Input.GetAxisRaw("Vertical");

        //Init OnGround
        if (yInput >= 0)
        {
            pHandler.curState = PlayerHandler.Estate.OnGround;
        }
    }
    void MovementsOnAir()
    {
        //Init Fly
        if (Input.GetButtonDown("XButton"))
        {
            pHandler.curState = PlayerHandler.Estate.OnFly;
        }

        //Input
        float xInput = Input.GetAxisRaw("Horizontal");

        //Move rigidbody on x-axis
        myRigidbody.velocity = new Vector2(xInput * moveSpeed, myRigidbody.velocity.y);
    }
    void MovementsOnFly()
    {
        //Always true when flying
        myRigidbody.gravityScale = 0;

        //Init OnAir
        if (Input.GetButtonUp("XButton"))
        {
            pHandler.curState = PlayerHandler.Estate.OnAir;
        }

        //Input
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        //Move
        //Move rigidbody on x/y axis
        myRigidbody.velocity = new Vector2(xInput * moveSpeed, yInput * moveSpeed);

        myAnimator.SetInteger("xInput", (isFacingLeft) ? (int)xInput * -1 : (int)xInput);
    }

    void AttacksOnGround()
    {
        //Melee Attack
        //Attack only when the animation is NOT playing - !isRegAttack()
        if (Input.GetButtonDown("SquareButton") && !isRegAttack())
        {
            myAnimator.SetTrigger("regAttack!");
        }

        //Shoots a knife projectile
        if (Input.GetButtonDown("RightBumper"))
        {
            Instantiate(knifePrefab, transform.position, (Quaternion.identity));
        }
    }
    void AttacksOnCrouch()
    {
        //Melee Attack
        //Attack only when the animation is NOT playing - !isCrouchRegAttack()
        if (Input.GetButtonDown("SquareButton") && !isCrouchRegAttack())
        {
            myAnimator.SetTrigger("crouchAttack!");
        }
    }
    void AttacksOnAir()
    {
        //Melee Attack
        //Attack only when the animation is NOT playing - !isCrouchRegAttack()
        if (Input.GetButtonDown("SquareButton"))
        {
            myAnimator.SetTrigger("airRegAttack!");
        }

        //Shoots a knife projectile
        if (Input.GetButtonDown("RightBumper"))
        {
            Instantiate(knifePrefab, transform.position, (Quaternion.identity));
        }
    }
    void AttacksOnFly()
    {
        //Shoots a knife projectile
        if (Input.GetButtonDown("RightBumper"))
        {
            Instantiate(knifePrefab, transform.position, (Quaternion.identity));
        }
    }
    ////////////////////////////////////////////////////////////////////////
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
    //Check if animations are playing
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