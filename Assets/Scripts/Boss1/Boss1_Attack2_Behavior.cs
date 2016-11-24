using UnityEngine;
using System.Collections;

public class Boss1_Attack2_Behavior : MonoBehaviour
{
    public GameObject prefabBullet;

    void Start()
    {
        for(int i = 0; i != 360; i+= 30)
        {
            Instantiate(prefabBullet, transform.position, Quaternion.Euler(0, 0, i));
        }
        Destroy(this.gameObject);
    }
}
