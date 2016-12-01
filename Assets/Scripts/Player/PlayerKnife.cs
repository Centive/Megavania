using UnityEngine;
using System.Collections;

public class PlayerKnife : MonoBehaviour
{
    public GameObject player;
    public float bulletSpeed = 12.0f;
    public float killBulletTimer = 0.2f;

    void Start()
    {
        player = GameObject.Find("Player");

        //Reverse bullet and sprite depending where the player is facing
        if(player.GetComponent<PlayerController>().isFacingLeft)
        {
            bulletSpeed *= -1.0f;
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void Update()
    {
        //Move knife
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(bulletSpeed * Time.deltaTime, 0.0f);

        pos += transform.rotation * velocity;
        transform.position = pos;

        //Destroy the object if it exists in the game for too long
        killBulletTimer -= Time.deltaTime;
        if (killBulletTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
