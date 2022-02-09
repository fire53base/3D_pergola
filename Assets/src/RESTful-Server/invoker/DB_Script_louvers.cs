using BzKovSoft.ObjectSlicerSamples;
using Newtonsoft.Json;
using RESTfulHTTPServer.src.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.ProBuilder;
using UnityEditor.ProBuilder.Actions;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnLogickFactory;
using static RESTfulHTTPServer.src.invoker.DB_script;


namespace RESTfulHTTPServer.src.invoker
{
    public class DB_Script_louvers : MonoBehaviour
    {

        public static SqlConnection sqlCnn = null;

        enum part_type_enum { horizontal, vertical, accessory, none, frame };

        public static GameObject Louver_Model;


        public static float _building_width, _building_height, _actual_building_width, _actual_building_height;

        public static bool louvers_I_type = false, louvers_L_type = false, louvers_U_type = false;

        public static GameObject VerticalBar_Parent, HorizontalBar_Parent, Grand_Parent, Accessory_Parent;

        public static Arrows_Measure_louvers arrows_Measure_Louvers_script;

        // Start is called before the first frame update
        void Start()
        {
            arrows_Measure_Louvers_script = GameObject.Find("Directional Light").GetComponent<Arrows_Measure_louvers>();
        }






        public async Task<Response> data_base_linking_Louvers(Request request)
        {
            completed = false;
            Response response = new Response();
            Debug.Log($"Application executing on thread {Thread.CurrentThread.ManagedThreadId}");
            var body = request.GetPOSTData();
            string json = body;


            pb_list = new List<ProBuilderMesh>();

            //deserializing json to get ID's

            louvers_info item = JsonConvert.DeserializeObject<louvers_info>(json);

            project_unique_id = item.project_unique_id;
            building_unique_id = item.building_unique_id;
            element_unique_id = item.element_unique_id;

            file_Dir = item.output_folder_path;

            unique_id = item.project_unique_id + "_" + item.building_unique_id + "_" + item.element_unique_id;

            string responseData = project_unique_id + "_" + building_unique_id + "_" + element_unique_id + ".obj";//".fbx";
            Task t = new Task(async () =>
            {
                Debug.Log($"Task executing on thread {Thread.CurrentThread.ManagedThreadId}");
                //string responseData = "test";

                connectionString = "";
                SqlCommand sqlCmd = null;
                SqlDataAdapter adapter = new SqlDataAdapter();

                //building measure sql-params
                string sql_element_louver_details = "";
                SqlCommand sqlCmd_get_building_mesurements = null;
                DataSet ds_element_louver_details = new DataSet();

                //project_elements sql-params
                string sql_element_louver_header = "";
                SqlCommand sqlCmd_get_project_elements = null;

                DataSet ds_element_louver_header = new DataSet();


                //connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=alukal;User ID=sa;Password=Password@1234 ";
                connectionString = @"Data Source=212.29.201.154,3701\SQLEXPRESS;Initial Catalog=alukal;User ID=sa;Password=Password@1234";
                //sql = $"select COUNT(*) as row_count from dbo.tbl_model  where project_unique_id = '1234'";

                //we are taking height and width of the model from "tbl_building_mesures" DB by taking 1 st top row from table
                sql_element_louver_details = $"select top(1) * from  tbl_element_louver_details where project_unique_id = '{item.project_unique_id}' and building_unique_id='{item.building_unique_id}' and  element_unique_id='{item.element_unique_id}'";

                //we are taking element details from DB
                sql_element_louver_header = $"select top(1) * from  tbl_element_louver_header  where project_unique_id = '{item.project_unique_id}' and building_unique_id='{item.building_unique_id}' and  element_unique_id='{item.element_unique_id}'";

                //we assign the connecting string to the variable cnn. The variable cnn, which is of type SqlConnection is used to establish the connection to the database.
                sqlCnn = new SqlConnection(connectionString);

                sqlCnn.Open();//to open the connection
                              //sqlCmd = new SqlCommand(sql, sqlCnn);

                try
                {
                    sqlCmd_get_building_mesurements = new SqlCommand(sql_element_louver_details, sqlCnn);
                    adapter.SelectCommand = sqlCmd_get_building_mesurements;
                    adapter.Fill(ds_element_louver_details);


                    sqlCmd_get_project_elements = new SqlCommand(sql_element_louver_header, sqlCnn);
                    adapter.SelectCommand = sqlCmd_get_project_elements;
                    adapter.Fill(ds_element_louver_header);

                    if (ds_element_louver_details.Tables.Count > 0 && ds_element_louver_header.Tables.Count > 0)
                    {

                        StartCoroutine(generate_model_Louvers(ds_element_louver_details, ds_element_louver_header));
                        //Task<int> result = await UnityMainThreadDispatcher.DispatchAsync(() => generate_model_Louvers(ds_element_louver_details, ds_element_louver_header));
                        //Debug.Log(result.Result);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Async method call : UnityMainThreadDispatcher.DispatchAsync(() => DB_points(ds)) : " + ex);
                }
                finally
                {
                    adapter.Dispose();
                    ////sqlCmd.Dispose();
                    //sqlCmd_get_building_mesurements.Dispose();
                    //sqlCmd_get_project_elements.Dispose();
                    sqlCnn.Close();
                }

                //responseData = unique_id + ".obj";//".fbx";
                response.SetHTTPStatusCode((int)HttpStatusCode.OK);

                response.SetContent(responseData);
                response.SetMimeType(Response.MIME_CONTENT_TYPE_TEXT);

            });
            //wait untill the above action to complete
            t.RunSynchronously();
            //t.Wait();
            await t;

            float secs = 0;
            while (!completed)
            {
                int delay = 1000;
                secs = secs + delay;

                await Task.Delay(delay);
                if (secs > 120000 * 2)
                {
                    Debug.Log("Took very long to generate");

                    //setting error code
                    responseData = "Took very long to generate";//".fbx";
                    response.SetHTTPStatusCode((int)HttpStatusCode.InternalServerError);
                    response.SetContent(responseData);
                    response.SetMimeType(Response.MIME_CONTENT_TYPE_TEXT);
                    completed = true;
                }
            }


            return response;
            /*file directory where obj's are stored*/
        }

        public enum RafafaAlignment
        {
            bottom,
            top,
            middle
        }

        public enum RafafaAdjustmentDirections
        {
            horizontal,
            vertical
        }
        public enum RafafaAdjustmentAlignments
        {
            top,
            center,
            bottom,
            left,
            right
        }
        //public async Task<int> generate_model_Louvers

        IEnumerator generate_model_Louvers(DataSet ds_element_louver_details, DataSet ds_element_louver_header)
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

            if (GameObject.Find("Louver_Model") != null)
            {
                DestroyImmediate(GameObject.Find("Louver_Model"));
            }

            #region Creation of Empty Parents

            Louver_Model = new GameObject("Louver_Model");



            HorizontalBar_Parent = new GameObject("HorizontalBar_Parent");
            HorizontalBar_Parent.transform.parent = Louver_Model.transform;


            VerticalBar_Parent = new GameObject("VerticalBar_Parent");
            VerticalBar_Parent.transform.parent = Louver_Model.transform;


            #endregion

            #endregion

            //element_louver_header Params

            int element_shape_id = int.Parse(ds_element_louver_header.Tables[0].Rows[0]["element_shape_id"].ToString());
            string element_sub_type_id = ds_element_louver_header.Tables[0].Rows[0]["element_sub_type_id"].ToString();
            string start_floor = ds_element_louver_header.Tables[0].Rows[0]["start_floor"].ToString();
            string end_floor = ds_element_louver_header.Tables[0].Rows[0]["end_floor"].ToString();
            string rafafa_type = ds_element_louver_header.Tables[0].Rows[0]["rafafa_type"].ToString();
            string rafafa_color_texture = ds_element_louver_header.Tables[0].Rows[0]["rafafa_color_texture"].ToString();
            float rafafa_spacing = float.Parse(ds_element_louver_header.Tables[0].Rows[0]["rafafa_spacing"].ToString());
            string rafafa_alignment_id = ds_element_louver_header.Tables[0].Rows[0]["rafafa_alignment_id"].ToString();
            string middle_pole_type = ds_element_louver_header.Tables[0].Rows[0]["middle_pole_type"].ToString();
            string side_pole_type = ds_element_louver_header.Tables[0].Rows[0]["side_pole_type"].ToString();
            string screw_type = ds_element_louver_header.Tables[0].Rows[0]["screw_type"].ToString();
            int element_divider_type_id = int.Parse(ds_element_louver_header.Tables[0].Rows[0]["element_divider_type_id"].ToString());
            float element_divider_custom_size = float.Parse(ds_element_louver_header.Tables[0].Rows[0]["element_divider_custom_size"].ToString());
            string top_frame_type_part_ids = ds_element_louver_header.Tables[0].Rows[0]["top_frame_type_part_ids"].ToString();
            string bottom_frame_type_part_ids = ds_element_louver_header.Tables[0].Rows[0]["bottom_frame_type_part_ids"].ToString();
            string gap_fill_on_sides_id = ds_element_louver_header.Tables[0].Rows[0]["gap_fill_on_sides_id"].ToString();
            string measuring_side = ds_element_louver_header.Tables[0].Rows[0]["measuring_side"].ToString();
            string side_A_vpole_distances = ds_element_louver_header.Tables[0].Rows[0]["side_A_vpole_distances"].ToString();
            //here string array of space between Vertical is taken
            string[] side_A_vpole_distances_Array = side_A_vpole_distances.ToString().Split(',');

            string side_B_vpole_distances = ds_element_louver_header.Tables[0].Rows[0]["side_B_vpole_distances"].ToString();
            //here string array of space between Vertical is taken
            string[] side_B_vpole_distances_Array = side_B_vpole_distances.ToString().Split(',');

            string side_C_vpole_distances = ds_element_louver_header.Tables[0].Rows[0]["side_C_vpole_distances"].ToString();
            //here string array of space between Vertical is taken
            string[] side_C_vpole_distances_Array = side_C_vpole_distances.ToString().Split(',');

            string side_A_vpole_EAR_direction = ds_element_louver_header.Tables[0].Rows[0]["side_A_vpole_EAR_direction"].ToString();
            string[] side_A_vpole_EAR_direction_Array = side_A_vpole_EAR_direction.ToString().Split(',');

            string side_B_vpole_EAR_direction = ds_element_louver_header.Tables[0].Rows[0]["side_B_vpole_EAR_direction"].ToString();
            string[] side_B_vpole_EAR_direction_Array = side_B_vpole_EAR_direction.ToString().Split(',');


            string side_C_vpole_EAR_direction = ds_element_louver_header.Tables[0].Rows[0]["side_C_vpole_EAR_direction"].ToString();
            string[] side_C_vpole_EAR_direction_Array = side_C_vpole_EAR_direction.ToString().Split(',');


            string element_shape_rotation_orientation_id = ds_element_louver_header.Tables[0].Rows[0]["element_shape_rotation_orientation_id"].ToString();

            int no_of_v_barA = (side_A_vpole_distances_Array.Length + 1);
            int no_of_v_barB = (side_B_vpole_distances_Array.Length + 1);
            int no_of_v_barC = (side_C_vpole_distances_Array.Length + 1);

            //element_louver_details Params

            //ds_element_louver_details.Tables[0].Rows.Count
            //for (int i = 0; i< ds_element_louver_details.Tables[0].Rows.Count; i++)
            //{

            string floor_cubeNum_info = ds_element_louver_details.Tables[0].Rows[0]["floor_cubeNum_info"].ToString();
            string object_type = ds_element_louver_details.Tables[0].Rows[0]["object_type"].ToString();
            string measure_type = ds_element_louver_details.Tables[0].Rows[0]["measure_type"].ToString();
            string value = ds_element_louver_details.Tables[0].Rows[0]["value"].ToString();
            string building_side = ds_element_louver_details.Tables[0].Rows[0]["building_side"].ToString();
            string num_of_floors = ds_element_louver_details.Tables[0].Rows[0]["num_of_floors"].ToString();
            string num_of_podest = ds_element_louver_details.Tables[0].Rows[0]["num_of_podest"].ToString();
            float building_width = float.Parse(ds_element_louver_details.Tables[0].Rows[0]["building_width"].ToString());
            float building_height = float.Parse(ds_element_louver_details.Tables[0].Rows[0]["building_height"].ToString());
            float building_actual_width = float.Parse(ds_element_louver_details.Tables[0].Rows[0]["building_actual_width"].ToString());
            float building_actual_height = float.Parse(ds_element_louver_details.Tables[0].Rows[0]["building_actual_height"].ToString());

            _building_width = building_width;
            _building_height = building_height;

            _actual_building_width = building_actual_width;
            _actual_building_height = building_actual_height;


            connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=alukal;User ID=sa;Password=Password@1234 ";

            //here we access the details of the parts , by passing their id
            PartSettings_Header_id rafafa_type_part_setting = new PartSettings_Header_id(rafafa_type, connectionString);

            PartSettings_Header_id side_pole_type_part_setting;


            PartSettings_Header_id middle_pole_type_part_setting = new PartSettings_Header_id(middle_pole_type, connectionString);

            GameObject horizontal_Prefab = (GameObject)Resources.Load($"prefabs/{rafafa_type_part_setting._part_id}", typeof(GameObject));

            GameObject vertical_Prefab = (GameObject)Resources.Load($"prefabs/{middle_pole_type_part_setting._part_id}", typeof(GameObject));

            GameObject side_pole_Prefab;
            if (!string.IsNullOrEmpty(side_pole_type) && side_pole_type.ToLower() != "none")
            {
                side_pole_type_part_setting = new PartSettings_Header_id(side_pole_type, connectionString);

                side_pole_Prefab = (GameObject)Resources.Load($"prefabs/{side_pole_type_part_setting._part_id}", typeof(GameObject));

            }
            string rafafa_part_type = rafafa_type_part_setting._part_type;
            string rafafa_part_id = rafafa_type_part_setting._part_id;
            string vertical_part_type = middle_pole_type_part_setting._part_type;
            string vertical_part_id = middle_pole_type_part_setting._part_id;
            //Frame Divider Code

            string vertical_part_name = "VerticalBar";
            string horizontal_part_name = "HorizontalBar";
            string horizontal_bar_name = "HorizontalBar";

            float verticalPoleDistance = 0f;
            DataTable general_setting_header_dataTable;

            float cut_length = 0;
            if (element_divider_type_id == 0)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        SqlDataAdapter daPartSetting = new SqlDataAdapter();
                        con.Open();
                        string qry = "";

                        qry = $"select * from tbl_general_setting_header";
                        SqlCommand sql = new SqlCommand(qry, con);
                        daPartSetting.SelectCommand = sql;
                        general_setting_header_dataTable = new DataTable();
                        daPartSetting.Fill(general_setting_header_dataTable);
                        con.Close();

                        cut_length = float.Parse(general_setting_header_dataTable.Rows[0]["max_paint_height"].ToString());
                    }
                }
                catch (Exception ex)
                {

                    Debug.Log("tbl_general_setting_header :" + ex);
                }
            }
            else if (element_divider_type_id == 1)
            {
                PartSettings_Header_Vertical_join midpole_cut_settings = new PartSettings_Header_Vertical_join(middle_pole_type, connectionString);

                cut_length = midpole_cut_settings._pole_max_length;
            }
            else if (element_divider_type_id == 2)
            {
                //element_divider_custom_size;

                cut_length = element_divider_custom_size;
            }

            if (element_shape_id == 0)
            {
                //int section_count =(int)Mathf.FloorToInt( building_height / cut_length);

                float remaining_length = building_height;// building_height % cut_length;

                verticalPoleDistance = 0f;
                if (building_side == "A")
                {
                    List<GameObject> temporary_first_vBar_line_clip_List = null;

                    float forward_offset_hbar = 0;

                    List<Transform> hori_parents = new List<Transform>();

                    List<Transform> parented_hori = new List<Transform>();


                    if (!String.IsNullOrEmpty(rafafa_type_part_setting._part_id)) //checking for hollow pergola
                    {

                        float scale = 1840;
                        scale = building_width;
                        //string horizontal_part_name = "ak - 18";
                        horizontal_part_name = rafafa_type_part_setting._part_id;
                        horizontal_Prefab = (GameObject)Resources.Load($"prefabs/{horizontal_part_name}", typeof(GameObject));


                        float h_bar_height = 0;
                        if (horizontal_Prefab == null)
                        {
                            horizontal_part_name = "ak - 18";

                            horizontal_Prefab = (GameObject)Resources.Load($"prefabs/{horizontal_part_name}", typeof(GameObject));
                        }


                        if (horizontal_Prefab != null)
                        {
                            //part_name = "ak - 18";

                            //Frame_Prefab =Instantiate(  (GameObject)Resources.Load($"prefabs/{part_name}", typeof(GameObject)), GameObject.Find("HorizontalBar_Parent").transform);

                            //Here we instatiate Dummy horizontal bar and find the offset nd destroy it 
                            GameObject Dummy_h_bar = Instantiate(horizontal_Prefab, HorizontalBar_Parent.transform);

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

                        float no_of_H_bars = Mathf.Floor(((building_height + rafafa_spacing) / (h_bar_height + rafafa_spacing)));//Mathf.Floor
                        int IntNumOf_H_bar = Convert.ToInt32(no_of_H_bars);


                        //HORIZONTALS ALWAYS MIDDLE ALIGNED.

                        //The difference of actual building height and the building heigth till the h_brs are placed --gives "h_Bar_offset"

                        float h_Bar_offset = building_height - ((IntNumOf_H_bar * h_bar_height) + (rafafa_spacing * (IntNumOf_H_bar - 1)));//top

                        string alignment = (Enum.GetName(typeof(RafafaAlignment), int.Parse(rafafa_alignment_id)));
                        float space_h_bar = rafafa_spacing;// building_height / 2;+h_bar_width


                        bool clips_in_model = false;



                        float clip_spare_part_height = 0; // the height which goes inside another clip.
                        float clip_width = 0;
                        float clip_depth = 0;
                        float vBar_for_clip_width = 0;
                        float vBar_for_clips_move_behind_clips_value = 0;
                        float clip_height_full = 0;




                        if (horizontal_part_name == "ak - 40")
                        {
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
                                    horizontal_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-72-40", typeof(GameObject));
                                }
                                else if (space_h_bar == 50)
                                {
                                    clip_name = "ak - 39";
                                    clip_height_full = 120.47f; //For space between 50, ak - 39

                                    //Load clip and horizontal combined prefab
                                    horizontal_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-39", typeof(GameObject));
                                }
                                else if (space_h_bar == 20)
                                {
                                    clip_name = "ak - 76";
                                    clip_height_full = 90.47f; // For space between 20, ak - 76

                                    //Load clip and horizontal combined prefab
                                    horizontal_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-76", typeof(GameObject));
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


                                float clip_height = ((clip_height_full - clip_spare_part_height));
                                no_of_H_bars = ((building_height - clip_spare_part_height) / clip_height_full);
                                IntNumOf_H_bar = Convert.ToInt32(no_of_H_bars);

                                h_Bar_offset = building_height - ((IntNumOf_H_bar * (clip_height_full - clip_spare_part_height)));
                                //h_Bar_offset = h_Bar_offset / 2;


                                temporary_first_vBar_line_clip_List = new List<GameObject>();


                                if (string.Equals(alignment, "MIDDLE", StringComparison.OrdinalIgnoreCase))
                                    h_Bar_offset = h_Bar_offset / 2;
                                else if (string.Equals(alignment, "BOTTOM", StringComparison.OrdinalIgnoreCase))
                                    h_Bar_offset = 0;
                                else if (string.Equals(alignment, "TOP", StringComparison.OrdinalIgnoreCase))
                                    h_Bar_offset = h_Bar_offset;
                                else
                                    h_Bar_offset = 0;

                                float Y_distance = h_Bar_offset;// h_bar_width;
                                                                //to calculate h_bar_width,forward_offset_hbar
                                if (horizontal_Prefab != null)
                                {
                                    //part_name = "ak - 18";

                                    //Frame_Prefab =Instantiate(  (GameObject)Resources.Load($"prefabs/{part_name}", typeof(GameObject)), GameObject.Find("HorizontalBar_Parent").transform);

                                    //Here we instatiate Dummy horizontal bar and find the offset nd destroy it 
                                    GameObject Dummy_h_bar = Instantiate(horizontal_Prefab, HorizontalBar_Parent.transform);

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
                                    //if (horizontal_Prefab.name.Contains("ak - 40-72-40") || horizontal_Prefab.name.Contains("ak - 40-72"))
                                    //{
                                    //    if(Y_distance + 2 * (h_bar_height)>building_height)
                                    //        break;
                                    //}
                                    //else
                                    //{
                                    //   if( Y_distance +( h_bar_height)>building_height)
                                    //        break;
                                    //}
                                    if (Y_distance > building_height) break;

                                    HorizontalBar_Parent = GameObject.Find("HorizontalBar_Parent");


                                    //GameObject horizontal = new GameObject("Horizontal" + j);
                                    //horizontal.transform.parent = HorizontalBar_Parent.transform;

                                    GameObject horizontal_bar = Instantiate(horizontal_Prefab, HorizontalBar_Parent.transform);//horizontal.transform

                                    //Bounds h_bar_bound = Calculate_b(horizontal_bar.transform);

                                    //if(horizontal_bar.GetComponentInChildren<MeshRenderer>()!=null)
                                    //horizontal_bar.GetComponentInChildren<MeshRenderer>().material= Resources.Load("Materials/Horizontal_material", typeof(Material)) as Material;

                                    //forward_offset_hbar = h_bar_bound.size.z;
                                    horizontal_bar.transform.localPosition = new Vector3(0, Y_distance, -forward_offset_hbar);//Vector3(h_bar_width, Y_distance, 0);

                                    hori_parents.Add(horizontal_bar.transform);
                                    //if its the combined prefab
                                    if (DB_script.combined_Clips_names.Contains(horizontal_Prefab.name))
                                    {

                                        GameObject clip = horizontal_bar.transform.GetChild(1).gameObject;

                                        //Bounds clip_bound = Calculate_b(clip.transform);

                                        //Bounds hbar_bound = Calculate_b(horizontal_bar.transform.GetChild(0));

                                        //forward_offset_hbar = hbar_bound.size.z + clip_bound.size.z;

                                        //horizontal_bar.transform.localPosition = new Vector3(0, Y_distance, -forward_offset_hbar);//-1.72f
                                        //Vector3 clip_pos = clip.transform.position;

                                        clip.transform.parent = HorizontalBar_Parent.transform;

                                        //GameObject clip_parent = new GameObject("clips" + j);
                                        //clip_parent.transform.parent = HorizontalBar_Parent.transform;

                                        //clip.transform.parent = clip_parent.transform;

                                        temporary_first_vBar_line_clip_List.Add(clip);

                                        hori_parents.Add(clip.transform);
                                        //clip.transform.position = clip_pos;
                                        //clip.transform.parent = null;

                                        string clip_prefab_name = clip_name;// clip.name;

                                        //asigning the vertical bar no at the end as - VB0
                                        clip.name = "Clip" + "_Side_A_" + j + "_VB0";// clip_prefab_name + "_Side_A_" + j + "_VB0";
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
                                        if (horizontal_Prefab.name.Contains("ak - 40-72-40"))
                                        {
                                            GameObject second_curve_h_bar = horizontal_bar.transform.Find("ak - 40").gameObject;

                                            string sec_clip_prefab_name = second_curve_h_bar.name;

                                            second_curve_h_bar.transform.SetParent(HorizontalBar_Parent.transform, true);
                                            //second_curve_h_bar.transform.SetParent(horizontal.transform, true);


                                            second_curve_h_bar.transform.localScale = new Vector3(scale, 1, 1);

                                            second_curve_h_bar.name = horizontal_bar_name + "_2" + "_Side_A_" + j;

                                            Characteristics second_hBar_chars_script = null;

                                            hori_parents.Add(second_curve_h_bar.transform);

                                            if (second_curve_h_bar.GetComponent<Characteristics>() != null)
                                            {
                                                second_hBar_chars_script = second_curve_h_bar.GetComponent<Characteristics>();

                                                //second_hBar_chars_script.part_name_id = second_curve_h_bar.name;// horizontal_part_name;

                                                //second_hBar_chars_script.part_unique_id = Guid.NewGuid().ToString();

                                                //second_hBar_chars_script.part_type = part_type_enum.horizontal.ToString();
                                            }
                                            else
                                            {
                                                second_hBar_chars_script = second_curve_h_bar.AddComponent<Characteristics>();

                                            }

                                            second_hBar_chars_script.part_name_id = sec_clip_prefab_name;// second_curve_h_bar.name;// horizontal_part_name;

                                            second_hBar_chars_script.part_type = part_type_enum.horizontal.ToString();

                                            second_hBar_chars_script.part_unique_id = Guid.NewGuid().ToString();
                                            //if(second_curve_h_bar.transform.GetComponentInChildren<Renderer>()!=null)
                                            //second_curve_h_bar.transform.GetComponentInChildren<Renderer>().material = Resources.Load("Materials/Horizontal_material", typeof(Material)) as Material;
                                        }
                                    }

                                    horizontal_bar.transform.localScale = new Vector3(scale, 1, 1);
                                    horizontal_bar.name = horizontal_bar_name + "_Side_A_" + j;


                                    #region chracteristics

                                    //ataching Characteristics script
                                    Characteristics characteristics_script = horizontal_bar.AddComponent<Characteristics>();

                                    //Assigning Frame Prefab :eg:ak - 288
                                    characteristics_script.part_name_id = horizontal_part_name;// clip_name;// horizontal_bar.name;// horizontal_part_name;//horizontal_Prefab.name;

                                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                                    characteristics_script.part_type = "horizontal";
                                    //here we add space and horizontal bar width , which will be new position for next bar
                                    //horizontal_bar.transform.Translate(-Vector3.forward * forward_offset_hbar);

                                    #endregion
                                    //**************as there are 2 horizontal bars inside the clip in this prefabe we mutiply 2* , while adding offset*******************//
                                    if (horizontal_Prefab.name.Contains("ak - 40-72-40") || horizontal_Prefab.name.Contains("ak - 40-72"))
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

                            }
                        }
                        else
                        {

                            //checking if the horizontal part is with clips
                            //if (DB_script.Clips.Contains(horizontal_part_name))
                            //{
                            //    //rotate_around_center(horizontal_bar, new Vector3(0, 180, 0));


                            //    clips_in_model = true;

                            //    //ADD CLIPS
                            //    string clip_name = "";

                            //    if (space_h_bar == -10)
                            //    {
                            //        clip_name = "ak - 72";
                            //        clip_height_full = 120.47f; //For space between 50, ak - 39

                            //        //Load clip and horizontal combined prefab
                            //        horizontal_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-72-40", typeof(GameObject));
                            //    }
                            //    else if (space_h_bar == 50)
                            //    {
                            //        clip_name = "ak - 39";
                            //        clip_height_full = 120.47f; //For space between 50, ak - 39

                            //        //Load clip and horizontal combined prefab
                            //        horizontal_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-39", typeof(GameObject));
                            //    }
                            //    else if (space_h_bar == 20)
                            //    {
                            //        clip_name = "ak - 76";
                            //        clip_height_full = 90.47f; // For space between 20, ak - 76

                            //        //Load clip and horizontal combined prefab
                            //        horizontal_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-76", typeof(GameObject));
                            //    }
                            //    else
                            //    {
                            //        throw new Exception($"Invalid rafafa spacing. Valid spacing is -10, 20, 50.");
                            //    }

                            //    clip_spare_part_height = 3.5f; // the height which goes inside another clip.
                            //    clip_width = 40.0f;
                            //    clip_depth = 34.08f;
                            //    vBar_for_clip_width = 40f;
                            //    vBar_for_clips_move_behind_clips_value = -43.88f;


                            //    float clip_height = Mathf.Ceil((clip_height_full - clip_spare_part_height));
                            //    no_of_H_bars = Mathf.Ceil((building_height - clip_spare_part_height) / clip_height_full);
                            //    IntNumOf_H_bar = Convert.ToInt32(no_of_H_bars);

                            //    h_Bar_offset = building_height - ((IntNumOf_H_bar * (clip_height_full - clip_spare_part_height)));
                            //    //h_Bar_offset = h_Bar_offset / 2;


                            //    temporary_first_vBar_line_clip_List = new List<GameObject>();
                            //}

                            if (string.Equals(alignment, "MIDDLE", StringComparison.OrdinalIgnoreCase))
                                h_Bar_offset = h_Bar_offset / 2;
                            else if (string.Equals(alignment, "BOTTOM", StringComparison.OrdinalIgnoreCase))
                                h_Bar_offset = 0;
                            else if (string.Equals(alignment, "TOP", StringComparison.OrdinalIgnoreCase))
                                h_Bar_offset = h_Bar_offset;
                            else
                                h_Bar_offset = 0;

                            float Y_distance = h_Bar_offset;// h_bar_width;
                                                            //to calculate h_bar_width,forward_offset_hbar
                            if (horizontal_Prefab != null)
                            {
                                //part_name = "ak - 18";

                                //Frame_Prefab =Instantiate(  (GameObject)Resources.Load($"prefabs/{part_name}", typeof(GameObject)), GameObject.Find("HorizontalBar_Parent").transform);

                                //Here we instatiate Dummy horizontal bar and find the offset nd destroy it 
                                GameObject Dummy_h_bar = Instantiate(horizontal_Prefab, HorizontalBar_Parent.transform);

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
                                if (Y_distance + h_bar_height > building_height) break;

                                HorizontalBar_Parent = GameObject.Find("HorizontalBar_Parent");

                                //GameObject horizontal = new GameObject("Horizontal" + j);
                                //horizontal.transform.parent = HorizontalBar_Parent.transform;

                                GameObject horizontal_bar = Instantiate(horizontal_Prefab, HorizontalBar_Parent.transform);//horizontal.transform
                                //Bounds h_bar_bound = Calculate_b(horizontal_bar.transform);
                                if (horizontal_bar.transform.GetChild(0).gameObject.GetComponent<BoxCollider>() == null)
                                    horizontal_bar.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                //horizontal_bar.transform.GetChild(0).gameObject.layer = horizontal_layer;
                                //if(horizontal_bar.GetComponentInChildren<MeshRenderer>()!=null)
                                //horizontal_bar.GetComponentInChildren<MeshRenderer>().material= Resources.Load("Materials/Horizontal_material", typeof(Material)) as Material;

                                //forward_offset_hbar = h_bar_bound.size.z;
                                horizontal_bar.transform.localPosition = new Vector3(0, Y_distance, -forward_offset_hbar);//Vector3(h_bar_width, Y_distance, 0);
                                hori_parents.Add(horizontal_bar.transform);
                                //if its the combined prefab
                                #region clip arr
                                //if (DB_script.combined_Clips_names.Contains(horizontal_Prefab.name))
                                //{

                                //    GameObject clip = horizontal_bar.transform.GetChild(1).gameObject;

                                //    //Bounds clip_bound = Calculate_b(clip.transform);

                                //    //Bounds hbar_bound = Calculate_b(horizontal_bar.transform.GetChild(0));

                                //    //forward_offset_hbar = hbar_bound.size.z + clip_bound.size.z;

                                //    //horizontal_bar.transform.localPosition = new Vector3(0, Y_distance, -forward_offset_hbar);//-1.72f
                                //    //Vector3 clip_pos = clip.transform.position;

                                //    clip.transform.parent = HorizontalBar_Parent.transform;

                                //    temporary_first_vBar_line_clip_List.Add(clip);


                                //    //clip.transform.position = clip_pos;
                                //    //clip.transform.parent = null;

                                //    string clip_prefab_name = clip.name;

                                //    //asigning the vertical bar no at the end as - VB0
                                //    clip.name = clip_prefab_name + "_Side_A_" + j + "_VB0";
                                //    #region chracteristics
                                //    Characteristics clip_chars_script;

                                //    if (clip.GetComponent<Characteristics>() != null)
                                //    {
                                //        clip_chars_script = clip.GetComponent<Characteristics>();

                                //        //****** adiding old prafab name and unique id for part *****//
                                //        clip_chars_script.part_name_id = clip_prefab_name;
                                //        clip_chars_script.part_unique_id = Guid.NewGuid().ToString();

                                //        clip_chars_script.part_type = part_type_enum.accessory.ToString();
                                //    }

                                //    else
                                //    {
                                //        clip_chars_script = clip.AddComponent<Characteristics>();

                                //        //****** adiding old prafab name and unique id for part *****//
                                //        clip_chars_script.part_name_id = clip_prefab_name;
                                //        clip_chars_script.part_unique_id = Guid.NewGuid().ToString();

                                //        clip_chars_script.part_type = part_type_enum.accessory.ToString();

                                //    }

                                //    #endregion

                                //    //*************************Here the prefab contains (Parent ) ak - 40 ,children-> (ak -72, ak - 40)**********************************//
                                //    if (horizontal_Prefab.name.Contains("ak - 40-72-40"))
                                //    {
                                //        GameObject second_curve_h_bar = horizontal_bar.transform.Find("ak - 40").gameObject;



                                //        second_curve_h_bar.transform.SetParent(HorizontalBar_Parent.transform, true);


                                //        second_curve_h_bar.transform.localScale = new Vector3(scale, 1, 1);

                                //        second_curve_h_bar.name = horizontal_part_name + "_2" + "_Side_A_" + j;

                                //        Characteristics second_hBar_chars_script = null;

                                //        if (second_curve_h_bar.GetComponent<Characteristics>() != null)
                                //        {
                                //            second_hBar_chars_script = second_curve_h_bar.GetComponent<Characteristics>();

                                //            second_hBar_chars_script.part_name_id = horizontal_part_name;

                                //            second_hBar_chars_script.part_unique_id = Guid.NewGuid().ToString();

                                //            second_hBar_chars_script.part_type = part_type_enum.horizontal.ToString();
                                //        }
                                //        else
                                //        {
                                //            second_hBar_chars_script = second_curve_h_bar.AddComponent<Characteristics>();

                                //            second_hBar_chars_script.part_name_id = horizontal_part_name;

                                //            second_hBar_chars_script.part_type = part_type_enum.horizontal.ToString();

                                //            second_hBar_chars_script.part_unique_id = Guid.NewGuid().ToString();
                                //        }

                                //        //if(second_curve_h_bar.transform.GetComponentInChildren<Renderer>()!=null)
                                //        //second_curve_h_bar.transform.GetComponentInChildren<Renderer>().material = Resources.Load("Materials/Horizontal_material", typeof(Material)) as Material;
                                //    }
                                //}
                                #endregion
                                horizontal_bar.transform.localScale = new Vector3(scale, 1, 1);
                                horizontal_bar.name = horizontal_bar_name + "_Side_A_" + j;


                                #region chracteristics

                                //ataching Characteristics script
                                Characteristics characteristics_script = horizontal_bar.AddComponent<Characteristics>();

                                //Assigning Frame Prefab :eg:ak - 288
                                characteristics_script.part_name_id = horizontal_Prefab.name;// horizontal_bar.name;// horizontal_part_name;//horizontal_Prefab.name;

                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                                characteristics_script.part_type = "horizontal";
                                //here we add space and horizontal bar width , which will be new position for next bar
                                //horizontal_bar.transform.Translate(-Vector3.forward * forward_offset_hbar);

                                #endregion
                                //**************as there are 2 horizontal bars inside the clip in this prefabe we mutiply 2* , while adding offset*******************//
                                //if (horizontal_Prefab.name.Contains("ak - 40-72-40") || horizontal_Prefab.name.Contains("ak - 40-72"))
                                //    Y_distance += 2 * (space_h_bar + h_bar_height);
                                //else
                                Y_distance += (space_h_bar + h_bar_height);

                                //if there are clips then the clip_spare_part_height shoukd be decreased after 1st clip to all other clips
                                //if (clips_in_model)
                                //{

                                //    //Y_distance=h_bar_width;
                                //    Y_distance -= clip_spare_part_height;

                                //    //Y_distance=clip_height_full- clip_spare_part_height;
                                //}

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

                            //if (temporary_first_vBar_line_clip_List != null && temporary_first_vBar_line_clip_List.Count > 0)
                            //{
                            //    Accessory_Parent = new GameObject("Accessory_Parent");

                            //    Accessory_Parent.transform.parent = Louver_Model.transform;



                            //    GameObject first_V_bar = verticals_order.Keys.First();

                            //    //foreach(GameObject v_bar in verticals_order.Keys )
                            //    for (int k = 0; k < verticals_order.Count; k++)
                            //    {


                            //        if (k > 0)
                            //        {
                            //            GameObject v_bar = verticals_order.ElementAt(k).Key;

                            //            float dist = Vector3.Distance(first_V_bar.transform.localPosition, v_bar.transform.localPosition);
                            //            if (Mathf.Floor((int)dist) > 0)
                            //            {
                            //                foreach (GameObject clipElement in temporary_first_vBar_line_clip_List)
                            //                {
                            //                    //Adding Clip as child of Accessory_Parent
                            //                    clipElement.transform.parent = Accessory_Parent.transform;

                            //                    GameObject new_clip = Instantiate(clipElement, Accessory_Parent.transform);

                            //                    new_clip.transform.Translate(new_clip.transform.right * dist);


                            //                    ////Adding new_clip as child of Accessory_Parent



                            //                    Characteristics characteristics_script;
                            //                    if (new_clip.GetComponent<Characteristics>() != null)
                            //                    {
                            //                        characteristics_script = new_clip.GetComponent<Characteristics>();

                            //                        //here part_name_id will be same as they are cloned game objects but the part_unique_id should be changed
                            //                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                            //                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                            //                    }
                            //                    else
                            //                    {
                            //                        //here part_name_id will be same as they are cloned game objects but the part_unique_id should be changed
                            //                        characteristics_script = new_clip.AddComponent<Characteristics>();

                            //                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                            //                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                            //                    }

                            //                    string clipName = clipElement.transform.name.ToString();
                            //                    //clipName.Remove(clipName.IndexOf("("), clipName.IndexOf(")"));
                            //                    int index = clipName.IndexOf("VB");
                            //                    string sub = clipName.Substring(0, index);


                            //                    new_clip.name = sub + $"VB{ k}";
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                        }



                    }

                    //await Task.Delay(300);


                    yield return new WaitForSeconds(0.01F);

                    //*****************Vertical Bars section ****************************************//
                    SortedDictionary<int, float> section_name_height_list = new SortedDictionary<int, float>();

                    float sum_cut_length = 0;

                    int k = 0;

                    //scale length changes according to remaining length
                    float scale_length = cut_length;

                    ///************************************//Here we check if the vertical section should be built by sectioning ,we add the scale length to sum_cut_length and subtract sum length from remaining_length****************//
                    bool build_section = true;
                    while (build_section)
                    {
                        verticalPoleDistance = 0f;


                        Vector3 origin = new Vector3(verticalPoleDistance + middle_pole_type_part_setting._part_width / 2, sum_cut_length + cut_length, middle_pole_type_part_setting._part_depth / 2);
                        Debug.DrawRay(origin, -Vector3.forward * 10000, Color.blue, 1500f);



                        Bounds hit_bar_bounds;
                        RaycastHit hit1;
                        if (Physics.Raycast(origin, -Vector3.forward, out hit1, Mathf.Infinity))
                        {
                            Debug.Log("object hit: " + hit1.collider.transform.parent.name);
                            GameObject hit_go = hit1.collider.gameObject;
                            hit_bar_bounds = hit_go.GetComponent<MeshFilter>().mesh.bounds;

                            Vector3 global_center_hitGo = hit_go.transform.TransformPoint(hit_bar_bounds.center);

                            Vector3 bottom_global_point = global_center_hitGo - Vector3.up * Vector3.Dot(Vector3.up, hit_bar_bounds.size) / 2;

                            float dist = Mathf.Abs(origin.y - bottom_global_point.y);
                            //Vector3 bottom_point_hitGo=

                            scale_length = cut_length - dist;


                        }

                        //checking if the sum length exceeds building length
                        if (sum_cut_length + cut_length > building_height && cut_length >= remaining_length)
                        {
                            //if it exceeded , we check for remaining length is non zero 
                            if ((sum_cut_length + remaining_length <= building_height) && remaining_length != 0)
                            {

                                scale_length = remaining_length;

                            }
                            else
                            {
                                build_section = false;
                                break;

                            }

                        }

                        GameObject vertical_section = new GameObject("Vertical_Section_" + k);
                        vertical_section.transform.parent = VerticalBar_Parent.transform;
                        for (int i = 0; i < no_of_v_barA; i++)
                        {
                            #region Instantiation and positioning
                            //GameObject vertical = new GameObject("vertical_"  +k + "_" + i);
                            //vertical.transform.parent = VerticalBar_Parent.transform;

                            GameObject vertical_bar = Instantiate(vertical_Prefab, vertical_section.transform);
                            vertical_bar.transform.position = new Vector3(verticalPoleDistance, sum_cut_length, 0);
                            vertical_bar.transform.localScale = new Vector3(1, scale_length, 1);

                            if (vertical_bar.transform.GetChild(0).gameObject.GetComponent<BoxCollider>() == null)
                                vertical_bar.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            vertical_bar.name = vertical_part_name + "_Side_A_" + k + "_" + i;

                            //vertical.transform.parent = vertical_section.transform;
                            #endregion

                            #region chracteristics
                            //ataching Characteristics script
                            Characteristics characteristics_script = vertical_bar.AddComponent<Characteristics>();

                            Bounds vertical_bar_bounds = vertical_bar.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                            string ear_bar_name = "ak - 34a";
                            bool ear = false;
                            float v_bar_width;
                            if (vertical_part_id == ear_bar_name)
                            {
                                ear = true;
                                float ear_width = 20f;

                                //as we take distance from bar end to ber end , and not considering the ear
                                v_bar_width = vertical_bar_bounds.size.x - ear_width;

                                if (i == (no_of_v_barA - 2))
                                {
                                    //for last bar we offset by 20 to move ear inwards and later rotate it about center
                                    v_bar_width = v_bar_width - ear_width;
                                }
                            }
                            else
                            {
                                v_bar_width = vertical_bar_bounds.size.x;
                            }


                            float vertical_bar_width_offset = v_bar_width;
                            //Assigning Frame Prefab :eg:ak - 288
                            characteristics_script.part_name_id = vertical_Prefab.name;// vertical_bar.name;// vertical_part_name;//vertical_Prefab.name;

                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                            if (k == 0)
                            {
                                verticals_order.Add(vertical_bar, i);
                            }

                            characteristics_script.part_type = part_type_enum.vertical.ToString();

                            if (vertical_bar.transform.GetChild(0) != null)
                            {
                                vertical_bar.transform.GetChild(0).gameObject.layer = divider_layer;
                            }
                            #endregion
                            // for first and last bars we add layer mask
                            if (i == 0 || i == (no_of_v_barA - 1))
                            {
                                //add layer to V-bar

                                vertical_bar.layer = end_V_Bar;
                                if (vertical_bar.transform.GetChild(0) != null)
                                {
                                    vertical_bar.transform.GetChild(0).gameObject.layer = end_V_Bar;
                                }
                            }
                            //as spaces will be 1 less than the vertical bars
                            if (i < no_of_v_barA - 1)
                            {//verticalPoleDistance += building_width;
                             //as we are moving from origin moving distance of each by  bar width
                                verticalPoleDistance += float.Parse(side_A_vpole_distances_Array[i]);
                                verticalPoleDistance += vertical_bar_width_offset;
                            }

                            if (side_A_vpole_EAR_direction_Array[i] == "2" && vertical_part_id == "ak - 34a")
                            {
                                //to rotate the bar about its center which makes its ear face inwards
                                rotate_around_center(vertical_bar, new Vector3(0, 0, 180));
                            }
                        }
                        sum_cut_length = sum_cut_length + scale_length;
                        remaining_length = remaining_length - scale_length;

                        //list of section and the height
                        section_name_height_list.Add(k, sum_cut_length);

                        k++;
                    }

                    Vector3 horizontal_bar_scaling_direction = Vector3.right;

                    forward_offset_hbar = 0;

                    //taking direction first and last vertical bar
                    if (verticals_order.Keys.First() != null && verticals_order.Keys.Last() != null)
                    {

                        //this part is to find the direction of scaling
                        GameObject first_V_bar = verticals_order.Keys.First();

                        GameObject last_V_bar = verticals_order.Keys.Last();

                        Bounds first_V_bar_bound = first_V_bar.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                        Bounds last_V_bar_bound = last_V_bar.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                        /***************Forward movement offset**************/

                        //forward_offset_hbar = first_V_bar_bound.size.z;
                        //we take direction of scaling by 
                        horizontal_bar_scaling_direction = first_V_bar.transform.TransformPoint(first_V_bar_bound.center) - last_V_bar.transform.TransformPoint(last_V_bar_bound.center);


                    }


                    if (temporary_first_vBar_line_clip_List != null && temporary_first_vBar_line_clip_List.Count > 0)
                    {
                        //Accessory_Parent = new GameObject("Accessory_Parent");

                        //Accessory_Parent.transform.parent = Louver_Model.transform;



                        GameObject first_V_bar = verticals_order.Keys.First();

                        //foreach(GameObject v_bar in verticals_order.Keys )
                        for (int n = 0; n < verticals_order.Count; n++)
                        {


                            if (n > 0)
                            {
                                GameObject v_bar = verticals_order.ElementAt(n).Key;

                                float dist = Vector3.Distance(first_V_bar.transform.localPosition, v_bar.transform.localPosition);
                                if (Mathf.Floor((int)dist) > 0)
                                {
                                    foreach (GameObject clipElement in temporary_first_vBar_line_clip_List)
                                    {
                                        //Adding Clip as child of Accessory_Parent
                                        //clipElement.transform.parent = Accessory_Parent.transform;

                                        GameObject new_clip = Instantiate(clipElement, clipElement.transform.parent);

                                        new_clip.transform.Translate(new_clip.transform.right * dist);

                                        hori_parents.Add(new_clip.transform);
                                        ////Adding new_clip as child of Accessory_Parent



                                        Characteristics characteristics_script;
                                        if (new_clip.GetComponent<Characteristics>() != null)
                                        {
                                            characteristics_script = new_clip.GetComponent<Characteristics>();
                                            characteristics_script.part_name_id = clipElement.GetComponent<Characteristics>().part_name_id;
                                            //here part_name_id will be same as they are cloned game objects but the part_unique_id should be changed
                                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else
                                        {
                                            //here part_name_id will be same as they are cloned game objects but the part_unique_id should be changed
                                            characteristics_script = new_clip.AddComponent<Characteristics>();
                                            characteristics_script.part_name_id = clipElement.GetComponent<Characteristics>().part_name_id;
                                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }

                                        string clipName = clipElement.transform.name.ToString();
                                        //clipName.Remove(clipName.IndexOf("("), clipName.IndexOf(")"));
                                        int index = clipName.IndexOf("VB");
                                        string sub = clipName.Substring(0, index);


                                        new_clip.name = sub + $"VB{ n}";
                                    }
                                }
                            }
                        }
                    }

                    foreach (KeyValuePair<int, float> t in section_name_height_list)
                    {
                        GameObject horizontal_section = new GameObject("Horizontal_Section_" + t.Key);
                        horizontal_section.transform.parent = HorizontalBar_Parent.transform;
                        //if (HorizontalBar_Parent!=null)
                        //foreach(Transform hori_par in HorizontalBar_Parent.transform)
                        //{

                        GameObject clip_parent = null;
                        foreach (Transform h_bar in hori_parents)
                        {
                            if (h_bar.transform.position.y <= t.Value && !parented_hori.Contains(h_bar))
                            {
                                h_bar.transform.parent = horizontal_section.transform;
                                //hori_parents.Remove(h_bar);

                                if (h_bar.name.ToLower().Contains("clip"))
                                {
                                    if (GameObject.Find("Clip_Parent_section" + t.Key) == null)
                                    {
                                        clip_parent = new GameObject("Clip_Parent_section" + t.Key);
                                    }
                                    else
                                    {
                                        clip_parent = GameObject.Find("Clip_Parent_section" + t.Key);
                                    }
                                    clip_parent.transform.parent = horizontal_section.transform;

                                    if (clip_parent != null)
                                    {
                                        h_bar.transform.parent = clip_parent.transform;
                                    }
                                }

                                parented_hori.Add(h_bar);
                            }
                        }
                        //}
                    }
                    //foreach (Transform ch in HorizontalBar_Parent.transform)
                    //{
                    //    hori_parents.Add(ch);
                    //}

                }
            }

            if (element_shape_id == 0 && false) //i Shape
            {
                if (building_side == "A")
                {

                    for (int i = 0; i < no_of_v_barA; i++)
                    {
                        #region Instantiation and positioning
                        GameObject vertical = new GameObject("vertical" + i);
                        vertical.transform.parent = VerticalBar_Parent.transform;

                        GameObject vertical_bar = Instantiate(vertical_Prefab, vertical.transform);
                        vertical_bar.transform.localPosition = new Vector3(verticalPoleDistance, 0, 0);
                        vertical_bar.transform.localScale = new Vector3(1, building_height, 1);
                        vertical_bar.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        vertical_bar.name = vertical_part_name + "_Side_A_" + i;


                        #endregion

                        #region chracteristics
                        //ataching Characteristics script
                        Characteristics characteristics_script = vertical_bar.AddComponent<Characteristics>();

                        Bounds vertical_bar_bounds = vertical_bar.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                        string ear_bar_name = "ak - 34a";
                        bool ear = false;
                        float v_bar_width;
                        if (vertical_part_id == ear_bar_name)
                        {
                            ear = true;
                            float ear_width = 20f;

                            //as we take distance from bar end to ber end , and not considering the ear
                            v_bar_width = vertical_bar_bounds.size.x - ear_width;

                            if (i == (no_of_v_barA - 2))
                            {
                                //for last bar we offset by 20 to move ear inwards and later rotate it about center
                                v_bar_width = v_bar_width - ear_width;
                            }
                        }
                        else
                        {
                            v_bar_width = vertical_bar_bounds.size.x;
                        }


                        float vertical_bar_width_offset = v_bar_width;
                        //Assigning Frame Prefab :eg:ak - 288
                        characteristics_script.part_name_id = vertical_Prefab.name;// vertical_bar.name;// vertical_part_name;//vertical_Prefab.name;

                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        verticals_order.Add(vertical_bar, i);

                        characteristics_script.part_type = part_type_enum.vertical.ToString();

                        if (vertical_bar.transform.GetChild(0) != null)
                        {
                            vertical_bar.transform.GetChild(0).gameObject.layer = divider_layer;
                        }
                        #endregion
                        // for first and last bars we add layer mask
                        if (i == 0 || i == (no_of_v_barA - 1))
                        {
                            //add layer to V-bar

                            vertical_bar.layer = end_V_Bar;
                            if (vertical_bar.transform.GetChild(0) != null)
                            {
                                vertical_bar.transform.GetChild(0).gameObject.layer = end_V_Bar;
                            }
                        }
                        //as spaces will be 1 less than the vertical bars
                        if (i < no_of_v_barA - 1)
                        {//verticalPoleDistance += building_width;
                         //as we are moving from origin moving distance of each by  bar width
                            verticalPoleDistance += float.Parse(side_A_vpole_distances_Array[i]);
                            verticalPoleDistance += vertical_bar_width_offset;
                        }

                        if (side_A_vpole_EAR_direction_Array[i] == "2" && vertical_part_id == "ak - 34a")
                        {
                            //to rotate the bar about its center which makes its ear face inwards
                            rotate_around_center(vertical_bar, new Vector3(0, 0, 180));
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

                        /***************Forward movement offset**************/

                        //forward_offset_hbar = first_V_bar_bound.size.z;
                        //we take direction of scaling by 
                        horizontal_bar_scaling_direction = first_V_bar.transform.TransformPoint(first_V_bar_bound.center) - last_V_bar.transform.TransformPoint(last_V_bar_bound.center);


                    }

                    if (!String.IsNullOrEmpty(rafafa_type_part_setting._part_id)) //checking for hollow pergola
                    {

                        float scale = 1840;
                        scale = building_width;
                        //string horizontal_part_name = "ak - 18";
                        horizontal_part_name = rafafa_type_part_setting._part_id;
                        horizontal_Prefab = (GameObject)Resources.Load($"prefabs/{horizontal_part_name}", typeof(GameObject));

                        float h_bar_height = 0;
                        if (horizontal_Prefab == null)
                        {
                            horizontal_part_name = "ak - 18";

                            horizontal_Prefab = (GameObject)Resources.Load($"prefabs/{horizontal_part_name}", typeof(GameObject));
                        }


                        if (horizontal_Prefab != null)
                        {
                            //part_name = "ak - 18";

                            //Frame_Prefab =Instantiate(  (GameObject)Resources.Load($"prefabs/{part_name}", typeof(GameObject)), GameObject.Find("HorizontalBar_Parent").transform);

                            //Here we instatiate Dummy horizontal bar and find the offset nd destroy it 
                            GameObject Dummy_h_bar = Instantiate(horizontal_Prefab, HorizontalBar_Parent.transform);

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

                        float no_of_H_bars = Mathf.Floor(((building_height + rafafa_spacing) / (h_bar_height + rafafa_spacing)));
                        int IntNumOf_H_bar = Convert.ToInt32(no_of_H_bars);


                        //HORIZONTALS ALWAYS MIDDLE ALIGNED.

                        //The difference of actual building height and the building heigth till the h_brs are placed --gives "h_Bar_offset"

                        float h_Bar_offset = building_height - ((IntNumOf_H_bar * h_bar_height) + (rafafa_spacing * (IntNumOf_H_bar - 1)));//top

                        string alignment = (Enum.GetName(typeof(RafafaAlignment), int.Parse(rafafa_alignment_id)));
                        float space_h_bar = rafafa_spacing;// building_height / 2;+h_bar_width


                        bool clips_in_model = false;



                        float clip_spare_part_height = 0; // the height which goes inside another clip.
                        float clip_width = 0;
                        float clip_depth = 0;
                        float vBar_for_clip_width = 0;
                        float vBar_for_clips_move_behind_clips_value = 0;
                        float clip_height_full = 0;


                        List<GameObject> temporary_first_vBar_line_clip_List = null;


                        if (horizontal_part_name == "ak - 40")
                        {
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
                                    horizontal_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-72-40", typeof(GameObject));
                                }
                                else if (space_h_bar == 50)
                                {
                                    clip_name = "ak - 39";
                                    clip_height_full = 120.47f; //For space between 50, ak - 39

                                    //Load clip and horizontal combined prefab
                                    horizontal_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-39", typeof(GameObject));
                                }
                                else if (space_h_bar == 20)
                                {
                                    clip_name = "ak - 76";
                                    clip_height_full = 90.47f; // For space between 20, ak - 76

                                    //Load clip and horizontal combined prefab
                                    horizontal_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-76", typeof(GameObject));
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
                                no_of_H_bars = Mathf.Ceil((building_height - clip_spare_part_height) / clip_height_full);
                                IntNumOf_H_bar = Convert.ToInt32(no_of_H_bars);

                                h_Bar_offset = building_height - ((IntNumOf_H_bar * (clip_height_full - clip_spare_part_height)));
                                //h_Bar_offset = h_Bar_offset / 2;


                                temporary_first_vBar_line_clip_List = new List<GameObject>();


                                if (string.Equals(alignment, "MIDDLE", StringComparison.OrdinalIgnoreCase))
                                    h_Bar_offset = h_Bar_offset / 2;
                                else if (string.Equals(alignment, "BOTTOM", StringComparison.OrdinalIgnoreCase))
                                    h_Bar_offset = 0;
                                else if (string.Equals(alignment, "TOP", StringComparison.OrdinalIgnoreCase))
                                    h_Bar_offset = h_Bar_offset;
                                else
                                    h_Bar_offset = 0;

                                float Y_distance = h_Bar_offset;// h_bar_width;
                                                                //to calculate h_bar_width,forward_offset_hbar
                                if (horizontal_Prefab != null)
                                {
                                    //part_name = "ak - 18";

                                    //Frame_Prefab =Instantiate(  (GameObject)Resources.Load($"prefabs/{part_name}", typeof(GameObject)), GameObject.Find("HorizontalBar_Parent").transform);

                                    //Here we instatiate Dummy horizontal bar and find the offset nd destroy it 
                                    GameObject Dummy_h_bar = Instantiate(horizontal_Prefab, HorizontalBar_Parent.transform);

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


                                    GameObject horizontal = new GameObject("Horizontal" + j);
                                    horizontal.transform.parent = HorizontalBar_Parent.transform;

                                    horizontal_bar = Instantiate(horizontal_Prefab, horizontal.transform);
                                    //Bounds h_bar_bound = Calculate_b(horizontal_bar.transform);

                                    //if(horizontal_bar.GetComponentInChildren<MeshRenderer>()!=null)
                                    //horizontal_bar.GetComponentInChildren<MeshRenderer>().material= Resources.Load("Materials/Horizontal_material", typeof(Material)) as Material;

                                    //forward_offset_hbar = h_bar_bound.size.z;
                                    horizontal_bar.transform.localPosition = new Vector3(0, Y_distance, -forward_offset_hbar);//Vector3(h_bar_width, Y_distance, 0);

                                    //if its the combined prefab
                                    if (DB_script.combined_Clips_names.Contains(horizontal_Prefab.name))
                                    {

                                        GameObject clip = horizontal_bar.transform.GetChild(1).gameObject;

                                        //Bounds clip_bound = Calculate_b(clip.transform);

                                        //Bounds hbar_bound = Calculate_b(horizontal_bar.transform.GetChild(0));

                                        //forward_offset_hbar = hbar_bound.size.z + clip_bound.size.z;

                                        //horizontal_bar.transform.localPosition = new Vector3(0, Y_distance, -forward_offset_hbar);//-1.72f
                                        //Vector3 clip_pos = clip.transform.position;

                                        //clip.transform.parent = HorizontalBar_Parent.transform;

                                        GameObject clip_parent = new GameObject("clips" + j);
                                        clip_parent.transform.parent = horizontal.transform;

                                        clip.transform.parent = clip_parent.transform;

                                        temporary_first_vBar_line_clip_List.Add(clip);


                                        //clip.transform.position = clip_pos;
                                        //clip.transform.parent = null;

                                        string clip_prefab_name = clip_name;// clip.name;

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
                                        if (horizontal_Prefab.name.Contains("ak - 40-72-40"))
                                        {
                                            GameObject second_curve_h_bar = horizontal_bar.transform.Find("ak - 40").gameObject;

                                            string sec_clip_prefab_name = second_curve_h_bar.name;

                                            //second_curve_h_bar.transform.SetParent(HorizontalBar_Parent.transform, true);
                                            second_curve_h_bar.transform.SetParent(horizontal.transform, true);


                                            second_curve_h_bar.transform.localScale = new Vector3(scale, 1, 1);

                                            second_curve_h_bar.name = horizontal_part_name + "_2" + "_Side_A_" + j;

                                            Characteristics second_hBar_chars_script = null;

                                            if (second_curve_h_bar.GetComponent<Characteristics>() != null)
                                            {
                                                second_hBar_chars_script = second_curve_h_bar.GetComponent<Characteristics>();

                                                //second_hBar_chars_script.part_name_id = second_curve_h_bar.name;// horizontal_part_name;

                                                //second_hBar_chars_script.part_unique_id = Guid.NewGuid().ToString();

                                                //second_hBar_chars_script.part_type = part_type_enum.horizontal.ToString();
                                            }
                                            else
                                            {
                                                second_hBar_chars_script = second_curve_h_bar.AddComponent<Characteristics>();

                                            }

                                            second_hBar_chars_script.part_name_id = sec_clip_prefab_name;// second_curve_h_bar.name;// horizontal_part_name;

                                            second_hBar_chars_script.part_type = part_type_enum.horizontal.ToString();

                                            second_hBar_chars_script.part_unique_id = Guid.NewGuid().ToString();
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
                                    characteristics_script.part_name_id = horizontal_part_name;// clip_name;// horizontal_bar.name;// horizontal_part_name;//horizontal_Prefab.name;

                                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                                    characteristics_script.part_type = "horizontal";
                                    //here we add space and horizontal bar width , which will be new position for next bar
                                    //horizontal_bar.transform.Translate(-Vector3.forward * forward_offset_hbar);

                                    #endregion
                                    //**************as there are 2 horizontal bars inside the clip in this prefabe we mutiply 2* , while adding offset*******************//
                                    if (horizontal_Prefab.name.Contains("ak - 40-72-40") || horizontal_Prefab.name.Contains("ak - 40-72"))
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

                            }
                        }
                        else
                        {

                            //checking if the horizontal part is with clips
                            //if (DB_script.Clips.Contains(horizontal_part_name))
                            //{
                            //    //rotate_around_center(horizontal_bar, new Vector3(0, 180, 0));


                            //    clips_in_model = true;

                            //    //ADD CLIPS
                            //    string clip_name = "";

                            //    if (space_h_bar == -10)
                            //    {
                            //        clip_name = "ak - 72";
                            //        clip_height_full = 120.47f; //For space between 50, ak - 39

                            //        //Load clip and horizontal combined prefab
                            //        horizontal_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-72-40", typeof(GameObject));
                            //    }
                            //    else if (space_h_bar == 50)
                            //    {
                            //        clip_name = "ak - 39";
                            //        clip_height_full = 120.47f; //For space between 50, ak - 39

                            //        //Load clip and horizontal combined prefab
                            //        horizontal_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-39", typeof(GameObject));
                            //    }
                            //    else if (space_h_bar == 20)
                            //    {
                            //        clip_name = "ak - 76";
                            //        clip_height_full = 90.47f; // For space between 20, ak - 76

                            //        //Load clip and horizontal combined prefab
                            //        horizontal_Prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/ak - 40-76", typeof(GameObject));
                            //    }
                            //    else
                            //    {
                            //        throw new Exception($"Invalid rafafa spacing. Valid spacing is -10, 20, 50.");
                            //    }

                            //    clip_spare_part_height = 3.5f; // the height which goes inside another clip.
                            //    clip_width = 40.0f;
                            //    clip_depth = 34.08f;
                            //    vBar_for_clip_width = 40f;
                            //    vBar_for_clips_move_behind_clips_value = -43.88f;


                            //    float clip_height = Mathf.Ceil((clip_height_full - clip_spare_part_height));
                            //    no_of_H_bars = Mathf.Ceil((building_height - clip_spare_part_height) / clip_height_full);
                            //    IntNumOf_H_bar = Convert.ToInt32(no_of_H_bars);

                            //    h_Bar_offset = building_height - ((IntNumOf_H_bar * (clip_height_full - clip_spare_part_height)));
                            //    //h_Bar_offset = h_Bar_offset / 2;


                            //    temporary_first_vBar_line_clip_List = new List<GameObject>();
                            //}

                            //if (string.Equals(alignment, "MIDDLE", StringComparison.OrdinalIgnoreCase))
                            //    h_Bar_offset = h_Bar_offset / 2;
                            //else if (string.Equals(alignment, "BOTTOM", StringComparison.OrdinalIgnoreCase))
                            //    h_Bar_offset = 0;
                            //else if (string.Equals(alignment, "TOP", StringComparison.OrdinalIgnoreCase))
                            //    h_Bar_offset = h_Bar_offset;
                            //else
                            //    h_Bar_offset = 0;

                            float Y_distance = h_Bar_offset;// h_bar_width;
                                                            //to calculate h_bar_width,forward_offset_hbar
                            if (horizontal_Prefab != null)
                            {
                                //part_name = "ak - 18";

                                //Frame_Prefab =Instantiate(  (GameObject)Resources.Load($"prefabs/{part_name}", typeof(GameObject)), GameObject.Find("HorizontalBar_Parent").transform);

                                //Here we instatiate Dummy horizontal bar and find the offset nd destroy it 
                                GameObject Dummy_h_bar = Instantiate(horizontal_Prefab, HorizontalBar_Parent.transform);

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

                                GameObject horizontal = new GameObject("Horizontal" + j);
                                horizontal.transform.parent = HorizontalBar_Parent.transform;

                                horizontal_bar = Instantiate(horizontal_Prefab, horizontal.transform);
                                //Bounds h_bar_bound = Calculate_b(horizontal_bar.transform);

                                //if(horizontal_bar.GetComponentInChildren<MeshRenderer>()!=null)
                                //horizontal_bar.GetComponentInChildren<MeshRenderer>().material= Resources.Load("Materials/Horizontal_material", typeof(Material)) as Material;

                                //forward_offset_hbar = h_bar_bound.size.z;
                                horizontal_bar.transform.localPosition = new Vector3(0, Y_distance, -forward_offset_hbar);//Vector3(h_bar_width, Y_distance, 0);

                                //if its the combined prefab
                                #region clip arr
                                //if (DB_script.combined_Clips_names.Contains(horizontal_Prefab.name))
                                //{

                                //    GameObject clip = horizontal_bar.transform.GetChild(1).gameObject;

                                //    //Bounds clip_bound = Calculate_b(clip.transform);

                                //    //Bounds hbar_bound = Calculate_b(horizontal_bar.transform.GetChild(0));

                                //    //forward_offset_hbar = hbar_bound.size.z + clip_bound.size.z;

                                //    //horizontal_bar.transform.localPosition = new Vector3(0, Y_distance, -forward_offset_hbar);//-1.72f
                                //    //Vector3 clip_pos = clip.transform.position;

                                //    clip.transform.parent = HorizontalBar_Parent.transform;

                                //    temporary_first_vBar_line_clip_List.Add(clip);


                                //    //clip.transform.position = clip_pos;
                                //    //clip.transform.parent = null;

                                //    string clip_prefab_name = clip.name;

                                //    //asigning the vertical bar no at the end as - VB0
                                //    clip.name = clip_prefab_name + "_Side_A_" + j + "_VB0";
                                //    #region chracteristics
                                //    Characteristics clip_chars_script;

                                //    if (clip.GetComponent<Characteristics>() != null)
                                //    {
                                //        clip_chars_script = clip.GetComponent<Characteristics>();

                                //        //****** adiding old prafab name and unique id for part *****//
                                //        clip_chars_script.part_name_id = clip_prefab_name;
                                //        clip_chars_script.part_unique_id = Guid.NewGuid().ToString();

                                //        clip_chars_script.part_type = part_type_enum.accessory.ToString();
                                //    }

                                //    else
                                //    {
                                //        clip_chars_script = clip.AddComponent<Characteristics>();

                                //        //****** adiding old prafab name and unique id for part *****//
                                //        clip_chars_script.part_name_id = clip_prefab_name;
                                //        clip_chars_script.part_unique_id = Guid.NewGuid().ToString();

                                //        clip_chars_script.part_type = part_type_enum.accessory.ToString();

                                //    }

                                //    #endregion

                                //    //*************************Here the prefab contains (Parent ) ak - 40 ,children-> (ak -72, ak - 40)**********************************//
                                //    if (horizontal_Prefab.name.Contains("ak - 40-72-40"))
                                //    {
                                //        GameObject second_curve_h_bar = horizontal_bar.transform.Find("ak - 40").gameObject;



                                //        second_curve_h_bar.transform.SetParent(HorizontalBar_Parent.transform, true);


                                //        second_curve_h_bar.transform.localScale = new Vector3(scale, 1, 1);

                                //        second_curve_h_bar.name = horizontal_part_name + "_2" + "_Side_A_" + j;

                                //        Characteristics second_hBar_chars_script = null;

                                //        if (second_curve_h_bar.GetComponent<Characteristics>() != null)
                                //        {
                                //            second_hBar_chars_script = second_curve_h_bar.GetComponent<Characteristics>();

                                //            second_hBar_chars_script.part_name_id = horizontal_part_name;

                                //            second_hBar_chars_script.part_unique_id = Guid.NewGuid().ToString();

                                //            second_hBar_chars_script.part_type = part_type_enum.horizontal.ToString();
                                //        }
                                //        else
                                //        {
                                //            second_hBar_chars_script = second_curve_h_bar.AddComponent<Characteristics>();

                                //            second_hBar_chars_script.part_name_id = horizontal_part_name;

                                //            second_hBar_chars_script.part_type = part_type_enum.horizontal.ToString();

                                //            second_hBar_chars_script.part_unique_id = Guid.NewGuid().ToString();
                                //        }

                                //        //if(second_curve_h_bar.transform.GetComponentInChildren<Renderer>()!=null)
                                //        //second_curve_h_bar.transform.GetComponentInChildren<Renderer>().material = Resources.Load("Materials/Horizontal_material", typeof(Material)) as Material;
                                //    }
                                //}
                                #endregion
                                horizontal_bar.transform.localScale = new Vector3(scale, 1, 1);
                                horizontal_bar.name = horizontal_part_name + "_Side_A_" + j;


                                #region chracteristics

                                //ataching Characteristics script
                                Characteristics characteristics_script = horizontal_bar.AddComponent<Characteristics>();

                                //Assigning Frame Prefab :eg:ak - 288
                                characteristics_script.part_name_id = horizontal_Prefab.name;// horizontal_bar.name;// horizontal_part_name;//horizontal_Prefab.name;

                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                                characteristics_script.part_type = "horizontal";
                                //here we add space and horizontal bar width , which will be new position for next bar
                                //horizontal_bar.transform.Translate(-Vector3.forward * forward_offset_hbar);

                                #endregion
                                //**************as there are 2 horizontal bars inside the clip in this prefabe we mutiply 2* , while adding offset*******************//
                                //if (horizontal_Prefab.name.Contains("ak - 40-72-40") || horizontal_Prefab.name.Contains("ak - 40-72"))
                                //    Y_distance += 2 * (space_h_bar + h_bar_height);
                                //else
                                Y_distance += (space_h_bar + h_bar_height);

                                //if there are clips then the clip_spare_part_height shoukd be decreased after 1st clip to all other clips
                                //if (clips_in_model)
                                //{

                                //    //Y_distance=h_bar_width;
                                //    Y_distance -= clip_spare_part_height;

                                //    //Y_distance=clip_height_full- clip_spare_part_height;
                                //}

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

                            //if (temporary_first_vBar_line_clip_List != null && temporary_first_vBar_line_clip_List.Count > 0)
                            //{
                            //    Accessory_Parent = new GameObject("Accessory_Parent");

                            //    Accessory_Parent.transform.parent = Louver_Model.transform;



                            //    GameObject first_V_bar = verticals_order.Keys.First();

                            //    //foreach(GameObject v_bar in verticals_order.Keys )
                            //    for (int k = 0; k < verticals_order.Count; k++)
                            //    {


                            //        if (k > 0)
                            //        {
                            //            GameObject v_bar = verticals_order.ElementAt(k).Key;

                            //            float dist = Vector3.Distance(first_V_bar.transform.localPosition, v_bar.transform.localPosition);
                            //            if (Mathf.Floor((int)dist) > 0)
                            //            {
                            //                foreach (GameObject clipElement in temporary_first_vBar_line_clip_List)
                            //                {
                            //                    //Adding Clip as child of Accessory_Parent
                            //                    clipElement.transform.parent = Accessory_Parent.transform;

                            //                    GameObject new_clip = Instantiate(clipElement, Accessory_Parent.transform);

                            //                    new_clip.transform.Translate(new_clip.transform.right * dist);


                            //                    ////Adding new_clip as child of Accessory_Parent



                            //                    Characteristics characteristics_script;
                            //                    if (new_clip.GetComponent<Characteristics>() != null)
                            //                    {
                            //                        characteristics_script = new_clip.GetComponent<Characteristics>();

                            //                        //here part_name_id will be same as they are cloned game objects but the part_unique_id should be changed
                            //                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                            //                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                            //                    }
                            //                    else
                            //                    {
                            //                        //here part_name_id will be same as they are cloned game objects but the part_unique_id should be changed
                            //                        characteristics_script = new_clip.AddComponent<Characteristics>();

                            //                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                            //                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                            //                    }

                            //                    string clipName = clipElement.transform.name.ToString();
                            //                    //clipName.Remove(clipName.IndexOf("("), clipName.IndexOf(")"));
                            //                    int index = clipName.IndexOf("VB");
                            //                    string sub = clipName.Substring(0, index);


                            //                    new_clip.name = sub + $"VB{ k}";
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                        }

                        if (temporary_first_vBar_line_clip_List != null && temporary_first_vBar_line_clip_List.Count > 0)
                        {
                            //Accessory_Parent = new GameObject("Accessory_Parent");

                            //Accessory_Parent.transform.parent = Louver_Model.transform;



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
                                            //clipElement.transform.parent = Accessory_Parent.transform;

                                            GameObject new_clip = Instantiate(clipElement, clipElement.transform.parent);

                                            new_clip.transform.Translate(new_clip.transform.right * dist);


                                            ////Adding new_clip as child of Accessory_Parent



                                            Characteristics characteristics_script;
                                            if (new_clip.GetComponent<Characteristics>() != null)
                                            {
                                                characteristics_script = new_clip.GetComponent<Characteristics>();
                                                characteristics_script.part_name_id = clipElement.GetComponent<Characteristics>().part_name_id;
                                                //here part_name_id will be same as they are cloned game objects but the part_unique_id should be changed
                                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                                                characteristics_script.part_type = part_type_enum.accessory.ToString();
                                            }
                                            else
                                            {
                                                //here part_name_id will be same as they are cloned game objects but the part_unique_id should be changed
                                                characteristics_script = new_clip.AddComponent<Characteristics>();
                                                characteristics_script.part_name_id = clipElement.GetComponent<Characteristics>().part_name_id;
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

                    }
                }
            }
            //completed = true;



            try
            {
                apply_material_Louvers();
                //Save_to_db_Louvers();
                //camera_resizing_louvers();
            }
            catch (Exception ex)
            {

                Debug.Log("Saving Lovers DB:" + ex);
            }
            //return 5;
        }
        bool wait = true;
        IEnumerator delay_function()
        {

            yield return new WaitForSeconds(2);

            wait = false;
        }

        class PartSettings_Header_id
        {
            string _connectionstring = "";

            public string _part_id;
            public string _part_type;
            public float _part_height;
            public float _part_width;
            public float _part_depth;

            public PartSettings_Header_id(string id, string connectionString)
            {
                _connectionstring = connectionString;
                DataTable dtPartSettings = get_part_measurement(id);
                if (dtPartSettings.Rows.Count <= 0) throw new Exception($"Part {id} measurement setting data not found.");

                _part_id = dtPartSettings.Rows[0]["part_id"].ToString();
                _part_type = dtPartSettings.Rows[0]["part_type"].ToString();
                _part_height = Convert.ToSingle(dtPartSettings.Rows[0]["part_height"]);
                _part_width = Convert.ToSingle(dtPartSettings.Rows[0]["part_width"]);
                _part_depth = Convert.ToSingle(dtPartSettings.Rows[0]["part_depth"]);
            }

            private DataTable get_part_measurement(string id)
            {
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {
                    SqlDataAdapter daPartSetting = new SqlDataAdapter();
                    con.Open();
                    string qry = "";

                    qry = $"select part_id, part_type, part_height, part_width, part_depth from tbl_part_setting_header where id = '{id}'";
                    SqlCommand sql = new SqlCommand(qry, con);
                    daPartSetting.SelectCommand = sql;
                    DataTable dtPartSettings = new DataTable();
                    daPartSetting.Fill(dtPartSettings);
                    con.Close();
                    return dtPartSettings;
                }
            }
        }

        class PartSettings_Header_Vertical_join
        {
            string _connectionstring = "";

            public string _pole_type_id;

            public float _pole_max_length;


            public PartSettings_Header_Vertical_join(string id, string connectionString)
            {
                _connectionstring = connectionString;
                DataTable dtPartSettings = get_part_measurement(id);
                if (dtPartSettings.Rows.Count <= 0) throw new Exception($"Part {id} measurement setting data not found.");

                _pole_type_id = dtPartSettings.Rows[0]["pole_type_id"].ToString();

                _pole_max_length = Convert.ToSingle(dtPartSettings.Rows[0]["pole_max_length"]);

            }

            private DataTable get_part_measurement(string id)
            {
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {
                    SqlDataAdapter daPartSetting = new SqlDataAdapter();
                    con.Open();
                    string qry = "";

                    qry = $"select v.id, v.pole_type_id, v.pole_max_length from tbl_part_setting_verticals v inner join tbl_part_setting_header h on v.id = h.id and h.id = '{id}'";
                    SqlCommand sql = new SqlCommand(qry, con);
                    daPartSetting.SelectCommand = sql;
                    DataTable dtPartSettings = new DataTable();
                    daPartSetting.Fill(dtPartSettings);
                    con.Close();
                    return dtPartSettings;
                }
            }
        }
        class General_setting_header
        {
            string _connectionstring = "";

            public float _stock_quantity;

            public float _max_paint_height;


            private DataTable get_part_measurement(string id)
            {
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {
                    SqlDataAdapter daPartSetting = new SqlDataAdapter();
                    con.Open();
                    string qry = "";

                    qry = $"select v.id, v.pole_type_id, v.pole_max_length from tbl_part_setting_verticals v inner join tbl_part_setting_header h on v.id = h.id and h.id = '{id}'";
                    SqlCommand sql = new SqlCommand(qry, con);
                    daPartSetting.SelectCommand = sql;
                    DataTable dtPartSettings = new DataTable();
                    daPartSetting.Fill(dtPartSettings);
                    con.Close();
                    return dtPartSettings;
                }
            }


        }
        private void Save_to_db_Louvers()
        {
            string query = "";//= "Insert Into dbo.louvers_points (unity_heirarchy_name,unity_heirarchy_level,pos_x,pos_y,pos_z,rot_x,rot_y,rot_z,scale_x,scale_y,scale_z,part_name_id) Values (@unity_heirarchy_name,@unity_heirarchy_level,@pos_x,@pos_y,@pos_z,@rot_x,@rot_y,@rot_z,@scale_x,@scale_y,@scale_z,@part_name_id)";

            connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=alukal;User ID=sa;Password=Password@1234";
            //connectionString = @"Data Source=localhost\SQL_LEARNING;Initial Catalog=alukal;User ID=sa;Password=Password@1234";
            SqlTransaction transaction = null;
            sqlCnn = new SqlConnection(connectionString);
            sqlCnn.Open();//to open the connection
            bool BlnTransactionBegin = false;

            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            transaction = sqlCnn.BeginTransaction();

            SqlCommand command = sqlCnn.CreateCommand();
            command.Connection = sqlCnn;
            command.Transaction = transaction;
            BlnTransactionBegin = true;
            try
            {
                string project_unique_id = DB_script.project_unique_id, building_unique_id = DB_script.building_unique_id, element_unique_id = DB_script.element_unique_id;

                //try
                //{
                //    query = $"DELETE FROM tbl_element_louver_points WHERE project_unique_id = '{project_unique_id}' AND building_unique_id = '{building_unique_id}' AND element_unique_id = '{element_unique_id}'";
                //    command.CommandText = query;
                //    command.ExecuteNonQuery();

                //}
                //catch (Exception ex)
                //{
                //    Debug.Log("Deletion query" + ex);
                //    //throw;
                //}

                if (pb_list == null)
                    pb_list = new List<ProBuilderMesh>();
                foreach (GameObject go in allObjects)
                {
                    if (!NotAllowed_in_db.Contains(go.name))
                    {
                        Characteristics characteristics_script;
                        if (go.GetComponent<Characteristics>() == null)
                        {
                            characteristics_script = go.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = go.name;
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_type = part_type_enum.none.ToString();
                        }
                        else
                        {
                            characteristics_script = go.GetComponent<Characteristics>();
                        }
                        string unity_heirarchy_name = "", part_name_id = "", section_name = "", part_unique_id = "", part_type = "", part_group = "";
                        int unity_heirarchy_level = -1, instantiate = 0;
                        string left_end_cut_angle = "", right_end_cut_angle = "", icon_filename = "";

                        double pos_x, pos_y, pos_z, rot_x, rot_y, rot_z, scale_x, scale_y, scale_z;

                        pos_x = go.transform.localPosition.x;
                        pos_y = go.transform.localPosition.y;
                        pos_z = go.transform.localPosition.z;

                        rot_x = go.transform.localRotation.eulerAngles.x;
                        rot_y = go.transform.localRotation.eulerAngles.y;
                        rot_z = go.transform.localRotation.eulerAngles.z;


                        scale_x = go.transform.localScale.x;
                        scale_y = go.transform.localScale.y;
                        scale_z = go.transform.localScale.z;

                        part_name_id = characteristics_script.part_name_id;
                        section_name = Characteristics.section_name;
                        part_unique_id = characteristics_script.part_unique_id;
                        part_type = characteristics_script.part_type;
                        part_group = characteristics_script.part_group;
                        left_end_cut_angle = characteristics_script.left_end_cut_angle;
                        right_end_cut_angle = characteristics_script.right_end_cut_angle;
                        icon_filename = characteristics_script.icon_filename;
                        instantiate = characteristics_script.instantiate;

                        //instantiate = characteristics_script.instantiate;

                        if (go.transform.parent == null) //top level in hierarchy - no parent
                        {
                            unity_heirarchy_name = go.name;
                            unity_heirarchy_level = 0;
                        }
                        else if (go.transform.parent != null && go.transform.parent.parent == null)
                        {
                            // 1 level in hierarchy - 1 parent
                            unity_heirarchy_name = go.transform.parent.name + "/" + go.name;
                            unity_heirarchy_level = 1;
                        }
                        else if (go.transform.parent.parent != null && go.transform.parent.parent.parent == null)
                        {
                            // 2 level in hierarchy - 2 parent
                            unity_heirarchy_name = go.transform.parent.parent.name + "/" + go.transform.parent.name + "/" + go.name;
                            unity_heirarchy_level = 2;
                        }
                        else if (go.transform.parent.parent.parent != null && go.transform.parent.parent.parent.parent == null)
                        {
                            // 3 level in hierarchy - 3 parent
                            unity_heirarchy_name = go.transform.parent.parent.parent.name + "/" + go.transform.parent.parent.name + "/" + go.transform.parent.name + "/" + go.name;
                            unity_heirarchy_level = 3;
                        }
                        else if (go.transform.parent.parent.parent.parent != null && go.transform.parent.parent.parent.parent.parent == null)
                        {
                            // 4 level in hierarchy - 4 parent
                            unity_heirarchy_name = go.transform.parent.parent.parent.parent.name + "/" + go.transform.parent.parent.parent.name + "/" + go.transform.parent.parent.name + "/" + go.transform.parent.name + "/" + go.name;
                            unity_heirarchy_level = 4;
                        }
                        else if (go.transform.parent.parent.parent.parent.parent != null && go.transform.parent.parent.parent.parent.parent.parent == null)
                        {
                            // 5 level in hierarchy - 4 parent
                            unity_heirarchy_name = go.transform.parent.parent.parent.parent.parent.name + "/" + go.transform.parent.parent.parent.parent.name + "/" + go.transform.parent.parent.parent.name + "/" + go.transform.parent.parent.name + "/" + go.transform.parent.name + "/" + go.name;
                            unity_heirarchy_level = 5;
                        }

                        if (!string.IsNullOrEmpty(unity_heirarchy_name) && !string.IsNullOrEmpty(part_name_id) && unity_heirarchy_level != -1)
                        {


                            query = $"Insert Into tbl_element_louver_points (unity_heirarchy_name,unity_heirarchy_level,pos_x,pos_y,pos_z,rot_x,rot_y,rot_z,scale_x,scale_y,scale_z,part_name_id,project_unique_id,building_unique_id,element_unique_id,section_name,part_unique_id,part_type,part_group,left_end_cut_angle,right_end_cut_angle,icon_filename,instantiate) Values ('{unity_heirarchy_name}',{unity_heirarchy_level},{pos_x},{pos_y},{pos_z},{rot_x},{rot_y},{rot_z},{scale_x},{scale_y},{scale_z},'{part_name_id}','{project_unique_id}','{building_unique_id}','{element_unique_id}','{section_name}','{part_unique_id}', '{part_type}','{part_group} ','{left_end_cut_angle}','{right_end_cut_angle}','{icon_filename}','{instantiate}')";
                            command.CommandText = query;
                            command.ExecuteNonQuery();
                        }

                    }

                    if (go.gameObject.GetComponent<ProBuilderMesh>() != null)
                    {
                        if (!pb_list.Contains(go.gameObject.GetComponent<ProBuilderMesh>()))
                            pb_list.Add(go.gameObject.GetComponent<ProBuilderMesh>());
                    }
                }

                transaction.Commit();
            }
            catch (SqlException ex)
            {
                Debug.LogError(ex.Message);
                if (BlnTransactionBegin) transaction.Rollback();
            }
            finally
            {
                if (sqlCnn.State == ConnectionState.Open) sqlCnn.Close();
            }
        }

        public static void apply_material_Louvers()
        {
            #region Applying materails to Game objects
            if (VerticalBar_Parent != null)
                foreach (Transform v_Bar in VerticalBar_Parent.transform)
                {

                    foreach (Transform vertical_bar in v_Bar)
                    {

                        if (vertical_bar.transform.GetChild(0) != null)
                            vertical_bar.transform.GetChild(0).gameObject.layer = divider_layer;

                        var mats = vertical_bar.GetComponentInChildren<Renderer>().materials;

                        foreach (Material m in mats)
                        {
                            m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                            m.shader = Shader.Find("Unlit/Color");
                            Color color;


                            if (ColorUtility.TryParseHtmlString("#A8A8A8", out color))
                            { m.color = color; }





                        }
                        //if (vertical_bar.GetComponentInChildren<MeshRenderer>() != null)
                        //    vertical_bar.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/Vertical_material", typeof(Material)) as Material;
                    }
                }

            if (HorizontalBar_Parent != null)
                foreach (Transform h_bar in HorizontalBar_Parent.transform)
                {
                    foreach (Transform horizontal_bar in h_bar)
                    {
                        if (horizontal_bar.GetComponentInChildren<MeshRenderer>() != null)
                        {
                            var mats = horizontal_bar.GetComponentInChildren<Renderer>().materials;
                            foreach (Material m in mats)
                            {
                                m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                m.shader = Shader.Find("Unlit/Color");

                                Color color;

                                //if (ColorUtility.TryParseHtmlString("#69686A", out color))
                                //{ m.color = color; }

                                if (horizontal_bar.name.Contains("clips"))
                                {
                                    foreach (Transform clips in horizontal_bar)
                                    {
                                        mats = clips.GetComponentInChildren<Renderer>().materials;
                                        foreach (Material m2 in mats)
                                        {
                                            m2.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                            m2.shader = Shader.Find("Unlit/Color");
                                            if (ColorUtility.TryParseHtmlString("#484848", out color))
                                            { m2.color = color; }
                                        }
                                    }
                                }
                                else
                                {
                                    if (ColorUtility.TryParseHtmlString("#69686A", out color))
                                    { m.color = color; }

                                }
                                //else
                                //{
                                //    if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                //    { m.color = color; }


                                //}
                            }
                            //horizontal_bar.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/Horizontal_material", typeof(Material)) as Material;
                        }
                    }
                }

            //if (Accessory_Parent != null)
            //    foreach (Transform accs in Accessory_Parent.transform)
            //    {
            //        if (accs.GetComponentInChildren<MeshRenderer>() != null)
            //            accs.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/Accessory_material", typeof(Material)) as Material;
            //    }
            #endregion
        }

        public static async void camera_resizing_louvers()
        {
            #region orthographic_camera size,near_clip,far_clip region setting
            if (GameObject.Find("Louver_Model") != null && GameObject.Find("HorizontalBar_Parent") != null && GameObject.Find("HorizontalBar_Parent") != null)
            {

                Louver_Model = GameObject.Find("Louver_Model");
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



                //if the Model was generated for the first time then we place the HorizontalBar_Parent & VerticalBar_Parent at the combined center
                HorizontalBar_Parent.transform.position = new Vector3(-combined_bounds.center.x, -combined_bounds.center.y, -combined_bounds.center.z);//to place parent at the center 

                VerticalBar_Parent.transform.position = new Vector3(-combined_bounds.center.x, -combined_bounds.center.y, -combined_bounds.center.z);//to place parent at the center

                if (Accessory_Parent != null)
                {
                    Accessory_Parent.transform.position = VerticalBar_Parent.transform.position = new Vector3(-combined_bounds.center.x, -combined_bounds.center.y, -combined_bounds.center.z);//to place parent at the center
                }

                HorizontalBar_Parent.transform.parent = Louver_Model.transform;

                VerticalBar_Parent.transform.parent = Louver_Model.transform;

                if (Accessory_Parent != null)
                {
                    Accessory_Parent.transform.parent = Louver_Model.transform;
                }

                //Louver_Model.transform.position = VerticalBar_Parent.transform.position;

                //Attaching rotateonmouse script to new Game object
                if (Louver_Model.gameObject.GetComponent<rotateonmouse>() == null)
                {
                    rotateonmouse RotateOnMouseScript = Louver_Model.gameObject.AddComponent<rotateonmouse>();

                    ////setting target as Louver_Model
                    RotateOnMouseScript.target = Louver_Model.transform;
                }

                if (Louver_Model.gameObject.GetComponent<PanAndZoom>() == null)
                {
                    //Attaching pan and zoom script
                    PanAndZoom PanAndZoomScript = Louver_Model.gameObject.AddComponent<PanAndZoom>();
                }
                //Parent.transform.parent = Louver_Model.transform;
                float margin = 2f;//extra gap given to the game object along x direction on screen
                                  //Parent.AddComponent<MeshRenderer>();

                Camera.main.transform.GetComponent<Camera>().orthographicSize = combined_bounds.extents.x * margin;//setting the orthographic camera size to fit the size of game object(Model)

                Vector3 max_size = combined_bounds.max;

                float x_max = max_size.x;
                float y_max = max_size.y;
                float z_max = max_size.z;



                float max_axis = Mathf.Max(x_max, y_max, z_max);

                Camera.main.transform.GetComponent<Camera>().nearClipPlane = -max_axis * 1.2f;

                Camera.main.transform.GetComponent<Camera>().farClipPlane = max_axis * 1.2f;


                string path = file_Dir;



                Directory.CreateDirectory(path);//creating directory
                string filePath_obj = System.IO.Path.Combine(path, "OBJ");//, unique_id);

                Directory.CreateDirectory(filePath_obj);
                filePath_obj = System.IO.Path.Combine(filePath_obj, unique_id);
                string filePath_obj_zip = System.IO.Path.Combine(path, "OBJ.zip");
                ExportObj.path = filePath_obj;//assigning path for exporting USING PROBUILDER

                //settings for OBJ exporting from Pro builder

                ObjOptions obOpt = new ObjOptions()
                {

                    handedness = ObjOptions.Handedness.Right,
                    textureOffsetScale = false,
                    copyTextures = true

                };

                ExportObj.ExportWithFileDialog(pb_list, true, true, obOpt); //from probuilder



                //To export as FBX file
                //To export as FBX file

                FbxExportSettings fx = new FbxExportSettings()
                {
                    embedTextures = true,
                    embedShaderProperty = true,

                };

                string filePath_fbx = System.IO.Path.Combine(path, "FBX");

                Directory.CreateDirectory(filePath_fbx);

                filePath_fbx = System.IO.Path.Combine(filePath_fbx, $"{unique_id}.fbx");
                string filePath_fbx_zip = System.IO.Path.Combine(path, "FBX.zip");


                FbxExporter.Export(filePath_fbx, fx, Louver_Model.transform);

                arrows_Measure_Louvers_script.arrows_Louvers("_FRONT");
            }
            #endregion
        }
    }
}
