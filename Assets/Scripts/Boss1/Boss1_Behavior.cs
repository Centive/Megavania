using UnityEngine;
using System.Collections;

public class Boss1_Behavior : MonoBehaviour
{
    public enum EBehavior
    {
        None,
        Move,
        Attack1,
        Attack2,
        Attack3
    }
    
    public EBehavior curBehavior;
    private bool lockCoroutine = true;
    
    private int myHealth;
    public GameObject attack1Prefab;
    public GameObject attack2Prefab;
    public GameObject attack3Prefab;


    void Start()
    {
        curBehavior = EBehavior.None;
        myHealth = GetComponent<Enemy_Handler>().myHealth;
        GetComponent<EnemyBehavior_FollowTarget>().target = GameObject.Find("Player_Sprite").transform;
    }
    
    void Update()
    {
        if (lockCoroutine)
        {
            StartCoroutine(BehaviorCoroutine());
            lockCoroutine = false;
        }

        if (myHealth <= 0)
        {
            StopCoroutine(BehaviorCoroutine());
            curBehavior = EBehavior.None;
        }
    }

    void GetState()
    {
        switch (curBehavior)
        {
            case EBehavior.None:
                {
                    //Nothing
                    break;
                }
            case EBehavior.Move:
                {
                    GetComponent<EnemyBehavior_FollowTarget>().isFollowing = true;
                    break;
                }
            case EBehavior.Attack1:
                {
                    State_Attack1();
                    GetComponent<EnemyBehavior_FollowTarget>().isFollowing = false;
                    break;
                }
            case EBehavior.Attack2:
                {
                    State_Attack2();
                    GetComponent<EnemyBehavior_FollowTarget>().isFollowing = false;
                    break;
                }
            case EBehavior.Attack3:
                {
                    State_Attack3();
                    GetComponent<EnemyBehavior_FollowTarget>().isFollowing = false;
                    break;
                }
        }
    }   //Plays the current behavior

    //Attacking states
    void State_Attack1()
    {
        Instantiate(attack1Prefab, transform.position, Quaternion.Euler(0, 0, 90));
        Instantiate(attack1Prefab, transform.position, Quaternion.Euler(0, 0, -90));
    }
    void State_Attack2()
    {
        Instantiate(attack1Prefab, transform.position, Quaternion.Euler(0, 0, 90));
        Instantiate(attack1Prefab, transform.position, Quaternion.Euler(0, 0, -90));
        Instantiate(attack2Prefab, transform.position, Quaternion.identity);
    }
    void State_Attack3()
    {
        Invoke("Invoke1_Attack3", 3);
        Invoke("Invoke1_Attack3", 4);
        Invoke("Invoke1_Attack3", 5);
    }

    //Spawn a set of objs at a certain time for State_Attack3()
    void Invoke1_Attack3()
    {
        GameObject newObj = Instantiate(attack3Prefab, transform.position, Quaternion.identity) as GameObject;
        newObj.GetComponent<EnemyBehavior_FollowTarget>().target = this.transform;
    }   
    void Invoke2_Attack3()
    {
        GameObject newObj = Instantiate(attack3Prefab, transform.position, Quaternion.identity) as GameObject;
        newObj.GetComponent<EnemyBehavior_FollowTarget>().target = this.transform;
    }
    void Invoke3_Attack3()
    {
        GameObject newObj = Instantiate(attack3Prefab, transform.position, Quaternion.identity) as GameObject;
        newObj.GetComponent<EnemyBehavior_FollowTarget>().target = this.transform;
    }
    
    IEnumerator BehaviorCoroutine()
    {
        while (myHealth > 0)
        {
            yield return new WaitForSeconds(6);
            curBehavior = EBehavior.Move;
            GetState();

            yield return new WaitForSeconds(2);
            curBehavior = EBehavior.Attack1;
            GetState();
            
            yield return new WaitForSeconds(4);
            curBehavior = EBehavior.Attack1;
            GetState();

            yield return new WaitForSeconds(1);
            curBehavior = EBehavior.Move;
            GetState();

            yield return new WaitForSeconds(2);
            curBehavior = EBehavior.Attack2;
            GetState();

            yield return new WaitForSeconds(1);
            curBehavior = EBehavior.Attack3;
            GetState();
        }
    }
}
