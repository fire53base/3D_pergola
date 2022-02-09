using RESTfulHTTPServer.src.invoker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Model_Generation : MonoBehaviour
{
    public static GameObject VerticalBar_Parent, HorizontalBar_Parent, Grand_Parent, Accessory_Parent, Bar_Prefab;

    enum part_type_enum { horizontal, vertical, accessory, none };

   

    public static LayerMask end_V_Bar;

    void Start()
    {
        if (GameObject.Find("Parent") != null)
            VerticalBar_Parent = GameObject.Find("Parent");

        end_V_Bar = LayerMask.NameToLayer("end_V_Bar");
    }
    GameObject horizontal_bar;
    public async Task<int> generate_model(DataSet dsMeasurements, DataSet ds_project_elements)
    {
        Dictionary<GameObject, int> verticals_order = new Dictionary<GameObject, int>();

        #region Creation and Destruction of Game objects

        if (GameObject.Find("HorizontalBar_Parent") != null)
        {
            DestroyImmediate(GameObject.Find("HorizontalBar_Parent"));
        }


        if (GameObject.Find("VerticalBar_Parent") != null)
        {
            DestroyImmediate(GameObject.Find("VerticalBar_Parent"));
        }

        if (GameObject.Find("Accessory_Parent") != null)
        {
            DestroyImmediate(GameObject.Find("Accessory_Parent"));
        }

        if (GameObject.Find("Grand_Parent") != null)
        {
            DestroyImmediate(GameObject.Find("Grand_Parent"));
        }

        #region Creation of Empty Parents

        Grand_Parent = new GameObject("Grand_Parent");



        HorizontalBar_Parent = new GameObject("HorizontalBar_Parent");

        VerticalBar_Parent = new GameObject("VerticalBar_Parent");


        #endregion

        #endregion


        //**************Project Elements Params*********************//
        string horizontal_type = "", mid_verticals = "";
        horizontal_type = ds_project_elements.Tables[0].Rows[0]["horizontal_type"].ToString();

        mid_verticals = ds_project_elements.Tables[0].Rows[0]["mid_verticals"].ToString();

        String vertical_part_name = "ak - 288";

        //here string array of space between Vertical is taken
        string[] side_A_vpole_distances_Array = ds_project_elements.Tables[0].Rows[0]["side_A_vpole_distances"].ToString().Split(',');

        //space between horizontals
        float space_between_horizontal = float.Parse(ds_project_elements.Tables[0].Rows[0]["space_between_horizontal"].ToString());


        vertical_part_name = mid_verticals;
        Bar_Prefab = (GameObject)Resources.Load($"prefabs/{vertical_part_name}", typeof(GameObject));

        // if the part doesnt extist load "ak - 109a"
        if (Bar_Prefab == null)
        {
            vertical_part_name = "ak - 109a";


            Bar_Prefab = (GameObject)Resources.Load($"prefabs/{vertical_part_name}", typeof(GameObject));
        }
        float scale = 1000;
        float X_distance = 0;

        float building_width = 0, building_height = 0;

        //***********************Measurements Params***********************************//

        building_width = float.Parse(dsMeasurements.Tables[0].Rows[0]["building_width"].ToString());

        building_height = float.Parse(dsMeasurements.Tables[0].Rows[0]["building_height"].ToString());

        //static elements are assigned here
        Characteristics.project_unique_id = (dsMeasurements.Tables[0].Rows[0]["proj_id"].ToString());

        Characteristics.element_unique_id = (dsMeasurements.Tables[0].Rows[0]["element_id"].ToString());

        Characteristics.building_unique_id = (dsMeasurements.Tables[0].Rows[0]["building_id"].ToString());

        Characteristics.section_name = "S1";
        //building_height = space*n+barwidth*(n+1)

        //no of spaces
        //float n = Mathf.Floor( (building_height - h_bar_width) / (space_between_horizontal + h_bar_width));

        scale = building_height;

        int no_of_v_bars = (side_A_vpole_distances_Array.Length + 1);






        for (int i = 0; i < no_of_v_bars; i++)
        {
            #region Instantiation and positioning
            GameObject vertical_bar = Instantiate(Bar_Prefab, VerticalBar_Parent.transform);
            vertical_bar.transform.localPosition = new Vector3(X_distance, 0, 0);
            vertical_bar.transform.localScale = new Vector3(1, scale, 1);
            vertical_bar.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
            vertical_bar.name = vertical_part_name + "_Side_A_" + i;


            #endregion

            #region chracteristics
            //ataching Characteristics script
            Characteristics characteristics_script = vertical_bar.AddComponent<Characteristics>();

            Bounds vertical_bar_bounds = vertical_bar.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

            float vertical_bar_width_offset = vertical_bar_bounds.size.x;
            //Assigning Frame Prefab :eg:ak - 288
            characteristics_script.part_name_id = vertical_part_name;//Bar_Prefab.name;

            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
            verticals_order.Add(vertical_bar, i);

            characteristics_script.part_type = part_type_enum.vertical.ToString();
            #endregion
            // for first and last bars we add layer mask
            if (i == 0 || i == (no_of_v_bars - 1))
            {
                //add layer to V-bar

                vertical_bar.layer = end_V_Bar;
                if (vertical_bar.transform.GetChild(0) != null)
                {
                    vertical_bar.transform.GetChild(0).gameObject.layer = end_V_Bar;
                }
            }
            //as spaces will be 1 less than the vertical bars
            if (i < no_of_v_bars - 1)
            {//X_distance += building_width;
             //as we are moving from origin moving distance of each by  bar width
                X_distance += float.Parse(side_A_vpole_distances_Array[i]);
                X_distance += vertical_bar_width_offset;
            }
        }

        Vector3 horizontal_bar_scaling_direction = Vector3.right;

        float forward_offset_hbar = 0;

        //taking direction first and last vertical bar
        if (verticals_order.Keys.First() != null && verticals_order.Keys.Last() != null)
        {

            //this part is to find the direction of scaling
            GameObject first_V_bar = verticals_order.Keys.First();

            GameObject last_V_bar = verticals_order.Keys.Last();

            Bounds first_V_bar_bound = first_V_bar.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

            Bounds last_V_bar_bound = last_V_bar.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

            /***************Forwar movement offset**************/

            //forward_offset_hbar = first_V_bar_bound.size.z;
            //we take direction of scaling by 
            horizontal_bar_scaling_direction = first_V_bar.transform.TransformPoint(first_V_bar_bound.center) - last_V_bar.transform.TransformPoint(last_V_bar_bound.center);


        }





        scale = 1840;
        scale = building_width;
        string horizontal_part_name = "ak - 18";
        horizontal_part_name = horizontal_type;
        Bar_Prefab = (GameObject)Resources.Load($"prefabs/{horizontal_part_name}", typeof(GameObject));

        float h_bar_height = 0;
        if (Bar_Prefab == null)
        {
            horizontal_part_name = "ak - 18";

            Bar_Prefab = (GameObject)Resources.Load($"prefabs/{horizontal_part_name}", typeof(GameObject));
        }


        if (Bar_Prefab != null)
        {
            //part_name = "ak - 18";

            //Frame_Prefab =Instantiate(  (GameObject)Resources.Load($"prefabs/{part_name}", typeof(GameObject)), GameObject.Find("HorizontalBar_Parent").transform);

            //Here we instatiate Dummy horizontal bar and find the offset nd destroy it 
            GameObject Dummy_h_bar = Instantiate(Bar_Prefab, HorizontalBar_Parent.transform);

            Bounds h_bar_bound = Calculate_b(Dummy_h_bar.transform);


            forward_offset_hbar = h_bar_bound.size.z;


            Dummy_h_bar.transform.localScale = new Vector3(scale, 1, 1);


            h_bar_height = Dummy_h_bar.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.size.y;
            if (Dummy_h_bar != null)
            {
                DestroyImmediate(Dummy_h_bar);
            }
        }




        //***********Here we calculate number of Bars for given bar Spacing,Building height and width of the horizontal- bar

        float no_of_H_bars = Mathf.Floor(((building_height + space_between_horizontal) / (h_bar_height + space_between_horizontal)));
        int IntNumOf_H_bar = Convert.ToInt32(no_of_H_bars);


        //HORIZONTALS ALWAYS MIDDLE ALIGNED.

        //The difference of actual building height and the building heigth till the h_brs are placed --gives "h_Bar_offset"

        float h_Bar_offset = building_height - ((IntNumOf_H_bar * h_bar_height) + (space_between_horizontal * (IntNumOf_H_bar - 1)));//top

        string alignment = ds_project_elements.Tables[0].Rows[0]["horizontals_align"].ToString();// "bottom";//middle,top
        float space_h_bar = space_between_horizontal;// building_height / 2;+h_bar_width


        bool clips_in_model = false;



        float clip_spare_part_height = 0; // the height which goes inside another clip.
        float clip_width = 0;
        float clip_depth = 0;
        float vBar_for_clip_width = 0;
        float vBar_for_clips_move_behind_clips_value = 0;
        float clip_height_full = 0;


        List<GameObject> temporary_first_vBar_line_clip_List = null;

        //checking if the horizontal part is with clips
        if (DB_script.Clips.Contains(horizontal_part_name))
        {
            //rotate_around_center(horizontal_bar, new Vector3(0, 180, 0));
            clips_in_model = true;

            //ADD CLIPS
            string clip_name = "";

            if (space_h_bar == -10)
            {
                clip_name = "ak - 72";
                clip_height_full = 120.47f; //For space between 50, ak - 39

                //Load clip and horizontal combined prefab
                Bar_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-72-40", typeof(GameObject));
            }
            else if (space_h_bar == 50)
            {
                clip_name = "ak - 39";
                clip_height_full = 120.47f; //For space between 50, ak - 39

                //Load clip and horizontal combined prefab
                Bar_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-39", typeof(GameObject));
            }
            else if (space_h_bar == 20)
            {
                clip_name = "ak - 76";
                clip_height_full = 90.47f; // For space between 20, ak - 76

                //Load clip and horizontal combined prefab
                Bar_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-76", typeof(GameObject));
            }
            else
            {
                throw new Exception($"Invalid rafafa spacing. Valid spacing is -10, 20, 50.");
            }

            clip_spare_part_height = 3.5f; // the height which goes inside another clip.
            clip_width = 40.0f;
            clip_depth = 34.08f;
            vBar_for_clip_width = 40f;
            vBar_for_clips_move_behind_clips_value = -43.88f;


            float clip_height = Mathf.Ceil((clip_height_full - clip_spare_part_height));
            no_of_H_bars = Mathf.Floor((building_height - clip_spare_part_height) / clip_height_full);
            IntNumOf_H_bar = Convert.ToInt32(no_of_H_bars);

            h_Bar_offset = building_height - ((IntNumOf_H_bar * (clip_height_full - clip_spare_part_height)));
            //h_Bar_offset = h_Bar_offset / 2;


            temporary_first_vBar_line_clip_List = new List<GameObject>();
        }

        if (alignment == "MIDDLE")
            h_Bar_offset = h_Bar_offset / 2;
        else if (alignment == "BOTTOM")
            h_Bar_offset = 0;
        else if (alignment == "TOP")
            h_Bar_offset = h_Bar_offset;

        float Y_distance = h_Bar_offset;// h_bar_width;
                                        //to calculate h_bar_width,forward_offset_hbar
        if (Bar_Prefab != null)
        {
            //part_name = "ak - 18";

            //Frame_Prefab =Instantiate(  (GameObject)Resources.Load($"prefabs/{part_name}", typeof(GameObject)), GameObject.Find("HorizontalBar_Parent").transform);

            //Here we instatiate Dummy horizontal bar and find the offset nd destroy it 
            GameObject Dummy_h_bar = Instantiate(Bar_Prefab, HorizontalBar_Parent.transform);

            Bounds h_bar_bound = Calculate_b(Dummy_h_bar.transform);


            forward_offset_hbar = h_bar_bound.size.z;

            //Dummy_h_bar.transform.localPosition = new Vector3(0, Y_distance, -forward_offset_hbar);//Vector3(h_bar_width, X_distance, 0);
            Dummy_h_bar.transform.localScale = new Vector3(scale, 1, 1);


            h_bar_height = Dummy_h_bar.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.size.y;
            if (Dummy_h_bar != null)
            {
                DestroyImmediate(Dummy_h_bar);
            }
        }




        for (int j = 0; j < IntNumOf_H_bar; j++)
        {
            //if the bar exceeds building height then bar is not instatiated
            if (Y_distance > building_height) break;

            HorizontalBar_Parent = GameObject.Find("HorizontalBar_Parent");

            horizontal_bar = Instantiate(Bar_Prefab, HorizontalBar_Parent.transform);
            //Bounds h_bar_bound = Calculate_b(horizontal_bar.transform);

            //if(horizontal_bar.GetComponentInChildren<MeshRenderer>()!=null)
            //horizontal_bar.GetComponentInChildren<MeshRenderer>().material= Resources.Load("Materials/Horizontal_material", typeof(Material)) as Material;

            //forward_offset_hbar = h_bar_bound.size.z;
            horizontal_bar.transform.localPosition = new Vector3(0, Y_distance, -forward_offset_hbar);//Vector3(h_bar_width, Y_distance, 0);

            //if its the combined prefab
            if (DB_script.combined_Clips_names.Contains(Bar_Prefab.name))
            {

                GameObject clip = horizontal_bar.transform.GetChild(1).gameObject;

                //Bounds clip_bound = Calculate_b(clip.transform);

                //Bounds hbar_bound = Calculate_b(horizontal_bar.transform.GetChild(0));

                //forward_offset_hbar = hbar_bound.size.z + clip_bound.size.z;

                //horizontal_bar.transform.localPosition = new Vector3(0, Y_distance, -forward_offset_hbar);//-1.72f
                //Vector3 clip_pos = clip.transform.position;

                clip.transform.parent = HorizontalBar_Parent.transform;

                temporary_first_vBar_line_clip_List.Add(clip);


                //clip.transform.position = clip_pos;
                //clip.transform.parent = null;

                string clip_prefab_name = clip.name;

                //asigning the vertical bar no at the end as - VB0
                clip.name = clip_prefab_name + "_Side_A_" + j + "_VB0";
                #region chracteristics
                Characteristics clip_chars_script;

                if (clip.GetComponent<Characteristics>() != null)
                {
                    clip_chars_script = clip.GetComponent<Characteristics>();

                    //****** adiding old prafab name and unique id for part *****//
                    clip_chars_script.part_name_id = clip_prefab_name;
                    clip_chars_script.part_unique_id = Guid.NewGuid().ToString();

                    clip_chars_script.part_type = part_type_enum.accessory.ToString();
                }

                else
                {
                    clip_chars_script = clip.AddComponent<Characteristics>();

                    //****** adiding old prafab name and unique id for part *****//
                    clip_chars_script.part_name_id = clip_prefab_name;
                    clip_chars_script.part_unique_id = Guid.NewGuid().ToString();

                    clip_chars_script.part_type = part_type_enum.accessory.ToString();

                }

                #endregion

                //*************************Here the prefab contains (Parent ) ak - 40 ,children-> (ak -72, ak - 40)**********************************//
                if (Bar_Prefab.name.Contains("ak - 40-72-40"))
                {
                    GameObject second_curve_h_bar = horizontal_bar.transform.Find("ak - 40").gameObject;



                    second_curve_h_bar.transform.SetParent(HorizontalBar_Parent.transform, true);


                    second_curve_h_bar.transform.localScale = new Vector3(scale, 1, 1);

                    second_curve_h_bar.name = horizontal_part_name + "_2" + "_Side_A_" + j;

                    Characteristics second_hBar_chars_script = null;

                    if (second_curve_h_bar.GetComponent<Characteristics>() != null)
                    {
                        second_hBar_chars_script = second_curve_h_bar.GetComponent<Characteristics>();

                        second_hBar_chars_script.part_name_id = horizontal_part_name;

                        second_hBar_chars_script.part_unique_id = Guid.NewGuid().ToString();

                        second_hBar_chars_script.part_type = part_type_enum.horizontal.ToString();
                    }
                    else
                    {
                        second_hBar_chars_script = second_curve_h_bar.AddComponent<Characteristics>();

                        second_hBar_chars_script.part_name_id = horizontal_part_name;

                        second_hBar_chars_script.part_type = part_type_enum.horizontal.ToString();

                        second_hBar_chars_script.part_unique_id = Guid.NewGuid().ToString();
                    }

                    //if(second_curve_h_bar.transform.GetComponentInChildren<Renderer>()!=null)
                    //second_curve_h_bar.transform.GetComponentInChildren<Renderer>().material = Resources.Load("Materials/Horizontal_material", typeof(Material)) as Material;
                }
            }

            horizontal_bar.transform.localScale = new Vector3(scale, 1, 1);
            horizontal_bar.name = horizontal_part_name + "_Side_A_" + j;


            #region chracteristics

            //ataching Characteristics script
            Characteristics characteristics_script = horizontal_bar.AddComponent<Characteristics>();

            //Assigning Frame Prefab :eg:ak - 288
            characteristics_script.part_name_id = horizontal_part_name;//Bar_Prefab.name;

            characteristics_script.part_unique_id = Guid.NewGuid().ToString();

            characteristics_script.part_type = "horizontal";
            //here we add space and horizontal bar width , which will be new position for next bar
            //horizontal_bar.transform.Translate(-Vector3.forward * forward_offset_hbar);

            #endregion
            //**************as there are 2 horizontal bars inside the clip in this prefabe we mutiply 2* , while adding offset*******************//
            if (Bar_Prefab.name.Contains("ak - 40-72-40") || Bar_Prefab.name.Contains("ak - 40-72"))
                Y_distance += 2 * (space_h_bar + h_bar_height);
            else
                Y_distance += (space_h_bar + h_bar_height);

            //if there are clips then the clip_spare_part_height shoukd be decreased after 1st clip to all other clips
            if (clips_in_model)
            {

                //Y_distance=h_bar_width;
                Y_distance -= clip_spare_part_height;

                //Y_distance=clip_height_full- clip_spare_part_height;
            }

            //**************Raycast to move horizontal bars to place************************// 

            //RaycastHit hit;

            //if (Physics.Raycast(horizontal_bar.transform.GetChild(0).TransformPoint(horizontal_bar.transform.GetChild(0).transform.GetComponent<MeshFilter>().mesh.bounds.center), -horizontal_bar.transform.right, out hit, Mathf.Infinity,1<<end_V_Bar))
            //{
            //    float distance = hit.distance;
            //    float actual_distance = scale / 2;
            //    float move = distance - actual_distance;
            //    horizontal_bar.transform.Translate(Vector3.right * move);
            //}
            //moving h_bar forward
        }

        if (temporary_first_vBar_line_clip_List != null && temporary_first_vBar_line_clip_List.Count > 0)
        {
            Accessory_Parent = new GameObject("Accessory_Parent");

            Accessory_Parent.transform.parent = Grand_Parent.transform;



            GameObject first_V_bar = verticals_order.Keys.First();

            //foreach(GameObject v_bar in verticals_order.Keys )
            for (int k = 0; k < verticals_order.Count; k++)
            {


                if (k > 0)
                {
                    GameObject v_bar = verticals_order.ElementAt(k).Key;

                    float dist = Vector3.Distance(first_V_bar.transform.localPosition, v_bar.transform.localPosition);
                    if (Mathf.Floor((int)dist) > 0)
                    {
                        foreach (GameObject clipElement in temporary_first_vBar_line_clip_List)
                        {
                            //Adding Clip as child of Accessory_Parent
                            clipElement.transform.parent = Accessory_Parent.transform;

                            GameObject new_clip = Instantiate(clipElement, Accessory_Parent.transform);

                            new_clip.transform.Translate(new_clip.transform.right * dist);


                            ////Adding new_clip as child of Accessory_Parent



                            Characteristics characteristics_script;
                            if (new_clip.GetComponent<Characteristics>() != null)
                            {
                                characteristics_script = new_clip.GetComponent<Characteristics>();

                                //here part_name_id will be same as they are cloned game objects but the part_unique_id should be changed
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                                characteristics_script.part_type = part_type_enum.accessory.ToString();
                            }
                            else
                            {
                                //here part_name_id will be same as they are cloned game objects but the part_unique_id should be changed
                                characteristics_script = new_clip.AddComponent<Characteristics>();

                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                                characteristics_script.part_type = part_type_enum.accessory.ToString();
                            }

                            string clipName = clipElement.transform.name.ToString();
                            //clipName.Remove(clipName.IndexOf("("), clipName.IndexOf(")"));
                            int index = clipName.IndexOf("VB");
                            string sub = clipName.Substring(0, index);


                            new_clip.name = sub + $"VB{ k}";
                        }
                    }
                }
            }
        }
        //place_clips(space_h_bar, verticals_order.Keys.ToList());


        DB_script.apply_material();

        #region orthographic_camera size,near_clip,far_clip region setting
        //Bounds bounds1 = Calculate_b(VerticalBar_Parent.transform);
        //Bounds b2;


        //HorizontalBar_Parent.transform.position = new Vector3(-bounds1.center.x, -bounds1.center.y, -bounds1.center.z);//to place parent at the center 

        //VerticalBar_Parent.transform.position = new Vector3(-bounds1.center.x, -bounds1.center.y, -bounds1.center.z);//to place parent at the center

        //HorizontalBar_Parent.transform.parent = Grand_Parent.transform;

        //VerticalBar_Parent.transform.parent = Grand_Parent.transform;
        ////Parent.transform.parent = Grand_Parent.transform;
        //float margin = 2f;//extra gap given to the game object alomg x direction on screen
        //                  //Parent.AddComponent<MeshRenderer>();

        //Camera.main.transform.GetComponent<Camera>().orthographicSize = bounds1.extents.x * margin;//setting the orthographic camera size to fit the size of game object(Model)

        //Vector3 max_size = bounds1.max;

        //float x_max = max_size.x;
        //float y_max = max_size.y;
        //float z_max = max_size.z;

        ////float max_axis = x_max > y_max ? (x_max > z_max ? x_max : z_max) : (y_max > z_max ? y_max : z_max);

        //float max_axis = Mathf.Max(x_max, y_max, z_max);

        //Camera.main.transform.GetComponent<Camera>().nearClipPlane = -max_axis * 100;

        //Camera.main.transform.GetComponent<Camera>().farClipPlane = max_axis * 100;
        //Model.transform.parent= GameObject.Find("Parent").transform;
        #endregion

        //here we place resize the camera view port to match the model dimension
        await UnityMainThreadDispatcher.DispatchAsync(() =>camera_resizing());

        try
        {

            //Save_to_db();
        }
        catch (Exception ex)
        {
            Debug.Log("while saving to DB: " + ex);
        }
        return 5;
    }

    public static async void camera_resizing(bool from_DB_points = false)
    {
        #region orthographic_camera size,near_clip,far_clip region setting
        if (GameObject.Find("Grand_Parent") != null && GameObject.Find("HorizontalBar_Parent") != null && GameObject.Find("HorizontalBar_Parent") != null)
        {

            Grand_Parent = GameObject.Find("Grand_Parent");
            HorizontalBar_Parent = GameObject.Find("HorizontalBar_Parent");

            VerticalBar_Parent = GameObject.Find("VerticalBar_Parent");



            Bounds bounds1 = Calculate_b(VerticalBar_Parent.transform);


            //here we combine bounds of VerticalBar_Parent and HorizontalBar_Parent
            Bounds combined_bounds = bounds1;

            Bounds bounds2 = Calculate_b(HorizontalBar_Parent.transform);

            combined_bounds.Encapsulate(bounds2);

            if (Accessory_Parent != null)
            {
                Bounds bounds3 = Calculate_b(Accessory_Parent.transform);

                combined_bounds.Encapsulate(bounds3);
            }
            if (from_DB_points == false)
            {


                //if the Model was generated for the first time then we place the HorizontalBar_Parent & VerticalBar_Parent at the combined center
                HorizontalBar_Parent.transform.position = new Vector3(-combined_bounds.center.x, -combined_bounds.center.y, -combined_bounds.center.z);//to place parent at the center 

                VerticalBar_Parent.transform.position = new Vector3(-combined_bounds.center.x, -combined_bounds.center.y, -combined_bounds.center.z);//to place parent at the center

                if (Accessory_Parent != null)
                {
                    Accessory_Parent.transform.position = VerticalBar_Parent.transform.position = new Vector3(-combined_bounds.center.x, -combined_bounds.center.y, -combined_bounds.center.z);//to place parent at the center
                }

                HorizontalBar_Parent.transform.parent = Grand_Parent.transform;

                VerticalBar_Parent.transform.parent = Grand_Parent.transform;

                if (Accessory_Parent != null)
                {
                    Accessory_Parent.transform.parent = Grand_Parent.transform;
                }

                //Grand_Parent.transform.position = VerticalBar_Parent.transform.position;
            }
            //Attaching rotateonmouse script to new Game object
            if (Grand_Parent.gameObject.GetComponent<rotateonmouse>() == null)
            {
                rotateonmouse RotateOnMouseScript = Grand_Parent.gameObject.AddComponent<rotateonmouse>();

                ////setting target as Grand_parent
                RotateOnMouseScript.target = Grand_Parent.transform;
            }

            if (Grand_Parent.gameObject.GetComponent<PanAndZoom>() == null)
            {
                //Attaching pan and zoom script
                PanAndZoom PanAndZoomScript = Grand_Parent.gameObject.AddComponent<PanAndZoom>();
            }
            //Parent.transform.parent = Grand_Parent.transform;
            float margin = 2f;//extra gap given to the game object along x direction on screen
                              //Parent.AddComponent<MeshRenderer>();

            Camera.main.transform.GetComponent<Camera>().orthographicSize = combined_bounds.extents.x * margin;//setting the orthographic camera size to fit the size of game object(Model)

            Vector3 max_size = combined_bounds.max;

            float x_max = max_size.x;
            float y_max = max_size.y;
            float z_max = max_size.z;



            float max_axis = Mathf.Max(x_max, y_max, z_max);

            Camera.main.transform.GetComponent<Camera>().nearClipPlane = -max_axis * 100;

            Camera.main.transform.GetComponent<Camera>().farClipPlane = max_axis * 100;

        }
        #endregion
    }


    public static Bounds Calculate_b(Transform TheObject)//To calculate bound of all the bars 
    {
        var renderers = TheObject.GetComponentsInChildren<Renderer>();
        Bounds combinedBounds = renderers[0].bounds;
        for (int i = 0; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }
        return combinedBounds;
    }

}
