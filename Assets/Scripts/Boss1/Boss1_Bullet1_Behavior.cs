using UnityEngine;
using System.Collections;

public class Boss1_Bullet1_Behavior : MonoBehaviour
{
    public float MaxBulletDSpeed = 0.7f;
    public float MaxBulletFSpeed = 2f;
    public float bulletSpeed;
    public float killBulletTimer = 0.2f;
    public float bulletDelay = 1f;

    // Use this for initialization
    void Start()
    {
        bulletSpeed = MaxBulletDSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        bulletDelay -= Time.deltaTime;

        if (bulletDelay <= 0f)
            bulletSpeed = MaxBulletFSpeed;

        //move bullet
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(bulletSpeed * Time.deltaTime, 0.0f);

        pos += transform.rotation * velocity;
        transform.position = pos;


        //Kill bullet
        killBulletTimer -= Time.deltaTime;
        if (killBulletTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
