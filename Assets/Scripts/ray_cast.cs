using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ray_cast : MonoBehaviour
{
    public float x, y, z;
    void Start()
    {
        
    }

    private RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.up, out hit,Mathf.Infinity))
        {
            x = hit.point.x;
            y = hit.point.y;
            z = hit.point.z;
         Debug.DrawRay(transform.position, transform.up * hit.distance, Color.green);

        }
        else Debug.DrawRay(transform.position, transform.up * 1000, Color.red);
    }
}
