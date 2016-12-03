using UnityEngine;
using System.Collections;

public class EnemyBehavior_FollowTarget : MonoBehaviour
{
    public Transform target;
    public float maxSpeed = 0.1f;
    public float mySpeed;
    public float yOffset = 0;
    public bool isFollowing = false;

    void Start()
    {
        mySpeed = maxSpeed;
    }

    void Update()
    {
        if(target)
        {
            if (isFollowing)
            {
                Vector2 test = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),
                                                            new Vector2(target.position.x, target.position.y + yOffset),
                                                            mySpeed * Time.deltaTime);
                //Debug.Log(test);
                transform.position = test;
            }
        }
    }
}
