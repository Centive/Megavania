using UnityEngine;
using System.Collections;

public class Boss1_Attack3_Behavior : MonoBehaviour
{
    public float killTimer;

    void Update()
    {
        //kill this
        killTimer -= Time.deltaTime;
        if (killTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
