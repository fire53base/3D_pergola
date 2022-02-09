using RESTfulHTTPServer.src.invoker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Arrows_Measure_louvers : MonoBehaviour
{

    public static GameObject Arrow_Parent, Arrow_prefab;

    public static SortedDictionary<string, string> field_arrow_names_dist;

    public enum Views { _TOP, _FRONT, _FIELD, _SIDE_FIELD }
    DB_script db_script;

    public static GameObject sphere_;

    Dotted_line_custom.Dotted_line_custom dotted_Line_Custom_script;

    public enum Parents_name { SupportBars_Parent, Frames_Parent, Field_Parent, Divider_Parent, Arrow_Parent, FieldDividers_Parent, FrameDividers_Parent }

    public static GameObject Arrow_Side_Destroy_field_width;

    static string dummy_text = "%*%#^%&";

    public static float scale_model_factor = 1;

    enum dotted_lines_dir {horizontal,vertical }
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
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
        
            arrows_Louvers(Views._FRONT.ToString());
        
    }
    public async void arrows_Louvers(string view = "_FRONT")
    {
        try
        {

            if (GameObject.Find("HorizontalBar_Parent") != null)
            {
                DB_Script_louvers.HorizontalBar_Parent = GameObject.Find("HorizontalBar_Parent");
            }


            if (GameObject.Find("VerticalBar_Parent") != null)
            {
                DB_Script_louvers.VerticalBar_Parent = GameObject.Find("VerticalBar_Parent");
            }


            if (GameObject.Find("Louver_Model") != null)
            {
                DB_Script_louvers.Louver_Model = GameObject.Find("Louver_Model");
            }
            if (GameObject.Find("Louver_Model") != null)
            {
                GameObject Louver_Model = GameObject.Find("Louver_Model");


                if (Arrow_Parent != null)
                {
                    foreach (Transform Arrow in Arrow_Parent.transform)
                    {
                        DestroyImmediate(Arrow.gameObject);
                    }
                }


                GameObject vertical0 = GameObject.Find("Louver_Model/VerticalBar_Parent/vertical_0_0");

                Bounds bound_v0 = vertical0.transform.GetChild(0).GetComponentInChildren<MeshFilter>().mesh.bounds;

                rotateonmouse rotateonmouse_script = Louver_Model.GetComponent<rotateonmouse>();


                if (view == Views._FRONT.ToString())
                {
                   

                    arrow_parm arrow_Building_height = new arrow_parm()
                    {
                        position_of_arrow = vertical0.transform.TransformPoint(bound_v0.center),
                        direction_of_arrow = (Louver_Model.transform.up),
                        length_of_arrow = DB_Script_louvers._building_height,
                        arrow_name = "Arrow_Building_height_" + vertical0.name,



                        //pergola_fwd = -fwd_offset,// pergola_forward_offset,


                        pergola_right = -(Mathf.Abs(Vector3.Dot(vertical0.transform.InverseTransformDirection(Louver_Model.transform.right), bound_v0.size)) + 300),//Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Louver_Model.transform.right), bound_c.size)) + 300,

                        pergola_up = 0,// Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Louver_Model.transform.up), bound_c.size))+300

                        dotted_line_offset = -(Mathf.Abs(Vector3.Dot(vertical0.transform.InverseTransformDirection(Louver_Model.transform.right), bound_v0.size)) + 300),

                        Dotted_line_Direction = Louver_Model.transform.right,



                    };

                    arrows_function(arrow_Building_height);

                    arrow_parm arrow_Building_width = new arrow_parm()
                    {
                        position_of_arrow = vertical0.transform.TransformPoint(bound_v0.center),
                        direction_of_arrow = (Louver_Model.transform.right),
                        length_of_arrow = DB_Script_louvers._building_width,
                        arrow_name = "Arrow_Building_building_width_" + vertical0.name,



                        //pergola_fwd = -fwd_offset,// pergola_forward_offset,


                        pergola_right = -(Mathf.Abs(Vector3.Dot(vertical0.transform.InverseTransformDirection(Louver_Model.transform.right), bound_v0.size))) / 2,// + 300),//Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Louver_Model.transform.right), bound_c.size)) + 300,

                        pergola_up = 300 + DB_Script_louvers._building_height,

                        dotted_line_offset = (Mathf.Abs(Vector3.Dot(vertical0.transform.InverseTransformDirection(Louver_Model.transform.right), bound_v0.size)) + 300),

                        Dotted_line_Direction = Louver_Model.transform.up,

                        orientation_dotted_lines = dotted_lines_dir.horizontal.ToString(),



                    };

                    arrows_function(arrow_Building_width);

                    if (rotateonmouse_script != null)
                    {
                        rotateonmouse_script.rotate_FRONT_Louvers();
                    }

                    if (Arrow_Parent != null)
                        foreach (Transform Arrow in Arrow_Parent.transform)
                        {
                            if (Arrow.Find("Tail").GetChild(0) != null)
                                foreach (Transform dots in Arrow.Find("Tail").GetChild(0))
                                {
                                    dots.forward = Louver_Model.transform.forward;
                                }

                            if (Arrow.Find("Head").GetChild(0) != null)
                                foreach (Transform dots in Arrow.Find("Head").GetChild(0))
                                {
                                    dots.forward = Louver_Model.transform.forward;

                                }

                            if (Arrow.Find("Text (TMP)") != null)
                            {

                                Transform text = Arrow.Find("Text (TMP)");


                                if (Arrow.name.Contains("Arrow_Building_height_vertical"))
                                {
                                    text.GetComponent<RectTransform>().localRotation = Quaternion.Euler(90, -90, 0);
                                }
                                else Arrow.Find("Text (TMP)").forward = Louver_Model.transform.forward;


                            }
                        }
                }
                else if (view == Views._TOP.ToString())
                {
                    await Task.Delay(300);

                    if (Arrow_Parent != null)
                    {
                        foreach (Transform Arrow in Arrow_Parent.transform)
                        {
                            DestroyImmediate(Arrow.gameObject);
                        }
                    }
                    foreach (Transform field_div in DB_Script_louvers.VerticalBar_Parent.transform)
                    {
                        GameObject Arrow_Green = null;

                        GameObject field_R_frameDivider = field_div.GetChild(0).gameObject;

                        Bounds divider_field_bound = field_R_frameDivider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                        float max_lenth_of_field_R_frameDivider = Mathf.Abs(Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z));

                        //max_lenth_of_field_R_frameDivider += 400;//to bring the arrows forward extra offset of 400

                        float min_distance_2 = 0; //Mathf.Max(field_R_frameDivider.transform.localScale.x, field_R_frameDivider.transform.localScale.y, field_R_frameDivider.transform.localScale.z);

                        Vector3 arrow_direction_wrt_frameParent_dir = Louver_Model.transform.right;

                        Vector3 arrow_for_end_field_divider_diretion = -arrow_direction_wrt_frameParent_dir;
                        float center2edge_dist = Mathf.Abs(Vector3.Dot(field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized), divider_field_bound.size)) / 2;
                        Vector3 arrow_pos = field_R_frameDivider.transform.TransformPoint(divider_field_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);


                        float frame_depth = Mathf.Abs(Vector3.Dot(field_div.transform.InverseTransformDirection(Louver_Model.transform.forward), divider_field_bound.size));

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

                                pergola_fwd = frame_depth,
                                //pergola_right = hit1.distance/2//max_lenth_of_field_R_frameDivider * 1 / 4

                                Dotted_line_Direction = Louver_Model.transform.forward,

                                dotted_line_offset = frame_depth,

                                orientation_dotted_lines = dotted_lines_dir.horizontal.ToString(),
                            };

                            //Updating the list to access while taking front screen shot as distance is varied
                            //if (!field_arrow_names_dist.ContainsKey(ar_field_div.arrow_name))
                            //{
                            //    field_arrow_names_dist.Add(ar_field_div.arrow_name, ((float)Math.Floor(hit1.distance)).ToString());
                            //}


                            arrows_function(ar_field_div);

                        }

                        //To get the last arrow of field divider we raycst in opposite direction

                        //if (field_div.GetSiblingIndex() == Field_divider_Parent.transform.childCount - 1)
                        //{
                        //    //assigning opposite direction to arrow

                        //    arrow_direction_wrt_frameParent_dir = Louver_Model.transform.forward;


                        //    arrow_pos = field_R_frameDivider.transform.TransformPoint(divider_field_bound.center + field_R_frameDivider.transform.InverseTransformDirection(arrow_direction_wrt_frameParent_dir.normalized) * center2edge_dist);

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

                    if (Arrow_Parent != null)
                        foreach (Transform Arrow in Arrow_Parent.transform)
                        {
                            //if (Arrow.Find("Tail").GetChild(0) != null)
                            //    foreach (Transform dots in Arrow.Find("Tail").GetChild(0))
                            //    {
                            //        dots.forward = Louver_Model.transform.forward;
                            //    }

                            //if (Arrow.Find("Head").GetChild(0) != null)
                            //    foreach (Transform dots in Arrow.Find("Head").GetChild(0))
                            //    {
                            //        dots.forward = Louver_Model.transform.forward;

                            //    }

                            if (Arrow.Find("Text (TMP)") != null)
                            {

                                Transform text = Arrow.Find("Text (TMP)");


                                //if (Arrow.name.Contains("Arrow_Building_height_vertical"))
                                //{
                                text.GetComponent<RectTransform>().localRotation = Quaternion.Euler(90, -90, 0);
                                //}
                                //else
                                //Arrow.Find("Text (TMP)").forward = Camera.main.transform.forward;


                            }
                        }

                    if (rotateonmouse_script != null)
                    {
                        rotateonmouse_script.rotate_TOP_Louvers();
                    }
                }

            }
        
        }
        catch (Exception ex_arr)
        {

            Debug.Log("arrows louvers: "+ex_arr);
        }
        finally
    {
    Screen_Shot sc_sh = Camera.main.GetComponent<Screen_Shot>();
    StartCoroutine(sc_sh.Texture_Render(view, false));
    }
    }


 
    public GameObject arrows_function(arrow_parm ar = null)
    {
        //This function takes object of parameters of the arrow and Generate an arow

        //if (GameObject.Find("Frames_Parent") != null)
        //    DB_script.Frames_Parent = GameObject.Find("Frames_Parent");

        //if (GameObject.Find("Louver_Model") != null)
        //    DB_Script_louvers.Louver_Model = GameObject.Find("Louver_Model");

        //if (GameObject.Find("Divider_Parent") != null)
        //    DB_script.Divider_Parent = GameObject.Find("Divider_Parent");


        //if (GameObject.Find("FrameDividers_Parent") != null)
        //    DB_script.FrameDividers_Parent = GameObject.Find("FrameDividers_Parent");

        //if (GameObject.Find("FieldDividers_Parent") != null)
        //    DB_script.FieldDividers_Parent = GameObject.Find("FieldDividers_Parent");

        //if (GameObject.Find("Field_Parent") != null)
        //    DB_script.Field_Parent = GameObject.Find("Field_Parent");

        //if (GameObject.Find("SupportBars_Parent") != null)
        //    DB_script.SupportBars_Parent = GameObject.Find("SupportBars_Parent");

        GameObject Louver_Model = GameObject.Find("Louver_Model");

        float min_distance_LEFT = ar.length_of_arrow;// frame_C.transform.localScale.y;
        GameObject Arrow = null;
        if (min_distance_LEFT >= 0)
        {
            Arrow = null;

            if (Arrow_Parent == null)
                Arrow_Parent = new GameObject("Arrow_Parent");
            else
                Arrow_Parent = GameObject.Find("Arrow_Parent");


            Arrow_Parent.transform.parent = Louver_Model.transform;


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
                Arrow.transform.forward = ar.direction_of_arrow;// (Louver_Model.transform.forward); //look at left vertical bar

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
                    Arrow.transform.Translate(Louver_Model.transform.right * ar.pergola_right, Space.World);
                }

                if (ar.pergola_up != 0)
                {
                    Arrow.transform.Translate(Louver_Model.transform.up * ar.pergola_up, Space.World);
                }

                if (ar.pergola_fwd != 0)
                {
                    Arrow.transform.Translate(Louver_Model.transform.forward * ar.pergola_fwd, Space.World);
                }


                if (ar.txt_right != 0)
                {
                    textMeshPro.gameObject.transform.Translate(Louver_Model.transform.right * ar.txt_right, Space.World);
                }

                if (ar.txt_up != 0)
                {
                    textMeshPro.gameObject.transform.Translate(Louver_Model.transform.up * ar.txt_up, Space.World);
                }


                if (ar.txt_fwd != 0)
                {
                    textMeshPro.gameObject.transform.Translate(Louver_Model.transform.forward * ar.txt_fwd, Space.World);
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

                Vector3 Dotted_line_fwd_visibility_dir = Louver_Model.transform.up;

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

    public void Scale_fields_Accs_Frame(float scale_fact)
    {
        //**************here we Scale frame A , B ,fields, related accessories for model to apperar bigger in screen shot

        //TO DO
        //float scale_fact = 1;
        //scale_fact = scale_model_factor;
        if (GameObject.Find("Frames_Parent") != null)
            DB_script.Frames_Parent = GameObject.Find("Frames_Parent");

        if (GameObject.Find("Louver_Model") != null)
            DB_Script_louvers.Louver_Model = GameObject.Find("Louver_Model");

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
            GameObject Louver_Model = GameObject.Find("Louver_Model");


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
                            Vector3 pos_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(frms.transform.position);

                            //Bounds b_frm = frms.GetComponentInChildren<MeshFilter>().mesh.bounds;


                            //Vector3 global_center = frms.TransformPoint(b_frm.center);
                            //Vector3 pos_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(global_center);

                            frames_local_z.Add(frms, pos_wrt_Louver_Model.z);
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

                    Vector3 pos_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(field_div.GetChild(0).transform.position);
                    //Bounds b_field_div = field_div.GetComponentInChildren<MeshFilter>().mesh.bounds;


                    //Vector3 global_center = field_div.TransformPoint(b_field_div.center);

                    //Vector3 pos_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(global_center);

                    field_div_local_z.Add(field_div.GetChild(0), pos_wrt_Louver_Model.z);
                }
            }

            foreach (Transform frame_div in DB_script.FrameDividers_Parent.transform)
            {
                if (frame_div.GetChild(0) != null)
                {
                    Vector3 pos_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(frame_div.GetChild(0).transform.position);


                    //Bounds b_field_div = frame_div.GetComponentInChildren<MeshFilter>().mesh.bounds;


                    //Vector3 global_center = frame_div.TransformPoint(b_field_div.center);

                    //Vector3 pos_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(global_center);

                    frame_div_local_z.Add(frame_div.GetChild(0), pos_wrt_Louver_Model.z);
                }
            }


            // Field_Parent / Field_Group_1 / Accessories_1 / L_Accessory_bottomLeft_1
            foreach (string Accs_name in DB_script.Accessories_name)
            {
                try
                {


                    GameObject Accs = GameObject.Find(Accs_name);

                    Vector3 pos_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(Accs.transform.position);

                    Accs_local_z.Add(Accs.transform, pos_wrt_Louver_Model.z);
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

                                Vector3 pos_Frm_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(frm.transform.position);

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

                                        Vector3 pos_Accs_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(accs.transform.position);
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

                        //taking only up component ,of Louver_Model_direction

                        Vector3 orig_pos_up = Louver_Model.transform.up * Vector3.Dot(Louver_Model.transform.up, orig_pos);

                        //placing empty GameObject at center
                        frm_par_dummy.transform.position = frame.transform.TransformPoint(frame.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                        Transform prev_par = frame.parent;

                        //int si_ind = frame.GetSiblingIndex();

                        frame.transform.parent = frm_par_dummy.transform;

                        frm_par_dummy.transform.localScale = Vector3.one * scale_fact;


                        Vector3 new_pos = frame.position - Louver_Model.transform.up * Vector3.Dot(Louver_Model.transform.up, frame.position);

                        new_pos = new_pos + orig_pos_up;// Louver_Model.transform.up * Vector3.Dot(Louver_Model.transform.up, orig_pos);

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

                            Vector3 pos_Frm_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(f_div.transform.position);

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

                                    Vector3 pos_Accs_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(accs.transform.position);
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

                    //taking only up component ,of Louver_Model_direction

                    Vector3 orig_pos_up = Louver_Model.transform.up * Vector3.Dot(Louver_Model.transform.up, orig_pos);

                    //placing empty GameObject at center
                    div_par_dummy.transform.position = field_divider.transform.TransformPoint(field_divider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                    Transform prev_par = field_divider.parent;

                    field_divider.transform.parent = div_par_dummy.transform;

                    div_par_dummy.transform.localScale = Vector3.one * scale_fact;


                    Vector3 new_pos = field_divider.position - Louver_Model.transform.up * Vector3.Dot(Louver_Model.transform.up, field_divider.position);

                    new_pos = new_pos + orig_pos_up;// Louver_Model.transform.up * Vector3.Dot(Louver_Model.transform.up, orig_pos);

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

                            Vector3 pos_Frm_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(frm_div.transform.position);

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

                                    Vector3 pos_Accs_wrt_Louver_Model = Louver_Model.transform.InverseTransformPoint(accs.transform.position);
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

                    //taking only up component ,of Louver_Model_direction

                    Vector3 orig_pos_up = Louver_Model.transform.up * Vector3.Dot(Louver_Model.transform.up, orig_pos);

                    //placing empty GameObject at center
                    Accs_par_dummy.transform.position = frame_divider.transform.TransformPoint(frame_divider.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);

                    Transform prev_par = frame_divider.parent;

                    frame_divider.transform.parent = Accs_par_dummy.transform;

                    Accs_par_dummy.transform.localScale = Vector3.one * scale_fact;


                    Vector3 new_pos = frame_divider.position - Louver_Model.transform.up * Vector3.Dot(Louver_Model.transform.up, frame_divider.position);

                    new_pos = new_pos + orig_pos_up;// Louver_Model.transform.up * Vector3.Dot(Louver_Model.transform.up, orig_pos);

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

        Debug.Log("(1) Scaled Game Objects");
    }

    public void Scale_bars_louvers(float scale_fact)
    {
        if (GameObject.Find("HorizontalBar_Parent") != null)
        {
            DB_Script_louvers.HorizontalBar_Parent = GameObject.Find("HorizontalBar_Parent");
        }


        if (GameObject.Find("VerticalBar_Parent") != null)
        {
            DB_Script_louvers.VerticalBar_Parent = GameObject.Find("VerticalBar_Parent");
        }


        if (GameObject.Find("Louver_Model") != null)
        {
            DB_Script_louvers.Louver_Model = GameObject.Find("Louver_Model");
        }


        if(DB_Script_louvers.VerticalBar_Parent.transform.childCount>0&& DB_Script_louvers.VerticalBar_Parent.transform.GetChild(0).GetChild(0).localScale.x==1)
        {
            Dictionary<Transform, float> verticals_local_x = new Dictionary<Transform, float>();



        }
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
