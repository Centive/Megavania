using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss1_Attack1_Behavior : MonoBehaviour
{
    public float rotationSpeed;
    public float killTimer;
    public float nextFire = 0f;
    public float fireRate = 2f;
    public GameObject prefabBullet;
    public List<GameObject> spawnedBullets;
    private Transform spawnPos;
    private bool isKill = false;
    

    void Start()
    {
        spawnPos = transform.GetChild(0);
        spawnedBullets = new List<GameObject>();
    }
    
    void Update()
    {
        transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject newBullet = Instantiate(prefabBullet, new Vector3(spawnPos.position.x, spawnPos.position.y), transform.rotation) as GameObject;
            spawnedBullets.Add(newBullet);
        }

        //kill this
        killTimer -= Time.deltaTime;
        if(killTimer <= 0)
        {
            StartCoroutine(SyncBullets());
        }
        Debug.DrawLine(transform.position, spawnPos.position);
    }

    IEnumerator SyncBullets()
    {
        for (int i = 0; i < spawnedBullets.Count; i++)
        {
            float maxDelay = spawnedBullets[i].GetComponent<Boss1_Bullet1_Behavior>().maxDelay;
            float maxKill = spawnedBullets[i].GetComponent<Boss1_Bullet1_Behavior>().maxKillTimer;
            spawnedBullets[i].GetComponent<Boss1_Bullet1_Behavior>().curDelay = maxDelay;
            spawnedBullets[i].GetComponent<Boss1_Bullet1_Behavior>().curKillTimer = maxKill;
        }
        Destroy(this.gameObject);
        yield return 0;
    }
}
