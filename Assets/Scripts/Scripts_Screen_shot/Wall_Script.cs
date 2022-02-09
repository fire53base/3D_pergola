//using AsImpL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
using System.Linq;
using UnityEngine.UI;
using RESTfulHTTPServer.src.invoker;

public class Wall_Script : MonoBehaviour
{

    public static GameObject Wall;
    // Start is called before the first frame update

    public static float FrameA_length, FrameB_length;
    //static Slicer_Controller_bar3 slicer_Controller_Bar3_script;
    public static Toggle A, B, C, D;
    public static Transform suppBar_near_WallD, suppBar_near_WallB;
    public static GameObject Wall_Parent;

    public static bool wall_c = true;
    void Start()
    {
        Wall = (GameObject)Resources.Load("Prefabs/Wall_Cube", typeof(GameObject));//Wall_Cube_Wood//Wall_Cube//Wall_Cube_Texture
        if (GameObject.Find("Wall_A") != null && GameObject.Find("Wall_B") != null && GameObject.Find("Wall_C") != null && GameObject.Find("Wall_D") != null)
        {
            A = GameObject.Find("Wall_A").GetComponent<Toggle>();
            B = GameObject.Find("Wall_B").GetComponent<Toggle>();
            C = GameObject.Find("Wall_C").GetComponent<Toggle>();
            D = GameObject.Find("Wall_D").GetComponent<Toggle>();

            A.onValueChanged.AddListener((value) =>
            {
                Toggle_Build_Wall(value);
            });

            B.onValueChanged.AddListener((value) =>
            {
                Toggle_Build_Wall(value);
            });

            C.onValueChanged.AddListener((value) =>
            {
                Toggle_Build_Wall(value);
            });

            D.onValueChanged.AddListener((value) =>
            {
                Toggle_Build_Wall(value);
            });
        }
    }

    static List<Transform> childs = new List<Transform>();

    public static void FindEveryChild(Transform parent)
    {
        int count = parent.childCount;
        for (int i = 0; i < count; i++)
        {
            childs.Add(parent.GetChild(i));
        }
    }

    public void Toggle_Build_Wall(bool on)
    {
        //if (Slicer_Controller_bar3.I_Type)
        //{
        if (A.isOn == true)
        {
            build_wall_on_sides("A");
        }
        else if (A.isOn == false)
        {
            Destroy(GameObject.Find("Main_Wall_A"));
        }
        if (B.isOn == true)
        {
            build_wall_on_sides("B");
        }
        else if (B.isOn == false)
        {
            Destroy(GameObject.Find("Main_Wall_B"));

            if (suppBar_near_WallB != null)
            {
                //suppBar_near_WallB.gameObject.GetComponent<MeshRenderer>().enabled = true;
                finding_renderers_unhiding(suppBar_near_WallB);
            }
        }
        if (C.isOn == true)
        {
            build_wall_on_sides("C");
        }
        else if (C.isOn == false)
        {
            Destroy(GameObject.Find("Main_Wall_C"));
        }
        if (D.isOn == true)
        {
            build_wall_on_sides("D");
        }
        else if (D.isOn == false)
        {
            Destroy(GameObject.Find("Main_Wall_D"));

            if (suppBar_near_WallD != null)
            {
                //suppBar_near_WallD.gameObject.GetComponent<MeshRenderer>().enabled = true;
                finding_renderers_unhiding(suppBar_near_WallD);
            }
        }
        //}
    }
    public static void Build_wall()
    {
        try
        {
            GameObject Pergola_Model = GameObject.Find("Pergola_Model");

            //GameObject Pergola_Model;
            if (GameObject.Find("Pergola_Model") != null)
            {
                Pergola_Model = GameObject.Find("Pergola_Model");
            }

            Bounds bound_model = Calculate_bounds_collider(Pergola_Model.transform);
            float bolt_length = 23.97845f;//55.17f

            //if (Slicer_Controller_bar3.I_Type)
            //{
            #region uncomment for toggles
            //activatig wall toggles
            //A.gameObject.SetActive(true);
            //B.gameObject.SetActive(true);
            //C.gameObject.SetActive(true);
            //D.gameObject.SetActive(true);
            ////build_wall_on_sides("A");
            //A.isOn = true;
            #endregion

            //Adding Wall based on the Value in DB

            string side_name = "";

            Pergola_Model.transform.rotation = Quaternion.Euler(0, 0, 0);//rotating parent 

            place_cubes_for_Frameends();

        }
        catch (Exception ex)
        {
            Debug.Log("Wall section : " + ex.StackTrace);
        }
        finally
        {

        }


    }
    public static string GetPMAppSideName(string side_name, int corners)
    {
        string pmWepAppSideName = "";
        if (corners == 0)
        {
            switch (side_name)
            {
                case "A":
                    pmWepAppSideName = "C";
                    break;

                case "B":
                    pmWepAppSideName = "D";
                    break;

                case "C":
                    pmWepAppSideName = "A";
                    break;

                case "D":
                    pmWepAppSideName = "B";
                    break;

                default:
                    break;
            }
        }
        else if (corners == 1)
        {
            switch (side_name)
            {
                case "A":
                    pmWepAppSideName = "D";
                    break;

                case "B":
                    pmWepAppSideName = "E";
                    break;

                case "C":
                    pmWepAppSideName = "F";
                    break;

                case "D":
                    pmWepAppSideName = "A";
                    break;

                case "E":
                    pmWepAppSideName = "B";
                    break;
                case "F":
                    pmWepAppSideName = "C";
                    break;

                default:
                    break;
            }
        }
        else if (corners == 2)
        {
            switch (side_name)
            {
                case "A":
                    pmWepAppSideName = "E";
                    break;

                case "B":
                    pmWepAppSideName = "F";
                    break;

                case "C":
                    pmWepAppSideName = "G";
                    break;

                case "D":
                    pmWepAppSideName = "H";
                    break;
                case "E":
                    pmWepAppSideName = "A";
                    break;
                case "F":
                    pmWepAppSideName = "B";
                    break;
                case "G":
                    pmWepAppSideName = "C";
                    break;
                case "H":
                    pmWepAppSideName = "D";
                    break;

                default:
                    break;
            }
        }

        return pmWepAppSideName;
    }

