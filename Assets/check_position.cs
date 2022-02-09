using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class check_position : MonoBehaviour
{
    public Vector3 globalPosition;
    // Start is called before the first frame update
    void Start()
    {
        globalPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = globalPosition;
        Debug.Log(transform.position);
    }
}
