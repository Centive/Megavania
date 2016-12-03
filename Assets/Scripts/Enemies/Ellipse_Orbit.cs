using UnityEngine;
using System.Collections;

public class Ellipse_Orbit : MonoBehaviour
{
    public float xOffset;
    public float yOffset;
    public float alpha = 0;
    public float curX = 0;
    public float curY = 0;
    public float mySpeed;
    public Transform center;

    void Start()
    {
        center = transform.parent;
    }

    void Update()
    {
        alpha += mySpeed;
        curX = (xOffset * Mathf.Cos(alpha * 0.005f));
        curY = (yOffset * Mathf.Sin(alpha * 0.005f));
        transform.position = center.position + new Vector3(curX, curY);
    }
}
