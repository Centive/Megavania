using UnityEngine;
using System.Collections;

public class Skeleton_Bullet : MonoBehaviour
{
    public float throwPowerY = 0f;
    public float throwPowerX = 0f;
    public float curKillTimer = 0f;
    public float maxKillTimer = 0f;

    public Rigidbody2D myRigidbody;

    // Use this for initialization
    void Start()
    {
        curKillTimer = maxKillTimer;
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.AddForce(new Vector2(throwPowerX, throwPowerY), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //Kill bullet
        curKillTimer -= Time.deltaTime;
        if (curKillTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
