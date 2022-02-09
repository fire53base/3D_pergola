using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dispaly_dimentions : MonoBehaviour
{
    public float x, y, z;
    void Start()
    {
        x = gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x;
        y = gameObject.GetComponent<MeshFilter>().mesh.bounds.size.y;
        z = gameObject.GetComponent<MeshFilter>().mesh.bounds.size.z;
    }

    // Update is called once per frame
    void Update()
    {
        x = gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x;
        y = gameObject.GetComponent<MeshFilter>().mesh.bounds.size.y;
        z = gameObject.GetComponent<MeshFilter>().mesh.bounds.size.z;
    }
}
