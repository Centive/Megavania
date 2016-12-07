using UnityEngine;
using System.Collections;

public class Enemy_Skeleton : MonoBehaviour
{

    public enum WalkDir
    {
        Back,
        Forth
    }

    public float maxVisionRange = 0f;
    public float curVisionRange = 0f;
    public float curWalkingDist = 0f;
    public float maxWalkingDist = 0f;
    public float maxSpeed = 0f;
    public float curSpeed = 0f;

    public float nextFire = 0f;
    public float fireRate = 3f;
    public WalkDir curDirection = WalkDir.Forth;
    public GameObject prefabBullet;

    private GameObject playerTarget;
    private Rigidbody2D myRigidbody;

    // Use this for initialization
    void Start()
    {
        //init variables
        curVisionRange = maxVisionRange;
        curSpeed = maxSpeed;
        curWalkingDist = maxWalkingDist;

        myRigidbody = GetComponent<Rigidbody2D>();
        playerTarget = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //update Aggressive actions
        if (IsAggressive())
        {
            Debug.Log("AGGRESSIVE");
            if (Time.time > nextFire)
            {
                Debug.Log("SHOOT");
                nextFire = Time.time + fireRate;

                GameObject obj = Instantiate(prefabBullet, transform.position, Quaternion.identity) as GameObject;
                Debug.Log(obj.name + " is created");
            }

        }
        //Update Passive actions
        else
        {
            Vector3 currentScale = transform.localScale;
            myRigidbody.velocity = new Vector2(1 * curSpeed, myRigidbody.velocity.y);
            //Flip the x sprite moving left or right
            if (curDirection == WalkDir.Back)
            {
                currentScale.x = -0.1f;
                transform.localScale = currentScale;
            }

            if (curDirection == WalkDir.Forth)
            {
                currentScale.x = 0.1f;
                transform.localScale = currentScale;
            }

            //Move back and forth
            curWalkingDist -= Time.deltaTime;
            if (curWalkingDist < 0)
            {
                if (curDirection == WalkDir.Forth)
                {
                    curDirection = WalkDir.Back;
                    curSpeed *= -1f;
                }
                else
                {
                    curDirection = WalkDir.Forth;
                    curSpeed *= -1f;
                }
                curWalkingDist = maxWalkingDist;
            }
        }
    }

    bool IsAggressive()
    {

        Vector2 visionEnd = new Vector2(transform.position.x + curVisionRange, transform.position.y);

        if (curSpeed < 0f)
            visionEnd.x = transform.position.x + curVisionRange * -1;

        //Returns if linecast hits something
        RaycastHit2D hit = Physics2D.Linecast(transform.position, visionEnd, LayerMask.GetMask("Player"));

        Debug.DrawLine(transform.position, visionEnd, Color.red);

        //Check if player is within range
        if (hit.collider != null)
        {
            if (hit.transform.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
