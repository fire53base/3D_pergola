using BzKovSoft.ObjectSlicer;
using BzKovSoft.ObjectSlicerSamples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice_Model : MonoBehaviour
{
    public GameObject plane_slice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void slice_Model()
    {
        GameObject Pergola_Model=GameObject.Find("Pergola_Model");

        if (Pergola_Model != null)
        {

            GameObject frame_C = GameObject.Find("FrameC");

            Bounds bound_c = frame_C.GetComponent<MeshFilter>().mesh.bounds;

            MySlicer script;

            if (Pergola_Model.GetComponent<MySlicer>() != null)
                script = Pergola_Model.GetComponent<MySlicer>();
            else
                script = Pergola_Model.AddComponent<MySlicer>();

            script.defaultSliceMaterial = frame_C.GetComponent<Renderer>().material;


            plane_slice.transform.position = frame_C.transform.TransformPoint(bound_c.center);

            plane_slice.transform.up = Camera.main.transform.forward;


            var slicable = Pergola_Model.GetComponent<IBzSliceableAsync>();


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
                        //var mesh_pos = sliced_GO.outObjectPos.transform.GetChild(0).transform.GetComponent<MeshFilter>().mesh;
                        //var mesh_neg = sliced_GO.outObjectNeg.transform.GetChild(0).transform.GetComponent<MeshFilter>().mesh;

                        //Vector3 mesh_pos_size = mesh_pos.bounds.size;
                        //Vector3 mesh_neg_size = mesh_neg.bounds.size;

                        //var total_pos = mesh_pos_size.x * mesh_pos_size.y * mesh_pos_size.z;
                        //var total_neg = mesh_neg_size.x * mesh_neg_size.y * mesh_neg_size.z;

                        //if (total_pos >= total_neg)
                        //{
                        //    //DestroyImmediate(sliced_GO.outObjectNeg);
                        //    slicable = sliced_GO.outObjectPos.GetComponent<IBzSliceableAsync>();
                        //    //Parent = sliced_GO.outObjectPos;
                        //}
                        //else
                        //{
                        //    DestroyImmediate(sliced_GO.outObjectPos);
                        //    slicable = sliced_GO.outObjectNeg.GetComponent<IBzSliceableAsync>();
                        //    Parent = sliced_GO.outObjectNeg;
                        //}
                        //Parent.name = Parent_name;

                        //Hiding positive part
                        sliced_GO.outObjectPos.SetActive(false);
                }
                }

            };

            if (slicable != null)//checking IBzSliceableAsync script is attached to game object
            {
                var filter_l = plane_slice.transform.GetComponent<MeshFilter>();

                Vector3 normal_l;

                if (filter_l && filter_l.mesh.normals.Length > 0)
                {
                    normal_l = filter_l.transform.TransformDirection(filter_l.mesh.normals[0]);
                    var plane = new Plane(normal_l, plane_slice.transform.position);
                    slicable.Slice(plane, callback_action);
                }
            }
        }
    }

    public void slice__LouverModel()
    {
        GameObject VerticalBar_Parent = GameObject.Find("VerticalBar_Parent");

        if (VerticalBar_Parent != null)
        {

            GameObject frame_C = GameObject.Find("vertical0");

            Bounds bound_c = frame_C.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

            MySlicer script;

            if (VerticalBar_Parent.GetComponent<MySlicer>() != null)
                script = VerticalBar_Parent.GetComponent<MySlicer>();
            else
                script = VerticalBar_Parent.AddComponent<MySlicer>();

            script.defaultSliceMaterial = frame_C.GetComponentInChildren<Renderer>().material;


            //plane_slice.transform.position = frame_C.transform.TransformPoint(bound_c.center);

            //plane_slice.transform.up = Camera.main.transform.forward;


            var slicable = VerticalBar_Parent.GetComponent<IBzSliceableAsync>();


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
                        //var mesh_pos = sliced_GO.outObjectPos.transform.GetChild(0).transform.GetComponent<MeshFilter>().mesh;
                        //var mesh_neg = sliced_GO.outObjectNeg.transform.GetChild(0).transform.GetComponent<MeshFilter>().mesh;

                        //Vector3 mesh_pos_size = mesh_pos.bounds.size;
                        //Vector3 mesh_neg_size = mesh_neg.bounds.size;

                        //var total_pos = mesh_pos_size.x * mesh_pos_size.y * mesh_pos_size.z;
                        //var total_neg = mesh_neg_size.x * mesh_neg_size.y * mesh_neg_size.z;

                        //if (total_pos >= total_neg)
                        //{
                        //    //DestroyImmediate(sliced_GO.outObjectNeg);
                        //    slicable = sliced_GO.outObjectPos.GetComponent<IBzSliceableAsync>();
                        //    //Parent = sliced_GO.outObjectPos;
                        //}
                        //else
                        //{
                        //    DestroyImmediate(sliced_GO.outObjectPos);
                        //    slicable = sliced_GO.outObjectNeg.GetComponent<IBzSliceableAsync>();
                        //    Parent = sliced_GO.outObjectNeg;
                        //}
                        //Parent.name = Parent_name;

                        //Hiding positive part
                        sliced_GO.outObjectPos.SetActive(false);
                    }
                }

            };

            if (slicable != null)//checking IBzSliceableAsync script is attached to game object
            {
                var filter_l = plane_slice.transform.GetComponent<MeshFilter>();

                Vector3 normal_l;

                if (filter_l && filter_l.mesh.normals.Length > 0)
                {
                    normal_l = filter_l.transform.TransformDirection(filter_l.mesh.normals[0]);
                    var plane = new Plane(normal_l, plane_slice.transform.position);
                    slicable.Slice(plane, callback_action);
                }
            }
        }
    }


}
