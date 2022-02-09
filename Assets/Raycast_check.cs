using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast_check : MonoBehaviour
{

    public Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void raycast_check()
    {
        Debug.DrawRay(origin,-Vector3.forward*1000,Color.green,15);

        RaycastHit hit1;
       if( Physics.Raycast(origin, -Vector3.forward, out hit1, Mathf.Infinity))
        {
            Debug.Log("object hit: "+ hit1.collider.transform.parent.name);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
