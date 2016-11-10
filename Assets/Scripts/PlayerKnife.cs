using UnityEngine;
using System.Collections;

public class PlayerKnife : MonoBehaviour {

    public GameObject player;
    public float bulletSpeed = 12.0f;
    public float killBulletTimer = 0.2f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bulletSpeed = player.GetComponent<PlayerController>().isFacingLeft ? bulletSpeed * -1.0f : bulletSpeed;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(bulletSpeed * Time.deltaTime, 0.0f);

        pos += transform.rotation * velocity;
        transform.position = pos;


        killBulletTimer -= Time.deltaTime;
        if (killBulletTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
