using UnityEngine;
using System.Collections;

public class Boss1_Attack1_Behavior : MonoBehaviour
{
    public float rotationSpeed;
    public float killTimer;
    public float nextFire = 0f;
    public float fireRate = 2f;
    public GameObject prefabBullet;
    private Transform spawnPos;

    void Start()
    {
        spawnPos = transform.GetChild(0);
    }
    
    void Update()
    {
        transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(prefabBullet, new Vector3(spawnPos.position.x, spawnPos.position.y), transform.rotation);
        }

        //kill this
        killTimer -= Time.deltaTime;
        if(killTimer <= 0)
        {
            Destroy(this.gameObject);
        }
        Debug.DrawLine(transform.position, spawnPos.position);
    }
}
