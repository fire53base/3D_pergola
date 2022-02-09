using BzKovSoft.ObjectSlicer;
using BzKovSoft.ObjectSlicer.Samples;
using BzKovSoft.ObjectSlicerSamples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliceGameObject : MonoBehaviour
{
    GameObject Parent;
    public GameObject Right_Plane;
    public GameObject Left_Plane;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Slice_two_sides(GameObject Parent, Vector3 leftPlaneRotation, Vector3 rightPlaneRotation)
    {
        String Parent_name = Parent.name;
        MySlicer script = Parent.AddComponent<MySlicer>();
        script.defaultSliceMaterial = Parent.transform.GetChild(0).GetComponent<Renderer>().material;

        //Right side cut
        Right_Plane.transform.localRotation = Quaternion.Euler(rightPlaneRotation);
        Right_Plane.transform.position = Parent.transform.Find("Cube_right").position;
        var slicable = Parent.GetComponent<IBzSliceableAsync>();        

        Action<BzSliceTryResult> callback_action = (sliced_GO) =>
        {
            if (!sliced_GO.sliced)
            {
                Debug.Log("Model not sliced");
            }
            else
            {
                if (sliced_GO.outObjectPos != null && sliced_GO.outObjectNeg != null)
                {
                    var mesh_pos = sliced_GO.outObjectPos.transform.GetChild(0).transform.GetComponent<MeshFilter>().mesh;
                    var mesh_neg = sliced_GO.outObjectNeg.transform.GetChild(0).transform.GetComponent<MeshFilter>().mesh;

                    Vector3 mesh_pos_size = mesh_pos.bounds.size;
                    Vector3 mesh_neg_size = mesh_neg.bounds.size;

                    var total_pos = mesh_pos_size.x * mesh_pos_size.y * mesh_pos_size.z;
                    var total_neg = mesh_neg_size.x * mesh_neg_size.y * mesh_neg_size.z;

                    if (total_pos >= total_neg)
                    {
                        DestroyImmediate(sliced_GO.outObjectNeg);
                        slicable = sliced_GO.outObjectPos.GetComponent<IBzSliceableAsync>();
                        Parent = sliced_GO.outObjectPos;
                    }
                    else
                    {
                        DestroyImmediate(sliced_GO.outObjectPos);
                        slicable = sliced_GO.outObjectNeg.GetComponent<IBzSliceableAsync>();
                        Parent = sliced_GO.outObjectNeg;
                    }
                    Parent.name = Parent_name;

                }
            }
            };

        if (slicable != null)//checking IBzSliceableAsync script is attached to game object
        {
            var filter_l = Right_Plane.transform.GetComponent<MeshFilter>();
            Vector3 normal_l;

            if (filter_l && filter_l.mesh.normals.Length > 0)
            {
                normal_l = filter_l.transform.TransformDirection(filter_l.mesh.normals[0]);
                var plane = new Plane(normal_l, Right_Plane.transform.position);
                slicable.Slice(plane, callback_action);
            }
        }

        //Left side cut
        Left_Plane.transform.localRotation = Quaternion.Euler(leftPlaneRotation);
        Left_Plane.transform.position = Parent.transform.Find("Cube_left").position;

        if (slicable != null)//checking IBzSliceableAsync script is attached to game object
        {
            var filter_l = Left_Plane.transform.GetComponent<MeshFilter>();

            Vector3 normal_l;

            if (filter_l && filter_l.mesh.normals.Length > 0)
            {
                normal_l = filter_l.transform.TransformDirection(filter_l.mesh.normals[0]);
                var plane = new Plane(normal_l, Left_Plane.transform.position);
                slicable.Slice(plane, callback_action);
            }
        }
    }

    public void Slice_one_side(GameObject Parent, Vector3 leftPlaneRotation, Vector3 PlaneGlobalPosition)
    {
        String Parent_name = Parent.name;
        MySlicer script = Parent.GetComponent<MySlicer>();
        script.defaultSliceMaterial = Parent.transform.GetChild(0).GetComponent<Renderer>().material;
        var slicable = Parent.GetComponent<IBzSliceableAsync>();


        Action<BzSliceTryResult> callback_action = (sliced_GO) =>
        {
            if (!sliced_GO.sliced)
            {
                Debug.Log("Model not sliced");
            }
            else
            {
                if (sliced_GO.outObjectPos != null && sliced_GO.outObjectNeg != null)
                {
                    var mesh_pos = sliced_GO.outObjectPos.transform.GetChild(0).transform.GetComponent<MeshFilter>().mesh;
                    var mesh_neg = sliced_GO.outObjectNeg.transform.GetChild(0).transform.GetComponent<MeshFilter>().mesh;

                    Vector3 mesh_pos_size = mesh_pos.bounds.size;
                    Vector3 mesh_neg_size = mesh_neg.bounds.size;

                    var total_pos = mesh_pos_size.x * mesh_pos_size.y * mesh_pos_size.z;
                    var total_neg = mesh_neg_size.x * mesh_neg_size.y * mesh_neg_size.z;

                    if (total_pos >= total_neg)
                    {
                        DestroyImmediate(sliced_GO.outObjectNeg);
                        slicable = sliced_GO.outObjectPos.GetComponent<IBzSliceableAsync>();
                        Parent = sliced_GO.outObjectPos;
                    }
                    else
                    {
                        DestroyImmediate(sliced_GO.outObjectPos);
                        slicable = sliced_GO.outObjectNeg.GetComponent<IBzSliceableAsync>();
                        Parent = sliced_GO.outObjectNeg;
                    }
                    Parent.name = Parent_name;
                }
            }
        };

        Left_Plane.transform.localRotation = Quaternion.Euler(leftPlaneRotation);
        Left_Plane.transform.position = PlaneGlobalPosition;

        if (slicable != null)//checking IBzSliceableAsync script is attached to game object
        {
            var filter_l = Left_Plane.transform.GetComponent<MeshFilter>();

            Vector3 normal_l;

            if (filter_l && filter_l.mesh.normals.Length > 0)
            {
                normal_l = filter_l.transform.TransformDirection(filter_l.mesh.normals[0]);
                var plane = new Plane(normal_l, Left_Plane.transform.position);
                slicable.Slice(plane, callback_action);
            }
        }
    }
}
