using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    //variables
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    //components
    public Transform target;
    private Camera myCamera;
    
    void Start()
    {
        myCamera = GetComponent<Camera>();
    }
    
    void Update()
    {
        if (target)
        {
            Vector3 point = myCamera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.4f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
}