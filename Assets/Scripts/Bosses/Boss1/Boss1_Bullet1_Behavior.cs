using UnityEngine;
using System.Collections;

public class Boss1_Bullet1_Behavior : MonoBehaviour
{
    public float MaxBulletDSpeed;
    public float MaxBulletFSpeed;
    public float bulletSpeed;
    public float curKillTimer;
    public float curDelay;
    public float maxKillTimer;
    public float maxDelay;

    // Use this for initialization
    void Start()
    {
        bulletSpeed = MaxBulletDSpeed;
        curKillTimer = maxKillTimer;
        curDelay = maxDelay;
    }

    // Update is called once per frame
    void Update()
    {
        curDelay -= Time.deltaTime;

        if (curDelay <= 0f)
            bulletSpeed = MaxBulletFSpeed;

        //move bullet
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(bulletSpeed * Time.deltaTime, 0.0f);

        pos += transform.rotation * velocity;
        transform.position = pos;


        //Kill bullet
        curKillTimer -= Time.deltaTime;
        if (curKillTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
