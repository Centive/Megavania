﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //variables
    public float maxMoveSpeed = 5f;
    public float moveSpeed = 5f;
    public float jumpPower = 5f;
    public float maxDashSpeed = 0f;
    public float dashTimer = 0f;
    public bool enableControls = true;
    public bool isDashing = false;
    public float dashSpeed = 0f;
    //private DashState dashState = DashState.Ready;

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
        dashSpeed = maxDashSpeed;
    }

    void Update()
    {
        if(enableControls)
        {
            //Enable UpdateControls
            UpdateControls();
        }

        //Currently checks if the player's y velocity is decreasing. Play falling animation
        myAnimator.SetFloat("VelocityY", myRigidbody.velocity.y);
    }

    void UpdateControls()
    {
        //Input
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        //Enable animations
        myAnimator.SetInteger("State", (int)pHandler.curState);
        myAnimator.SetBool("isDashing", isDashing);

        //Whenever the player is not flying, turn on the gravity
        if (pHandler.curState != PlayerHandler.Estate.OnFly)
        {
            myRigidbody.gravityScale = 1;
        }


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

                    //init dash
                    if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
                    {
                        StopCoroutine(DashCoroutine(xInput));
                        StartCoroutine(DashCoroutine(xInput));
                        myAnimator.SetTrigger("gDash!");
                    }

                    //

                    //Melee Attack
                    //Attack only when the animation is NOT playing - !isGroundRegAttack()
                    if (Input.GetKeyDown(KeyCode.J) && !isGroundRegAttack())//if (Input.GetButtonDown("SquareButton") && !isGroundRegAttack())
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

                    //init dash
                    if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
                    {
                        StopCoroutine(DashCoroutine(xInput));
                        StartCoroutine(DashCoroutine(xInput));
                        myAnimator.SetTrigger("aDash!");
                    }

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

                    //init dash
                    if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
                    {
                        StopCoroutine(DashCoroutine(xInput));
                        StartCoroutine(DashCoroutine(xInput));
                        myAnimator.SetTrigger("fDash!");
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
    bool isGroundRegAttack()
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
    IEnumerator DashCoroutine(float xInput)
    {
        float time = 0.0f;
        isDashing = true;

        if (xInput != 0f)
        {
            dashSpeed += 1.5f;
        }
        Vector2 dashDirection;
        if (pHandler.curState == PlayerHandler.Estate.OnFly)
        {
            //Dash at the direction the player is facing
            dashDirection = (xInput < 0) ? new Vector2(-dashSpeed, myRigidbody.velocity.y)
                : new Vector2(dashSpeed, myRigidbody.velocity.y);
        }
        else
        {
            //Dash at the direction the player is facing
            dashDirection = (isFacingLeft) ? new Vector2(-dashSpeed, myRigidbody.velocity.y)
                : new Vector2(dashSpeed, myRigidbody.velocity.y);
        }

        //Handles animation transition after dashing
        myAnimator.SetInteger("xInput", Mathf.Abs((int)xInput));

        while (dashTimer > time)
        {
            time += Time.deltaTime;
            myRigidbody.velocity = dashDirection;
            yield return 0;
        }
        
        //Wait till timer is finished so that the player can dash again
        yield return new WaitForSeconds(dashTimer);
        dashSpeed = maxDashSpeed;
        myAnimator.SetInteger("xInput", 0);
        isDashing = false; 
    }
    /*
    void CancelMovements()
    {
        if (isGroundRegAttack() || isCrouching)
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