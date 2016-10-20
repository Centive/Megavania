using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //variables
    public float            moveSpeed = 0;
    public float            xMovement;//test input
    public float            yMovement;//test input
    //components
    private Animator        myAnimator;
    private Rigidbody2D     myRigidbody;

    //prefabs

    void Start()
    {
        myAnimator      = GetComponent<Animator>();
        myRigidbody     = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");

        //myRigidbody.velocity = new Vector2(xMovement * moveSpeed, myRigidbody.velocity.y);
    }
}
