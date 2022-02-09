using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class update_appearance_for_Texture : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.hasChanged)
        {
            //Debug.Log("Updating tiling");
            transform.hasChanged = false;
            transform.GetComponent<Renderer>().material.mainTextureScale = new Vector2(Mathf.Ceil(transform.localScale.x/1000), Mathf.Ceil(transform.localScale.y/1000));
        }
    }
}