    public static string GetModelSideName(string side_name, int corners)
    {
        string modelSideName = "";
        if (corners == 0)
        {
            switch (side_name)
            {

                case "A":
                    modelSideName = "C";
                    break;

                case "B":
                    modelSideName = "D";
                    break;

                case "C":
                    modelSideName = "A";
                    break;

                case "D":
                    modelSideName = "B";
                    break;

                default:
                    break;
            }
        }
        else if (corners == 1)
        {
            switch (side_name)
            {
                case "A":
                    modelSideName = "D";
                    break;

                case "B":
                    modelSideName = "E";
                    break;

                case "C":
                    modelSideName = "F";
                    break;

                case "D":
                    modelSideName = "A";
                    break;

                case "E":
                    modelSideName = "B";
                    break;

                case "F":
                    modelSideName = "C";
                    break;
                default:
                    break;
            }
        }
        else if (corners == 2)
        {
            switch (side_name)
            {
                case "G":
                    modelSideName = "C";
                    break;

                case "H":
                    modelSideName = "D";
                    break;

                case "A":
                    modelSideName = "E";
                    break;

                case "B":
                    modelSideName = "F";
                    break;
                case "C":
                    modelSideName = "G";
                    break;
                case "D":
                    modelSideName = "H";
                    break;
                case "E":
                    modelSideName = "A";
                    break;
                case "F":
                    modelSideName = "B";
                    break;

                default:
                    break;
            }
        }

        return modelSideName;
    }

    public static void place_cubes_for_Frameends()
    {
        GameObject Pergola_Model = GameObject.Find("Pergola_Model");

        GameObject FrameA = GameObject.Find("FrameA") ? GameObject.Find("FrameA") : GameObject.Find("FrameA_1");

        GameObject FrameC = GameObject.Find("FrameC") ? GameObject.Find("FrameC") : GameObject.Find("FrameC_1");

        GameObject FrameB = GameObject.Find("FrameB") ? GameObject.Find("FrameB") : GameObject.Find("FrameB_1");

        GameObject FrameD = GameObject.Find("FrameD") ? GameObject.Find("FrameD") : GameObject.Find("FrameD_1");

        Bounds bound_B = FrameB.GetComponent<MeshFilter>().mesh.bounds;

        Bounds bound_D = FrameD.GetComponent<MeshFilter>().mesh.bounds;

        Bounds bound_A = FrameA.GetComponent<MeshFilter>().mesh.bounds;

        Bounds bound_C = FrameC.GetComponent<MeshFilter>().mesh.bounds;

        #region FrameA region


        Vector3 global_center_A = FrameA.transform.TransformPoint(bound_A.center);

        Vector3 global_center_C = FrameC.transform.TransformPoint(bound_C.center);

        Vector3 global_center_B = FrameB.transform.TransformPoint(bound_B.center);

        Vector3 global_center_D = FrameD.transform.TransformPoint(bound_D.center);

        //taking direction from center of A to center of  C
        Vector3 AC_direction = global_center_A - global_center_C;


        //Direction from A to C
        Vector3 direction_A2C_dist_wrt_local_A = FrameA.transform.InverseTransformDirection(AC_direction).normalized;


        //distance from A to C (length of Bar 'B' )
        float distance_AC = Vector3.Distance(global_center_A, global_center_C);

        //Taking round pf x,y,z components of direction_dist_wrt_local_A vector
        direction_A2C_dist_wrt_local_A = new Vector3(Mathf.Round(direction_A2C_dist_wrt_local_A.x), Mathf.Round(direction_A2C_dist_wrt_local_A.y), Mathf.Round(direction_A2C_dist_wrt_local_A.z));


        //Dot product of (0,0,1 ).(0,0,80)=80
        if ((direction_A2C_dist_wrt_local_A.x == 0 && direction_A2C_dist_wrt_local_A.y == 0) || (direction_A2C_dist_wrt_local_A.y == 0 && direction_A2C_dist_wrt_local_A.z == 0) || (direction_A2C_dist_wrt_local_A.x == 0 && direction_A2C_dist_wrt_local_A.z == 0))
        {
            distance_AC += Mathf.Abs(Vector3.Dot(direction_A2C_dist_wrt_local_A, bound_A.size));
            FrameB_length = Mathf.Round(distance_AC);
        }
        Debug.Log("Distance bw A & C =" + distance_AC);

        //taking direction from center of B to center of D
        Vector3 BD_direction = global_center_B - global_center_D;

        //Direction from  A to C
        Vector3 direction_B2D_dist_wrt_local_B = FrameB.transform.InverseTransformDirection(BD_direction).normalized;

        //Taking round pf x,y,z components of direction_dist_wrt_local_B vector
        direction_B2D_dist_wrt_local_B = new Vector3(Mathf.Round(direction_B2D_dist_wrt_local_B.x), Mathf.Round(direction_B2D_dist_wrt_local_B.y), Mathf.Round(direction_B2D_dist_wrt_local_B.z));

        float distance_BD = Vector3.Distance(global_center_B, global_center_D);

        //Dot product of (0,0,1 ).(0,0,80)=80
        if ((direction_B2D_dist_wrt_local_B.x == 0 && direction_B2D_dist_wrt_local_B.y == 0) || (direction_B2D_dist_wrt_local_B.x == 0 && direction_B2D_dist_wrt_local_B.z == 0) || (direction_B2D_dist_wrt_local_B.y == 0 && direction_B2D_dist_wrt_local_B.z == 0))
        {
            distance_BD += Mathf.Abs(Vector3.Dot(direction_B2D_dist_wrt_local_B, bound_B.size));

            FrameA_length = Mathf.Round(distance_BD);
        }
        Debug.Log("Distance bw B & D =" + distance_BD);

        #region uncomment for placing cube
        //GameObject cube_A2B = GameObject.CreatePrimitive(PrimitiveType.Cube);//generating cube to be placed at position of support clamp cube of supportBar positioning

        //cube_A2B.transform.position =FrameB.transform.TransformPoint( bound_B.center);

        //cube_A2B.transform.rotation = FrameA.transform.rotation;

        //cube_A2B.name = "Cube_A2B";


        #endregion
        //cube_A2B.transform.parent = Pergola_Model.transform;

        //Vector3 to_moveup= FrameB.transform.TransformDirection(FrameB.transform.up);

        //float to_moveup_x =Mathf.Round(to_moveup.x);
        //float to_moveup_y = Mathf.Round(to_moveup.y);
        //float to_moveup_z = Mathf.Round(to_moveup.z);

        //Vector3 center_A_global = FrameA.transform.parent.TransformPoint(bound_A.center);
        //if(to_moveup_x==0&&to_moveup_y==0)
        //{

        //    cube_A2B.transform.position = new Vector3(cube_A2B.transform.position.x, cube_A2B.transform.localPosition.y,center_A_global.z);
        //}
        //else if(to_moveup_x==0&&to_moveup_z==0)
        //{
        //    cube_A2B.transform.position = new Vector3(cube_A2B.transform.position.x, center_A_global.y, cube_A2B.transform.position.z);
        //}
        //else if(to_moveup_y==0&&to_moveup_z==0)
        //{
        //    cube_A2B.transform.position = new Vector3(center_A_global.x,cube_A2B.transform.position.y , cube_A2B.transform.position.z);
        //}

        #region uncomment for placing cube
        //cube_A2B.transform.parent = FrameA.transform;

        //GameObject cube_A2D = GameObject.CreatePrimitive(PrimitiveType.Cube);//generating cube to be placed at position of support clamp cube of supportBar positioning

        //cube_A2D.transform.position = FrameD.transform.TransformPoint( bound_D.center);

        //cube_A2D.transform.rotation = FrameA.transform.rotation;

        //cube_A2D.name = "Cube_A2D";
        //cube_A2D.transform.parent = FrameA.transform;
        #endregion
        //cube_A2D.transform.parent = Pergola_Model.transform;

        //to_moveup = FrameD.transform.TransformDirection(FrameD.transform.up);

        // to_moveup_x = Mathf.Round(to_moveup.x);
        // to_moveup_y = Mathf.Round(to_moveup.y);
        // to_moveup_z = Mathf.Round(to_moveup.z);


        //if (to_moveup_x == 0 && to_moveup_y == 0)
        //{

        //    cube_A2D.transform.position = new Vector3(cube_A2D.transform.position.x, cube_A2D.transform.localPosition.y, center_A_global.z);
        //}
        //else if (to_moveup_x == 0 && to_moveup_z == 0)
        //{
        //    cube_A2D.transform.position = new Vector3(cube_A2D.transform.position.x, center_A_global.y, cube_A2D.transform.position.z);
        //}
        //else if (to_moveup_y == 0 && to_moveup_z == 0)
        //{
        //    cube_A2D.transform.position = new Vector3(center_A_global.x, cube_A2D.transform.position.y, cube_A2D.transform.position.z);
        //}

        #endregion


    }
    public static Bounds Calculate_bounds_collider(Transform TheObject)//To calculate bound of all the bars 
    {
        var renderers = TheObject.GetComponentsInChildren<MeshFilter>();
        Bounds combinedBounds = new Bounds();
        if (renderers.Length > 0)
        {
            combinedBounds = renderers[0].mesh.bounds;
        }
        for (int i = 0; i < renderers.Length; i++)
            combinedBounds.Encapsulate(renderers[i].mesh.bounds);

        return combinedBounds;
    }

