using UnityEngine;
using System.Collections;

public class TestControlsAndAnimations : MonoBehaviour
{
    //variables
    public bool isRunning = false;
    public bool isJumping = false;

    //components
    private Animator myAnimator;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    
    void Update()
    {
        //run test
        myAnimator.SetBool("isRunning", isRunning);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            isRunning = false;
        }

        //attack test
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            myAnimator.SetTrigger("isAttacking");
        }

        //jump test
        myAnimator.SetBool("isJumping", isJumping);
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isJumping = true;
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            isJumping = false;
        }
    }
}
