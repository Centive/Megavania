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
    public float maxWalkingDist = 0f;

    public float maxSpeed = 0f;
    public float curSpeed = 0f;
    public WalkDir curDirection = WalkDir.Forth;

    // Use this for initialization
    void Start()
    {
        //init variables
        curVisionRange = maxVisionRange;
        curSpeed = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //update aggressive
        if (IsAggressive())
        {
            Debug.Log("is aggressive");
        }
        else
        {
            Debug.Log("is Passive");
        }
    }

    bool IsAggressive()
    {
        Vector2 visionEnd = new Vector2(transform.position.x + curVisionRange, transform.position.y);

        //Returns if linecast hits something
        RaycastHit2D hit = Physics2D.Linecast(transform.position, visionEnd, LayerMask.GetMask("Enemy_Visions"));

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
