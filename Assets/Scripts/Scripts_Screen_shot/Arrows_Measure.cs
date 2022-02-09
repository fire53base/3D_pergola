using RESTfulHTTPServer.src.invoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Arrows_Measure : MonoBehaviour
{

    public static GameObject Arrow_Parent, Arrow_prefab;

    public static SortedDictionary<String, string> field_arrow_names_dist;

    public enum Views { _TOP, _FRONT, _FIELD, _SIDE_FIELD, _B_B, _SIDE_FIELD_SECTION_1, _SIDE_FIELD_SECTION_2, _SIDE_FIELD_SECTION_3, _FIELD_SECTION_1, _FIELD_SECTION_2, _FIELD_SECTION_3 }
    DB_script db_script;

    public static GameObject sphere_;

    Dotted_line_custom.Dotted_line_custom dotted_Line_Custom_script;

    public enum Parents_name { SupportBars_Parent, Frames_Parent, Field_Parent, Divider_Parent, Arrow_Parent, FieldDividers_Parent, FrameDividers_Parent }

    public static GameObject Arrow_Side_Destroy_field_width;

    public static SortedList<float, Transform> dividers_section1_z_sort;

    public enum rafaffa_Placement_string { type_2, type_3 };
    // Start is called before the first frame update
    void Start()
    {
        Arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange");

        db_script = GameObject.Find("Directional Light").GetComponent<DB_script>();

        dotted_Line_Custom_script = GameObject.Find("Controller").GetComponent<Dotted_line_custom.Dotted_line_custom>();

        sphere_ = (GameObject)Resources.Load("Prefabs/Spheres/Sphere");
    }

    static string dummy_text = "%*%#^%&";

    public void Refresh()
    {
        Arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange");

        db_script = GameObject.Find("Directional Light").GetComponent<DB_script>();

        dotted_Line_Custom_script = GameObject.Find("Controller").GetComponent<Dotted_line_custom.Dotted_line_custom>();

        sphere_ = (GameObject)Resources.Load("Prefabs/Spheres/Sphere");

    }

    public void Take_Screen_Shot()
    {
        Invoke("to_invoke", 0.2f);
    }

    public void to_invoke()
    {
        //To DO
        if (DB_script.I_type)
            arrs(Views._TOP.ToString());
        else
            arrs(Views._TOP.ToString());
    }
    public async void orientation_sc_shot()
    {
        if (GameObject.Find("Pergola_Model") != null)
        {
            GameObject Pergola_Model = GameObject.Find("Pergola_Model");


            rotateonmouse rotateonmouse_script = Pergola_Model.GetComponent<rotateonmouse>();
            for (int i = 0; i < 4; i++)
            {
                string view = "";
                switch (i)
                {
                    case 0:
                        //rotateonmouse_script.rotate_TOP();
                        view = Views._TOP.ToString();
                        await UnityMainThreadDispatcher.DispatchAsync(() => arrs(view));

                        break;
                    case 3:

                        view = Views._FRONT.ToString();

                        await UnityMainThreadDispatcher.DispatchAsync(() => arrs(view));
                        break;


                    case 1:

                        view = Views._SIDE_FIELD.ToString();
                        await UnityMainThreadDispatcher.DispatchAsync(() => arrs(view));
                        break;

                    case 2:

                        view = Views._FIELD.ToString();
                        await UnityMainThreadDispatcher.DispatchAsync(() => arrs(view));
                        break;


                }
                //if(i>0)
                //{
                //    Reset_Script.Reset_Model();
                //}


            }
        }

    }
    public static float scale_model_factor = 1;
    public async void arrs(string view = "_Front")
    {
        try
        {


            if (GameObject.Find("Frames_Parent") != null)
                DB_script.Frames_Parent = GameObject.Find("Frames_Parent");

            if (GameObject.Find("Pergola_Model") != null)
                DB_script.Pergola_Model = GameObject.Find("Pergola_Model");

            if (GameObject.Find("Divider_Parent") != null)
                DB_script.Divider_Parent = GameObject.Find("Divider_Parent");


            if (GameObject.Find("FrameDividers_Parent") != null)
                DB_script.FrameDividers_Parent = GameObject.Find("FrameDividers_Parent");

            if (GameObject.Find("FieldDividers_Parent") != null)
                DB_script.FieldDividers_Parent = GameObject.Find("FieldDividers_Parent");

            if (GameObject.Find("Field_Parent") != null)
                DB_script.Field_Parent = GameObject.Find("Field_Parent");

            if (GameObject.Find("SupportBars_Parent") != null)
                DB_script.SupportBars_Parent = GameObject.Find("SupportBars_Parent");

            float assembly_tolerance = DB_script.assembly_tolerance;

            float L_width = DB_script.L_accessory_thickess;// 1.2f;

            float U_width = DB_script.U_accessory_thickess;// 2f;
            //right=Pergola_Model.transform.forward ,up=Pergola_Model.transform.up,forward=Pergola_Model.transform.right


            if (DB_script.I_type)
            {
                if (GameObject.Find("Pergola_Model") != null)
                {
                    GameObject Pergola_Model = GameObject.Find("Pergola_Model");

                    GameObject frame_A;
                    if (GameObject.Find("FrameA_0"))
                    {
                        frame_A = GameObject.Find("FrameA_0");
                    }
                    else
                    {

                        frame_A = GameObject.Find("FrameA");
                    }

                    GameObject frame_B;

                    if (GameObject.Find("FrameB_0"))
                    {
                        frame_B = GameObject.Find("FrameB_0");
                    }
                    else
                    {
                        frame_B = GameObject.Find("FrameB");
                    }


                    GameObject frame_C;
                    if (GameObject.Find("FrameC_0"))
                    {
                        frame_C = GameObject.Find("FrameC_0");
                    }
                    else
                    {
                        frame_C = GameObject.Find("FrameC");
                    }
                    GameObject frame_D;

                    if (GameObject.Find("FrameD_0"))
                    {
                        frame_D = GameObject.Find("FrameD_0");
                    }
                    else
                    {
                        frame_D = GameObject.Find("FrameD");
                    }

                    Bounds bound_a = frame_A.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    Bounds bound_b = frame_B.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    Bounds bound_c = frame_C.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    Bounds bound_d = frame_D.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    //view = "_Top";
                    rotateonmouse rotateonmouse_script = Pergola_Model.GetComponent<rotateonmouse>();

                    //directions from Pergola_Model perspective

                    //GameObject FrameDividers_Parent_Section_0001 = DB_script.FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0001").gameObject;

                    //GameObject FieldDividers_Parent_Section_0001 = DB_script.FieldDividers_Parent.transform.Find("FieldDividers_Parent_Section_0001").gameObject;

                    Vector3 req_right = Pergola_Model.transform.forward, req_up = -Pergola_Model.transform.up, req_fwd = Pergola_Model.transform.right;

                    if (view == Views._TOP.ToString())
                    {
                        field_arrow_names_dist = new SortedDictionary<string, string>();
                        GameObject Field_divider_Parent = DB_script.FieldDividers_Parent;
                        GameObject FrameDivider_Parent = DB_script.FrameDividers_Parent;


                        Arrows_Measure.dividers_section1_z_sort = new SortedList<float, Transform>();

                        foreach (Transform field_div in Field_divider_Parent.transform)
                        {

                            Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(field_div.GetChild(0).transform.position);

                            if (!Arrows_Measure.dividers_section1_z_sort.ContainsValue(field_div.GetChild(0))&&!Arrows_Measure.dividers_section1_z_sort.ContainsKey(f_div_pos_wrt_Perg_Mod.z))
                                Arrows_Measure.dividers_section1_z_sort.Add(f_div_pos_wrt_Perg_Mod.z, field_div.GetChild(0));

                            //GameObject Arrow_Green = null;

                            //GameObject field_R_frameDivider = field_div.GetChild(0).gameObject;

                            //Bounds horizontal_field_bound = field_R_frameDivider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                            //float max_lenth_of_field_R_frameDivider = Mathf.Abs(Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z));

                            ////max_lenth_of_field_R_frameDivider += 400;//to bring the arrows forward extra offset of 400

                            //float min_distance_2 = 0; //Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z);

                            //Vector3 arrow_direction_wrt_frameParent_dir = -Pergola_Model.transform.forward;

                            //Vector3 arrow_for_end_field_divider_diretion = -arrow_direction_wrt_frameParent_dir;
                            //float center2edge_dist = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized), horizontal_field_bound.size)) / 2;
                            //Vector3 arrow_pos = field_R_frameDivider.transform.TransformPoint(horizontal_field_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);


                            //float frame_depth = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_b.size));

                            //frame_depth += 200;//to bring the arrows up extra offset of 200
                            //Vector3 prefab_Tmp_text_scale = Arrow_prefab.transform.GetChild(1).GetComponent<TextMeshPro>().rectTransform.localScale * 2;//ACCESSING LOCALSCALE OF TEXT

                            //RaycastHit hit1;



                            //if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                            //{
                            //    Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);



                            //    arrow_parm ar_field_div = new arrow_parm()
                            //    {
                            //        position_of_arrow = arrow_pos,

                            //        direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                            //        length_of_arrow = hit1.distance,

                            //        arrow_name = "Arrow_Gap_" + field_R_frameDivider.name,

                            //        pergola_right = -max_lenth_of_field_R_frameDivider * 1 / 4
                            //    };

                            //    //Updating the list to access while taking front screen shot as distance is varied
                            //    if (!field_arrow_names_dist.ContainsKey(ar_field_div.arrow_name))
                            //    {
                            //        field_arrow_names_dist.Add(ar_field_div.arrow_name, ((float)Math.Floor(hit1.distance)).ToString());
                            //    }


                            //    arrows_function(ar_field_div);

                            //}

                            ////To get the last arrow of field divider we raycst in opposite direction

                            //if (field_div.GetSiblingIndex() == Field_divider_Parent.transform.childCount - 1)
                            //{
                            //    //assigning opposite direction to arrow

                            //    arrow_direction_wrt_frameParent_dir = Pergola_Model.transform.forward;


                            //    arrow_pos = field_R_frameDivider.transform.TransformPoint(horizontal_field_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);

                            //    if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                            //    {
                            //        Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);




                            //        arrow_parm ar_field_div = new arrow_parm()
                            //        {
                            //            position_of_arrow = arrow_pos,

                            //            direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                            //            length_of_arrow = hit1.distance,

                            //            arrow_name = "Arrow_Gap_2" + field_R_frameDivider.name,

                            //            pergola_right = -max_lenth_of_field_R_frameDivider * 1 / 4,




                            //        };

                            //        //Updating the list to access while taking front screen shot as distance is varied
                            //        if (!field_arrow_names_dist.ContainsKey(ar_field_div.arrow_name))
                            //        {
                            //            field_arrow_names_dist.Add(ar_field_div.arrow_name, ((float)Math.Floor(hit1.distance)).ToString());
                            //        }

                            //        arrows_function(ar_field_div);

                            //    }

                            //}
                        }

                        foreach (Transform frame_div in FrameDivider_Parent.transform)
                        {

                            Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(frame_div.GetChild(0).transform.position);

                            if (!Arrows_Measure.dividers_section1_z_sort.ContainsValue(frame_div.GetChild(0))&&!Arrows_Measure.dividers_section1_z_sort.ContainsKey(f_div_pos_wrt_Perg_Mod.z))
                                Arrows_Measure.dividers_section1_z_sort.Add(f_div_pos_wrt_Perg_Mod.z, frame_div.GetChild(0));
                            //GameObject Arrow_Green = null;

                            //GameObject field_R_frameDivider = frame_div.GetChild(0).gameObject;

                            //Bounds horizontal_field_bound = field_R_frameDivider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
                            ////Arrow_Parent.transform.position = 
                            //float max_lenth_of_field_R_frameDivider = Mathf.Abs(Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z));

                            ////max_lenth_of_field_R_frameDivider += 400;//to bring the arrows forward extra offset of 400

                            //float min_distance_2 = 0; //Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z);

                            //Vector3 arrow_direction_wrt_frameParent_dir = -Pergola_Model.transform.forward;

                            //Vector3 arrow_for_end_field_divider_diretion = -arrow_direction_wrt_frameParent_dir;
                            //float center2edge_dist = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized), horizontal_field_bound.size)) / 2;
                            //Vector3 arrow_pos = field_R_frameDivider.transform.TransformPoint(horizontal_field_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);


                            //float frame_depth = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_b.size));

                            //frame_depth += 200;//to bring the arrows up extra offset of 200
                            //Vector3 prefab_Tmp_text_scale = Arrow_prefab.transform.GetChild(1).GetComponent<TextMeshPro>().rectTransform.localScale * 2;//ACCESSING LOCALSCALE OF TEXT

                            //RaycastHit hit1;



                            //if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                            //{
                            //    Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);


                            //    arrow_parm ar_frame_div = new arrow_parm()
                            //    {
                            //        position_of_arrow = arrow_pos,

                            //        direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                            //        length_of_arrow = hit1.distance,

                            //        arrow_name = "Arrow_Gap_" + field_R_frameDivider.name,

                            //        pergola_right = -max_lenth_of_field_R_frameDivider * 1 / 4

                            //    };


                            //    //Updating the list to access while taking front screen shot as distance is varied
                            //    if (!field_arrow_names_dist.ContainsKey(ar_frame_div.arrow_name))
                            //    {
                            //        field_arrow_names_dist.Add(ar_frame_div.arrow_name, ((float)Math.Floor(hit1.distance)).ToString());
                            //    }
                            //    arrows_function(ar_frame_div);

                            //}
                        }

                        foreach (Transform field_R_div in Arrows_Measure.dividers_section1_z_sort.Values)
                        {
                            GameObject Arrow_Green = null;

                            GameObject field_R_frameDivider = field_R_div.gameObject;

                            Bounds horizontal_field_bound = field_R_frameDivider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                            float max_lenth_of_field_R_frameDivider = Mathf.Abs(Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z));

                            //max_lenth_of_field_R_frameDivider += 400;//to bring the arrows forward extra offset of 400

                            float min_distance_2 = 0; //Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z);

                            Vector3 arrow_direction_wrt_frameParent_dir = -Pergola_Model.transform.forward;

                            Vector3 arrow_for_end_field_divider_diretion = -arrow_direction_wrt_frameParent_dir;
                            float center2edge_dist = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized), horizontal_field_bound.size)) / 2;
                            Vector3 arrow_pos = field_R_frameDivider.transform.TransformPoint(horizontal_field_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);


                            float frame_depth = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_b.size));

                            frame_depth += 200;//to bring the arrows up extra offset of 200
                            Vector3 prefab_Tmp_text_scale = Arrow_prefab.transform.GetChild(1).GetComponent<TextMeshPro>().rectTransform.localScale * 2;//ACCESSING LOCALSCALE OF TEXT

                            RaycastHit hit1;



                            if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                            {
                                Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);



                                arrow_parm ar_field_div = new arrow_parm()
                                {
                                    position_of_arrow = arrow_pos,

                                    direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                                    length_of_arrow = hit1.distance,

                                    arrow_name = "Arrow_Gap_" + field_R_frameDivider.name,

                                    pergola_right = -max_lenth_of_field_R_frameDivider * 1 / 4
                                };

                                //Updating the list to access while taking front screen shot as distance is varied
                                if (!field_arrow_names_dist.ContainsKey(ar_field_div.arrow_name))
                                {
                                    field_arrow_names_dist.Add(ar_field_div.arrow_name, ((float)Math.Floor(hit1.distance)).ToString());
                                }


                                arrows_function(ar_field_div);

                            }

                            //To get the last arrow of field divider we raycst in opposite direction

                            if (Arrows_Measure.dividers_section1_z_sort.IndexOfValue(field_R_div) == Arrows_Measure.dividers_section1_z_sort.Count - 1)
                            {
                                //assigning opposite direction to arrow

                                arrow_direction_wrt_frameParent_dir = Pergola_Model.transform.forward;


                                arrow_pos = field_R_frameDivider.transform.TransformPoint(horizontal_field_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);

                                if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                                {
                                    Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);




                                    arrow_parm ar_field_div = new arrow_parm()
                                    {
                                        position_of_arrow = arrow_pos,

                                        direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                                        length_of_arrow = hit1.distance,

                                        arrow_name = "Arrow_Gap_2" + field_R_frameDivider.name,

                                        pergola_right = -max_lenth_of_field_R_frameDivider * 1 / 4,




                                    };

                                    //Updating the list to access while taking front screen shot as distance is varied
                                    if (!field_arrow_names_dist.ContainsKey(ar_field_div.arrow_name))
                                    {
                                        field_arrow_names_dist.Add(ar_field_div.arrow_name, ((float)Math.Floor(hit1.distance)).ToString());
                                    }

                                    arrows_function(ar_field_div);

                                }

                            }
                        }

                        float pergola_forward_offset = -DB_script.frame_C_length / 2;
                        #region Arrow for Frame_C 

                        Vector3 dotted_line_frame_C_dir = Pergola_Model.transform.right;

                        float right_offset_frm_C = Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_c.size)) + 300;

                        float fwd_offset = frame_C.transform.localScale.y / 2;
                        if (GameObject.Find("FrameC_0"))
                        {
                            fwd_offset = DB_script.frame_C_length - frame_C.transform.localScale.y / 2;

                        }

                        arrow_parm arrow_Frame_C_params = new arrow_parm()
                        {
                            position_of_arrow = frame_C.transform.TransformPoint(bound_c.center),
                            direction_of_arrow = (Pergola_Model.transform.forward),
                            length_of_arrow = DB_script.frame_C_length,
                            arrow_name = "Arrow_" + frame_C.name,



                            pergola_fwd = -fwd_offset,// pergola_forward_offset,


                            pergola_right = right_offset_frm_C,//Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_c.size)) + 300,

                            pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_c.size))+300

                            dotted_line_offset = right_offset_frm_C,

                            Dotted_line_Direction = Pergola_Model.transform.right,



                        };

                        //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                        arrows_function(arrow_Frame_C_params);


                        #endregion


                        float pergola_rigth_offset = DB_script.frame_B_length / 2;
                        #region Arrow for Frame_B

                        Vector3 dotted_line_frame_B_dir = Pergola_Model.transform.forward;

                        float fwd_offset_frm_B = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_b.size)) + 300;
                        arrow_parm arrow_Frame_B_params = new arrow_parm()
                        {
                            position_of_arrow = frame_B.transform.TransformPoint(bound_b.center),
                            direction_of_arrow = -(Pergola_Model.transform.right),
                            length_of_arrow = DB_script.frame_B_length,
                            arrow_name = "Arrow_" + frame_B.name,



                            pergola_fwd = fwd_offset_frm_B,


                            pergola_right = frame_B.transform.localScale.y / 2,//Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) + 100,

                            pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_B.size))+300


                            Dotted_line_Direction = Pergola_Model.transform.forward,

                            dotted_line_offset = fwd_offset_frm_B,

                            orientation_dotted_lines = "horizontal"
                        };

                        //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                        arrows_function(arrow_Frame_B_params);

                        #endregion


                        #region Support_bar_Arrow
                        if (DB_script.wall_on_side_C == true)
                        {
                             Transform supp_bar = DB_script.SupportBars_Parent.transform.GetChild(DB_script.SupportBars_Parent.transform.childCount-1);//.gameObject;
                                GameObject supportClamp0=null;// = GameObject.Find("Clamp_onFrame_1");

                                foreach(Transform sup_ch in supp_bar)
                                {
                                    if(sup_ch.name.Contains("Clamp_onFrame_"))
                                    {
                                        supportClamp0 = sup_ch.gameObject;
                                    }
                                }
                            // string last_clamp_name = "Clamp_onFrame_" + DB_script.SupportBars_Parent.transform.childCount;

                            if (supportClamp0 != null)
                            {
                                // GameObject supportClamp0 = GameObject.Find(last_clamp_name);

                                Bounds support_clamp_bound = supportClamp0.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                //Arrow_Parent.transform.position = 
                                float max_lenth_of_field_R_frameDivider1 = Mathf.Abs(Mathf.Max(supportClamp0.transform.localScale.x, supportClamp0.transform.localScale.y, supportClamp0.transform.localScale.z));

                                max_lenth_of_field_R_frameDivider1 += 400;//to bring the arrows forward extra offset of 400

                                float min_distance_1 = 0; //Mathf.Max(field_R_frameDivider1.transform.localScale.x, field_R_frameDivider1.transform.localScale.y, field_R_frameDivider1.transform.localScale.z);

                                Vector3 arrow_direction_wrt_frameParent_dir1 = Pergola_Model.transform.right;

                                Vector3 arrow_for_end_field_divider_diretion_1 = -arrow_direction_wrt_frameParent_dir1;
                                float center2edge_dist_1 = Mathf.Abs(Vector3.Dot(supportClamp0.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir1.normalized), support_clamp_bound.size)) / 2;

                                float clamp_depth = Mathf.Abs(Vector3.Dot(supportClamp0.transform.InverseTransformDirection(Pergola_Model.transform.up), support_clamp_bound.size));

                                float frame_depth_1 = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_b.size));

                                Vector3 arrow_pos_1 = supportClamp0.transform.TransformPoint(support_clamp_bound.center + supportClamp0.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir1.normalized) * center2edge_dist_1 + supportClamp0.transform.InverseTransformDirection(-Pergola_Model.transform.up) * clamp_depth / 2);
                                //frame_depth_1 += 200;//to bring the arrows up extra offset of 200
                                //Vector3 prefab_Tmp_text_scale_1 = Arrow_Orange_Prefab.transform.GetChild(1).GetComponent<TextMeshPro>().rectTransform.localScale * 2;//ACCESSING LOCALSCALE OF TEXT


                                float frameA_width = Mathf.Abs(Vector3.Dot(frame_A.transform.InverseTransformDirection(Pergola_Model.transform.right).normalized, bound_a.size));///2;

                                float dist_ = Vector3.Dot(Pergola_Model.transform.forward, supportClamp0.transform.position) - Vector3.Dot(Pergola_Model.transform.forward, frame_D.transform.TransformPoint(bound_d.center));

                                float offset_ext=frameA_width / 2 + 300;

                                float support_arrow_right_off_dist =Mathf.Abs( dist_)+ offset_ext;



                                RaycastHit hitA;

                                float len_arrow = 0;
                                if (Physics.Raycast(arrow_pos_1, arrow_direction_wrt_frameParent_dir1, out hitA, Mathf.Infinity, 1 << DB_script.frame_layer.value))
                                {
                                    //adding frame A width as ray hits box colloider
                                    len_arrow = hitA.distance + frameA_width;

                                }
                                arrow_parm ar_supp_bar = new arrow_parm()
                                {
                                    position_of_arrow = arrow_pos_1,
                                    direction_of_arrow = arrow_direction_wrt_frameParent_dir1,
                                    length_of_arrow = len_arrow,
                                    arrow_name = "Arrow_" + supportClamp0.name,



                                    pergola_fwd = -support_arrow_right_off_dist,


                                    //pergola_right = frame_B.transform.localScale.y / 2,//Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) + 100,

                                    pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_B.size))+300


                                    Dotted_line_Direction = Pergola_Model.transform.forward,

                                    dotted_line_offset = -offset_ext,//support_arrow_right_off_dist,

                                    orientation_dotted_lines = "horizontal"

                                };

                                arrows_function(ar_supp_bar);
                            }
                        }
                        else
                        {

                           // if (GameObject.Find("Clamp_onFrame_1") != null)
                           if(DB_script.SupportBars_Parent.transform.childCount>0)
                            {
                                Transform supp_bar = DB_script.SupportBars_Parent.transform.GetChild(0);//.gameObject;
                                GameObject supportClamp0=null;// = GameObject.Find("Clamp_onFrame_1");

                                foreach(Transform sup_ch in supp_bar)
                                {
                                    if(sup_ch.name.Contains("Clamp_onFrame_"))
                                    {
                                        supportClamp0 = sup_ch.gameObject;
                                    }
                                }
                                if (supportClamp0 != null)
                                {
                                    Bounds support_clamp_bound = supportClamp0.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                    //Arrow_Parent.transform.position = 
                                    float max_lenth_of_field_R_frameDivider1 = Mathf.Abs(Mathf.Max(supportClamp0.transform.localScale.x, supportClamp0.transform.localScale.y, supportClamp0.transform.localScale.z));

                                    max_lenth_of_field_R_frameDivider1 += 400;//to bring the arrows forward extra offset of 400

                                    float min_distance_1 = 0; //Mathf.Max(field_R_frameDivider1.transform.localScale.x, field_R_frameDivider1.transform.localScale.y, field_R_frameDivider1.transform.localScale.z);

                                    Vector3 arrow_direction_wrt_frameParent_dir1 = Pergola_Model.transform.right;

                                    Vector3 arrow_for_end_field_divider_diretion_1 = -arrow_direction_wrt_frameParent_dir1;
                                    float center2edge_dist_1 = Mathf.Abs(Vector3.Dot(supportClamp0.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir1.normalized), support_clamp_bound.size)) / 2;

                                    float clamp_depth = Mathf.Abs(Vector3.Dot(supportClamp0.transform.InverseTransformDirection(Pergola_Model.transform.up), support_clamp_bound.size));

                                    float frame_depth_1 = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_b.size));

                                    // Vector3 arrow_pos_1 = supportClamp0.transform.TransformPoint(support_clamp_bound.center + supportClamp0.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir1.normalized) * center2edge_dist_1 + supportClamp0.transform.InverseTransformDirection(-Pergola_Model.transform.up) * clamp_depth / 2);
                                    //frame_depth_1 += 200;//to bring the arrows up extra offset of 200
                                    //Vector3 prefab_Tmp_text_scale_1 = Arrow_Orange_Prefab.transform.GetChild(1).GetComponent<TextMeshPro>().rectTransform.localScale * 2;//ACCESSING LOCALSCALE OF TEXT
                                    //Vector3 arrow_pos_1 = supportClamp0.transform.TransformPoint(support_clamp_bound.center + supportClamp0.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir1.normalized) * center2edge_dist_1 + supportClamp0.transform.InverseTransformDirection(-Pergola_Model.transform.up) *(clamp_depth / 2+frame_depth_1/2));
                                    Vector3 arrow_pos_1 = supportClamp0.transform.TransformPoint(support_clamp_bound.center + supportClamp0.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir1.normalized) * center2edge_dist_1 + supportClamp0.transform.InverseTransformDirection(-Pergola_Model.transform.up) * clamp_depth / 2);
                                    float frameA_width = Mathf.Abs(Vector3.Dot(frame_A.transform.InverseTransformDirection(Pergola_Model.transform.right).normalized, bound_a.size));///2;

                                    float dist_ = Vector3.Dot( Pergola_Model.transform.forward, supportClamp0.transform.position) - Vector3.Dot( Pergola_Model.transform.forward, frame_D.transform.TransformPoint(bound_d.center));


                                                    float offset_ext=frameA_width / 2 + 300;
                                    float support_arrow_right_off_dist = Mathf.Abs(dist_) + offset_ext;



                                    RaycastHit hitA;

                                    float len_arrow = 0;
                                    Debug.DrawRay(arrow_pos_1, arrow_direction_wrt_frameParent_dir1 * 10000, Color.cyan, 150);

                                    if (Physics.Raycast(arrow_pos_1, arrow_direction_wrt_frameParent_dir1, out hitA, Mathf.Infinity, 1 << DB_script.frame_layer.value))
                                    {
                                        //adding frame A width as ray hits box colloider
                                        len_arrow = hitA.distance + frameA_width;

                                    }
                                    arrow_parm ar_supp_bar = new arrow_parm()
                                    {
                                        position_of_arrow = arrow_pos_1,
                                        direction_of_arrow = arrow_direction_wrt_frameParent_dir1,
                                        length_of_arrow = len_arrow,
                                        arrow_name = "Arrow_" + supportClamp0.name,



                                        pergola_fwd = -support_arrow_right_off_dist,


                                        //pergola_right = frame_B.transform.localScale.y / 2,//Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) + 100,

                                        pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_B.size))+300


                                        Dotted_line_Direction = Pergola_Model.transform.forward,

                                        dotted_line_offset = -offset_ext,//support_arrow_right_off_dist,

                                        orientation_dotted_lines = "horizontal"

                                    };

                                    arrows_function(ar_supp_bar);
                                }
                            }
                        }
                        #endregion

                        //if (DB_script.Field_Parent != null)
                        //    DB_script.Field_Parent.SetActive(false);

                        //if (DB_script.SupportBars_Parent != null)
                        //    DB_script.SupportBars_Parent.SetActive(false);


                        rotateonmouse_script.rotate_TOP();

                    }
                    else
                    {
                        GameObject Wall_Parent = GameObject.Find("Wall_Parent");
                        if (Wall_Parent != null)
                            Wall_Parent.SetActive(false);
                    }

                    if (view == Views._FRONT.ToString())
                    {
                        float scale_factor = 1;//1
                        scale_factor = (int)((DB_script.frame_A_length * Mathf.Pow(10, -3)));


                        if (scale_factor < 1)
                        {
                            scale_factor = 1;
                        }
                        else if (scale_factor > 5)
                        {
                            scale_factor = 5;
                        }
                      


                        //Task t = new Task(() =>
                        //{

                        GameObject Field_divider_Parent = GameObject.Find("FieldDividers_Parent");
                        GameObject FrameDividers_Parent = GameObject.Find("FrameDividers_Parent");
                        float gap_f_div = 300;
                        float frame_depth = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_b.size)) * scale_factor;

                        Arrows_Measure.dividers_section1_z_sort = new SortedList<float, Transform>();
                        foreach (Transform field_div in Field_divider_Parent.transform)
                        {

                            Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(field_div.GetChild(0).transform.position);

                            if (!Arrows_Measure.dividers_section1_z_sort.ContainsValue(field_div.GetChild(0))&&!Arrows_Measure.dividers_section1_z_sort.ContainsKey(f_div_pos_wrt_Perg_Mod.z))
                                Arrows_Measure.dividers_section1_z_sort.Add(f_div_pos_wrt_Perg_Mod.z, field_div.GetChild(0));
                            //GameObject Arrow_Green = null;

                            //GameObject field_R_frameDivider = field_div.GetChild(0).gameObject;

                            //Bounds field_div_bound = field_R_frameDivider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                            //float max_lenth_of_field_R_frameDivider = Mathf.Abs(Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z));

                            //max_lenth_of_field_R_frameDivider += 400;//to bring the arrows forward extra offset of 400

                            //float min_distance_2 = 0; //Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z);

                            //Vector3 arrow_direction_wrt_frameParent_dir = -Pergola_Model.transform.forward;

                            //Vector3 arrow_for_end_field_divider_diretion = -arrow_direction_wrt_frameParent_dir;

                            //Vector3 ls_b = frame_B.transform.localScale;


                            //Vector3 actual_Frame_B_size = new Vector3(ls_b.x * bound_b.size.x, ls_b.y * bound_b.size.y, ls_b.z * bound_b.size.z);

                            //Vector3 actual_size = new Vector3(field_div_bound.size.x * field_R_frameDivider.transform.localScale.x, field_div_bound.size.y * field_R_frameDivider.transform.localScale.y, field_div_bound.size.z * field_R_frameDivider.transform.localScale.z);

                            //float center2edge_dist = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized), field_div_bound.size)) / 2;
                            //Vector3 arrow_pos = field_R_frameDivider.transform.TransformPoint(field_div_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);



                            //float f_div_depth_half = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(Pergola_Model.transform.up), field_div_bound.size)) / 2 * scale_factor;



                            //float offset_up = frame_depth - f_div_depth_half + gap_f_div;// + gap_f_div;

                            //float arrow_off = frame_depth - 2 * f_div_depth_half + gap_f_div;
                            ////frame_depth += gap_field_div;//to bring the arrows up extra offset of 200
                            //Vector3 prefab_Tmp_text_scale = Arrow_prefab.transform.GetChild(1).GetComponent<TextMeshPro>().rectTransform.localScale * 2;//ACCESSING LOCALSCALE OF TEXT

                            //RaycastHit hit1;



                            //if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                            //{
                            //    Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);

                            //    string arr_name = "Arrow_Gap_" + field_R_frameDivider.name;


                            //    float txt_display = 0;

                            //    //Taking values from Dictionary
                            //    if (field_arrow_names_dist != null)
                            //        if (field_arrow_names_dist.ContainsKey(arr_name))
                            //        {
                            //            txt_display = float.Parse(field_arrow_names_dist[arr_name]);
                            //        }

                            //    if (txt_display <= 0)
                            //    {
                            //        txt_display = hit1.distance;
                            //    }


                            //    arrow_parm ar_field_div = new arrow_parm()
                            //    {
                            //        position_of_arrow = arrow_pos,

                            //        direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                            //        length_of_arrow = hit1.distance,

                            //        arrow_name = "Arrow_Gap_" + field_div.name,

                            //        pergola_up = offset_up,

                            //        //Dotted Line params

                            //        Dotted_line_Direction = Pergola_Model.transform.up,

                            //        dotted_line_offset = arrow_off,

                            //        Dotted_facing_direction = Pergola_Model.transform.right,

                            //        orientation_dotted_lines = "horizontal",

                            //        txt = txt_display.ToString(),


                            //    };

                            //    arrows_function(ar_field_div);

                            //}

                            ////To get the last arrow of field divider we raycst in opposite direction

                            //if (field_div.GetSiblingIndex() == Field_divider_Parent.transform.childCount - 1)
                            //{
                            //    //assigning opposite direction to arrow

                            //    arrow_direction_wrt_frameParent_dir = Pergola_Model.transform.forward;


                            //    arrow_pos = field_R_frameDivider.transform.TransformPoint(field_div_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);

                            //    if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                            //    {
                            //        Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);


                            //        string arr_name = "Arrow_Gap_2" + field_R_frameDivider.name;

                            //        float txt_display = 0;
                            //        //Taking values from Dictionary
                            //        if (field_arrow_names_dist != null)
                            //            if (field_arrow_names_dist.ContainsKey(arr_name))
                            //            {
                            //                txt_display = float.Parse(field_arrow_names_dist[arr_name]);
                            //            }

                            //        if (txt_display <= 0)
                            //        {
                            //            txt_display = hit1.distance;
                            //        }

                            //        if(field_arrow_names_dist!=null)
                            //        txt_display = float.Parse(field_arrow_names_dist.Last().Value); //todo fix here

                            //        arrow_parm ar_field_div = new arrow_parm()
                            //        {
                            //            position_of_arrow = arrow_pos,

                            //            direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                            //            length_of_arrow = hit1.distance,

                            //            arrow_name = arr_name,

                            //            pergola_up = offset_up,

                            //            //Dotted Line params

                            //            Dotted_line_Direction = Pergola_Model.transform.up,

                            //            dotted_line_offset = arrow_off,

                            //            Dotted_facing_direction = Pergola_Model.transform.right,

                            //            orientation_dotted_lines = "horizontal",

                            //            txt = txt_display.ToString(),

                            //        };

                            //        arrows_function(ar_field_div);

                            //    }
                            //    else
                            //    {
                            //        Debug.DrawRay(arrow_pos, arrow_direction_wrt_frameParent_dir * 2000, Color.green, 15);
                            //    }

                            //}
                        }

                        foreach (Transform frame_div in FrameDividers_Parent.transform)
                        {


                            Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(frame_div.GetChild(0).transform.position);

                            if (!Arrows_Measure.dividers_section1_z_sort.ContainsValue(frame_div.GetChild(0))&&!Arrows_Measure.dividers_section1_z_sort.ContainsKey(f_div_pos_wrt_Perg_Mod.z))
                                Arrows_Measure.dividers_section1_z_sort.Add(f_div_pos_wrt_Perg_Mod.z, frame_div.GetChild(0));
                            //GameObject Arrow_Green = null;

                            //GameObject field_R_frameDivider = frame_div.GetChild(0).gameObject;

                            //Bounds frm_div_bound = field_R_frameDivider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
                            ////Arrow_Parent.transform.position = 
                            //float max_lenth_of_field_R_frameDivider = Mathf.Abs(Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z));

                            //max_lenth_of_field_R_frameDivider += 400;//to bring the arrows forward extra offset of 400

                            //float min_distance_2 = 0; //Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z);

                            //Vector3 arrow_direction_wrt_frameParent_dir = -Pergola_Model.transform.forward;

                            //Vector3 arrow_for_end_field_divider_diretion = -arrow_direction_wrt_frameParent_dir;

                            //Vector3 ls_b = frame_B.transform.localScale;

                            //Vector3 actual_Frame_B_size = new Vector3(ls_b.x * bound_b.size.x, ls_b.y * bound_b.size.y, ls_b.z * bound_b.size.z);

                            //Vector3 actual_size = new Vector3(frm_div_bound.size.x * field_R_frameDivider.transform.localScale.x, frm_div_bound.size.y * field_R_frameDivider.transform.localScale.y, frm_div_bound.size.z * field_R_frameDivider.transform.localScale.z);

                            //float center2edge_dist = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized), frm_div_bound.size)) / 2;
                            //Vector3 arrow_pos = field_R_frameDivider.transform.TransformPoint(frm_div_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);




                            //float f_div_depth_half = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(Pergola_Model.transform.up), frm_div_bound.size)) / 2 * scale_factor;
                            ////float gap_1 = 200;
                            ////frame_depth += gap_1;//to bring the arrows up extra offset of 200
                            //float offset_up_frm_div = frame_depth - f_div_depth_half + gap_f_div;

                            //float arrow_off = frame_depth - 2 * f_div_depth_half + gap_f_div;
                            //Vector3 prefab_Tmp_text_scale = Arrow_prefab.transform.GetChild(1).GetComponent<TextMeshPro>().rectTransform.localScale * 2;//ACCESSING LOCALSCALE OF TEXT

                            //RaycastHit hit1;



                            //if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                            //{
                            //    Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);

                            //    string arr_name = "Arrow_Gap_" + field_R_frameDivider.name;

                            //    float txt_display = 0;

                            //    //Taking values from Dictionary
                            //    if (field_arrow_names_dist != null)
                            //        if (field_arrow_names_dist.ContainsKey(arr_name))
                            //        {
                            //            txt_display = float.Parse(field_arrow_names_dist[arr_name]);
                            //        }

                            //    if (txt_display <= 0)
                            //    {
                            //        txt_display = hit1.distance;
                            //    }


                            //    arrow_parm ar_frame_div = new arrow_parm()
                            //    {
                            //        position_of_arrow = arrow_pos,

                            //        direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                            //        length_of_arrow = hit1.distance,

                            //        arrow_name = "Arrow_Gap_" + field_R_frameDivider.name,

                            //        pergola_up = offset_up_frm_div,

                            //        //Dotted Line params

                            //        Dotted_line_Direction = Pergola_Model.transform.up,

                            //        dotted_line_offset = arrow_off,

                            //        Dotted_facing_direction = Pergola_Model.transform.right,

                            //        orientation_dotted_lines = "horizontal",

                            //        txt = txt_display.ToString(),

                            //    };

                            //    arrows_function(ar_frame_div);

                            //}
                            //else
                            //{
                            //    Debug.DrawRay(arrow_pos, arrow_direction_wrt_frameParent_dir * 2000, Color.green, 15);
                            //}

                            ////To get the last arrow of field divider we raycst in opposite direction

                            //if (frame_div.GetSiblingIndex() == FrameDividers_Parent.transform.childCount - 1)
                            //{
                            //    //assigning opposite direction to arrow

                            //    arrow_direction_wrt_frameParent_dir = Pergola_Model.transform.forward;


                            //    arrow_pos = field_R_frameDivider.transform.TransformPoint(frm_div_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);

                            //    if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                            //    {
                            //        Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);


                            //        string arr_name = "Arrow_Gap_2" + field_R_frameDivider.name;

                            //        float txt_display = 0;
                            //        //Taking values from Dictionary
                            //        if (field_arrow_names_dist != null)
                            //            if (field_arrow_names_dist.ContainsKey(arr_name))
                            //            {
                            //                txt_display = float.Parse(field_arrow_names_dist[arr_name]);
                            //            }

                            //        if (txt_display <= 0)
                            //        {
                            //            txt_display = hit1.distance;
                            //        }

                            //        if(field_arrow_names_dist!=null)
                            //        txt_display = float.Parse(field_arrow_names_dist.Last().Value); //todo fix here

                            //        arrow_parm ar_frame_div = new arrow_parm()
                            //        {
                            //            position_of_arrow = arrow_pos,

                            //            direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                            //            length_of_arrow = hit1.distance,

                            //            arrow_name = arr_name,

                            //            pergola_up = offset_up_frm_div,

                            //            //Dotted Line params

                            //            Dotted_line_Direction = Pergola_Model.transform.up,

                            //            dotted_line_offset = arrow_off,

                            //            Dotted_facing_direction = Pergola_Model.transform.right,

                            //            orientation_dotted_lines = "horizontal",

                            //            txt = txt_display.ToString(),

                            //        };

                            //        arrows_function(ar_frame_div);

                            //    }
                            //    else
                            //    {
                            //        Debug.DrawRay(arrow_pos, arrow_direction_wrt_frameParent_dir * 2000, Color.green, 15);
                            //    }

                            //}

                        }

                          try
                        {
                            //await UnityMainThreadDispatcher.DispatchAsync(() => Scale_fields_Accs_Frame(scale_factor));
                            scale_model_factor = scale_factor;
                            Scale_fields_Accs_Frame(scale_factor);
                            //Invoke("Scale_fields_Accs_Frame", 0f);
                            await Task.Delay(300);//100ms delay
                                                  //Giving Delay after scaling for the gameobjects to appaear with actual scale in hierarchy
                                                  //await Task.Delay(300);//100ms delay
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("While scaling frames, field div and Accessories :" + ex);
                        }


                        foreach (Transform field_R_div in Arrows_Measure.dividers_section1_z_sort.Values)
                        {
                            GameObject Arrow_Green = null;

                            GameObject field_R_frameDivider = field_R_div.gameObject;

                            Bounds field_div_bound = field_R_frameDivider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                            float max_lenth_of_field_R_frameDivider = Mathf.Abs(Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z));

                            max_lenth_of_field_R_frameDivider += 400;//to bring the arrows forward extra offset of 400

                            float min_distance_2 = 0; //Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z);

                            Vector3 arrow_direction_wrt_frameParent_dir = -Pergola_Model.transform.forward;

                            Vector3 arrow_for_end_field_divider_diretion = -arrow_direction_wrt_frameParent_dir;

                            Vector3 ls_b = frame_B.transform.localScale;


                            Vector3 actual_Frame_B_size = new Vector3(ls_b.x * bound_b.size.x, ls_b.y * bound_b.size.y, ls_b.z * bound_b.size.z);

                            Vector3 actual_size = new Vector3(field_div_bound.size.x * field_R_frameDivider.transform.localScale.x, field_div_bound.size.y * field_R_frameDivider.transform.localScale.y, field_div_bound.size.z * field_R_frameDivider.transform.localScale.z);

                            float center2edge_dist = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized), field_div_bound.size)) / 2;
                            Vector3 arrow_pos = field_R_frameDivider.transform.TransformPoint(field_div_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);



                            float f_div_depth_half = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(Pergola_Model.transform.up), field_div_bound.size)) / 2 * scale_factor;



                            float offset_up = frame_depth - f_div_depth_half + gap_f_div;// + gap_f_div;

                            float arrow_off = frame_depth - 2 * f_div_depth_half + gap_f_div;
                            //frame_depth += gap_field_div;//to bring the arrows up extra offset of 200
                            Vector3 prefab_Tmp_text_scale = Arrow_prefab.transform.GetChild(1).GetComponent<TextMeshPro>().rectTransform.localScale * 2;//ACCESSING LOCALSCALE OF TEXT

                            RaycastHit hit1;



                            if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                            {
                                Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);

                                string arr_name = "Arrow_Gap_" + field_R_frameDivider.name;


                                float txt_display = 0;

                                //Taking values from Dictionary
                                if (field_arrow_names_dist != null)
                                    if (field_arrow_names_dist.ContainsKey(arr_name))
                                    {
                                        txt_display = float.Parse(field_arrow_names_dist[arr_name]);
                                    }

                                if (txt_display <= 0)
                                {
                                    txt_display = hit1.distance;
                                }


                                arrow_parm ar_field_div = new arrow_parm()
                                {
                                    position_of_arrow = arrow_pos,

                                    direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                                    length_of_arrow = hit1.distance,

                                    arrow_name = "Arrow_Gap_" + field_R_div.name,

                                    pergola_up = offset_up,

                                    //Dotted Line params

                                    Dotted_line_Direction = Pergola_Model.transform.up,

                                    dotted_line_offset = arrow_off,

                                    Dotted_facing_direction = Pergola_Model.transform.right,

                                    orientation_dotted_lines = "horizontal",

                                    txt = txt_display.ToString(),


                                };

                                arrows_function(ar_field_div);

                            }

                            //To get the last arrow of field divider we raycst in opposite direction

                            if (Arrows_Measure.dividers_section1_z_sort.IndexOfValue(field_R_div) == Arrows_Measure.dividers_section1_z_sort.Count - 1)
                            {
                                //assigning opposite direction to arrow

                                arrow_direction_wrt_frameParent_dir = Pergola_Model.transform.forward;


                                arrow_pos = field_R_frameDivider.transform.TransformPoint(field_div_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);

                                if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                                {
                                    Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);


                                    string arr_name = "Arrow_Gap_2" + field_R_frameDivider.name;

                                    float txt_display = 0;
                                    //Taking values from Dictionary
                                    if (field_arrow_names_dist != null)
                                        if (field_arrow_names_dist.ContainsKey(arr_name))
                                        {
                                            txt_display = float.Parse(field_arrow_names_dist[arr_name]);
                                        }

                                    if (txt_display <= 0)
                                    {
                                        txt_display = hit1.distance;
                                    }

                                    if (field_arrow_names_dist != null)
                                        if (field_arrow_names_dist.Count>0)
                                            txt_display = float.Parse(field_arrow_names_dist.Last().Value); //todo fix here

                                    arrow_parm ar_field_div = new arrow_parm()
                                    {
                                        position_of_arrow = arrow_pos,

                                        direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                                        length_of_arrow = hit1.distance,

                                        arrow_name = arr_name,

                                        pergola_up = offset_up,

                                        //Dotted Line params

                                        Dotted_line_Direction = Pergola_Model.transform.up,

                                        dotted_line_offset = arrow_off,

                                        Dotted_facing_direction = Pergola_Model.transform.right,

                                        orientation_dotted_lines = "horizontal",

                                        txt = txt_display.ToString(),

                                    };

                                    arrows_function(ar_field_div);

                                }
                                else
                                {
                                    Debug.DrawRay(arrow_pos, arrow_direction_wrt_frameParent_dir * 2000, Color.green, 15);
                                }

                            }
                        }

                        float gap = 300;
                        float pergola_up_dir_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_c.size)) * scale_factor / 2 + gap);

                        #region Arrow for Frame_C 

                        //float len_arr = frame_C.transform.localScale.y * bound_b.size.y;
                        //arrow_parm arrow_Frame_C_params = new arrow_parm()
                        //{
                        //    position_of_arrow = frame_C.transform.TransformPoint(bound_c.center),
                        //    direction_of_arrow = (Pergola_Model.transform.forward),
                        //    length_of_arrow = len_arr,
                        //    arrow_name = "Arrow_" + frame_C.name,



                        //    pergola_fwd = -len_arr/ 2,


                        //    //pergola_right = Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_c.size*5)) + 300,

                        //    pergola_up =pergola_up_dir_offset,

                        //    //dotted Lines params 
                        //    Dotted_line_Direction=Pergola_Model.transform.up,

                        //    dotted_line_offset=pergola_up_dir_offset,

                        //    Dotted_facing_direction=Pergola_Model.transform.right,


                        //    orientation_dotted_lines="horizontal",




                        //};

                        ////await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                        //arrows_function(arrow_Frame_C_params);

                        #endregion

                        Vector3 pos_of_Raycast = frame_D.transform.TransformPoint(bound_d.center + frame_D.transform.InverseTransformDirection(Pergola_Model.transform.forward) * Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_d.size)) / 2);

                        RaycastHit hit_frame_D;

                        float actual_width_of_frame_B = Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_b.size)) * scale_factor;

                        if (Physics.Raycast(pos_of_Raycast, Pergola_Model.transform.forward, out hit_frame_D, Mathf.Infinity, 1 << DB_script.frame_layer.value))
                        {

                            float length_arrow = hit_frame_D.distance + 2 * actual_width_of_frame_B;
                            arrow_parm ar_field_div = new arrow_parm()
                            {
                                position_of_arrow = pos_of_Raycast,

                                direction_of_arrow = Pergola_Model.transform.forward,

                                length_of_arrow = length_arrow,

                                arrow_name = "Arrow_" + frame_C.name,

                                pergola_up = pergola_up_dir_offset,

                                pergola_fwd = -actual_width_of_frame_B,

                                txt = DB_script.frame_C_length.ToString(),

                                //    //dotted Lines params 
                                Dotted_line_Direction = Pergola_Model.transform.up,

                                dotted_line_offset = -gap,

                                Dotted_facing_direction = Pergola_Model.transform.right,


                                orientation_dotted_lines = "horizontal",
                            };

                            arrows_function(ar_field_div);
                        }
                        else
                        {
                            Debug.DrawRay(pos_of_Raycast, Pergola_Model.transform.forward * Mathf.Infinity, Color.black, 10);
                        }


                        //Screen_Shot sc_sh = Camera.main.GetComponent<Screen_Shot>();

                        //sc_sh.take_screenShot();
                        Debug.Log("(2) Raycasting and Arrows");
                        //});
                        //t.RunSynchronously();
                        //t.Wait();

                        //Giving error
                        rotateonmouse_script.rotate_FRONT();



                    }
                    if (view == Views._SIDE_FIELD.ToString())
                    {
                        await Task.Delay(100);//100ms delay

                        #region Taking L accessory height
                        //if (GameObject.Find("Accessories_1/L_Accessory_bottomLeft_1") != null)
                        //{
                        //    GameObject L_Accessory_1 = GameObject.Find("Accessories_1/L_Accessory_bottomLeft_1");

                        //    Bounds bound_L_Accs_1 = L_Accessory_1.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                        //    GameObject arr_pfb = null;

                        //    if((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text")!=null)
                        //    {
                        //        arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                        //    }

                        //    arrow_parm arrow_L_Accs = new arrow_parm()
                        //    {

                        //        position_of_arrow = L_Accessory_1.transform.TransformPoint(bound_L_Accs_1.center),


                        //        direction_of_arrow = -Pergola_Model.transform.right,

                        //        length_of_arrow = L_Accessory_1.transform.localScale.y,

                        //        arrow_name = "Arrow_" + L_Accessory_1.name,

                        //        pergola_fwd = 0,

                        //        pergola_right = L_Accessory_1.transform.localScale.y / 2,

                        //        pergola_up = Mathf.Abs(Vector3.Dot(L_Accessory_1.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_L_Accs_1.size)) + 300,

                        //        arrow_prefab=arr_pfb ,

                        //    };
                        #endregion

                        //if (GameObject.Find("Accessories_1/L_Accessory_bottomLeft_1") != null)
                        //{
                        GameObject FrameD = frame_D;

                        Bounds bound_L_Accs_1 = FrameD.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                        GameObject arr_pfb = null;

                        //Changing prefab as we need space left of side screen shot
                        if ((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_space_side") != null)
                        {
                            //arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                            arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_space_side");
                        }

                        float width_of_frame = Mathf.Abs(Vector3.Dot(frame_A.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_a.size));

                        //as we are using length of frame we must subtract it 2 times by width 
                        float len_arrow = FrameD.transform.localScale.y - 2 * width_of_frame;

                        // length of accessory is -15 of the divider lengths
                        len_arrow = len_arrow - assembly_tolerance;


                        float up_dir_offset_L_acc_arrow = Mathf.Abs(Vector3.Dot(FrameD.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_d.size)) + 300;

                        arrow_parm arrow_L_Accs = new arrow_parm()
                        {

                            position_of_arrow = FrameD.transform.TransformPoint(bound_L_Accs_1.center),


                            direction_of_arrow = -Pergola_Model.transform.right,

                            length_of_arrow = len_arrow,

                            arrow_name = "Arrow_" + FrameD.name,

                            pergola_fwd = 0,

                            pergola_right = len_arrow / 2,

                            pergola_up = up_dir_offset_L_acc_arrow,

                            arrow_prefab = arr_pfb,

                            Dotted_line_Direction = Pergola_Model.transform.up,

                            dotted_line_offset = up_dir_offset_L_acc_arrow,

                            orientation_dotted_lines = "horizontal",

                            Dotted_facing_direction = Pergola_Model.transform.forward


                        };
                        arrows_function(arrow_L_Accs);

                        //}

                        if ((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange") != null)
                        {
                            //Changing back to previous prefab
                            //arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                            arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange");
                        }
                        bool L_Accs = false, U_Accs = false;

                       

                        float horizontal_width = 0;
                        if (GameObject.Find("Fields_1/Field_1") != null)
                        {
                            GameObject h_field;

                            float offest_for_child_ak40 = 0;
                            bool ak_40x40 = false;
                            if (GameObject.Find("Fields_1/Field_1").transform.GetChild(0).name.Contains("ak - 40"))
                            {


                                h_field = GameObject.Find("Fields_1/Field_1").transform.GetChild(0).gameObject;
                                ak_40x40 = true;

                            }
                            else
                            {
                                h_field = GameObject.Find("Field_1");
                            }
                            //taking mesh from child
                            Bounds bound_h_field = h_field.GetComponentInChildren<MeshFilter>().mesh.bounds;

                            //scale from child
                            horizontal_width = h_field.transform.localScale.x;


                            float field_space = horizontal_width;

                            float fwd_offset = 0;
                            // if (ak_40x40 == true)
                            // {
                            //     fwd_offset = field_space;
                            // }
                            // else
                            // {
                                fwd_offset = field_space / 2;
                            // }


                            float offset_right_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                            arrow_parm arr_space_params = new arrow_parm()
                            {

                                position_of_arrow = h_field.transform.TransformPoint(bound_h_field.center),

                                direction_of_arrow = -req_right,

                                length_of_arrow = field_space,

                                arrow_name = "Arrow_Space_Invisible_text" + h_field.name,

                                pergola_right = offset_right_field_gap,

                                pergola_up = 0,

                                pergola_fwd = fwd_offset,//field_space / 2,

                                Dotted_line_Direction = Pergola_Model.transform.right,

                                dotted_line_offset = offset_right_field_gap,
                                //arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_Invisible_Text"),

                                arrow_prefab = arr_pfb,
                            };

                            Arrow_Side_Destroy_field_width = arrows_function(arr_space_params);

                        }
                        rotateonmouse_script.rotate_SIDE();
                        //rotateonmouse_script.rotate_Raffafa_SIDE();
                    }
                    if (view == Views._FIELD.ToString())
                    {
                      
                        await Task.Delay(500);//100ms delay


                        bool L_Accs = false, U_Accs = false;

                        //float L_width = 1.2f;

                        //float U_width = 2f;

                        float horizontal_width = 0;
                        if (GameObject.Find("Fields_1/Field_1") != null)
                        {
                            GameObject h_field;

                            float offest_for_child_ak40 = 0;
                            bool ak_40x40 = false;
                            if (GameObject.Find("Fields_1/Field_1").transform.GetChild(0).name.Contains("ak - 40"))
                            {
                                h_field = GameObject.Find("Fields_1/Field_1").transform.GetChild(0).gameObject;
                                ak_40x40 = true;

                            }
                            else
                            {
                                h_field = GameObject.Find("Field_1");
                            }
                            //taking mesh from child
                            Bounds bound_h_field = h_field.GetComponentInChildren<MeshFilter>().mesh.bounds;

                            //scale from child
                            horizontal_width = h_field.transform.localScale.x;


                            float field_space = horizontal_width;

                            //If L accessory is there add the width of L accessory
                            if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                            {
                                field_space = field_space + (2 * L_width);

                            }

                            if (GameObject.Find("U_Accessory_Left_3") != null)
                            {
                                field_space = field_space + (2 * U_width);
                            }


                            float fwd_offset = 0;
                            // if (ak_40x40 == true)
                            // {
                            //     fwd_offset = field_space;
                            // }
                            // else
                            // {
                                fwd_offset = field_space / 2;
                            // }

                            float offset_right_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                            arrow_parm arr_space_params = new arrow_parm()
                            {

                                position_of_arrow = h_field.transform.TransformPoint(bound_h_field.center),

                                direction_of_arrow = -req_right,

                                length_of_arrow = field_space,

                                arrow_name = "Arrow_Space_" + h_field.name,

                                pergola_right = offset_right_field_gap,

                                pergola_up = 0,

                                pergola_fwd = fwd_offset,

                                Dotted_line_Direction = Pergola_Model.transform.right,

                                dotted_line_offset = offset_right_field_gap,


                            };

                            arrows_function(arr_space_params);

                        }

                        //we need 2 single Arrows here

                        if (GameObject.Find("Fields_1") != null)
                        {
                            int single_Sect_field_count = GameObject.Find("Fields_1").transform.childCount;

                            int middle_field_no = single_Sect_field_count / 2;

                            string horizontal_field_prefix = "Field_";

                            string middle_field_name = horizontal_field_prefix + middle_field_no;

                            string next_field_name = horizontal_field_prefix + (middle_field_no + 1).ToString();


                            if (GameObject.Find($"Fields_1/{middle_field_name}") != null)
                            {
                                GameObject middle_field;

                                if (GameObject.Find($"Fields_1/{middle_field_name}").transform.GetChild(0).name.Contains("ak - 40"))
                                {
                                    //middle_field = GameObject.Find("Fields_1").transform.GetChild(0).GetChild(0).gameObject;
                                    middle_field = GameObject.Find($"Fields_1/{middle_field_name}").transform.GetChild(0).gameObject;
                                }
                                else
                                {
                                    middle_field = GameObject.Find($"Fields_1/{middle_field_name}");
                                }

                                Bounds bound_middle_field = middle_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                Vector3 global_center_of_middle_field =middle_field.transform.TransformPoint(bound_middle_field.center);


                                Vector3 global_pos_of_arrow = global_center_of_middle_field - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_middle_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_D.transform.TransformPoint(bound_d.center)));

                                float length_of_arrow = Mathf.Max(frame_D.transform.localScale.x, frame_D.transform.localScale.y, frame_D.transform.localScale.z) / 10;

                                if (length_of_arrow < 150)
                                {
                                    length_of_arrow = 151;
                                }

                                arrow_parm arr_mid_field = new arrow_parm()
                                {
                                    position_of_arrow = global_pos_of_arrow,

                                    length_of_arrow = length_of_arrow,

                                    direction_of_arrow = Pergola_Model.transform.right,

                                    arrow_name = "Arrow_" + middle_field_name,

                                    arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                    pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_d.size)) + 300),

                                    pergola_right = -Mathf.Abs(Vector3.Dot(middle_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_middle_field.size)) / 2,

                                    txt = DB_script.raffaf_spacing_pergola.ToString(),

                                    txt_right = -length_of_arrow * 3 / 4,

                                    Dotted_line_Direction = Pergola_Model.transform.forward,

                                    dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_d.size)) + 300),

                                    orientation_dotted_lines = "horizontal"

                                };

                                arrows_function(arr_mid_field);
                            }


                            if (GameObject.Find($"Fields_1/{next_field_name}") != null)
                            {
                                GameObject next_field;

                                if (GameObject.Find($"Fields_1/{next_field_name}").transform.GetChild(0).name.Contains("ak - 40"))
                                {
                                    //next_field = GameObject.Find("Fields_1").transform.GetChild(0).GetChild(0).gameObject;
                                    next_field = GameObject.Find($"Fields_1/{next_field_name}").transform.GetChild(0).gameObject;
                                }
                                else
                                {
                                    next_field = GameObject.Find($"Fields_1/{next_field_name}");
                                }

                                Bounds bound_next_field = next_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                Vector3 global_center_of_next_field = next_field.transform.TransformPoint(bound_next_field.center);


                                Vector3 global_pos_of_arrow = global_center_of_next_field - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_next_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_D.transform.TransformPoint(bound_d.center)));

                                float length_of_arrow = Mathf.Max(frame_D.transform.localScale.x, frame_D.transform.localScale.y, frame_D.transform.localScale.z) / 10;

                                if (length_of_arrow < 110)
                                {
                                    length_of_arrow = 111;
                                }
                                arrow_parm arr_next_field = new arrow_parm()
                                {
                                    position_of_arrow = global_pos_of_arrow,

                                    length_of_arrow = length_of_arrow,

                                    direction_of_arrow = -Pergola_Model.transform.right,

                                    arrow_name = "Arrow_" + next_field_name,

                                    arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                    pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_d.size)) + 300),

                                    pergola_right = Mathf.Abs(Vector3.Dot(next_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_next_field.size)) / 2,

                                    //***********Dummy text***********//

                                    txt = dummy_text,//

                                    Dotted_line_Direction = Pergola_Model.transform.forward,

                                    dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_d.size)) + 300),

                                    orientation_dotted_lines = "horizontal"

                                };

                                arrows_function(arr_next_field);
                            }
                        }


                        //we need 2 single head arrows for Gap from the ground

                        if (GameObject.Find("Fields_1/Field_1") != null)
                        {
                            GameObject h_field;

                            float offest_for_child_ak40 = 0;
                            bool ak_40x40 = false;
                            if (GameObject.Find("Fields_1/Field_1").transform.GetChild(0).name.Contains("ak - 40"))
                            {
                                h_field = GameObject.Find("Fields_1/Field_1").transform.GetChild(0).gameObject;
                                ak_40x40 = true;

                            }
                            else
                            {
                                h_field = GameObject.Find("Field_1");
                            }
                            //taking mesh from child
                            Bounds bound_h_field = h_field.GetComponentInChildren<MeshFilter>().mesh.bounds;

                            //scale from child
                            horizontal_width = h_field.transform.localScale.x;


                            float field_space = horizontal_width;

                            //If L accessory is there add the width of L accessory
                            if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                            {
                                field_space = field_space + (2 * L_width);

                            }

                            if (GameObject.Find("U_Accessory_Left_3") != null)
                            {
                                field_space = field_space + (2 * U_width);
                            }


                            float fwd_offset = 0;
                            // if (ak_40x40 == true)
                            // {
                            //     fwd_offset = field_space;
                            // }
                            // else
                            // {
                                fwd_offset = field_space / 2;
                            // }

                            Bounds bound_middle_field = h_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                            Vector3 global_center_of_middle_field = h_field.transform.TransformPoint(bound_middle_field.center);


                            Vector3 global_pos_of_arrow = global_center_of_middle_field - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_middle_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_D.transform.TransformPoint(bound_d.center)));


                            float length_of_arrow = Mathf.Max(frame_D.transform.localScale.x, frame_D.transform.localScale.y, frame_D.transform.localScale.z) / 10;
                            float offset_right_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size));// + 300;

                            if (length_of_arrow < 150)
                            {
                                length_of_arrow = 151;
                            }
                            Vector3 front_point, back_point;

                            //we are taking front and back points 

                            front_point = h_field.transform.TransformPoint(bound_h_field.center + h_field.transform.InverseTransformDirection(Pergola_Model.transform.up) * Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_h_field.center)));

                            back_point = h_field.transform.TransformPoint(bound_h_field.center + h_field.transform.InverseTransformDirection(-Pergola_Model.transform.up) * Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_h_field.center))); ;// global_center_of_middle_field - Pergola_Model.transform.up * (Vector3.Dot(Pergola_Model.transform.up, global_center_of_middle_field));
                            float distance_from_floor = 0;
                            RaycastHit hit_accs_front, hit_accs_back;

                            Debug.DrawRay(front_point, Pergola_Model.transform.forward * 10000, Color.red, 150);
                            Debug.DrawRay(back_point, Pergola_Model.transform.forward * 10000, Color.green, 150);
                                RaycastHit hit_accs;
                            bool obj_hit=false;

                            //Here we Raycast Pergola_Model.transform.forward and Find Which Accessory that is hit
                            if (Physics.Raycast(front_point, Pergola_Model.transform.forward, out hit_accs_front, Mathf.Infinity) )
                            {
                                obj_hit = true;
                            }
                             if( Physics.Raycast(back_point, Pergola_Model.transform.forward, out hit_accs_back, Mathf.Infinity))
                            {
                                obj_hit = true;
                            }
                            if(obj_hit)
                            { 
                               if( hit_accs_front.transform.parent.name.Contains("Accessory"))
                                {
                                    hit_accs = hit_accs_front;
                                }
                               else
                                {
                                    hit_accs = hit_accs_back;
                                }
                               if(hit_accs.transform!=null)
                                if (hit_accs.transform.parent != null )
                                {
                                    if (hit_accs.transform.parent.name.Contains("Accessory"))
                                    {
                                        Debug.Log("Hit object" + hit_accs.transform.parent.name);

                                        Bounds bound_hit_accs = hit_accs.transform.parent.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                        Vector3 tip_of_accs = hit_accs.transform.TransformPoint(bound_hit_accs.center + hit_accs.transform.InverseTransformDirection(Pergola_Model.transform.right) * Mathf.Abs(Vector3.Dot(hit_accs.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_hit_accs.center)));// global_center_of_middle_field - Pergola_Model.transform.up * (Vector3.Dot(Pergola_Model.transform.up, global_center_of_middle_field));

                                        Vector3 tip_of_acc_wrt_Pergola_Model = Pergola_Model.transform.InverseTransformPoint(tip_of_accs);

                                        Vector3 down_point = h_field.transform.TransformPoint(bound_h_field.center + h_field.transform.InverseTransformDirection(Pergola_Model.transform.right) * Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.center)));

                                        Debug.DrawRay(tip_of_accs, Pergola_Model.transform.forward * 10000, Color.blue, 150);
                                        Debug.DrawRay(down_point, Pergola_Model.transform.forward * 10000, Color.cyan, 150);
                                        Vector3 arrow_1_pos_wrt_Pergola_Model = Pergola_Model.transform.InverseTransformPoint(down_point);
                                        distance_from_floor = Vector3.Dot(Pergola_Model.transform.right, tip_of_accs) - Vector3.Dot(Pergola_Model.transform.right, down_point);
                                        Debug.Log("Distance  from ground =" + (tip_of_acc_wrt_Pergola_Model.x - arrow_1_pos_wrt_Pergola_Model.x + " DOT PROD DIST =" + distance_from_floor));
                                    }
                                }

                            }
                            //if arrow length is less
                            if (length_of_arrow / 2 <= 150)

                            {
                                length_of_arrow = 2 * 150;
                            }
                            arrow_parm arr_space_1_params = new arrow_parm()
                            {

                                position_of_arrow = global_pos_of_arrow,//GameObject.Find("Field_1").transform.TransformPoint(bound_h_field.center),

                                direction_of_arrow = -Pergola_Model.transform.right,

                                length_of_arrow = length_of_arrow/2,

                                arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                arrow_name = "Arrow_Space1_height" + h_field.name,

                                pergola_right = offset_right_field_gap/2,

                                txt=Mathf.FloorToInt( distance_from_floor).ToString(),
                                pergola_up = 0,

                                //300
                                pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_d.size)) + 300),//fwd_offset-Mathf.Abs(h_field.transform.localScale.x)-300,

                                Dotted_line_Direction = Pergola_Model.transform.forward,

                                dotted_line_offset = -(2*Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_d.size)) + 300),//offset_right_field_gap,

                                orientation_dotted_lines="horizontal",

                            };

                                GameObject arrow_1=   arrows_function(arr_space_1_params);

                        

                            arrow_parm arr_space_2_params = new arrow_parm()
                            {

                                position_of_arrow = global_pos_of_arrow,//GameObject.Find("Field_1").transform.TransformPoint(bound_h_field.center),

                                direction_of_arrow = Pergola_Model.transform.right,

                                length_of_arrow = length_of_arrow / 2,

                                arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                arrow_name = "Arrow_Space2_height" + h_field.name,

                                pergola_right = offset_right_field_gap / 2+Mathf.Abs( distance_from_floor),

                                pergola_up = 0,
                                //300
                                pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_d.size)) + 300),//fwd_offset-Mathf.Abs(h_field.transform.localScale.x)-300,

                                //***********Dummy text***********//

                                txt = dummy_text,

                                Dotted_line_Direction = Pergola_Model.transform.forward,

                                dotted_line_offset = -(2 * Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_d.size)) + 300),//offset_right_field_gap,

                                orientation_dotted_lines = "horizontal",

                            };

                            arrows_function(arr_space_2_params);

                        }

                        //rotateonmouse_script.rotate_Raffafa_TOP();

                        rotateonmouse_script.rotate_TOP();
                    }

                    //Arranging and rotating text in each arrows
                    if (Arrow_Parent != null)
                        foreach (Transform Arrow in Arrow_Parent.transform)
                        {
                            Arrow.GetChild(1).forward = Camera.main.transform.forward;

                            if (Arrow.name.Contains("Arrow_Field_"))
                            {
                                Arrow.transform.GetChild(1).transform.GetComponent<RectTransform>().localRotation = Quaternion.Euler(90, 90, 0);// (90, 180, 90);//(40, 90, 0)
                            }

                            if (Arrow.name.Contains("FrameB"))
                            {
                                Arrow.transform.GetChild(1).transform.GetComponent<RectTransform>().localRotation = Quaternion.Euler(90, 0, 90);
                            }

                            if (Arrow.name.Contains("Arrow_Gap_") && view == Views._TOP.ToString())
                            {
                                var recttrns = Arrow.GetChild(1).GetComponent<RectTransform>();

                                //recttrns.transform.forward = Camera.main.transform.forward;
                                if (Arrow.transform.name.Contains("Arrow_Gap_2"))
                                    recttrns.localRotation = Quaternion.Euler(90, 0, 180);
                                else
                                    recttrns.localRotation = Quaternion.Euler(90, 0, 0);



                                TextMeshPro tmpro = Arrow.GetChild(1).GetComponent<TextMeshPro>();
                                float f_s = tmpro.fontSize;

                                Arrow.GetChild(1).transform.Translate(Pergola_Model.transform.right * f_s / 2, Space.World);


                                float txt_width = tmpro.preferredWidth;

                                Arrow.GetChild(1).transform.Translate(Pergola_Model.transform.forward * txt_width / 2, Space.World);

                            }

                            if (view == Views._FRONT.ToString())
                            {
                                if (Arrow.name.Contains("Arrow_Gap_"))
                                {
                                    var recttrns = Arrow.GetChild(1).GetComponent<RectTransform>();

                                    //recttrns.transform.forward = Camera.main.transform.forward;
                                    if (Arrow.transform.name.Contains("Arrow_Gap_2"))
                                        recttrns.localRotation = Quaternion.Euler(0, -90, 90);
                                    else
                                        recttrns.localRotation = Quaternion.Euler(180, -90, -90);



                                    TextMeshPro tmpro = Arrow.GetChild(1).GetComponent<TextMeshPro>();
                                    float f_s = tmpro.fontSize;

                                    Arrow.GetChild(1).transform.Translate(-Pergola_Model.transform.up * f_s / 2, Space.World);


                                    float txt_width = tmpro.preferredWidth;

                                    Arrow.GetChild(1).transform.Translate(Pergola_Model.transform.forward * txt_width / 2, Space.World);

                                }
                                if (Arrow.name.Contains("FrameC"))
                                {
                                    var recttrns = Arrow.GetChild(1).GetComponent<RectTransform>();


                                    recttrns.localRotation = Quaternion.Euler(0, -90, 0);
                                }
                                //Giving rotation exclusively for Front Arrow Dots
                                foreach (Transform dots in Arrow.transform.GetChild(3).GetChild(0))
                                {
                                    dots.transform.localRotation = Quaternion.Euler(0, 90, 0);
                                }
                                foreach (Transform dots in Arrow.transform.GetChild(0).GetChild(0))
                                {
                                    dots.transform.localRotation = Quaternion.Euler(0, 90, 0);
                                }

                            }

                            if (Arrow.name.Contains("Arrow_FrameD"))
                            {
                                var recttrns = Arrow.GetChild(1).GetComponent<RectTransform>();

                                recttrns.transform.localRotation = Quaternion.Euler(180, -90, 0);
                                //TextMeshPro tmpro = Arrow.GetChild(1).GetComponent<TextMeshPro>();
                                //float f_s = tmpro.fontSize;

                                //Here we have an extra text to indicate the section count in Pergola
                                if (Arrow.GetChild(0).GetChild(1) != null)
                                {
                                    TextMeshPro Arrow_Head_tmpro = Arrow.GetChild(0).GetChild(1).GetComponent<TextMeshPro>();
                                    //Adding Field section count
                                    Arrow_Head_tmpro.text = "X" + DB_script.Field_Parent.transform.childCount;

                                    var r_tr = Arrow_Head_tmpro.transform.GetComponent<RectTransform>();

                                    //r_tr.localRotation = Quaternion.Euler(120, -90, -90);// (0, 90, 90);
                                    r_tr.transform.localRotation = Quaternion.Euler(180, -90, -90);
                                }
                                //Here we have an extra text to indicate the section count in Pergola
                                if (Arrow.GetChild(1).GetChild(0) != null)
                                {
                                    TextMeshPro Arrow_count_field = Arrow.GetChild(1).GetChild(0).GetComponent<TextMeshPro>();

                                    //Arrow_count_field.GetComponent<MeshRenderer>().enabled=true;

                                    GameObject Fields_1 = GameObject.Find("Field_Parent/Field_Group_0001/Fields_1");

                                    int rafafa_count = 10;
                                    if (Fields_1 != null)
                                    {
                                        rafafa_count = Fields_1.transform.childCount;
                                    }


                                        //Adding Field section count
                                        Arrow_count_field.text = "(" + rafafa_count.ToString()+")";

                                    var r_tr = Arrow_count_field.transform.GetComponent<RectTransform>();

                                    //r_tr.localRotation = Quaternion.Euler(120, -90, -90);// (0, 90, 90);
                                    //r_tr.transform.localRotation = Quaternion.Euler(180, -90, -90);
                                }
                            }

                            if (Arrow.name.Contains("Arrow_Clamp_onFrame"))
                            {
                                GameObject txt = Arrow.GetChild(1).gameObject;

                                var recttrns = Arrow.GetChild(1).GetComponent<RectTransform>();

                                TextMeshPro tmpro = txt.GetComponent<TextMeshPro>();

                                recttrns.localRotation = Quaternion.Euler(90, 90, 0);

                                //moving arrow to right side of top view 
                                txt.transform.Translate(Pergola_Model.transform.forward * tmpro.preferredHeight, Space.World);
                            }

                            if (Arrow.name.Contains("Arrow_Space_Invisible"))
                            {
                                GameObject txt = Arrow.GetChild(1).gameObject;

                                DestroyImmediate(txt);
                            }

                            if(Arrow.name.Contains("_height"))
                            {


                                Arrow.transform.GetChild(1).transform.GetComponent<RectTransform>().localRotation = Quaternion.Euler(90, -90, 0);

                                TextMeshPro tmpro = Arrow.transform.GetChild(1).GetComponent<TextMeshPro>();
                                float f_s = tmpro.fontSize;

                                //Arrow.transform.GetChild(1).transform.Translate(Pergola_Model.transform.right * f_s / 2, Space.World);
                            }
                        }

                    if (view == Views._TOP.ToString())
                    {

                        if (GameObject.Find(Parents_name.SupportBars_Parent.ToString()) != null)
                        {
                            foreach (Transform support_bar in DB_script.SupportBars_Parent.transform)
                            {
                                GameObject support_clamp = support_bar.transform.GetChild(0).gameObject;

                                if (support_clamp != null)
                                {
                                    GameObject sp = Instantiate(sphere_, Pergola_Model.transform);//duplicate spheres
                                    sp.name = "sphere_" + support_clamp.name;//giving name sphere_spportClam_0

                                    Bounds suppp_clamp_bounds = support_clamp.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                    Vector3 support_clamp_center_global = support_clamp.transform.TransformPoint(suppp_clamp_bounds.center);

                                    float center2edge_dist_1 = Mathf.Abs(Vector3.Dot(support_clamp.transform.InverseTransformDirection(Pergola_Model.transform.right), suppp_clamp_bounds.size)) / 2;

                                    Vector3 sph_pos = support_clamp_center_global;

                                    sp.transform.position = sph_pos;

                                    //****TO scale Sphere according to width of a frame***********//
                                    float scale_sphere = 2F * Vector3.Dot(frame_A.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_a.size);

                                    sp.transform.localScale = new Vector3(scale_sphere, scale_sphere, scale_sphere);


                                }
                            }
                        }





                        if (GameObject.Find(Parents_name.Field_Parent.ToString()) != null)
                        {
                            GameObject.Find(Parents_name.Field_Parent.ToString()).SetActive(false);
                        }

                        if (GameObject.Find(Parents_name.SupportBars_Parent.ToString()) != null)
                        {
                            GameObject.Find(Parents_name.SupportBars_Parent.ToString()).SetActive(false);
                        }
                    }

                    else if (view == Views._FRONT.ToString())
                    {

                        //hide Support bars
                        if (GameObject.Find(Parents_name.SupportBars_Parent.ToString()) != null)
                        {
                            GameObject.Find(Parents_name.SupportBars_Parent.ToString()).SetActive(false);

                        }

                        //hide Frame_A and Frame_C

                        //frame_A.SetActive(false);
                        //frame_C.SetActive(false);
                        if (GameObject.Find(Parents_name.Frames_Parent.ToString()) != null)
                        {
                            foreach (Transform frame_subparent in GameObject.Find(Parents_name.Frames_Parent.ToString()).transform)
                            {
                                if (frame_subparent.name == "Frame_Parent")
                                {
                                    foreach (Transform frms in frame_subparent)
                                    {
                                        if (frms.name.Contains("FrameA") || frms.name.Contains("FrameC"))
                                        {
                                            frms.gameObject.SetActive(false);

                                           
                                        }
                                        else
                                        {
                                            foreach (Transform frm_ch in frms)
                                            {
                                                if (frm_ch.name.Contains("170000059"))
                                                {
                                                    frm_ch.gameObject.SetActive(false);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (frame_subparent.name == "Accesories_Parent")
                                {
                                    //foreach (Transform frms in frame_subparent)
                                    //{
                                    //    if (frms.name.Contains("ak - 79"))
                                    //    {
                                    //        frms.gameObject.SetActive(false);
                                    //    }
                                    //}
                                    frame_subparent.gameObject.SetActive(false);
                                }

                            }
                        }
                        //hide fields
                        if (GameObject.Find(Parents_name.Field_Parent.ToString()) != null)
                        {

                            GameObject Field_Parent = GameObject.Find(Parents_name.Field_Parent.ToString());
                            foreach (Transform fields_group in Field_Parent.transform)
                            {
                                //setting field false
                                if (fields_group.transform.childCount > 1)
                                    fields_group.transform.GetChild(1).gameObject.SetActive(false);

                                if (fields_group.transform.childCount > 0)
                                    foreach (Transform accs in fields_group.transform.GetChild(0))
                                    {
                                        //Hiding Top L_Accessory 
                                        if (accs.name.Contains("L_Accessory_top") || accs.name.Contains("ak - 109a"))
                                            accs.gameObject.SetActive(false);
                                        else
                                            accs.gameObject.SetActive(true);

                                    }
                            }
                        }

                        GameObject FielDiv_Parent = GameObject.Find(Parents_name.FieldDividers_Parent.ToString());
                        if (FielDiv_Parent != null)
                        {
                            foreach (Transform field_div in FielDiv_Parent.transform)
                            {
                                foreach (Transform div_ch in field_div)
                                {
                                    // if (field_div.GetChild(0) != null)
                                    if (div_ch.name.Contains("FieldDivider"))
                                    {

                                        div_ch.gameObject.SetActive(true);
                                                                          
                                    }
                                    else
                                    {
                                        div_ch.gameObject.SetActive(false);
                                    }
                                }
                               
                            }
                        }

                        GameObject FrameDivider_parent = GameObject.Find(Parents_name.FrameDividers_Parent.ToString());
                        if (FielDiv_Parent != null)
                        {
                            foreach (Transform frame_div in FrameDivider_parent.transform)
                            {
                                foreach (Transform div_ch in frame_div)
                                {
                                    // if (field_div.GetChild(0) != null)
                                    if (div_ch.name.Contains("FrameDivider"))
                                    {

                                        div_ch.gameObject.SetActive(true);

                                    }
                                    else
                                    {
                                        div_ch.gameObject.SetActive(false);
                                    }
                                }

                            }
                        }
                    }

                    else if (view == Views._FIELD.ToString())
                    {


                        //HIDE  FRAME PARENT

                        if (GameObject.Find(Parents_name.Frames_Parent.ToString()) != null)
                        {
                            GameObject.Find(Parents_name.Frames_Parent.ToString()).SetActive(false);
                        }

                        //HIDE SUPPORT BAR PARENT
                        if (GameObject.Find(Parents_name.SupportBars_Parent.ToString()) != null)
                        {
                            GameObject.Find(Parents_name.SupportBars_Parent.ToString()).SetActive(false);
                        }

                        //HIDE DIVIDER BAR PARENT
                        if (GameObject.Find(Parents_name.Divider_Parent.ToString()) != null)
                        {
                            GameObject.Find(Parents_name.Divider_Parent.ToString()).SetActive(false);
                        }

                        //hide fields
                        if (GameObject.Find(Parents_name.Field_Parent.ToString()) != null)
                        {

                            GameObject Field_Parent = GameObject.Find(Parents_name.Field_Parent.ToString());
                            foreach (Transform fields_group in Field_Parent.transform)
                            {
                                //setting field false
                                //fields_group.transform.GetChild(1).gameObject.SetActive(false);
                                if (fields_group.name == "Field_Group_0001")
                                {
                                    fields_group.gameObject.SetActive(true);
                                    foreach (Transform accs in fields_group.transform.GetChild(0))
                                    {
                                        //Hiding Top L_Accessory 
                                        if (accs.name.Contains("L_Accessory_top") || accs.name.Contains("ak - 109a"))
                                            accs.gameObject.SetActive(false);
                                        else
                                            accs.gameObject.SetActive(true);

                                    }
                                }
                                else
                                {
                                    fields_group.gameObject.SetActive(false);
                                }
                            }
                        }
                        ////hide fields
                        //if (GameObject.Find(Parents_name.Field_Parent.ToString()) != null)
                        //{

                        //    GameObject Field_Parent = GameObject.Find(Parents_name.Field_Parent.ToString());
                        //    foreach (Transform fields_group in Field_Parent.transform)
                        //    {
                        //        //setting field false
                        //        //fields_group.transform.GetChild(1).gameObject.SetActive(false);


                        //    }
                        //}

                    }

                    if (view == Views._SIDE_FIELD.ToString())
                    {
                        foreach (Transform Parent in Pergola_Model.transform)
                        {
                            //Hide all Parent except field and Arrow Parent
                            if (Parent.name == Parents_name.Field_Parent.ToString() || Parent.name == Parents_name.Arrow_Parent.ToString())
                            {
                                Parent.gameObject.SetActive(true);
                            }
                            else
                            {
                                Parent.gameObject.SetActive(false);
                            }


                            //hide fields
                            if (GameObject.Find(Parents_name.Field_Parent.ToString()) != null)
                            {

                                GameObject Field_Parent = GameObject.Find(Parents_name.Field_Parent.ToString());
                                foreach (Transform fields_group in Field_Parent.transform)
                                {
                                    //setting field false
                                    //fields_group.transform.GetChild(1).gameObject.SetActive(false);

                                    foreach (Transform accs in fields_group.transform.GetChild(0))
                                    {
                                        //Hiding Top L_Accessory 
                                        if (accs.name.Contains("L_Accessory_top") || accs.name.Contains("ak - 109a"))
                                            accs.gameObject.SetActive(false);
                                        else
                                            accs.gameObject.SetActive(true);

                                    }
                                }
                            }
                        }
                    }

                }
            }
            else if (DB_script.L_type)
            {


                    print("region 1,2 3 count :"+DB_script.region1_fields_count+","+DB_script.region2_fields_count+","+DB_script.region3_fields_count);

                int sc_cnv_regio1 = DB_script.region2_fields_count, sc_cnv_regio2 = DB_script.region3_fields_count, sc_cnv_regio3 = DB_script.region1_fields_count;

                if (GameObject.Find("Pergola_Model") != null)
                {
                    GameObject Pergola_Model = GameObject.Find("Pergola_Model");

                    GameObject frame_A;
                    if (GameObject.Find("FrameA_0"))
                    {
                        frame_A = GameObject.Find("FrameA_0");
                    }
                    else
                    {

                        frame_A = GameObject.Find("FrameA");
                    }

                    GameObject frame_B;

                    if (GameObject.Find("FrameB_0"))
                    {
                        frame_B = GameObject.Find("FrameB_0");
                    }
                    else
                    {
                        frame_B = GameObject.Find("FrameB");
                    }


                    GameObject frame_C;
                    if (GameObject.Find("FrameC_0"))
                    {
                        frame_C = GameObject.Find("FrameC_0");
                    }
                    else
                    {
                        frame_C = GameObject.Find("FrameC");
                    }
                    GameObject frame_D;

                    if (GameObject.Find("FrameD_0"))
                    {
                        frame_D = GameObject.Find("FrameD_0");
                    }
                    else
                    {
                        frame_D = GameObject.Find("FrameD");
                    }


                    GameObject frame_E;

                    if (GameObject.Find("FrameE_0"))
                    {
                        frame_E = GameObject.Find("FrameE_0");
                    }
                    else
                    {
                        frame_E = GameObject.Find("FrameE");
                    }

                    GameObject frame_F;

                    if (GameObject.Find("FrameF_0"))
                    {
                        frame_F = GameObject.Find("FrameF_0");
                    }
                    else
                    {
                        frame_F = GameObject.Find("FrameF");
                    }
                    GameObject FrameDividers_Parent_Section_0001 = DB_script.FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0001").gameObject;

                    GameObject FieldDividers_Parent_Section_0001 = DB_script.FieldDividers_Parent.transform.Find("FieldDividers_Parent_Section_0001").gameObject;

                    GameObject FrameDividers_Parent_Section_0002 = DB_script.FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").gameObject;

                    GameObject FieldDividers_Parent_Section_0002 = DB_script.FieldDividers_Parent.transform.Find("FieldDividers_Parent_Section_0002").gameObject;

                    Bounds bound_a = frame_A.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    Bounds bound_b = frame_B.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    Bounds bound_c = frame_C.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    Bounds bound_d = frame_D.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;


                    Bounds bound_e = frame_E.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;


                    Bounds bound_f = frame_F.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    Vector3 req_right = Pergola_Model.transform.forward, req_up = -Pergola_Model.transform.up, req_fwd = Pergola_Model.transform.right;
                    //view = "_Top";
                    rotateonmouse rotateonmouse_script = Pergola_Model.GetComponent<rotateonmouse>();

                    bool type_2 = DB_script.rafafa_placement_type == rafaffa_Placement_string.type_2.ToString();


                    if (view == Views._TOP.ToString())
                    {
                        field_arrow_names_dist = new SortedDictionary<string, string>();
                        GameObject Field_divider_Parent = DB_script.FieldDividers_Parent;
                        GameObject FrameDivider_Parent = DB_script.FrameDividers_Parent;

                        //creating a sorted List wrt Z axis value wrt Pergola_Model
                        SortedList<float, Transform> dividers_section1_z_sort_ = new SortedList<float, Transform>();

                        //creating a sorted List wrt X axis value wrt Pergola_Model
                        SortedList<float, Transform> dividers_section2_x_sort_ = new SortedList<float, Transform>();

                        //GameObject FieldDividers_Parent_Section_0001 = DB_script.FieldDividers_Parent.transform.Find("FieldDividers_Parent_Section_0001").gameObject;

                        foreach (Transform FieldDividers_Parent_Section_000 in Field_divider_Parent.transform)
                        {
                            if (FieldDividers_Parent_Section_000.name.Contains("FieldDividers_Parent_Section_0001"))
                            {

                                foreach (Transform field_div in FieldDividers_Parent_Section_000.transform)
                                {
                                    foreach (Transform div_ch in field_div)
                                    {
                                        // if (field_div.GetChild(0) != null)
                                      if (div_ch.name.Contains("FrameDivider")||div_ch.name.Contains("FieldDivider"))
                                        {

                                            Vector3 global_center_f_div = div_ch.TransformPoint(div_ch.gameObject.GetComponentInChildren<MeshFilter>().mesh.bounds.center);
                                            //Taking local pos wrt Pergola model
                                            Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(global_center_f_div);
                                            if (true || DB_script.rafafa_placement_type == rafaffa_Placement_string.type_2.ToString())
                                            {

                                                //Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(div_ch.transform.position);
                                                //
                                                if (!dividers_section1_z_sort_.ContainsKey(f_div_pos_wrt_Perg_Mod.z))
                                                    dividers_section1_z_sort_.Add(f_div_pos_wrt_Perg_Mod.z, div_ch);
                                            }
                                            else
                                            {
                                                //Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(div_ch.transform.position);

                                                //Adding negative value so as to add and sort in negative order
                                                // if (!dividers_section2_x_sort_.ContainsKey(f_div_pos_wrt_Perg_Mod.x))
                                                //     dividers_section2_x_sort_.Add(f_div_pos_wrt_Perg_Mod.x, div_ch);
                                            }
                                        }
                                    }

                                }
                            }
                            else if (FieldDividers_Parent_Section_000.name.Contains("FieldDividers_Parent_Section_0002"))
                            {
                                foreach (Transform field_div in FieldDividers_Parent_Section_000.transform)
                                {
                                    foreach (Transform div_ch in field_div)
                                    {
                                        // if (field_div.GetChild(0) != null)
                                      if (div_ch.name.Contains("FrameDivider")||div_ch.name.Contains("FieldDivider"))
                                        {

                                            Vector3 global_center_f_div = div_ch.TransformPoint(div_ch.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                                            //Taking local pos wrt Pergola model
                                            Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(global_center_f_div);
                                            if (true || DB_script.rafafa_placement_type == rafaffa_Placement_string.type_2.ToString())
                                            {
                                                //Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(div_ch.transform.position);

                                                //Adding negative value so as to add and sort in negative order
                                                if (!dividers_section2_x_sort_.ContainsKey(f_div_pos_wrt_Perg_Mod.x))
                                                    dividers_section2_x_sort_.Add(f_div_pos_wrt_Perg_Mod.x, div_ch);

                                            }
                                            else
                                            {
                                                //Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(div_ch.transform.position);

                                                // if (!dividers_section1_z_sort_.ContainsKey(f_div_pos_wrt_Perg_Mod.z))
                                                //     dividers_section1_z_sort_.Add(f_div_pos_wrt_Perg_Mod.z, div_ch);
                                            }
                                        }
                                    }

                                }
                            }
                        }

                        foreach (Transform FrameDividers_Parent_Section_000 in FrameDivider_Parent.transform)
                        {
                            if (FrameDividers_Parent_Section_000.transform.name.Contains("FrameDividers_Parent_Section_0001"))
                            {
                                foreach (Transform frame_div in FrameDividers_Parent_Section_000.transform)
                                {

                                    foreach (Transform div_ch in frame_div)
                                    {
                                        // if (frame_div.GetChild(0) != null)
                                        if (div_ch.name.Contains("FrameDivider")||div_ch.name.Contains("FieldDivider"))
                                        {

                                            if (true || DB_script.rafafa_placement_type == rafaffa_Placement_string.type_2.ToString())
                                            {

                                                Vector3 global_center_f_div = div_ch.TransformPoint(div_ch.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                                                //Taking local pos wrt Pergola model
                                                Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(global_center_f_div);
                                                //Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(div_ch.transform.position);

                                                if (!dividers_section1_z_sort_.ContainsKey(f_div_pos_wrt_Perg_Mod.z))
                                                    dividers_section1_z_sort_.Add(f_div_pos_wrt_Perg_Mod.z, div_ch);
                                            }
                                            else
                                            {
                                                // Vector3 global_center_f_div = div_ch.TransformPoint(div_ch.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                                                // //Taking local pos wrt Pergola model
                                                // Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(global_center_f_div);

                                                // //Adding negative value so as to add and sort in negative order
                                                // if (!dividers_section2_x_sort_.ContainsKey(f_div_pos_wrt_Perg_Mod.x))
                                                //     dividers_section2_x_sort_.Add(f_div_pos_wrt_Perg_Mod.x, div_ch);
                                            }
                                        }
                                    }

                                }
                            }
                            if (FrameDividers_Parent_Section_000.transform.name.Contains("FrameDividers_Parent_Section_0002"))
                            {
                                foreach (Transform frame_div in FrameDividers_Parent_Section_000.transform)
                                {

                                    foreach (Transform div_ch in frame_div)
                                    {
                                        // if (frame_div.GetChild(0) != null)
                                        if (div_ch.name.Contains("FrameDivider")||div_ch.name.Contains("FieldDivider"))
                                        {

                                            if (true || DB_script.rafafa_placement_type == rafaffa_Placement_string.type_2.ToString())
                                            {
                                                Vector3 global_center_f_div = div_ch.TransformPoint(div_ch.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                                                //Taking local pos wrt Pergola model
                                                Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(global_center_f_div);

                                                //Adding negative value so as to add and sort in negative order
                                                if (!dividers_section2_x_sort_.ContainsValue(div_ch)&&!dividers_section2_x_sort_.ContainsKey(f_div_pos_wrt_Perg_Mod.x))
                                                    dividers_section2_x_sort_.Add(f_div_pos_wrt_Perg_Mod.x, div_ch);
                                            }
                                            else
                                            {
                                                // Vector3 global_center_f_div = div_ch.TransformPoint(div_ch.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                                                // //Taking local pos wrt Pergola model
                                                // Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(global_center_f_div);

                                                // if (!dividers_section1_z_sort_.ContainsValue(div_ch))
                                                //     dividers_section1_z_sort_.Add(f_div_pos_wrt_Perg_Mod.z, div_ch);
                                            }

                                        }
                                    }
                                }
                            }

                        }

                        if (true||DB_script.rafafa_placement_type == rafaffa_Placement_string.type_2.ToString())
                        {
                            foreach (Transform field_R_div_section1 in dividers_section1_z_sort_.Values)
                            {
                                GameObject Arrow_Green = null;

                                GameObject field_R_frameDivider = field_R_div_section1.gameObject;

                                Bounds horizontal_field_bound = field_R_frameDivider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                float max_lenth_of_field_R_frameDivider = Mathf.Abs(Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z));

                                //max_lenth_of_field_R_frameDivider += 400;//to bring the arrows forward extra offset of 400

                                float min_distance_2 = 0; //Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z);

                                Vector3 arrow_direction_wrt_frameParent_dir = -Pergola_Model.transform.forward;

                                Vector3 arrow_for_end_field_divider_diretion = -arrow_direction_wrt_frameParent_dir;
                                float center2edge_dist = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized), horizontal_field_bound.size)) / 2;
                                Vector3 arrow_pos = field_R_frameDivider.transform.TransformPoint(horizontal_field_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);


                                float frame_depth = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_b.size));

                                frame_depth += 200;//to bring the arrows up extra offset of 200
                                Vector3 prefab_Tmp_text_scale = Arrow_prefab.transform.GetChild(1).GetComponent<TextMeshPro>().rectTransform.localScale * 2;//ACCESSING LOCALSCALE OF TEXT

                                RaycastHit hit1;



                                if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                                {
                                    Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);



                                    arrow_parm ar_field_div = new arrow_parm()
                                    {
                                        position_of_arrow = arrow_pos,

                                        direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                                        length_of_arrow = hit1.distance,

                                        arrow_name = "Arrow_Gap_" + field_R_frameDivider.name+"_"+hit1.transform.name,

                                        pergola_right = -max_lenth_of_field_R_frameDivider * 1 / 4
                                    };

                                    //Updating the list to access while taking front screen shot as distance is varied
                                    if (!field_arrow_names_dist.ContainsKey(ar_field_div.arrow_name))
                                    {
                                        field_arrow_names_dist.Add(ar_field_div.arrow_name, ((float)Math.Floor(hit1.distance)).ToString());
                                    }


                                    arrows_function(ar_field_div);

                                }

                                //To get the last arrow of field divider we raycst in opposite direction

                                if (dividers_section1_z_sort_.IndexOfValue(field_R_div_section1) == dividers_section1_z_sort_.Count - 1)
                                {
                                    //assigning opposite direction to arrow

                                    arrow_direction_wrt_frameParent_dir = Pergola_Model.transform.forward;


                                    arrow_pos = field_R_frameDivider.transform.TransformPoint(horizontal_field_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);

                                    if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                                    {
                                        Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);




                                        arrow_parm ar_field_div = new arrow_parm()
                                        {
                                            position_of_arrow = arrow_pos,

                                            direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                                            length_of_arrow = hit1.distance,

                                            arrow_name = "Arrow_Gap_2" + field_R_frameDivider.name + "_" + hit1.transform.name,

                                            pergola_right = -max_lenth_of_field_R_frameDivider * 1 / 4,




                                        };

                                        //Updating the list to access while taking front screen shot as distance is varied
                                        if (!field_arrow_names_dist.ContainsKey(ar_field_div.arrow_name))
                                        {
                                            field_arrow_names_dist.Add(ar_field_div.arrow_name, ((float)Math.Floor(hit1.distance)).ToString());
                                        }

                                        arrows_function(ar_field_div);

                                    }

                                }
                            }

                            foreach (Transform field_R_div_section2 in dividers_section2_x_sort_.Values)
                            {
                                GameObject Arrow_Green = null;

                                GameObject field_R_frameDivider = field_R_div_section2.gameObject;

                                Bounds horizontal_field_bound = field_R_frameDivider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                float max_lenth_of_field_R_frameDivider = Mathf.Abs(Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z));

                                //max_lenth_of_field_R_frameDivider += 400;//to bring the arrows forward extra offset of 400

                                float min_distance_2 = 0; //Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z);

                                Vector3 arrow_direction_wrt_frameParent_dir = -Pergola_Model.transform.right;

                                Vector3 arrow_for_end_field_divider_diretion = -arrow_direction_wrt_frameParent_dir;
                                float center2edge_dist = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized), horizontal_field_bound.size)) / 2;
                                Vector3 arrow_pos = field_R_frameDivider.transform.TransformPoint(horizontal_field_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);


                                float frame_depth = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_b.size));

                                frame_depth += 200;//to bring the arrows up extra offset of 200
                                Vector3 prefab_Tmp_text_scale = Arrow_prefab.transform.GetChild(1).GetComponent<TextMeshPro>().rectTransform.localScale * 2;//ACCESSING LOCALSCALE OF TEXT

                                RaycastHit hit1;



                                if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                                {
                                    Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);



                                    arrow_parm ar_field_div = new arrow_parm()
                                    {
                                        position_of_arrow = arrow_pos,

                                        direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                                        length_of_arrow = hit1.distance,

                                        arrow_name = "Arrow_Gap_" + field_R_frameDivider.name,

                                        //pergola_right = -max_lenth_of_field_R_frameDivider * 1 / 4
                                    };

                                    //Updating the list to access while taking front screen shot as distance is varied
                                    if (!field_arrow_names_dist.ContainsKey(ar_field_div.arrow_name))
                                    {
                                        field_arrow_names_dist.Add(ar_field_div.arrow_name, ((float)Math.Floor(hit1.distance)).ToString());
                                    }


                                    arrows_function(ar_field_div);

                                }

                                //To get the last arrow of field divider we raycst in opposite direction
                                if(type_2==false)
                                if (dividers_section2_x_sort_.IndexOfValue(field_R_div_section2) == dividers_section2_x_sort_.Count - 1)
                                {
                                    //assigning opposite direction to arrow

                                    arrow_direction_wrt_frameParent_dir = Pergola_Model.transform.right;


                                    arrow_pos = field_R_frameDivider.transform.TransformPoint(horizontal_field_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);

                                    if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                                    {
                                        Debug.DrawLine(arrow_pos, hit1.point, Color.red, 10);




                                        arrow_parm ar_field_div = new arrow_parm()
                                        {
                                            position_of_arrow = arrow_pos,

                                            direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                                            length_of_arrow = hit1.distance,

                                            arrow_name = "Arrow_Gap_2" + field_R_frameDivider.name,

                                            //pergola_right = -max_lenth_of_field_R_frameDivider * 1 / 4,




                                        };

                                        //Updating the list to access while taking front screen shot as distance is varied
                                        if (!field_arrow_names_dist.ContainsKey(ar_field_div.arrow_name))
                                        {
                                            field_arrow_names_dist.Add(ar_field_div.arrow_name, ((float)Math.Floor(hit1.distance)).ToString());
                                        }

                                        arrows_function(ar_field_div);

                                    }

                                }
                            }
                        }
                        else
                        {

                        }

                        //continuation
                        #region Arrow for Frame_C 
                        float pergola_forward_offset = -DB_script.frame_C_length / 2;

                        Vector3 dotted_line_frame_C_dir = Pergola_Model.transform.forward;

                        float fwd_offset = Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300;

                        float right_offset = frame_C.transform.localScale.y / 2;
                        if (GameObject.Find("FrameC_0"))
                        {
                            right_offset = DB_script.frame_C_length - frame_C.transform.localScale.y / 2;

                        }

                        arrow_parm arrow_Frame_C_params = new arrow_parm()
                        {
                            position_of_arrow = frame_C.transform.TransformPoint(bound_c.center),
                            direction_of_arrow = -(Pergola_Model.transform.right),
                            length_of_arrow = DB_script.frame_C_length,
                            arrow_name = "Arrow_" + frame_C.name,



                            pergola_fwd = fwd_offset,// pergola_forward_offset,


                            pergola_right = right_offset,//Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300,

                            pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_c.size))+300

                            dotted_line_offset = fwd_offset,

                            Dotted_line_Direction = Pergola_Model.transform.forward,

                            orientation_dotted_lines = "horizontal",


                        };

                        //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                        arrows_function(arrow_Frame_C_params);


                        #endregion


                        float pergola_rigth_offset = DB_script.frame_B_length / 2;
                        #region Arrow for Frame_B

                        Vector3 dotted_line_frame_B_dir = Pergola_Model.transform.right;

                        right_offset = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) + 300;

                        fwd_offset = frame_B.transform.localScale.y / 2;


                        if (GameObject.Find("FrameB_0"))
                        {
                            fwd_offset = DB_script.frame_B_length - frame_B.transform.localScale.y / 2;

                        }

                        arrow_parm arrow_Frame_B_params = new arrow_parm()
                        {
                            position_of_arrow = frame_B.transform.TransformPoint(bound_b.center),
                            direction_of_arrow = -(Pergola_Model.transform.forward),
                            length_of_arrow = DB_script.frame_B_length,
                            arrow_name = "Arrow_" + frame_B.name,



                            pergola_fwd = fwd_offset,


                            pergola_right = -right_offset,//Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) + 100,

                            pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_B.size))+300


                            Dotted_line_Direction = -Pergola_Model.transform.right,

                            dotted_line_offset = right_offset,

                            //orientation_dotted_lines = "horizontal"
                        };

                        //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                        arrows_function(arrow_Frame_B_params);

                        #endregion



                        #region Arrow for Frame_A 
                        pergola_forward_offset = -DB_script.frame_A_length / 2;

                        Vector3 dotted_line_frame_A_dir = Pergola_Model.transform.forward;

                        fwd_offset = Mathf.Abs(Vector3.Dot(frame_A.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_a.size)) + 300;

                        float frm_B_width = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size));

                        right_offset = frame_A.transform.localScale.y / 2;
                        if (GameObject.Find("FrameA_0"))
                        {
                            right_offset = DB_script.frame_A_length - frame_A.transform.localScale.y / 2;

                        }

                        arrow_parm arrow_Frame_A_params = new arrow_parm()
                        {
                            position_of_arrow = frame_A.transform.TransformPoint(bound_a.center),
                            direction_of_arrow = -(Pergola_Model.transform.right),
                            length_of_arrow = DB_script.frame_A_length,

                            txt = DB_script.frame_A_length.ToString(),
                            arrow_name = "Arrow_" + frame_A.name,



                            pergola_fwd = fwd_offset,// pergola_forward_offset,


                            pergola_right = right_offset - frm_B_width,//Mathf.Abs(Vector3.Dot(frame_A.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300,

                            pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_A.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_c.size))+300

                            dotted_line_offset = fwd_offset,

                            Dotted_line_Direction = Pergola_Model.transform.forward,

                            orientation_dotted_lines = "horizontal",


                        };

                        //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                        arrows_function(arrow_Frame_A_params);


                        #endregion

                        #region Arrow for Frame_D

                        Vector3 dotted_line_frame_D_dir = Pergola_Model.transform.right;

                        right_offset = Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_d.size)) + 300;

                        fwd_offset = frame_D.transform.localScale.y / 2;


                        if (GameObject.Find("FrameD_0"))
                        {
                            fwd_offset = DB_script.frame_D_length - frame_D.transform.localScale.y / 2;

                        }

                        arrow_parm arrow_Frame_D_params = new arrow_parm()
                        {
                            position_of_arrow = frame_D.transform.TransformPoint(bound_d.center),
                            direction_of_arrow = -(Pergola_Model.transform.forward),
                            length_of_arrow = DB_script.frame_D_length,
                            arrow_name = "Arrow_" + frame_D.name,



                            pergola_fwd = fwd_offset,


                            pergola_right = right_offset,//Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) + 100,

                            pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_D.size))+300


                            Dotted_line_Direction = Pergola_Model.transform.right,

                            dotted_line_offset = right_offset,

                            //orientation_dotted_lines = "horizontal"
                        };

                        //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                        arrows_function(arrow_Frame_D_params);

                        #endregion


                        #region Arrow for Frame_E 
                        pergola_forward_offset = -DB_script.frame_E_length / 2;

                        Vector3 dotted_line_frame_E_dir = -Pergola_Model.transform.forward;

                        fwd_offset = Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300;

                        right_offset = frame_E.transform.localScale.y / 2;
                        if (GameObject.Find("FrameE_0"))
                        {
                            right_offset = DB_script.frame_E_length - frame_E.transform.localScale.y / 2;

                        }

                        arrow_parm arrow_Frame_E_params = new arrow_parm()
                        {
                            position_of_arrow = frame_E.transform.TransformPoint(bound_c.center),
                            direction_of_arrow = -(Pergola_Model.transform.right),
                            length_of_arrow = DB_script.frame_E_length,
                            arrow_name = "Arrow_" + frame_E.name,



                            pergola_fwd = -fwd_offset,// pergola_forward_offset,


                            pergola_right = right_offset,//Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300,

                            pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_c.size))+300

                            dotted_line_offset = -fwd_offset,

                            Dotted_line_Direction = Pergola_Model.transform.forward,

                            orientation_dotted_lines = "horizontal",


                        };

                        //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                        arrows_function(arrow_Frame_E_params);


                        #endregion

                        #region Arrow for Frame_F

                        Vector3 dotted_line_frame_F_dir = Pergola_Model.transform.right;

                        right_offset = Mathf.Abs(Vector3.Dot(frame_F.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) + 300;

                        fwd_offset = frame_F.transform.localScale.y / 2;


                        if (GameObject.Find("FrameF_0"))
                        {
                            fwd_offset = DB_script.frame_F_length - frame_F.transform.localScale.y / 2;

                        }

                        arrow_parm arrow_Frame_F_params = new arrow_parm()
                        {
                            position_of_arrow = frame_F.transform.TransformPoint(bound_b.center),
                            direction_of_arrow = -(Pergola_Model.transform.forward),
                            length_of_arrow = DB_script.frame_F_length,
                            arrow_name = "Arrow_" + frame_F.name,



                            pergola_fwd = fwd_offset,


                            pergola_right = -right_offset,//Mathf.Abs(Vector3.Dot(frame_F.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) + 100,

                            pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_F.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_F.size))+300


                            Dotted_line_Direction = -Pergola_Model.transform.right,

                            dotted_line_offset = right_offset,

                            //orientation_dotted_lines = "horizontal"
                        };

                        //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                        arrows_function(arrow_Frame_F_params);

                        #endregion

                        #region Support_bar_Arrow

                        GameObject SupportBars_Parent_Section1 = GameObject.Find("SupportBars_Parent/ SupportBars_Parent_Section1");
                        if (SupportBars_Parent_Section1 != null)
                        {
                            GameObject last_support_bar_parent = SupportBars_Parent_Section1.transform.GetChild(SupportBars_Parent_Section1.transform.childCount - 1).gameObject;



                            if (GameObject.Find("Clamp_onFrame_1") != null)
                            {
                                GameObject supportClamp0 = GameObject.Find("Clamp_onFrame_1");

                                Bounds support_clamp_bound = supportClamp0.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                //Arrow_Parent.transform.position = 
                                float max_lenth_of_field_R_frameDivider1 = Mathf.Abs(Mathf.Max(supportClamp0.transform.localScale.x, supportClamp0.transform.localScale.y, supportClamp0.transform.localScale.z));

                                max_lenth_of_field_R_frameDivider1 += 400;//to bring the arrows forward extra offset of 400

                                float min_distance_1 = 0; //Mathf.Max(field_R_frameDivider1.transform.localScale.x, field_R_frameDivider1.transform.localScale.y, field_R_frameDivider1.transform.localScale.z);

                                Vector3 arrow_direction_wrt_frameParent_dir1 = Pergola_Model.transform.right;

                                Vector3 arrow_for_end_field_divider_diretion_1 = -arrow_direction_wrt_frameParent_dir1;
                                float center2edge_dist_1 = Mathf.Abs(Vector3.Dot(supportClamp0.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir1.normalized), support_clamp_bound.size)) / 2;

                                float clamp_depth = Mathf.Abs(Vector3.Dot(supportClamp0.transform.InverseTransformDirection(Pergola_Model.transform.up), support_clamp_bound.size));

                                float frame_depth_1 = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_b.size));

                                // Vector3 arrow_pos_1 = supportClamp0.transform.TransformPoint(support_clamp_bound.center + supportClamp0.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir1.normalized) * center2edge_dist_1 + supportClamp0.transform.InverseTransformDirection(-Pergola_Model.transform.up) * clamp_depth / 2);
                                //frame_depth_1 += 200;//to bring the arrows up extra offset of 200
                                //Vector3 prefab_Tmp_text_scale_1 = Arrow_Orange_Prefab.transform.GetChild(1).GetComponent<TextMeshPro>().rectTransform.localScale * 2;//ACCESSING LOCALSCALE OF TEXT
                                Vector3 arrow_pos_1 = supportClamp0.transform.TransformPoint(support_clamp_bound.center + supportClamp0.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir1.normalized) * center2edge_dist_1 + supportClamp0.transform.InverseTransformDirection(-Pergola_Model.transform.up) * (clamp_depth / 2 + frame_depth_1 / 2));

                                float frameA_width = Mathf.Abs(Vector3.Dot(frame_A.transform.InverseTransformDirection(Pergola_Model.transform.right).normalized, bound_a.size));///2;


                                float support_arrow_right_off_dist = frameA_width + 300;



                                RaycastHit hitA;

                                float len_arrow = 0;
                                Debug.DrawRay(arrow_pos_1, arrow_direction_wrt_frameParent_dir1 * 10000, Color.cyan, 150);

                                if (Physics.Raycast(arrow_pos_1, arrow_direction_wrt_frameParent_dir1, out hitA, Mathf.Infinity, 1 << DB_script.frame_layer.value))
                                {
                                    //adding frame A width as ray hits box colloider
                                    len_arrow = hitA.distance + frameA_width;

                                }
                                arrow_parm ar_supp_bar = new arrow_parm()
                                {
                                    position_of_arrow = arrow_pos_1,
                                    direction_of_arrow = arrow_direction_wrt_frameParent_dir1,
                                    length_of_arrow = len_arrow,
                                    arrow_name = "Arrow_" + supportClamp0.name,



                                    pergola_fwd = -support_arrow_right_off_dist,


                                    //pergola_right = frame_B.transform.localScale.y / 2,//Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) + 100,

                                    pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_B.size))+300


                                    Dotted_line_Direction = Pergola_Model.transform.forward,

                                    dotted_line_offset = -support_arrow_right_off_dist,

                                    orientation_dotted_lines = "horizontal"

                                };

                                arrows_function(ar_supp_bar);
                            }
                        }
                        #endregion


                        rotateonmouse_script.rotate_TOP();

                    }
                    else
                    {
                        GameObject Wall_Parent = GameObject.Find("Wall_Parent");
                        if (Wall_Parent != null)
                            Wall_Parent.SetActive(false);
                    }

                    if (view == Views._FRONT.ToString())
                    {
                        float scale_factor = 1;//1
                        scale_factor = (int)((DB_script.frame_D_length * Mathf.Pow(10, -3)));


                        if (scale_factor < 1)
                        {
                            scale_factor = 1;
                        }
                        else if (scale_factor > 5)
                        {
                            scale_factor = 5;
                        }
                        try
                        {
                            rotateonmouse_script.rotate_FRONT();
                            //await UnityMainThreadDispatcher.DispatchAsync(() => Scale_fields_Accs_Frame(scale_factor));
                            scale_model_factor = scale_factor;
                           
                            

                            // field_arrow_names_dist = new SortedDictionary<string, string>();
                            //GameObject Field_divider_Parent = DB_script.FieldDividers_Parent;
                            //GameObject FrameDivider_Parent = DB_script.FrameDividers_Parent;
                            GameObject Field_divider_Parent = GameObject.Find("FieldDividers_Parent");
                            GameObject FrameDividers_Parent = GameObject.Find("FrameDividers_Parent");

                            //Creating a Global variable SortedList
                             Arrows_Measure.dividers_section1_z_sort = new SortedList<float, Transform>();

                            //GameObject FieldDividers_Parent_Section_0001 = DB_script.FieldDividers_Parent.transform.Find("FieldDividers_Parent_Section_0001").gameObject;
                            float gap_f_div = 300;
                            float frame_depth = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_b.size)) * scale_factor;
                            foreach (Transform FieldDividers_Parent_Section_000 in Field_divider_Parent.transform)
                            {
                                if (FieldDividers_Parent_Section_000.name.Contains("FieldDividers_Parent_Section_0001"))
                                {
                                    foreach (Transform field_div in FieldDividers_Parent_Section_000.transform)
                                    {
                                        if (true||DB_script.rafafa_placement_type == rafaffa_Placement_string.type_2.ToString())
                                        {
                                            foreach (Transform div_ch in field_div)
                                            {
                                                // if (frame_div.GetChild(0) != null)
                                                if (div_ch.name.Contains("FrameDivider") || div_ch.name.Contains("FieldDivider"))
                                                {
                                                    //Taking local pos wrt Pergola model
                                                    //field_div.GetChild(0)
                                                    //Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(div_ch.transform.position);
                                                    Vector3 global_center_f_div = div_ch.TransformPoint(div_ch.gameObject.GetComponentInChildren<MeshFilter>().mesh.bounds.center);
                                                    //Taking local pos wrt Pergola model
                                                    Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(global_center_f_div);
                                                    if (!Arrows_Measure.dividers_section1_z_sort.ContainsValue(div_ch)&&!Arrows_Measure.dividers_section1_z_sort.ContainsKey(f_div_pos_wrt_Perg_Mod.z))
                                                        Arrows_Measure.dividers_section1_z_sort.Add(f_div_pos_wrt_Perg_Mod.z, div_ch);
                                                }
                                            }
                                        }
                                        else
                                        {

                                        }
                                       
                                    }



                                }
                               


                            }

                            foreach (Transform FrameDividers_Parent_Section_000 in FrameDividers_Parent.transform)
                            {
                                if (FrameDividers_Parent_Section_000.transform.name.Contains("FrameDividers_Parent_Section_0001"))
                                {
                                    foreach (Transform frame_div in FrameDividers_Parent_Section_000.transform)
                                    {
                                        if (true||DB_script.rafafa_placement_type == rafaffa_Placement_string.type_2.ToString())
                                        {
                                            foreach (Transform div_ch in frame_div)
                                            {
                                                // if (field_div.GetChild(0) != null)
                                                   if (div_ch.name.Contains("FrameDivider")||div_ch.name.Contains("FieldDivider"))
                                                {
                                                    //Taking local pos wrt Pergola model
                                                    //frame_div.GetChild(0)
                                                    //Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(frame_div.GetChild(0).transform.position);
                                                    Vector3 global_center_f_div = div_ch.TransformPoint(div_ch.gameObject.GetComponentInChildren<MeshFilter>().mesh.bounds.center);
                                                    //Taking local pos wrt Pergola model
                                                    Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(global_center_f_div);
                                                    if (!Arrows_Measure.dividers_section1_z_sort.ContainsValue(div_ch)&&!Arrows_Measure.dividers_section1_z_sort.ContainsKey(f_div_pos_wrt_Perg_Mod.z))
                                                        Arrows_Measure.dividers_section1_z_sort.Add(f_div_pos_wrt_Perg_Mod.z,div_ch);
                                                }
                                            }
                                        }
                                        else
                                        {

                                        }
                                     
                                    }
                                }
                               

                            }

                            try
                            {

                           
                            Scale_fields_Accs_Frame(scale_factor);

                            await Task.Delay(300);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            foreach (Transform field_R_div in Arrows_Measure.dividers_section1_z_sort.Values)
                            {
                                //Vector3 f_div_pos_wrt_Perg_Mod = Pergola_Model.transform.InverseTransformPoint(field_div.transform.position);

                                //Arrows_Measure.dividers_section1_z_sort.Add(f_div_pos_wrt_Perg_Mod.z, field_div);

                                GameObject Arrow_Green = null;
//GetChild(0)
                                GameObject field_R_frameDivider = field_R_div.gameObject;

                                //GetComponentInChildren<MeshFilter>()
                                Bounds field_div_bound = field_R_frameDivider.transform.GetComponent<MeshFilter>().mesh.bounds;

                                float max_lenth_of_field_R_frameDivider = Mathf.Abs(Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z));

                                max_lenth_of_field_R_frameDivider += 400;//to bring the arrows forward extra offset of 400

                                float min_distance_2 = 0; //Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z);

                                Vector3 arrow_direction_wrt_frameParent_dir = -Pergola_Model.transform.forward;

                                Vector3 arrow_for_end_field_divider_diretion = -arrow_direction_wrt_frameParent_dir;

                                Vector3 ls_b = frame_B.transform.localScale;


                                Vector3 actual_Frame_B_size = new Vector3(ls_b.x * bound_b.size.x, ls_b.y * bound_b.size.y, ls_b.z * bound_b.size.z);

                                Vector3 actual_size = new Vector3(field_div_bound.size.x * field_R_frameDivider.transform.localScale.x, field_div_bound.size.y * field_R_frameDivider.transform.localScale.y, field_div_bound.size.z * field_R_frameDivider.transform.localScale.z);

                                float center2edge_dist = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized), field_div_bound.size)) / 2;



                                float f_div_depth_half = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(Pergola_Model.transform.up), field_div_bound.size)) / 2 * scale_factor;
                                Vector3 arrow_pos = field_R_frameDivider.transform.TransformPoint(field_div_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);// + field_R_frameDivider.transform.InverseTransformDirection(Pergola_Model.transform.up.normalized) * f_div_depth_half;



                                float offset_up = frame_depth - f_div_depth_half + gap_f_div;// + gap_f_div;

                                float arrow_off = frame_depth - 2 * f_div_depth_half + gap_f_div;
                                //frame_depth += gap_field_div;//to bring the arrows up extra offset of 200
                                Vector3 prefab_Tmp_text_scale = Arrow_prefab.transform.GetChild(1).GetComponent<TextMeshPro>().rectTransform.localScale * 2;//ACCESSING LOCALSCALE OF TEXT

                                RaycastHit hit1;



                                if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                                {
                                    Debug.DrawLine(arrow_pos, hit1.point, Color.red, 100);

                                    string arr_name = "Arrow_Gap_" + field_R_frameDivider.transform.name + "_" + hit1.transform.name;


                                    float txt_display = 0;

                                    //Taking values from Dictionary
                                    if (field_arrow_names_dist != null)
                                        if (field_arrow_names_dist.ContainsKey(arr_name))
                                        {
                                            txt_display = float.Parse(field_arrow_names_dist[arr_name]);
                                        }

                                    if (txt_display <= 0)
                                    {
                                        txt_display = hit1.distance;
                                    }


                                    arrow_parm ar_field_div = new arrow_parm()
                                    {
                                        position_of_arrow = arrow_pos,

                                        direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                                        length_of_arrow = hit1.distance,

                                        arrow_name = arr_name,//"Arrow_Gap_" + field_R_div.name,

                                        pergola_up = offset_up,

                                        //Dotted Line params

                                        Dotted_line_Direction = Pergola_Model.transform.up,

                                        dotted_line_offset = arrow_off,

                                        Dotted_facing_direction = Pergola_Model.transform.right,

                                        orientation_dotted_lines = "horizontal",

                                        txt = txt_display.ToString(),


                                    };

                                    arrows_function(ar_field_div);

                                }

                                //To get the last arrow of field divider we raycst in opposite direction
                                //field_div.GetSiblingIndex() == Field_divider_Parent.transform.childCount - 1

                                if (Arrows_Measure.dividers_section1_z_sort.IndexOfValue(field_R_div) == Arrows_Measure.dividers_section1_z_sort.Count - 1)
                                {
                                    //assigning opposite direction to arrow

                                    arrow_direction_wrt_frameParent_dir = Pergola_Model.transform.forward;


                                    arrow_pos = field_R_frameDivider.transform.TransformPoint(field_div_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);

                                    if (Physics.Raycast(arrow_pos, arrow_direction_wrt_frameParent_dir, out hit1, Mathf.Infinity, 1 << DB_script.frame_layer.value | 1 << DB_script.divider_layer.value))
                                    {
                                        Debug.DrawLine(arrow_pos, hit1.point, Color.red, 100);


                                        string arr_name = "Arrow_Gap_2" + field_R_frameDivider.transform.parent.name + "_" + hit1.transform.parent.name;

                                        float txt_display = 0;
                                        //Taking values from Dictionary
                                        if (field_arrow_names_dist != null)
                                            if (field_arrow_names_dist.ContainsKey(arr_name))
                                            {
                                                txt_display = float.Parse(field_arrow_names_dist[arr_name]);
                                            }

                                        if (txt_display <= 0)
                                        {
                                            txt_display = hit1.distance;
                                        }

                                        if (field_arrow_names_dist != null)
                                            if (field_arrow_names_dist.Count() > 0)
                                            txt_display = float.Parse(field_arrow_names_dist.Last().Value); //todo fix here

                                        arrow_parm ar_field_div = new arrow_parm()
                                        {
                                            position_of_arrow = arrow_pos,

                                            direction_of_arrow = arrow_direction_wrt_frameParent_dir,

                                            length_of_arrow = hit1.distance,

                                            arrow_name = arr_name,

                                            pergola_up = offset_up,

                                            //Dotted Line params

                                            Dotted_line_Direction = Pergola_Model.transform.up,

                                            dotted_line_offset = arrow_off,

                                            Dotted_facing_direction = Pergola_Model.transform.right,

                                            orientation_dotted_lines = "horizontal",

                                            txt = txt_display.ToString(),

                                        };

                                        arrows_function(ar_field_div);

                                    }
                                    else
                                    {
                                        Debug.DrawRay(arrow_pos, arrow_direction_wrt_frameParent_dir * 2000, Color.green, 150);
                                    }

                                }
                            }


                            Vector3 pos_of_Raycast = frame_C.transform.TransformPoint(bound_c.center + frame_C.transform.InverseTransformDirection(-Pergola_Model.transform.forward) * Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) / 2);

                            RaycastHit hit_frame_C;

                            float actual_width_of_frame_C = Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) * scale_factor;

                            //Debug.DrawRay(pos_of_Raycast, -Pergola_Model.transform.forward * 3000, Color.blue,150);
                            float size_model_up = Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_c.size)) * scale_factor;
                            float pergola_up_dir_offset = size_model_up / 2 + 200;
                            //Ray ray = new Ray(pos_of_Raycast, - Pergola_Model.transform.forward);
                            //if (frame_E.GetComponent<Collider>().Raycast(ray,out hit_frame_C,Mathf.Infinity))
                            if (Physics.Raycast(pos_of_Raycast, -Pergola_Model.transform.forward, out hit_frame_C, Mathf.Infinity, 1 << DB_script.frame_layer.value))
                            {

                                float length_arrow = hit_frame_C.distance + 2 * actual_width_of_frame_C;
                                arrow_parm ar_field_div = new arrow_parm()
                                {
                                    position_of_arrow = pos_of_Raycast,

                                    direction_of_arrow = -Pergola_Model.transform.forward,

                                    length_of_arrow = length_arrow,

                                    arrow_name = "Arrow_" + frame_C.name,

                                    pergola_up = -pergola_up_dir_offset,

                                    pergola_fwd = actual_width_of_frame_C,

                                    txt = DB_script.frame_D_length.ToString(),

                                    //    //dotted Lines params 
                                    Dotted_line_Direction = Pergola_Model.transform.up,

                                    dotted_line_offset = -(200),

                                    Dotted_facing_direction = Pergola_Model.transform.right,


                                    orientation_dotted_lines = "horizontal",
                                };

                                arrows_function(ar_field_div);
                            }
                            else
                            {
                                Debug.DrawRay(pos_of_Raycast, Pergola_Model.transform.forward * Mathf.Infinity, Color.black, 10);
                            }
                            //Invoke("Scale_fields_Accs_Frame", 0f);
                            //await Task.Delay(300);//100ms delay
                            //Giving Delay after scaling for the gameobjects to appaear with actual scale in hierarchy
                            //await Task.Delay(300);//100ms delay
                            foreach (Transform ch in Pergola_Model.transform)
                            {
                                if (ch.name.Contains(Parents_name.SupportBars_Parent.ToString()))
                                {
                                    ch.gameObject.SetActive(false);
                                }

                                if (ch.name.Contains(Parents_name.Frames_Parent.ToString()))
                                {
                                    //ch.gameObject.SetActive(false);

                                    foreach (Transform sch in ch)
                                    {
                                        if (sch.name.Contains("Frame_Parent"))
                                        {
                                            foreach (Transform scsch in sch)
                                            {

                                                if (!(scsch.name.Contains("FrameE") || scsch.name.Contains("FrameC")))
                                                    scsch.gameObject.SetActive(false);
                                            }
                                        }
                                        else
                                        {
                                            sch.gameObject.SetActive(false);
                                        }
                                    }

                                }

                                if (ch.name.Contains(Parents_name.Divider_Parent.ToString()))
                                {
                                    //ch.gameObject.SetActive(false);

                                    //if(ch.name.Contains("FrameDividers_Parent_Section_0001"))
                                    //{
                                    foreach (Transform sch in ch)
                                    {
                                        if (sch.name.Contains("FrameDividers_Parent"))
                                        {
                                            //sch.gameObject.SetActive(false);
                                            foreach (Transform ssch in sch)
                                            {
                                                if (ssch.name.Contains("Section_0001"))
                                                {
                                                    foreach (Transform FrameDivider_Parent in ssch)
                                                    {
                                                        foreach (Transform fd_parent_ch in FrameDivider_Parent)
                                                        {
                                                            if (!fd_parent_ch.name.Contains("FrameDivider"))
                                                            {
                                                                fd_parent_ch.gameObject.SetActive(false);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    ssch.gameObject.SetActive(false);
                                                }
                                            }
                                        }
                                        else if (sch.name.Contains("FieldDividers_Parent"))
                                        {
                                            foreach (Transform ssch in sch)
                                            {
                                                if (ssch.name.Contains("Section_0001"))
                                                {

                                                    foreach (Transform FieldDivider_Parent in ssch)
                                                    {
                                                        foreach (Transform fd_parent_ch in FieldDivider_Parent)
                                                        {
                                                            if (!fd_parent_ch.name.Contains("FieldDivider"))
                                                            {
                                                                fd_parent_ch.gameObject.SetActive(false);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    ssch.gameObject.SetActive(false);

                                                }
                                            }
                                        }
                                    }
                                    //}
                                    //else if(ch.name.Contains("FrameDividers_Parent_Section_0002"))
                                    //{
                                    //    foreach (Transform sch in ch)
                                    //    {
                                    //        if (sch.name.Contains("Section_0001"))
                                    //        {
                                    //            sch.gameObject.SetActive(false);
                                    //        }
                                    //    }
                                    //}
                                }

                                if (ch.name.Contains(Parents_name.Field_Parent.ToString()))
                                {
                                    foreach (Transform sub_child in ch)
                                    {
                                        if (!sub_child.transform.name.Contains("Field_Parent_Section_0001"))
                                        {
                                            sub_child.gameObject.SetActive(false);
                                        }
                                        else
                                        {
                                            //get_All_children(sub_child);
                                            try
                                            {

                                                List<GameObject> gos = get_All_children_of_Go(sub_child);
                                                //List<GameObject> gos = get_All_children(sub_child);

                                            }
                                            catch (Exception exce)
                                            {

                                                print("Find all children: " + exce);
                                            }
                                            //Transform[] g = sub_child.GetComponentsInChildren<Transform>();
                                            foreach (Transform f_g in sub_child)
                                            {
                                                //if (f_g.name.Contains("Field_Group_0001"))
                                                //{


                                                foreach (Transform acs in f_g)
                                                {
                                                    if (acs.name.Contains("Fields_"))
                                                    {
                                                        acs.gameObject.SetActive(false);
                                                    }
                                                    else
                                                    {
                                                        foreach (Transform comp in acs)
                                                        {
                                                            //Hiding Top L_Accessory 
                                                            if (comp.name.Contains("L_Accessory_top") || acs.name.Contains("ak - 109a"))
                                                                comp.gameObject.SetActive(false);
                                                            else
                                                                comp.gameObject.SetActive(true);
                                                        }
                                                    }
                                                }
                                                //}
                                                //else
                                                //{
                                                //    f_g.gameObject.SetActive(false); ;
                                                //}
                                            }
                                        }
                                    }
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            Debug.Log("While scaling frames, field div and Accessories :" + ex);
                        }


                    }

                    if (view == Views._B_B.ToString())
                    {
                        float scale_factor = 1;//1
                        scale_factor = (int)((DB_script.frame_B_length * Mathf.Pow(10, -3)));


                        if (scale_factor < 1)
                        {
                            scale_factor = 1;
                        }
                        else if (scale_factor > 5)
                        {
                            scale_factor = 5;
                        }
                        try
                        {
                            //await UnityMainThreadDispatcher.DispatchAsync(() => Scale_fields_Accs_Frame(scale_factor));
                            scale_model_factor = scale_factor;
                            Scale__Frame_B_B(scale_factor);
                            await Task.Delay(300);

                            Vector3 pos_of_Raycast = frame_B.transform.TransformPoint(bound_b.center + frame_B.transform.InverseTransformDirection(Pergola_Model.transform.right) * Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) / 2);

                            RaycastHit hit_frame_B;

                            float actual_width_of_frame_B = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) * scale_factor;
                            Debug.Log("B-B :" + frame_B.name + " pos: " + pos_of_Raycast);
                            Debug.DrawRay(pos_of_Raycast, Pergola_Model.transform.right * 3000, Color.blue, 150);
                            float size_model_up = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_b.size)) * scale_factor;
                            float pergola_up_dir_offset = size_model_up / 2 + 200;
                            //Ray ray = new Ray(pos_of_Raycast, - Pergola_Model.transform.right);
                            //if (frame_E.GetComponent<Collider>().Raycast(ray,out hit_frame_B,Mathf.Infinity))
                            if (Physics.Raycast(pos_of_Raycast, Pergola_Model.transform.right, out hit_frame_B, Mathf.Infinity, 1 << DB_script.frame_layer.value))
                            {

                                float length_arrow = hit_frame_B.distance + 2 * actual_width_of_frame_B;
                                arrow_parm ar_field_div = new arrow_parm()
                                {
                                    position_of_arrow = pos_of_Raycast,

                                    direction_of_arrow = Pergola_Model.transform.right,

                                    length_of_arrow = length_arrow,

                                    arrow_name = "Arrow_" + frame_B.name,

                                    pergola_up = -pergola_up_dir_offset,

                                    pergola_fwd = actual_width_of_frame_B,

                                    pergola_right = -actual_width_of_frame_B,

                                    txt = DB_script.frame_F_length.ToString(),

                                    //    //dotted Lines params 
                                    Dotted_line_Direction = Pergola_Model.transform.up,

                                    dotted_line_offset = -(200),

                                    Dotted_facing_direction = Pergola_Model.transform.forward,


                                    orientation_dotted_lines = "horizontal",
                                };

                                arrows_function(ar_field_div);
                            }
                            else
                            {
                                Debug.DrawRay(pos_of_Raycast, Pergola_Model.transform.right * Mathf.Infinity, Color.black, 10);
                            }
                            //Invoke("Scale_fields_Accs_Frame", 0f);
                            //await Task.Delay(300);//100ms delay
                            //Giving Delay after scaling for the gameobjects to appaear with actual scale in hierarchy
                            //await Task.Delay(300);//100ms delay

                            rotateonmouse_script.rotate_FRONT_B_B();

                            foreach (Transform ch in Pergola_Model.transform)
                            {
                                if (ch.name.Contains(Parents_name.SupportBars_Parent.ToString()))
                                {
                                    ch.gameObject.SetActive(false);
                                }

                                if (ch.name.Contains(Parents_name.Frames_Parent.ToString()))
                                {
                                    //ch.gameObject.SetActive(false);

                                    foreach (Transform sch in ch)
                                    {
                                        if (sch.name.Contains("Frame_Parent"))
                                        {
                                            foreach (Transform scsch in sch)
                                            {

                                                if (!(scsch.name.Contains("FrameB") || scsch.name.Contains("FrameD")))
                                                    scsch.gameObject.SetActive(false);
                                            }
                                        }
                                        else
                                        {
                                            sch.gameObject.SetActive(false);
                                        }
                                    }

                                }

                                if (ch.name.Contains(Parents_name.Divider_Parent.ToString()))
                                {
                                    ch.gameObject.SetActive(false);
                                }

                                if (ch.name.Contains(Parents_name.Field_Parent.ToString()))
                                {
                                    ch.gameObject.SetActive(false);
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("While scaling frames, field div and Accessories :" + ex);
                        }

                    }

                    if (view == Views._FIELD_SECTION_2.ToString())
                    {
                        await Task.Delay(500);//100ms delay
                        bool L_Accs = false, U_Accs = false;

                        //float L_width = 1.2f;

                        //float U_width = 2f;

                        float horizontal_width = 0;

                        //executes to all conditions else part doesnt execute
                        if (true|| type_2 )
                        {
                            #region  condition for type 2
                        //    if (GameObject.Find("Fields_1/Field_1") != null)
                        //    {
                        //        GameObject h_field;

                        //        float offest_for_child_ak40 = 0;
                        //        bool ak_40x40 = false;
                        //        if (GameObject.Find("Fields_1/Field_1").transform.GetChild(0).name.Contains("ak - 40"))
                        //        {
                        //            h_field = GameObject.Find("Fields_1/Field_1").transform.GetChild(0).gameObject;
                        //            ak_40x40 = true;

                        //        }
                        //        else
                        //        {
                        //            h_field = GameObject.Find("Field_1");
                        //        }
                        //        //taking mesh from child
                        //        Bounds bound_h_field = GameObject.Find("Field_1").GetComponentInChildren<MeshFilter>().mesh.bounds;

                        //        //scale from child
                        //        horizontal_width = h_field.transform.localScale.x;


                        //        float field_space = horizontal_width;

                        //        //If L accessory is there add the width of L accessory
                        //        if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                        //        {
                        //            field_space = field_space + (2 * L_width);

                        //        }

                        //        if (GameObject.Find("U_Accessory_Left_3") != null)
                        //        {
                        //            field_space = field_space + (2 * U_width);
                        //        }


                        //        float fwd_offset = 0;
                        //        if (ak_40x40 == true)
                        //        {
                        //            fwd_offset = field_space;
                        //        }
                        //        else
                        //        {
                        //            fwd_offset = field_space / 2;
                        //        }

                        //        float offset_right_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                        //        arrow_parm arr_space_params = new arrow_parm()
                        //        {

                        //            position_of_arrow = GameObject.Find("Field_1").transform.TransformPoint(bound_h_field.center),

                        //            direction_of_arrow = -req_right,

                        //            length_of_arrow = field_space,

                        //            arrow_name = "Arrow_Space_" + h_field.name,

                        //            pergola_right = offset_right_field_gap,

                        //            pergola_up = 0,

                        //            pergola_fwd = fwd_offset,

                        //            Dotted_line_Direction = Pergola_Model.transform.right,

                        //            dotted_line_offset = offset_right_field_gap,


                        //        };

                        //        arrows_function(arr_space_params);



                        //    }

                        //    //we need 2 single Arrows here

                        //    if (GameObject.Find("Fields_1") != null)
                        //    {
                        //        int single_Sect_field_count = GameObject.Find("Fields_1").transform.childCount;

                        //        int middle_field_no = single_Sect_field_count / 2;

                        //        string horizontal_field_prefix = "Field_";

                        //        string middle_field_name = horizontal_field_prefix + middle_field_no;

                        //        string next_field_name = horizontal_field_prefix + (middle_field_no + 1).ToString();


                        //        if (GameObject.Find($"Fields_1/{middle_field_name}") != null)
                        //        {
                        //            GameObject middle_field;

                        //            if (GameObject.Find($"Fields_1/{middle_field_name}").transform.GetChild(0).name.Contains("ak - 40"))
                        //            {
                        //                middle_field = GameObject.Find("Fields_1").transform.GetChild(0).GetChild(0).gameObject;
                        //            }
                        //            else
                        //            {
                        //                middle_field = GameObject.Find($"Fields_1/{middle_field_name}");
                        //            }

                        //            Bounds bound_middle_field = GameObject.Find($"Fields_1/{middle_field_name}").transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                        //            Vector3 global_center_of_middle_field = GameObject.Find($"Fields_1/{middle_field_name}").transform.TransformPoint(bound_middle_field.center);


                        //            Vector3 global_pos_of_arrow = global_center_of_middle_field - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_middle_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_C.transform.TransformPoint(bound_c.center)));

                        //            float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                        //            if (length_of_arrow < 110)
                        //            {
                        //                length_of_arrow = 111;
                        //            }

                        //            arrow_parm arr_mid_field = new arrow_parm()
                        //            {
                        //                position_of_arrow = global_pos_of_arrow,

                        //                length_of_arrow = length_of_arrow,

                        //                direction_of_arrow = Pergola_Model.transform.right,

                        //                arrow_name = "Arrow_" + middle_field_name,

                        //                arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                        //                //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                        //                pergola_fwd = -Mathf.Abs(frame_D.transform.localScale.y) - 300,

                        //                pergola_right = -Mathf.Abs(Vector3.Dot(middle_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_middle_field.size)) / 2,

                        //                txt = DB_script.raffaf_spacing_pergola.ToString(),

                        //                txt_right = -length_of_arrow * 3 / 4,
                        //                //txt_fwd = -length_of_arrow * 3 / 4,

                        //                Dotted_line_Direction = Pergola_Model.transform.forward,

                        //                //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                        //                dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)),
                        //                orientation_dotted_lines = "horizontal"

                        //            };

                        //            arrows_function(arr_mid_field);
                        //        }


                        //        if (GameObject.Find($"Fields_1/{next_field_name}") != null)
                        //        {
                        //            GameObject next_field;

                        //            if (GameObject.Find($"Fields_1/{next_field_name}").transform.GetChild(0).name.Contains("ak - 40"))
                        //            {
                        //                next_field = GameObject.Find("Fields_1").transform.GetChild(0).GetChild(0).gameObject;
                        //            }
                        //            else
                        //            {
                        //                next_field = GameObject.Find($"Fields_1/{next_field_name}");
                        //            }

                        //            Bounds bound_next_field = GameObject.Find($"Fields_1/{next_field_name}").transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                        //            Vector3 global_center_of_next_field = GameObject.Find($"Fields_1/{next_field_name}").transform.TransformPoint(bound_next_field.center);


                        //            Vector3 global_pos_of_arrow = global_center_of_next_field - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_next_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_C.transform.TransformPoint(bound_c.center)));

                        //            float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                        //            if (length_of_arrow < 110)
                        //            {
                        //                length_of_arrow = 111;
                        //            }
                        //            arrow_parm arr_next_field = new arrow_parm()
                        //            {
                        //                position_of_arrow = global_pos_of_arrow,

                        //                length_of_arrow = length_of_arrow,

                        //                direction_of_arrow = -Pergola_Model.transform.right,

                        //                arrow_name = "Arrow_" + next_field_name,

                        //                arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                        //                //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                        //                pergola_fwd = -Mathf.Abs(frame_D.transform.localScale.y) - 300,
                        //                pergola_right = Mathf.Abs(Vector3.Dot(next_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_next_field.size)) / 2,

                        //                //***********Dummy text***********//

                        //                txt = dummy_text,//

                        //                Dotted_line_Direction = Pergola_Model.transform.forward,

                        //                //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),

                        //                dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)),

                        //                orientation_dotted_lines = "horizontal"

                        //            };

                        //            arrows_function(arr_next_field);
                        //        }
                        //    }

                        //    rotateonmouse_script.rotate_TOP();
                        //    //rotateonmouse_script.rotate_Raffafa_TOP();

                        //    ////we need 2 single head arrows for Gap from the ground
                        //}
                        //else
                        //{
                            #endregion          
                            int ch_count = GameObject.Find("Field_Parent_Section_0001").transform.childCount;

                            GameObject last_chil_Field_Group_ = GameObject.Find("Field_Parent_Section_0001").transform.GetChild(ch_count-1).gameObject;

                            string last_no = last_chil_Field_Group_.name.Split('_').Last();

                            GameObject last_Fields = GameObject.Find($"Fields_{last_no}");

                            string field_name = last_Fields.transform.GetChild(0).name;

                            string last_field_no = field_name.Split('_').Last();

                            if (GameObject.Find($"Fields_{last_no}/Field_{last_field_no}") != null)
                            {
                                GameObject h_field;

                                float offest_for_child_ak40 = 0;
                                bool ak_40x40 = false;
                                if (GameObject.Find($"Fields_{last_no}/Field_{last_field_no}").transform.GetChild(0).name.Contains("ak - 40"))
                                {
                                    h_field = GameObject.Find($"Fields_{last_no}/Field_{last_field_no}").transform.GetChild(0).gameObject;
                                    ak_40x40 = true;

                                }
                                else
                                {
                                    h_field = GameObject.Find($"Field_{last_field_no}");
                                }
                                //taking mesh from child
                                Bounds bound_h_field = h_field.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                //scale from child
                                horizontal_width = h_field.transform.localScale.x;


                                float field_space = horizontal_width;

                                //If L accessory is there add the width of L accessory
                                if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                                {
                                    field_space = field_space + (2 * L_width);

                                }

                                if (GameObject.Find("U_Accessory_Left_3") != null)
                                {
                                    field_space = field_space + (2 * U_width);
                                }


                                float right_offset = 0;
                                // if (ak_40x40 == true)
                                // {
                                //     right_offset = field_space;
                                // }
                                // else
                                // {
                                    right_offset = field_space / 2;
                                // }

                                // float offset_fwd_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                                
                                 float offset_right_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                                arrow_parm arr_space_params = new arrow_parm()
                                {

                                    // position_of_arrow = GameObject.Find($"Field_{last_field_no}").transform.TransformPoint(bound_h_field.center),

                                    // direction_of_arrow = -req_fwd,

                                    // length_of_arrow = field_space,

                                    // arrow_name = "Arrow_Space_" + h_field.name,

                                    // //pergola_right = offset_right_field_gap,
                                    // pergola_right = right_offset,

                                    // pergola_up = 0,

                                    // pergola_fwd = offset_fwd_field_gap,

                                    // orientation_dotted_lines = "horizontal",

                                    // Dotted_line_Direction = Pergola_Model.transform.forward,

                                    // dotted_line_offset = offset_fwd_field_gap,

                                     position_of_arrow = h_field.transform.TransformPoint(bound_h_field.center),

                                    direction_of_arrow = -req_right,

                                    length_of_arrow = field_space,

                                    arrow_name = "Arrow_Space_" + h_field.name,

                                    pergola_right = offset_right_field_gap,

                                    pergola_up = 0,

                                    pergola_fwd = right_offset,

                                    Dotted_line_Direction = Pergola_Model.transform.right,

                                    dotted_line_offset = offset_right_field_gap,


                                };

                                arrows_function(arr_space_params);

                            }

                            //we need 2 single Arrows here

                            if (GameObject.Find($"Fields_{last_no}") != null)
                            {
                                //string last_fields_name = $"Field_Parent_Section_0001/Field_Group_{last_no}/Fields_{last_no}";

                                //field_name = GameObject.Find($"Fields_{last_no}").transform.GetChild(0).name;

                                //last_field_no = field_name.Split('_').Last();

                                //GameObject last_Fields = GameObject.Find(last_fields_name);

                                GameObject mid_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2).gameObject;

                                int single_Sect_field_count = last_Fields.transform.childCount;




                                int middle_field_no = (int.Parse(last_field_no) + last_Fields.transform.childCount - 1) / 2;

                                string horizontal_field_prefix = "Field_";

                                string middle_field_name = mid_field_in_Last_fields.name;// horizontal_field_prefix + middle_field_no;

                                GameObject next_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2 + 1).gameObject;

                                string next_field_name = next_field_in_Last_fields.name;// horizontal_field_prefix + (middle_field_no + 1).ToString();


                                //next_field_name = (int.Parse(middle_field_name.Split('_').Last()) + 1).ToString();


                                if (mid_field_in_Last_fields != null)
                                {
                                    GameObject middle_field;

                                    if (mid_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        middle_field = mid_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        middle_field = mid_field_in_Last_fields;
                                    }

                                    Bounds bound_middle_field = middle_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_middle_field = middle_field.transform.TransformPoint(bound_middle_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_middle_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_middle_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 150)
                                    {
                                        length_of_arrow = 151;
                                    }



                                    arrow_parm arr_mid_field = new arrow_parm()
                                    {
                                        

                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = Pergola_Model.transform.right,

                                        arrow_name = "Arrow_" + middle_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_fwd = -Mathf.Abs(middle_field.transform.localScale.x)/2-300,//- 300,//-Mathf.Abs(frame_D.transform.localScale.y)

                                        pergola_right = -Mathf.Abs(Vector3.Dot(middle_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_middle_field.size)) / 2,

                                        txt = DB_script.raffaf_spacing_pergola.ToString(),

                                        txt_right = -length_of_arrow * 3 / 4,
                                        //txt_fwd = -length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.forward,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset = -300,//- Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)),
                                        orientation_dotted_lines = "horizontal"

                                    };

                                    arrows_function(arr_mid_field);
                                }


                                if (next_field_in_Last_fields != null)
                                {
                                    GameObject next_field;

                                    if (next_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        next_field = next_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        next_field = next_field_in_Last_fields;
                                    }

                                    Bounds bound_next_field = next_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_next_field = next_field.transform.TransformPoint(bound_next_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_next_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_next_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 110)
                                    {
                                        length_of_arrow = 111;
                                    }
                                    arrow_parm arr_next_field = new arrow_parm()
                                    {
                                        //position_of_arrow = global_pos_of_arrow,

                                        //length_of_arrow = length_of_arrow,

                                        //direction_of_arrow = Pergola_Model.transform.forward,

                                        //arrow_name = "Arrow_" + next_field_name,

                                        //arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        ////pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        //pergola_right = -horizontal_width / 2 - 300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        //pergola_fwd = -Mathf.Abs(Vector3.Dot(next_field.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_next_field.size)) / 2,

                                        ////***********Dummy text***********//

                                        //txt = dummy_text,//

                                        //Dotted_line_Direction = Pergola_Model.transform.right,

                                        ////dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),

                                        //dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),

                                        //orientation_dotted_lines = "vertical"
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = -Pergola_Model.transform.right,

                                        arrow_name = "Arrow_" + next_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_fwd = -Mathf.Abs(next_field.transform.localScale.x)/2-300,// - 300,//-Mathf.Abs(frame_D.transform.localScale.y)
                                        pergola_right = Mathf.Abs(Vector3.Dot(next_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_next_field.size)) / 2,

                                        //***********Dummy text***********//

                                        txt = dummy_text,//

                                        Dotted_line_Direction = Pergola_Model.transform.forward,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),

                                        dotted_line_offset = -300,//- Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)),

                                        orientation_dotted_lines = "horizontal"

                                    };

                                    arrows_function(arr_next_field);
                                }
                            }

                            //we need 2 single head arrows for Gap from the ground
                            if (GameObject.Find($"Fields_{last_no}") != null)
                            {
                                //string last_fields_name = $"Field_Parent_Section_0001/Field_Group_{last_no}/Fields_{last_no}";

                                //field_name = GameObject.Find($"Fields_{last_no}").transform.GetChild(0).name;

                                //last_field_no = field_name.Split('_').Last();

                                //GameObject last_Fields = GameObject.Find(last_fields_name);

                                GameObject first_field_in_Last_fields = last_Fields.transform.GetChild(0).gameObject;

                                int single_Sect_field_count = last_Fields.transform.childCount;




                                //int middle_field_no = (int.Parse(last_field_no) + last_Fields.transform.childCount - 1) / 2;

                                string horizontal_field_prefix = "Field_";

                                string first_field_name = first_field_in_Last_fields.name;// horizontal_field_prefix + middle_field_no;

                                //GameObject next_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2 + 1).gameObject;

                                //string next_field_name = next_field_in_Last_fields.name;// horizontal_field_prefix + (middle_field_no + 1).ToString();


                                //next_field_name = (int.Parse(middle_field_name.Split('_').Last()) + 1).ToString();


                                if (first_field_in_Last_fields != null)
                                {
                                    GameObject first_field;

                                    if (first_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        first_field = first_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        first_field = first_field_in_Last_fields;
                                    }

                                    Bounds bound_first_field = first_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_middle_field = first_field.transform.TransformPoint(bound_first_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_middle_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_middle_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 150)
                                    {
                                        length_of_arrow = 151;
                                    }

                                    Vector3 front_point, back_point;

                                    //we are taking front and back points 

                                    front_point = first_field.transform.TransformPoint(bound_first_field.center + first_field.transform.InverseTransformDirection(Pergola_Model.transform.up) * Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_first_field.center)));
                                                                                                                                                                                                                      
                                    back_point = first_field.transform.TransformPoint(bound_first_field.center + first_field.transform.InverseTransformDirection(-Pergola_Model.transform.up) * Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_first_field.center))); ;// global_center_of_middle_field - Pergola_Model.transform.up * (Vector3.Dot(Pergola_Model.transform.up, global_center_of_middle_field));
                                    float distance_from_floor = 0;
                                    RaycastHit hit_accs_front, hit_accs_back;

                                    Debug.DrawRay(front_point, Pergola_Model.transform.forward * 10000, Color.red, 150);
                                    Debug.DrawRay(back_point, Pergola_Model.transform.forward * 10000, Color.green, 150);
                                    RaycastHit hit_accs;
                                    bool obj_hit = false;

                                    //Here we Raycast Pergola_Model.transform.forward and Find Which Accessory that is hit
                                    if (Physics.Raycast(front_point, Pergola_Model.transform.forward, out hit_accs_front, Mathf.Infinity))
                                    {
                                        obj_hit = true;
                                    }
                                    if (Physics.Raycast(back_point, Pergola_Model.transform.forward, out hit_accs_back, Mathf.Infinity))
                                    {
                                        obj_hit = true;
                                    }
                                    if (obj_hit)
                                    {
                                        if (hit_accs_front.transform.parent.name.Contains("Accessory"))
                                        {
                                            hit_accs = hit_accs_front;
                                        }
                                        else
                                        {
                                            hit_accs = hit_accs_back;
                                        }
                                        if (hit_accs.transform != null)
                                            if (hit_accs.transform.parent != null)
                                            {
                                                if (hit_accs.transform.parent.name.Contains("Accessory"))
                                            {
                                                Debug.Log("Hit object" + hit_accs.transform.parent.name);

                                                Bounds bound_hit_accs = hit_accs.transform.parent.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                                Vector3 tip_of_accs = hit_accs.transform.TransformPoint(bound_hit_accs.center + hit_accs.transform.InverseTransformDirection(Pergola_Model.transform.right) * Mathf.Abs(Vector3.Dot(hit_accs.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_hit_accs.center)));// global_center_of_middle_field - Pergola_Model.transform.up * (Vector3.Dot(Pergola_Model.transform.up, global_center_of_middle_field));

                                                Vector3 tip_of_acc_wrt_Pergola_Model = Pergola_Model.transform.InverseTransformPoint(tip_of_accs);

                                                Vector3 down_point = first_field.transform.TransformPoint(bound_first_field.center + first_field.transform.InverseTransformDirection(Pergola_Model.transform.right) * Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_first_field.center)));

                                                Debug.DrawRay(tip_of_accs, Pergola_Model.transform.forward * 10000, Color.blue, 150);
                                                Debug.DrawRay(down_point, Pergola_Model.transform.forward * 10000, Color.cyan, 150);
                                                Vector3 arrow_1_pos_wrt_Pergola_Model = Pergola_Model.transform.InverseTransformPoint(down_point);
                                                distance_from_floor = Vector3.Dot(Pergola_Model.transform.right, tip_of_accs) - Vector3.Dot(Pergola_Model.transform.right, down_point);
                                                Debug.Log("Distance field sect 1  from ground =" + (tip_of_acc_wrt_Pergola_Model.x - arrow_1_pos_wrt_Pergola_Model.x + " DOT PROD DIST =" + distance_from_floor));
                                            }
                                        }

                                    }
                                    //if arrow length is less


                                    arrow_parm arr_floor_gap_1_field = new arrow_parm()
                                    {
                                        //position_of_arrow = global_pos_of_arrow,

                                        //length_of_arrow = length_of_arrow,

                                        //direction_of_arrow = -Pergola_Model.transform.forward,

                                        //arrow_name = "Arrow_" + middle_field_name,

                                        //arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        ////pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        //pergola_right = -horizontal_width / 2 - 300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),

                                        //pergola_fwd = Mathf.Abs(Vector3.Dot(middle_field.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_middle_field.size)) / 2,

                                        //txt = DB_script.raffaf_spacing_pergola.ToString(),

                                        ////txt_right = -length_of_arrow * 2 * 3 / 4,
                                        //txt_fwd = length_of_arrow * 3 / 4,

                                        //Dotted_line_Direction = Pergola_Model.transform.right,

                                        ////dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        //dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        //orientation_dotted_lines = "vertical"
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = -Pergola_Model.transform.right,

                                        arrow_name = "Arrow_Space1_height" + first_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_fwd = -Mathf.Abs(first_field.transform.localScale.x) / 2 - 300,//- 300,//-Mathf.Abs(frame_D.transform.localScale.y)

                                        pergola_right = Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_first_field.size)) / 2,

                                        txt = Mathf.FloorToInt(distance_from_floor).ToString(),

                                        txt_right = length_of_arrow * 3 / 4,
                                        //txt_fwd = -length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.forward,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset = -300,//- Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)),
                                        orientation_dotted_lines = "horizontal"

                                    };

                                    arrows_function(arr_floor_gap_1_field);

                                    arrow_parm arr_floor_gap_2_field = new arrow_parm()
                                    {
                                        
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = Pergola_Model.transform.right,

                                        arrow_name = "Arrow_Space2_height" + first_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_fwd = -Mathf.Abs(first_field.transform.localScale.x) / 2 - 300,//- 300,//-Mathf.Abs(frame_D.transform.localScale.y)

                                        pergola_right = Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_first_field.size)) / 2+distance_from_floor,

                                        txt = dummy_text,

                                        //txt_right = -length_of_arrow * 3 / 4,
                                        //txt_fwd = -length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.forward,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset = -300,//- Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)),
                                        orientation_dotted_lines = "horizontal"

                                    };

                                    arrows_function(arr_floor_gap_2_field);
                                }


                              
                            }


                            rotateonmouse_script.rotate_PergolaModel(Views._FIELD_SECTION_2.ToString());
                        }
                            foreach (Transform ch in Pergola_Model.transform)
                            {
                                if (ch.name.Contains(Parents_name.SupportBars_Parent.ToString()))
                                {
                                    ch.gameObject.SetActive(false);
                                }

                                if (ch.name.Contains(Parents_name.Frames_Parent.ToString()))
                                {
                                    ch.gameObject.SetActive(false);
                                }

                                if (ch.name.Contains(Parents_name.Divider_Parent.ToString()))
                                {
                                    ch.gameObject.SetActive(false);
                                }

                                if (ch.name.Contains(Parents_name.Field_Parent.ToString()))
                                {
                                    foreach (Transform sub_child in ch)
                                    {
                                        if (!sub_child.transform.name.Contains("Field_Parent_Section_0001"))
                                        {
                                            sub_child.gameObject.SetActive(false);
                                        }
                                        else
                                        {
                                            //get_All_children(sub_child);
                                            //try
                                            //{

                                            //    List<GameObject> gos = get_All_children_of_Go(sub_child);
                                            //    //List<GameObject> gos = get_All_children(sub_child);

                                            //}
                                            //catch (Exception exce)
                                            //{

                                            //    print("Find all children: " + exce);
                                            //}
                                            //Transform[] g = sub_child.GetComponentsInChildren<Transform>();
                                            foreach (Transform f_g in sub_child)
                                            {
                                            int  child_number = 1;
                                            //if (type_2)
                                            //{
                                            //    child_number = 1;
                                            //}
                                            //else
                                            //{
                                                child_number = sub_child.childCount;
                                            //}

                                            //if (f_g.name.Contains("Field_Group_"+child_number))
                                            if (f_g.GetSiblingIndex() == child_number-1)
                                            {

                                                foreach (Transform acs in f_g)
                                                {
                                                    foreach (Transform comp in acs)
                                                    {
                                                        //Hiding Top L_Accessory 
                                                        if (comp.name.Contains("L_Accessory_top") || acs.name.Contains("ak - 109a"))
                                                            comp.gameObject.SetActive(false);
                                                        else
                                                            comp.gameObject.SetActive(true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                f_g.gameObject.SetActive(false); ;
                                            }
                                        }
                                        }
                                    }
                                }

                            }

                    }

                    if (view == Views._SIDE_FIELD_SECTION_2.ToString())
                    {
                        rotateonmouse_script.rotate_PergolaModel(Views._SIDE_FIELD_SECTION_2.ToString());

                        GameObject FrameC = frame_C;

                        Bounds bound_frameC = FrameC.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                        GameObject arr_pfb = null;
                        if (true||type_2)
                        {
                            //Changing prefab as we need space left of side screen shot
                            if ((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_space_side") != null)
                            {
                                //arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                                arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_space_side");
                            }

                            float width_of_frame = Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_d.size));

                            //as we are using length of frame we must subtract it 2 times by width 
                            float len_arrow = FrameC.transform.localScale.y - 2 * width_of_frame;

                            // length of accessory is -5 of the divider lengths
                            len_arrow = len_arrow - assembly_tolerance;


                            float up_dir_offset_L_acc_arrow = Mathf.Abs(Vector3.Dot(FrameC.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_d.size)) + 300;

                            arrow_parm arrow_L_Accs = new arrow_parm()
                            {

                                position_of_arrow = FrameC.transform.TransformPoint(bound_frameC.center),


                                direction_of_arrow = -Pergola_Model.transform.right,

                                length_of_arrow = len_arrow,

                                arrow_name = "Arrow_" + FrameC.name,

                                pergola_fwd = 0,

                                pergola_right = len_arrow / 2,

                                pergola_up = up_dir_offset_L_acc_arrow,

                                arrow_prefab = arr_pfb,

                                Dotted_line_Direction = Pergola_Model.transform.up,

                                dotted_line_offset = up_dir_offset_L_acc_arrow,

                                orientation_dotted_lines = "vertical",

                                Dotted_facing_direction = Pergola_Model.transform.forward


                            };
                            arrows_function(arrow_L_Accs);


                            //Extra arrow
                            if ((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_Invisible_Text") != null)
                            {
                                //Changing back to previous prefab
                                //arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                                arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_Invisible_Text");
                            }

                            //float L_width = 1.2f;

                            //float U_width = 2f;

                            float horizontal_width = 0;

                            //new code
                            int ch_count = GameObject.Find("Field_Parent_Section_0001").transform.childCount;

                            GameObject last_chil_Field_Group_ = GameObject.Find("Field_Parent_Section_0001").transform.GetChild(ch_count-1).gameObject;

                            string last_no = last_chil_Field_Group_.name.Split('_').Last();

                            GameObject last_Fields = GameObject.Find($"Fields_{last_no}");

                            string field_name = last_Fields.transform.GetChild(0).name;

                            string last_field_no = field_name.Split('_').Last();

                            if (GameObject.Find($"Fields_{last_no}/Field_{last_field_no}") != null)
                            {
                                GameObject h_field;

                                float offest_for_child_ak40 = 0;
                                bool ak_40x40 = false;
                                if (GameObject.Find($"Fields_{last_no}/Field_{last_field_no}").transform.GetChild(0).name.Contains("ak - 40"))
                                {
                                    h_field = GameObject.Find($"Fields_{last_no}/Field_{last_field_no}").transform.GetChild(0).gameObject;
                                    ak_40x40 = true;

                                }
                                else
                                {
                                    h_field = GameObject.Find($"Field_{last_field_no}");
                                }
                                //taking mesh from child
                                Bounds bound_h_field = h_field.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                //scale from child
                                horizontal_width = h_field.transform.localScale.x;


                                float field_space = horizontal_width;

                                //If L accessory is there add the width of L accessory
                                if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                                {
                                    field_space = field_space + (2 * L_width);

                                }

                                if (GameObject.Find("U_Accessory_Left_3") != null)
                                {
                                    field_space = field_space + (2 * U_width);
                                }


                                float right_offset = 0;
                                // if (ak_40x40 == true)
                                // {
                                //     right_offset = field_space;
                                // }
                                // else
                                // {
                                    right_offset = field_space / 2;
                                // }

                                // float offset_fwd_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;

                                float offset_right_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                                arrow_parm arr_space_params = new arrow_parm()
                                {

                                    // position_of_arrow = GameObject.Find($"Field_{last_field_no}").transform.TransformPoint(bound_h_field.center),

                                    // direction_of_arrow = -req_fwd,

                                    // length_of_arrow = field_space,

                                    // arrow_name = "Arrow_Space_" + h_field.name,

                                    // //pergola_right = offset_right_field_gap,
                                    // pergola_right = right_offset,

                                    // pergola_up = 0,

                                    // pergola_fwd = offset_fwd_field_gap,

                                    // orientation_dotted_lines = "horizontal",

                                    // Dotted_line_Direction = Pergola_Model.transform.forward,

                                    // dotted_line_offset = offset_fwd_field_gap,

                                    position_of_arrow = h_field.transform.TransformPoint(bound_h_field.center),

                                    direction_of_arrow = -req_right,

                                    length_of_arrow = field_space,

                                    arrow_name = "Arrow_Space_" + h_field.name,

                                    pergola_right = offset_right_field_gap,

                                    pergola_up = 0,

                                    pergola_fwd = right_offset,

                                    arrow_prefab=arr_pfb,

                                    Dotted_line_Direction = Pergola_Model.transform.right,

                                    dotted_line_offset = offset_right_field_gap,


                                };

                                //this Arrow is destroyed in ScreenShot script
                                Arrow_Side_Destroy_field_width = arrows_function(arr_space_params);

                            }
                        }
                        else
                        {
                            //Changing prefab as we need space left of side screen shot
                            if ((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_space_side") != null)
                            {
                                //arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                                arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_space_side");
                            }

                            float width_of_frame = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size));

                            //as we are using length of frame we must subtract it 2 times by width 
                            float len_arrow = frame_B.transform.localScale.y - 2 * width_of_frame;

                            // length of accessory is -5 of the divider lengths
                            len_arrow = len_arrow - assembly_tolerance;


                            float up_dir_offset_L_acc_arrow = Mathf.Abs(Vector3.Dot(frame_B.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_b.size)) + 300;

                            arrow_parm arrow_L_Accs = new arrow_parm()
                            {

                                position_of_arrow = frame_B.transform.TransformPoint(bound_frameC.center),


                                direction_of_arrow = -Pergola_Model.transform.forward,

                                length_of_arrow = len_arrow,

                                arrow_name = "Arrow_" + frame_B.name,

                                //pergola_fwd = 0,

                                pergola_fwd = len_arrow / 2,

                                pergola_up = up_dir_offset_L_acc_arrow,

                                arrow_prefab = arr_pfb,

                                Dotted_line_Direction = Pergola_Model.transform.up,

                                dotted_line_offset = up_dir_offset_L_acc_arrow,

                                orientation_dotted_lines = "vertical",

                                Dotted_facing_direction = Pergola_Model.transform.right


                            };
                            arrows_function(arrow_L_Accs);


                            //Extra arrow
                            if ((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_Invisible_Text") != null)
                            {
                                //Changing back to previous prefab
                                //arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                                arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_Invisible_Text");
                            }

                            //float L_width = 1.2f;

                            //float U_width = 2f;

                            float horizontal_width = 0;

                            int ch_count = GameObject.Find("Field_Parent_Section_0001").transform.childCount;

                            GameObject last_chil_Field_Group_ = GameObject.Find("Field_Parent_Section_0001").transform.GetChild(ch_count - 1).gameObject;

                            string last_no = last_chil_Field_Group_.name.Split('_').Last();

                            GameObject last_Fields = GameObject.Find($"Fields_{last_no}");

                            string field_name = last_Fields.transform.GetChild(last_Fields.transform.childCount - 1).name;

                            string last_field_no = field_name.Split('_').Last();

                            if (GameObject.Find($"Fields_{last_no}/Field_{last_field_no}") != null)
                            {
                                GameObject h_field;

                                float offest_for_child_ak40 = 0;
                                bool ak_40x40 = false;
                                if (GameObject.Find($"Fields_{last_no}/Field_{last_field_no}").transform.GetChild(0).name.Contains("ak - 40"))
                                {
                                    h_field = GameObject.Find($"Fields_{last_no}/Field_{last_field_no}").transform.GetChild(0).gameObject;
                                    ak_40x40 = true;

                                }
                                else
                                {
                                    h_field = GameObject.Find($"Field_{last_field_no}");
                                }
                                //taking mesh from child
                                Bounds bound_h_field = h_field.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                //scale from child
                                horizontal_width = h_field.transform.localScale.x;


                                float field_space = horizontal_width;

                                //If L accessory is there add the width of L accessory
                                if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                                {
                                    field_space = field_space + (2 * L_width);

                                }

                                if (GameObject.Find("U_Accessory_Left_3") != null)
                                {
                                    field_space = field_space + (2 * U_width);
                                }


                                float right_offset = 0;
                                // if (ak_40x40 == true)
                                // {
                                //     right_offset = field_space;
                                // }
                                // else
                                // {
                                    right_offset = field_space / 2;
                                // }

                                float offset_fwd_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                                arrow_parm arr_space_params = new arrow_parm()
                                {

                                    position_of_arrow = h_field.transform.TransformPoint(bound_h_field.center),

                                    direction_of_arrow = -req_right,

                                    length_of_arrow = field_space,

                                    arrow_name = "Arrow_Space_" + h_field.name,

                                    //pergola_right = offset_right_field_gap,
                                    pergola_right = right_offset,

                                    pergola_up = 0,

                                    pergola_fwd = offset_fwd_field_gap,

                                    orientation_dotted_lines = "horizontal",

                                    Dotted_line_Direction = Pergola_Model.transform.forward,

                                    dotted_line_offset = offset_fwd_field_gap,

                                    arrow_prefab=arr_pfb,

                                };

                                arrows_function(arr_space_params);

                            }

                        }
                            foreach (Transform ch in Pergola_Model.transform)
                            {
                                if (ch.name.Contains(Parents_name.SupportBars_Parent.ToString()))
                                {
                                    ch.gameObject.SetActive(false);
                                }

                                if (ch.name.Contains(Parents_name.Frames_Parent.ToString()))
                                {
                                    ch.gameObject.SetActive(false);
                                }

                                if (ch.name.Contains(Parents_name.Divider_Parent.ToString()))
                                {
                                    ch.gameObject.SetActive(false);
                                }

                                if (ch.name.Contains(Parents_name.Field_Parent.ToString()))
                                {
                                    foreach (Transform sub_child in ch)
                                    {
                                        if (!sub_child.transform.name.Contains("Field_Parent_Section_0001"))
                                        {
                                            sub_child.gameObject.SetActive(false);
                                        }
                                        else
                                        {
                                        //get_All_children(sub_child);
                                        //try
                                        //{

                                        //    List<GameObject> gos = get_All_children_of_Go(sub_child);
                                        //    //List<GameObject> gos = get_All_children(sub_child);

                                        //}
                                        //catch (Exception exce)
                                        //{

                                        //    print("Find all children: " + exce);
                                        //}
                                        //Transform[] g = sub_child.GetComponentsInChildren<Transform>();

                                        int child_number = 1;
                                        //if (type_2)
                                        //{
                                        //    child_number = 1;
                                        //}
                                        //else
                                        //{
                                            child_number = sub_child.childCount;
                                        //}

                                        foreach (Transform f_g in sub_child)
                                        {
                                            //if (f_g.name.Contains("Field_Group_" + child_number))
                                            if (f_g.GetSiblingIndex()==child_number-1)
                                            {
                                                foreach (Transform acs in f_g)
                                                {
                                                    foreach (Transform comp in acs)
                                                    {
                                                        //Hiding Top L_Accessory 
                                                        if ((comp.name.Contains("L_Accessory_")&&comp.name.ToLower().Contains( "top")) || acs.name.Contains("ak - 109a"))
                                                            comp.gameObject.SetActive(false);
                                                        else
                                                            comp.gameObject.SetActive(true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                f_g.gameObject.SetActive(false);
                                            }
                                        }
                                        }
                                    }
                                }

                            }


                    }

                    if (view == Views._FIELD_SECTION_3.ToString())
                    {
                        await Task.Delay(500);//100ms delay
                        bool L_Accs = false, U_Accs = false;

                        //float L_width = 1.2f;

                        //float U_width = 2f;

                        float horizontal_width = 0;
                        if (true||type_2)
                        {

                            int ch_count = GameObject.Find("Field_Parent_Section_0002").transform.childCount;

                            GameObject last_chil_Field_Group_ = GameObject.Find("Field_Parent_Section_0002").transform.GetChild(ch_count - 1).gameObject;

                            string last_no = last_chil_Field_Group_.name.Split('_').Last();

                            GameObject Fields=null;

                            //to finds Fields bw accessory,fields group
                            foreach(Transform t in last_chil_Field_Group_.transform)
                            {
                                if(t.name.Contains("Fields"))
                                {
                                Fields=t.gameObject;
                                }
                            }

                            //{Fields.name} with Fields_{last_no}
                            string field_name = Fields.transform.GetChild(0).name;//GameObject.Find($"Fields_{last_no}").transform.GetChild(0).name;

                            string first_field_no = field_name.Split('_').Last();

                            if (GameObject.Find($"{Fields.name}/Field_{first_field_no}") != null)
                            {
                                GameObject h_field;

                                float offest_for_child_ak40 = 0;
                                bool ak_40x40 = false;
                                if (GameObject.Find($"{Fields.name}/Field_{first_field_no}").transform.GetChild(0).name.Contains("ak - 40"))
                                {
                                    h_field = GameObject.Find($"{Fields.name}/Field_{first_field_no}").transform.GetChild(0).gameObject;
                                    ak_40x40 = true;

                                }
                                else
                                {
                                    h_field = GameObject.Find($"Field_{first_field_no}");
                                }
                                //taking mesh from child
                                Bounds bound_h_field = h_field.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                //scale from child
                                horizontal_width =Mathf.Abs( h_field.transform.localScale.x);


                                float field_space = horizontal_width;

                                //If L accessory is there add the width of L accessory
                                if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                                {
                                    field_space = field_space + (2 * L_width);

                                }

                                if (GameObject.Find("U_Accessory_Left_3") != null)
                                {
                                    field_space = field_space + (2 * U_width);
                                }


                                float right_offset = 0;
                                // if (ak_40x40 == true)
                                // {
                                //     right_offset = field_space;
                                // }
                                // else
                                // {
                                    right_offset = field_space / 2;
                                // }

                                float offset_fwd_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                                arrow_parm arr_space_params = new arrow_parm()
                                {

                                    position_of_arrow = h_field.transform.TransformPoint(bound_h_field.center),

                                    direction_of_arrow = -req_fwd,

                                    length_of_arrow = field_space,

                                    arrow_name = "Arrow_Space_" + h_field.name,

                                    //pergola_right = offset_right_field_gap,
                                    pergola_right = right_offset,

                                    pergola_up = 0,

                                    pergola_fwd = -offset_fwd_field_gap,

                                    orientation_dotted_lines = "horizontal",

                                    Dotted_line_Direction = Pergola_Model.transform.forward,

                                    dotted_line_offset = -offset_fwd_field_gap,


                                };

                                arrows_function(arr_space_params);

                            }

                            //we need 2 single Arrows here


                            if (GameObject.Find($"{Fields.name}") != null)
                            {
                                string last_fields_name = $"Field_Parent_Section_0002/Field_Group_{last_no}/{Fields.name}";

                                field_name = GameObject.Find($"{Fields.name}").transform.GetChild(0).name;

                                first_field_no = field_name.Split('_').Last();

                                GameObject last_Fields = GameObject.Find(last_fields_name);

                                GameObject mid_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2).gameObject;

                                int single_Sect_field_count = last_Fields.transform.childCount;




                                int middle_field_no = (int.Parse(first_field_no) + last_Fields.transform.childCount - 1) / 2;

                                string horizontal_field_prefix = "Field_";

                                string middle_field_name = mid_field_in_Last_fields.name;// horizontal_field_prefix + middle_field_no;

                                GameObject next_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2 + 1).gameObject;

                                string next_field_name = next_field_in_Last_fields.name;// horizontal_field_prefix + (middle_field_no + 1).ToString();


                                //next_field_name = (int.Parse(middle_field_name.Split('_').Last()) + 1).ToString();


                                if (mid_field_in_Last_fields != null)
                                {
                                    GameObject middle_field;

                                    if (mid_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        middle_field = mid_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        middle_field = mid_field_in_Last_fields;
                                    }

                                    Bounds bound_middle_field = middle_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_middle_field = middle_field.transform.TransformPoint(bound_middle_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_middle_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_middle_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 150)
                                    {
                                        length_of_arrow = 151;
                                    }



                                    arrow_parm arr_mid_field = new arrow_parm()
                                    {
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = -Pergola_Model.transform.forward,

                                        arrow_name = "Arrow_" + middle_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_right = -horizontal_width / 2 - 300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),

                                        pergola_fwd = Mathf.Abs(Vector3.Dot(middle_field.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_middle_field.size)) / 2,

                                        txt = DB_script.raffaf_spacing_pergola.ToString(),

                                        //txt_right = -length_of_arrow * 2 * 3 / 4,
                                        txt_fwd = length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.right,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        orientation_dotted_lines = "vertical"

                                    };

                                    arrows_function(arr_mid_field);
                                }


                                if (next_field_in_Last_fields != null)
                                {
                                    GameObject next_field;

                                    if (next_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        next_field = next_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        next_field = next_field_in_Last_fields;
                                    }

                                    Bounds bound_next_field = next_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_next_field = next_field.transform.TransformPoint(bound_next_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_next_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_next_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 110)
                                    {
                                        length_of_arrow = 111;
                                    }
                                    arrow_parm arr_next_field = new arrow_parm()
                                    {
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = Pergola_Model.transform.forward,

                                        arrow_name = "Arrow_" + next_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_right = -horizontal_width / 2 - 300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        pergola_fwd = -Mathf.Abs(Vector3.Dot(next_field.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_next_field.size)) / 2,

                                        //***********Dummy text***********//

                                        txt = dummy_text,//

                                        Dotted_line_Direction = Pergola_Model.transform.right,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),

                                        dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),

                                        orientation_dotted_lines = "vertical"

                                    };

                                    arrows_function(arr_next_field);
                                }
                            }

                            //we need 2 single head arrows for Gap from the ground

                            if (GameObject.Find($"{Fields.name}") != null)
                            {
                                string last_fields_name = $"Field_Parent_Section_0002/Field_Group_{last_no}/{Fields.name}";

                                field_name = GameObject.Find($"{Fields.name}").transform.GetChild(0).name;

                                first_field_no = field_name.Split('_').Last();

                                GameObject last_Fields = GameObject.Find(last_fields_name);

                                GameObject first_field_in_Last_fields = last_Fields.transform.GetChild(0).gameObject;

                                int single_Sect_field_count = last_Fields.transform.childCount;




                                //int middle_field_no = (int.Parse(first_field_no) + last_Fields.transform.childCount - 1) / 2;

                                //string horizontal_field_prefix = "Field_";

                                string first_field_name = first_field_in_Last_fields.name;// horizontal_field_prefix + middle_field_no;

                                //GameObject next_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2 + 1).gameObject;

                                //string next_field_name = next_field_in_Last_fields.name;// horizontal_field_prefix + (middle_field_no + 1).ToString();


                                //next_field_name = (int.Parse(middle_field_name.Split('_').Last()) + 1).ToString();



                                if (first_field_in_Last_fields != null)
                                {
                                    GameObject first_field;

                                    if (first_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        first_field = first_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        first_field = first_field_in_Last_fields;
                                    }

                                    Bounds bound_first_field = first_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_middle_field = first_field.transform.TransformPoint(bound_first_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_middle_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_middle_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 150)
                                    {
                                        length_of_arrow = 151;
                                    }

                                    Vector3 front_point, back_point;

                                    front_point = first_field.transform.TransformPoint(bound_first_field.center + first_field.transform.InverseTransformDirection(Pergola_Model.transform.up) * Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_first_field.center)));

                                    back_point = first_field.transform.TransformPoint(bound_first_field.center + first_field.transform.InverseTransformDirection(-Pergola_Model.transform.up) * Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_first_field.center))); ;// global_center_of_middle_field - Pergola_Model.transform.up * (Vector3.Dot(Pergola_Model.transform.up, global_center_of_middle_field));
                                    float distance_from_floor = 0;
                                    RaycastHit hit_accs_front, hit_accs_back;
                                    RaycastHit hit_accs;

                                    Debug.DrawRay(front_point, Pergola_Model.transform.right * 10000, Color.red, 150);
                                    Debug.DrawRay(back_point, Pergola_Model.transform.right * 10000, Color.green, 150);

                                    bool obj_hit = false;

                                    //Here we Raycast Pergola_Model.transform.forward and Find Which Accessory that is hit
                                    if (Physics.Raycast(front_point, Pergola_Model.transform.right, out hit_accs_front, Mathf.Infinity))
                                    {
                                        obj_hit = true;
                                    }
                                    if (Physics.Raycast(back_point, Pergola_Model.transform.right, out hit_accs_back, Mathf.Infinity))
                                    {
                                        obj_hit = true;
                                    }
                                    if (obj_hit)
                                    {
                                        if (hit_accs_front.transform.parent.name.Contains("Accessory"))
                                        {
                                            hit_accs = hit_accs_front;
                                        }
                                        else
                                        {
                                            hit_accs = hit_accs_back;
                                        }
                                        //}

                                        //    if (Physics.Raycast(front_point, Pergola_Model.transform.right, out hit_accs, Mathf.Infinity) || Physics.Raycast(back_point, Pergola_Model.transform.right, out hit_accs, Mathf.Infinity))
                                        //{
                                        if (hit_accs.transform.parent != null)
                                        {
                                            if (hit_accs.transform.parent.name.Contains("Accessory"))
                                            {
                                                Debug.Log("Hit object" + hit_accs.transform.parent.name);

                                                Bounds bound_hit_accs = hit_accs.transform.parent.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                                Vector3 tip_of_accs = hit_accs.transform.TransformPoint(bound_hit_accs.center + hit_accs.transform.InverseTransformDirection(-Pergola_Model.transform.forward) * Mathf.Abs(Vector3.Dot(hit_accs.transform.InverseTransformDirection(-Pergola_Model.transform.forward), bound_hit_accs.center)));// global_center_of_middle_field - Pergola_Model.transform.up * (Vector3.Dot(Pergola_Model.transform.up, global_center_of_middle_field));

                                                Vector3 tip_of_acc_wrt_Pergola_Model = Pergola_Model.transform.InverseTransformPoint(tip_of_accs);

                                                Vector3 down_point = first_field.transform.TransformPoint(bound_first_field.center + first_field.transform.InverseTransformDirection(-Pergola_Model.transform.forward) * Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(-Pergola_Model.transform.forward), bound_first_field.center)));

                                                Debug.DrawRay(tip_of_accs, Pergola_Model.transform.right* 10000, Color.blue, 150);
                                                Debug.DrawRay(down_point, Pergola_Model.transform.right * 10000, Color.cyan, 150);
                                                Vector3 arrow_1_pos_wrt_Pergola_Model = Pergola_Model.transform.InverseTransformPoint(down_point);
                                                distance_from_floor =Mathf.Abs( Vector3.Dot(Pergola_Model.transform.forward, tip_of_accs) - Vector3.Dot(Pergola_Model.transform.forward, down_point));
                                                Debug.Log("Distance field sect 2  from ground =" + (tip_of_acc_wrt_Pergola_Model.z - arrow_1_pos_wrt_Pergola_Model.z + " DOT PROD DIST =" + distance_from_floor));
                                            }
                                        }

                                    }
                                    //if arrow length is less

                                    arrow_parm arr_space1_field = new arrow_parm()
                                    {
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = Pergola_Model.transform.forward,

                                        arrow_name = "Arrow_Space1_height" + first_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_right = -horizontal_width / 2 - 300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),

                                        pergola_fwd = -Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_first_field.size)) / 2,

                                        txt = Mathf.FloorToInt(distance_from_floor).ToString(),

                                        //txt_right = -length_of_arrow * 2 * 3 / 4,
                                        txt_fwd = length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.right,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        orientation_dotted_lines = "vertical"

                                    };

                                    arrows_function(arr_space1_field);

                                    arrow_parm arr_space2_field = new arrow_parm()
                                    {
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = -Pergola_Model.transform.forward,

                                        arrow_name = "Arrow_Space2_height" + first_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_right = -horizontal_width / 2 - 300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),

                                        pergola_fwd = -Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_first_field.size)) / 2-distance_from_floor,

                                        txt = dummy_text,

                                        //txt_right = -length_of_arrow * 2 * 3 / 4,
                                        txt_fwd = length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.right,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        orientation_dotted_lines = "vertical"

                                    };

                                    arrows_function(arr_space2_field);
                                }


                              
                            }

                            //rotateonmouse_script.rotate_Raffafa_TOP();
                        }
                        else
                        {
                            //int ch_count = GameObject.Find("Field_Parent_Section_0002").transform.childCount;

                            GameObject first_child_Field_Group_ = GameObject.Find("Field_Parent_Section_0002").transform.GetChild(0).gameObject;

                            string last_no = first_child_Field_Group_.name.Split('_').Last();

                            GameObject Field_ = GameObject.Find($"Fields_{last_no}");

                            string field_name = Field_.transform.GetChild(Field_.transform.childCount-1).name;

                            string first_field_no = field_name.Split('_').Last();

                            if (GameObject.Find($"Fields_{last_no}/Field_{first_field_no}") != null)
                            {
                                GameObject h_field;

                                float offest_for_child_ak40 = 0;
                                bool ak_40x40 = false;
                                if (GameObject.Find($"Fields_{last_no}/Field_{first_field_no}").transform.GetChild(0).name.Contains("ak - 40"))
                                {
                                    h_field = GameObject.Find($"Fields_{last_no}/Field_{first_field_no}").transform.GetChild(0).gameObject;
                                    ak_40x40 = true;

                                }
                                else
                                {
                                    h_field = GameObject.Find($"Field_{first_field_no}");
                                }
                                //taking mesh from child
                                Bounds bound_h_field = h_field.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                //scale from child
                                horizontal_width =Mathf.Abs( h_field.transform.localScale.x);


                                float field_space = horizontal_width;

                                //If L accessory is there add the width of L accessory
                                if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                                {
                                    field_space = field_space + (2 * L_width);

                                }

                                if (GameObject.Find("U_Accessory_Left_3") != null)
                                {
                                    field_space = field_space + (2 * U_width);
                                }


                                float fwd_offset = 0;
                                // if (ak_40x40 == true)
                                // {
                                //     fwd_offset = field_space;
                                // }
                                // else
                                // {
                                    fwd_offset = field_space / 2;
                                // }

                                float offset_right_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                                arrow_parm arr_space_params = new arrow_parm()
                                {

                                    position_of_arrow = h_field.transform.TransformPoint(bound_h_field.center),

                                    direction_of_arrow = Pergola_Model.transform.forward,

                                    length_of_arrow = field_space,

                                    arrow_name = "Arrow_Space_" + h_field.name,

                                    //pergola_right = offset_right_field_gap,
                                    pergola_fwd = -fwd_offset,
                                    

                                    pergola_up = 0,

                                    pergola_right = -offset_right_field_gap,

                                    orientation_dotted_lines = "vertical",

                                    Dotted_line_Direction = Pergola_Model.transform.right,

                                    dotted_line_offset = -offset_right_field_gap,


                                };

                                arrows_function(arr_space_params);

                            }

                            //we need 2 single Arrows here

                            if (GameObject.Find($"Fields_{last_no}") != null)
                            {
                                string last_fields_name = $"Field_Parent_Section_0002/Field_Group_{last_no}/Fields_{last_no}";

                                field_name = GameObject.Find($"Fields_{last_no}").transform.GetChild(0).name;

                                first_field_no = field_name.Split('_').Last();

                                GameObject last_Fields = GameObject.Find(last_fields_name);

                                GameObject mid_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2).gameObject;

                                int single_Sect_field_count = last_Fields.transform.childCount;




                                int middle_field_no = (int.Parse(first_field_no) + last_Fields.transform.childCount - 1) / 2;

                                string horizontal_field_prefix = "Field_";

                                string middle_field_name = mid_field_in_Last_fields.name;// horizontal_field_prefix + middle_field_no;

                                GameObject next_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2 + 1).gameObject;

                                string next_field_name = next_field_in_Last_fields.name;// horizontal_field_prefix + (middle_field_no + 1).ToString();


                                //next_field_name = (int.Parse(middle_field_name.Split('_').Last()) + 1).ToString();


                                if (mid_field_in_Last_fields != null)
                                {
                                    GameObject middle_field;

                                    if (mid_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        middle_field = mid_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        middle_field = mid_field_in_Last_fields;
                                    }

                                    Bounds bound_middle_field = middle_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_middle_field = middle_field.transform.TransformPoint(bound_middle_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_middle_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_middle_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 110)
                                    {
                                        length_of_arrow = 111;
                                    }



                                    arrow_parm arr_mid_field = new arrow_parm()
                                    {
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = Pergola_Model.transform.right,

                                        arrow_name = "Arrow_" + middle_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        //pergola_right = -horizontal_width / 2 - 300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),

                                        pergola_right = -Mathf.Abs(Vector3.Dot(middle_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_middle_field.size)) / 2,
                                        pergola_fwd = Mathf.Abs(middle_field.transform.localScale.x)/2+300,
                                        txt = DB_script.raffaf_spacing_pergola.ToString(),

                                        //txt_right = -length_of_arrow * 2 * 3 / 4,
                                        txt_fwd = length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.forward,

                                        dotted_line_offset=300,
                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        //dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        orientation_dotted_lines = "horizontal"

                                    };

                                    arrows_function(arr_mid_field);
                                }


                                if (next_field_in_Last_fields != null)
                                {
                                    GameObject next_field;

                                    if (next_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        next_field = next_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        next_field = next_field_in_Last_fields;
                                    }

                                    Bounds bound_next_field = next_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_next_field = next_field.transform.TransformPoint(bound_next_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_next_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_next_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 110)
                                    {
                                        length_of_arrow = 111;
                                    }
                                    arrow_parm arr_next_field = new arrow_parm()
                                    {
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = -Pergola_Model.transform.right,

                                        arrow_name = "Arrow_" + next_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        pergola_fwd = Mathf.Abs(next_field.transform.localScale.x)/2+300,
                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        //pergola_right = -horizontal_width / 2 - 300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        pergola_right = Mathf.Abs(Vector3.Dot(next_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_next_field.size)) / 2,

                                        //***********Dummy text***********//

                                        txt = dummy_text,//

                                        Dotted_line_Direction = Pergola_Model.transform.forward,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset=300,
                                        //dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),

                                        orientation_dotted_lines = "horizontal"

                                    };

                                    arrows_function(arr_next_field);
                                }
                            }
                        }
                        rotateonmouse_script.rotate_PergolaModel(Views._FIELD_SECTION_3.ToString());
                        foreach (Transform ch in Pergola_Model.transform)
                        {
                            if (ch.name.Contains(Parents_name.SupportBars_Parent.ToString()))
                            {
                                ch.gameObject.SetActive(false);
                            }

                            if (ch.name.Contains(Parents_name.Frames_Parent.ToString()))
                            {
                                ch.gameObject.SetActive(false);
                            }

                            if (ch.name.Contains(Parents_name.Divider_Parent.ToString()))
                            {
                                ch.gameObject.SetActive(false);
                            }

                            if (ch.name.Contains(Parents_name.Field_Parent.ToString()))
                            {
                                foreach (Transform sub_child in ch)
                                {
                                    if (!sub_child.transform.name.Contains("Field_Parent_Section_0002"))
                                    {
                                        sub_child.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        //get_All_children(sub_child);
                                        //try
                                        //{

                                        //    List<GameObject> gos = get_All_children_of_Go(sub_child);
                                        //    //List<GameObject> gos = get_All_children(sub_child);

                                        //}
                                        //catch (Exception exce)
                                        //{

                                        //    print("Find all children: " + exce);
                                        //}
                                        //Transform[] g = sub_child.GetComponentsInChildren<Transform>();

                                        int child_number = 1;
                                        //if (type_2)
                                        //{
                                            child_number = sub_child.childCount;
                                        //}
                                        //else
                                        //{
                                        //    child_number = sub_child.childCount;
                                        //    //child_number = 1;
                                        //}
                                        foreach (Transform f_g in sub_child)
                                        {
                                            if (f_g.GetSiblingIndex() == child_number- 1)
                                            {

                                                foreach (Transform acs in f_g)
                                                {
                                                    foreach (Transform comp in acs)
                                                    {
                                                        //Hiding Top L_Accessory 
                                                        if ((comp.name.Contains("L_Accessory_") && comp.name.ToLower().Contains("top")) || acs.name.Contains("ak - 109a"))
                                                            comp.gameObject.SetActive(false);
                                                        else
                                                            comp.gameObject.SetActive(true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                f_g.gameObject.SetActive(false); ;
                                            }
                                        }
                                    }
                                }
                            }

                        }

                    }

                    if (view == Views._SIDE_FIELD_SECTION_3.ToString())
                    {
                        rotateonmouse_script.rotate_PergolaModel(Views._SIDE_FIELD_SECTION_3.ToString());

                        GameObject FrameF = frame_F;

                        Bounds bound_L_Accs_1 = FrameF.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                        GameObject arr_pfb = null;

                        //Changing prefab as we need space left of side screen shot
                        if ((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_space_side") != null)
                        {
                            //arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                            arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_space_side");
                        }

                        float width_of_frame = Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_f.size));

                        //as we are using length of frame we must subtract it 2 times by width 
                        float len_arrow = FrameF.transform.localScale.y - 2 * width_of_frame;

                        // length of accessory is -5 of the divider lengths
                        len_arrow = len_arrow -assembly_tolerance;


                        float up_dir_offset_L_acc_arrow = Mathf.Abs(Vector3.Dot(FrameF.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_f.size)) + 300;

                        arrow_parm arrow_L_Accs = new arrow_parm()
                        {

                            position_of_arrow = FrameF.transform.TransformPoint(bound_L_Accs_1.center),


                            direction_of_arrow = -Pergola_Model.transform.forward,

                            length_of_arrow = len_arrow,

                            arrow_name = "Arrow_" + FrameF.name,

                            //pergola_fwd = 0,

                            pergola_fwd = len_arrow / 2,

                            pergola_up = up_dir_offset_L_acc_arrow,

                            arrow_prefab = arr_pfb,

                            Dotted_line_Direction = Pergola_Model.transform.up,

                            dotted_line_offset = up_dir_offset_L_acc_arrow,

                            orientation_dotted_lines = "vertical",

                            Dotted_facing_direction = Pergola_Model.transform.right


                        };

                        arrows_function(arrow_L_Accs);
                        //Extra arrow
                        if ((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_Invisible_Text") != null)
                        {
                            //Changing back to previous prefab
                            //arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                            arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_Invisible_Text");
                        }

                        //float L_width = 1.2f;

                        //float U_width = 2f;

                        float horizontal_width = 0;

                        int ch_count = GameObject.Find("Field_Parent_Section_0002").transform.childCount;

                        GameObject last_chil_Field_Group_ = GameObject.Find("Field_Parent_Section_0002").transform.GetChild(ch_count - 1).gameObject;

                        string last_no = last_chil_Field_Group_.name.Split('_').Last();

                        GameObject Fields=null;

                        foreach(Transform ch in last_chil_Field_Group_.transform)
                        {
                            if(ch.name.Contains("Fields"))
                            {
                                Fields=ch.gameObject;
                            }
                        }


                        string field_name = Fields.transform.GetChild(0).name;

                        string first_field_no = field_name.Split('_').Last();

                        GameObject first_field=Fields.transform.GetChild(0).gameObject;

                        if (first_field != null)
                        {
                            GameObject h_field;

                            float offest_for_child_ak40 = 0;
                            bool ak_40x40 = false;
                            if (first_field.transform.GetChild(0).name.Contains("ak - 40"))
                            {
                                h_field = first_field.transform.GetChild(0).gameObject;
                                ak_40x40 = true;

                            }
                            else
                            {
                                h_field = GameObject.Find($"Field_{first_field_no}");
                            }
                            //taking mesh from child
                            Bounds bound_h_field = h_field.GetComponentInChildren<MeshFilter>().mesh.bounds;

                            //scale from child
                            horizontal_width = h_field.transform.localScale.x;


                            float field_space = horizontal_width;

                            //If L accessory is there add the width of L accessory
                            if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                            {
                                field_space = field_space + (2 * L_width);

                            }

                            if (GameObject.Find("U_Accessory_Left_3") != null)
                            {
                                field_space = field_space + (2 * U_width);
                            }


                            float right_offset = 0;
                            // if (ak_40x40 == true)
                            // {
                            //     right_offset = field_space;
                            // }
                            // else
                            // {
                                right_offset = field_space / 2;
                            // }

                            float offset_fwd_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                            arrow_parm arr_space_params = new arrow_parm()
                            {

                                position_of_arrow = GameObject.Find($"Field_{first_field_no}").transform.TransformPoint(bound_h_field.center),

                                direction_of_arrow = Pergola_Model.transform.right,

                                length_of_arrow = field_space,

                                arrow_name = "Arrow_Space_" + h_field.name,


                                //pergola_right = offset_right_field_gap,
                                pergola_right = right_offset,

                                pergola_up = 0,

                                pergola_fwd = -offset_fwd_field_gap,

                                orientation_dotted_lines = "horizontal",

                                Dotted_line_Direction = Pergola_Model.transform.forward,

                                dotted_line_offset = -offset_fwd_field_gap,

                                arrow_prefab = arr_pfb,
                            };

                            //this Arrow is destroyed in ScreenShot script
                            Arrow_Side_Destroy_field_width = arrows_function(arr_space_params);

                        }

                        foreach (Transform ch in Pergola_Model.transform)
                        {
                            if (ch.name.Contains(Parents_name.SupportBars_Parent.ToString()))
                            {
                                ch.gameObject.SetActive(false);
                            }

                            if (ch.name.Contains(Parents_name.Frames_Parent.ToString()))
                            {
                                ch.gameObject.SetActive(false);
                            }

                            if (ch.name.Contains(Parents_name.Divider_Parent.ToString()))
                            {
                                ch.gameObject.SetActive(false);
                            }

                            if (ch.name.Contains(Parents_name.Field_Parent.ToString()))
                            {
                                foreach (Transform sub_child in ch)
                                {
                                    if (!sub_child.transform.name.Contains("Field_Parent_Section_0002"))
                                    {
                                        sub_child.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        //get_All_children(sub_child);
                                        try
                                        {

                                            //List<GameObject> gos = get_All_children_of_Go(sub_child);
                                            //List<GameObject> gos = get_All_children(sub_child);

                                        }
                                        catch (Exception exce)
                                        {

                                            print("Find all children: " + exce);
                                        }
                                        //Transform[] g = sub_child.GetComponentsInChildren<Transform>();
                                        foreach (Transform f_g in sub_child)
                                        {

                                            int child_number = 1;
                                            // if (type_2)
                                            // {
                                                child_number = sub_child.childCount;
                                            // }
                                            // else
                                            // {
                                            //     child_number = 1;
                                            // }
                                            if (f_g.GetSiblingIndex() == child_number-1)
                                            {
                                                foreach (Transform acs in f_g)
                                                {
                                                    foreach (Transform comp in acs)
                                                    {
                                                        //Hiding Top L_Accessory 
                                                        if ((comp.name.Contains("L_Accessory")&& comp.name.ToLower().Contains("top") )|| acs.name.Contains("ak - 109a"))
                                                            comp.gameObject.SetActive(false);
                                                        else
                                                            comp.gameObject.SetActive(true);
                                                    }
                                                }
                                            }else
                                            {
                                                f_g.gameObject.SetActive(false);
                                            }
                                        }
                                    }
                                }
                            }

                        }


                    }

                    //center section
                    if(view==Views._FIELD_SECTION_1.ToString())
                    {
                            await Task.Delay(500);//100ms delay
                            bool L_Accs = false, U_Accs = false;

                          

                            float horizontal_width = 0;
                        if(type_2)
                        {

                            int ch_count = GameObject.Find("Field_Parent_Section_0001").transform.childCount;

                            GameObject last_chil_Field_Group_ = GameObject.Find("Field_Parent_Section_0001").transform.GetChild(0).gameObject;

                            string last_no = last_chil_Field_Group_.name.Split('_').Last();

                            GameObject last_Fields = GameObject.Find($"Fields_{last_no}");

                            string field_name = last_Fields.transform.GetChild(0).name;

                            string last_field_no = field_name.Split('_').Last();

                            if (GameObject.Find($"Fields_{last_no}/Field_{last_field_no}") != null)
                            {
                                GameObject h_field;

                                float offest_for_child_ak40 = 0;
                                bool ak_40x40 = false;
                                if (GameObject.Find($"Fields_{last_no}/Field_{last_field_no}").transform.GetChild(0).name.Contains("ak - 40"))
                                {
                                    h_field = GameObject.Find($"Fields_{last_no}/Field_{last_field_no}").transform.GetChild(0).gameObject;
                                    ak_40x40 = true;

                                }
                                else
                                {
                                    h_field = GameObject.Find($"Field_{last_field_no}");
                                }
                                //taking mesh from child
                                Bounds bound_h_field = h_field.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                //scale from child
                                horizontal_width = h_field.transform.localScale.x;


                                float field_space = horizontal_width;

                                //If L accessory is there add the width of L accessory
                                if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                                {
                                    field_space = field_space + (2 * L_width);

                                }

                                if (GameObject.Find("U_Accessory_Left_3") != null)
                                {
                                    field_space = field_space + (2 * U_width);
                                }


                                float right_offset = 0;
                                // if (ak_40x40 == true)
                                // {
                                //     right_offset = field_space;
                                // }
                                // else
                                // {
                                    right_offset = field_space / 2;
                                // }

                                // float offset_fwd_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;

                                float offset_right_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                                arrow_parm arr_space_params = new arrow_parm()
                                {

                                    // position_of_arrow = GameObject.Find($"Field_{last_field_no}").transform.TransformPoint(bound_h_field.center),

                                    // direction_of_arrow = -req_fwd,

                                    // length_of_arrow = field_space,

                                    // arrow_name = "Arrow_Space_" + h_field.name,

                                    // //pergola_right = offset_right_field_gap,
                                    // pergola_right = right_offset,

                                    // pergola_up = 0,

                                    // pergola_fwd = offset_fwd_field_gap,

                                    // orientation_dotted_lines = "horizontal",

                                    // Dotted_line_Direction = Pergola_Model.transform.forward,

                                    // dotted_line_offset = offset_fwd_field_gap,

                                    position_of_arrow = h_field.transform.TransformPoint(bound_h_field.center),

                                    direction_of_arrow = -req_right,

                                    length_of_arrow = field_space,

                                    arrow_name = "Arrow_Space_" + h_field.name,

                                    pergola_right = offset_right_field_gap,

                                    pergola_up = 0,

                                    pergola_fwd = right_offset,

                                    Dotted_line_Direction = Pergola_Model.transform.right,

                                    dotted_line_offset = offset_right_field_gap,


                                };

                                arrows_function(arr_space_params);

                            }

                            //we need 2 single Arrows here

                            if (GameObject.Find($"Fields_{last_no}") != null)
                            {
                                //string last_fields_name = $"Field_Parent_Section_0001/Field_Group_{last_no}/Fields_{last_no}";

                                //field_name = GameObject.Find($"Fields_{last_no}").transform.GetChild(0).name;

                                //last_field_no = field_name.Split('_').Last();

                                //GameObject last_Fields = GameObject.Find(last_fields_name);

                                GameObject mid_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2).gameObject;

                                int single_Sect_field_count = last_Fields.transform.childCount;




                                int middle_field_no = (int.Parse(last_field_no) + last_Fields.transform.childCount - 1) / 2;

                                string horizontal_field_prefix = "Field_";

                                string middle_field_name = mid_field_in_Last_fields.name;// horizontal_field_prefix + middle_field_no;

                                GameObject next_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2 + 1).gameObject;

                                string next_field_name = next_field_in_Last_fields.name;// horizontal_field_prefix + (middle_field_no + 1).ToString();


                                //next_field_name = (int.Parse(middle_field_name.Split('_').Last()) + 1).ToString();


                                if (mid_field_in_Last_fields != null)
                                {
                                    GameObject middle_field;

                                    if (mid_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        middle_field = mid_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        middle_field = mid_field_in_Last_fields;
                                    }

                                    Bounds bound_middle_field = middle_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_middle_field = middle_field.transform.TransformPoint(bound_middle_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_middle_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_middle_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 150)
                                    {
                                        length_of_arrow = 151;
                                    }
                                    float distance_from_floor = 0;
                                    RaycastHit hit_accs_front, hit_accs_back;

                                 



                                    arrow_parm arr_mid_field = new arrow_parm()
                                    {


                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = Pergola_Model.transform.right,

                                        arrow_name = "Arrow_" + middle_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_fwd = -Mathf.Abs(middle_field.transform.localScale.x) / 2 - 300,//- 300,//-Mathf.Abs(frame_D.transform.localScale.y)

                                        pergola_right = -Mathf.Abs(Vector3.Dot(middle_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_middle_field.size)) / 2,

                                        txt = DB_script.raffaf_spacing_pergola.ToString(),

                                        txt_right = -length_of_arrow * 3 / 4,
                                        //txt_fwd = -length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.forward,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset = -300,//- Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)),
                                        orientation_dotted_lines = "horizontal"

                                    };

                                    arrows_function(arr_mid_field);
                                }


                                if (next_field_in_Last_fields != null)
                                {
                                    GameObject next_field;

                                    if (next_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        next_field = next_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        next_field = next_field_in_Last_fields;
                                    }

                                    Bounds bound_next_field = next_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_next_field = next_field.transform.TransformPoint(bound_next_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_next_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_next_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 110)
                                    {
                                        length_of_arrow = 111;
                                    }
                                    arrow_parm arr_next_field = new arrow_parm()
                                    {
                                        //position_of_arrow = global_pos_of_arrow,

                                        //length_of_arrow = length_of_arrow,

                                        //direction_of_arrow = Pergola_Model.transform.forward,

                                        //arrow_name = "Arrow_" + next_field_name,

                                        //arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        ////pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        //pergola_right = -horizontal_width / 2 - 300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        //pergola_fwd = -Mathf.Abs(Vector3.Dot(next_field.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_next_field.size)) / 2,

                                        ////***********Dummy text***********//

                                        //txt = dummy_text,//

                                        //Dotted_line_Direction = Pergola_Model.transform.right,

                                        ////dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),

                                        //dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),

                                        //orientation_dotted_lines = "vertical"
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = -Pergola_Model.transform.right,

                                        arrow_name = "Arrow_" + next_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_fwd = -Mathf.Abs(next_field.transform.localScale.x) / 2 - 300,// - 300,//-Mathf.Abs(frame_D.transform.localScale.y)
                                        pergola_right = Mathf.Abs(Vector3.Dot(next_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_next_field.size)) / 2,

                                        //***********Dummy text***********//

                                        txt = dummy_text,//

                                        Dotted_line_Direction = Pergola_Model.transform.forward,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),

                                        dotted_line_offset = -300,//- Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)),

                                        orientation_dotted_lines = "horizontal"

                                    };

                                    arrows_function(arr_next_field);
                                }
                            }

                            //we need 2 single head arrows for Gap from the ground
                            if (GameObject.Find($"Fields_{last_no}") != null)
                            {
                                //string last_fields_name = $"Field_Parent_Section_0001/Field_Group_{last_no}/Fields_{last_no}";

                                //field_name = GameObject.Find($"Fields_{last_no}").transform.GetChild(0).name;

                                //last_field_no = field_name.Split('_').Last();

                                //GameObject last_Fields = GameObject.Find(last_fields_name);

                                GameObject first_field_in_Last_fields = last_Fields.transform.GetChild(0).gameObject;

                                int single_Sect_field_count = last_Fields.transform.childCount;




                                //int middle_field_no = (int.Parse(last_field_no) + last_Fields.transform.childCount - 1) / 2;

                                string horizontal_field_prefix = "Field_";

                                string first_field_name = first_field_in_Last_fields.name;// horizontal_field_prefix + middle_field_no;

                                //GameObject next_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2 + 1).gameObject;

                                //string next_field_name = next_field_in_Last_fields.name;// horizontal_field_prefix + (middle_field_no + 1).ToString();


                                //next_field_name = (int.Parse(middle_field_name.Split('_').Last()) + 1).ToString();


                                if (first_field_in_Last_fields != null)
                                {
                                    GameObject first_field;

                                    if (first_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        first_field = first_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        first_field = first_field_in_Last_fields;
                                    }

                                    Bounds bound_first_field = first_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_middle_field = first_field.transform.TransformPoint(bound_first_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_middle_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_middle_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 150)
                                    {
                                        length_of_arrow = 151;
                                    }


                                    Vector3 front_point, back_point;

                                    front_point = first_field.transform.TransformPoint(bound_first_field.center + first_field.transform.InverseTransformDirection(Pergola_Model.transform.up) * Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_first_field.center)));

                                    back_point = first_field.transform.TransformPoint(bound_first_field.center + first_field.transform.InverseTransformDirection(-Pergola_Model.transform.up) * Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_first_field.center))); ;// global_center_of_middle_field - Pergola_Model.transform.up * (Vector3.Dot(Pergola_Model.transform.up, global_center_of_middle_field));
                                    float distance_from_floor = 0;
                                    RaycastHit hit_accs_front, hit_accs_back;
                                    RaycastHit hit_accs;

                                    Debug.DrawRay(front_point, Pergola_Model.transform.forward * 10000, Color.red, 150);
                                    Debug.DrawRay(back_point, Pergola_Model.transform.forward * 10000, Color.green, 150);

                                    bool obj_hit = false;
                                    if (Physics.Raycast(front_point, Pergola_Model.transform.forward, out hit_accs_front, Mathf.Infinity))
                                    {
                                        obj_hit = true;
                                    }
                                    if (Physics.Raycast(back_point, Pergola_Model.transform.forward, out hit_accs_back, Mathf.Infinity))
                                    {
                                        obj_hit = true;
                                    }


                                    if (obj_hit)
                                    {
                                        if (hit_accs_front.transform.parent.name.Contains("Accessory"))
                                        {
                                            hit_accs = hit_accs_front;
                                        }
                                        else
                                        {
                                            hit_accs = hit_accs_back;
                                        }
                                        if (hit_accs.transform != null)
                                            if (hit_accs.transform.parent != null)
                                            {
                                                if (hit_accs.transform.parent.name.Contains("Accessory"))
                                            {
                                                Debug.Log("Hit object" + hit_accs.transform.parent.name);

                                                Bounds bound_hit_accs = hit_accs.transform.parent.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                                Vector3 tip_of_accs = hit_accs.transform.TransformPoint(bound_hit_accs.center + hit_accs.transform.InverseTransformDirection(Pergola_Model.transform.right) * Mathf.Abs(Vector3.Dot(hit_accs.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_hit_accs.center)));// global_center_of_middle_field - Pergola_Model.transform.up * (Vector3.Dot(Pergola_Model.transform.up, global_center_of_middle_field));

                                                Vector3 tip_of_acc_wrt_Pergola_Model = Pergola_Model.transform.InverseTransformPoint(tip_of_accs);

                                                Vector3 down_point = first_field.transform.TransformPoint(bound_first_field.center + first_field.transform.InverseTransformDirection(Pergola_Model.transform.right) * Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_first_field.center)));

                                                Debug.DrawRay(tip_of_accs, Pergola_Model.transform.forward * 10000, Color.blue, 150);
                                                Debug.DrawRay(down_point, Pergola_Model.transform.forward * 10000, Color.cyan, 150);
                                                Vector3 arrow_1_pos_wrt_Pergola_Model = Pergola_Model.transform.InverseTransformPoint(down_point);
                                                distance_from_floor = Vector3.Dot(Pergola_Model.transform.right, tip_of_accs) - Vector3.Dot(Pergola_Model.transform.right, down_point);
                                                Debug.Log("Distance field sect 1  from ground =" + (tip_of_acc_wrt_Pergola_Model.x - arrow_1_pos_wrt_Pergola_Model.x + " DOT PROD DIST =" + distance_from_floor));
                                            }
                                        }

                                    }
                                    //if arrow length is less


                                    arrow_parm arr_floor_gap_1_field = new arrow_parm()
                                    {
                                        //position_of_arrow = global_pos_of_arrow,

                                        //length_of_arrow = length_of_arrow,

                                        //direction_of_arrow = -Pergola_Model.transform.forward,

                                        //arrow_name = "Arrow_" + middle_field_name,

                                        //arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        ////pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        //pergola_right = -horizontal_width / 2 - 300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),

                                        //pergola_fwd = Mathf.Abs(Vector3.Dot(middle_field.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_middle_field.size)) / 2,

                                        //txt = DB_script.raffaf_spacing_pergola.ToString(),

                                        ////txt_right = -length_of_arrow * 2 * 3 / 4,
                                        //txt_fwd = length_of_arrow * 3 / 4,

                                        //Dotted_line_Direction = Pergola_Model.transform.right,

                                        ////dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        //dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        //orientation_dotted_lines = "vertical"
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = -Pergola_Model.transform.right,

                                        arrow_name = "Arrow_Space1_height" + first_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_fwd = -Mathf.Abs(first_field.transform.localScale.x) / 2 - 300,//- 300,//-Mathf.Abs(frame_D.transform.localScale.y)

                                        pergola_right = Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_first_field.size)) / 2,

                                        txt = Mathf.FloorToInt(distance_from_floor).ToString(),

                                        txt_right = length_of_arrow * 3 / 4,
                                        //txt_fwd = -length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.forward,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset = -300,//- Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)),
                                        orientation_dotted_lines = "horizontal"

                                    };

                                    arrows_function(arr_floor_gap_1_field);

                                    arrow_parm arr_floor_gap_2_field = new arrow_parm()
                                    {

                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = Pergola_Model.transform.right,

                                        arrow_name = "Arrow_Space2_height" + first_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_fwd = -Mathf.Abs(first_field.transform.localScale.x) / 2 - 300,//- 300,//-Mathf.Abs(frame_D.transform.localScale.y)

                                        pergola_right = Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_first_field.size)) / 2 + distance_from_floor,

                                        txt = dummy_text,

                                        //txt_right = -length_of_arrow * 3 / 4,
                                        //txt_fwd = -length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.forward,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset = -300,//- Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)),
                                        orientation_dotted_lines = "horizontal"

                                    };

                                    arrows_function(arr_floor_gap_2_field);
                                }



                            }


                            rotateonmouse_script.rotate_PergolaModel(Views._FIELD_SECTION_2.ToString());
                        }
                        else
                        {
                            int ch_count = GameObject.Find("Field_Parent_Section_0002").transform.childCount;

                            GameObject last_chil_Field_Group_ = GameObject.Find("Field_Parent_Section_0002").transform.GetChild(0).gameObject;

                            string last_no = last_chil_Field_Group_.name.Split('_').Last();

                            GameObject Fields = null;

                            //to finds Fields bw accessory,fields group
                            foreach (Transform t in last_chil_Field_Group_.transform)
                            {
                                if (t.name.Contains("Fields"))
                                {
                                    Fields = t.gameObject;
                                }
                            }

                            //{Fields.name} with Fields_{last_no}
                            string field_name = Fields.transform.GetChild(0).name;//GameObject.Find($"Fields_{last_no}").transform.GetChild(0).name;

                            string first_field_no = field_name.Split('_').Last();

                            if (GameObject.Find($"{Fields.name}/Field_{first_field_no}") != null)
                            {
                                GameObject h_field;

                                float offest_for_child_ak40 = 0;
                                bool ak_40x40 = false;
                                if (GameObject.Find($"{Fields.name}/Field_{first_field_no}").transform.GetChild(0).name.Contains("ak - 40"))
                                {
                                    h_field = GameObject.Find($"{Fields.name}/Field_{first_field_no}").transform.GetChild(0).gameObject;
                                    ak_40x40 = true;

                                }
                                else
                                {
                                    h_field = GameObject.Find($"Field_{first_field_no}");
                                }
                                //taking mesh from child
                                Bounds bound_h_field = GameObject.Find($"Field_{first_field_no}").GetComponentInChildren<MeshFilter>().mesh.bounds;

                                //scale from child
                                horizontal_width =Mathf.Abs( h_field.transform.localScale.x);


                                float field_space = horizontal_width;

                                //If L accessory is there add the width of L accessory
                                if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                                {
                                    field_space = field_space + (2 * L_width);

                                }

                                if (GameObject.Find("U_Accessory_Left_3") != null)
                                {
                                    field_space = field_space + (2 * U_width);
                                }


                                float right_offset = 0;
                                // if (ak_40x40 == true)
                                // {
                                //     right_offset = field_space;
                                // }
                                // else
                                // {
                                    right_offset = field_space / 2;
                                // }

                                float offset_fwd_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                                arrow_parm arr_space_params = new arrow_parm()
                                {

                                    position_of_arrow = GameObject.Find($"Field_{first_field_no}").transform.TransformPoint(bound_h_field.center),

                                    direction_of_arrow = -req_fwd,

                                    length_of_arrow = field_space,

                                    arrow_name = "Arrow_Space_" + h_field.name+"_first_sect_1",

                                    //pergola_right = offset_right_field_gap,
                                    // pergola_right = right_offset,

                                    pergola_up = 0,

                                    pergola_fwd = -offset_fwd_field_gap,

                                    orientation_dotted_lines = "horizontal",

                                    Dotted_line_Direction = Pergola_Model.transform.forward,

                                    dotted_line_offset = -offset_fwd_field_gap,


                                };

                                arrows_function(arr_space_params);

                            }

                            //we need 2 single Arrows here


                            if (GameObject.Find($"{Fields.name}") != null)
                            {
                                string last_fields_name = $"Field_Parent_Section_0002/Field_Group_{last_no}/{Fields.name}";

                                field_name = GameObject.Find($"{Fields.name}").transform.GetChild(0).name;

                                first_field_no = field_name.Split('_').Last();

                                GameObject last_Fields = GameObject.Find(last_fields_name);

                                GameObject mid_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2).gameObject;

                                int single_Sect_field_count = last_Fields.transform.childCount;




                                int middle_field_no = (int.Parse(first_field_no) + last_Fields.transform.childCount - 1) / 2;

                                string horizontal_field_prefix = "Field_";

                                string middle_field_name = mid_field_in_Last_fields.name;// horizontal_field_prefix + middle_field_no;

                                GameObject next_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2 + 1).gameObject;

                                string next_field_name = next_field_in_Last_fields.name;// horizontal_field_prefix + (middle_field_no + 1).ToString();


                                //next_field_name = (int.Parse(middle_field_name.Split('_').Last()) + 1).ToString();


                                if (mid_field_in_Last_fields != null)
                                {
                                    GameObject middle_field;

                                    if (mid_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        middle_field = mid_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        middle_field = mid_field_in_Last_fields;
                                    }
                                   
                                    horizontal_width =Mathf.Abs( middle_field.transform.localScale.x); 
                                    float right_offset = 0;
                                    float field_space = horizontal_width;
                                    //If L accessory is there add the width of L accessory
                                    if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                                    {
                                        field_space = field_space + (2 * L_width);

                                    }

                                    if (GameObject.Find("U_Accessory_Left_3") != null)
                                    {
                                        field_space = field_space + (2 * U_width);
                                    }

                                    right_offset = field_space / 2;

                                    Bounds bound_middle_field = middle_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_middle_field = middle_field.transform.TransformPoint(bound_middle_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_middle_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_middle_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 150)
                                    {
                                        length_of_arrow = 151;
                                    }



                                    arrow_parm arr_mid_field = new arrow_parm()
                                    {
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = -Pergola_Model.transform.forward,

                                        arrow_name = "Arrow_" + middle_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_right =-( horizontal_width / 2 + 300 + Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size))),
                                        //pergola_right = right_offset,
                                        pergola_fwd = Mathf.Abs(Vector3.Dot(middle_field.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_middle_field.size)) / 2,

                                        txt = DB_script.raffaf_spacing_pergola.ToString(),

                                        //txt_right = -length_of_arrow * 2 * 3 / 4,
                                        txt_fwd = length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.right,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        orientation_dotted_lines = "vertical"

                                    };

                                    arrows_function(arr_mid_field);
                                }


                                if (next_field_in_Last_fields != null)
                                {
                                    GameObject next_field;

                                    if (next_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        next_field = next_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        next_field = next_field_in_Last_fields;
                                    }

                                    float right_offset = 0;
                                    float field_space = next_field.transform.localScale.x;
                                    //If L accessory is there add the width of L accessory
                                    if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                                    {
                                        field_space = field_space + (2 * L_width);

                                    }

                                    if (GameObject.Find("U_Accessory_Left_3") != null)
                                    {
                                        field_space = field_space + (2 * U_width);
                                    }

                                    right_offset = field_space / 2;


                                    Bounds bound_next_field = next_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_next_field = next_field.transform.TransformPoint(bound_next_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_next_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_next_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 110)
                                    {
                                        length_of_arrow = 111;
                                    }
                                    arrow_parm arr_next_field = new arrow_parm()
                                    {
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = Pergola_Model.transform.forward,

                                        arrow_name = "Arrow_" + next_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_right =-( horizontal_width / 2 + 300 + Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size))),

                                        //pergola_right=right_offset,
                                        pergola_fwd = -Mathf.Abs(Vector3.Dot(next_field.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_next_field.size)) / 2,

                                        //***********Dummy text***********//

                                        txt = dummy_text,//

                                        Dotted_line_Direction = Pergola_Model.transform.right,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),

                                        dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),

                                        orientation_dotted_lines = "vertical"

                                    };

                                    arrows_function(arr_next_field);
                                }
                            }

                            //we need 2 single head arrows for Gap from the ground

                            if (GameObject.Find($"{Fields.name}") != null)
                            {
                                string last_fields_name = $"Field_Parent_Section_0002/Field_Group_{last_no}/{Fields.name}";

                                field_name = GameObject.Find($"{Fields.name}").transform.GetChild(0).name;

                                first_field_no = field_name.Split('_').Last();

                                GameObject last_Fields = GameObject.Find(last_fields_name);

                                GameObject first_field_in_Last_fields = last_Fields.transform.GetChild(0).gameObject;

                                int single_Sect_field_count = last_Fields.transform.childCount;




                                //int middle_field_no = (int.Parse(first_field_no) + last_Fields.transform.childCount - 1) / 2;

                                //string horizontal_field_prefix = "Field_";

                                string first_field_name = first_field_in_Last_fields.name;// horizontal_field_prefix + middle_field_no;

                                //GameObject next_field_in_Last_fields = last_Fields.transform.GetChild((last_Fields.transform.childCount) / 2 + 1).gameObject;

                                //string next_field_name = next_field_in_Last_fields.name;// horizontal_field_prefix + (middle_field_no + 1).ToString();


                                //next_field_name = (int.Parse(middle_field_name.Split('_').Last()) + 1).ToString();



                                if (first_field_in_Last_fields != null)
                                {
                                    GameObject first_field;

                                    if (first_field_in_Last_fields.transform.GetChild(0).name.Contains("ak - 40"))
                                    {
                                        first_field = first_field_in_Last_fields.transform.GetChild(0).gameObject;
                                    }
                                    else
                                    {
                                        first_field = first_field_in_Last_fields;
                                    }

                                    Bounds bound_first_field = first_field.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    Vector3 global_center_of_middle_field = first_field.transform.TransformPoint(bound_first_field.center);


                                    Vector3 global_pos_of_arrow = global_center_of_middle_field;// - Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, global_center_of_middle_field)) + Pergola_Model.transform.forward * (Vector3.Dot(Pergola_Model.transform.forward, frame_F.transform.TransformPoint(bound_f.center)));

                                    float length_of_arrow = Mathf.Max(frame_C.transform.localScale.x, frame_C.transform.localScale.y, frame_C.transform.localScale.z) / 10;

                                    if (length_of_arrow < 150)
                                    {
                                        length_of_arrow = 151;
                                    }

                                    Vector3 front_point, back_point;

                                    front_point = first_field.transform.TransformPoint(bound_first_field.center + first_field.transform.InverseTransformDirection(Pergola_Model.transform.up) * Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_first_field.center)));

                                    back_point = first_field.transform.TransformPoint(bound_first_field.center + first_field.transform.InverseTransformDirection(-Pergola_Model.transform.up) * Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_first_field.center))); ;// global_center_of_middle_field - Pergola_Model.transform.up * (Vector3.Dot(Pergola_Model.transform.up, global_center_of_middle_field));
                                    float distance_from_floor = 0;
                                    RaycastHit hit_accs_front, hit_accs_back;
                                    RaycastHit hit_accs;

                                    Debug.DrawRay(front_point, Pergola_Model.transform.right * 10000, Color.red, 150);
                                    Debug.DrawRay(back_point, Pergola_Model.transform.right * 10000, Color.green, 150);

                                    bool obj_hit = false;

                                    //Here we Raycast Pergola_Model.transform.forward and Find Which Accessory that is hit
                                    if (Physics.Raycast(front_point, Pergola_Model.transform.right, out hit_accs_front, Mathf.Infinity))
                                    {
                                        obj_hit = true;
                                    }
                                    if (Physics.Raycast(back_point, Pergola_Model.transform.right, out hit_accs_back, Mathf.Infinity))
                                    {
                                        obj_hit = true;
                                    }
                                    if (obj_hit)
                                    {
                                        if (hit_accs_front.transform.parent.name.Contains("Accessory"))
                                        {
                                            hit_accs = hit_accs_front;
                                        }
                                        else
                                        {
                                            hit_accs = hit_accs_back;
                                        }
                                    //}

                                    //    if (Physics.Raycast(front_point, Pergola_Model.transform.right, out hit_accs, Mathf.Infinity) || Physics.Raycast(back_point, Pergola_Model.transform.right, out hit_accs, Mathf.Infinity))
                                    //{
                                        if (hit_accs.transform.parent != null)
                                        {
                                            if (hit_accs.transform.parent.name.Contains("Accessory"))
                                            {
                                                Debug.Log("Hit object" + hit_accs.transform.parent.name);

                                                Bounds bound_hit_accs = hit_accs.transform.parent.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                                Vector3 tip_of_accs = hit_accs.transform.TransformPoint(bound_hit_accs.center + hit_accs.transform.InverseTransformDirection(-Pergola_Model.transform.forward) * Mathf.Abs(Vector3.Dot(hit_accs.transform.InverseTransformDirection(-Pergola_Model.transform.forward), bound_hit_accs.center)));// global_center_of_middle_field - Pergola_Model.transform.up * (Vector3.Dot(Pergola_Model.transform.up, global_center_of_middle_field));

                                                Vector3 tip_of_acc_wrt_Pergola_Model = Pergola_Model.transform.InverseTransformPoint(tip_of_accs);

                                                Vector3 down_point = first_field.transform.TransformPoint(bound_first_field.center + first_field.transform.InverseTransformDirection(-Pergola_Model.transform.forward) * Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(-Pergola_Model.transform.forward), bound_first_field.center)));

                                                Debug.DrawRay(tip_of_accs, Pergola_Model.transform.right * 10000, Color.blue, 150);
                                                Debug.DrawRay(down_point, Pergola_Model.transform.right * 10000, Color.cyan, 150);
                                                Vector3 arrow_1_pos_wrt_Pergola_Model = Pergola_Model.transform.InverseTransformPoint(down_point);
                                                distance_from_floor = Mathf.Abs(Vector3.Dot(Pergola_Model.transform.forward, tip_of_accs) - Vector3.Dot(Pergola_Model.transform.forward, down_point));
                                                Debug.Log("Distance field sect 2  from ground =" + (tip_of_acc_wrt_Pergola_Model.z - arrow_1_pos_wrt_Pergola_Model.z + " DOT PROD DIST =" + distance_from_floor));
                                            }
                                        }

                                    }
                                    //if arrow length is less

                                    arrow_parm arr_space1_field = new arrow_parm()
                                    {
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = Pergola_Model.transform.forward,

                                        arrow_name = "Arrow_Space1_height" + first_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_right =-( horizontal_width / 2 + 300 + Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size))),

                                        pergola_fwd = -Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_first_field.size)) / 2,

                                        txt = Mathf.FloorToInt(distance_from_floor).ToString(),

                                        //txt_right = -length_of_arrow * 2 * 3 / 4,
                                        txt_fwd = length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.right,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        orientation_dotted_lines = "vertical"

                                    };

                                    arrows_function(arr_space1_field);

                                    arrow_parm arr_space2_field = new arrow_parm()
                                    {
                                        position_of_arrow = global_pos_of_arrow,

                                        length_of_arrow = length_of_arrow,

                                        direction_of_arrow = -Pergola_Model.transform.forward,

                                        arrow_name = "Arrow_Space2_height" + first_field_name,

                                        arrow_prefab = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Single_Head"),

                                        //pergola_fwd = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300) - Mathf.Abs(frame_D.transform.localScale.y),
                                        pergola_right =-( horizontal_width / 2 + 300 + Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size))),

                                        pergola_fwd = -Mathf.Abs(Vector3.Dot(first_field.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_first_field.size)) / 2 - distance_from_floor,

                                        txt = dummy_text,

                                        //txt_right = -length_of_arrow * 2 * 3 / 4,
                                        txt_fwd = length_of_arrow * 3 / 4,

                                        Dotted_line_Direction = Pergola_Model.transform.right,

                                        //dotted_line_offset = -(Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300),
                                        dotted_line_offset = -300 - Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_e.size)),
                                        orientation_dotted_lines = "vertical"

                                    };

                                    arrows_function(arr_space2_field);
                                }



                            }


                            rotateonmouse_script.rotate_PergolaModel(Views._FIELD_SECTION_3.ToString());
                        }

                        foreach (Transform ch in Pergola_Model.transform)
                        {
                            if (ch.name.Contains(Parents_name.SupportBars_Parent.ToString()))
                            {
                                ch.gameObject.SetActive(false);
                            }

                            if (ch.name.Contains(Parents_name.Frames_Parent.ToString()))
                            {
                                ch.gameObject.SetActive(false);
                            }

                            if (ch.name.Contains(Parents_name.Divider_Parent.ToString()))
                            {
                                ch.gameObject.SetActive(false);
                            }

                            if (ch.name.Contains(Parents_name.Field_Parent.ToString()))
                            {
                                foreach (Transform sub_child in ch)
                                {

                                    string Field_Parent_Section_name = "";

                                    if(type_2==true)
                                    {
                                        Field_Parent_Section_name = "Field_Parent_Section_0001";
                                    }
                                    else
                                    {
                                        Field_Parent_Section_name = "Field_Parent_Section_0002";
                                    }

                                    if (!sub_child.transform.name.Contains(Field_Parent_Section_name))
                                    {
                                        sub_child.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        //get_All_children(sub_child);
                                        //try
                                        //{

                                        //    List<GameObject> gos = get_All_children_of_Go(sub_child);
                                        //    //List<GameObject> gos = get_All_children(sub_child);

                                        //}
                                        //catch (Exception exce)
                                        //{

                                        //    print("Find all children: " + exce);
                                        //}
                                        //Transform[] g = sub_child.GetComponentsInChildren<Transform>();
                                        foreach (Transform f_g in sub_child)
                                        {
                                            int child_number = 1;
                                            //if (type_2)
                                            //{
                                            //    child_number = 1;
                                            //}
                                            //else
                                            //{
                                            child_number = sub_child.childCount;
                                            //}

                                            //if (f_g.name.Contains("Field_Group_"+child_number))
                                            if (f_g.GetSiblingIndex() == 0)
                                            {

                                                foreach (Transform acs in f_g)
                                                {
                                                    foreach (Transform comp in acs)
                                                    {
                                                        //Hiding Top L_Accessory 
                                                        if (comp.name.Contains("L_Accessory_top") || acs.name.Contains("ak - 109a"))
                                                            comp.gameObject.SetActive(false);
                                                        else
                                                            comp.gameObject.SetActive(true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                f_g.gameObject.SetActive(false); ;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    if(view==Views._SIDE_FIELD_SECTION_1.ToString())
                    {
                        if (type_2)
                        {
                            rotateonmouse_script.rotate_PergolaModel(Views._SIDE_FIELD_SECTION_2.ToString());

                            GameObject FrameC = frame_C;

                            Bounds bound_L_Accs_1 = FrameC.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                            GameObject arr_pfb = null;
                            //Changing prefab as we need space left of side screen shot
                            if ((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_space_side") != null)
                            {
                                //arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                                arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_space_side");
                            }

                            float width_of_frame = Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_d.size));

                            //as we are using length of frame we must subtract it 2 times by width 
                            float len_arrow = FrameC.transform.localScale.y - 2 * width_of_frame;

                            // length of accessory is -5 of the divider lengths
                            len_arrow = len_arrow - assembly_tolerance;


                            float up_dir_offset_L_acc_arrow = Mathf.Abs(Vector3.Dot(FrameC.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_d.size)) + 300;

                            arrow_parm arrow_L_Accs = new arrow_parm()
                            {

                                position_of_arrow = FrameC.transform.TransformPoint(bound_L_Accs_1.center),


                                direction_of_arrow = -Pergola_Model.transform.right,

                                length_of_arrow = len_arrow,

                                arrow_name = "Arrow_" + FrameC.name,

                                pergola_fwd = 0,

                                pergola_right = len_arrow / 2,

                                pergola_up = up_dir_offset_L_acc_arrow,

                                arrow_prefab = arr_pfb,

                                Dotted_line_Direction = Pergola_Model.transform.up,

                                dotted_line_offset = up_dir_offset_L_acc_arrow,

                                orientation_dotted_lines = "vertical",

                                Dotted_facing_direction = Pergola_Model.transform.forward


                            };
                            arrows_function(arrow_L_Accs);


                            //Extra arrow
                            if ((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_Invisible_Text") != null)
                            {
                                //Changing back to previous prefab
                                //arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                                arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_Invisible_Text");
                            }

                            //float L_width = 1.2f;

                            //float U_width = 2f;

                            float horizontal_width = 0;

                            //new code
                            int ch_count = GameObject.Find("Field_Parent_Section_0001").transform.childCount;

                            GameObject last_chil_Field_Group_ = GameObject.Find("Field_Parent_Section_0001").transform.GetChild(0).gameObject;

                            string last_no = last_chil_Field_Group_.name.Split('_').Last();

                            GameObject last_Fields = GameObject.Find($"Fields_{last_no}");

                            string field_name = last_Fields.transform.GetChild(0).name;

                            string last_field_no = field_name.Split('_').Last();

                            if (GameObject.Find($"Fields_{last_no}/Field_{last_field_no}") != null)
                            {
                                GameObject h_field;

                                float offest_for_child_ak40 = 0;
                                bool ak_40x40 = false;
                                if (GameObject.Find($"Fields_{last_no}/Field_{last_field_no}").transform.GetChild(0).name.Contains("ak - 40"))
                                {
                                    h_field = GameObject.Find($"Fields_{last_no}/Field_{last_field_no}").transform.GetChild(0).gameObject;
                                    ak_40x40 = true;

                                }
                                else
                                {
                                    h_field = GameObject.Find($"Field_{last_field_no}");
                                }
                                //taking mesh from child
                                Bounds bound_h_field = GameObject.Find($"Field_{last_field_no}").GetComponentInChildren<MeshFilter>().mesh.bounds;

                                //scale from child
                                horizontal_width = h_field.transform.localScale.x;


                                float field_space = horizontal_width;

                                //If L accessory is there add the width of L accessory
                                if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                                {
                                    field_space = field_space + (2 * L_width);

                                }

                                if (GameObject.Find("U_Accessory_Left_3") != null)
                                {
                                    field_space = field_space + (2 * U_width);
                                }


                                float right_offset = 0;
                                // if (ak_40x40 == true)
                                // {
                                //     right_offset = field_space;
                                // }
                                // else
                                // {
                                    right_offset = field_space / 2;
                                // }

                                // float offset_fwd_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;

                                float offset_right_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                                arrow_parm arr_space_params = new arrow_parm()
                                {

                                    // position_of_arrow = GameObject.Find($"Field_{last_field_no}").transform.TransformPoint(bound_h_field.center),

                                    // direction_of_arrow = -req_fwd,

                                    // length_of_arrow = field_space,

                                    // arrow_name = "Arrow_Space_" + h_field.name,

                                    // //pergola_right = offset_right_field_gap,
                                    // pergola_right = right_offset,

                                    // pergola_up = 0,

                                    // pergola_fwd = offset_fwd_field_gap,

                                    // orientation_dotted_lines = "horizontal",

                                    // Dotted_line_Direction = Pergola_Model.transform.forward,

                                    // dotted_line_offset = offset_fwd_field_gap,

                                    position_of_arrow = h_field.transform.TransformPoint(bound_h_field.center),

                                    direction_of_arrow = -req_right,

                                    length_of_arrow = field_space,

                                    arrow_name = "Arrow_Space_" + h_field.name,

                                    pergola_right = offset_right_field_gap,

                                    pergola_up = 0,

                                    pergola_fwd = right_offset,

                                    arrow_prefab = arr_pfb,

                                    Dotted_line_Direction = Pergola_Model.transform.right,

                                    dotted_line_offset = offset_right_field_gap,


                                };

                                //this Arrow is destroyed in ScreenShot script
                                Arrow_Side_Destroy_field_width = arrows_function(arr_space_params);

                            }
                        }
                        else
                        {
                            rotateonmouse_script.rotate_PergolaModel(Views._SIDE_FIELD_SECTION_3.ToString());

                            GameObject FrameF = frame_F;

                            Bounds bound_L_Accs_1 = FrameF.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                            GameObject arr_pfb = null;

                            //Changing prefab as we need space left of side screen shot
                            if ((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_space_side") != null)
                            {
                                //arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                                arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_space_side");
                            }

                            float width_of_frame = Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_f.size));

                            //as we are using length of frame we must subtract it 2 times by width 
                            float len_arrow = FrameF.transform.localScale.y - 2 * width_of_frame;

                            // length of accessory is -5 of the divider lengths
                            len_arrow = len_arrow - assembly_tolerance;


                            float up_dir_offset_L_acc_arrow = Mathf.Abs(Vector3.Dot(FrameF.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_f.size)) + 300;

                            arrow_parm arrow_L_Accs = new arrow_parm()
                            {

                                position_of_arrow = FrameF.transform.TransformPoint(bound_L_Accs_1.center),


                                direction_of_arrow = -Pergola_Model.transform.forward,

                                length_of_arrow = len_arrow,

                                arrow_name = "Arrow_" + FrameF.name,

                                //pergola_fwd = 0,

                                pergola_fwd = len_arrow / 2,

                                pergola_up = up_dir_offset_L_acc_arrow,

                                arrow_prefab = arr_pfb,

                                Dotted_line_Direction = Pergola_Model.transform.up,

                                dotted_line_offset = up_dir_offset_L_acc_arrow,

                                orientation_dotted_lines = "vertical",

                                Dotted_facing_direction = Pergola_Model.transform.right


                            };

                            arrows_function(arrow_L_Accs);
                            //Extra arrow
                            if ((GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_Invisible_Text") != null)
                            {
                                //Changing back to previous prefab
                                //arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Head_Text");
                                arr_pfb = (GameObject)Resources.Load("Prefabs/Arrows/Arrow_Orange_Invisible_Text");
                            }

                            //float L_width = 1.2f;

                            //float U_width = 2f;

                            float horizontal_width = 0;

                            int ch_count = GameObject.Find("Field_Parent_Section_0002").transform.childCount;

                            GameObject last_chil_Field_Group_ = GameObject.Find("Field_Parent_Section_0002").transform.GetChild(0).gameObject;

                            string last_no = last_chil_Field_Group_.name.Split('_').Last();

                            GameObject Fields = null;

                            foreach (Transform ch in last_chil_Field_Group_.transform)
                            {
                                if (ch.name.Contains("Fields"))
                                {
                                    Fields = ch.gameObject;
                                }
                            }


                            string field_name = Fields.transform.GetChild(0).name;

                            string first_field_no = field_name.Split('_').Last();

                            GameObject first_field = Fields.transform.GetChild(0).gameObject;

                            if (first_field != null)
                            {
                                GameObject h_field;

                                float offest_for_child_ak40 = 0;
                                bool ak_40x40 = false;
                                if (first_field.transform.GetChild(0).name.Contains("ak - 40"))
                                {
                                    h_field = first_field.transform.GetChild(0).gameObject;
                                    ak_40x40 = true;

                                }
                                else
                                {
                                    h_field = GameObject.Find($"Field_{first_field_no}");
                                }
                                //taking mesh from child
                                Bounds bound_h_field = h_field.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                //scale from child
                                horizontal_width = h_field.transform.localScale.x;


                                float field_space = horizontal_width;

                                //If L accessory is there add the width of L accessory
                                if (GameObject.Find("L_Accessory_bottomLeft_1") != null)
                                {
                                    field_space = field_space + (2 * L_width);

                                }

                                if (GameObject.Find("U_Accessory_Left_3") != null)
                                {
                                    field_space = field_space + (2 * U_width);
                                }


                                float right_offset = 0;
                                // if (ak_40x40 == true)
                                // {
                                //     right_offset = field_space;
                                // }
                                // else
                                // {
                                    right_offset = field_space / 2;
                                // }

                                float offset_fwd_field_gap = Mathf.Abs(Vector3.Dot(h_field.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_h_field.size)) + 300;
                                arrow_parm arr_space_params = new arrow_parm()
                                {

                                    position_of_arrow = GameObject.Find($"Field_{first_field_no}").transform.TransformPoint(bound_h_field.center),

                                    direction_of_arrow = Pergola_Model.transform.right,

                                    length_of_arrow = field_space,

                                    arrow_name = "Arrow_Space_" + h_field.name,


                                    //pergola_right = offset_right_field_gap,
                                    pergola_right = right_offset,

                                    pergola_up = 0,

                                    pergola_fwd = -offset_fwd_field_gap,

                                    orientation_dotted_lines = "horizontal",

                                    Dotted_line_Direction = Pergola_Model.transform.forward,

                                    dotted_line_offset = -offset_fwd_field_gap,

                                    arrow_prefab = arr_pfb,
                                };

                                //this Arrow is destroyed in ScreenShot script
                                Arrow_Side_Destroy_field_width = arrows_function(arr_space_params);

                            }

                        }

                        foreach (Transform ch in Pergola_Model.transform)
                        {
                            if (ch.name.Contains(Parents_name.SupportBars_Parent.ToString()))
                            {
                                ch.gameObject.SetActive(false);
                            }

                            if (ch.name.Contains(Parents_name.Frames_Parent.ToString()))
                            {
                                ch.gameObject.SetActive(false);
                            }

                            if (ch.name.Contains(Parents_name.Divider_Parent.ToString()))
                            {
                                ch.gameObject.SetActive(false);
                            }

                            if (ch.name.Contains(Parents_name.Field_Parent.ToString()))
                            {
                                foreach (Transform sub_child in ch)
                                {

                                    string Field_Parent_Section_name = "";

                                    if (type_2 == true)
                                    {
                                        Field_Parent_Section_name = "Field_Parent_Section_0001";
                                    }
                                    else
                                    {
                                        Field_Parent_Section_name = "Field_Parent_Section_0002";
                                    }

                                    if (!sub_child.transform.name.Contains(Field_Parent_Section_name))
                                    {
                                        sub_child.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        //get_All_children(sub_child);
                                        //try
                                        //{

                                        //    List<GameObject> gos = get_All_children_of_Go(sub_child);
                                        //    //List<GameObject> gos = get_All_children(sub_child);

                                        //}
                                        //catch (Exception exce)
                                        //{

                                        //    print("Find all children: " + exce);
                                        //}
                                        //Transform[] g = sub_child.GetComponentsInChildren<Transform>();
                                        foreach (Transform f_g in sub_child)
                                        {
                                            int child_number = 1;
                                            //if (type_2)
                                            //{
                                            //    child_number = 1;
                                            //}
                                            //else
                                            //{
                                            child_number = sub_child.childCount;
                                            //}

                                            //if (f_g.name.Contains("Field_Group_"+child_number))
                                            if (f_g.GetSiblingIndex() == 0)
                                            {

                                                foreach (Transform acs in f_g)
                                                {
                                                    foreach (Transform comp in acs)
                                                    {
                                                        //Hiding Top L_Accessory 
                                                        if (comp.name.Contains("L_Accessory_top")|| (comp.name.Contains("L_")&& comp.name.ToLower().Contains("top")) || acs.name.Contains("ak - 109a"))
                                                            comp.gameObject.SetActive(false);
                                                        else
                                                            comp.gameObject.SetActive(true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                f_g.gameObject.SetActive(false); ;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    if (view == Views._TOP.ToString())
                    {
                        foreach (Transform ch in Pergola_Model.transform)
                            if (ch.name.Contains(Parents_name.SupportBars_Parent.ToString()))
                            {
                                foreach (Transform support_bar_parent in ch)
                                {

                                    foreach (Transform support_bar in support_bar_parent)
                                    {
                                        GameObject support_clamp = support_bar.GetChild(0).gameObject;

                                        if (support_clamp != null)
                                        {
                                            GameObject sp = Instantiate(sphere_, Pergola_Model.transform);//duplicate spheres
                                            sp.name = "sphere_" + support_clamp.name;//giving name sphere_spportClam_0

                                            Bounds suppp_clamp_bounds = support_clamp.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                            Vector3 support_clamp_center_global = support_clamp.transform.TransformPoint(suppp_clamp_bounds.center);

                                            float center2edge_dist_1 = Mathf.Abs(Vector3.Dot(support_clamp.transform.InverseTransformDirection(Pergola_Model.transform.right), suppp_clamp_bounds.size)) / 2;

                                            Vector3 sph_pos = support_clamp_center_global;

                                            sp.transform.position = sph_pos;

                                            //****TO scale Sphere according to width of a frame***********//
                                            float scale_sphere = 2F * Vector3.Dot(frame_A.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_a.size);

                                            sp.transform.localScale = new Vector3(scale_sphere, scale_sphere, scale_sphere);


                                        }
                                    }
                                }
                            }


                        if (GameObject.Find(Parents_name.Field_Parent.ToString()) != null)
                        {
                            GameObject.Find(Parents_name.Field_Parent.ToString()).SetActive(false);
                        }

                        if (GameObject.Find(Parents_name.SupportBars_Parent.ToString()) != null)
                        {
                            GameObject.Find(Parents_name.SupportBars_Parent.ToString()).SetActive(false);
                        }
                    }

                    //Arranging and rotating text in each arrows
                    if (Arrow_Parent != null)
                        foreach (Transform Arrow in Arrow_Parent.transform)
                        {
                            Arrow.GetChild(1).forward = Camera.main.transform.forward;

                            if (view == Views._TOP.ToString())
                            {

                                if ((Arrow.name.Contains("FrameE") || Arrow.name.Contains("FrameA") || Arrow.name.Contains("FrameC"))&&!Arrow.name.ToLower().Contains("divider"))
                                {
                                    GameObject txt = Arrow.GetChild(1).gameObject;

                                    var recttrns = Arrow.GetChild(1).GetComponent<RectTransform>();

                                    recttrns.Rotate(Vector3.forward, 90);
                                    //r_tr.localRotation = Quaternion.Euler(120, -90, -90);// (0, 90, 90);

                                    if (Arrow.name.Contains("FrameA") || Arrow.name.Contains("FrameC"))
                                    {
                                        TextMeshPro tmpro = txt.GetComponent<TextMeshPro>();

                                        //recttrns.localRotation = Quaternion.Euler(90, 90, 0);

                                        //moving arrow to right side of top view 
                                        //recttrns.transform.Translate(Pergola_Model.transform.forward * tmpro.preferredHeight, Pergola_Model.transform);

                                        recttrns.transform.Translate(Pergola_Model.transform.forward * tmpro.preferredWidth, Space.World);
                                    }
                                }

                                if (Arrow.name.Contains("FrameA"))
                                {
                                    Arrow.transform.Find("Head").GetChild(0).gameObject.SetActive(false);
                                }


                                if (Arrow.name.Contains("FrameB"))
                                {
                                    //Disabling the Tail Dots
                                    Arrow.transform.Find("Tail").GetChild(0).gameObject.SetActive(false);
                                }


                            }
                            if (view == Views._FIELD_SECTION_2.ToString()||(view==Views._FIELD_SECTION_1.ToString() ))//(view==Views._FIELD_SECTION_1.ToString() && type_2)
                            {
                                if (Arrow.name.Contains("Arrow_Field"))
                                {
                                    Arrow.transform.GetChild(1).transform.GetComponent<RectTransform>().localRotation = Quaternion.Euler(90, 90, 0);// (90, 180, 90);
                                }

                                if (Arrow.name.Contains("_height"))
                                {


                                    Arrow.transform.GetChild(1).transform.GetComponent<RectTransform>().localRotation = Quaternion.Euler(90, 0, 90);

                                    TextMeshPro tmpro = Arrow.transform.GetChild(1).GetComponent<TextMeshPro>();
                                    float f_s = tmpro.fontSize;//fontSize

                                    if (view == Views._FIELD_SECTION_1.ToString())
                                    {   if(type_2==true)
                                        Arrow.transform.GetChild(1).transform.Translate(-Pergola_Model.transform.right * f_s / 2, Space.World);
                                        else Arrow.transform.GetChild(1).transform.Translate(-Pergola_Model.transform.forward * f_s / 2, Space.World);
                                    }
                                }
                            }
                            if (view == Views._FIELD_SECTION_3.ToString())
                            {
                                if (Arrow.name.Contains("Arrow_Field"))
                                {
                                    Arrow.transform.GetChild(1).transform.GetComponent<RectTransform>().localRotation = Quaternion.Euler(90, 90, 0);// (90, 180, 90);
                                }
                                if(Arrow.name.Contains("height"))
                                {
                                    Arrow.transform.GetChild(1).transform.GetComponent<RectTransform>().localRotation = Quaternion.Euler(90, 180, -90);// (90, 180, 90);

                                    TextMeshPro tmpro = Arrow.transform.GetChild(1).GetComponent<TextMeshPro>();
                                    float f_s = tmpro.fontSize;

                                    Arrow.transform.GetChild(1).transform.Translate(-Pergola_Model.transform.forward * f_s / 2, Space.World);
                                }
                            }

                            else if (view == Views._SIDE_FIELD_SECTION_3.ToString()|| view == Views._SIDE_FIELD_SECTION_1.ToString())
                            {
                                if (Arrow.name.Contains("Arrow_FrameF"))
                                {
                                    GameObject txt = Arrow.GetChild(1).gameObject;

                                    var recttrns = Arrow.GetChild(1).GetComponent<RectTransform>();

                                    recttrns.transform.localRotation = Quaternion.Euler(-90, 0, 90);

                                    //recttrns.transform.Translate(Vector3.up * recttrns.sizeDelta.y, Space.Self);
                                    //recttrns.Rotate(Vector3.forward, 90);

                                    //Here we have an extra text to indicate the section count in Pergola
                                    if (Arrow.childCount > 0)
                                        if (Arrow.GetChild(0).childCount > 1)
                                            if (Arrow.GetChild(0).GetChild(1) != null)
                                            {
                                                TextMeshPro Arrow_Head_tmpro = Arrow.GetChild(0).GetChild(1).GetComponent<TextMeshPro>();
                                                //Adding Field section count
                                                int f_count = 0;

                                                if (view == Views._SIDE_FIELD_SECTION_1.ToString())
                                                {
                                                    f_count = sc_cnv_regio3;
                                                }
                                                else
                                                {
                                                    f_count = sc_cnv_regio2;
                                                }
                                                Arrow_Head_tmpro.text = "X" + f_count;// (FieldDividers_Parent_Section_0002.transform.childCount + FrameDividers_Parent_Section_0002.transform.childCount + 1);

                                                var r_tr = Arrow_Head_tmpro.transform.GetComponent<RectTransform>();

                                                //r_tr.localRotation = Quaternion.Euler(120, -90, -90);// (0, 90, 90);
                                                //r_tr.transform.localRotation = Quaternion.Euler(90, -90, -90);
                                                r_tr.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                                                GameObject cylinder_Parent = Arrow.transform.GetChild(2).gameObject;

                                                r_tr.Translate(-(Pergola_Model.transform.forward) * (Vector3.Distance(cylinder_Parent.transform.Find("Start_Cube").transform.position, cylinder_Parent.transform.Find("End_Cube").transform.position) - r_tr.sizeDelta.y), Space.World);
                                                r_tr.Translate(Vector3.right * r_tr.sizeDelta.x, Space.Self);
                                            }
                                }
                            }


                            if (Arrow.name.Contains("Arrow_FrameC"))
                            {
                                var recttrns = Arrow.GetChild(1).GetComponent<RectTransform>();

                                recttrns.transform.localRotation = Quaternion.Euler(90, -90, 0);
                                //TextMeshPro tmpro = Arrow.GetChild(1).GetComponent<TextMeshPro>();
                                //float f_s = tmpro.fontSize;

                                //Here we have an extra text to indicate the section count in Pergola
                                if (Arrow.childCount > 0)
                                    if (Arrow.GetChild(0).childCount > 1)
                                        if (Arrow.GetChild(0).GetChild(1) != null)
                                        {
                                            TextMeshPro Arrow_Head_tmpro = Arrow.GetChild(0).GetChild(1).GetComponent<TextMeshPro>();
                                            //Adding Field section count
                                            int f_count = 0;

                                            if(view==Views._SIDE_FIELD_SECTION_1.ToString())
                                            {
                                                f_count = sc_cnv_regio3;
                                            }
                                            else
                                            {
                                                f_count = sc_cnv_regio1;
                                            }

                                            Arrow_Head_tmpro.text = "X" + f_count;// (FieldDividers_Parent_Section_0001.transform.childCount + FrameDividers_Parent_Section_0001.transform.childCount + 1);

                                            var r_tr = Arrow_Head_tmpro.transform.GetComponent<RectTransform>();



                                            r_tr.Translate(Vector3.right * r_tr.sizeDelta.x, Space.Self);

                                            //r_tr.localRotation = Quaternion.Euler(120, -90, -90);// (0, 90, 90);
                                            r_tr.transform.localRotation = Quaternion.Euler(90, -90, -90);
                                        }
                            }

                            if (Arrow.name.Contains("Arrow_FrameC") && view == Views._FRONT.ToString())
                            {
                                var recttrns = Arrow.GetChild(1).GetComponent<RectTransform>();

                                recttrns.transform.localRotation = Quaternion.Euler(0, 90, 0);

                            }
                            if(view==Views._B_B.ToString())
                            {
                                if(Arrow.name.Contains("Arrow_FrameB"))
                                {
                                    var recttrns = Arrow.GetChild(1).GetComponent<RectTransform>();

                                    recttrns.transform.localRotation = Quaternion.Euler(180, -90, 180);
                                }
                            }
                        }
                }
            }
            else if(DB_script.U_type)
            {
                if (GameObject.Find("Pergola_Model") != null)
                {
                    GameObject Pergola_Model = GameObject.Find("Pergola_Model");

                    GameObject frame_A;
                    if (GameObject.Find("FrameA_0"))
                    {
                        frame_A = GameObject.Find("FrameA_0");
                    }
                    else
                    {

                        frame_A = GameObject.Find("FrameA");
                    }

                    GameObject frame_B;

                    if (GameObject.Find("FrameB_0"))
                    {
                        frame_B = GameObject.Find("FrameB_0");
                    }
                    else
                    {
                        frame_B = GameObject.Find("FrameB");
                    }


                    GameObject frame_C;
                    if (GameObject.Find("FrameC_0"))
                    {
                        frame_C = GameObject.Find("FrameC_0");
                    }
                    else
                    {
                        frame_C = GameObject.Find("FrameC");
                    }
                    GameObject frame_D;

                    if (GameObject.Find("FrameD_0"))
                    {
                        frame_D = GameObject.Find("FrameD_0");
                    }
                    else
                    {
                        frame_D = GameObject.Find("FrameD");
                    }


                    GameObject frame_E;

                    if (GameObject.Find("FrameE_0"))
                    {
                        frame_E = GameObject.Find("FrameE_0");
                    }
                    else
                    {
                        frame_E = GameObject.Find("FrameE");
                    }

                    GameObject frame_F;

                    if (GameObject.Find("FrameF_0"))
                    {
                        frame_F = GameObject.Find("FrameF_0");
                    }
                    else
                    {
                        frame_F = GameObject.Find("FrameF");
                    }

                    GameObject frame_G;

                    if (GameObject.Find("FrameG_0"))
                    {
                        frame_G = GameObject.Find("FrameG_0");
                    }
                    else
                    {
                        frame_G = GameObject.Find("FrameG");
                    }

                    GameObject frame_H;

                    if (GameObject.Find("FrameH_0"))
                    {
                        frame_H = GameObject.Find("FrameH_0");
                    }
                    else
                    {
                        frame_H = GameObject.Find("FrameH");
                    }

                    Bounds bound_a = frame_A.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    Bounds bound_b = frame_B.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    Bounds bound_c = frame_C.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    Bounds bound_d = frame_D.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;


                    Bounds bound_e = frame_E.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;


                    Bounds bound_f = frame_F.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    Bounds bound_g = frame_G.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    Bounds bound_h = frame_H.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;



                    #region Arrow for Frame_A 
                    float pergola_forward_offset = -DB_script.frame_A_length / 2;

                    Vector3 dotted_line_frame_A_dir = Pergola_Model.transform.forward;

                    float fwd_offset = Mathf.Abs(Vector3.Dot(frame_A.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_a.size)) + 300;

                    float right_offset = DB_script.frame_A_length/2;// frame_A.transform.localScale.y / 2;
                    if (GameObject.Find("FrameA_0"))
                    {
                        right_offset = DB_script.frame_A_length - frame_A.transform.localScale.y / 2;

                    }

                    arrow_parm arrow_Frame_A_params = new arrow_parm()
                    {
                        position_of_arrow = frame_A.transform.TransformPoint(bound_a.center),
                        direction_of_arrow = -(Pergola_Model.transform.right),
                        length_of_arrow =DB_script.frame_A_length,// Mathf.Abs(frame_A.transform.localScale.y),
                        arrow_name = "Arrow_" + frame_A.name,



                        pergola_fwd = fwd_offset,// pergola_forward_offset,


                        pergola_right = right_offset,//Mathf.Abs(Vector3.Dot(frame_A.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_A.size)) + 300,

                        pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_A.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_A.size))+300

                        dotted_line_offset = fwd_offset,

                        Dotted_line_Direction = Pergola_Model.transform.forward,

                        orientation_dotted_lines = "horizontal",

                        txt=DB_script.frame_A_length.ToString(),

                    };

                    //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                    arrows_function(arrow_Frame_A_params);


                    #endregion



                    #region Arrow for Frame_C 
                     pergola_forward_offset = -DB_script.frame_C_length / 2;

                    Vector3 dotted_line_frame_C_dir = Pergola_Model.transform.forward;

                     fwd_offset = Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_c.size)) + 300;

                     right_offset = frame_C.transform.localScale.y / 2;
                    if (GameObject.Find("FrameC_0"))
                    {
                        right_offset = DB_script.frame_C_length - frame_C.transform.localScale.y / 2;

                    }

                    arrow_parm arrow_Frame_C_params = new arrow_parm()
                    {
                        position_of_arrow = frame_C.transform.TransformPoint(bound_c.center),
                        direction_of_arrow = -(Pergola_Model.transform.right),
                        length_of_arrow = Mathf.Abs(frame_C.transform.localScale.y),
                        arrow_name = "Arrow_" + frame_C.name,



                        pergola_fwd = fwd_offset,// pergola_forward_offset,


                        pergola_right = right_offset,//Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_C.size)) + 300,

                        pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_C.size))+300

                        dotted_line_offset = fwd_offset,

                        Dotted_line_Direction = Pergola_Model.transform.forward,

                        orientation_dotted_lines = "horizontal",

                        txt = DB_script.frame_C_length.ToString(),

                    };

                    //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                    arrows_function(arrow_Frame_C_params);


                    #endregion


                    #region Arrow for Frame_G 
                    pergola_forward_offset = -DB_script.frame_G_length / 2;

                    Vector3 dotted_line_frame_G_dir = Pergola_Model.transform.forward;

                    fwd_offset = Mathf.Abs(Vector3.Dot(frame_G.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_g.size)) + 300;

                    right_offset = frame_G.transform.localScale.y / 2;
                    if (GameObject.Find("FrameG_0"))
                    {
                        right_offset = DB_script.frame_G_length - frame_G.transform.localScale.y / 2;

                    }

                    arrow_parm arrow_Frame_G_params = new arrow_parm()
                    {
                        position_of_arrow = frame_G.transform.TransformPoint(bound_g.center),
                        direction_of_arrow = -(Pergola_Model.transform.right),
                        length_of_arrow = Mathf.Abs(frame_G.transform.localScale.y),
                        arrow_name = "Arrow_" + frame_G.name,



                        pergola_fwd = fwd_offset,// pergola_forward_offset,


                        pergola_right = right_offset,//Mathf.Abs(Vector3.Dot(frame_G.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_G.size)) + 300,

                        pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_G.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_G.size))+300

                        dotted_line_offset = fwd_offset,

                        Dotted_line_Direction = Pergola_Model.transform.forward,

                        orientation_dotted_lines = "horizontal",

                        txt = DB_script.frame_G_length.ToString(),

                    };

                    //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                    arrows_function(arrow_Frame_G_params);


                    #endregion



                    #region Arrow for Frame_E 
                    pergola_forward_offset = -DB_script.frame_E_length / 2;

                    Vector3 dotted_line_frame_E_dir = Pergola_Model.transform.forward;

                    fwd_offset = Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_e.size)) + 300;

                    right_offset = frame_E.transform.localScale.y / 2;
                    if (GameObject.Find("FrameF_0"))
                    {
                        right_offset = DB_script.frame_E_length - frame_E.transform.localScale.y / 2;

                    }

                    arrow_parm arrow_Erame_E_params = new arrow_parm()
                    {
                        position_of_arrow = frame_E.transform.TransformPoint(bound_e.center),
                        direction_of_arrow = -(Pergola_Model.transform.right),
                        length_of_arrow = Mathf.Abs(frame_E.transform.localScale.y),
                        arrow_name = "Arrow_" + frame_E.name,



                        pergola_fwd = -fwd_offset,// pergola_forward_offset,


                        pergola_right = right_offset,//Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.forward), bound_E.size)) + 300,

                        pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_E.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_E.size))+300

                        dotted_line_offset = -fwd_offset,

                        Dotted_line_Direction = Pergola_Model.transform.forward,

                        orientation_dotted_lines = "horizontal",

                        txt = DB_script.frame_E_length.ToString(),

                    };

                    //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                    arrows_function(arrow_Erame_E_params);


                    #endregion

                    #region Arrow for Frame_D

                    Vector3 dotted_line_frame_D_dir = Pergola_Model.transform.right;

                    right_offset = Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_d.size)) + 300;

                    fwd_offset = frame_D.transform.localScale.y / 2;


                    if (GameObject.Find("FrameD_0"))
                    {
                        fwd_offset = DB_script.frame_D_length - frame_D.transform.localScale.y / 2;

                    }

                    arrow_parm arrow_Frame_D_params = new arrow_parm()
                    {
                        position_of_arrow = frame_D.transform.TransformPoint(bound_d.center),
                        direction_of_arrow = -(Pergola_Model.transform.forward),
                        length_of_arrow = DB_script.frame_D_length,
                        arrow_name = "Arrow_" + frame_D.name,



                        pergola_fwd = fwd_offset,


                        pergola_right = right_offset,//Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) + 100,

                        pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_D.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_D.size))+300


                        Dotted_line_Direction = Pergola_Model.transform.right,

                        dotted_line_offset = right_offset,

                        //orientation_dotted_lines = "horizontal"
                    };

                    //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                    arrows_function(arrow_Frame_D_params);

                    #endregion


                    #region Arrow for Frame_F

                    Vector3 dotted_line_frame_F_Fir = Pergola_Model.transform.right;

                    right_offset = Mathf.Abs(Vector3.Dot(frame_F.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_f.size)) + 300;

                    fwd_offset = frame_F.transform.localScale.y / 2;


                    if (GameObject.Find("FrameD_0"))
                    {
                        fwd_offset = DB_script.frame_F_length - frame_F.transform.localScale.y / 2;

                    }

                    arrow_parm arrow_Frame_F_params = new arrow_parm()
                    {
                        position_of_arrow = frame_F.transform.TransformPoint(bound_f.center),
                        direction_of_arrow = -(Pergola_Model.transform.forward),
                        length_of_arrow = DB_script.frame_F_length,
                        arrow_name = "Arrow_" + frame_F.name,



                        pergola_fwd = fwd_offset,


                        pergola_right = -right_offset,//Mathf.Abs(Vector3.Dot(frame_F.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_b.size)) + 100,

                        pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_F.transform.InverseTransformDirection(Pergola_Model.transform.up), bound_F.size))+300


                        Dotted_line_Direction = -Pergola_Model.transform.right,

                        dotted_line_offset = right_offset,

                        //orientation_Fotted_lines = "horizontal"
                    };

                    //await UnityMainThreadDispatcher.DispatchAsync(() => arrows_function("_Top"));
                    arrows_function(arrow_Frame_F_params);

                    #endregion

                }
            }
            //await UnityMainThreadDispatcher.DispatchAsync(() => sc_sh.Texture_Render(view));


            //await UnityMainThreadDispatcher.DispatchAsync(() => Reset_Script.Reset_Model());


            //await Task.Delay(100);
        }
        catch (Exception ex)
        {

            Debug.Log("Generating Arrows to the model: " + ex);
        }
        finally
        {
            if (DB_script.I_type || DB_script.L_type)
            {
                Screen_Shot sc_sh = Camera.main.GetComponent<Screen_Shot>();
                StartCoroutine(sc_sh.Texture_Render(view));
            }
        }

    }
    public GameObject arrows_function(arrow_parm ar = null)
    {
        //This function takes object of parameters of the arrow and Generate an arow

        if (GameObject.Find("Frames_Parent") != null)
            DB_script.Frames_Parent = GameObject.Find("Frames_Parent");

        if (GameObject.Find("Pergola_Model") != null)
            DB_script.Pergola_Model = GameObject.Find("Pergola_Model");

        if (GameObject.Find("Divider_Parent") != null)
            DB_script.Divider_Parent = GameObject.Find("Divider_Parent");


        if (GameObject.Find("FrameDividers_Parent") != null)
            DB_script.FrameDividers_Parent = GameObject.Find("FrameDividers_Parent");

        if (GameObject.Find("FieldDividers_Parent") != null)
            DB_script.FieldDividers_Parent = GameObject.Find("FieldDividers_Parent");

        if (GameObject.Find("Field_Parent") != null)
            DB_script.Field_Parent = GameObject.Find("Field_Parent");

        if (GameObject.Find("SupportBars_Parent") != null)
            DB_script.SupportBars_Parent = GameObject.Find("SupportBars_Parent");

        GameObject Pergola_Model = GameObject.Find("Pergola_Model");

        float min_distance_LEFT = ar.length_of_arrow;// frame_C.transform.localScale.y;
        GameObject Arrow = null;
        if (min_distance_LEFT >= 0)
        {
            Arrow = null;

            if (Arrow_Parent == null)
                Arrow_Parent = new GameObject("Arrow_Parent");
            else
                Arrow_Parent = GameObject.Find("Arrow_Parent");


            Arrow_Parent.transform.parent = Pergola_Model.transform;


            if (GameObject.Find($"Arrow_Parent/{ar.arrow_name}") != null && min_distance_LEFT > 110)
            {
                Arrow = GameObject.Find($"Arrow_Parent/{ar.arrow_name}");// Arrow_Parent.transform.Find("Left").gameObject;

            }
            else if (GameObject.Find($"Arrow_Parent/{ar.arrow_name}") == null && min_distance_LEFT > 110)
            {

                if (ar.arrow_prefab != null)
                {
                    Arrow_prefab = ar.arrow_prefab;
                }

                Arrow = Instantiate(Arrow_prefab, Arrow_Parent.transform);
                Arrow.name = ar.arrow_name;

            }
            if (Arrow != null)
            {
                Arrow.transform.localScale = Vector3.one * 100;

                Arrow.transform.position = ar.position_of_arrow;// bounds.center; //place at starting point

                float distance = Vector3.Distance(Arrow.transform.GetChild(2).GetChild(0).position, Arrow.transform.GetChild(2).GetChild(2).position);

                //Formula to scale cylinder
                float ratio = min_distance_LEFT / distance;
                Arrow.transform.GetChild(2).localScale = new Vector3(Arrow.transform.GetChild(2).localScale.x, Arrow.transform.GetChild(2).localScale.y, (Arrow.transform.GetChild(2).localScale.z * ratio) - 1); //100*0.2f is other vbar ear length     100*0.3f is arrow starting & ending point at center of vbar  [vbar is 100*0.6m is width], 100*0.2 from top width of vetical bar

                Vector3 cylinder_end_position = Arrow.transform.GetChild(2).GetChild(2).position;
                Arrow.transform.GetChild(3).position = cylinder_end_position;
                Arrow.transform.forward = ar.direction_of_arrow;// (Pergola_Model.transform.forward); //look at left vertical bar

                Arrow.transform.GetChild(3).transform.Translate(-Vector3.forward * (50f));

                float dimention_distance = Vector3.Distance(Arrow.transform.GetChild(0).GetChild(0).position, Arrow.transform.GetChild(3).GetChild(0).transform.position);

                TextMeshPro textMeshPro = Arrow.transform.GetChild(1).GetComponent<TextMeshPro>();
                float value = (float)Math.Floor(dimention_distance);//, 1);

                if (ar.txt != null && ar.txt != dummy_text)
                    textMeshPro.text = (ar.txt).ToString() + " mm";
                else if (ar.txt == dummy_text)
                    textMeshPro.text = " ";
                else
                    textMeshPro.text = ((int)value).ToString() + " mm";

                float font_size = 514.7f;

                //float new_font_size = ar.length_of_arrow/2 ;

                ////if (new_font_size > 300)
                ////{

                ////    textMeshPro.fontSize = font_size;
                ////}
                ////else
                ////{
                ////    textMeshPro.fontSize = 300;
                ////}
                //if(new_font_size>1000)
                //{
                //    new_font_size = 1000;
                //}


                //textMeshPro.fontSize = new_font_size;


                //Vector2 previous_size_delta = textMeshPro.gameObject.GetComponent<RectTransform>().sizeDelta;

                //textMeshPro.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(new_font_size / 2, previous_size_delta.y);

                //if(ar.length_of_arrow<110)
                //{
                //    textMeshPro.fontSize = 300;
                //}


                Arrow.transform.GetChild(1).position = Arrow.transform.GetChild(2).GetChild(1).GetChild(0).position;



                if (ar.pergola_right != 0)
                {
                    Arrow.transform.Translate(Pergola_Model.transform.right * ar.pergola_right, Space.World);
                }

                if (ar.pergola_up != 0)
                {
                    Arrow.transform.Translate(Pergola_Model.transform.up * ar.pergola_up, Space.World);
                }

                if (ar.pergola_fwd != 0)
                {
                    Arrow.transform.Translate(Pergola_Model.transform.forward * ar.pergola_fwd, Space.World);
                }


                if (ar.txt_right != 0)
                {
                    textMeshPro.gameObject.transform.Translate(Pergola_Model.transform.right * ar.txt_right, Space.World);
                }

                if (ar.txt_up != 0)
                {
                    textMeshPro.gameObject.transform.Translate(Pergola_Model.transform.up * ar.txt_up, Space.World);
                }


                if (ar.txt_fwd != 0)
                {
                    textMeshPro.gameObject.transform.Translate(Pergola_Model.transform.forward * ar.txt_fwd, Space.World);
                }


                #region Dotted lines

                GameObject Arrow_Tail = Arrow.transform.GetChild(3).GetChild(0).gameObject;
                GameObject Arrow_Head = Arrow.transform.GetChild(0).GetChild(0).gameObject;

                //float distance_of_arrow_tips = Vector3.Distance(Arrow_Green.transform.GetChild(0).GetChild(0).position, Arrow_Green.transform.GetChild(3).GetChild(0).transform.position);

                //Destroying other game objects

                foreach (Transform Arrow_TipDotChild in Arrow_Tail.transform)
                    Destroy(Arrow_TipDotChild.gameObject);

                foreach (Transform Arrow_headDotChild in Arrow_Head.transform)
                    Destroy(Arrow_headDotChild.gameObject);




                //from tip position taking end position by adding diatance in  "Parent.transform.forward" direction
                Vector3 Arrow_Head_dot_end_tip_pos = Arrow_Head.transform.position - ar.Dotted_line_Direction * ar.dotted_line_offset;

                Vector3 Arrow_Tail_dot_end_tip_pos = Arrow_Tail.transform.position - ar.Dotted_line_Direction * ar.dotted_line_offset;

                string orientation_of_dotted_lines = "";
                if (String.IsNullOrEmpty(ar.orientation_dotted_lines))
                {
                    //default orientation
                    orientation_of_dotted_lines = "vertical";
                }
                else
                {
                    orientation_of_dotted_lines = ar.orientation_dotted_lines;
                }

                dotted_Line_Custom_script.DrawDottedLine(Arrow_Head.transform.position, Arrow_Head_dot_end_tip_pos, Arrow_Head, orientation_of_dotted_lines);

                //if the arrow is single headed no need for tail dotted lines
                if (!Arrow_prefab.name.Contains("Arrow_Single_Head"))
                    dotted_Line_Custom_script.DrawDottedLine(Arrow_Tail.transform.position, Arrow_Tail_dot_end_tip_pos, Arrow_Tail, orientation_of_dotted_lines);

                Vector3 Dotted_line_fwd_visibility_dir = Pergola_Model.transform.up;

                if (ar.Dotted_facing_direction != Vector3.zero)
                {
                    Dotted_line_fwd_visibility_dir = ar.Dotted_facing_direction;
                }

                foreach (Transform dots in Arrow_Head.transform)
                {
                    dots.transform.forward = Dotted_line_fwd_visibility_dir;
                }

                foreach (Transform dots in Arrow_Tail.transform)
                {
                    dots.transform.forward = Dotted_line_fwd_visibility_dir;
                }
                #endregion
            }
            //destroy Arrows if smaller than the width of two Arrow heads(2*50=100)
            if (min_distance_LEFT < 110)
            {
                //Debug.Log("Destroyed Arrows");

                if (Arrow != null)
                    Destroy(Arrow);
            }



        }

        return Arrow;
    }
    // Update is called once per frame
    void Update()
    {

    }

    //public async void Scale_fields_Accs_Frame(float scale_fact = 5)
    public void Scale_fields_Accs_Frame(float scale_fact)
    {
        try 
        {
        //scale_fact = scale_model_factor;
        if (GameObject.Find("Frames_Parent") != null)
            DB_script.Frames_Parent = GameObject.Find("Frames_Parent");

        if (GameObject.Find("Pergola_Model") != null)
            DB_script.Pergola_Model = GameObject.Find("Pergola_Model");

        if (GameObject.Find("Divider_Parent") != null)
            DB_script.Divider_Parent = GameObject.Find("Divider_Parent");


        if (GameObject.Find("FrameDividers_Parent") != null)
            DB_script.FrameDividers_Parent = GameObject.Find("FrameDividers_Parent");

        if (GameObject.Find("FieldDividers_Parent") != null)
            DB_script.FieldDividers_Parent = GameObject.Find("FieldDividers_Parent");

        if (GameObject.Find("Field_Parent") != null)
            DB_script.Field_Parent = GameObject.Find("Field_Parent");

        if (GameObject.Find("SupportBars_Parent") != null)
            DB_script.SupportBars_Parent = GameObject.Find("SupportBars_Parent");

        if (DB_script.I_type)
        {
            //**************here we Scale frame A , B ,fields, related accessories for model to apperar bigger in screen shot

            bool scale_further = false;

            if (Arrows_Measure.dividers_section1_z_sort.Count > 0  )
            {
                if(Arrows_Measure.dividers_section1_z_sort.Values[0].localScale.z == 1)
                scale_further = true;
            }

            //TO DO
            //float scale_fact = 1;
            //if (DB_script.FieldDividers_Parent.transform.childCount > 1|| DB_script.FrameDividers_Parent.transform.childCount > 1)
            //if (DB_script.FieldDividers_Parent.transform.GetChild(0).childCount > 1 || DB_script.FrameDividers_Parent.transform.GetChild(0).childCount > 1)
            if(scale_further)
            {
                    //if( Math.Round(DB_script.FieldDividers_Parent.transform.GetChild(0).GetChild(0).transform.localScale.z) == 1|| Math.Round(DB_script.FrameDividers_Parent.transform.GetChild(0).GetChild(0).transform.localScale.z) == 1)
                    
                    GameObject Pergola_Model = GameObject.Find("Pergola_Model");


                    Dictionary<Transform, float> frames_local_z = new Dictionary<Transform, float>();

                    Dictionary<Transform, float> field_div_local_z = new Dictionary<Transform, float>();

                    Dictionary<Transform, float> frame_div_local_z = new Dictionary<Transform, float>();

                    Dictionary<Transform, float> Accs_local_z = new Dictionary<Transform, float>();



                    foreach (Transform frame_subparent in DB_script.Frames_Parent.transform)
                    {
                        try
                        {


                            if (frame_subparent.name == "Frame_Parent")
                                foreach (Transform frms in frame_subparent)
                                {
                                    Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frms.transform.position);

                                    //Bounds b_frm = frms.GetComponentInChildren<MeshFilter>().mesh.bounds;


                                    //Vector3 global_center = frms.TransformPoint(b_frm.center);
                                    //Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(global_center);

                                    frames_local_z.Add(frms, pos_wrt_pergola_model.z);
                                }
                        }
                        catch (Exception ex)
                        {

                            print("frame_subparent: " + ex);
                        }
                    }

                    foreach (Transform field_div in DB_script.FieldDividers_Parent.transform)
                    {
                        if (field_div.GetChild(0) != null)
                        {

                            Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(field_div.GetChild(0).transform.position);
                            //Bounds b_field_div = field_div.GetComponentInChildren<MeshFilter>().mesh.bounds;


                            //Vector3 global_center = field_div.TransformPoint(b_field_div.center);

                            //Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(global_center);

                            field_div_local_z.Add(field_div.GetChild(0), pos_wrt_pergola_model.z);
                        }
                    }

                    foreach (Transform frame_div in DB_script.FrameDividers_Parent.transform)
                    {
                        if (frame_div.GetChild(0) != null)
                        {
                            Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frame_div.GetChild(0).transform.position);


                            //Bounds b_field_div = frame_div.GetComponentInChildren<MeshFilter>().mesh.bounds;


                            //Vector3 global_center = frame_div.TransformPoint(b_field_div.center);

                            //Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(global_center);

                            frame_div_local_z.Add(frame_div.GetChild(0), pos_wrt_pergola_model.z);
                        }
                    }


                    // Field_Parent / Field_Group_0001 / Accessories_1 / L_Accessory_bottomLeft_1
                    foreach (string Accs_name in DB_script.Accessories_name)
                    {
                        try
                        {


                            GameObject Accs = GameObject.Find(Accs_name);

                            Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(Accs.transform.position);

                            Accs_local_z.Add(Accs.transform, pos_wrt_pergola_model.z);
                        }
                        catch (Exception ex)
                        {

                            print("Accs_name :+" + ex);
                        }
                    }


                    var sortedDict_z = from entry in frame_div_local_z orderby entry.Value ascending select entry;

                    //field_Dividers_local_x.Clear();

                    frame_div_local_z = new Dictionary<Transform, float>();
                    frame_div_local_z = sortedDict_z.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//is sorted wrt to its local x/y/z axis value

                    sortedDict_z = from entry in field_div_local_z orderby entry.Value ascending select entry;

                    //field_Dividers_local_x.Clear();

                    field_div_local_z = new Dictionary<Transform, float>();
                    field_div_local_z = sortedDict_z.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//is sorted wrt to its local x/y/z axis value


                    sortedDict_z = from entry in frame_div_local_z orderby entry.Value ascending select entry;

                    //field_Dividers_local_x.Clear();

                    frame_div_local_z = new Dictionary<Transform, float>();
                    frame_div_local_z = sortedDict_z.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//is sorted wrt to its local x/y/z axis value

                    sortedDict_z = from entry in Accs_local_z orderby entry.Value ascending select entry;

                    //field_Dividers_local_x.Clear();

                    Accs_local_z = new Dictionary<Transform, float>();
                    Accs_local_z = sortedDict_z.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//is sorted wrt to its local x/y/z axis value

                    //Dictionary<Transform, Transform> Accs_and_parent = new Dictionary<Transform, Transform>();

                    //used to store Accs_that are parented and to remove from dictionary
                    List<Transform> Accs_parented = new List<Transform>();

                    //Here we make the L&U Accessories that are nearer to the frames their child
                    if (frames_local_z.Count > 0)
                        foreach (Transform frame in frames_local_z.Keys)
                        {
                            string frame1_name = "FrameB", frame2_name = "FrameD";

                            if (DB_script.L_type == true)
                            {
                                frame1_name = "FrameE";

                                frame2_name = "FrameC";
                            }

                            if ((frame.name.Contains(frame1_name) || frame.name.Contains(frame2_name)))
                            {
                                try
                                {


                                    if (frame != null)
                                    {

                                        GameObject frm = frame.gameObject;

                                        Vector3 pos_Frm_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frm.transform.position);

                                        Bounds f_bound;





                                        if (frm.transform.GetComponentInChildren<BoxCollider>() != null)
                                            f_bound = frm.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                        else
                                        {
                                            frm.AddComponent<BoxCollider>();
                                            f_bound = frm.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                        }

                                        float frm_bound_max_size = Mathf.Max(f_bound.size.y, f_bound.size.z);
                                        if (Accs_local_z.Count > 0)
                                            foreach (Transform accs in Accs_local_z.Keys)
                                            {
                                                //Accs_and_parent.Add(accs, accs.parent);

                                                Characteristics chars_script = accs.GetComponent<Characteristics>();

                                                chars_script.previous_parent_transform = accs.parent;

                                                Vector3 pos_Accs_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(accs.transform.position);
                                                if (accs != null)
                                                {
                                                    //checking if distance difference is less than the bounds of divider
                                                    if (Mathf.Abs(frames_local_z[frame] - Accs_local_z[accs]) <= 2 * frm_bound_max_size)
                                                    {


                                                        accs.transform.parent = frm.transform;

                                                        //Adding Accs to List  REMOVING KEY AFTER PARENTING 
                                                        Accs_parented.Add(accs);
                                                    }

                                                }
                                            }
                                    }
                                }
                                catch (Exception ex)
                                {

                                    Debug.Log("frames Accs:" + ex);
                                }

                            }
                        }


                    foreach (Transform accs_par in Accs_parented)
                    {
                        if (Accs_local_z.ContainsKey(accs_par))
                        {
                            Accs_local_z.Remove(accs_par);
                        }
                    }


                    Accs_parented.Clear();


                    foreach (Transform frame in frames_local_z.Keys)
                    {
                        try
                        {

                            string frame1_name = "FrameB", frame2_name = "FrameD";

                            if (DB_script.L_type == true)
                            {
                                frame1_name = "FrameE";

                                frame2_name = "FrameC";
                            }

                            if ((frame.name.Contains(frame1_name) || frame.name.Contains(frame2_name)))
                            {
                                GameObject frm_par_dummy = new GameObject($"frm_par_dummy_{frame.name}");

                                int sib_index = frame.GetSiblingIndex();

                                Vector3 orig_pos = frame.position;

                                //taking only up component ,of pergola_model_direction

                                Vector3 orig_pos_up = Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                                //placing empty GameObject at center
                                frm_par_dummy.transform.position = frame.transform.TransformPoint(frame.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                                Transform prev_par = frame.parent;

                                //int si_ind = frame.GetSiblingIndex();

                                frame.transform.parent = frm_par_dummy.transform;

                                frm_par_dummy.transform.localScale = Vector3.one * scale_fact;


                                Vector3 new_pos = frame.position - Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, frame.position);

                                new_pos = new_pos + orig_pos_up;// Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                                frame.transform.position = new_pos;// orig_pos;

                                frame.transform.parent = prev_par;

                                frame.SetAsFirstSibling();

                                //frame.SetSiblingIndex(sib_index);

                                DestroyImmediate(frm_par_dummy);
                            }
                        }
                        catch (Exception ex)
                        {

                            Debug.Log("frame :" + ex);
                        }

                    }

                    Accs_parented = new List<Transform>();

                    //**!st loop for Accessories here we find the parent of accessories**************//

                    //Here we make the L & U Accessories that are nearer to the field divider as their child
                    if (field_div_local_z.Count > 0)
                        foreach (Transform field_div in field_div_local_z.Keys)
                        {

                            try
                            {



                                if (field_div != null)
                                {

                                    GameObject f_div = field_div.gameObject;

                                    Vector3 pos_Frm_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(f_div.transform.position);

                                    Bounds f_bound;





                                    if (f_div.transform.GetComponentInChildren<BoxCollider>() != null)
                                        f_bound = f_div.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                    else
                                    {
                                        f_div.AddComponent<BoxCollider>();
                                        f_bound = f_div.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    }

                                    float frm_bound_max_size = Mathf.Max(f_bound.size.y, f_bound.size.z);
                                    if (Accs_local_z.Count > 0)
                                        foreach (Transform accs in Accs_local_z.Keys)
                                        {
                                            //Accs_and_parent.Add(accs, accs.parent);

                                            //Characteristics chars_script = accs.GetComponent<Characteristics>();

                                            //chars_script.previous_parent_transform = accs.parent;

                                            Vector3 pos_Accs_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(accs.transform.position);
                                            if (accs != null)
                                            {
                                                //checking if distance difference is less than the bounds of divider
                                                if (Mathf.Abs(field_div_local_z[field_div] - Accs_local_z[accs]) <= 2.5f * frm_bound_max_size)
                                                {


                                                    accs.transform.parent = f_div.transform;


                                                    //Adding Accs to List  REMOVING KEY AFTER PARENTING 
                                                    Accs_parented.Add(accs);


                                                }

                                            }
                                        }
                                }
                            }
                            catch (Exception ex)
                            {

                                print("field_div_local_z accs :" + ex);
                            }
                        }

                    foreach (Transform accs_par in Accs_parented)
                    {
                        if (Accs_local_z.ContainsKey(accs_par))
                        {
                            Accs_local_z.Remove(accs_par);
                        }
                    }


                    Accs_parented.Clear();

                    foreach (Transform field_divider in field_div_local_z.Keys)
                    {
                        try
                        {


                            GameObject div_par_dummy = new GameObject($"FLD_DIV_par_dummy_{field_divider.name}");

                            int sib_index = field_divider.GetSiblingIndex();

                            Vector3 orig_pos = field_divider.position;

                            //taking only up component ,of pergola_model_direction

                            Vector3 orig_pos_up = Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                            //placing empty GameObject at center
                            div_par_dummy.transform.position = field_divider.transform.TransformPoint(field_divider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                            Transform prev_par = field_divider.parent;

                            field_divider.transform.parent = div_par_dummy.transform;

                            div_par_dummy.transform.localScale = Vector3.one * scale_fact;


                            Vector3 new_pos = field_divider.position - Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, field_divider.position);

                            new_pos = new_pos + orig_pos_up;// Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                            field_divider.transform.position = new_pos;// orig_pos;

                            field_divider.transform.parent = prev_par;

                            field_divider.SetAsFirstSibling();

                            field_divider.SetSiblingIndex(sib_index);

                            DestroyImmediate(div_par_dummy);
                        }
                        catch (Exception ex)
                        {

                            print("field_divider :" + ex);
                        }

                    }

                    Accs_parented = new List<Transform>();
                    //Here we make the L&U Accessories that are nearer to the field divider as their child
                    if (frame_div_local_z.Count > 0)
                        foreach (Transform frame_div in frame_div_local_z.Keys)
                        {
                            try
                            {

                                if (frame_div != null)
                                {

                                    GameObject frm_div = frame_div.gameObject;

                                    Vector3 pos_Frm_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frm_div.transform.position);

                                    Bounds f_bound;





                                    if (frm_div.transform.GetComponentInChildren<BoxCollider>() != null)
                                        f_bound = frm_div.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                    else
                                    {
                                        frm_div.AddComponent<BoxCollider>();
                                        f_bound = frm_div.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    }

                                    float frm_bound_max_size = Mathf.Max(f_bound.size.y, f_bound.size.z);
                                    if (Accs_local_z.Count > 0)
                                        foreach (Transform accs in Accs_local_z.Keys)
                                        {
                                            //Accs_and_parent.Add(accs, accs.parent);

                                            //Characteristics chars_script = accs.GetComponent<Characteristics>();

                                            //chars_script.previous_parent_transform = accs.parent;

                                            Vector3 pos_Accs_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(accs.transform.position);
                                            if (accs != null)
                                            {
                                                //checking if distance difference is less than the bounds of divider
                                                if (Mathf.Abs(frame_div_local_z[frame_div] - Accs_local_z[accs]) <= 2.5f * frm_bound_max_size)
                                                {


                                                    accs.transform.parent = frm_div.transform;


                                                    //Adding Accs to List  REMOVING KEY AFTER PARENTING 
                                                    Accs_parented.Add(accs);
                                                }

                                            }
                                        }
                                }
                            }
                            catch (Exception ex)
                            {

                                print("frame_div_local_z accs:" + ex);
                            }

                        }

                    foreach (Transform accs_par in Accs_parented)
                    {
                        if (Accs_local_z.ContainsKey(accs_par))
                        {
                            Accs_local_z.Remove(accs_par);
                        }
                    }


                    Accs_parented.Clear();

                    foreach (Transform frame_divider in frame_div_local_z.Keys)
                    {

                        try
                        {


                            GameObject Accs_par_dummy = new GameObject($"FRM_DIV_par_dummy_{frame_divider.name}");

                            int sib_index = frame_divider.GetSiblingIndex();

                            Vector3 orig_pos = frame_divider.position;

                            //taking only up component ,of pergola_model_direction

                            Vector3 orig_pos_up = Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                            //placing empty GameObject at center
                            Accs_par_dummy.transform.position = frame_divider.transform.TransformPoint(frame_divider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                            Transform prev_par = frame_divider.parent;

                            frame_divider.transform.parent = Accs_par_dummy.transform;

                            Accs_par_dummy.transform.localScale = Vector3.one * scale_fact;


                            Vector3 new_pos = frame_divider.position - Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, frame_divider.position);

                            new_pos = new_pos + orig_pos_up;// Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                            frame_divider.transform.position = new_pos;// orig_pos;

                            frame_divider.transform.parent = prev_par;

                            frame_divider.SetAsFirstSibling();

                            //frame_divider.SetSiblingIndex(sib_index);

                            DestroyImmediate(Accs_par_dummy);
                        }
                        catch (Exception ex)
                        {

                            print("frame_divider :" + ex);
                        }

                    }

                    try
                    {

                        foreach (string Accs_name in DB_script.Accessories_name)
                        {
                            Transform Accs = GameObject.Find(Accs_name).transform;

                            Characteristics chars_script = Accs.GetComponent<Characteristics>();
                            if (chars_script != null)
                                if (chars_script.previous_parent_transform != null)
                                {
                                    //Accs.transform.parent = chars_script.previous_parent_transform;

                                    Accs.SetParent(chars_script.previous_parent_transform, true);

                                    if (Accs.name.Contains("180005738") || Accs.name.Contains("180001258") || Accs.name.Contains("150000446") || Accs.name.Contains("ak - 109a") || Accs.name.Contains("L_Accessory_top"))
                                    {
                                        Accs.gameObject.SetActive(false);
                                    }
                                }

                        }
                    }
                    catch (Exception ex)
                    {

                        print("Reparenting :" + ex);
                    }
                
            }
        }
        else if (DB_script.L_type)
        {

            bool scale_further = false;

            if (Arrows_Measure.dividers_section1_z_sort.Count > 0)
            {
                if (Arrows_Measure.dividers_section1_z_sort.Values[0].localScale.z == 1)
                    scale_further = true;
            }
            //if (DB_script.FieldDividers_Parent.transform.GetChild(0).GetChild(0) != null && Math.Round(DB_script.FieldDividers_Parent.transform.GetChild(0).GetChild(0).transform.localScale.z) == 1)
            
            if(scale_further)
            {
                GameObject Pergola_Model = GameObject.Find("Pergola_Model");


                Dictionary<Transform, float> frames_local_z = new Dictionary<Transform, float>();

                Dictionary<Transform, float> field_div_local_z = new Dictionary<Transform, float>();

                Dictionary<Transform, float> frame_div_local_z = new Dictionary<Transform, float>();

                Dictionary<Transform, float> Accs_local_z = new Dictionary<Transform, float>();

                GameObject FrameDividers_Parent_Section_0001 = DB_script.FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0001").gameObject;

                GameObject FieldDividers_Parent_Section_0001 = DB_script.FieldDividers_Parent.transform.Find("FieldDividers_Parent_Section_0001").gameObject;

                foreach (Transform frame_subparent in DB_script.Frames_Parent.transform)
                {
                    try
                    {


                        if (frame_subparent.name == "Frame_Parent")
                            foreach (Transform frms in frame_subparent)
                            {
                                Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frms.transform.position);

                                //Bounds b_frm = frms.GetComponentInChildren<MeshFilter>().mesh.bounds;


                                //Vector3 global_center = frms.TransformPoint(b_frm.center);
                                //Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(global_center);

                                frames_local_z.Add(frms, pos_wrt_pergola_model.z);
                            }
                    }
                    catch (Exception ex)
                    {

                        print("frame_subparent: " + ex);
                    }
                }

                foreach (Transform field_div in FieldDividers_Parent_Section_0001.transform)
                {
                    foreach(Transform   div_ch in field_div )
                    {
                    // if (field_div.GetChild(0) != null)
                    if(div_ch.name.Contains("FieldDivider"))
                    {

                        Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(div_ch.transform.position);//field_div.GetChild(0)
                                                                                                                                                //Bounds b_field_div = field_div.GetComponentInChildren<MeshFilter>().mesh.bounds;


                                //Vector3 global_center = field_div.TransformPoint(b_field_div.center);

                                //Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(global_center);

                                field_div_local_z.Add(div_ch, pos_wrt_pergola_model.z);
                    }
                    }
                    //if (field_div != null)
                    //{

                    //    Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(field_div.transform.position);
                    //    //Bounds b_field_div = field_div.GetComponentInChildren<MeshFilter>().mesh.bounds;


                    //    //Vector3 global_center = field_div.TransformPoint(b_field_div.center);

                    //    //Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(global_center);

                    //    field_div_local_z.Add(field_div, pos_wrt_pergola_model.z);
                    //}
                }

                foreach (Transform frame_div in FrameDividers_Parent_Section_0001.transform)
                {
                    foreach(Transform div_ch in frame_div)
                    {
                    // if (frame_div.GetChild(0) != null)
                    if(div_ch.name.Contains("FrameDivider"))
                    {

                        Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(div_ch.transform.position);
                        //Bounds b_field_div = field_div.GetComponentInChildren<MeshFilter>().mesh.bounds;


                        //Vector3 global_center = field_div.TransformPoint(b_field_div.center);

                        //Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(global_center);

                        frame_div_local_z.Add(div_ch, pos_wrt_pergola_model.z);
                    }
                    }

                    //if (frame_div.name.Contains("FrameDivider"))
                    //{
                    //    Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frame_div.GetChild(0).transform.position);


                    //    //Bounds b_field_div = frame_div.GetComponentInChildren<MeshFilter>().mesh.bounds;


                    //    //Vector3 global_center = frame_div.TransformPoint(b_field_div.center);

                    //    //Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(global_center);
                    //    if(!frame_div_local_z.ContainsKey(frame_div))
                    //    frame_div_local_z.Add(frame_div, pos_wrt_pergola_model.z);
                    //}

                    //if (frame_div != null)
                    //{
                    //    Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frame_div.transform.position);


                    //    //Bounds b_field_div = frame_div.GetComponentInChildren<MeshFilter>().mesh.bounds;


                    //    //Vector3 global_center = frame_div.TransformPoint(b_field_div.center);

                    //    //Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(global_center);

                    //    frame_div_local_z.Add(frame_div, pos_wrt_pergola_model.z);
                    //}
                }


                // Field_Parent / Field_Group_0001 / Accessories_1 / L_Accessory_bottomLeft_1
                foreach (string Accs_name in DB_script.Accessories_name)
                {
                    try
                    {


                        GameObject Accs = GameObject.Find(Accs_name);
                        if (Accs.transform.parent.parent.parent != null)
                            if (Accs.transform.parent.parent.parent.name.Contains("Field_Parent_Section_0001"))
                            {
                                Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(Accs.transform.position);

                                Accs_local_z.Add(Accs.transform, pos_wrt_pergola_model.z);
                            }
                    }
                    catch (Exception ex)
                    {

                        print("Accs_name :+" + ex);
                    }
                }


                var sortedDict_z = from entry in frame_div_local_z orderby entry.Value ascending select entry;

                //field_Dividers_local_x.Clear();

                frame_div_local_z = new Dictionary<Transform, float>();
                frame_div_local_z = sortedDict_z.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//is sorted wrt to its local x/y/z axis value

                sortedDict_z = from entry in field_div_local_z orderby entry.Value ascending select entry;

                //field_Dividers_local_x.Clear();

                field_div_local_z = new Dictionary<Transform, float>();
                field_div_local_z = sortedDict_z.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//is sorted wrt to its local x/y/z axis value


                sortedDict_z = from entry in frame_div_local_z orderby entry.Value ascending select entry;

                //field_Dividers_local_x.Clear();

                frame_div_local_z = new Dictionary<Transform, float>();
                frame_div_local_z = sortedDict_z.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//is sorted wrt to its local x/y/z axis value

                sortedDict_z = from entry in Accs_local_z orderby entry.Value ascending select entry;

                //field_Dividers_local_x.Clear();

                Accs_local_z = new Dictionary<Transform, float>();
                Accs_local_z = sortedDict_z.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//is sorted wrt to its local x/y/z axis value

                //Dictionary<Transform, Transform> Accs_and_parent = new Dictionary<Transform, Transform>();

                //used to store Accs_that are parented and to remove from dictionary
                List<Transform> Accs_parented = new List<Transform>();

                //Here we make the L&U Accessories that are nearer to the frames their child
                if (frames_local_z.Count > 0)
                    foreach (Transform frame in frames_local_z.Keys)
                    {
                        string frame1_name = "FrameB", frame2_name = "FrameD";

                        if (DB_script.L_type == true)
                        {
                            frame1_name = "FrameE";

                            frame2_name = "FrameC";
                        }

                        if ((frame.name.Contains(frame1_name) || frame.name.Contains(frame2_name)))
                        {
                            try
                            {


                                if (frame != null)
                                {

                                    GameObject frm = frame.gameObject;

                                    Vector3 pos_Frm_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frm.transform.position);

                                    Bounds f_bound;





                                    if (frm.transform.GetComponentInChildren<BoxCollider>() != null)
                                        f_bound = frm.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                    else
                                    {
                                        frm.AddComponent<BoxCollider>();
                                        f_bound = frm.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                    }

                                    float frm_bound_max_size = Mathf.Max(f_bound.size.y, f_bound.size.z);
                                    if (Accs_local_z.Count > 0)
                                        foreach (Transform accs in Accs_local_z.Keys)
                                        {
                                            //Accs_and_parent.Add(accs, accs.parent);

                                            Characteristics chars_script = accs.GetComponent<Characteristics>();

                                            chars_script.previous_parent_transform = accs.parent;

                                            Vector3 pos_Accs_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(accs.transform.position);
                                            if (accs != null)
                                            {
                                                //checking if distance difference is less than the bounds of divider
                                                if (Mathf.Abs(frames_local_z[frame] - Accs_local_z[accs]) <= 2 * frm_bound_max_size)
                                                {


                                                    accs.transform.parent = frm.transform;

                                                    //Adding Accs to List  REMOVING KEY AFTER PARENTING 
                                                    Accs_parented.Add(accs);
                                                }

                                            }
                                        }
                                }
                            }
                            catch (Exception ex)
                            {

                                Debug.Log("frames Accs:" + ex);
                            }

                        }
                    }


                foreach (Transform accs_par in Accs_parented)
                {
                    if (Accs_local_z.ContainsKey(accs_par))
                    {
                        Accs_local_z.Remove(accs_par);
                    }
                }


                Accs_parented.Clear();


                foreach (Transform frame in frames_local_z.Keys)
                {
                    try
                    {

                        string frame1_name = "FrameB", frame2_name = "FrameD";

                        if (DB_script.L_type == true)
                        {
                            frame1_name = "FrameE";

                            frame2_name = "FrameC";
                        }

                        if ((frame.name.Contains(frame1_name) || frame.name.Contains(frame2_name)))
                        {
                            GameObject frm_par_dummy = new GameObject($"frm_par_dummy_{frame.name}");

                            int sib_index = frame.GetSiblingIndex();

                            Vector3 orig_pos = frame.position;

                            //taking only up component ,of pergola_model_direction

                            Vector3 orig_pos_up = Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                            //placing empty GameObject at center
                            frm_par_dummy.transform.position = frame.transform.TransformPoint(frame.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                            Transform prev_par = frame.parent;

                            //int si_ind = frame.GetSiblingIndex();

                            frame.transform.parent = frm_par_dummy.transform;

                            frm_par_dummy.transform.localScale = Vector3.one * scale_fact;


                            Vector3 new_pos = frame.position - Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, frame.position);

                            new_pos = new_pos + orig_pos_up;// Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                            frame.transform.position = new_pos;// orig_pos;

                            frame.transform.parent = prev_par;

                            frame.SetAsFirstSibling();

                            //frame.SetSiblingIndex(sib_index);

                            DestroyImmediate(frm_par_dummy);
                        }
                    }
                    catch (Exception ex)
                    {

                        Debug.Log("frame :" + ex);
                    }

                }

                Accs_parented = new List<Transform>();

                //**!st loop for Accessories here we find the parent of accessories**************//

                //Here we make the L & U Accessories that are nearer to the field divider as their child
                if (field_div_local_z.Count > 0)
                    foreach (Transform field_div in field_div_local_z.Keys)
                    {

                        try
                        {



                            if (field_div != null)
                            {

                                GameObject f_div = field_div.gameObject;

                                Vector3 pos_Frm_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(f_div.transform.position);

                                Bounds f_bound;





                                if (f_div.transform.GetComponentInChildren<BoxCollider>() != null)
                                    f_bound = f_div.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                else
                                {
                                    f_div.AddComponent<BoxCollider>();
                                    f_bound = f_div.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                }

                                float frm_bound_max_size = Mathf.Max(f_bound.size.y, f_bound.size.z);
                                if (Accs_local_z.Count > 0)
                                    foreach (Transform accs in Accs_local_z.Keys)
                                    {
                                        //Accs_and_parent.Add(accs, accs.parent);

                                        //Characteristics chars_script = accs.GetComponent<Characteristics>();

                                        //chars_script.previous_parent_transform = accs.parent;

                                        Vector3 pos_Accs_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(accs.transform.position);
                                        if (accs != null)
                                        {
                                            //checking if distance difference is less than the bounds of divider
                                            if (Mathf.Abs(field_div_local_z[field_div] - Accs_local_z[accs]) <= 2.5f * frm_bound_max_size)
                                            {


                                                accs.transform.parent = f_div.transform;


                                                //Adding Accs to List  REMOVING KEY AFTER PARENTING 
                                                Accs_parented.Add(accs);


                                            }

                                        }
                                    }
                            }
                        }
                        catch (Exception ex)
                        {

                            print("field_div_local_z accs :" + ex);
                        }
                    }

                foreach (Transform accs_par in Accs_parented)
                {
                    if (Accs_local_z.ContainsKey(accs_par))
                    {
                        Accs_local_z.Remove(accs_par);
                    }
                }


                Accs_parented.Clear();

                foreach (Transform field_divider in field_div_local_z.Keys)
                {
                    try
                    {


                        GameObject div_par_dummy = new GameObject($"FLD_DIV_par_dummy_{field_divider.name}");

                        int sib_index = field_divider.GetSiblingIndex();

                        Vector3 orig_pos = field_divider.position;

                        //taking only up component ,of pergola_model_direction

                        Vector3 orig_pos_up = Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                        //placing empty GameObject at center
                        div_par_dummy.transform.position = field_divider.transform.TransformPoint(field_divider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                        Transform prev_par = field_divider.parent;

                        field_divider.transform.parent = div_par_dummy.transform;

                        div_par_dummy.transform.localScale = Vector3.one * scale_fact;


                        Vector3 new_pos = field_divider.position - Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, field_divider.position);

                        new_pos = new_pos + orig_pos_up;// Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                        field_divider.transform.position = new_pos;// orig_pos;

                        field_divider.transform.parent = prev_par;

                        field_divider.SetAsFirstSibling();

                        field_divider.SetSiblingIndex(sib_index);

                        DestroyImmediate(div_par_dummy);
                    }
                    catch (Exception ex)
                    {

                        print("field_divider :" + ex);
                    }

                }

                Accs_parented = new List<Transform>();
                //Here we make the L&U Accessories that are nearer to the field divider as their child
                if (frame_div_local_z.Count > 0)
                    foreach (Transform frame_div in frame_div_local_z.Keys)
                    {
                        try
                        {

                            if (frame_div != null)
                            {

                                GameObject frm_div = frame_div.gameObject;

                                Vector3 pos_Frm_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frm_div.transform.position);

                                Bounds f_bound;





                                if (frm_div.transform.GetComponentInChildren<BoxCollider>() != null)
                                    f_bound = frm_div.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                else
                                {
                                    frm_div.AddComponent<BoxCollider>();
                                    f_bound = frm_div.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                }

                                float frm_bound_max_size = Mathf.Max(f_bound.size.y, f_bound.size.z);
                                if (Accs_local_z.Count > 0)
                                    foreach (Transform accs in Accs_local_z.Keys)
                                    {
                                        //Accs_and_parent.Add(accs, accs.parent);

                                        //Characteristics chars_script = accs.GetComponent<Characteristics>();

                                        //chars_script.previous_parent_transform = accs.parent;

                                        Vector3 pos_Accs_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(accs.transform.position);
                                        if (accs != null)
                                        {
                                            //checking if distance difference is less than the bounds of divider
                                            if (Mathf.Abs(frame_div_local_z[frame_div] - Accs_local_z[accs]) <= 2.5f * frm_bound_max_size)
                                            {


                                                accs.transform.parent = frm_div.transform;


                                                //Adding Accs to List  REMOVING KEY AFTER PARENTING 
                                                Accs_parented.Add(accs);
                                            }

                                        }
                                    }
                            }
                        }
                        catch (Exception ex)
                        {

                            print("frame_div_local_z accs:" + ex);
                        }

                    }

                foreach (Transform accs_par in Accs_parented)
                {
                    if (Accs_local_z.ContainsKey(accs_par))
                    {
                        Accs_local_z.Remove(accs_par);
                    }
                }


                Accs_parented.Clear();

                foreach (Transform frame_divider in frame_div_local_z.Keys)
                {

                    try
                    {


                        GameObject Accs_par_dummy = new GameObject($"FRM_DIV_par_dummy_{frame_divider.name}");

                        int sib_index = frame_divider.GetSiblingIndex();

                        Vector3 orig_pos = frame_divider.position;

                        //taking only up component ,of pergola_model_direction

                        Vector3 orig_pos_up = Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                        //placing empty GameObject at center
                        Accs_par_dummy.transform.position = frame_divider.transform.TransformPoint(frame_divider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                        Transform prev_par = frame_divider.parent;

                        frame_divider.transform.parent = Accs_par_dummy.transform;

                        Accs_par_dummy.transform.localScale = Vector3.one * scale_fact;


                        Vector3 new_pos = frame_divider.position - Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, frame_divider.position);

                        new_pos = new_pos + orig_pos_up;// Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                        frame_divider.transform.position = new_pos;// orig_pos;

                        frame_divider.transform.parent = prev_par;

                        frame_divider.SetAsFirstSibling();

                        //frame_divider.SetSiblingIndex(sib_index);

                        DestroyImmediate(Accs_par_dummy);
                    }
                    catch (Exception ex)
                    {

                        print("frame_divider :" + ex);
                    }

                }

                try
                {

                    foreach (string Accs_name in DB_script.Accessories_name)
                    {
                        Transform Accs = GameObject.Find(Accs_name).transform;

                        Characteristics chars_script = Accs.GetComponent<Characteristics>();
                        if (chars_script != null)
                            if (chars_script.previous_parent_transform != null)
                            {
                                //Accs.transform.parent = chars_script.previous_parent_transform;

                                Accs.SetParent(chars_script.previous_parent_transform, true);

                                if (Accs.name.Contains("180005738") || Accs.name.Contains("180001258") || Accs.name.Contains("150000446") || Accs.name.Contains("ak - 109a") || Accs.name.Contains("L_Accessory_top"))
                                {
                                    Accs.gameObject.SetActive(false);
                                }
                            }

                    }
                }
                catch (Exception ex)
                {

                    print("Reparenting :" + ex);
                }
            }
        }
        Debug.Log("(1) Scaled Game Objects");
        }
        catch(Exception ex)
        {
            Debug.Log("Scaling function: "+ex);
        }
    }

    public void Scale__Frame_B_B(float scale_fact)
    {
        //**************here we Scale frame A , B ,fields, related accessories for model to apperar bigger in screen shot

        //TO DO
        //float scale_fact = 1;
        //scale_fact = scale_model_factor;
        if (GameObject.Find("Frames_Parent") != null)
            DB_script.Frames_Parent = GameObject.Find("Frames_Parent");

        if (GameObject.Find("Pergola_Model") != null)
            DB_script.Pergola_Model = GameObject.Find("Pergola_Model");

        if (GameObject.Find("Divider_Parent") != null)
            DB_script.Divider_Parent = GameObject.Find("Divider_Parent");


        if (GameObject.Find("FrameDividers_Parent") != null)
            DB_script.FrameDividers_Parent = GameObject.Find("FrameDividers_Parent");

        if (GameObject.Find("FieldDividers_Parent") != null)
            DB_script.FieldDividers_Parent = GameObject.Find("FieldDividers_Parent");

        if (GameObject.Find("Field_Parent") != null)
            DB_script.Field_Parent = GameObject.Find("Field_Parent");

        if (GameObject.Find("SupportBars_Parent") != null)
            DB_script.SupportBars_Parent = GameObject.Find("SupportBars_Parent");

        if (DB_script.FieldDividers_Parent.transform.GetChild(0).GetChild(0) != null && Math.Round(DB_script.FieldDividers_Parent.transform.GetChild(0).GetChild(0).transform.localScale.z) == 1)
        {
            GameObject Pergola_Model = GameObject.Find("Pergola_Model");


            Dictionary<Transform, float> frames_local_z = new Dictionary<Transform, float>();

            Dictionary<Transform, float> field_div_local_z = new Dictionary<Transform, float>();

            Dictionary<Transform, float> frame_div_local_z = new Dictionary<Transform, float>();

            Dictionary<Transform, float> Accs_local_z = new Dictionary<Transform, float>();



            foreach (Transform frame_subparent in DB_script.Frames_Parent.transform)
            {
                try
                {


                    if (frame_subparent.name == "Frame_Parent")
                        foreach (Transform frms in frame_subparent)
                        {
                            Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frms.transform.position);

                            //Bounds b_frm = frms.GetComponentInChildren<MeshFilter>().mesh.bounds;


                            //Vector3 global_center = frms.TransformPoint(b_frm.center);
                            //Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(global_center);

                            frames_local_z.Add(frms, pos_wrt_pergola_model.z);
                        }
                }
                catch (Exception ex)
                {

                    print("frame_subparent: " + ex);
                }
            }

            foreach (Transform field_div in DB_script.FieldDividers_Parent.transform)
            {
                if (field_div.GetChild(0) != null)
                {

                    Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(field_div.GetChild(0).transform.position);
                    //Bounds b_field_div = field_div.GetComponentInChildren<MeshFilter>().mesh.bounds;


                    //Vector3 global_center = field_div.TransformPoint(b_field_div.center);

                    //Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(global_center);

                    field_div_local_z.Add(field_div.GetChild(0), pos_wrt_pergola_model.z);
                }
            }

            foreach (Transform frame_div in DB_script.FrameDividers_Parent.transform)
            {
                if (frame_div.GetChild(0) != null)
                {
                    Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frame_div.GetChild(0).transform.position);


                    //Bounds b_field_div = frame_div.GetComponentInChildren<MeshFilter>().mesh.bounds;


                    //Vector3 global_center = frame_div.TransformPoint(b_field_div.center);

                    //Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(global_center);

                    frame_div_local_z.Add(frame_div.GetChild(0), pos_wrt_pergola_model.z);
                }
            }


            // Field_Parent / Field_Group_0001 / Accessories_1 / L_Accessory_bottomLeft_1
            foreach (string Accs_name in DB_script.Accessories_name)
            {
                try
                {


                    GameObject Accs = GameObject.Find(Accs_name);

                    Vector3 pos_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(Accs.transform.position);

                    Accs_local_z.Add(Accs.transform, pos_wrt_pergola_model.z);
                }
                catch (Exception ex)
                {

                    print("Accs_name :+" + ex);
                }
            }


            var sortedDict_z = from entry in frame_div_local_z orderby entry.Value ascending select entry;

            //field_Dividers_local_x.Clear();

            frame_div_local_z = new Dictionary<Transform, float>();
            frame_div_local_z = sortedDict_z.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//is sorted wrt to its local x/y/z axis value

            sortedDict_z = from entry in field_div_local_z orderby entry.Value ascending select entry;

            //field_Dividers_local_x.Clear();

            field_div_local_z = new Dictionary<Transform, float>();
            field_div_local_z = sortedDict_z.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//is sorted wrt to its local x/y/z axis value


            sortedDict_z = from entry in frame_div_local_z orderby entry.Value ascending select entry;

            //field_Dividers_local_x.Clear();

            frame_div_local_z = new Dictionary<Transform, float>();
            frame_div_local_z = sortedDict_z.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//is sorted wrt to its local x/y/z axis value

            sortedDict_z = from entry in Accs_local_z orderby entry.Value ascending select entry;

            //field_Dividers_local_x.Clear();

            Accs_local_z = new Dictionary<Transform, float>();
            Accs_local_z = sortedDict_z.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);//is sorted wrt to its local x/y/z axis value

            //Dictionary<Transform, Transform> Accs_and_parent = new Dictionary<Transform, Transform>();

            //used to store Accs_that are parented and to remove from dictionary
            List<Transform> Accs_parented = new List<Transform>();

            //Here we make the L&U Accessories that are nearer to the frames their child
            //if (frames_local_z.Count > 0)
            //    foreach (Transform frame in frames_local_z.Keys)
            //    {
            //        string frame1_name = "FrameB", frame2_name = "FrameD";

            //        //if (DB_script.L_type == true)
            //        //{
            //        //    frame1_name = "FrameE";

            //        //    frame2_name = "FrameC";
            //        //}

            //        if ((frame.name.Contains(frame1_name) || frame.name.Contains(frame2_name)))
            //        {
            //            try
            //            {


            //                if (frame != null)
            //                {

            //                    GameObject frm = frame.gameObject;

            //                    Vector3 pos_Frm_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frm.transform.position);

            //                    Bounds f_bound;





            //                    if (frm.transform.GetComponentInChildren<BoxCollider>() != null)
            //                        f_bound = frm.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
            //                    else
            //                    {
            //                        frm.AddComponent<BoxCollider>();
            //                        f_bound = frm.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

            //                    }

            //                    float frm_bound_max_size = Mathf.Max(f_bound.size.y, f_bound.size.z);
            //                    if (Accs_local_z.Count > 0)
            //                        foreach (Transform accs in Accs_local_z.Keys)
            //                        {
            //                            //Accs_and_parent.Add(accs, accs.parent);

            //                            Characteristics chars_script = accs.GetComponent<Characteristics>();

            //                            chars_script.previous_parent_transform = accs.parent;

            //                            Vector3 pos_Accs_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(accs.transform.position);
            //                            if (accs != null)
            //                            {
            //                                //checking if distance difference is less than the bounds of divider
            //                                if (Mathf.Abs(frames_local_z[frame] - Accs_local_z[accs]) <= 2 * frm_bound_max_size)
            //                                {


            //                                    accs.transform.parent = frm.transform;

            //                                    //Adding Accs to List  REMOVING KEY AFTER PARENTING 
            //                                    Accs_parented.Add(accs);
            //                                }

            //                            }
            //                        }
            //                }
            //            }
            //            catch (Exception ex)
            //            {

            //                Debug.Log("frames Accs:" + ex);
            //            }

            //        }
            //    }


            //foreach (Transform accs_par in Accs_parented)
            //{
            //    if (Accs_local_z.ContainsKey(accs_par))
            //    {
            //        Accs_local_z.Remove(accs_par);
            //    }
            //}


            //Accs_parented.Clear();


            foreach (Transform frame in frames_local_z.Keys)
            {
                try
                {

                    string frame1_name = "FrameB", frame2_name = "FrameD";

                    //if (DB_script.L_type == true)
                    //{
                    //    frame1_name = "FrameE";

                    //    frame2_name = "FrameC";
                    //}

                    if ((frame.name.Contains(frame1_name) || frame.name.Contains(frame2_name)))
                    {
                        GameObject frm_par_dummy = new GameObject($"frm_par_dummy_{frame.name}");

                        int sib_index = frame.GetSiblingIndex();

                        Vector3 orig_pos = frame.position;

                        //taking only up component ,of pergola_model_direction

                        Vector3 orig_pos_up = Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                        //placing empty GameObject at center
                        frm_par_dummy.transform.position = frame.transform.TransformPoint(frame.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                        Transform prev_par = frame.parent;

                        //int si_ind = frame.GetSiblingIndex();

                        frame.transform.parent = frm_par_dummy.transform;

                        frm_par_dummy.transform.localScale = Vector3.one * scale_fact;


                        Vector3 new_pos = frame.position - Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, frame.position);

                        new_pos = new_pos + orig_pos_up;// Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

                        frame.transform.position = new_pos;// orig_pos;

                        frame.transform.parent = prev_par;

                        frame.SetAsFirstSibling();

                        //frame.SetSiblingIndex(sib_index);

                        DestroyImmediate(frm_par_dummy);
                    }
                }
                catch (Exception ex)
                {

                    Debug.Log("frame :" + ex);
                }

            }

            //Accs_parented = new List<Transform>();

            ////**!st loop for Accessories here we find the parent of accessories**************//

            ////Here we make the L & U Accessories that are nearer to the field divider as their child
            //if (field_div_local_z.Count > 0)
            //    foreach (Transform field_div in field_div_local_z.Keys)
            //    {

            //        try
            //        {



            //            if (field_div != null)
            //            {

            //                GameObject f_div = field_div.gameObject;

            //                Vector3 pos_Frm_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(f_div.transform.position);

            //                Bounds f_bound;





            //                if (f_div.transform.GetComponentInChildren<BoxCollider>() != null)
            //                    f_bound = f_div.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
            //                else
            //                {
            //                    f_div.AddComponent<BoxCollider>();
            //                    f_bound = f_div.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

            //                }

            //                float frm_bound_max_size = Mathf.Max(f_bound.size.y, f_bound.size.z);
            //                if (Accs_local_z.Count > 0)
            //                    foreach (Transform accs in Accs_local_z.Keys)
            //                    {
            //                        //Accs_and_parent.Add(accs, accs.parent);

            //                        //Characteristics chars_script = accs.GetComponent<Characteristics>();

            //                        //chars_script.previous_parent_transform = accs.parent;

            //                        Vector3 pos_Accs_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(accs.transform.position);
            //                        if (accs != null)
            //                        {
            //                            //checking if distance difference is less than the bounds of divider
            //                            if (Mathf.Abs(field_div_local_z[field_div] - Accs_local_z[accs]) <= 2.5f * frm_bound_max_size)
            //                            {


            //                                accs.transform.parent = f_div.transform;


            //                                //Adding Accs to List  REMOVING KEY AFTER PARENTING 
            //                                Accs_parented.Add(accs);


            //                            }

            //                        }
            //                    }
            //            }
            //        }
            //        catch (Exception ex)
            //        {

            //            print("field_div_local_z accs :" + ex);
            //        }
            //    }

            //foreach (Transform accs_par in Accs_parented)
            //{
            //    if (Accs_local_z.ContainsKey(accs_par))
            //    {
            //        Accs_local_z.Remove(accs_par);
            //    }
            //}


            //Accs_parented.Clear();

            //foreach (Transform field_divider in field_div_local_z.Keys)
            //{
            //    try
            //    {


            //        GameObject div_par_dummy = new GameObject($"FLD_DIV_par_dummy_{field_divider.name}");

            //        int sib_index = field_divider.GetSiblingIndex();

            //        Vector3 orig_pos = field_divider.position;

            //        //taking only up component ,of pergola_model_direction

            //        Vector3 orig_pos_up = Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

            //        //placing empty GameObject at center
            //        div_par_dummy.transform.position = field_divider.transform.TransformPoint(field_divider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

            //        Transform prev_par = field_divider.parent;

            //        field_divider.transform.parent = div_par_dummy.transform;

            //        div_par_dummy.transform.localScale = Vector3.one * scale_fact;


            //        Vector3 new_pos = field_divider.position - Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, field_divider.position);

            //        new_pos = new_pos + orig_pos_up;// Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

            //        field_divider.transform.position = new_pos;// orig_pos;

            //        field_divider.transform.parent = prev_par;

            //        field_divider.SetAsFirstSibling();

            //        field_divider.SetSiblingIndex(sib_index);

            //        DestroyImmediate(div_par_dummy);
            //    }
            //    catch (Exception ex)
            //    {

            //        print("field_divider :" + ex);
            //    }

            //}

            //Accs_parented = new List<Transform>();
            ////Here we make the L&U Accessories that are nearer to the field divider as their child
            //if (frame_div_local_z.Count > 0)
            //    foreach (Transform frame_div in frame_div_local_z.Keys)
            //    {
            //        try
            //        {

            //            if (frame_div != null)
            //            {

            //                GameObject frm_div = frame_div.gameObject;

            //                Vector3 pos_Frm_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(frm_div.transform.position);

            //                Bounds f_bound;





            //                if (frm_div.transform.GetComponentInChildren<BoxCollider>() != null)
            //                    f_bound = frm_div.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;
            //                else
            //                {
            //                    frm_div.AddComponent<BoxCollider>();
            //                    f_bound = frm_div.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

            //                }

            //                float frm_bound_max_size = Mathf.Max(f_bound.size.y, f_bound.size.z);
            //                if (Accs_local_z.Count > 0)
            //                    foreach (Transform accs in Accs_local_z.Keys)
            //                    {
            //                        //Accs_and_parent.Add(accs, accs.parent);

            //                        //Characteristics chars_script = accs.GetComponent<Characteristics>();

            //                        //chars_script.previous_parent_transform = accs.parent;

            //                        Vector3 pos_Accs_wrt_pergola_model = Pergola_Model.transform.InverseTransformPoint(accs.transform.position);
            //                        if (accs != null)
            //                        {
            //                            //checking if distance difference is less than the bounds of divider
            //                            if (Mathf.Abs(frame_div_local_z[frame_div] - Accs_local_z[accs]) <= 2.5f * frm_bound_max_size)
            //                            {


            //                                accs.transform.parent = frm_div.transform;


            //                                //Adding Accs to List  REMOVING KEY AFTER PARENTING 
            //                                Accs_parented.Add(accs);
            //                            }

            //                        }
            //                    }
            //            }
            //        }
            //        catch (Exception ex)
            //        {

            //            print("frame_div_local_z accs:" + ex);
            //        }

            //    }

            //foreach (Transform accs_par in Accs_parented)
            //{
            //    if (Accs_local_z.ContainsKey(accs_par))
            //    {
            //        Accs_local_z.Remove(accs_par);
            //    }
            //}


            //Accs_parented.Clear();

            //foreach (Transform frame_divider in frame_div_local_z.Keys)
            //{

            //    try
            //    {


            //        GameObject Accs_par_dummy = new GameObject($"FRM_DIV_par_dummy_{frame_divider.name}");

            //        int sib_index = frame_divider.GetSiblingIndex();

            //        Vector3 orig_pos = frame_divider.position;

            //        //taking only up component ,of pergola_model_direction

            //        Vector3 orig_pos_up = Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

            //        //placing empty GameObject at center
            //        Accs_par_dummy.transform.position = frame_divider.transform.TransformPoint(frame_divider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

            //        Transform prev_par = frame_divider.parent;

            //        frame_divider.transform.parent = Accs_par_dummy.transform;

            //        Accs_par_dummy.transform.localScale = Vector3.one * scale_fact;


            //        Vector3 new_pos = frame_divider.position - Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, frame_divider.position);

            //        new_pos = new_pos + orig_pos_up;// Pergola_Model.transform.up * Vector3.Dot(Pergola_Model.transform.up, orig_pos);

            //        frame_divider.transform.position = new_pos;// orig_pos;

            //        frame_divider.transform.parent = prev_par;

            //        frame_divider.SetAsFirstSibling();

            //        //frame_divider.SetSiblingIndex(sib_index);

            //        DestroyImmediate(Accs_par_dummy);
            //    }
            //    catch (Exception ex)
            //    {

            //        print("frame_divider :" + ex);
            //    }

            //}

            try
            {

                foreach (string Accs_name in DB_script.Accessories_name)
                {
                    Transform Accs = GameObject.Find(Accs_name).transform;

                    Characteristics chars_script = Accs.GetComponent<Characteristics>();
                    if (chars_script != null)
                        if (chars_script.previous_parent_transform != null)
                        {
                            //Accs.transform.parent = chars_script.previous_parent_transform;

                            Accs.SetParent(chars_script.previous_parent_transform, true);

                            if (Accs.name.Contains("180005738") || Accs.name.Contains("180001258") || Accs.name.Contains("150000446") || Accs.name.Contains("ak - 109a") || Accs.name.Contains("L_Accessory_top"))
                            {
                                Accs.gameObject.SetActive(false);
                            }
                        }

                }
            }
            catch (Exception ex)
            {

                print("Reparenting :" + ex);
            }
        }

        Debug.Log("(1) Scaled Game Objects");
    }
    public class arrow_parm
    {
        public Vector3 position_of_arrow = Vector3.zero;

        //to show in which direction the arrow should be scaled
        public Vector3 direction_of_arrow = Vector3.zero;

        //to indicate the length of the arrow
        public float length_of_arrow = 0;

        public string txt = null;

        public string arrow_name = null;

        // to translate in direction of pergola model(Main/root Parent)
        public float pergola_right = 0, pergola_up = 0, pergola_fwd = 0;

        public GameObject arrow_prefab = null;

        public float txt_right = 0, txt_up = 0, txt_fwd = 0;


        //Dotted lines params

        /// <summary>
        /// </summary>
        /// 

        /// line direction and Dot facing direction
        public Vector3 Dotted_line_Direction, Dotted_facing_direction;

        public float dotted_line_offset;

        public string orientation_dotted_lines;

        //public float font_size = 0;
    }

   static List<GameObject> all_children = new List<GameObject>();

    public static List<GameObject> get_All_children(Transform go, bool create_new_list = true)
    {
        if (all_children == null || create_new_list == true)
        {
            all_children = new List<GameObject>();
        }
        for (int j = 0; j < go.childCount; j++)
        {
            if (!all_children.Contains(go.GetChild(j).gameObject))
                all_children.Add(go.GetChild(j).gameObject);

            get_All_children(go.GetChild(j), false);
        }

        return all_children;
    }

    public static List<GameObject> get_All_children_of_Go(Transform go)
    {
        //A new List is created to hold all the Gameobjects (children at different hierarchy level , under the gameobject (go))
        List<GameObject> al_go_ch = new List<GameObject>();

        for (int j = 0; j < go.childCount; j++)
        {
            //here we check if the List already has that game object , if not we add that to a List
            if (!al_go_ch.Contains(go.GetChild(j).gameObject))
                al_go_ch.Add(go.GetChild(j).gameObject);

            //al_go_ch=al_go_ch.Concat(get_All_children_new(go.GetChild(j))).ToList();
            //here , we iteratively call function "get_All_children_of_Go(go.GetChild(j))" to get all the children inside "go.GetChild(j)"

            //the returned list is , Concatenated with "al_go_ch" list
            al_go_ch.AddRange(get_All_children_of_Go(go.GetChild(j)));
        }
        //Finally the List "al_go_ch" that contains all children is returned
        return al_go_ch;
    }
}