    public static void finding_renderers_hiding_unhiding(Transform TheObject)//To calculate bound of all the bars 
    {
        if (TheObject.GetComponentsInChildren<MeshRenderer>() != null)
        {
            var renderers = TheObject.GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < renderers.Length; i++)
            {

                renderers[i].enabled = false;


            }
        }

    }

    public static void finding_renderers_unhiding(Transform TheObject)//To calculate bound of all the bars 
    {
        if (TheObject.GetComponentsInChildren<MeshRenderer>() != null)
        {
            var renderers = TheObject.GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < renderers.Length; i++)
            {

                renderers[i].enabled = true;


            }
        }

    }
    public void build_wall_on_sides(string side_name, string wall_prefab_name = "")
    {
        if (DB_script.supportBarLengths != null)
        {
            GameObject Pergola_Model = GameObject.Find("Pergola_Model");
            Wall_Parent = GameObject.Find("Wall_Parent");
            if (Wall_Parent == null)
            {
                Wall_Parent = new GameObject("Wall_Parent");
                Wall_Parent.transform.parent = Pergola_Model.transform;
            }
            //else
            //{
            //    foreach(Transform wall in Wall_Parent.transform)
            //    {


            //        DestroyImmediate(wall.gameObject);
            //    }
            //}

            if (!string.IsNullOrEmpty(wall_prefab_name))
            {
                Wall = (GameObject)Resources.Load($"Prefabs/{wall_prefab_name}", typeof(GameObject));

                if (Wall == null)
                {
                    Wall = (GameObject)Resources.Load("Prefabs/Wall_Cube", typeof(GameObject));
                }
            }
            else
            {
                Wall = (GameObject)Resources.Load("Prefabs/Wall_Cube", typeof(GameObject));
            }
            Characteristics chrs = Wall.gameObject.GetComponent<Characteristics>();

            if (chrs == null)
            {

                chrs = Wall.gameObject.AddComponent<Characteristics>();

            }
            chrs.part_type = "cube";
            chrs.part_name_id = "WALL";
            chrs.part_unique_id = Guid.NewGuid().ToString();
     

            //GameObject Pergola_Model = Pergola_Model.transform.Find("Pergola_Model").gameObject;
            Bounds bound_model = Calculate_bounds_collider(Pergola_Model.transform);
            float bolt_length = 23.97845f;//55.17f
            float bound_model_x = Vector3.Dot(Vector3.right, bound_model.size);

            float bound_model_y = Vector3.Dot(Vector3.up, bound_model.size);

            float bound_model_z = Vector3.Dot(Vector3.forward, bound_model.size);

            if (DB_script.I_type)
            {

                string FrameAname = GameObject.Find("FrameA") != null ? "FrameA" : "FrameA_0";

                GameObject FrameA = GameObject.Find(FrameAname);

                Bounds FrameA_bounds = FrameA.transform.GetComponent<MeshFilter>().mesh.bounds;

                string FrameBname = GameObject.Find("FrameB") != null ? "FrameB" : "FrameB_0";

                GameObject FrameB = GameObject.Find(FrameBname);

                Bounds FrameB_bounds = FrameB.GetComponent<MeshFilter>().mesh.bounds;

                float frameB_x = Mathf.Abs(Vector3.Dot(FrameB.transform.InverseTransformDirection(Pergola_Model.transform.right).normalized, FrameB_bounds.size));

                float frameB_y = Mathf.Abs(Vector3.Dot(FrameB.transform.InverseTransformDirection(Pergola_Model.transform.up).normalized, FrameB_bounds.size));

                float frameB_z = Mathf.Abs(Vector3.Dot(FrameB.transform.InverseTransformDirection(Pergola_Model.transform.forward).normalized, FrameB_bounds.size));

                string FrameCname = GameObject.Find("FrameC") != null ? "FrameC" : "FrameC_0";
                GameObject FrameC = GameObject.Find(FrameCname);
                Bounds FrameC_bounds = GameObject.Find(FrameCname).GetComponent<MeshFilter>().mesh.bounds;

                float frameC_x = Mathf.Abs(Vector3.Dot(FrameC.transform.InverseTransformDirection(Pergola_Model.transform.right).normalized, FrameC_bounds.size));

                float frameC_y = Mathf.Abs(Vector3.Dot(FrameC.transform.InverseTransformDirection(Pergola_Model.transform.up).normalized, FrameC_bounds.size));

                float frameC_z = Mathf.Abs(Vector3.Dot(FrameC.transform.InverseTransformDirection(Pergola_Model.transform.forward).normalized, FrameC_bounds.size));


                string FrameDname = GameObject.Find("FrameD") != null ? "FrameD" : "FrameD_0";

                GameObject FrameD = GameObject.Find(FrameDname);

                Bounds FrameD_bounds = FrameD.GetComponent<MeshFilter>().mesh.bounds;

                float frameD_x = Mathf.Abs(Vector3.Dot(FrameD.transform.InverseTransformDirection(Pergola_Model.transform.right).normalized, FrameD_bounds.size));

                float frameD_y = Mathf.Abs(Vector3.Dot(FrameD.transform.InverseTransformDirection(Pergola_Model.transform.up).normalized, FrameD_bounds.size));

                float frameD_z = Mathf.Abs(Vector3.Dot(FrameD.transform.InverseTransformDirection(Pergola_Model.transform.forward).normalized, FrameD_bounds.size));


                //HEIGHT OF THE WALL
                float wall_y_height = DB_script.supportBarLengths.supportWall_length + 300;

                //wIDTH OF L TYPE
                float L_width = 1.2f;


                float wall_width = 100;

                if (DB_script.frame_A_length != 0)
                {
                    wall_width = DB_script.frame_A_length / 50;
                }

                if (wall_width < 100)
                {
                    wall_width = 100;
                }
                else if (wall_width > 400)
                {
                    wall_width = 400;
                }

                if (side_name == "A")//&& GameObject.Find("Main_Wall_" + "A") == null)
                {
                    //A.isOn = true;
                    string wall_name = "Main_Wall_" + "A";

                    if (GameObject.Find(wall_name) != null)
                    {
                        DestroyImmediate(GameObject.Find(wall_name));
                    }

                    GameObject Main_Wall = Instantiate(Wall);
                    Main_Wall.name = wall_name;
                    Main_Wall.transform.parent = Pergola_Model.transform;
                    Main_Wall.transform.rotation = Pergola_Model.transform.rotation;
                    Main_Wall.transform.localPosition = new Vector3(0, 0, 0);
                    Main_Wall.transform.Rotate(0, 90, 0, Space.Self);
                    float size_A_0 = Vector3.Distance(new Vector3(FrameA.transform.GetChild(0).position.x, 0f, 0f), new Vector3(FrameA.transform.GetChild(1).position.x, 0f, 0f));//1st frame calculation
                    float size_A_frame = 0;//= Vector3.Distance(new Vector3(FrameA.transform.GetChild(0).position.x, 0f, 0f), new Vector3(FrameA.transform.GetChild(1).position.x, 0f, 0f));


                    foreach (Transform child in Pergola_Model.transform)
                    {
                        if (child.name.Contains("FrameA"))
                        {
                            //FrameA_list.Add(child.gameObject);

                            size_A_frame += Vector3.Distance(new Vector3(child.GetChild(0).position.x, 0f, 0f), new Vector3(child.GetChild(1).position.x, 0f, 0f));
                        }
                    }
                    Main_Wall.transform.parent = Pergola_Model.transform;


                    float side_a_value = 0;
                    side_a_value = DB_script.frame_A_length;
                    float fwd_offset = DB_script.frame_A_length;


                    Main_Wall.transform.localScale = new Vector3(side_a_value, wall_y_height, wall_width);
                    Vector3 pos_wall_A = new Vector3(FrameA_bounds.center.y, wall_y_height / 2, side_a_value / 2);

                    Main_Wall.transform.position = FrameA.transform.TransformPoint(FrameA_bounds.center);


                    float width_of_A_x = Mathf.Abs(Vector3.Dot(FrameA.transform.InverseTransformDirection(Pergola_Model.transform.right), FrameA_bounds.size));

                    float width_of_A_y = Mathf.Abs(Vector3.Dot(FrameA.transform.InverseTransformDirection(Pergola_Model.transform.up), FrameA_bounds.size));
                    //Main_Wall.transform.localPosition = (pos_wall_A);
                    if (GameObject.Find("FrameA_0"))
                    {
                        fwd_offset = DB_script.frame_A_length / 2 - FrameA.transform.localScale.y / 2;
                        Main_Wall.transform.Translate(Pergola_Model.transform.forward * fwd_offset, Space.World);
                    }

                    Main_Wall.transform.Translate(Pergola_Model.transform.right * -(wall_width / 2 + width_of_A_x / 2 + L_width), Space.World);
                    //Main_Wall.transform.Translate(-wall_width / 2, 0, fwd_offset);//FrameA_bounds.size.z/2
                    Main_Wall.transform.Translate(Pergola_Model.transform.up * wall_y_height / 2, Space.World);
                    Main_Wall.transform.Translate(-Pergola_Model.transform.up * width_of_A_y / 2, Space.World);
                    Main_Wall.transform.parent = Wall_Parent.transform;

                    
                }

                else if (side_name == "D") //&& GameObject.Find("Main_Wall_D") == null)
                {


                    string wall_name = "Main_Wall_D";

                    if (GameObject.Find(wall_name) != null)
                    {
                        DestroyImmediate(GameObject.Find(wall_name));
                    }
                    //D.isOn = true;
                    //***********************FrameD and Wall_D_main**************************************//
                    GameObject Main_Wall_D = Instantiate(Wall);


                    Main_Wall_D.name = wall_name;

                    float side_d_value = 0;

                    side_d_value = DB_script.frame_D_length;

                    Main_Wall_D.transform.parent = Pergola_Model.transform;
                    Main_Wall_D.transform.rotation = Pergola_Model.transform.rotation;
                    Main_Wall_D.transform.localPosition = new Vector3(0, 0, 0);
                    float FrameD_size = Vector3.Distance(FrameD.transform.GetChild(0).position, FrameD.transform.GetChild(1).position);

                    Vector3 pos_wall_D = FrameD_bounds.center;// + new Vector3(-bolt_length,0f,0f);// + new Vector3(-( FrameD_bounds.size.x), 0f, 0f);

                    Main_Wall_D.transform.position = FrameD.transform.TransformPoint(FrameD_bounds.center);
                    float right_offset = DB_script.frame_D_length;


                    Main_Wall_D.transform.localScale = new Vector3(side_d_value, wall_y_height, wall_width);
                    float width_of_B_z = Mathf.Abs(Vector3.Dot(FrameD.transform.InverseTransformDirection(Pergola_Model.transform.forward), FrameD_bounds.size));
                    float width_of_B_y = Mathf.Abs(Vector3.Dot(FrameD.transform.InverseTransformDirection(Pergola_Model.transform.up), FrameD_bounds.size));

                    if (GameObject.Find("FrameD_0"))
                    {
                        right_offset = DB_script.frame_D_length / 2 - FrameD.transform.localScale.y / 2;
                        Main_Wall_D.transform.Translate(Pergola_Model.transform.right * (right_offset / 2), Space.World);
                    }

                    Main_Wall_D.transform.Translate(-Pergola_Model.transform.forward * (width_of_B_z / 2 + wall_width / 2 + L_width), Space.World);
                    Main_Wall_D.transform.Translate(-Pergola_Model.transform.up * width_of_B_y / 2, Space.World);
                    Main_Wall_D.transform.Translate(Pergola_Model.transform.up * wall_y_height / 2, Space.World);
                    Main_Wall_D.transform.parent = Wall_Parent.transform;

                    //DB_script.support_bar_nearest(GameObject.Find("SupportBars_Parent"), Main_Wall_D, Pergola_Model.transform.forward);
                }

                //Wall_B
                else if (side_name == "B")// && GameObject.Find("Main_Wall_B") == null)
                {
                    string wall_name = "Main_Wall_B";

                    if (GameObject.Find(wall_name) != null)
                    {
                        DestroyImmediate(GameObject.Find(wall_name));
                    }
                    //B.isOn = true;
                    GameObject Main_Wall_B = Instantiate(Wall);


                    Main_Wall_B.name = wall_name;

                    ////Main_Wall.transform.position = FrameC.transform.TransformPoint(pos_wall_C);
                    //var side_B = Generate_Model.formPergola_global.pergola_measurement.FirstOrDefault(m => m.side_name == "B");
                    float side_B_value = 0;
                    side_B_value = DB_script.frame_B_length;
                    //if (side_B != null) side_B_value = (float)(side_B.side_value);


                    Main_Wall_B.transform.parent = Pergola_Model.transform;

                    Main_Wall_B.transform.rotation = Pergola_Model.transform.rotation;

                    Main_Wall_B.transform.localPosition = new Vector3(0, 0, 0);


                    Main_Wall_B.transform.position = FrameB.transform.TransformPoint(FrameB_bounds.center);
                    float right_offset = DB_script.frame_B_length;


                    Main_Wall_B.transform.localScale = new Vector3(side_B_value, wall_y_height, wall_width);

                    float width_of_B_z = Mathf.Abs(Vector3.Dot(FrameB.transform.InverseTransformDirection(Pergola_Model.transform.forward), FrameB_bounds.size));

                    float width_of_B_y = Mathf.Abs(Vector3.Dot(FrameB.transform.InverseTransformDirection(Pergola_Model.transform.up), FrameB_bounds.size));


                    //Main_Wall_B.transform.Translate(-Pergola_Model.transform.right * right_offset,Space.World);
                    if (GameObject.Find("FrameB_0"))
                    {
                        right_offset = DB_script.frame_B_length / 2 - FrameB.transform.localScale.y / 2;
                        Main_Wall_B.transform.Translate(Pergola_Model.transform.right * (right_offset / 2), Space.World);
                    }


                    //Main_Wall_B.transform.Translate(Pergola_Model.transform.right * (right_offset/2), Space.World);
                    Main_Wall_B.transform.Translate(Pergola_Model.transform.forward * (width_of_B_z / 2 + wall_width / 2 + L_width), Space.World);
                    Main_Wall_B.transform.Translate(-Pergola_Model.transform.up * width_of_B_y / 2, Space.World);
                    Main_Wall_B.transform.Translate(Pergola_Model.transform.up * wall_y_height / 2, Space.World);
                    Main_Wall_B.transform.parent = Wall_Parent.transform;

                    //DB_script.support_bar_nearest(GameObject.Find("SupportBars_Parent"), Main_Wall_B, Pergola_Model.transform.forward);
                }
                //Wall C
                else if (side_name == "C")// && GameObject.Find("Main_Wall_C") == null)
                {

                    string wall_name = "Main_Wall_C";

                    if (GameObject.Find(wall_name) != null)
                    {
                        DestroyImmediate(GameObject.Find(wall_name));
                    }
                    //C.isOn = true;
                    GameObject Main_Wall_C = Instantiate(Wall);

                    Main_Wall_C.name = wall_name;

                    Main_Wall_C.transform.parent = Pergola_Model.transform;
                    Main_Wall_C.transform.rotation = Pergola_Model.transform.rotation;


                    Main_Wall_C.transform.localPosition = new Vector3(0, 0, 0);

                    Main_Wall_C.transform.Rotate(0, 90, 0);

                    //var side_C = Generate_Model.formPergola_global.pergola_measurement.FirstOrDefault(m => m.side_name == "C");
                    float side_C_value = 0;
                    side_C_value = DB_script.frame_C_length;
                    //if (side_C != null) side_C_value = (float)(side_C.side_value);

                    Vector3 FrameC_global_center = FrameC.transform.TransformPoint(FrameC_bounds.center);

                    float fwd_offset = DB_script.frame_C_length;



                    Main_Wall_C.transform.localScale = new Vector3(side_C_value, wall_y_height, wall_width);
                    Vector3 pos_wall_A = new Vector3(FrameC_bounds.center.y, wall_y_height / 2, side_C_value / 2);

                    Main_Wall_C.transform.position = FrameC.transform.TransformPoint(FrameC_bounds.center);


                    float width_of_C_x = Mathf.Abs(Vector3.Dot(FrameC.transform.InverseTransformDirection(Pergola_Model.transform.right), FrameC_bounds.size));

                    float width_of_C_y = Mathf.Abs(Vector3.Dot(FrameC.transform.InverseTransformDirection(Pergola_Model.transform.up), FrameC_bounds.size));
                    if (GameObject.Find("FrameC_0"))
                    {
                        fwd_offset = DB_script.frame_C_length / 2 - FrameC.transform.localScale.y / 2;
                        Main_Wall_C.transform.Translate(-Pergola_Model.transform.forward * fwd_offset, Space.World);

                    }
                    Main_Wall_C.transform.Translate(Pergola_Model.transform.up * wall_y_height / 2, Space.World);
                    Main_Wall_C.transform.Translate(-Pergola_Model.transform.up * width_of_C_y / 2, Space.World);
                    Main_Wall_C.transform.Translate(Pergola_Model.transform.right * (wall_width / 2 + width_of_C_x / 2 + L_width), Space.World);
                    Main_Wall_C.transform.parent = Wall_Parent.transform;
                }
            }

            else if (DB_script.L_type)
            {

                string FrameAname = GameObject.Find("FrameA") != null ? "FrameA" : "FrameA_0";

                GameObject FrameA = GameObject.Find(FrameAname);

                Bounds FrameA_bounds = FrameA.transform.GetComponent<MeshFilter>().mesh.bounds;

                string FrameBname = GameObject.Find("FrameB") != null ? "FrameB" : "FrameB_0";

                GameObject FrameB = GameObject.Find(FrameBname);

                Bounds FrameB_bounds = FrameB.GetComponent<MeshFilter>().mesh.bounds;

                float frameB_x = Mathf.Abs(Vector3.Dot(FrameB.transform.InverseTransformDirection(Pergola_Model.transform.right).normalized, FrameB_bounds.size));

                float frameB_y = Mathf.Abs(Vector3.Dot(FrameB.transform.InverseTransformDirection(Pergola_Model.transform.up).normalized, FrameB_bounds.size));

                float frameB_z = Mathf.Abs(Vector3.Dot(FrameB.transform.InverseTransformDirection(Pergola_Model.transform.forward).normalized, FrameB_bounds.size));

                string FrameCname = GameObject.Find("FrameC") != null ? "FrameC" : "FrameC_0";
                GameObject FrameC = GameObject.Find(FrameCname);
                Bounds FrameC_bounds = GameObject.Find(FrameCname).GetComponent<MeshFilter>().mesh.bounds;

                float frameC_x = Mathf.Abs(Vector3.Dot(FrameC.transform.InverseTransformDirection(Pergola_Model.transform.right).normalized, FrameC_bounds.size));

                float frameC_y = Mathf.Abs(Vector3.Dot(FrameC.transform.InverseTransformDirection(Pergola_Model.transform.up).normalized, FrameC_bounds.size));

                float frameC_z = Mathf.Abs(Vector3.Dot(FrameC.transform.InverseTransformDirection(Pergola_Model.transform.forward).normalized, FrameC_bounds.size));


                string FrameDname = GameObject.Find("FrameD") != null ? "FrameD" : "FrameD_0";

                GameObject FrameD = GameObject.Find(FrameDname);

                Bounds FrameD_bounds = FrameD.GetComponent<MeshFilter>().mesh.bounds;

                float frameD_x = Mathf.Abs(Vector3.Dot(FrameD.transform.InverseTransformDirection(Pergola_Model.transform.right).normalized, FrameD_bounds.size));

                float frameD_y = Mathf.Abs(Vector3.Dot(FrameD.transform.InverseTransformDirection(Pergola_Model.transform.up).normalized, FrameD_bounds.size));

                float frameD_z = Mathf.Abs(Vector3.Dot(FrameD.transform.InverseTransformDirection(Pergola_Model.transform.forward).normalized, FrameD_bounds.size));

                string FrameEname = GameObject.Find("FrameE") != null ? "FrameE" : "FrameE_0";

                GameObject FrameE = GameObject.Find(FrameEname);

                Bounds FrameE_bounds = FrameE.GetComponent<MeshFilter>().mesh.bounds;

                string FrameFname = GameObject.Find("FrameF") != null ? "FrameF" : "FrameF_0";

                GameObject FrameF = GameObject.Find(FrameFname);

                Bounds FrameF_bounds = FrameE.GetComponent<MeshFilter>().mesh.bounds;



                //HEIGHT OF THE WALL
                float wall_y_height = DB_script.supportBarLengths.supportWall_length + 300;

                //wIDTH OF L TYPE
                float L_width = 1.2f;


                float wall_width = 100;

                if (DB_script.frame_A_length != 0)
                {
                    wall_width = DB_script.frame_A_length / 50;
                }

                if (wall_width < 100)
                {
                    wall_width = 100;
                }
                else if (wall_width > 400)
                {
                    wall_width = 400;
                }

                if (side_name == "A")//&& GameObject.Find("Main_Wall_" + "A") == null)
                {
                    //A.isOn = true;
                    string wall_name = "Main_Wall_" + "A";

                    if (GameObject.Find(wall_name) != null)
                    {
                        DestroyImmediate(GameObject.Find(wall_name));
                    }

                    GameObject Main_Wall = Instantiate(Wall);
                    Main_Wall.name = wall_name;
                    Main_Wall.transform.parent = Pergola_Model.transform;
                    Main_Wall.transform.rotation = Pergola_Model.transform.rotation;
                    Main_Wall.transform.localPosition = new Vector3(0, 0, 0);
                    //Main_Wall.transform.Rotate(0, 90, 0, Space.Self);
                    float size_A_0 = Vector3.Distance(new Vector3(FrameA.transform.GetChild(0).position.x, 0f, 0f), new Vector3(FrameA.transform.GetChild(1).position.x, 0f, 0f));//1st frame calculation
                    float size_A_frame = 0;//= Vector3.Distance(new Vector3(FrameA.transform.GetChild(0).position.x, 0f, 0f), new Vector3(FrameA.transform.GetChild(1).position.x, 0f, 0f));


                    foreach (Transform child in Pergola_Model.transform)
                    {
                        if (child.name.Contains("FrameA"))
                        {
                            //FrameA_list.Add(child.gameObject);

                            size_A_frame += Vector3.Distance(new Vector3(child.GetChild(0).position.x, 0f, 0f), new Vector3(child.GetChild(1).position.x, 0f, 0f));
                        }
                    }
                    Main_Wall.transform.parent = Pergola_Model.transform;


                    float side_a_value = 0;
                    side_a_value = DB_script.frame_A_length;
                    float fwd_offset = DB_script.frame_A_length;


                    Main_Wall.transform.localScale = new Vector3(side_a_value, wall_y_height, wall_width);
                    Vector3 pos_wall_A = new Vector3(FrameA_bounds.center.y, wall_y_height / 2, side_a_value / 2);

                    Main_Wall.transform.position = FrameA.transform.TransformPoint(FrameA_bounds.center);


                    float width_of_A_x = Mathf.Abs(Vector3.Dot(FrameA.transform.InverseTransformDirection(Pergola_Model.transform.right), FrameA_bounds.size));

                    float width_of_A_y = Mathf.Abs(Vector3.Dot(FrameA.transform.InverseTransformDirection(Pergola_Model.transform.up), FrameA_bounds.size));

                    float width_of_A_z = Mathf.Abs(Vector3.Dot(FrameA.transform.InverseTransformDirection(Pergola_Model.transform.forward), FrameA_bounds.size));
                    //Main_Wall.transform.localPosition = (pos_wall_A);
                    if (GameObject.Find("FrameA_0"))
                    {
                        fwd_offset = DB_script.frame_A_length / 2 - FrameA.transform.localScale.y / 2;
                        Main_Wall.transform.Translate(Pergola_Model.transform.right * fwd_offset, Space.World);//right for right
                    }

                    Main_Wall.transform.Translate(Pergola_Model.transform.right * -( width_of_A_z / 2 ), Space.World);
                    //Main_Wall.transform.Translate(-wall_width / 2, 0, fwd_offset);//FrameA_bounds.size.z/2
                    Main_Wall.transform.Translate(Pergola_Model.transform.up * wall_y_height / 2, Space.World);
                    Main_Wall.transform.Translate(-Pergola_Model.transform.up * width_of_A_y / 2, Space.World);
                    Main_Wall.transform.Translate(-Pergola_Model.transform.forward * -(wall_width /2 + width_of_A_z / 2 + L_width), Space.World);
                    //Main_Wall.transform.Translate(Pergola_Model.transform.forward *(wall_width), Space.World);
                    Main_Wall.transform.parent = Wall_Parent.transform;
                }

                else if (side_name == "D") //&& GameObject.Find("Main_Wall_D") == null)
                {


                    string wall_name = "Main_Wall_D";

                    if (GameObject.Find(wall_name) != null)
                    {
                        DestroyImmediate(GameObject.Find(wall_name));
                    }
                    //D.isOn = true;
                    //***********************FrameD and Wall_D_main**************************************//
                    GameObject Main_Wall_D = Instantiate(Wall);


                    Main_Wall_D.name = wall_name;

                    float side_d_value = 0;

                    side_d_value = DB_script.frame_D_length;

                    Main_Wall_D.transform.parent = Pergola_Model.transform;
                    Main_Wall_D.transform.rotation = Pergola_Model.transform.rotation;
                    Main_Wall_D.transform.localPosition = new Vector3(0, 0, 0);
                    float FrameD_size = Vector3.Distance(FrameD.transform.GetChild(0).position, FrameD.transform.GetChild(1).position);
                    Main_Wall_D.transform.Rotate(0, 90, 0, Space.Self);

                    Vector3 pos_wall_D = FrameD_bounds.center;// + new Vector3(-bolt_length,0f,0f);// + new Vector3(-( FrameD_bounds.size.x), 0f, 0f);

                    Main_Wall_D.transform.position = FrameD.transform.TransformPoint(FrameD_bounds.center);
                    float right_offset = DB_script.frame_D_length;


                    Main_Wall_D.transform.localScale = new Vector3(side_d_value, wall_y_height, wall_width);
                    float width_of_B_x = Mathf.Abs(Vector3.Dot(FrameD.transform.InverseTransformDirection(Pergola_Model.transform.right), FrameD_bounds.size));
                    float width_of_B_y = Mathf.Abs(Vector3.Dot(FrameD.transform.InverseTransformDirection(Pergola_Model.transform.up), FrameD_bounds.size));

                    if (GameObject.Find("FrameD_0"))
                    {
                        right_offset = DB_script.frame_D_length / 2 - FrameD.transform.localScale.y / 2;
                        Main_Wall_D.transform.Translate(Pergola_Model.transform.right * (right_offset / 2), Space.World);
                    }

                    Main_Wall_D.transform.Translate(Pergola_Model.transform.right * (width_of_B_x / 2 + wall_width / 2 + L_width), Space.World);
                    Main_Wall_D.transform.Translate(-Pergola_Model.transform.up * width_of_B_y / 2, Space.World);
                    Main_Wall_D.transform.Translate(Pergola_Model.transform.up * wall_y_height / 2, Space.World);
                    Main_Wall_D.transform.parent = Wall_Parent.transform;
                }

                //Wall_B
                else if (side_name == "B")// && GameObject.Find("Main_Wall_B") == null)
                {
                    string wall_name = "Main_Wall_B";

                    if (GameObject.Find(wall_name) != null)
                    {
                        DestroyImmediate(GameObject.Find(wall_name));
                    }
                    //B.isOn = true;
                    GameObject Main_Wall_B = Instantiate(Wall);


                    Main_Wall_B.name = wall_name;

                    ////Main_Wall.transform.position = FrameC.transform.TransformPoint(pos_wall_C);
                    //var side_B = Generate_Model.formPergola_global.pergola_measurement.FirstOrDefault(m => m.side_name == "B");
                    float side_B_value = 0;
                    side_B_value = DB_script.frame_B_length;
                    //if (side_B != null) side_B_value = (float)(side_B.side_value);


                    Main_Wall_B.transform.parent = Pergola_Model.transform;

                    Main_Wall_B.transform.rotation = Pergola_Model.transform.rotation;

                    Main_Wall_B.transform.localPosition = new Vector3(0, 0, 0);
                    Main_Wall_B.transform.Rotate(0, 90, 0, Space.Self);

                    Main_Wall_B.transform.position = FrameB.transform.TransformPoint(FrameB_bounds.center);
                    float right_offset = DB_script.frame_B_length;


                    Main_Wall_B.transform.localScale = new Vector3(side_B_value, wall_y_height,   wall_width);

                    float width_of_B_x = Mathf.Abs(Vector3.Dot(FrameB.transform.InverseTransformDirection(Pergola_Model.transform.right), FrameB_bounds.size));

                    float width_of_B_y = Mathf.Abs(Vector3.Dot(FrameB.transform.InverseTransformDirection(Pergola_Model.transform.up), FrameB_bounds.size));

                    float width_of_B_z = Mathf.Abs(Vector3.Dot(FrameB.transform.InverseTransformDirection(Pergola_Model.transform.forward), FrameB_bounds.size));


                    //Main_Wall_B.transform.Translate(-Pergola_Model.transform.right * right_offset,Space.World);
                    if (GameObject.Find("FrameB_0"))
                    {
                        right_offset = DB_script.frame_B_length / 2 - FrameB.transform.localScale.y / 2;
                        Main_Wall_B.transform.Translate(Pergola_Model.transform.forward * (right_offset / 2), Space.World);
                    }


                    //Main_Wall_B.transform.Translate(Pergola_Model.transform.right * (right_offset/2), Space.World);
                    Main_Wall_B.transform.Translate(-Pergola_Model.transform.right * (width_of_B_x / 2 + wall_width / 2 + L_width), Space.World);
                    Main_Wall_B.transform.Translate(-Pergola_Model.transform.up * width_of_B_y / 2, Space.World);
                    Main_Wall_B.transform.Translate(Pergola_Model.transform.up * wall_y_height / 2, Space.World);
                    Main_Wall_B.transform.Translate(Pergola_Model.transform.forward * width_of_B_x / 2, Space.World);
                    Main_Wall_B.transform.parent = Wall_Parent.transform;
                }
                //Wall C
                else if (side_name == "C")// && GameObject.Find("Main_Wall_C") == null)
                {

                    string wall_name = "Main_Wall_C";

                    if (GameObject.Find(wall_name) != null)
                    {
                        DestroyImmediate(GameObject.Find(wall_name));
                    }
                    //C.isOn = true;
                    GameObject Main_Wall_C = Instantiate(Wall);

                    Main_Wall_C.name = wall_name;

                    Main_Wall_C.transform.parent = Pergola_Model.transform;
                    Main_Wall_C.transform.rotation = Pergola_Model.transform.rotation;


                    Main_Wall_C.transform.localPosition = new Vector3(0, 0, 0);

                    //Main_Wall_C.transform.Rotate(0, 90, 0);

                    //var side_C = Generate_Model.formPergola_global.pergola_measurement.FirstOrDefault(m => m.side_name == "C");
                    float side_C_value = 0;
                    side_C_value = DB_script.frame_C_length;
                    //if (side_C != null) side_C_value = (float)(side_C.side_value);

                    Vector3 FrameC_global_center = FrameC.transform.TransformPoint(FrameC_bounds.center);

                    float fwd_offset = DB_script.frame_C_length;



                    Main_Wall_C.transform.localScale = new Vector3(side_C_value, wall_y_height, wall_width);
                    Vector3 pos_wall_A = new Vector3(FrameC_bounds.center.y, wall_y_height / 2, side_C_value / 2);

                    Main_Wall_C.transform.position = FrameC.transform.TransformPoint(FrameC_bounds.center);


                    float width_of_C_z = Mathf.Abs(Vector3.Dot(FrameC.transform.InverseTransformDirection(Pergola_Model.transform.forward), FrameC_bounds.size));

                    float width_of_C_y = Mathf.Abs(Vector3.Dot(FrameC.transform.InverseTransformDirection(Pergola_Model.transform.up), FrameC_bounds.size));
                    if (GameObject.Find("FrameC_0"))
                    {
                        fwd_offset = DB_script.frame_C_length / 2 - FrameC.transform.localScale.y / 2;
                        Main_Wall_C.transform.Translate(-Pergola_Model.transform.right * fwd_offset, Space.World);

                    }
                    Main_Wall_C.transform.Translate(Pergola_Model.transform.up * wall_y_height / 2, Space.World);
                    Main_Wall_C.transform.Translate(-Pergola_Model.transform.up * width_of_C_y / 2, Space.World);
                    Main_Wall_C.transform.Translate(Pergola_Model.transform.forward * (wall_width / 2 + width_of_C_z / 2 + L_width), Space.World);
                    Main_Wall_C.transform.parent = Wall_Parent.transform;
                }

                else if (side_name == "E")// && GameObject.Find("Main_Wall_C") == null)
                {

                    string wall_name = "Main_Wall_E";

                    if (GameObject.Find(wall_name) != null)
                    {
                        DestroyImmediate(GameObject.Find(wall_name));
                    }
                    //C.isOn = true;
                    GameObject Main_Wall_E = Instantiate(Wall);

                    Main_Wall_E.name = wall_name;

                    Main_Wall_E.transform.parent = Pergola_Model.transform;
                    Main_Wall_E.transform.rotation = Pergola_Model.transform.rotation;


                    Main_Wall_E.transform.localPosition = new Vector3(0, 0, 0);

                    //Main_Wall_C.transform.Rotate(0, 90, 0);

                    //var side_C = Generate_Model.formPergola_global.pergola_measurement.FirstOrDefault(m => m.side_name == "C");
                    float side_E_value = 0;
                    side_E_value = DB_script.frame_E_length;
                    //if (side_C != null) side_C_value = (float)(side_C.side_value);

                    Vector3 FrameE_global_center = FrameE.transform.TransformPoint(FrameE_bounds.center);

                    float fwd_offset = DB_script.frame_E_length;



                    Main_Wall_E.transform.localScale = new Vector3(side_E_value, wall_y_height, wall_width);
                    Vector3 pos_wall_A = new Vector3(FrameE_bounds.center.y, wall_y_height / 2, side_E_value / 2);

                    Main_Wall_E.transform.position = FrameE.transform.TransformPoint(FrameE_bounds.center);


                    float width_of_E_z = Mathf.Abs(Vector3.Dot(FrameE.transform.InverseTransformDirection(Pergola_Model.transform.forward), FrameE_bounds.size));

                    float width_of_E_y = Mathf.Abs(Vector3.Dot(FrameE.transform.InverseTransformDirection(Pergola_Model.transform.up), FrameE_bounds.size));
                    if (GameObject.Find("FrameE_0"))
                    {
                        fwd_offset = DB_script.frame_E_length / 2 - FrameE.transform.localScale.y / 2;
                        Main_Wall_E.transform.Translate(-Pergola_Model.transform.right * fwd_offset, Space.World);

                    }
                    Main_Wall_E.transform.Translate(Pergola_Model.transform.up * wall_y_height / 2, Space.World);
                    Main_Wall_E.transform.Translate(-Pergola_Model.transform.up * width_of_E_y / 2, Space.World);
                    Main_Wall_E.transform.Translate(-Pergola_Model.transform.forward * (wall_width / 2 + width_of_E_z / 2 + L_width), Space.World);
                    Main_Wall_E.transform.parent = Wall_Parent.transform;
                }

                //Wall_B
                else if (side_name == "F")// && GameObject.Find("Main_Wall_B") == null)
                {
                    string wall_name = "Main_Wall_F";

                    if (GameObject.Find(wall_name) != null)
                    {
                        DestroyImmediate(GameObject.Find(wall_name));
                    }
                    //B.isOn = true;
                    GameObject Main_Wall_F = Instantiate(Wall);


                    Main_Wall_F.name = wall_name;

                    ////Main_Wall.transform.position = FrameC.transform.TransformPoint(pos_wall_C);
                    //var side_B = Generate_Model.formPergola_global.pergola_measurement.FirstOrDefault(m => m.side_name == "B");
                    float side_F_value = 0;
                    side_F_value = DB_script.frame_F_length;
                    //if (side_B != null) side_B_value = (float)(side_B.side_value);


                    Main_Wall_F.transform.parent = Pergola_Model.transform;

                    Main_Wall_F.transform.rotation = Pergola_Model.transform.rotation;

                    Main_Wall_F.transform.localPosition = new Vector3(0, 0, 0);
                    Main_Wall_F.transform.Rotate(0, 90, 0, Space.Self);

                    Main_Wall_F.transform.position = FrameF.transform.TransformPoint(FrameF_bounds.center);
                    float right_offset = DB_script.frame_F_length;


                    Main_Wall_F.transform.localScale = new Vector3(side_F_value, wall_y_height, wall_width);

                    float width_of_B_x = Mathf.Abs(Vector3.Dot(FrameF.transform.InverseTransformDirection(Pergola_Model.transform.right), FrameF_bounds.size));

                    float width_of_B_y = Mathf.Abs(Vector3.Dot(FrameF.transform.InverseTransformDirection(Pergola_Model.transform.up), FrameF_bounds.size));

                    float width_of_B_z = Mathf.Abs(Vector3.Dot(FrameF.transform.InverseTransformDirection(Pergola_Model.transform.forward), FrameF_bounds.size));


                    //Main_Wall_B.transform.Translate(-Pergola_Model.transform.right * right_offset,Space.World);
                    if (GameObject.Find("FrameF_0"))
                    {
                        right_offset = DB_script.frame_F_length / 2 - FrameF.transform.localScale.y / 2;
                        Main_Wall_F.transform.Translate(Pergola_Model.transform.forward * (right_offset / 2), Space.World);
                    }


                    //Main_Wall_B.transform.Translate(Pergola_Model.transform.right * (right_offset/2), Space.World);
                    Main_Wall_F.transform.Translate(-Pergola_Model.transform.right * (width_of_B_x / 2 + wall_width / 2 + L_width), Space.World);
                    Main_Wall_F.transform.Translate(-Pergola_Model.transform.up * width_of_B_y / 2, Space.World);
                    Main_Wall_F.transform.Translate(Pergola_Model.transform.up * wall_y_height / 2, Space.World);
                    //Main_Wall_F.transform.Translate(Pergola_Model.transform.forward * width_of_B_x / 2, Space.World);
                    Main_Wall_F.transform.parent = Wall_Parent.transform;
                }


            }
            }
    }
}
