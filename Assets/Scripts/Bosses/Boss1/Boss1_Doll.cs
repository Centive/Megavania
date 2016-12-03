using UnityEngine;
using System.Collections;

public class Boss1_Doll : MonoBehaviour
{
    public float nextFire = 0f;
    public float fireRate = 3f;
    public GameObject prefabBullet;
    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        /* If the player is alive
         * fire the bullet AT the player
         */
        if(player != null)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                if (transform.InverseTransformPoint(player.transform.position).x < 0)
                    Instantiate(prefabBullet, transform.position, Quaternion.Euler(0, 0, 180));
                else
                    Instantiate(prefabBullet, transform.position, Quaternion.identity);
            }
        }
    }
}
