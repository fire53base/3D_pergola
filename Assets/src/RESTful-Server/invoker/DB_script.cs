using BzKovSoft.ObjectSlicer.Samples;
using BzKovSoft.ObjectSlicerSamples;
using Newtonsoft.Json;
using RESTfulHTTPServer.src.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.ProBuilder;
using UnityEditor.ProBuilder.Actions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
using UnLogickFactory;

namespace RESTfulHTTPServer.src.invoker
{
    public class DB_script : MonoBehaviour
    {
        public static string unique_id;
        public static string file_Dir = @"D:\pergolawebapp\wwwroot\svg\";
        public static float left_end_cut_angle = 0, right_end_cut_angle = 0;
        public static GameObject Bar_Prefab;
        public static float frame_A_length, frame_B_length, frame_C_length, frame_D_length, frame_E_length, frame_F_length, frame_G_length, frame_H_length;
        public static GameObject VerticalBar_Parent, HorizontalBar_Parent, Grand_Parent, Accessory_Parent;

        //Layer mask
        public static LayerMask frame_layer, divider_layer, horizontal_layer;
        //for pergola
        public static float L_accessory_thickess = 1.2f;
        public static float U_accessory_thickess = 2.0f;
        public static GameObject Frames_Parent, Pergola_Model, Divider_Parent, FrameDividers_Parent, FieldDividers_Parent, FieldDividers_Parent_Section1, FieldDividers_Parent_Section2, Field_Parent, Field_Parent_Section2, Field_Parent_Section1, SupportBars_Parent1, SupportBars_Parent_Section3, SupportBars_Parent3, SupportBars_Parent2, SupportBars_Parent_Section2, SupportBar_Parent, SupportBars_Parent,
            FieldDivider_Parent_Section1, FieldDivider_Parent_Section2, FrameDividers_Parent_Section1, FrameDividers_Parent_Section2, FrameDividers_Parent_Section3, FrameDividers_Parent_Section4;
        public static sliceGameObject sliceGO_script;
        public static GameObject Controller;
        public static GameObject frameBar_prefab, DividerBar_prefab, fieldBar_prefab, L_accessoryBar_prefab, U_accessoryBar_prefab, l_clamp_onFrame_full_Prefab, l_clamp_onFrame_twoThird_Prefab, T_clamp_onFramePrefab, l_clamp_onWallPrefab, telescope_prefab,
                   Rubber_Angle_prefab, c_clamp_fordivider_prefab, Spider_to_the_crown_prefab, Spider_accessory_prefab, vertical_40x40_Bar_Prefab, spider_prefab, ak_79_L_accessory_prefab,
                    Spider_to_the_crown_v1_prefab, spider_v1_prefab, Spider_accessory_v1_prefab, l_clamp_onWallPrefab_for_l;
        public static float assembly_tolerance = 15; //15mm

        SqlConnection sqlCnn = null;
        //these Game object names are not alllowed while updating to DB
        public static String[] NotAllowed_in_db = { "Main Camera", "Directional Light", "UnityMainThreadDispatcher", "default", "Cube_left", "Cube_right", "HorizontalBar_Parent (1)", "Parent","Locator_top","Left Plane","Right Plane","Controller"
        ,"Cube_supportBart_PartA_edge","Cube_supportBart_PartA","ALL.00.04_Cube","Locator_1","Locator_2","ALL.00.02_Cube","ALL.00.03_Cube","Locator_TopInside","Locator_TopInside_for_cClamp",
        "Locator_TopInside_for_Spider","Locator_TopInside_for_Clip","Locator_Top","supportBar_part_telescope_Cube","Button_Arrow","Canvas","EventSystem"};
        public static String[] Clips = { "ak - 40", "ak - 72", "ak - 76", "ak - 39" };
        public static String[] combined_Clips_names = { "ak - 40-39", "ak - 40-76", "ak - 40-72", "ak - 40-72-40" };
        public static string[] horizontalBar_forUType_Accessory = { "32290", "ak - 4", "ak - 21" };
        public static float cut_length = 7000;
        public static float space_btw_rafafa = 0;
        public static float MAX_SUPPORTBAR_DIVIDER_DISTANCE = 3000;
        public static float MAX_FIELD_DIVIDER_DISTANCE = 2000;
        public static string rafafa_placement_type = "", frame_color_texture="", rafafa_color_texture="";
        public static int region1_fields_count = 0, region2_fields_count = 0, region3_fields_count = 0;
        String[] vertical_names = { "ak - 109a", "ak - 34a" };

        String[] crown_names = {"ak - 31a new", "32306" };
        enum model_Types
        {
            pergola_model
 , architect_3D, louver
        }

        Dictionary<string, string> textures = new Dictionary<string, string>() {
            {"american cherry textured 18","18_part" },
            {"cherry textured 82","82 textured_part" }, {"cherry textured 84","84_part" },
            { "cherry textured 85","85_part" },
            {"cherry gloss 82","82_part" },
            {"mahogany texture 92","92_part" },
            {"mediterranean cherry textured 86","86_part" },
            {"natural oak textured 78","78_part" },
            {"oak textured 73","73_part" },
            {"oak textured 74","74_part" },
            {"walnut gloss 10","10_part" },
            {"walnut textured 10","10 textured_part"},
            {"walnut textured 19","19_part"}};


        Dictionary<string, string> ral_colors = new Dictionary<string, string>(){
            {"RAL-9006", "#A1A1A0"},
            { "RAL-9007","#878581" },
            { "RAL-1013","#E3D9C6" },
            { "RAL-7042","#8E9291" },
            { "RAL-7037","#7A7B7A" },
            { "RAL-7015","#4F5358" },
            {"RAL 9016","#EDEDE6" } ,
            {"RAL 9003","#EBEEEC" },
            {"RAL 7012","#575D5E" },
            {"RAL 7043","#4F5250" },
            {"RAL 7016","#383E42" },
            {"RAL 7011","#52595D" } };


        String[] horizontal_names = { };

        enum part_type_enum { horizontal, vertical, accessory, none, frame };
        public static LayerMask end_V_Bar;

        public static string project_unique_id, building_unique_id, element_unique_id, serverAddress;
        public static string connectionString = "";

        public static List<string> Accessories_name;

        public static Screen_Shot screen_Shot_script;
        public static Arrows_Measure arrows_Measure_script;

        public static bool completed = false;
        public static SupportBarLengths supportBarLengths;
        string url;
        public static bool no_fields = false;
        public static Wall_Script wall_Script_;

        public static bool I_type, L_type, U_type;

        DB_Script_louvers dB_Script_Louvers_script;

        public static bool Architect_3D = false;
        void Start()
        {
            //Note: below is dummy, as for each request new class object is created and reference made in Start() will be null
            if (GameObject.Find("Parent") != null)
                VerticalBar_Parent = GameObject.Find("Parent");

            end_V_Bar = LayerMask.NameToLayer("end_V_Bar");
            frame_layer = LayerMask.NameToLayer("frame");
            divider_layer = LayerMask.NameToLayer("divider");
            horizontal_layer = LayerMask.NameToLayer("horizontal");
            screen_Shot_script = Camera.main.gameObject.GetComponent<Screen_Shot>();
            arrows_Measure_script = GameObject.Find("Directional Light").GetComponent<Arrows_Measure>();


            dB_Script_Louvers_script = GameObject.Find("Directional Light").GetComponent<DB_Script_louvers>();
        }

        public static List<ProBuilderMesh> pb_list, pb_list_3D_Architect;


        public class RequestForModel
        {


            public Request request;


            public RequestForModel(Request req)
            {

                request = req;

            }
        }
        public static bool modelGenInProgress = false;
        Queue<RequestForModel> QmodelRequests = new Queue<RequestForModel>();


        public async Task<Response> data_base_linking_general(Request request)
        {
            Architect_3D = false;

            RequestForModel m = new RequestForModel(request);

            Response response = new Response();
            QmodelRequests.Enqueue(m);

            while (QmodelRequests.Count > 0)
            {
                if (!modelGenInProgress)
                {
                    RequestForModel curr_model = QmodelRequests.Dequeue();
                    string json = request.GetPOSTData();

                    try
                    {

                        IModel3D item = JsonConvert.DeserializeObject<IModel3D>(json);

                         UnityMainThreadDispatcher.Dispatch(() =>
                        {
                            if (GameObject.Find("Louver_Model") != null)
                            {

                                DestroyImmediate(GameObject.Find("Louver_Model"));


                            }

                            if (GameObject.Find("Pergola_Model") != null)
                                DestroyImmediate(GameObject.Find("Pergola_Model"));
                        });

                        if (request.GetRoute().GetUrl() == "/pergola")
                        {

                            //if (!string.IsNullOrEmpty(item.unique_id))
                            //{
                            //    modelGenInProgress = true;

                            //    Architect_3D = true;
                            //    response = data_base_linking_pergola_architect(request).Result;
                            //}
                            //else
                            //{
                                modelGenInProgress = true;
                                response = data_base_linking_pergola(request).Result;
                                //Debug.Log("Louver Model");
                            //}

                            Debug.Log("Pergola");
                        }
                        else if(request.GetRoute().GetUrl() == "/architect/pergola")
                        {
                            modelGenInProgress = true;
                            response = data_base_linking_pergola(request,"architect_3D").Result;
                            Debug.Log("3D Architect Model Model");
                        }
                        else
                        {
                            modelGenInProgress = true;

                            response = await UnityMainThreadDispatcher.DispatchAsync(() =>
                                   GameObject.Find("Directional Light").GetComponent<DB_Script_louvers>().data_base_linking_Louvers(request)
                               ).Result;
                            //data_base_linking
                            //t.RunSynchronously();
                            //t.Wait();
                        }
                    }
                    catch (Exception ex)
                    {
                        modelGenInProgress = false;
                        //try
                        //{
                        //  response=  data_base_linking_pergola(request).Result;
                        //}
                        //catch (Exception exc)
                        //{

                        //    Debug.Log("Desktop App model generation: "+exc);
                        //}
                        Debug.Log("Model request: " + ex);
                    }



                    //if (completed == true)
                    //{
                    //    modelGenInProgress = false;
                    //}

                }
                else
                {
                    await Task.Delay(2000);
                }
            }




            return response;
        }

   
        public class IModel3D
        {

            public string element_name { get; set; }

            public int element_shape_id { get; set; }

            public string measuring_unit { get; set; }

            public string frame_type { get; set; }

            public string divider_type { get; set; }

            public string support_line_placement { get; set; }

            public string frame_color_texture { get; set; }

            public string rafafa_type { get; set; }

            public string rafafa_color_texture { get; set; }

            public int rafafa_spacing { get; set; }

            public string rafafa_placement_type { get; set; }

            public string unique_id { get; set; }
            public string output_folder_path { get; set; }


            public decimal frame_divider_distance { get; set; }
            public pergola_details[] pergola_details { get; set; }
        }

        public class pergola_details
        {
            public string side_name { get; set; }

            public float side_value { get; set; }


            public int is_fixed_to_wall { get; set; }

        }

    
    
        public void save_obj_dxf()
        {
            GameObject Wall_Parent = GameObject.Find("Wall_Parent");

            GameObject SupportBars_Parent = GameObject.Find("SupportBars_Parent");


            GameObject Accesories_Parent = GameObject.Find("Accesories_Parent");
            if (Wall_Parent != null)
                Wall_Parent.SetActive(false);

            if (SupportBars_Parent != null)
                SupportBars_Parent.SetActive(false);

            if (Accesories_Parent != null)
                Accesories_Parent.SetActive(false);

            //foreach(Transform sch in SupportBars_Parent.transform)
            //{
            //    sch.gameObject.SetActive(false);
            //}
            pb_list_3D_Architect = new List<ProBuilderMesh>();

            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
            {
                if (go.gameObject.GetComponent<ProBuilderMesh>() != null)
                {
                    if (!pb_list_3D_Architect.Contains(go.gameObject.GetComponent<ProBuilderMesh>()))
                        pb_list_3D_Architect.Add(go.gameObject.GetComponent<ProBuilderMesh>());


                }
            }


            string path = file_Dir;

            Directory.CreateDirectory(path);//creating directory
            string filePath_dxf_obj = System.IO.Path.Combine(path, "OBJ_forDXF");//, unique_id);

            Directory.CreateDirectory(filePath_dxf_obj);
            filePath_dxf_obj = System.IO.Path.Combine(filePath_dxf_obj, unique_id + "_forDXF");

            ExportObj.path = filePath_dxf_obj;//assigning path for exporting USING PROBUILDER

            //settings for OBJ exporting from Pro builder

            ObjOptions obOpt = new ObjOptions()
            {

                handedness = ObjOptions.Handedness.Right,
                textureOffsetScale = false,
                copyTextures = true

            };

            ExportObj.ExportWithFileDialog(pb_list_3D_Architect, true, true, obOpt); //from probuilder
            if (Wall_Parent != null)
                Wall_Parent.SetActive(true);

            if (SupportBars_Parent != null)
                SupportBars_Parent.SetActive(true);

            if (Accesories_Parent != null)
                Accesories_Parent.SetActive(true);

            foreach (Transform sch in SupportBars_Parent.transform)
            {
                sch.gameObject.SetActive(true);
            }
            pb_list_3D_Architect.Clear();
        }

     
        public async Task<Response> data_base_linking_pergola(Request request,string model_type="pergola_model")
        {
            no_fields = false;

            string db = "";

            if(model_type=="pergola_model")
            {
                db = "alukal";
            }
            else
            {
                db = "Architect";
            }

            //connectionString = $@"Data Source=212.29.201.154,3701\SQLEXPRESS;Initial Catalog={db};User ID=sa;Password=Password@1234";
            connectionString = $@"Data Source=localhost\SQLEXPRESS;Initial Catalog={db};User ID=sa;Password=Password@1234";
            //connectionString = @"Data Source=LAPTOP-MSD69D7E\SQL_LEARNING;Initial Catalog=alukal;User ID=sa;Password=Password@1234";

            string json = request.GetPOSTData();
            pergola_info item = JsonConvert.DeserializeObject<pergola_info>(json);

            project_unique_id = item.project_unique_id;
            building_unique_id = item.building_unique_id;
            element_unique_id = item.element_unique_id;
            file_Dir = item.output_folder_path;

            Response response = new Response();
            completed = false;
            unique_id = item.project_unique_id + "_" + item.building_unique_id + "_" + item.element_unique_id;
            string responseData = unique_id + ".obj";//".fbx";

            pb_list = new List<ProBuilderMesh>();
            I_type = false;
            L_type = false;
            U_type = false;
            Task t = new Task(async () =>
            {
                SqlDataAdapter adapter = new SqlDataAdapter();

                //tbl_element_pergola_header sql-params
                string sql_element_pergola_header = "";
                DataSet dsElement_pergola_header = new DataSet();

                //tbl_element_pergola_details sql-params
                string sql_tbl_element_pergola_details = "";
                DataSet dsElement_pergola_details = new DataSet();


                sqlCnn = new SqlConnection(connectionString);
                Int32 count = 0;
                try
                {
                    sqlCnn.Open();
                    //checking if values exist in points table
                    //string get_points_query = $"select COUNT(*) as row_count from dbo.tbl_element_pergola_points where project_unique_id = '{project_unique_id}' and building_unique_id = '{building_unique_id}' and element_unique_id = '{element_unique_id}'";
                    //SqlCommand sqlCmd_get_points = new SqlCommand(get_points_query, sqlCnn);
                    //count = (Int32)sqlCmd_get_points.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }

                //if (count > 0)
                //{
                //    //generate model from points
                //}
                //else
                //{ //generate model from scratch

                sql_tbl_element_pergola_details = $"select * from tbl_element_pergola_details where project_unique_id = '{item.project_unique_id}' and building_unique_id='{item.building_unique_id}' and  element_unique_id='{item.element_unique_id}' order by side_name asc";
                adapter.SelectCommand = new SqlCommand(sql_tbl_element_pergola_details, sqlCnn);
                adapter.Fill(dsElement_pergola_details);


                sql_element_pergola_header = $"select TOP(1) * from tbl_element_pergola_header where project_unique_id = '{item.project_unique_id}' and building_unique_id='{item.building_unique_id}' and  element_unique_id='{item.element_unique_id}'";
                adapter.SelectCommand = new SqlCommand(sql_element_pergola_header, sqlCnn);
                adapter.Fill(dsElement_pergola_header);

                try
                {
                    if (dsElement_pergola_details.Tables.Count > 0 && dsElement_pergola_header.Tables.Count > 0)
                    {
                        Task<int> result = await UnityMainThreadDispatcher.DispatchAsync(() => generate_model_pergola(dsElement_pergola_details, dsElement_pergola_header,model_type));
                        responseData = unique_id + ".obj";//".fbx";
                        response.SetHTTPStatusCode((int)HttpStatusCode.OK);
                        response.SetContent(responseData);
                        response.SetMimeType(Response.MIME_CONTENT_TYPE_TEXT);
                        //completed = true;
                        Debug.Log(result.Result);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Async method call : UnityMainThreadDispatcher.DispatchAsync(() => DB_points(ds)) : " + ex);
                    responseData = ex.Message.ToString();//".fbx";
                    response.SetHTTPStatusCode((int)HttpStatusCode.InternalServerError);
                    response.SetContent(responseData);
                    response.SetMimeType(Response.MIME_CONTENT_TYPE_TEXT);
                    completed = true;
                }
                finally
                {
                    adapter.Dispose();
                    sqlCnn.Close();
                }
                //}
            });
            t.RunSynchronously();
            await t;

            float secs = 0;
            while (!completed)
            {
                int delay = 1000;
                secs = secs + delay;

                await Task.Delay(delay);
                if (secs > 120000)
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
        }

        public static float raffaf_spacing_pergola = 0;

        public static List<string> wall_sides;
        public static bool wall_on_side_C = false;


        public async Task<int> generate_model_pergola(DataSet dsElement_pergola_details, DataSet dsElement_pergola_header,string model_type="pergola_model")
        {
            wall_on_side_C = false;

            Accessories_name = new List<string>();

            wall_sides = new List<string>();
            #region Creation and Destruction of Game objects

            if (GameObject.Find("Pergola_Model") != null)
            {
                DestroyImmediate(GameObject.Find("Pergola_Model"));
            }

            Resources.UnloadUnusedAssets();


            Pergola_Model = new GameObject("Pergola_Model");
            Frames_Parent = new GameObject("Frames_Parent");
            Frames_Parent.transform.parent = Pergola_Model.transform;

            Controller = GameObject.Find("Controller");
            sliceGO_script = Controller.GetComponent<sliceGameObject>();
            #endregion

            string frame_type = dsElement_pergola_header.Tables[0].Rows[0]["frame_type"].ToString();
            string divider_type = dsElement_pergola_header.Tables[0].Rows[0]["divider_type"].ToString();
            string horizontal_type = dsElement_pergola_header.Tables[0].Rows[0]["rafafa_type"].ToString();
            string support_line_placement = dsElement_pergola_header.Tables[0].Rows[0]["support_line_placement"].ToString();
            frame_color_texture = dsElement_pergola_header.Tables[0].Rows[0]["frame_color_texture"].ToString();
            rafafa_color_texture = dsElement_pergola_header.Tables[0].Rows[0]["rafafa_color_texture"].ToString();
             rafafa_placement_type = dsElement_pergola_header.Tables[0].Rows[0]["rafafa_placement_type"].ToString();
            float support_line_placement_value = 0;

            float frame_divider_distance = 0;
            float field_divider_distance= 0;
            try
            {
            if(dsElement_pergola_header.Tables[0].Rows[0]["frame_divider_distance"]!=null)
            frame_divider_distance= float.Parse( dsElement_pergola_header.Tables[0].Rows[0]["frame_divider_distance"].ToString());
            }
              catch (Exception ex)
            {

                print("column missing-frame_divider_distance: " + ex.Message);
            }


            if(frame_divider_distance<=0)
            {
                frame_divider_distance = 3000;
            }

            MAX_SUPPORTBAR_DIVIDER_DISTANCE = frame_divider_distance;
            try
            {
                if (dsElement_pergola_header.Tables[0].Rows[0]["field_divider_distance"] != null)
                    field_divider_distance = float.Parse(dsElement_pergola_header.Tables[0].Rows[0]["field_divider_distance"].ToString());
            }
            catch (Exception ex)
            {

                print("column missing-field_divider_distance: " + ex.Message);
            }


            if (field_divider_distance <= 0)
            {
                field_divider_distance = 1000;
            }

            MAX_FIELD_DIVIDER_DISTANCE = field_divider_distance;

            string l_clamp_onFrame_full = "ALL.00.04_Full";
            string l_clamp_onFrame_two_third = "ALL.00.04_TwoThird";
            string l_clamp_for_l = "L_clamp";
            string t_clamp_onFrame = "ALL.00.03";
            string l_clamp_onWall = "ALL.00.02";
            string L_accessory_type = "ak - 68";
            string U_accessory_type = "ak - 16";
            string telescope = "Telescope";
            string L_Rubber_accessory = "150000446";
            string clampfordivider = "170000048";
            string spidertocrown = "180001258";
            string spider_accessory = "180005738";
            string spider = "170000059";
            string ak_79 = "ak - 79";
            string spidertocrown_v1 = "180001258_v1";
            string Spider_accessory_v1 = "180005738_v1";
            string spider_v1 = "170000059_v1";



            space_btw_rafafa = Convert.ToSingle(dsElement_pergola_header.Tables[0].Rows[0]["rafafa_spacing"]);
            string vertical_40x40_bar = "ak - 109a";
            raffaf_spacing_pergola = space_btw_rafafa;

            frameBar_prefab = (GameObject)Resources.Load($"prefabs/{frame_type}", typeof(GameObject));
            DividerBar_prefab = (GameObject)Resources.Load($"prefabs/{divider_type}", typeof(GameObject));
            fieldBar_prefab = (GameObject)Resources.Load($"prefabs/{horizontal_type}", typeof(GameObject));
            L_accessoryBar_prefab = (GameObject)Resources.Load($"prefabs/{L_accessory_type}", typeof(GameObject));
            U_accessoryBar_prefab = (GameObject)Resources.Load($"prefabs/{U_accessory_type}", typeof(GameObject));
            l_clamp_onFrame_full_Prefab = (GameObject)Resources.Load($"prefabs/{l_clamp_onFrame_full}", typeof(GameObject));
            l_clamp_onFrame_twoThird_Prefab = (GameObject)Resources.Load($"prefabs/{l_clamp_onFrame_two_third}", typeof(GameObject));
            l_clamp_onWallPrefab_for_l = (GameObject)Resources.Load($"prefabs/{l_clamp_for_l}", typeof(GameObject));

            T_clamp_onFramePrefab = (GameObject)Resources.Load($"prefabs/{t_clamp_onFrame}", typeof(GameObject));
            l_clamp_onWallPrefab = (GameObject)Resources.Load($"prefabs/{l_clamp_onWall}", typeof(GameObject));
            telescope_prefab = (GameObject)Resources.Load($"prefabs/{telescope}", typeof(GameObject));
            Rubber_Angle_prefab = (GameObject)Resources.Load($"prefabs/{L_Rubber_accessory}", typeof(GameObject));
            c_clamp_fordivider_prefab = (GameObject)Resources.Load($"prefabs/{clampfordivider}", typeof(GameObject));
            Spider_to_the_crown_prefab = (GameObject)Resources.Load($"prefabs/{spidertocrown}", typeof(GameObject));
            Spider_accessory_prefab = (GameObject)Resources.Load($"prefabs/{spider_accessory}", typeof(GameObject));
            vertical_40x40_Bar_Prefab = (GameObject)Resources.Load($"prefabs/{vertical_40x40_bar}", typeof(GameObject));
            spider_prefab = (GameObject)Resources.Load($"prefabs/{spider}", typeof(GameObject));
            Spider_accessory_v1_prefab = (GameObject)Resources.Load($"prefabs/{Spider_accessory_v1}", typeof(GameObject));
            spider_v1_prefab = (GameObject)Resources.Load($"prefabs/{spider_v1}", typeof(GameObject));
            Spider_to_the_crown_v1_prefab = (GameObject)Resources.Load($"prefabs/{spidertocrown_v1}", typeof(GameObject));

            ak_79_L_accessory_prefab = (GameObject)Resources.Load($"prefabs/{ak_79}", typeof(GameObject));

            

            PartSettings framePartSettings = new PartSettings(frame_type, connectionString);
            PartSettings verticalPartSettings = new PartSettings(divider_type, connectionString);
            PartSettings verticalPartAccessory = new PartSettings(vertical_40x40_bar, connectionString);
            PartSettings hPartSetting = null;
            if (!String.IsNullOrEmpty(horizontal_type))
                hPartSetting = new PartSettings(horizontal_type, connectionString);

            GameObject Frame_Parent = null;

            if (Convert.ToInt32(dsElement_pergola_header.Tables[0].Rows[0]["element_shape_id"]) == 0) //i Shape
            {
                I_type = true;
                float scale_a = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[0]["side_value"]); //side A
                float scale_b = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[1]["side_value"]); //side B
                float scale_c = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[2]["side_value"]); //side C
                float scale_d = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[3]["side_value"]); //side D

                int side_a_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[0]["is_fixed_to_wall"]);
                int side_b_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[1]["is_fixed_to_wall"]);
                int side_c_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[2]["is_fixed_to_wall"]);
                int side_d_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[3]["is_fixed_to_wall"]);

                if (side_a_wall == 1)
                    wall_sides.Add("A");

                if (side_b_wall == 1)
                    wall_sides.Add("B");

                if (side_c_wall == 1)
                    wall_sides.Add("C");

                if (side_d_wall == 1)
                    wall_sides.Add("D");


                //assigning the total length of the frame to Global variables to be used in other scripts
                frame_A_length = scale_a;
                frame_B_length = scale_b;
                frame_C_length = scale_c;
                frame_D_length = scale_d;

                FrameDividers frameDividers = new FrameDividers(scale_c, framePartSettings, verticalPartSettings, MAX_SUPPORTBAR_DIVIDER_DISTANCE);

                if (!string.IsNullOrEmpty(dsElement_pergola_header.Tables[0].Rows[0]["support_line_placement_value"].ToString()))
                {
                    support_line_placement_value = float.Parse(dsElement_pergola_header.Tables[0].Rows[0]["support_line_placement_value"].ToString());
                }

                float manual_support_bar_distance = 200;// 0.666667f * scale_b;

                if (support_line_placement == "manual")
                {
                    manual_support_bar_distance = support_line_placement_value;
                }

                if (manual_support_bar_distance < 200 || manual_support_bar_distance > scale_b || support_line_placement == "two_third")
                {
                    manual_support_bar_distance = support_line_placement_value;
                }
                #region Instantiation and positioning Frame

                //Note: to get Width of the Divider, use below 2 lines
                //Bounds frameDivider_bound = Calculate_b(vertical_bar_sideC.transform);
                //float right_width = Vector3.Dot(vertical_bar_sideC.transform.parent.InverseTransformPoint(Pergola_Model.transform.forward), frameDivider_bound.size);
                GameObject vertical_bar_sideC = null, vertical_bar_sideB = null, vertical_bar_sideA = null, vertical_bar_sideD = null;
                Characteristics characteristics_script = null;

                Frame_Parent = new GameObject("Frame_Parent");
                Frame_Parent.transform.parent = Frames_Parent.transform;

                if (scale_c > cut_length && frameDividers.numberOfDividerPoles > 0)
                {
                    float framePoleCutScale = frameDividers.each_field_width + framePartSettings._part_depth - 200;
                    float frame_components_naming_counter = 0;
                    float side_c_reversePosition = scale_c;
                    for (int i = 0; i < frameDividers.numberOfDividerPoles + 1; i++)
                    {
                        vertical_bar_sideC = Instantiate(frameBar_prefab, Frame_Parent.transform);
                        vertical_bar_sideC.AddComponent<MySlicer>();
                        vertical_bar_sideC.transform.localPosition = new Vector3(0, 0, side_c_reversePosition);
                        vertical_bar_sideC.transform.localRotation = Quaternion.Euler(0, -90, 90);
                        vertical_bar_sideC.transform.localScale = new Vector3(1, framePoleCutScale, 1);
                        vertical_bar_sideC.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        vertical_bar_sideC.name = "FrameC_" + frame_components_naming_counter++;
                        characteristics_script = vertical_bar_sideC.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = frame_type;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.frame.ToString();
                        characteristics_script.part_group = "Frame";
                        characteristics_script.left_end_cut_angle = "90, 0, 0";
                        characteristics_script.right_end_cut_angle = "90, 0, 0";
                        side_c_reversePosition -= framePoleCutScale;
                        framePoleCutScale = frameDividers.each_field_width + verticalPartSettings._part_depth;
                        if (i == 0)
                        {
                            characteristics_script.left_end_cut_angle = "270, 0, 135";
                            characteristics_script.right_end_cut_angle = "90, 0, 0";
                            characteristics_script.icon_filename = "frame0-45.jpg";
                            sliceGO_script.Slice_one_side(vertical_bar_sideC, new Vector3(270, 0, 135), vertical_bar_sideC.transform.Find("Cube_left").position);
                        }
                        if (i == frameDividers.numberOfDividerPoles - 1) //last frame bar piece needs to fill the remaining gap
                        {
                            framePoleCutScale = 200 + frameDividers.each_field_width + verticalPartSettings._part_depth + framePartSettings._part_depth;
                        }
                        else if (i == frameDividers.numberOfDividerPoles)
                        {
                            characteristics_script.left_end_cut_angle = "90, 0, 0";
                            characteristics_script.right_end_cut_angle = "270, 0, -135";
                            characteristics_script.icon_filename = "frame450.jpg";
                            sliceGO_script.Slice_one_side(vertical_bar_sideC, new Vector3(270, 0, -135), vertical_bar_sideC.transform.Find("Cube_right").position);
                        }
                    }
                    vertical_bar_sideC = Frame_Parent.transform.Find("FrameC_0").gameObject; //because original will be destroyed after slicing
                }
                else
                {
                    vertical_bar_sideC = Instantiate(frameBar_prefab, Frame_Parent.transform);
                    vertical_bar_sideC.transform.localPosition = new Vector3(0, 0, scale_c);
                    vertical_bar_sideC.transform.localRotation = Quaternion.Euler(0, -90, 90);
                    vertical_bar_sideC.transform.localScale = new Vector3(1, scale_c, 1);
                    vertical_bar_sideC.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    vertical_bar_sideC.name = "FrameC";
                    characteristics_script = vertical_bar_sideC.AddComponent<Characteristics>();
                    characteristics_script.part_name_id = frame_type;
                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                    characteristics_script.part_type = part_type_enum.frame.ToString();
                    characteristics_script.part_group = "Frame";
                    characteristics_script.left_end_cut_angle = "270, 0, 135";
                    characteristics_script.right_end_cut_angle = "270, 0, -135";
                    characteristics_script.icon_filename = "frame45-45.jpg";
                    sliceGO_script.Slice_two_sides(vertical_bar_sideC, new Vector3(270, 0, 135), new Vector3(270, 0, -135));
                    vertical_bar_sideC = Frame_Parent.transform.Find("FrameC").gameObject; //because original will be destroyed after slicing
                }

                vertical_bar_sideB = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideB.transform.localPosition = new Vector3(-scale_b, 0, scale_c); //to offset
                vertical_bar_sideB.transform.localRotation = Quaternion.Euler(0, 180, 90);
                vertical_bar_sideB.transform.localScale = new Vector3(1, scale_b, 1);
                vertical_bar_sideB.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideB.name = "FrameB";
                characteristics_script = vertical_bar_sideB.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, -135";
                characteristics_script.right_end_cut_angle = "270, 0, 135";
                characteristics_script.icon_filename = "frameRight-4545.jpg";
                sliceGO_script.Slice_two_sides(vertical_bar_sideB, new Vector3(270, 0, -135), new Vector3(270, 0, 135));
                vertical_bar_sideB = Frame_Parent.transform.Find("FrameB").gameObject; //because original will be destroyed after slicing

                if (scale_a > cut_length && frameDividers.numberOfDividerPoles > 0)
                {
                    float framePoleCutScale = frameDividers.each_field_width + framePartSettings._part_depth - 200;
                    float frame_components_naming_counter = 0;
                    float side_a_reversePosition = 0;
                    for (int i = 0; i < frameDividers.numberOfDividerPoles + 1; i++)
                    {
                        vertical_bar_sideA = Instantiate(frameBar_prefab, Frame_Parent.transform);
                        vertical_bar_sideA.AddComponent<MySlicer>();
                        vertical_bar_sideA.transform.localPosition = new Vector3(-scale_b, 0, side_a_reversePosition);
                        vertical_bar_sideA.transform.localRotation = Quaternion.Euler(0, -270, 90);
                        vertical_bar_sideA.transform.localScale = new Vector3(1, framePoleCutScale, 1);
                        vertical_bar_sideA.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        vertical_bar_sideA.name = "FrameA_" + frame_components_naming_counter++;
                        characteristics_script = vertical_bar_sideA.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = frame_type;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.frame.ToString();
                        characteristics_script.part_group = "Frame";
                        characteristics_script.left_end_cut_angle = "90, 0, 0";
                        characteristics_script.right_end_cut_angle = "90, 0, 0";
                        side_a_reversePosition += framePoleCutScale;
                        framePoleCutScale = frameDividers.each_field_width + verticalPartSettings._part_depth;
                        if (i == 0)
                        {
                            characteristics_script.left_end_cut_angle = "270, 0, 135";
                            characteristics_script.right_end_cut_angle = "90, 0, 0";
                            characteristics_script.icon_filename = "frame-450.jpg";
                            sliceGO_script.Slice_one_side(vertical_bar_sideA, new Vector3(270, 0, 135), vertical_bar_sideA.transform.Find("Cube_left").position);
                        }
                        if (i == frameDividers.numberOfDividerPoles - 1) //last frame bar piece needs to fill the remaining gap
                        {
                            framePoleCutScale = 200 + frameDividers.each_field_width + verticalPartSettings._part_depth + framePartSettings._part_depth;
                        }
                        else if (i == frameDividers.numberOfDividerPoles) //last bar piece
                        {
                            characteristics_script.left_end_cut_angle = "90, 0, 0";
                            characteristics_script.right_end_cut_angle = "270, 0, -135";
                            characteristics_script.icon_filename = "frame045.jpg";
                            sliceGO_script.Slice_one_side(vertical_bar_sideA, new Vector3(270, 0, -135), vertical_bar_sideA.transform.Find("Cube_right").position);
                        }
                    }
                    vertical_bar_sideA = Frame_Parent.transform.Find("FrameA_0").gameObject; //because original will be destroyed after slicing
                }
                else
                {
                    vertical_bar_sideA = Instantiate(frameBar_prefab, Frame_Parent.transform);
                    vertical_bar_sideA.transform.localPosition = new Vector3(-scale_b, 0, 0); //to offset
                    vertical_bar_sideA.transform.localRotation = Quaternion.Euler(0, -270, 90);
                    vertical_bar_sideA.transform.localScale = new Vector3(1, scale_a, 1);
                    vertical_bar_sideA.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    vertical_bar_sideA.name = "FrameA";
                    characteristics_script = vertical_bar_sideA.AddComponent<Characteristics>();
                    characteristics_script.part_name_id = frame_type;
                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                    characteristics_script.part_type = part_type_enum.frame.ToString();
                    characteristics_script.part_group = "Frame";
                    characteristics_script.left_end_cut_angle = "270, 0, 135";
                    characteristics_script.right_end_cut_angle = "270, 0, -135";
                    characteristics_script.icon_filename = "frame-4545.jpg";
                    sliceGO_script.Slice_two_sides(vertical_bar_sideA, new Vector3(270, 0, 135), new Vector3(270, 0, -135));
                    vertical_bar_sideA = Frame_Parent.transform.Find("FrameA").gameObject; //because original will be destroyed after slicing
                }

                vertical_bar_sideD = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideD.transform.localPosition = new Vector3(0, 0, 0); //to offset
                vertical_bar_sideD.transform.localRotation = Quaternion.Euler(0, 0, 90);
                vertical_bar_sideD.transform.localScale = new Vector3(1, scale_d, 1);
                vertical_bar_sideD.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideD.name = "FrameD";
                characteristics_script = vertical_bar_sideD.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, -135";
                characteristics_script.right_end_cut_angle = "270, 0, 135";
                characteristics_script.icon_filename = "frameLeft45-45.jpg";
                sliceGO_script.Slice_two_sides(vertical_bar_sideD, new Vector3(270, 0, -135), new Vector3(270, 0, 135));
                vertical_bar_sideD = Frame_Parent.transform.Find("FrameD").gameObject; //because original will be destroyed after slicing

                GameObject Accesories_Parent = new GameObject("Accesories_Parent");
                Accesories_Parent.transform.parent = Frames_Parent.transform;

                int inside_frame_accessories_count = 0;
                if (frame_type == "ak - 288") //80x80
                {
                    List<GameObject> accessories_together_spider_ak288 = new List<GameObject>();
                    GameObject Spider_Accessory_front = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);

                    Spider_Accessory_front.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                          vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                          vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_Accessory_front.transform.Find("Locator_top").localRotation = Quaternion.Euler(270, 0, 180);
                    Spider_Accessory_front.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_Accessory_front.name = spider_accessory + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together_spider_ak288.Add(Spider_Accessory_front);


                    GameObject Spider_crown_Left_Front_ = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -90);
                    Spider_crown_Left_Front_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_Left_Front_.name = spider_accessory + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together_spider_ak288.Add(Spider_crown_Left_Front_);

                    GameObject Spider_crown_right = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_right.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_right.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right.name = spider_accessory + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together_spider_ak288.Add(Spider_crown_right);

                    GameObject Spider_crown_right_front = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_right_front.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right_front.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 90);
                    Spider_crown_right_front.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_front.name = spider_accessory + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together_spider_ak288.Add(Spider_crown_right_front);

                    foreach (GameObject accessory in accessories_together_spider_ak288)
                    {
                        characteristics_script = accessory.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = spider_accessory;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";
                    }

                }
                else if (frame_type == "ak - 31a new" || frame_type == "32306") //Crown 150, 200
                {
                    List<GameObject> accessories_together = new List<GameObject>();
                    List<GameObject> accessories_together_spider = new List<GameObject>();
                    float Bottom_Rubber_Offset = -3.5f;

                    GameObject Right_rear_Bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    float L_Shape_Rubber_height = Right_rear_Bottom.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.z;
                    Right_rear_Bottom.transform.localPosition = new Vector3(-1, L_Shape_Rubber_height - 1, 1); //to offset
                    Right_rear_Bottom.transform.localRotation = Quaternion.Euler(90, 0, 90);
                    Right_rear_Bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Right_rear_Bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    Right_rear_Bottom.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together.Add(Right_rear_Bottom);

                    GameObject Right_rear_Top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Right_rear_Top.transform.localPosition = new Vector3(-1, framePartSettings._part_width - 1, 1); //to offset
                    Right_rear_Top.transform.localRotation = Quaternion.Euler(90, 0, 90);
                    Right_rear_Top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Right_rear_Top.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together.Add(Right_rear_Top);

                    GameObject Right_Front_Bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Right_Front_Bottom.transform.localPosition = new Vector3(-scale_b + 1, L_Shape_Rubber_height - 1, 1); //to offset
                    Right_Front_Bottom.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    Right_Front_Bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Right_Front_Bottom.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++.ToString("D4");
                    Right_Front_Bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(Right_Front_Bottom);

                    GameObject Right_Front_Top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Right_Front_Top.transform.localPosition = new Vector3(-scale_b + 1, framePartSettings._part_width - 1, 1); //to offset
                    Right_Front_Top.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    Right_Front_Top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Right_Front_Top.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together.Add(Right_Front_Top);

                    GameObject Left_Rear_bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Left_Rear_bottom.transform.localPosition = new Vector3(-1, L_Shape_Rubber_height - 1, scale_a - 1); //to offset
                    Left_Rear_bottom.transform.localRotation = Quaternion.Euler(90, 0, -180);
                    Left_Rear_bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Left_Rear_bottom.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++.ToString("D4");
                    Left_Rear_bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(Left_Rear_bottom);


                    GameObject Left_Rear_Top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Left_Rear_Top.transform.localPosition = new Vector3(-1, framePartSettings._part_width - 1, scale_a - 1); //to offset
                    Left_Rear_Top.transform.localRotation = Quaternion.Euler(90, 0, -180);
                    Left_Rear_Top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Left_Rear_Top.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together.Add(Left_Rear_Top);

                    GameObject Left_Front_bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Left_Front_bottom.transform.localPosition = new Vector3(-scale_b + 1, L_Shape_Rubber_height - 1, scale_a - 1); //to offset
                    Left_Front_bottom.transform.localRotation = Quaternion.Euler(90, 0, -90);
                    Left_Front_bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Left_Front_bottom.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++.ToString("D4");
                    Left_Front_bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(Left_Front_bottom);

                    GameObject Left_Front_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Left_Front_top.transform.localPosition = new Vector3(-scale_b + 1, framePartSettings._part_width - 1, scale_a - 1); //to offset
                    Left_Front_top.transform.localRotation = Quaternion.Euler(90, 0, -90);
                    Left_Front_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Left_Front_top.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together.Add(Left_Front_top);

                    foreach (GameObject accessory in accessories_together)
                    {
                        characteristics_script = accessory.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = L_Rubber_accessory;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";
                    }

                    GameObject Spider_crown_Left_Front_top = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_Left_Front_top.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                          vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                          vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_Left_Front_top.transform.Find("Locator_top").localRotation = Quaternion.Euler(270, 0, 180);
                    Spider_crown_Left_Front_top.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_Left_Front_top.name = spidertocrown + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together_spider.Add(Spider_crown_Left_Front_top);


                    GameObject Spider_crown_Left_Front_ = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -90);
                    Spider_crown_Left_Front_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_Left_Front_.name = spidertocrown + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together_spider.Add(Spider_crown_Left_Front_);

                    GameObject Spider_crown_right = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_right.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_right.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right.name = spidertocrown + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together_spider.Add(Spider_crown_right);

                    GameObject Spider_crown_right_front = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_right_front.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right_front.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 90);
                    Spider_crown_right_front.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_front.name = spidertocrown + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together_spider.Add(Spider_crown_right_front);

                    foreach (GameObject accessory in accessories_together_spider)
                    {
                        characteristics_script = accessory.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = spidertocrown;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";
                    }
                }


                else if (frame_type == "ak - 279") //80x40
                {
                    List<GameObject> accessories_together_ak279 = new List<GameObject>();

                    GameObject Spider_front = Instantiate(spider_prefab, Accesories_Parent.transform);

                    Spider_front.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                          vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                          vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_front.transform.Find("Locator_top").localRotation = Quaternion.Euler(270, 0, 180);
                    Spider_front.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_front.name = spider + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together_ak279.Add(Spider_front);


                    GameObject Spider_Left_Front_ = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_Left_Front_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_Left_Front_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -90);
                    Spider_Left_Front_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_Left_Front_.name = spider + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together_ak279.Add(Spider_Left_Front_);

                    GameObject Spider_right = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_right.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_right.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_right.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_right.name = spider + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together_ak279.Add(Spider_right);

                    GameObject Spider_right_front = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_right_front.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_right_front.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 90);
                    Spider_right_front.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_right_front.name = spider + "_" + inside_frame_accessories_count++.ToString("D4");
                    accessories_together_ak279.Add(Spider_right_front);

                    foreach (GameObject accessory in accessories_together_ak279)
                    {
                        characteristics_script = accessory.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = spider;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";
                    }
                }

                foreach (Transform accs in Accesories_Parent.transform)
                {
                    if (!Accessories_name.Contains(accs.name))
                        Accessories_name.Add(accs.name);
                }
                #endregion

                #region Divider

                Divider_Parent = new GameObject("Divider_Parent");
                Divider_Parent.transform.parent = Pergola_Model.transform;

                FrameDividers_Parent = new GameObject("FrameDividers_Parent");
                FrameDividers_Parent.transform.parent = Divider_Parent.transform;
                RaycastHit hit;
                GameObject frame_C = Frame_Parent.transform.GetChild(0).gameObject;
                Bounds frame_C_bound = Calculate_b(frame_C.transform);
                Vector3 global_center_Frame_C = Pergola_Model.transform.TransformPoint(frame_C_bound.center);
                Vector3 ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.right);
                Vector3 top_point_frame_C = global_center_Frame_C + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, frame_C_bound.size)) / 2;

                if (Physics.Raycast(top_point_frame_C, ray_cast_dir, out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(top_point_frame_C, ray_cast_dir * hit.distance, Color.yellow);
                    Debug.Log($"Did Hit & Distance={hit.distance}");
                }
                else
                {
                    Debug.DrawRay(top_point_frame_C, ray_cast_dir * 10000, Color.white, 10);
                    Debug.Log("Did not Hit");
                }

                //Frame Divider Code
                float dividerPoleDistance = frameDividers.each_field_width + framePartSettings._part_depth;
                int clamp_naming_counter = 0;

                for (int i = 0; i < frameDividers.numberOfDividerPoles; i++)
                {
                    GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + i.ToString("D4"));
                    frameDivider_GO_parent.transform.parent = FrameDividers_Parent.transform;

                    GameObject frameDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                    frameDivider_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, dividerPoleDistance); //Todo add(offset) frame B or D width to each field width
                    frameDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    frameDivider_GO.transform.localScale = new Vector3(1, hit.distance, 1);
                    frameDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    frameDivider_GO.name = "FrameDivider_" + i.ToString("D4");
                    dividerPoleDistance += (frameDividers.each_field_width + verticalPartSettings._part_depth);

                    if (divider_type == "ak - 279")
                    {
                        GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                        C_Clamp_ForDivider.transform.Find("Locator_top").position = frameDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                        C_Clamp_ForDivider.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                        C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                        C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");


                        if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                            Accessories_name.Add(C_Clamp_ForDivider.name);

                        GameObject C_Clamp_ForDivider_2;

                        C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                        C_Clamp_ForDivider_2.transform.Find("Locator_top").Translate(hit.distance * Vector3.right);
                        C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                        C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                        C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                        if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                            Accessories_name.Add((C_Clamp_ForDivider_2.name));
                    }
                }

                // characteristics
                foreach (Transform t in FrameDividers_Parent.transform)
                {
                    foreach (Transform child_t in t)
                    {
                        characteristics_script = child_t.gameObject.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = clampfordivider;
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_group = "Frame";
                        if (child_t.name.Contains("FrameDivider"))
                        {
                            characteristics_script.part_name_id = divider_type;
                            characteristics_script.part_type = part_type_enum.vertical.ToString();
                        }
                    }
                }


                //Field Divider Code
                FieldDividers_Parent = new GameObject("FieldDividers_Parent");
                FieldDividers_Parent.transform.parent = Divider_Parent.transform;
                InnerFieldDividers innerFieldDividers = new InnerFieldDividers(frameDividers.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                float inner_dividerPoleDistance = framePartSettings._part_depth + innerFieldDividers.each_inner_field_width; ;
                int divider_naming_counter = 0;
                for (int i = 0; i < frameDividers.numbefOfFields; i++)
                {
                    for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                    {

                        GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + divider_naming_counter.ToString("D4"));
                        fieldDivider_GO_parent.transform.parent = FieldDividers_Parent.transform;

                        GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);
                        fieldDivider_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, inner_dividerPoleDistance); //Todo add(offset) frame B or D width to each field width
                        fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        fieldDivider_GO.transform.localScale = new Vector3(1, hit.distance, 1);
                        fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        fieldDivider_GO.name = "FieldDivider_" + divider_naming_counter++.ToString("D4");
                        inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2;

                            C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").Translate(hit.distance * Vector3.right);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add(C_Clamp_ForDivider_2.name);
                        }
                    }
                    inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);
                }

                // characteristics
                foreach (Transform t in FieldDividers_Parent.transform)
                {
                    foreach (Transform child_t in t)
                    {
                        characteristics_script = child_t.gameObject.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = clampfordivider;
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_group = "Frame";
                        if (child_t.name.Contains("FieldDivider"))
                        {
                            characteristics_script.part_name_id = divider_type;
                            characteristics_script.part_type = part_type_enum.vertical.ToString();
                        }
                    }
                }

                #endregion

                #region Fields and Accessories

                Field_Parent = new GameObject("Field_Parent");
                Field_Parent.transform.parent = Pergola_Model.transform;

                int accessory_naming_counter = 1;
                int field_naming_counter = 1;
                float accessory_placement = framePartSettings._part_depth;
                int total_number_of_fields = frameDividers.numbefOfFields * innerFieldDividers.numbefOfInnerFields;

                if (horizontal_type == "ak - 40")
                {
                    float rafafa_placement = 0;
                    float field_height = scale_b - (2 * framePartSettings._part_depth) - assembly_tolerance;

                    float clip_height_full = 90.47f;
                    float clip_spare_part_height = 3.5f;
                    if (space_btw_rafafa == 20)
                    {
                        //clip_name = "ak - 76";
                        clip_height_full = 90.47f;
                        fieldBar_prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/pergola/ak - 40-76-76", typeof(GameObject));
                    }
                    else if (space_btw_rafafa == 50)
                    {
                        //clip_name = "ak - 39";
                        clip_height_full = 120.47f; //For space between 50, ak - 39
                        fieldBar_prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/pergola/ak - 40-39", typeof(GameObject));
                    }
                    else if (space_btw_rafafa == -10)
                    {
                        //clip_name = "ak - 72";
                        clip_height_full = 120.47f; //For space between 50, ak - 39
                        fieldBar_prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/pergola/ak - 40-72-40", typeof(GameObject));
                    }

                    int NumOfRafafa = Convert.ToInt32(System.Math.Floor((field_height - clip_spare_part_height) / clip_height_full));
                    float h_Bar_offset = (field_height + clip_spare_part_height) - ((NumOfRafafa * (clip_height_full - clip_spare_part_height)));

                    //HORIZONTALS ALWAYS MIDDLE ALIGNED.
                    h_Bar_offset = h_Bar_offset / 2;

                    if (h_Bar_offset <= 0)
                        h_Bar_offset = space_btw_rafafa;

                    for (int i = 1; i <= total_number_of_fields; i++)
                    {
                        GameObject FieldGroup_GO = new GameObject("Field_Group_" + i.ToString("D4"));
                        FieldGroup_GO.transform.parent = Field_Parent.transform;

                        //Accessories
                        GameObject Accessories_parent = new GameObject("Accessories_" + i.ToString("D4"));
                        Accessories_parent.transform.parent = FieldGroup_GO.transform;

                        for (int j = 1; j <= 2; j++)// L shape accessory
                        {
                            GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                            GameObject vertical_40x40_Bar = Instantiate(vertical_40x40_Bar_Prefab, Accessories_parent.transform);

                            switch (j)
                            {
                                case 1: //left bottom
                                    Accessory_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                                    Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                    Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");

                                    vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                    vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counter++.ToString("D4");
                                    vertical_40x40_Bar.transform.Translate(verticalPartAccessory._part_depth * Vector3.forward);
                                    vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.right, Space.World);
                                    vertical_40x40_Bar.transform.Translate((assembly_tolerance / 2) * Vector3.forward, Space.World);
                                    vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                    break;
                                case 2: //right bottom
                                    Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                    Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers.each_inner_field_width - assembly_tolerance);
                                    Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");

                                    vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                    vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counter++.ToString("D4");
                                    vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.right, Space.World);
                                    vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward, Space.World);
                                    vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                    break;
                            }
                            Accessory_GO.transform.localScale = new Vector3(1, hit.distance - assembly_tolerance, 1);
                            Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = L_accessory_type;
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_type = part_type_enum.vertical.ToString();
                            characteristics_script.part_group = "Frame";


                            vertical_40x40_Bar.transform.Translate((verticalPartAccessory._part_width + L_accessory_thickess) * Vector3.up);
                            vertical_40x40_Bar.transform.localRotation = Quaternion.Euler(-90, 90, 0);
                            vertical_40x40_Bar.transform.localScale = new Vector3(1, hit.distance - assembly_tolerance, 1);
                            vertical_40x40_Bar.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            characteristics_script = vertical_40x40_Bar.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = vertical_40x40_bar;
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_type = part_type_enum.vertical.ToString();
                            characteristics_script.part_group = "Field";


                            if (!Accessories_name.Contains(vertical_40x40_Bar.name))
                                Accessories_name.Add(vertical_40x40_Bar.name);

                            if (!Accessories_name.Contains(Accessory_GO.name))
                                Accessories_name.Add(Accessory_GO.name);
                        }
                        accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers.each_inner_field_width);

                        if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                        {
                            no_fields = false;

                            //Fields
                            GameObject Fields_GO = new GameObject("Fields_" + i.ToString("D4"));
                            Fields_GO.transform.parent = FieldGroup_GO.transform;
                            Fields_GO.transform.Translate(rafafa_placement * Vector3.forward);
                            //float individual_field_placement = h_Bar_offset + clip_height_full - clip_spare_part_height;
                            for (int j = 0; j < NumOfRafafa; j++)
                            {
                                GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                Field_GO.GetComponent<Follow>().follow_child = Field_GO.transform.Find("Locator_1");
                                Field_GO.GetComponent<Follow>().update_location_and_rotation();
                                Field_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideC.transform.Find("Locator_TopInside_for_Clip").position.x,
                                                                                                vertical_bar_sideD.transform.Find("Locator_TopInside_for_Clip").position.y,
                                                                                                vertical_bar_sideD.transform.Find("Locator_TopInside_for_Clip").position.z);
                                Field_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, -90, 0);
                                Field_GO.GetComponent<Follow>().move_parent_relative_toChild();
                                Field_GO.transform.GetChild(0).localScale = new Vector3(innerFieldDividers.each_inner_field_width - (assembly_tolerance / 2) - (2 * L_accessory_thickess), 1, 1);
                                if (space_btw_rafafa == -10)
                                {
                                    Field_GO.transform.GetChild(1).localScale = new Vector3(innerFieldDividers.each_inner_field_width - (assembly_tolerance / 2) - (2 * L_accessory_thickess), 1, 1);
                                    Field_GO.transform.GetChild(3).Translate((innerFieldDividers.each_inner_field_width - (assembly_tolerance / 2) - (2 * L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                }
                                else
                                {
                                    Field_GO.transform.GetChild(2).Translate((innerFieldDividers.each_inner_field_width - assembly_tolerance - (2 * L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                    Field_GO.transform.GetChild(2).GetChild(0).gameObject.AddComponent<BoxCollider>(); //for clips having spacing -10, box collider already added in prefab
                                }

                                Field_GO.transform.GetChild(0).GetChild(0).gameObject.AddComponent<BoxCollider>();
                                Field_GO.transform.GetChild(1).GetChild(0).gameObject.AddComponent<BoxCollider>();

                                Field_GO.transform.Translate(-(clip_height_full - clip_spare_part_height) * j * Vector3.up);

                                Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");


                                // characteristics for Fields
                                foreach (Transform t in Field_GO.transform)
                                {
                                    characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                    characteristics_script.part_group = "Field";
                                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                                    if (t.name.Contains("40"))
                                    {
                                        characteristics_script.part_name_id = horizontal_type;
                                        characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                    }
                                    else if (t.name.Contains("39"))
                                    {
                                        characteristics_script.part_name_id = "ak - 39";
                                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                                    }
                                    else if (t.name.Contains("72"))
                                    {
                                        characteristics_script.part_name_id = "ak - 72";
                                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                                    }
                                    else if (t.name.Contains("76"))
                                    {
                                        characteristics_script.part_name_id = "ak - 76";
                                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                                    }
                                }
                            }

                            Fields_GO.transform.Translate(-h_Bar_offset * Vector3.right);
                            Fields_GO.transform.Translate((innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth) * (i - 1) * Vector3.forward);
                            rafafa_placement += (verticalPartSettings._part_depth + innerFieldDividers.each_inner_field_width);
                        }
                        else no_fields = true;

                        FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                        FieldGroup_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                    }
                }
                else
                {
                    float field_height = 0;
                    int NumOfRafafa = 0;
                    float h_Bar_offset = 0;
                    if (!String.IsNullOrEmpty(horizontal_type))
                    {
                        field_height = scale_b - (2 * framePartSettings._part_depth) - assembly_tolerance;
                        NumOfRafafa = Convert.ToInt32(System.Math.Floor((field_height + space_btw_rafafa) / (hPartSetting._part_height + space_btw_rafafa)));

                        //HORIZONTALS ALWAYS MIDDLE ALIGNED.
                        h_Bar_offset = field_height - ((NumOfRafafa * hPartSetting._part_height) + (space_btw_rafafa * (NumOfRafafa - 1)));
                        h_Bar_offset = h_Bar_offset / 2;

                        if (h_Bar_offset <= 0)
                            h_Bar_offset = space_btw_rafafa;
                    }

                    float rafafa_placement = 0;
                    bool place_u_accessory = false;
                    if (Array.Exists(horizontalBar_forUType_Accessory, element => element == horizontal_type))
                        place_u_accessory = true;

                    for (int i = 1; i <= total_number_of_fields; i++)
                    {
                        GameObject FieldGroup_GO = new GameObject("Field_Group_" + i.ToString("D4"));
                        FieldGroup_GO.transform.parent = Field_Parent.transform;

                        //Accessories
                        GameObject Accessories_parent = new GameObject("Accessories_" + i.ToString("D4"));
                        Accessories_parent.transform.parent = FieldGroup_GO.transform;
                        if (place_u_accessory)
                        {
                            if (!String.IsNullOrEmpty(horizontal_type))
                            {
                                for (int j = 1; j <= 2; j++)// L shape accessory
                                {
                                    GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                    switch (j)
                                    {
                                        case 1: //left bottom
                                            Accessory_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                                            Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                            Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");
                                            break;
                                        case 2: //right bottom
                                            Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                            Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers.each_inner_field_width - assembly_tolerance);
                                            Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");
                                            break;
                                    }
                                    Accessory_GO.transform.localScale = new Vector3(1, hit.distance - assembly_tolerance, 1);
                                    Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                    characteristics_script.part_name_id = L_accessory_type;
                                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                    characteristics_script.part_type = part_type_enum.vertical.ToString();
                                    characteristics_script.part_group = "Frame";
                                }

                                for (int j = 1; j <= 2; j++) // U shape accessory
                                {
                                    GameObject Accessory_GO = Instantiate(U_accessoryBar_prefab, Accessories_parent.transform);
                                    switch (j)
                                    {
                                        case 1: //left bottom
                                            Accessory_GO.transform.localRotation = Quaternion.Euler(90, 0, 90);
                                            Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, L_accessory_thickess, accessory_placement + L_accessory_thickess); //Todo add(offset) frame B or D width to each field width
                                            float U_accessory_height = Accessory_GO.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.z;
                                            Accessory_GO.transform.Translate(-U_accessory_height * Accessory_GO.transform.right);
                                            Accessory_GO.name = "U_Accessory_Left_" + accessory_naming_counter++.ToString("D4");
                                            break;
                                        case 2: //right bottom
                                            Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                            Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, L_accessory_thickess, accessory_placement + innerFieldDividers.each_inner_field_width - assembly_tolerance - L_accessory_thickess); //Todo add(offset) frame B or D width to each field width
                                            Accessory_GO.name = "U_Accessory_Right_" + accessory_naming_counter++.ToString("D4");
                                            break;
                                    }

                                    Accessory_GO.transform.localScale = new Vector3(1, hit.distance - assembly_tolerance, 1);
                                    Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                    characteristics_script.part_name_id = U_accessory_type;
                                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                    characteristics_script.part_type = part_type_enum.vertical.ToString();
                                    characteristics_script.part_group = "Field";
                                }
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(horizontal_type))
                            {
                                for (int j = 1; j <= 4; j++)// L shape accessory
                                {
                                    GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                    switch (j)
                                    {
                                        case 1: //left bottom
                                            Accessory_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                                            Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                            Accessory_GO.transform.localScale = new Vector3(1, hit.distance - assembly_tolerance, 1);
                                            Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward);
                                            Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");
                                            break;
                                        case 2: //right bottom
                                            Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                            Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers.each_inner_field_width - assembly_tolerance);
                                            Accessory_GO.transform.localScale = new Vector3(1, hit.distance - assembly_tolerance, 1);
                                            Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                                            Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");
                                            break;
                                        case 3: //left top                                       
                                            Accessory_GO.transform.localRotation = Quaternion.Euler(90, 0, 90);
                                            Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, hPartSetting._part_depth + (2 * L_accessory_thickess), accessory_placement); //Accessory thickness is multiplied twice for bottom and top.so that it do not intersect with field
                                            Accessory_GO.transform.localScale = new Vector3(1, hit.distance - assembly_tolerance, 1);
                                            Accessory_GO.name = "L_Accessory_topLeft_" + accessory_naming_counter++.ToString("D4");
                                            break;
                                        case 4: //right top                                     
                                            Accessory_GO.transform.localRotation = Quaternion.Euler(180, 0, 90);
                                            Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, hPartSetting._part_depth + (2 * L_accessory_thickess), accessory_placement + innerFieldDividers.each_inner_field_width - assembly_tolerance);//Accessory thickness is multiplied twice for bottom and top.so that it do not intersect with field
                                            Accessory_GO.transform.localScale = new Vector3(1, hit.distance - assembly_tolerance, 1);
                                            Accessory_GO.name = "L_Accessory_topRight_" + accessory_naming_counter++.ToString("D4");
                                            break;
                                    }
                                    Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                    characteristics_script.part_name_id = L_accessory_type;
                                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                    characteristics_script.part_type = part_type_enum.vertical.ToString();
                                    if (j == 1 || j == 2)
                                        characteristics_script.part_group = "Frame";
                                    else characteristics_script.part_group = "Field";
                                }
                            }
                        }
                        accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers.each_inner_field_width);

                        foreach (Transform t in Accessories_parent.transform)
                        {
                            if (!Accessories_name.Contains(t.name))
                                Accessories_name.Add(t.name);
                        }

                        if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                        {
                            no_fields = false;
                            //Fields
                            GameObject Fields_GO = new GameObject("Fields_" + i.ToString("D4"));
                            Fields_GO.transform.parent = FieldGroup_GO.transform;
                            Fields_GO.transform.Translate(rafafa_placement * Vector3.forward);
                            float individual_field_placement = h_Bar_offset + framePartSettings._part_depth + hPartSetting._part_height;
                            for (int j = 0; j < NumOfRafafa; j++)
                            {
                                GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                Field_GO.transform.localRotation = Quaternion.Euler(-90, -90, 0);
                                if (place_u_accessory)
                                {
                                    Field_GO.transform.localPosition = new Vector3(-individual_field_placement, L_accessory_thickess + U_accessory_thickess, framePartSettings._part_depth + L_accessory_thickess + U_accessory_thickess);
                                    Field_GO.transform.localScale = new Vector3(innerFieldDividers.each_inner_field_width - assembly_tolerance - (2 * L_accessory_thickess) - (2 * U_accessory_thickess), 1, 1);
                                }
                                else
                                {
                                    Field_GO.transform.localPosition = new Vector3(-individual_field_placement, L_accessory_thickess, framePartSettings._part_depth + L_accessory_thickess);
                                    Field_GO.transform.localScale = new Vector3(innerFieldDividers.each_inner_field_width - assembly_tolerance - (2 * L_accessory_thickess), 1, 1);
                                }

                                Field_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");
                                individual_field_placement += space_btw_rafafa + hPartSetting._part_height;
                            }
                            rafafa_placement += (verticalPartSettings._part_depth + innerFieldDividers.each_inner_field_width);

                            // characteristics for Fields
                            foreach (Transform t in Fields_GO.transform)
                            {
                                characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = horizontal_type;//frameBar_prefab.name;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                characteristics_script.part_group = "Field";
                            }
                        }
                        else no_fields = true;

                        FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                        FieldGroup_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                    }
                }

                #endregion

                try
                {
                    await UnityMainThreadDispatcher.DispatchAsync(() => step_cut(frame_type, divider_type));
                    #region Step_cut

                    //Frame_Parent = GameObject.Find("Frame_Parent");
                    //    foreach (Transform frames in Frame_Parent.transform)
                    //    {
                    //        GameObject mesh_ch = frames.GetChild(0).gameObject;

                    //        mesh_ch.gameObject.layer = frame_layer;
                    //    }
                    //    foreach (Transform Div_ch in Divider_Parent.transform)
                    //    {
                    //        foreach (Transform frm_field_ch in Div_ch)
                    //        {


                    //            foreach (Transform divs_par in frm_field_ch)
                    //            {
                    //                GameObject div = null;
                    //                //if (divs_par.childCount > 0)
                    //                div = divs_par.gameObject;//.GetChild(0)
                    //                if (div != null)
                    //                {

                    //                    if (crown_names.Contains(frame_type))
                    //                    {
                    //                        float val = div.transform.localScale.y, step_cut_width = 0;
                    //                        bool step_cut = false;
                    //                        Vector3 dir = div.transform.up;
                    //                        Bounds bound_div_bar = div.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    //                        float max_depth = Mathf.Max(bound_div_bar.size.x, bound_div_bar.size.y, bound_div_bar.size.z);
                    //                        if (max_depth > 80)
                    //                        {
                    //                            //continuation
                    //                            GameObject type_q_pf = (GameObject)Resources.Load($"prefabs/{divider_type}_type1", typeof(GameObject));
                    //                            if (type_q_pf != null)
                    //                            {
                    //                                Bounds t1_bounds;//= type_q_pf.GetComponentInChildren<MeshFilter>().mesh.bounds;
                    //                                GameObject loc1 = div.transform.Find("Locator_1").gameObject;
                    //                                GameObject loc2 = div.transform.Find("Locator_2").gameObject;
                    //                                RaycastHit hit_frameRfield;

                    //                                //If frame is hit we can place the cut part of the divider for crown
                    //                                if (Physics.Raycast(div.transform.TransformPoint(bound_div_bar.center), (-div.transform.up), out hit_frameRfield, Mathf.Infinity, 1 << frame_layer.value))
                    //                                {
                    //                                    Debug.DrawRay(div.transform.TransformPoint(bound_div_bar.center), -div.transform.up * 10000, Color.blue, 20f);
                    //                                    if (hit_frameRfield.transform.parent != null)
                    //                                        if (hit_frameRfield.transform.parent.name.ToLower().Contains("frame") && !hit_frameRfield.transform.parent.name.ToLower().Contains("divider"))
                    //                                        {
                    //                                            GameObject t1 = GameObject.Instantiate(type_q_pf);
                    //                                            Follow follow = t1.AddComponent<Follow>();
                    //                                            t1.transform.Find("Locator").position = loc1.transform.position;
                    //                                            t1.transform.Find("Locator").rotation = loc1.transform.rotation;
                    //                                            follow.follow_child = t1.transform.Find("Locator");
                    //                                            t1.name = type_q_pf.name + "_div_1";
                    //                                            // follow.update_location_and_rotation();
                    //                                            follow.move_parent_relative_toChild();
                    //                                            //t1.transform.position=  div.transform.Find("Locator_1").transform.position;
                    //                                            t1.transform.parent = div.transform;

                    //                                            //rotate_around_center_rotateAround(t2, Vector3.up, 180);
                    //                                            t1_bounds = t1.GetComponentInChildren<MeshFilter>().mesh.bounds;
                    //                                            //step cut characteristics
                    //                                            step_cut = true;
                    //                                            step_cut_width += t1_bounds.size.y;

                    //                                        }
                    //                                }


                    //                                RaycastHit hit_frameRfield_2;
                    //                                //If frame is hit we can place the cut part of the divider for crown
                    //                                if (Physics.Raycast(div.transform.TransformPoint(bound_div_bar.center), (div.transform.up), out hit_frameRfield_2, Mathf.Infinity, 1 << frame_layer.value))
                    //                                {
                    //                                    Debug.DrawRay(div.transform.TransformPoint(bound_div_bar.center), div.transform.up * 10000, Color.red, 20f);
                    //                                    if (hit_frameRfield_2.transform.parent != null)
                    //                                        if (hit_frameRfield_2.transform.parent.name.ToLower().Contains("frame") && !hit_frameRfield_2.transform.parent.name.ToLower().Contains("divider"))
                    //                                        {

                    //                                            GameObject t2 = GameObject.Instantiate(type_q_pf);
                    //                                            Follow follow_2 = t2.AddComponent<Follow>();
                    //                                            t2.transform.Find("Locator").position = loc1.transform.position;
                    //                                            t2.transform.Find("Locator").rotation = loc1.transform.rotation;
                    //                                            follow_2.follow_child = t2.transform.Find("Locator");
                    //                                            follow_2.move_parent_relative_toChild();
                    //                                            t2.name = type_q_pf.name + "_div_2";
                    //                                            //rotate_around_center_rotateAround(t2, Vector3.up, 180);
                    //                                            //Bounds t2_bounds = t2.GetComponentInChildren<MeshFilter>().mesh.bounds;
                    //                                            t1_bounds = t2.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    //                                            t2.transform.Translate(div.transform.InverseTransformDirection(-div.transform.up) * (Mathf.Abs(val) + t1_bounds.size.y));
                    //                                            //follow.update_location_and_rotation();
                    //                                            //t2.transform.position=  div.transform.Find("Locator_2").transform.position;
                    //                                            t2.transform.parent = div.transform;
                    //                                            //step cut characteristics
                    //                                            step_cut = true;
                    //                                            step_cut_width += t1_bounds.size.y;
                    //                                        }
                    //                                }

                    //                                Characteristics chrs = div.gameObject.GetComponent<Characteristics>();


                    //                                if (chrs == null)
                    //                                {

                    //                                    chrs = div.gameObject.AddComponent<Characteristics>();

                    //                                }
                    //                                chrs.part_type = part_type_enum.vertical.ToString();
                    //                                chrs.part_name_id = divider_type;
                    //                                chrs.part_unique_id = Guid.NewGuid().ToString();
                    //                                if (step_cut == true)
                    //                                {
                    //                                    chrs.step_cut = step_cut;

                    //                                    chrs.step_cut_width = step_cut_width;
                    //                                }
                    //                            }

                    //                        }

                    //                    }

                    //                    var mc = div.gameObject.AddComponent<MeshCombiner>();
                    //                    mc.CreateMultiMaterialMesh = true;
                    //                    mc.DestroyCombinedChildren = true;
                    //                    mc.CombineMeshes(false);
                    //                    mc.transform.gameObject.AddComponent<BoxCollider>();
                    //                    mc.gameObject.layer = divider_layer;
                    //                    Probuilderize_gameObject(div.transform);
                    //                }
                    //            }
                    //        }
                    //    }
                    #endregion
                }
                catch (Exception divider_part_add)
                {

                    print("Divider Parts function call :" + divider_part_add);
                }



                try
                {
                    frame_C_clamp_groove_Alignment_();
                    #region Groove Alignment
                    //GameObject first_170000048_Front_0 = GameObject.Find("170000048_Front_0");

                    //if(first_170000048_Front_0!=null)
                    //{
                    //    GameObject first_170000048_Locator_groove = first_170000048_Front_0.transform.Find("Locator_groove").gameObject;




                    //    GameObject FrameC;// GameObject frame_C;
                    //    if (GameObject.Find("FrameC_0"))
                    //    {
                    //        FrameC = GameObject.Find("FrameC_0");
                    //    }
                    //    else
                    //    {
                    //        FrameC = GameObject.Find("FrameC");
                    //    }


                    //    GameObject FrameC_locator_groove = FrameC.transform.Find("Locator_groove").gameObject;

                    //    if (FrameC_locator_groove != null)
                    //    {
                    //        float dist = FrameC_locator_groove.transform.position.y - first_170000048_Locator_groove.transform.position.y;

                    //        print("Distance between Grooves : "+dist);

                    //        Pergola_Model = GameObject.Find("Pergola_Model");
                    //        foreach(Transform ch in Pergola_Model.transform)
                    //        {
                    //            if(!(ch.name.Contains("Frames_Parent")))//||ch.name.Contains("Wall_Parent")||ch.name.Contains("SupportBars_Parent")
                    //            {
                    //                print(ch.name+": pos ="+ch.position +"before");
                    //                ch.transform.Translate(Vector3.up * dist, Space.World);
                    //                // ch.transform.position = ch.transform.position + new Vector3(0, dist, 0);
                    //                print(ch.name+": pos ="+ch.position +"after");
                    //            }
                    //        }
                    //    }


                    //}
                    #endregion

                }
                catch (Exception move_dividers_groove)
                {

                    //throw;
                    print("Moving dividers up" + move_dividers_groove);
                }
                if (!string.IsNullOrEmpty(support_line_placement))
                {
                    #region SupportBar

                    if (support_line_placement == "full")
                        supportBarLengths = new SupportBarLengths(scale_b);
                    else supportBarLengths = new SupportBarLengths(manual_support_bar_distance);//scale_b * 0.666667f

                    SupportBars_Parent = new GameObject("SupportBars_Parent");
                    SupportBars_Parent.transform.parent = Pergola_Model.transform;
                    int supportbar_components_naming_counter = 1;
                    wall_on_side_C = wall_sides.Contains("C");



                    for (int i = 1; i <= frameDividers.numberOfDividerPoles + 2; i++)
                    {
                        GameObject SupportBar_Parent = new GameObject("SupportBar_Parent_" + i.ToString("D4"));
                        SupportBar_Parent.transform.parent = SupportBars_Parent.transform;

                        //Clamps on Frame
                        GameObject Clamp_onFrame_GO = null;
                        if (i == 1 || i == frameDividers.numberOfDividerPoles + 2)
                        {
                            Clamp_onFrame_GO = Instantiate(l_clamp_onFrame_full_Prefab, FieldDividers_Parent.transform);
                            characteristics_script = Clamp_onFrame_GO.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = "ALL.00.04_Full";
                        }
                        else
                        {
                            if (support_line_placement == "full")
                            {
                                Clamp_onFrame_GO = Instantiate(T_clamp_onFramePrefab, FieldDividers_Parent.transform);
                                characteristics_script = Clamp_onFrame_GO.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = t_clamp_onFrame;

                            }
                            else if (support_line_placement == "two_third" || support_line_placement == "manual")
                            {
                                Clamp_onFrame_GO = Instantiate(l_clamp_onFrame_twoThird_Prefab, FieldDividers_Parent.transform);
                                characteristics_script = Clamp_onFrame_GO.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = "ALL.00.04_TwoThird";
                            }
                        }

                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";

                        Clamp_onFrame_GO.transform.parent = SupportBar_Parent.transform;
                        if (wall_on_side_C)
                        {
                            if (i == 1)
                            {
                                Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, 0, 90 + 180);
                                Clamp_onFrame_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideA.transform.Find("Locator_Top").position.x,
                                                                                                    vertical_bar_sideB.transform.Find("Locator_Top").position.y,
                                                                                                    vertical_bar_sideB.transform.Find("Locator_Top").position.z);
                                Clamp_onFrame_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                if (support_line_placement == "two_third" || support_line_placement == "manual")
                                {
                                    Clamp_onFrame_GO.transform.Translate((scale_b - manual_support_bar_distance) * Vector3.up);
                                }
                            }
                            else if (i == frameDividers.numberOfDividerPoles + 2) //last clamp
                            {
                                Clamp_onFrame_GO.transform.GetComponent<Follow>().follow_child = Clamp_onFrame_GO.transform.Find("Locator_2").transform;
                                Clamp_onFrame_GO.transform.GetComponent<Follow>().update_location_and_rotation();
                                Clamp_onFrame_GO.transform.Find("Locator_2").localRotation = Quaternion.Euler(-90, 0, -270);
                                Clamp_onFrame_GO.transform.Find("Locator_2").position = new Vector3(vertical_bar_sideA.transform.Find("Locator_Top").position.x,
                                                                                                    vertical_bar_sideD.transform.Find("Locator_Top").position.y,
                                                                                                    vertical_bar_sideD.transform.Find("Locator_Top").position.z);
                                Clamp_onFrame_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();

                                if (support_line_placement == "two_third" || support_line_placement == "manual")
                                {
                                    Clamp_onFrame_GO.transform.Translate(-(scale_b - manual_support_bar_distance) * Vector3.up);
                                }
                            }
                            else
                            {
                                if (support_line_placement == "two_third" || support_line_placement == "manual")
                                    Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, 0, 270);
                                else
                                    Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(180, 0, -270);
                                Clamp_onFrame_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideA.transform.Find("Locator_Top").position.x,
                                                                                                    FrameDividers_Parent.transform.GetChild(i - 2).GetChild(0).transform.Find("Locator_Top").position.y,
                                                                                                    FrameDividers_Parent.transform.GetChild(i - 2).GetChild(0).transform.Find("Locator_Top").position.z);
                                Clamp_onFrame_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();

                                if (support_line_placement == "two_third" || support_line_placement == "manual")
                                    Clamp_onFrame_GO.transform.Translate((scale_b - manual_support_bar_distance) * Vector3.up);
                                else
                                {
                                    characteristics_script.part_name_id = t_clamp_onFrame;
                                }
                            }
                        }
                        else
                        {
                            if (i == 1)
                            {
                                Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, 0, 90);
                                Clamp_onFrame_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideC.transform.Find("Locator_Top").position.x,
                                                                                                    vertical_bar_sideD.transform.Find("Locator_Top").position.y,
                                                                                                    vertical_bar_sideD.transform.Find("Locator_Top").position.z);
                                Clamp_onFrame_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                if (support_line_placement == "two_third" || support_line_placement == "manual")
                                {
                                    Clamp_onFrame_GO.transform.Translate((scale_b - manual_support_bar_distance) * Vector3.up);
                                }
                            }
                            else if (i == frameDividers.numberOfDividerPoles + 2) //last clamp
                            {
                                Clamp_onFrame_GO.transform.GetComponent<Follow>().follow_child = Clamp_onFrame_GO.transform.Find("Locator_2").transform;
                                Clamp_onFrame_GO.transform.GetComponent<Follow>().update_location_and_rotation();
                                Clamp_onFrame_GO.transform.Find("Locator_2").localRotation = Quaternion.Euler(-90, 0, -90);
                                Clamp_onFrame_GO.transform.Find("Locator_2").position = new Vector3(vertical_bar_sideC.transform.Find("Locator_Top").position.x,
                                                                                                  vertical_bar_sideB.transform.Find("Locator_Top").position.y,
                                                                                                  vertical_bar_sideB.transform.Find("Locator_Top").position.z);
                                Clamp_onFrame_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();

                                if (support_line_placement == "two_third" || support_line_placement == "manual")
                                {
                                    Clamp_onFrame_GO.transform.Translate(-(scale_b - manual_support_bar_distance) * Vector3.up);
                                }
                            }
                            else
                            {
                                if (support_line_placement == "two_third" || support_line_placement == "manual")
                                    Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, 0, 90);
                                else
                                    Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(0, 0, -90);
                                Clamp_onFrame_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideC.transform.Find("Locator_Top").position.x,
                                                                                                    FrameDividers_Parent.transform.GetChild(i - 2).GetChild(0).transform.Find("Locator_Top").position.y,
                                                                                                    FrameDividers_Parent.transform.GetChild(i - 2).GetChild(0).transform.Find("Locator_Top").position.z);
                                Clamp_onFrame_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();

                                if (support_line_placement == "two_third" || support_line_placement == "manual")
                                    Clamp_onFrame_GO.transform.Translate((scale_b - manual_support_bar_distance) * Vector3.up);
                                else
                                {
                                    characteristics_script.part_name_id = t_clamp_onFrame;
                                }
                            }
                        }
                        Clamp_onFrame_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        Clamp_onFrame_GO.name = "Clamp_onFrame_" + supportbar_components_naming_counter.ToString("D4");


                        //Clamps on Wall
                        GameObject Clamp_onWall_GO = Instantiate(l_clamp_onWallPrefab, FieldDividers_Parent.transform);
                        Clamp_onWall_GO.transform.parent = SupportBar_Parent.transform;
                        Clamp_onWall_GO.transform.localPosition = new Vector3(0, Clamp_onFrame_GO.transform.localPosition.y, Clamp_onFrame_GO.transform.localPosition.z);
                        if (wall_on_side_C == false)
                            Clamp_onWall_GO.transform.Translate((-scale_b) * Vector3.right);




                        Clamp_onWall_GO.transform.Translate(supportBarLengths.supportWall_length * Vector3.up);

                        if (wall_on_side_C)
                            Clamp_onWall_GO.transform.localRotation = Quaternion.Euler(-90, 0, 90);
                        else
                            Clamp_onWall_GO.transform.localRotation = Quaternion.Euler(-90, 0, -90);

                        Clamp_onWall_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        Clamp_onWall_GO.name = "Clamp_onWall_" + supportbar_components_naming_counter.ToString("D4");

                        characteristics_script = Clamp_onWall_GO.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = l_clamp_onWall;//frameBar_prefab.name;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";

                        #region arranging clamp on wall according to clamp on frame bolt cube

                        //*****************this is to align clamps along their bolt cubes in a straight line*********************//

                        Follow follow_clamp_onWall = null;
                        if (Clamp_onWall_GO.GetComponent<Follow>() == null)
                        {
                            follow_clamp_onWall = Clamp_onWall_GO.AddComponent<Follow>();
                        }
                        else
                        {
                            follow_clamp_onWall = Clamp_onWall_GO.GetComponent<Follow>();
                        }
                        follow_clamp_onWall.follow_child = Clamp_onWall_GO.transform.GetChild(1);//Find("ALL.00.02_Cube");

                        follow_clamp_onWall.update_location_and_rotation();

                        Vector3 cube_bolt_clamp_on_frame_pos_wrt_Perg_mod_ = give_local_pos_wrt_Pergola_Model(Clamp_onFrame_GO.transform.GetChild(1).position);//Find("ALL.00.04_Cube")

                        Vector3 cube_bolt_clamp_on_wall_pos_wrt_Perg_mod_ = give_local_pos_wrt_Pergola_Model(Clamp_onWall_GO.transform.GetChild(1).position);

                        //giving the z component of cube_bolt_clamp_on_frame_pos_wrt_Perg_mod_ to  cube_bolt_clamp_on_wall_pos_wrt_Perg_mod_
                        cube_bolt_clamp_on_wall_pos_wrt_Perg_mod_ = new Vector3(cube_bolt_clamp_on_wall_pos_wrt_Perg_mod_.x, cube_bolt_clamp_on_wall_pos_wrt_Perg_mod_.y, cube_bolt_clamp_on_frame_pos_wrt_Perg_mod_.z);

                        follow_clamp_onWall.follow_child.transform.position = give_global_pos_wrt_Pergola_Model(cube_bolt_clamp_on_wall_pos_wrt_Perg_mod_);

                        //moving Wall clamp relative to the bolt cube
                        follow_clamp_onWall.move_parent_relative_toChild();

                        #endregion

                        //Telescope
                        GameObject telescope_GO = Instantiate(telescope_prefab, FieldDividers_Parent.transform);
                        telescope_GO.transform.parent = SupportBar_Parent.transform;
                        telescope_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        telescope_GO.name = "Telescope_" + supportbar_components_naming_counter.ToString("D4");
                        telescope_GO.transform.GetChild(0).GetChild(0).position = Clamp_onFrame_GO.transform.GetChild(1).position;
                        telescope_GO.transform.GetChild(0).GetChild(0).LookAt(Clamp_onWall_GO.transform.GetChild(1)); //this alligns forward vector towards target, we need UP vector to be aligned
                        telescope_GO.transform.GetChild(0).GetChild(0).RotateAround(telescope_GO.transform.GetChild(0).GetChild(0).position, telescope_GO.transform.GetChild(0).GetChild(0).right, 90);//aligning up vector towards target
                        telescope_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();

                        characteristics_script = telescope_GO.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = telescope;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";


                        //SupportBar_PartA
                        GameObject ConnectingBar_PartA_prefab = (GameObject)Resources.Load($"prefabs/supportBart_PartA", typeof(GameObject));
                        GameObject ConnectingBar_PartA_GO = Instantiate(ConnectingBar_PartA_prefab);
                        ConnectingBar_PartA_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        ConnectingBar_PartA_GO.transform.parent = SupportBar_Parent.transform;
                        ConnectingBar_PartA_GO.transform.GetChild(2).position = Clamp_onWall_GO.transform.GetChild(1).position;
                        ConnectingBar_PartA_GO.transform.GetChild(2).LookAt(Clamp_onFrame_GO.transform.GetChild(1));
                        ConnectingBar_PartA_GO.transform.GetChild(2).RotateAround(ConnectingBar_PartA_GO.transform.GetChild(2).position, ConnectingBar_PartA_GO.transform.GetChild(2).right, 90);
                        ConnectingBar_PartA_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();
                        ConnectingBar_PartA_GO.name = "ConnectingBar_PartA_" + supportbar_components_naming_counter.ToString("D4");
                        ConnectingBar_PartA_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        characteristics_script = ConnectingBar_PartA_GO.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = "supportBart_PartA";
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.none.ToString();
                        characteristics_script.part_group = "none";

                        //Suppport Bar
                        GameObject ConnectingBar_prefab = (GameObject)Resources.Load($"prefabs/supportBar", typeof(GameObject));
                        GameObject ConnectingBar_GO = Instantiate(ConnectingBar_prefab);
                        ConnectingBar_GO.transform.GetChild(0).gameObject.AddComponent<CapsuleCollider>();
                        ConnectingBar_GO.transform.parent = SupportBar_Parent.transform;
                        ConnectingBar_GO.transform.GetChild(1).position = ConnectingBar_PartA_GO.transform.GetChild(1).position;
                        ConnectingBar_GO.transform.GetChild(1).LookAt(Clamp_onFrame_GO.transform.GetChild(1));
                        ConnectingBar_GO.transform.GetChild(1).RotateAround(ConnectingBar_GO.transform.GetChild(1).position, ConnectingBar_GO.transform.GetChild(1).right, 90);
                        ConnectingBar_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();
                        Vector3 telescope_pos = telescope_GO.transform.Find("Locater_top").position;
                        Vector3 connecting_bar_pos = ConnectingBar_PartA_GO.transform.Find("Cube_supportBart_PartA_edge").position;
                        float dist = Vector3.Distance(connecting_bar_pos, telescope_pos);
                        ConnectingBar_GO.transform.localScale = new Vector3(1, dist, 1);
                        //ConnectingBar_GO.transform.localScale = new Vector3(1, supportBarLengths.supportBar_length - 89, 1); //removing 89mm for PartA supportBar
                        ConnectingBar_GO.name = "ConnectingBar_" + supportbar_components_naming_counter.ToString("D4");

                        characteristics_script = ConnectingBar_GO.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = "supportBar";
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                        characteristics_script.part_group = "Frame";

                        supportbar_components_naming_counter++;
                    }


                    if (side_b_wall == 1)
                    {
                        //DestroyImmediate(support_bar_nearest(SupportBars_Parent, GameObject.Find("FrameB"), Pergola_Model.transform.forward));
                        DestroyImmediate(GetClosest_SupportBar(SupportBars_Parent.transform, GameObject.Find("FrameB").transform));
                        //DestroyImmediate(SupportBars_Parent.transform.GetChild(SupportBars_Parent.transform.childCount - 1).gameObject);
                        GameObject L_Accesory_nearWall_GO = Instantiate(ak_79_L_accessory_prefab, Frames_Parent.transform.GetChild(1));
                        L_Accesory_nearWall_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        L_Accesory_nearWall_GO.transform.localScale = new Vector3(1, 1, scale_b);
                        L_Accesory_nearWall_GO.transform.localRotation = Quaternion.Euler(0, 90, -90);
                        L_Accesory_nearWall_GO.transform.Translate(Vector3.forward * (scale_c + 2), Space.World);
                        L_Accesory_nearWall_GO.transform.Translate(Vector3.right * 2);
                        L_Accesory_nearWall_GO.name = ak_79 + "_sideB";
                    }

                    if (side_d_wall == 1)
                    {
                        //DestroyImmediate(support_bar_nearest(SupportBars_Parent, GameObject.Find("FrameD"), Pergola_Model.transform.forward));
                        DestroyImmediate(GetClosest_SupportBar(SupportBars_Parent.transform, GameObject.Find("FrameD").transform));
                        //DestroyImmediate(SupportBars_Parent.transform.GetChild(0).gameObject);
                        GameObject L_Accesory_nearWall_GO = Instantiate(ak_79_L_accessory_prefab, Frames_Parent.transform.GetChild(1));
                        L_Accesory_nearWall_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        L_Accesory_nearWall_GO.transform.localScale = new Vector3(1, 1, scale_d);
                        L_Accesory_nearWall_GO.transform.localRotation = Quaternion.Euler(0, 90, 0);
                        L_Accesory_nearWall_GO.transform.Translate(-Vector3.forward * 2, Space.World);
                        L_Accesory_nearWall_GO.transform.Translate(-Vector3.up * 2, Space.World);
                        L_Accesory_nearWall_GO.name = ak_79 + "_sideD";
                    }

                    if (side_a_wall == 1)
                    {
                        GameObject L_Accesory_nearWall_GO = Instantiate(ak_79_L_accessory_prefab, Frames_Parent.transform.GetChild(1));
                        L_Accesory_nearWall_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        L_Accesory_nearWall_GO.transform.localScale = new Vector3(1, 1, scale_a);
                        L_Accesory_nearWall_GO.transform.localPosition = new Vector3(-(scale_b + 2), -2, 0);
                        L_Accesory_nearWall_GO.transform.localRotation = Quaternion.Euler(180, 0, 180);
                        L_Accesory_nearWall_GO.name = ak_79 + "_sideA";
                    }

                    if (side_c_wall == 1)
                    {
                        GameObject L_Accesory_nearWall_GO = Instantiate(ak_79_L_accessory_prefab, Frames_Parent.transform.GetChild(1));
                        L_Accesory_nearWall_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        L_Accesory_nearWall_GO.transform.localScale = new Vector3(1, 1, scale_c);
                        L_Accesory_nearWall_GO.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        L_Accesory_nearWall_GO.transform.Translate(Vector3.forward * scale_c);
                        L_Accesory_nearWall_GO.transform.Translate(Vector3.right * 2);
                        L_Accesory_nearWall_GO.transform.Translate(-Vector3.up * 2);
                        L_Accesory_nearWall_GO.name = ak_79 + "_sideC";
                    }

                    if (side_a_wall == 1 && side_b_wall == 1 && side_c_wall == 1 && side_d_wall == 1)
                    {
                        List<GameObject> supportBarList = new List<GameObject>();

                        foreach (Transform supportBar in SupportBars_Parent.transform)
                            supportBarList.Add(supportBar.gameObject);

                        foreach (GameObject supportBar in supportBarList)
                            DestroyImmediate(supportBar);
                    }

                    #endregion
                }
            }

            else if (Convert.ToInt32(dsElement_pergola_header.Tables[0].Rows[0]["element_shape_id"]) == 1)//L Shape
            {
                L_type = true;

                float scale_a = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[0]["side_value"]); //side A
                float scale_b = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[1]["side_value"]); //side B
                float scale_c = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[2]["side_value"]); //side C
                float scale_d = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[3]["side_value"]); //side D
                float scale_e = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[4]["side_value"]); //side E
                float scale_f = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[5]["side_value"]); //side F

                int side_a_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[0]["is_fixed_to_wall"]);
                int side_b_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[1]["is_fixed_to_wall"]);
                int side_c_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[2]["is_fixed_to_wall"]);
                int side_d_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[3]["is_fixed_to_wall"]);
                int side_e_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[4]["is_fixed_to_wall"]);
                int side_f_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[5]["is_fixed_to_wall"]);

                if (side_a_wall == 1)
                    wall_sides.Add("A");

                if (side_b_wall == 1)
                    wall_sides.Add("B");

                if (side_c_wall == 1)
                    wall_sides.Add("C");

                if (side_d_wall == 1)
                    wall_sides.Add("D");

                if (side_e_wall == 1)
                    wall_sides.Add("E");

                if (side_f_wall == 1)
                    wall_sides.Add("F");

                //assigning the total length of the frame to Global variables to be used in other scripts
                frame_A_length = scale_a;
                frame_B_length = scale_b;
                frame_C_length = scale_c;
                frame_D_length = scale_d;
                frame_E_length = scale_e;
                frame_F_length = scale_f;


                FrameDividers frameDividers = new FrameDividers(scale_c, framePartSettings, verticalPartSettings, MAX_SUPPORTBAR_DIVIDER_DISTANCE);

                if (!string.IsNullOrEmpty(dsElement_pergola_header.Tables[0].Rows[0]["support_line_placement_value"].ToString()))
                {
                    support_line_placement_value = float.Parse(dsElement_pergola_header.Tables[0].Rows[0]["support_line_placement_value"].ToString());
                }

                float manual_support_bar_distance = 200;// 0.666667f * scale_b;

                if (support_line_placement == "manual" || support_line_placement == "two_third")
                {

                    manual_support_bar_distance = support_line_placement_value;
                }

                #region Instantiation and positioning Frame
                GameObject vertical_bar_sideC = null, vertical_bar_sideB = null, vertical_bar_sideA = null, vertical_bar_sideD = null, vertical_bar_sideE = null, vertical_bar_sideF = null;
                Characteristics characteristics_script = null;

                Frame_Parent = new GameObject("Frame_Parent");
                Frame_Parent.transform.parent = Frames_Parent.transform;


                vertical_bar_sideD = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideD.transform.localPosition = new Vector3(0, 0, scale_d);
                vertical_bar_sideD.transform.localRotation = Quaternion.Euler(0, -90, 90);
                vertical_bar_sideD.transform.localScale = new Vector3(1, scale_d, 1);
                vertical_bar_sideD.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideD.name = "FrameD";
                characteristics_script = vertical_bar_sideD.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, 135";
                characteristics_script.right_end_cut_angle = "270, 0, -135";
                characteristics_script.icon_filename = "frame45-45.jpg";
                sliceGO_script.Slice_two_sides(vertical_bar_sideD, new Vector3(270, 0, 135), new Vector3(270, 0, -135));
                vertical_bar_sideD = Frame_Parent.transform.Find("FrameD").gameObject; //because original will be destroyed after slicing


                vertical_bar_sideC = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideC.transform.localPosition = new Vector3(-scale_c, 0, scale_d); //to offset
                vertical_bar_sideC.transform.localRotation = Quaternion.Euler(0, 180, 90);
                vertical_bar_sideC.transform.localScale = new Vector3(1, scale_c, 1);
                vertical_bar_sideC.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideC.name = "FrameC";
                characteristics_script = vertical_bar_sideC.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, -135";
                characteristics_script.right_end_cut_angle = "270, 0, 135";
                characteristics_script.icon_filename = "frameRight-4545.jpg";
                sliceGO_script.Slice_two_sides(vertical_bar_sideC, new Vector3(270, 0, -135), new Vector3(270, 0, 135));
                vertical_bar_sideC = Frame_Parent.transform.Find("FrameC").gameObject; //because original will be destroyed after slicing


                vertical_bar_sideB = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideB.transform.localPosition = new Vector3(-scale_c, 0, (scale_d - scale_b)); //to offset
                vertical_bar_sideB.transform.localRotation = Quaternion.Euler(0, -270, 90);
                vertical_bar_sideB.transform.localScale = new Vector3(1, scale_b + framePartSettings._part_depth, 1); //user feeds outside line, we need to add depth to get inside line
                vertical_bar_sideB.transform.Translate(-Vector3.forward * framePartSettings._part_depth, Space.World);
                vertical_bar_sideB.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideB.name = "FrameB";
                characteristics_script = vertical_bar_sideB.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, -135";
                characteristics_script.right_end_cut_angle = "270, 0, -135";
                characteristics_script.icon_filename = "frame4545.jpg";
                vertical_bar_sideB.transform.Find("Cube_left").Translate(Vector3.forward * framePartSettings._part_depth, Space.World);
                sliceGO_script.Slice_two_sides(vertical_bar_sideB, new Vector3(270, 0, -135), new Vector3(270, 0, -135));
                vertical_bar_sideB = Frame_Parent.transform.Find("FrameB").gameObject; //because original will be destroyed after slicing

                vertical_bar_sideA = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideA.transform.localPosition = new Vector3(-scale_e + framePartSettings._part_depth, 0, scale_f + framePartSettings._part_depth);
                vertical_bar_sideA.transform.localRotation = Quaternion.Euler(0, 180, 90);
                vertical_bar_sideA.transform.localScale = new Vector3(1, scale_a + framePartSettings._part_depth, 1);
                vertical_bar_sideA.transform.Translate(-Vector3.forward * framePartSettings._part_depth, Space.World);
                vertical_bar_sideA.transform.Translate(-Vector3.right * framePartSettings._part_depth, Space.World);
                vertical_bar_sideA.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideA.name = "FrameA";
                characteristics_script = vertical_bar_sideA.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, -135";
                characteristics_script.right_end_cut_angle = "270, 0, -135";
                characteristics_script.icon_filename = "frameLeft-4545.jpg";
                vertical_bar_sideA.transform.Find("Cube_right").Translate(-Vector3.right * framePartSettings._part_depth, Space.World);
                sliceGO_script.Slice_two_sides(vertical_bar_sideA, new Vector3(270, 0, -135), new Vector3(270, 0, -135));
                vertical_bar_sideA = Frame_Parent.transform.Find("FrameA").gameObject; //because original will be destroyed after slicing

                vertical_bar_sideF = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideF.transform.localPosition = new Vector3(-scale_e + framePartSettings._part_depth, 0, 0);
                vertical_bar_sideF.transform.localRotation = Quaternion.Euler(180, -90, -90);
                vertical_bar_sideF.transform.localScale = new Vector3(1, scale_f, 1);
                vertical_bar_sideF.transform.Translate(-Vector3.right * framePartSettings._part_depth, Space.World);
                vertical_bar_sideF.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideF.name = "FrameF";
                characteristics_script = vertical_bar_sideF.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, 135";
                characteristics_script.right_end_cut_angle = "270, 0, -135";
                characteristics_script.icon_filename = "frame-4545.jpg";
                sliceGO_script.Slice_two_sides(vertical_bar_sideF, new Vector3(270, 0, 135), new Vector3(270, 0, -135));
                vertical_bar_sideF = Frame_Parent.transform.Find("FrameF").gameObject; //because original will be destroyed after slicing

                vertical_bar_sideE = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideE.transform.localPosition = new Vector3(0, 0, 0);
                vertical_bar_sideE.transform.localRotation = Quaternion.Euler(0, 0, 90);
                vertical_bar_sideE.transform.localScale = new Vector3(1, scale_e, 1);
                vertical_bar_sideE.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideE.name = "FrameE";
                characteristics_script = vertical_bar_sideE.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, -135";
                characteristics_script.right_end_cut_angle = "270, 0, 135";
                characteristics_script.icon_filename = "frameLeft45-45.jpg";
                sliceGO_script.Slice_two_sides(vertical_bar_sideE, new Vector3(270, 0, -135), new Vector3(270, 0, 135));
                vertical_bar_sideE = Frame_Parent.transform.Find("FrameE").gameObject; //because original will be destroyed after slicing


                GameObject Accesories_Parent = new GameObject("Accesories_Parent");
                Accesories_Parent.transform.parent = Frames_Parent.transform;

                int inside_frame_accessories_count = 0;
                if (frame_type == "ak - 288") //80x80
                {
                    List<GameObject> accessories_together_spider_ak288 = new List<GameObject>();

                    GameObject Spider_crown_Left_Front_ = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -90);
                    Spider_crown_Left_Front_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_Left_Front_.name = spider_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_spider_ak288.Add(Spider_crown_Left_Front_);

                    GameObject Spider_crown_right_front = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_right_front.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right_front.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 180);
                    Spider_crown_right_front.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_front.name = spider_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_spider_ak288.Add(Spider_crown_right_front);

                    GameObject Spider_crown_right_top_ = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_right_top_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right_top_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 90);
                    Spider_crown_right_top_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_top_.name = spider_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_spider_ak288.Add(Spider_crown_right_top_);



                    GameObject Spider_crown_middle_rear = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_middle_rear.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_middle_rear.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_middle_rear.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_middle_rear.name = spider_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_spider_ak288.Add(Spider_crown_middle_rear);

                    GameObject Spider_crown_left_rear = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_left_rear.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_left_rear.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_left_rear.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_left_rear.name = spider_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_spider_ak288.Add(Spider_crown_left_rear);

                    GameObject Spider_crown_middle_bottom = Instantiate(Spider_accessory_v1_prefab, Accesories_Parent.transform);
                    Spider_crown_middle_bottom.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_middle_bottom.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -180);
                    Spider_crown_middle_bottom.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_middle_bottom.name = spider_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_spider_ak288.Add(Spider_crown_middle_bottom);

                    foreach (GameObject accessory in accessories_together_spider_ak288)
                    {
                        characteristics_script = accessory.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = spider_accessory;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";
                    }

                }
                else if (frame_type == "ak - 31a new" || frame_type == "32306") //Crown 150, 200
                {
                    List<GameObject> accessories_together = new List<GameObject>();
                    List<GameObject> accessories_together_spider = new List<GameObject>();
                    float Bottom_Rubber_Offset = -3.5f;
                    float l_angle_width = 25f;
                    float Adjustment = 0.1f;

                    GameObject left_Front_Bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    float L_Shape_Rubber_height = left_Front_Bottom.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.z;
                    left_Front_Bottom.transform.localPosition = new Vector3(-Adjustment, L_Shape_Rubber_height - Adjustment, Adjustment); //to offset
                    left_Front_Bottom.transform.localRotation = Quaternion.Euler(90, 0, 90);
                    left_Front_Bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    left_Front_Bottom.name = L_Rubber_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    left_Front_Bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(left_Front_Bottom);

                    GameObject left_Front_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    left_Front_top.transform.localPosition = new Vector3(-Adjustment, framePartSettings._part_width - Adjustment, Adjustment); //to offset
                    left_Front_top.transform.localRotation = Quaternion.Euler(90, 0, 90);
                    left_Front_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    left_Front_top.name = L_Rubber_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together.Add(left_Front_top);


                    GameObject right_Front_bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    right_Front_bottom.transform.localPosition = new Vector3(-Adjustment, L_Shape_Rubber_height - Adjustment, scale_d - Adjustment); //to offset
                    right_Front_bottom.transform.localRotation = Quaternion.Euler(90, 0, -180);
                    right_Front_bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    right_Front_bottom.name = L_Rubber_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    right_Front_bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(right_Front_bottom);

                    GameObject Right_Front_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Right_Front_top.transform.localPosition = new Vector3(-Adjustment, framePartSettings._part_width - Adjustment, scale_d - Adjustment); //to offset
                    Right_Front_top.transform.localRotation = Quaternion.Euler(90, 0, -180);
                    Right_Front_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Right_Front_top.name = L_Rubber_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together.Add(Right_Front_top);

                    GameObject right_rear_bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    right_rear_bottom.transform.localPosition = new Vector3(-scale_c + Adjustment, L_Shape_Rubber_height - Adjustment, scale_d - Adjustment); //to offset
                    right_rear_bottom.transform.localRotation = Quaternion.Euler(90, 0, -90);
                    right_rear_bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    right_rear_bottom.name = L_Rubber_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    right_rear_bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(right_rear_bottom);

                    GameObject Right_rear_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Right_rear_top.transform.localPosition = new Vector3(-scale_c + Adjustment, framePartSettings._part_width - Adjustment, scale_d - Adjustment); //to offset
                    Right_rear_top.transform.localRotation = Quaternion.Euler(90, 0, -90);
                    Right_rear_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Right_rear_top.name = L_Rubber_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together.Add(Right_rear_top);

                    GameObject middle_front_bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    middle_front_bottom.transform.localPosition = new Vector3(-scale_c + l_angle_width, L_Shape_Rubber_height - Adjustment, vertical_bar_sideA.transform.localPosition.z - l_angle_width); //to offset
                    middle_front_bottom.transform.localRotation = Quaternion.Euler(90, 0, 90);
                    middle_front_bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    middle_front_bottom.name = L_Rubber_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    middle_front_bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(middle_front_bottom);

                    GameObject Middle_front_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Middle_front_top.transform.localPosition = new Vector3(-scale_c + l_angle_width, framePartSettings._part_width - Adjustment, vertical_bar_sideA.transform.localPosition.z - l_angle_width); //to offset
                    Middle_front_top.transform.localRotation = Quaternion.Euler(90, 0, 90);
                    Middle_front_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Middle_front_top.name = L_Rubber_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together.Add(Middle_front_top);


                    GameObject middle_rear_bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    middle_rear_bottom.transform.localPosition = new Vector3(-scale_e + Adjustment, L_Shape_Rubber_height - Adjustment, vertical_bar_sideA.transform.localPosition.z - Adjustment); //to offset
                    middle_rear_bottom.transform.localRotation = Quaternion.Euler(90, 0, -90);
                    middle_rear_bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    middle_rear_bottom.name = L_Rubber_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    middle_rear_bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(middle_rear_bottom);

                    GameObject Middle_rear_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Middle_rear_top.transform.localPosition = new Vector3(-scale_e + Adjustment, framePartSettings._part_width - Adjustment, vertical_bar_sideA.transform.localPosition.z - Adjustment); //to offset
                    Middle_rear_top.transform.localRotation = Quaternion.Euler(90, 0, -90);
                    Middle_rear_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Middle_rear_top.name = L_Rubber_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together.Add(Middle_rear_top);

                    GameObject left_rear_bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    left_rear_bottom.transform.localPosition = new Vector3(-scale_e + Adjustment, L_Shape_Rubber_height - Adjustment, Adjustment); //to offset
                    left_rear_bottom.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    left_rear_bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    left_rear_bottom.name = L_Rubber_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    left_rear_bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(left_rear_bottom);

                    GameObject left_rear_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    left_rear_top.transform.localPosition = new Vector3(-scale_e + Adjustment, framePartSettings._part_width - Adjustment, Adjustment); //to offset
                    left_rear_top.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    left_rear_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    left_rear_top.name = L_Rubber_accessory + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together.Add(left_rear_top);


                    foreach (GameObject accessory in accessories_together)
                    {
                        characteristics_script = accessory.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = L_Rubber_accessory;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";
                    }


                    GameObject Spider_crown_Left_Front_ = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_TopInside_for_Spider").position.x - Adjustment,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.y - Adjustment,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.z - Adjustment);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -90);
                    Spider_crown_Left_Front_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_Left_Front_.name = spidertocrown + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_spider.Add(Spider_crown_Left_Front_);



                    GameObject Spider_crown_right_front = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_right_front.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.x + Adjustment,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.y - Adjustment,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.z - Adjustment);
                    Spider_crown_right_front.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 180);
                    Spider_crown_right_front.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_front.name = spidertocrown + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_spider.Add(Spider_crown_right_front);

                    GameObject Spider_crown_right_top_ = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_right_top_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.x + Adjustment,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.y - Adjustment,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.z + Adjustment);
                    Spider_crown_right_top_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 90);
                    Spider_crown_right_top_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_top_.name = spidertocrown + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_spider.Add(Spider_crown_right_top_);



                    GameObject Spider_crown_middle_rear = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_middle_rear.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.x - Adjustment,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.y - Adjustment,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.z + Adjustment);
                    Spider_crown_middle_rear.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_middle_rear.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_middle_rear.name = spidertocrown + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_spider.Add(Spider_crown_middle_rear);

                    GameObject Spider_crown_left_rear = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_left_rear.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_TopInside_for_Spider").position.x - Adjustment,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.y - Adjustment,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.z + Adjustment);
                    Spider_crown_left_rear.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_left_rear.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_left_rear.name = spidertocrown + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_spider.Add(Spider_crown_left_rear);


                    GameObject Spider_crown_middle_bottom = Instantiate(Spider_to_the_crown_v1_prefab, Accesories_Parent.transform);
                    Spider_crown_middle_bottom.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_middle_bottom.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -180);
                    //Spider_crown_middle_bottom.transform.GetChild(0).rotation =  Quaternion.Euler(0, 0, -180);
                    //Spider_crown_middle_bottom.transform.GetChild(2).GetChild(3).rotation = Quaternion.Euler(180, 0, 90);
                    //Spider_crown_middle_bottom.transform.GetChild(4).GetChild(5).rotation = Quaternion.Euler(0, 0, 180);
                    Spider_crown_middle_bottom.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_middle_bottom.name = spidertocrown + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_spider.Add(Spider_crown_middle_bottom);

                    foreach (GameObject accessory in accessories_together_spider)
                    {
                        characteristics_script = accessory.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = spidertocrown;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";
                    }
                }
                else if (frame_type == "ak - 279") //80x40
                {
                    List<GameObject> accessories_together_ak279 = new List<GameObject>();

                    GameObject Spider_crown_Left_Front_ = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -90);
                    Spider_crown_Left_Front_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_Left_Front_.name = spider + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_ak279.Add(Spider_crown_Left_Front_);

                    GameObject Spider_crown_right_front = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_crown_right_front.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right_front.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 180);
                    Spider_crown_right_front.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_front.name = spider + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_ak279.Add(Spider_crown_right_front);

                    GameObject Spider_crown_right_top_ = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_crown_right_top_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right_top_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 90);
                    Spider_crown_right_top_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_top_.name = spider + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_ak279.Add(Spider_crown_right_top_);



                    GameObject Spider_crown_middle_rear = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_crown_middle_rear.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_middle_rear.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_middle_rear.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_middle_rear.name = spider + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_ak279.Add(Spider_crown_middle_rear);

                    GameObject Spider_crown_left_rear = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_crown_left_rear.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_left_rear.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_left_rear.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_left_rear.name = spider + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_ak279.Add(Spider_crown_left_rear);

                    GameObject Spider_crown_middle_bottom = Instantiate(spider_v1_prefab, Accesories_Parent.transform);
                    Spider_crown_middle_bottom.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_middle_bottom.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -180);
                    Spider_crown_middle_bottom.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_middle_bottom.name = spider + "_" + (inside_frame_accessories_count++).ToString("D4");
                    accessories_together_ak279.Add(Spider_crown_middle_bottom);

                    foreach (GameObject accessory in accessories_together_ak279)
                    {
                        characteristics_script = accessory.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = spider;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";
                    }
                }

                foreach (Transform accs in Accesories_Parent.transform)
                {
                    if (!Accessories_name.Contains(accs.name))
                        Accessories_name.Add(accs.name);
                }
                #endregion

                RaycastHit hit_BtoD = new RaycastHit();
                RaycastHit hit_AtoE = new RaycastHit();
                RaycastHit hit_DividertoE = new RaycastHit();
                RaycastHit hit_DividertoC = new RaycastHit();
                RaycastHit hit_DividertoF = new RaycastHit();
                int total_number_of_fields_DividertoE = 1;
                int total_number_of_fields_DividertoC = 0;
                int total_number_of_fields_DividertoF = 0;
                float each_inner_field_width_fieldDividertoF = 0;
                float each_inner_field_width_fieldDividertoC = 0;
                float each_inner_field_width_fieldDividertoE = 0;
                int Number_of_dividers_in_section_2 = 0;
                int Number_of_dividers_in_section_3 = 0;

                int supportBarCount_in_Region1 = 0;
                int supportBarCount_in_Region2 = 0;


                if (rafafa_placement_type == "type_2")
                {
                    #region Divider

                    Divider_Parent = new GameObject("Divider_Parent");
                    Divider_Parent.transform.parent = Pergola_Model.transform;

                    FrameDividers_Parent = new GameObject("FrameDividers_Parent");
                    FrameDividers_Parent.transform.parent = Divider_Parent.transform;

                    FrameDividers_Parent_Section1 = new GameObject("FrameDividers_Parent_Section_0001");
                    FrameDividers_Parent_Section1.transform.parent = FrameDividers_Parent.transform;

                    FrameDividers_Parent_Section2 = new GameObject("FrameDividers_Parent_Section_0002");
                    FrameDividers_Parent_Section2.transform.parent = FrameDividers_Parent.transform;


                    GameObject frame_B = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameB")) //TODO: here check if frame is sliced, then get the first or last sliced part accordingly (FrameB_0 or FrameB_n)
                            frame_B = frame.gameObject;
                    }
                    Bounds frame_B_bound = Calculate_b(frame_B.transform);
                    Vector3 global_center_Frame_B = Pergola_Model.transform.TransformPoint(frame_B_bound.center);
                    Vector3 ray_cast_dir = Pergola_Model.transform.TransformDirection(Pergola_Model.transform.right);
                    Vector3 top_point_frame_B = global_center_Frame_B + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, frame_B_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_B, ray_cast_dir, out hit_BtoD, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_B, ray_cast_dir * hit_BtoD.distance, Color.yellow);
                        Debug.Log($"Did Hit & Distance={hit_BtoD.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_B, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("Did not Hit");
                    }

                    GameObject frame_A = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameA"))
                            frame_A = frame.gameObject;
                    }
                    Bounds frame_A_bound = Calculate_b(frame_A.transform);
                    Vector3 global_center_Frame_A = Pergola_Model.transform.TransformPoint(frame_A_bound.center);
                    ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.forward);
                    Vector3 top_point_frame_A = global_center_Frame_A + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, frame_A_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_A, ray_cast_dir, out hit_AtoE, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_A, ray_cast_dir * hit_AtoE.distance, Color.yellow);
                        Debug.Log($"Did Hit & Distance={hit_BtoD.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_A, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("Did not Hit");
                    }


                    //Frame Divider Code
                    float dividerPoleDistance = frameDividers.each_field_width + framePartSettings._part_depth;
                    int clamp_naming_counter = 0;

                    for (int i = 0; i < 2; i++)
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + i.ToString("D4"));
                        if (i == 0)
                            frameDivider_GO_parent.transform.parent = FrameDividers_Parent_Section1.transform;
                        if (i == 1)
                            frameDivider_GO_parent.transform.parent = FrameDividers_Parent_Section2.transform;

                        GameObject frameDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        frameDivider_GO.transform.localPosition = vertical_bar_sideB.transform.localPosition; //Todo add(offset) frame B or D width to each field width
                        if (i == 0)
                        {
                            frameDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            frameDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                            frameDivider_GO.transform.Translate(Vector3.right * (hit_BtoD.distance + framePartSettings._part_depth), Space.World);
                        }
                        else if (i == 1)
                        {
                            frameDivider_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                            frameDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                            frameDivider_GO.transform.Translate(Vector3.right * framePartSettings._part_depth, Space.World);
                        }

                        frameDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        frameDivider_GO.name = "FrameDivider_" + i.ToString("D4");
                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = frameDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = frameDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Translate(frameDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }


                    }

                    // characteristics
                    int framecounttype2 = 0;
                    foreach (Transform t in FrameDividers_Parent.transform)
                    {

                        foreach (Transform child_t in t)
                        {
                            characteristics_script = child_t.gameObject.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = clampfordivider;
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_group = "Frame";

                            if (child_t.name.Contains("FrameDivider"))
                            {
                                characteristics_script.part_name_id = divider_type;
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                            }

                            if (framecounttype2 == 0)
                            {
                                characteristics_script.part_group = "Frame";
                            }
                            else if (framecounttype2 == 1)
                            {
                                characteristics_script.part_group = "Frame";
                            }
                            framecounttype2++;
                        }

                    }


                    //Field Divider Code
                    FieldDividers_Parent = new GameObject("FieldDividers_Parent");
                    FieldDividers_Parent.transform.parent = Divider_Parent.transform;

                    FieldDividers_Parent_Section1 = new GameObject("FieldDividers_Parent_Section_0001");
                    FieldDividers_Parent_Section1.transform.parent = FieldDividers_Parent.transform;

                    FieldDividers_Parent_Section2 = new GameObject("FieldDividers_Parent_Section_0002");
                    FieldDividers_Parent_Section2.transform.parent = FieldDividers_Parent.transform;

                    int FrameDiviider_naming_count = 2;

                    int divider_naming_counter = 0;
                    GameObject FrameDivider_0 = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0001").Find("FrameDivider_Parent_0000").Find("FrameDivider_0000").gameObject;
                    Bounds FrameDivider_0_bound = Calculate_b(FrameDivider_0.transform);
                    Vector3 global_center_FrameDivider_0 = Pergola_Model.transform.TransformPoint(FrameDivider_0_bound.center);
                    ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.forward);
                    Vector3 top_point_FrameDivider_0 = global_center_FrameDivider_0 + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, FrameDivider_0_bound.size)) / 2;
                    if (Physics.Raycast(top_point_FrameDivider_0, ray_cast_dir, out hit_DividertoE, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * hit_DividertoE.distance, Color.yellow);
                        Debug.Log($"hit_DividertoE  Did Hit & Distance={hit_DividertoE.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * 10000, Color.white, Mathf.Infinity);
                        Debug.Log("hit_DividertoE  Did not Hit");
                    }

                    FrameDividersForL frameDividersForL = new FrameDividersForL(hit_DividertoE.distance, framePartSettings, verticalPartSettings);
                    float inner_dividerPoleDistance = framePartSettings._part_depth + frameDividersForL.each_field_width;

                    for (int j = 0; j < frameDividersForL.numberOfDividerPoles; j++)  //region 1
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + FrameDiviider_naming_count.ToString("D4"));
                        frameDivider_GO_parent.transform.parent = FrameDividers_Parent_Section1.transform;

                        GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        fieldDivider_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, inner_dividerPoleDistance); //Todo add(offset) frame B or D width to each field width
                        fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        fieldDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                        fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        fieldDivider_GO.name = "FrameDivider_" + FrameDiviider_naming_count++.ToString("D4");
                        inner_dividerPoleDistance += (frameDividersForL.each_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }
                    total_number_of_fields_DividertoE = frameDividersForL.numbefOfFields;
                    each_inner_field_width_fieldDividertoE = frameDividersForL.each_field_width;

                    InnerFieldDividers innerFieldDividers = new InnerFieldDividers(frameDividersForL.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                    inner_dividerPoleDistance = framePartSettings._part_depth + innerFieldDividers.each_inner_field_width;
                    region1_fields_count = frameDividersForL.numbefOfFields * innerFieldDividers.numbefOfInnerFields;
                    for (int i = 0; i < frameDividersForL.numbefOfFields; i++)
                    {
                        for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                        {
                            GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + divider_naming_counter.ToString("D4"));
                            fieldDivider_GO_parent.transform.parent = FieldDividers_Parent_Section1.transform;

                            GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);
                            fieldDivider_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, inner_dividerPoleDistance); //Todo add(offset) frame B or D width to each field width
                            fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            fieldDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                            fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            fieldDivider_GO.name = "FieldDivider_" + divider_naming_counter++.ToString("D4");
                            inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                            if (divider_type == "ak - 279")
                            {
                                GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                                C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                                C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                                C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                                if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                    Accessories_name.Add(C_Clamp_ForDivider.name);

                                GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                                C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                                C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                                C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                                if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                    Accessories_name.Add((C_Clamp_ForDivider_2.name));
                            }

                        }
                        inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                    }

                    ray_cast_dir = Pergola_Model.transform.TransformDirection(Pergola_Model.transform.forward);
                    top_point_FrameDivider_0 = global_center_FrameDivider_0 + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, FrameDivider_0_bound.size)) / 2;
                    if (Physics.Raycast(top_point_FrameDivider_0, ray_cast_dir, out hit_DividertoC, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * hit_DividertoC.distance, Color.yellow);
                        Debug.Log($"hit_DividertoC  Did Hit & Distance={hit_DividertoC.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("hit_DividertoC  Did not Hit");
                    }

                    //**************************************
                    frameDividersForL = new FrameDividersForL(hit_DividertoC.distance, framePartSettings, verticalPartSettings);
                    inner_dividerPoleDistance = vertical_bar_sideB.transform.localPosition.z + frameDividersForL.each_field_width + verticalPartSettings._part_depth;
                    supportBarCount_in_Region1 = frameDividersForL.numberOfDividerPoles;

                    Number_of_dividers_in_section_2 = frameDividersForL.numberOfDividerPoles;
                    for (int j = 0; j < frameDividersForL.numberOfDividerPoles; j++)
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + FrameDiviider_naming_count.ToString("D4"));
                        frameDivider_GO_parent.transform.parent = FrameDividers_Parent_Section1.transform;

                        GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        fieldDivider_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, inner_dividerPoleDistance); //Todo add(offset) frame B or D width to each field width
                        fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        fieldDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                        fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        fieldDivider_GO.name = "FrameDivider_" + FrameDiviider_naming_count++.ToString("D4");
                        inner_dividerPoleDistance += (frameDividersForL.each_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }
                    total_number_of_fields_DividertoC = frameDividersForL.numbefOfFields;
                    each_inner_field_width_fieldDividertoC = frameDividersForL.each_field_width;

                    innerFieldDividers = new InnerFieldDividers(frameDividersForL.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                    inner_dividerPoleDistance = vertical_bar_sideB.transform.localPosition.z + innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth;
                    region2_fields_count = frameDividersForL.numbefOfFields * innerFieldDividers.numbefOfInnerFields;
                    for (int i = 0; i < frameDividersForL.numbefOfFields; i++)
                    {
                        for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                        {
                            GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + divider_naming_counter.ToString("D4"));
                            fieldDivider_GO_parent.transform.parent = FieldDividers_Parent_Section1.transform;

                            GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);
                            fieldDivider_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, inner_dividerPoleDistance); //Todo add(offset) frame B or D width to each field width
                            fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            fieldDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                            fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            fieldDivider_GO.name = "FieldDivider_" + divider_naming_counter++.ToString("D4");
                            inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                            if (divider_type == "ak - 279")
                            {
                                GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                                C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                                C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                                C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                                if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                    Accessories_name.Add(C_Clamp_ForDivider.name);

                                GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                                C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                                C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                                C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                                if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                    Accessories_name.Add((C_Clamp_ForDivider_2.name));
                            }

                        }
                        inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                    }

                    GameObject FrameDivider_1 = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").gameObject;
                    Bounds FrameDivider_1_bound = Calculate_b(FrameDivider_1.transform);
                    Vector3 global_center_FrameDivider_1 = Pergola_Model.transform.TransformPoint(FrameDivider_1_bound.center);
                    ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.right);
                    Vector3 top_point_FrameDivider_1 = global_center_FrameDivider_1 + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, FrameDivider_1_bound.size)) / 2;
                    if (Physics.Raycast(top_point_FrameDivider_1, ray_cast_dir, out hit_DividertoF, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_FrameDivider_1, ray_cast_dir * hit_DividertoF.distance, Color.yellow);
                        Debug.Log($"hit_DividertoF  Did Hit & Distance={hit_DividertoF.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_FrameDivider_1, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("hit_DividertoF  Did not Hit");
                    }
                    //*****************************


                    frameDividersForL = new FrameDividersForL(hit_DividertoF.distance, framePartSettings, verticalPartSettings);
                    inner_dividerPoleDistance = -(FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition.x - frameDividersForL.each_field_width - verticalPartSettings._part_depth);
                    Number_of_dividers_in_section_3 = frameDividersForL.numberOfDividerPoles;
                    supportBarCount_in_Region2 = frameDividersForL.numberOfDividerPoles;

                    for (int j = 0; j < frameDividersForL.numberOfDividerPoles; j++)
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + FrameDiviider_naming_count.ToString("D4"));
                        frameDivider_GO_parent.transform.parent = FrameDividers_Parent_Section2.transform;

                        GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                        fieldDivider_GO.transform.localPosition = new Vector3(-inner_dividerPoleDistance, 0, framePartSettings._part_depth);
                        fieldDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                        fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        fieldDivider_GO.transform.Translate(-Vector3.right * verticalPartSettings._part_depth, Space.World);
                        fieldDivider_GO.name = "FrameDivider_" + FrameDiviider_naming_count++.ToString("D4");
                        inner_dividerPoleDistance += (frameDividersForL.each_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }

                    total_number_of_fields_DividertoF = frameDividersForL.numbefOfFields;
                    each_inner_field_width_fieldDividertoF = frameDividersForL.each_field_width;

                    innerFieldDividers = new InnerFieldDividers(frameDividersForL.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                    inner_dividerPoleDistance = -(FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition.x - innerFieldDividers.each_inner_field_width - (verticalPartSettings._part_depth * 2));
                    //inner_dividerPoleDistance = -(-vertical_bar_sideB.transform.localPosition.z - innerFieldDividers.each_inner_field_width - verticalPartSettings._part_depth * 2);
                    region3_fields_count = frameDividersForL.numbefOfFields * innerFieldDividers.numbefOfInnerFields;

                    for (int i = 0; i < frameDividersForL.numbefOfFields; i++)
                    {
                        for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                        {
                            GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + divider_naming_counter.ToString("D4"));
                            fieldDivider_GO_parent.transform.parent = FieldDividers_Parent_Section2.transform;
                            GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);

                            fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                            //  fieldDivider_GO.transform.localPosition = new Vector3((vertical_bar_sideB.transform.localPosition.x)+framePartSettings._part_depth, 0, framePartSettings._part_depth);
                            fieldDivider_GO.transform.localPosition = new Vector3(-inner_dividerPoleDistance, 0, framePartSettings._part_depth);
                            fieldDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                            fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            //fieldDivider_GO.transform.Translate(-Vector3.right * verticalPartSettings._part_depth, Space.World);
                            fieldDivider_GO.name = "FieldDivider_" + divider_naming_counter++.ToString("D4");
                            inner_dividerPoleDistance += innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth;


                            if (divider_type == "ak - 279")
                            {
                                GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                                C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                                C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                                C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                                if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                    Accessories_name.Add(C_Clamp_ForDivider.name);

                                GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                                C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                                C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                                C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                                if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                    Accessories_name.Add((C_Clamp_ForDivider_2.name));
                            }

                        }
                        inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                    }

                    int sec1count = 0;
                    foreach (Transform t in FrameDividers_Parent_Section1.transform)
                    {
                        if (sec1count == 0)
                        {

                        }
                        else
                        {
                            characteristics_script = t.gameObject.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = clampfordivider;
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_group = "Frame";
                            if (t.name.Contains("FrameDivider"))
                            {
                                characteristics_script.part_name_id = divider_type;
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                            }
                        }
                        sec1count++;
                    }
                    int sec2count = 0;
                    foreach (Transform t in FrameDividers_Parent_Section2.transform)
                    {
                        if (sec2count == 0)
                        {

                        }
                        else
                        {
                            characteristics_script = t.gameObject.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = clampfordivider;
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_group = "Frame";
                            if (t.name.Contains("FrameDivider"))
                            {
                                characteristics_script.part_name_id = divider_type;
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                            }
                        }
                        sec2count++;

                    }


                    foreach (Transform t in FieldDividers_Parent_Section1.transform)
                    {


                        characteristics_script = t.gameObject.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = clampfordivider;
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_group = "Frame";
                        if (t.name.Contains("FieldDivider"))
                        {
                            characteristics_script.part_name_id = divider_type;
                            characteristics_script.part_type = part_type_enum.vertical.ToString();
                        }

                    }

                    foreach (Transform t in FieldDividers_Parent_Section2.transform)
                    {

                        characteristics_script = t.gameObject.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = clampfordivider;
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_group = "Frame";
                        if (t.name.Contains(""))
                        {
                            characteristics_script.part_name_id = divider_type;
                            characteristics_script.part_type = part_type_enum.vertical.ToString();
                        }

                    }

                    #endregion
                }
                else if (rafafa_placement_type == "type_3")
                {
                    #region Divider Inverted 

                    Divider_Parent = new GameObject("Divider_Parent");
                    Divider_Parent.transform.parent = Pergola_Model.transform;

                    FrameDividers_Parent = new GameObject("FrameDividers_Parent");
                    FrameDividers_Parent.transform.parent = Divider_Parent.transform;

                    FrameDividers_Parent_Section1 = new GameObject("FrameDividers_Parent_Section_0001");
                    FrameDividers_Parent_Section1.transform.parent = FrameDividers_Parent.transform;

                    FrameDividers_Parent_Section2 = new GameObject("FrameDividers_Parent_Section_0002");
                    FrameDividers_Parent_Section2.transform.parent = FrameDividers_Parent.transform;

                    //RaycastHit hit_BtoD;
                    GameObject frame_B = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameB")) //TODO: here check if frame is sliced, then get the first or last sliced part accordingly (FrameB_0 or FrameB_n)
                            frame_B = frame.gameObject;
                    }
                    Bounds frame_B_bound = Calculate_b(frame_B.transform);
                    Vector3 global_center_Frame_B = Pergola_Model.transform.TransformPoint(frame_B_bound.center);
                    Vector3 ray_cast_dir = Pergola_Model.transform.TransformDirection(Pergola_Model.transform.right);
                    Vector3 top_point_frame_B = global_center_Frame_B + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, frame_B_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_B, ray_cast_dir, out hit_BtoD, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_B, ray_cast_dir * hit_BtoD.distance, Color.yellow);
                        Debug.Log($"Did Hit & Distance={hit_BtoD.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_B, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("Did not Hit");
                    }

                    //RaycastHit hit_AtoE;
                    GameObject frame_A = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameA"))
                            frame_A = frame.gameObject;
                    }

                    Bounds frame_A_bound = Calculate_b(frame_A.transform);
                    Vector3 global_center_Frame_A = Pergola_Model.transform.TransformPoint(frame_A_bound.center);
                    ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.forward);
                    Vector3 top_point_frame_A = global_center_Frame_A + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, frame_A_bound.size)) / 2;
                    if (Physics.Raycast(top_point_frame_A, ray_cast_dir, out hit_AtoE, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_A, ray_cast_dir * hit_AtoE.distance, Color.yellow);
                        Debug.Log($"Did Hit & Distance={hit_BtoD.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_A, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("Did not Hit");
                    }


                    //Frame Divider Code
                    float dividerPoleDistance = frameDividers.each_field_width + framePartSettings._part_depth;
                    int clamp_naming_counter = 0;

                    for (int i = 0; i < 2; i++)
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + i.ToString("D4"));
                        if (i == 0)
                            frameDivider_GO_parent.transform.parent = FrameDividers_Parent_Section1.transform;
                        if (i == 1)
                            frameDivider_GO_parent.transform.parent = FrameDividers_Parent_Section2.transform;

                        GameObject frameDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        frameDivider_GO.transform.localPosition = vertical_bar_sideB.transform.localPosition; //Todo add(offset) frame B or D width to each field width
                        if (i == 0)
                        {
                            frameDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            frameDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                            frameDivider_GO.transform.Translate(Vector3.right * (hit_BtoD.distance + framePartSettings._part_depth), Space.World);
                        }
                        else if (i == 1)
                        {
                            frameDivider_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                            frameDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                            frameDivider_GO.transform.Translate(Vector3.right * framePartSettings._part_depth, Space.World);
                        }

                        frameDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        frameDivider_GO.name = "FrameDivider_" + i.ToString("D4");
                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = frameDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = frameDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Translate(frameDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }

                    }
                    int temp_frameCounter = 0;
                    // characteristics
                    foreach (Transform t in FrameDividers_Parent.transform)
                    {

                        foreach (Transform child_t in t)
                        {
                            characteristics_script = child_t.gameObject.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = clampfordivider;
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_group = "Frame";

                            if (child_t.name.Contains("FrameDivider"))
                            {
                                characteristics_script.part_name_id = divider_type;
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                            }

                            if (temp_frameCounter == 0)
                            {
                                characteristics_script.part_group = "Frame";

                            }
                            else if (temp_frameCounter == 1)
                            {
                                characteristics_script.part_group = "Frame";
                            }
                            temp_frameCounter++;
                        }

                    }


                    //Field Divider Code
                    FieldDividers_Parent = new GameObject("FieldDividers_Parent");
                    FieldDividers_Parent.transform.parent = Divider_Parent.transform;

                    FieldDividers_Parent_Section1 = new GameObject("FieldDividers_Parent_Section_0001");
                    FieldDividers_Parent_Section1.transform.parent = FieldDividers_Parent.transform;

                    FieldDividers_Parent_Section2 = new GameObject("FieldDividers_Parent_Section_0002");
                    FieldDividers_Parent_Section2.transform.parent = FieldDividers_Parent.transform;

                    int divider_naming_counter = 2;
                    int Field_divider_naming_count = 0;

                    GameObject FrameDivider_1 = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").gameObject;
                    Bounds FrameDivider_1_bound = Calculate_b(FrameDivider_1.transform);
                    Vector3 global_center_FrameDivider_1 = Pergola_Model.transform.TransformPoint(FrameDivider_1_bound.center);
                    ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.right);
                    Vector3 top_point_FrameDivider_1 = global_center_FrameDivider_1 + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, FrameDivider_1_bound.size)) / 2;
                    if (Physics.Raycast(top_point_FrameDivider_1, ray_cast_dir, out hit_DividertoF, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_FrameDivider_1, ray_cast_dir * hit_DividertoF.distance, Color.yellow);
                        Debug.Log($"hit_DividertoF  Did Hit & Distance={hit_DividertoF.distance}");
                    } //section 3
                    else
                    {
                        Debug.DrawRay(top_point_FrameDivider_1, ray_cast_dir * 10000, Color.white, Mathf.Infinity);
                        Debug.Log("hit_DividertoF  Did not Hit");
                    }

                    FrameDividersForL frameDividersForL = new FrameDividersForL(hit_DividertoF.distance, framePartSettings, verticalPartSettings);
                    float inner_dividerPoleDistance = -(FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition.x - frameDividersForL.each_field_width - verticalPartSettings._part_depth);
                    Number_of_dividers_in_section_3 = frameDividersForL.numberOfDividerPoles;
                    supportBarCount_in_Region2 = frameDividersForL.numberOfDividerPoles;

                    for (int j = 0; j < frameDividersForL.numberOfDividerPoles; j++)
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + divider_naming_counter.ToString("D4"));
                        frameDivider_GO_parent.transform.parent = FrameDividers_Parent_Section2.transform;

                        GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                        fieldDivider_GO.transform.localPosition = new Vector3(-inner_dividerPoleDistance, 0, framePartSettings._part_depth);
                        fieldDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                        fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        fieldDivider_GO.transform.Translate(-Vector3.right * verticalPartSettings._part_depth, Space.World);
                        fieldDivider_GO.name = "FrameDivider_" + divider_naming_counter++.ToString("D4");
                        inner_dividerPoleDistance += (frameDividersForL.each_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }


                    InnerFieldDividers innerFieldDividers = new InnerFieldDividers(frameDividersForL.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                    inner_dividerPoleDistance = -(FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition.x - (verticalPartSettings._part_depth * 2) - innerFieldDividers.each_inner_field_width);
                    // float Offser = vertical_bar_sideB.transform.localPosition.z + verticalPartSettings._part_depth;
                    //inner_dividerPoleDistance = innerFieldDividers.each_inner_field_width  + verticalPartSettings._part_depth ;
                    region3_fields_count = frameDividersForL.numbefOfFields * innerFieldDividers.numbefOfInnerFields;
                    for (int i = 0; i < frameDividersForL.numbefOfFields; i++)
                    {
                        for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                        {
                            GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + Field_divider_naming_count.ToString("D4"));
                            fieldDivider_GO_parent.transform.parent = FieldDividers_Parent_Section2.transform;

                            GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);
                            fieldDivider_GO.transform.localPosition = new Vector3(-inner_dividerPoleDistance, 0, framePartSettings._part_depth); //Todo add(offset) frame B or D width to each field width
                            fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                            fieldDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                            fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            fieldDivider_GO.name = "FieldDivider_" + Field_divider_naming_count++.ToString("D4");
                            inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                            if (divider_type == "ak - 279")
                            {
                                GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                                C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                                C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                                C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                                if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                    Accessories_name.Add(C_Clamp_ForDivider.name);

                                GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                                C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                                C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                                C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                                if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                    Accessories_name.Add((C_Clamp_ForDivider_2.name));
                            }
                        }  //section 3 loop  divider to F

                        inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                    }

                    total_number_of_fields_DividertoC = innerFieldDividers.numbefOfInnerFields;
                    each_inner_field_width_fieldDividertoC = innerFieldDividers.each_inner_field_width;


                    GameObject FrameDivider_0 = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0001").Find("FrameDivider_Parent_0000").Find("FrameDivider_0000").gameObject;
                    Bounds FrameDivider_0_bound = Calculate_b(FrameDivider_0.transform);
                    Vector3 global_center_FrameDivider_0 = Pergola_Model.transform.TransformPoint(FrameDivider_0_bound.center);
                    ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.forward);
                    Vector3 top_point_FrameDivider_0 = global_center_FrameDivider_0 + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, FrameDivider_0_bound.size)) / 2;
                    if (Physics.Raycast(top_point_FrameDivider_0, ray_cast_dir, out hit_DividertoE, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * hit_DividertoE.distance, Color.yellow);
                        Debug.Log($"hit_DividertoE  Did Hit & Distance={hit_DividertoE.distance}");
                    }  // section 1
                    else
                    {
                        Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * 10000, Color.white, Mathf.Infinity);
                        Debug.Log("hit_DividertoE  Did not Hit");
                    }

                    frameDividersForL = new FrameDividersForL(hit_BtoD.distance, framePartSettings, verticalPartSettings);
                    inner_dividerPoleDistance = -frameDividersForL.each_field_width - framePartSettings._part_depth;
                    for (int j = 0; j < frameDividersForL.numberOfDividerPoles; j++)
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + divider_naming_counter.ToString("D4"));
                        frameDivider_GO_parent.transform.parent = FrameDividers_Parent_Section2.transform;

                        GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                        fieldDivider_GO.transform.localPosition = new Vector3(inner_dividerPoleDistance, 0, framePartSettings._part_depth);
                        fieldDivider_GO.transform.localScale = new Vector3(1, hit_DividertoE.distance, 1);
                        fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        fieldDivider_GO.transform.Translate(-Vector3.right * verticalPartSettings._part_depth, Space.World);
                        fieldDivider_GO.name = "FrameDivider_" + divider_naming_counter++.ToString("D4");
                        inner_dividerPoleDistance += (frameDividersForL.each_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }

                    innerFieldDividers = new InnerFieldDividers(frameDividersForL.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                    inner_dividerPoleDistance = framePartSettings._part_depth + innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth;
                    region1_fields_count = frameDividersForL.numbefOfFields * innerFieldDividers.numbefOfInnerFields;
                    for (int i = 0; i < frameDividersForL.numbefOfFields; i++)
                    {
                        for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                        {

                            GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + Field_divider_naming_count.ToString("D4"));
                            fieldDivider_GO_parent.transform.parent = FieldDividers_Parent_Section2.transform;

                            GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);
                            fieldDivider_GO.transform.localPosition = new Vector3(-inner_dividerPoleDistance, 0, framePartSettings._part_depth); //Todo add(offset) frame B or D width to each field width
                            fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                            fieldDivider_GO.transform.localScale = new Vector3(1, hit_DividertoE.distance, 1);
                            fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            fieldDivider_GO.name = "FieldDivider_" + Field_divider_naming_count++.ToString("D4");
                            inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                            if (divider_type == "ak - 279")
                            {
                                GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                                C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                                C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                                C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                                if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                    Accessories_name.Add(C_Clamp_ForDivider.name);

                                GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                                C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                                C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                                C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                                if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                    Accessories_name.Add((C_Clamp_ForDivider_2.name));
                            }
                        }  //section 1 loop dividertoe
                        inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                    }
                    total_number_of_fields_DividertoE = innerFieldDividers.numbefOfInnerFields;
                    each_inner_field_width_fieldDividertoE = innerFieldDividers.each_inner_field_width;

                    ray_cast_dir = Pergola_Model.transform.TransformDirection(Pergola_Model.transform.forward);
                    top_point_FrameDivider_0 = global_center_FrameDivider_0 + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, FrameDivider_0_bound.size)) / 2;
                    if (Physics.Raycast(top_point_FrameDivider_0, ray_cast_dir, out hit_DividertoC, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * hit_DividertoC.distance, Color.yellow);
                        Debug.Log($"hit_DividertoC  Did Hit & Distance={hit_DividertoC.distance}");
                    } //section 2 raycast
                    else
                    {
                        Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("hit_DividertoC  Did not Hit");
                    }

                    frameDividersForL = new FrameDividersForL(hit_DividertoC.distance, framePartSettings, verticalPartSettings);
                    inner_dividerPoleDistance = vertical_bar_sideB.transform.localPosition.z + frameDividersForL.each_field_width + verticalPartSettings._part_depth;
                    Number_of_dividers_in_section_2 = frameDividersForL.numberOfDividerPoles;
                    supportBarCount_in_Region1 = frameDividersForL.numberOfDividerPoles;

                    for (int j = 0; j < frameDividersForL.numberOfDividerPoles; j++)
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + divider_naming_counter.ToString("D4"));
                        frameDivider_GO_parent.transform.parent = FrameDividers_Parent_Section1.transform;

                        GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        fieldDivider_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, inner_dividerPoleDistance); //Todo add(offset) frame B or D width to each field width
                        fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        fieldDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                        fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        fieldDivider_GO.name = "FrameDivider_" + divider_naming_counter++.ToString("D4");
                        inner_dividerPoleDistance += (frameDividersForL.each_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }
                    total_number_of_fields_DividertoC = frameDividersForL.numbefOfFields;
                    each_inner_field_width_fieldDividertoC = frameDividersForL.each_field_width;

                    innerFieldDividers = new InnerFieldDividers(frameDividersForL.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                    float offset = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0001").Find("FrameDivider_Parent_0000").Find("FrameDivider_0000").transform.localPosition.z;
                    inner_dividerPoleDistance = verticalPartSettings._part_depth + innerFieldDividers.each_inner_field_width;
                    region2_fields_count = frameDividersForL.numbefOfFields * innerFieldDividers.numbefOfInnerFields;
                    for (int i = 0; i < frameDividersForL.numbefOfFields; i++)
                    {
                        for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                        {
                            GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + Field_divider_naming_count.ToString("D4"));
                            fieldDivider_GO_parent.transform.parent = FieldDividers_Parent_Section1.transform;

                            GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);
                            fieldDivider_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, inner_dividerPoleDistance + offset); //Todo add(offset) frame B or D width to each field width
                            fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            fieldDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                            fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            fieldDivider_GO.name = "FieldDivider_" + Field_divider_naming_count++.ToString("D4");
                            inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                            if (divider_type == "ak - 279")
                            {
                                GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                                C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                                C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                                C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++.ToString("D4");

                                if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                    Accessories_name.Add(C_Clamp_ForDivider.name);

                                GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                                C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                                C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                                C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                                C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++.ToString("D4");

                                if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                    Accessories_name.Add((C_Clamp_ForDivider_2.name));
                            }
                        } // section 2 
                        inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                    }
                    total_number_of_fields_DividertoF = innerFieldDividers.numbefOfInnerFields;
                    each_inner_field_width_fieldDividertoF = innerFieldDividers.each_inner_field_width;

                    // characteristics
                    int sec1count = 0;
                    foreach (Transform t in FrameDividers_Parent_Section1.transform)
                    {
                        if (sec1count == 0)
                        {

                        }
                        else
                        {
                            characteristics_script = t.gameObject.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = clampfordivider;
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_group = "Frame";
                            if (t.name.Contains("FrameDivider"))
                            {
                                characteristics_script.part_name_id = divider_type;
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                            }
                        }
                        sec1count++;
                    }
                    int sec2count = 0;
                    foreach (Transform t in FrameDividers_Parent_Section2.transform)
                    {
                        if (sec2count == 0)
                        {

                        }
                        else
                        {
                            characteristics_script = t.gameObject.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = clampfordivider;
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_group = "Frame";
                            if (t.name.Contains("FrameDivider"))
                            {
                                characteristics_script.part_name_id = divider_type;
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                            }
                        }
                        sec2count++;

                    }

                    foreach (Transform t in FieldDividers_Parent_Section1.transform)
                    {


                        characteristics_script = t.gameObject.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = clampfordivider;
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_group = "Frame";
                        if (t.name.Contains("FieldDivider"))
                        {
                            characteristics_script.part_name_id = divider_type;
                            characteristics_script.part_type = part_type_enum.vertical.ToString();
                        }

                    }

                    foreach (Transform t in FieldDividers_Parent_Section2.transform)
                    {

                        characteristics_script = t.gameObject.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = clampfordivider;
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_group = "Frame";
                        if (t.name.Contains(""))
                        {
                            characteristics_script.part_name_id = divider_type;
                            characteristics_script.part_type = part_type_enum.vertical.ToString();
                        }

                    }
                    #endregion
                }

                #region Fields and Accessories

                Field_Parent = new GameObject("Field_Parent");
                Field_Parent.transform.parent = Pergola_Model.transform;

                Field_Parent_Section1 = new GameObject("Field_Parent_Section_0001");
                Field_Parent_Section1.transform.parent = Field_Parent.transform;

                Field_Parent_Section2 = new GameObject("Field_Parent_Section_0002");
                Field_Parent_Section2.transform.parent = Field_Parent.transform;

                int accessory_naming_counter = 1;
                int accessory_naming_counterL = 1;
                int accessory_naming_counterBar = 1;
                int field_naming_counter = 1;
                int naming_count_for_region2 = 0;
                float accessory_placement = framePartSettings._part_depth;
                bool place_u_accessory = false;
                if (Array.Exists(horizontalBar_forUType_Accessory, element => element == horizontal_type))
                    place_u_accessory = true;

                if (horizontal_type == "ak - 40")
                {

                    if (dsElement_pergola_header.Tables[0].Rows[0]["rafafa_placement_type"].ToString() == "type_2")
                    {
                        float field_height = scale_c - (2 * framePartSettings._part_depth) - assembly_tolerance;

                        float clip_height_full = 90.47f;
                        float clip_spare_part_height = 3.5f;
                        if (space_btw_rafafa == 20)
                        {
                            //clip_name = "ak - 76";
                            clip_height_full = 90.47f;
                            fieldBar_prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/pergola/ak - 40-76-76", typeof(GameObject));
                        }
                        else if (space_btw_rafafa == 50)
                        {
                            //clip_name = "ak - 39";
                            clip_height_full = 120.47f; //For space between 50, ak - 39
                            fieldBar_prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/pergola/ak - 40-39", typeof(GameObject));
                        }
                        else if (space_btw_rafafa == -10)
                        {
                            //clip_name = "ak - 72";
                            clip_height_full = 120.47f; //For space between 50, ak - 39
                            fieldBar_prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/pergola/ak - 40-72-40", typeof(GameObject));
                        }

                        int NumOfRafafa = Convert.ToInt32(System.Math.Floor((field_height - clip_spare_part_height) / clip_height_full));
                        float h_Bar_offset = (field_height + clip_spare_part_height) - ((NumOfRafafa * (clip_height_full - clip_spare_part_height)));

                        //HORIZONTALS ALWAYS MIDDLE ALIGNED.
                        h_Bar_offset = h_Bar_offset / 2;

                        if (h_Bar_offset <= 0)
                            h_Bar_offset = space_btw_rafafa;

                        FrameDividersForL frameDividersForL_dividertoe = new FrameDividersForL(hit_DividertoE.distance, framePartSettings, verticalPartSettings);
                        InnerFieldDividers innerFieldDividers_for_Divider_to_E = new InnerFieldDividers(frameDividersForL_dividertoe.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                        for (int i = 1; i <= region1_fields_count; i++)
                        {
                            GameObject FieldGroup_GO = new GameObject("Field_Group_" + i.ToString("D4"));
                            FieldGroup_GO.transform.parent = Field_Parent_Section1.transform;

                            //Accessories
                            GameObject Accessories_parent = new GameObject("Accessories_" + accessory_naming_counter.ToString("D4"));
                            Accessories_parent.transform.parent = FieldGroup_GO.transform;

                            for (int j = 1; j <= 2; j++)
                            {
                                GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                GameObject vertical_40x40_Bar = Instantiate(vertical_40x40_Bar_Prefab, Accessories_parent.transform);

                                switch (j)
                                {
                                    case 1: //left bottom
                                        Accessory_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                                        Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                        Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counterL++.ToString("D4");

                                        vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                        vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counterBar++.ToString("D4");
                                        vertical_40x40_Bar.transform.Translate(verticalPartAccessory._part_depth * Vector3.forward);
                                        // vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.right, Space.World);
                                        //vertical_40x40_Bar.transform.Translate((assembly_tolerance / 2) * Vector3.forward, Space.World);
                                        vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                        break;
                                    case 2: //right bottom
                                        Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                        Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers_for_Divider_to_E.each_inner_field_width);
                                        Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counterL++.ToString("D4");

                                        vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                        vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counterBar++.ToString("D4");
                                        //  vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.right, Space.World);
                                        //   vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward, Space.World);
                                        vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                        accessory_naming_counter++;
                                        break;
                                }
                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = L_accessory_type;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                                characteristics_script.part_group = "Frame";

                                vertical_40x40_Bar.transform.Translate((verticalPartAccessory._part_width + L_accessory_thickess) * Vector3.up);
                                vertical_40x40_Bar.transform.localRotation = Quaternion.Euler(-90, 90, 0);
                                vertical_40x40_Bar.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                vertical_40x40_Bar.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                characteristics_script = vertical_40x40_Bar.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = vertical_40x40_bar;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                                characteristics_script.part_group = "Field_1";

                                if (!Accessories_name.Contains(vertical_40x40_Bar.name))
                                    Accessories_name.Add(vertical_40x40_Bar.name);

                                if (!Accessories_name.Contains(Accessory_GO.name))
                                    Accessories_name.Add(Accessory_GO.name);
                            }
                            accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_E.each_inner_field_width);

                            if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                            {
                                no_fields = false;

                                //Fields
                                GameObject Fields_GO = new GameObject("Fields_" + i.ToString("D4"));
                                Fields_GO.transform.parent = FieldGroup_GO.transform;
                                for (int j = 0; j < NumOfRafafa; j++)
                                {
                                    GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                    Field_GO.GetComponent<Follow>().follow_child = Field_GO.transform.Find("Locator_1");
                                    Field_GO.GetComponent<Follow>().update_location_and_rotation();
                                    Field_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Clip").position.x,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_TopInside_for_Clip").position.y,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_TopInside_for_Clip").position.z);


                                    Field_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, -90, 0);
                                    Field_GO.GetComponent<Follow>().move_parent_relative_toChild();
                                    Field_GO.transform.GetChild(0).localScale = new Vector3(innerFieldDividers_for_Divider_to_E.each_inner_field_width - (assembly_tolerance / 2) - (2 * L_accessory_thickess), 1, 1);
                                    if (space_btw_rafafa == -10)
                                    {
                                        Field_GO.transform.GetChild(1).localScale = new Vector3(innerFieldDividers_for_Divider_to_E.each_inner_field_width - (assembly_tolerance / 2) - (2 * L_accessory_thickess), 1, 1);
                                        Field_GO.transform.GetChild(3).Translate((innerFieldDividers_for_Divider_to_E.each_inner_field_width - (L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(2).Translate(L_accessory_thickess * Vector3.right);
                                    }
                                    else
                                    {
                                        Field_GO.transform.GetChild(2).Translate((innerFieldDividers_for_Divider_to_E.each_inner_field_width - (2 * L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(1).Translate(L_accessory_thickess * Vector3.right);
                                        Field_GO.transform.GetChild(2).GetChild(0).gameObject.AddComponent<BoxCollider>(); //for clips having spacing -10, box collider already added in prefab
                                    }

                                    Field_GO.transform.GetChild(0).GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    Field_GO.transform.GetChild(1).GetChild(0).gameObject.AddComponent<BoxCollider>();

                                    Field_GO.transform.Translate(-(clip_height_full - clip_spare_part_height) * j * Vector3.up);

                                    Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");


                                    // characteristics for Fields
                                    foreach (Transform t in Field_GO.transform)
                                    {
                                        characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                        characteristics_script.part_group = "Field_1";
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();

                                        if (t.name.Contains("40"))
                                        {
                                            characteristics_script.part_name_id = horizontal_type;
                                            characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                        }
                                        else if (t.name.Contains("39"))
                                        {
                                            characteristics_script.part_name_id = "ak - 39";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else if (t.name.Contains("72"))
                                        {
                                            characteristics_script.part_name_id = "ak - 72";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else if (t.name.Contains("76"))
                                        {
                                            characteristics_script.part_name_id = "ak - 76";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                    }
                                }

                                Fields_GO.transform.Translate(-h_Bar_offset * Vector3.right);
                                Fields_GO.transform.Translate((innerFieldDividers_for_Divider_to_E.each_inner_field_width + verticalPartSettings._part_depth) * (i - 1) * Vector3.forward);
                            }
                            else no_fields = true;

                            // FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                            //FieldGroup_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);

                        }


                        FrameDividersForL frameDividersForL_dividertC = new FrameDividersForL(hit_DividertoC.distance, framePartSettings, verticalPartSettings);
                        InnerFieldDividers innerFieldDividers_for_Divider_to_C = new InnerFieldDividers(frameDividersForL_dividertC.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                        for (int i = 1; i <= region2_fields_count; i++)
                        {
                            GameObject FieldGroup_GO = new GameObject("Field_Group_" + (i + region1_fields_count).ToString("D4"));
                            FieldGroup_GO.transform.parent = Field_Parent_Section1.transform;

                            //Accessories
                            GameObject Accessories_parent = new GameObject("Accessories_" + (i + region1_fields_count).ToString("D4"));
                            Accessories_parent.transform.parent = FieldGroup_GO.transform;

                            for (int j = 1; j <= 2; j++)
                            {
                                GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                GameObject vertical_40x40_Bar = Instantiate(vertical_40x40_Bar_Prefab, Accessories_parent.transform);

                                switch (j)
                                {
                                    case 1: //left bottom
                                        Accessory_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                                        Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                        Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counterL++.ToString("D4");

                                        vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                        vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counterBar++.ToString("D4");
                                        vertical_40x40_Bar.transform.Translate(verticalPartAccessory._part_depth * Vector3.forward);
                                        // vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.right, Space.World);
                                        // vertical_40x40_Bar.transform.Translate((assembly_tolerance / 2) * Vector3.forward, Space.World);
                                        vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                        break;
                                    case 2: //right bottom
                                        Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                        Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers_for_Divider_to_C.each_inner_field_width);
                                        Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counterL++.ToString("D4");

                                        vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                        vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counterBar++.ToString("D4");
                                        // vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.right, Space.World);
                                        // vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward, Space.World);
                                        vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                        accessory_naming_counter++;
                                        break;
                                }
                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = L_accessory_type;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                                characteristics_script.part_group = "Frame";

                                vertical_40x40_Bar.transform.Translate((verticalPartAccessory._part_depth + L_accessory_thickess) * Vector3.up);
                                vertical_40x40_Bar.transform.localRotation = Quaternion.Euler(-90, 90, 0);
                                vertical_40x40_Bar.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                vertical_40x40_Bar.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                characteristics_script = vertical_40x40_Bar.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = vertical_40x40_bar;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                                characteristics_script.part_group = "Field_2";

                                if (!Accessories_name.Contains(vertical_40x40_Bar.name))
                                    Accessories_name.Add(vertical_40x40_Bar.name);

                                if (!Accessories_name.Contains(Accessory_GO.name))
                                    Accessories_name.Add(Accessory_GO.name);
                            }
                            accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_C.each_inner_field_width);

                            if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                            {
                                no_fields = false;
                                //Fields
                                GameObject Fields_GO = new GameObject("Fields_" + (i + region1_fields_count).ToString("D4"));
                                Fields_GO.transform.parent = FieldGroup_GO.transform;
                                //Fields_GO.transform.Translate((rafafa_placement) * Vector3.forward);
                                for (int j = 0; j < NumOfRafafa; j++)
                                {
                                    GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                    Field_GO.GetComponent<Follow>().follow_child = Field_GO.transform.Find("Locator_1");
                                    Field_GO.GetComponent<Follow>().update_location_and_rotation();
                                    Field_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Clip").position.x,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_TopInside_for_Clip").position.y,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_TopInside_for_Clip").position.z);
                                    Field_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, -90, 0);
                                    Field_GO.GetComponent<Follow>().move_parent_relative_toChild();
                                    Field_GO.transform.GetChild(0).localScale = new Vector3(innerFieldDividers_for_Divider_to_C.each_inner_field_width - (assembly_tolerance / 2) - (2 * L_accessory_thickess), 1, 1);
                                    if (space_btw_rafafa == -10)
                                    {
                                        Field_GO.transform.GetChild(1).localScale = new Vector3(innerFieldDividers_for_Divider_to_C.each_inner_field_width - (2 * L_accessory_thickess), 1, 1);
                                        Field_GO.transform.GetChild(3).Translate((innerFieldDividers_for_Divider_to_C.each_inner_field_width - (L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(2).Translate(L_accessory_thickess * Vector3.right);
                                    }
                                    else
                                    {
                                        Field_GO.transform.GetChild(2).Translate((innerFieldDividers_for_Divider_to_C.each_inner_field_width - (L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(1).Translate(L_accessory_thickess * Vector3.right);
                                        Field_GO.transform.GetChild(2).GetChild(0).gameObject.AddComponent<BoxCollider>(); //for clips having spacing -10, box collider already added in prefab
                                    }

                                    Field_GO.transform.GetChild(0).GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    Field_GO.transform.GetChild(1).GetChild(0).gameObject.AddComponent<BoxCollider>();

                                    Field_GO.transform.Translate(-(clip_height_full - clip_spare_part_height) * j * Vector3.up);
                                    Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");


                                    // characteristics for Fields
                                    foreach (Transform t in Field_GO.transform)
                                    {
                                        characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                        characteristics_script.part_group = "Field_2";
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        if (t.name.Contains("40"))
                                        {
                                            characteristics_script.part_name_id = horizontal_type;
                                            characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                        }
                                        else if (t.name.Contains("39"))
                                        {
                                            characteristics_script.part_name_id = "ak - 39";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else if (t.name.Contains("72"))
                                        {
                                            characteristics_script.part_name_id = "ak - 72";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else if (t.name.Contains("76"))
                                        {
                                            characteristics_script.part_name_id = "ak - 76";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                    }
                                }

                                Fields_GO.transform.Translate(-h_Bar_offset * Vector3.right);
                                Fields_GO.transform.Translate((hit_DividertoE.distance + verticalPartSettings._part_depth) * Vector3.forward);
                                Fields_GO.transform.Translate((innerFieldDividers_for_Divider_to_C.each_inner_field_width + verticalPartSettings._part_depth) * (i - 1) * Vector3.forward);
                            }
                            else no_fields = true;

                            //FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                            //FieldGroup_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                        }


                        field_height = scale_f - (2 * framePartSettings._part_depth) - assembly_tolerance;
                        NumOfRafafa = Convert.ToInt32(System.Math.Floor((field_height - clip_spare_part_height) / clip_height_full));
                        h_Bar_offset = (field_height + clip_spare_part_height) - ((NumOfRafafa * (clip_height_full - clip_spare_part_height)));
                        h_Bar_offset = h_Bar_offset / 2;
                        if (h_Bar_offset <= 0)
                            h_Bar_offset = space_btw_rafafa;
                        FrameDividersForL frameDividersForL_dividertF = new FrameDividersForL(hit_DividertoF.distance, framePartSettings, verticalPartSettings);
                        InnerFieldDividers innerFieldDividers_for_Divider_to_F = new InnerFieldDividers(frameDividersForL_dividertF.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                        accessory_placement = verticalPartSettings._part_depth;
                        for (int i = 1; i <= region3_fields_count; i++)
                        {
                            GameObject FieldGroup_GO = new GameObject("Field_Group_" + (i + region1_fields_count + region2_fields_count).ToString("D4"));
                            FieldGroup_GO.transform.parent = Field_Parent_Section2.transform;

                            //Accessories
                            GameObject Accessories_parent = new GameObject("Accessories_" + (i + region1_fields_count + region2_fields_count).ToString("D4"));
                            Accessories_parent.transform.parent = FieldGroup_GO.transform;

                            float X_pos_offset_for_DivF = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition.x;
                            float Z_pos_offset_for_DivF = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition.z;

                            for (int j = 1; j <= 2; j++)
                            {
                                GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                GameObject vertical_40x40_Bar = Instantiate(vertical_40x40_Bar_Prefab, Accessories_parent.transform);

                                switch (j)
                                {
                                    case 1: //left bottom
                                        Accessory_GO.transform.localRotation = Quaternion.Euler(-90, -90, 90);
                                        Accessory_GO.transform.localPosition = new Vector3(X_pos_offset_for_DivF - accessory_placement - innerFieldDividers_for_Divider_to_F.each_inner_field_width, 0, Z_pos_offset_for_DivF);
                                        // Accessory_GO.transform.localRotation = Quaternion.Euler(-90, 90, -90);
                                        //Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                        Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counterL++.ToString("D4");
                                        vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                        vertical_40x40_Bar.transform.localRotation = Quaternion.Euler(-90, 0, 180);
                                        vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counterBar++.ToString("D4");
                                        vertical_40x40_Bar.transform.Translate((verticalPartAccessory._part_width + L_accessory_thickess) * Vector3.up, Space.World);
                                        vertical_40x40_Bar.transform.Translate(-(verticalPartAccessory._part_width) * Vector3.forward, Space.World);

                                        // vertical_40x40_Bar.transform.Translate(verticalPartAccessory._part_depth * Vector3.forward);

                                        //vertical_40x40_Bar.transform.Translate((assembly_tolerance / 2) * Vector3.forward, Space.World);
                                        vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                        break;
                                    case 2: //right bottom
                                        Accessory_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                                        Accessory_GO.transform.localPosition = new Vector3(X_pos_offset_for_DivF - accessory_placement, 0, Z_pos_offset_for_DivF);

                                        //  Accessory_GO.transform.localRotation = Quaternion.Euler(-90, -90, 90);
                                        //Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers_for_Divider_to_F.each_inner_field_width - assembly_tolerance);
                                        Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counterL++.ToString("D4");


                                        vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                        vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counterBar++.ToString("D4");
                                        vertical_40x40_Bar.transform.Translate(-(verticalPartSettings._part_depth) * Vector3.right, Space.World);
                                        // vertical_40x40_Bar.transform.Translate(-((assembly_tolerance / 2) )* Vector3.right, Space.World);
                                        //vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward, Space.World);
                                        vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                        accessory_naming_counter++;
                                        break;
                                }
                                Accessory_GO.transform.localScale = new Vector3(1, hit_AtoE.distance - assembly_tolerance, 1);
                                Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = L_accessory_type;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                                characteristics_script.part_group = "Frame";

                                vertical_40x40_Bar.transform.Translate((verticalPartAccessory._part_width + L_accessory_thickess) * Vector3.up);
                                vertical_40x40_Bar.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                                vertical_40x40_Bar.transform.localScale = new Vector3(1, hit_AtoE.distance - assembly_tolerance, 1);
                                vertical_40x40_Bar.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                characteristics_script = vertical_40x40_Bar.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = vertical_40x40_bar;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                                characteristics_script.part_group = "Field_3";


                                if (!Accessories_name.Contains(vertical_40x40_Bar.name))
                                    Accessories_name.Add(vertical_40x40_Bar.name);

                                if (!Accessories_name.Contains(Accessory_GO.name))
                                    Accessories_name.Add(Accessory_GO.name);

                                //accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_F.each_inner_field_width);
                            }

                            accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_F.each_inner_field_width);
                            if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                            {
                                no_fields = false;

                                //Fields
                                GameObject Fields_GO = new GameObject("Fields_" + (i + region1_fields_count + region2_fields_count).ToString("D4"));
                                Fields_GO.transform.parent = FieldGroup_GO.transform;
                                for (int j = 0; j < NumOfRafafa; j++)
                                {
                                    GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                    Field_GO.GetComponent<Follow>().follow_child = Field_GO.transform.Find("Locator_1");
                                    Field_GO.GetComponent<Follow>().update_location_and_rotation();



                                    Field_GO.transform.Find("Locator_1").position = new Vector3(FrameDividers_Parent.transform.GetChild(1).GetChild(0).GetChild(0).transform.position.x,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_TopInside_for_Clip").position.y,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_TopInside_for_Clip").position.z);
                                    Field_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, 0, 0);
                                    Field_GO.GetComponent<Follow>().move_parent_relative_toChild();
                                    Field_GO.transform.GetChild(0).localScale = new Vector3(innerFieldDividers_for_Divider_to_F.each_inner_field_width - (assembly_tolerance / 2) - (2 * L_accessory_thickess), 1, 1);
                                    if (space_btw_rafafa == -10)
                                    {
                                        Field_GO.transform.GetChild(1).localScale = new Vector3(innerFieldDividers_for_Divider_to_F.each_inner_field_width - (assembly_tolerance / 2) - (2 * L_accessory_thickess), 1, 1);
                                        Field_GO.transform.GetChild(3).Translate((innerFieldDividers_for_Divider_to_F.each_inner_field_width - (L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(2).Translate((L_accessory_thickess) * Vector3.right);
                                    }
                                    else
                                    {
                                        Field_GO.transform.GetChild(2).Translate((innerFieldDividers_for_Divider_to_F.each_inner_field_width - (L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(1).Translate((L_accessory_thickess) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(2).GetChild(0).gameObject.AddComponent<BoxCollider>(); //for clips having spacing -10, box collider already added in prefab
                                    }

                                    Field_GO.transform.GetChild(0).GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    Field_GO.transform.GetChild(1).GetChild(0).gameObject.AddComponent<BoxCollider>();

                                    Field_GO.transform.Translate(-(clip_height_full - clip_spare_part_height) * j * Vector3.up);

                                    Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");


                                    // characteristics for Fields
                                    foreach (Transform t in Field_GO.transform)
                                    {
                                        characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                        characteristics_script.part_group = "Field_3";
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        if (t.name.Contains("40"))
                                        {
                                            characteristics_script.part_name_id = horizontal_type;
                                            characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                        }
                                        else if (t.name.Contains("39"))
                                        {
                                            characteristics_script.part_name_id = "ak - 39";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else if (t.name.Contains("72"))
                                        {
                                            characteristics_script.part_name_id = "ak - 72";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else if (t.name.Contains("76"))
                                        {
                                            characteristics_script.part_name_id = "ak - 76";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                    }
                                }

                                Fields_GO.transform.Translate(h_Bar_offset * Vector3.forward);
                                Debug.Log("$$$" + h_Bar_offset);
                                Fields_GO.transform.Translate(-(innerFieldDividers_for_Divider_to_F.each_inner_field_width + verticalPartSettings._part_depth) * i * Vector3.right);
                            }
                            else no_fields = true;

                            // FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                            // FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.right);
                        }

                    }

                    else if (dsElement_pergola_header.Tables[0].Rows[0]["rafafa_placement_type"].ToString() == "type_3")
                    {
                        float field_height = scale_f - (2 * framePartSettings._part_depth) - assembly_tolerance;

                        float clip_height_full = 90.47f;
                        float clip_spare_part_height = 3.5f;
                        if (space_btw_rafafa == 20)
                        {
                            //clip_name = "ak - 76";
                            clip_height_full = 90.47f;
                            fieldBar_prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/pergola/ak - 40-76-76", typeof(GameObject));
                        }
                        else if (space_btw_rafafa == 50)
                        {
                            //clip_name = "ak - 39";
                            clip_height_full = 120.47f; //For space between 50, ak - 39
                            fieldBar_prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/pergola/ak - 40-39", typeof(GameObject));
                        }
                        else if (space_btw_rafafa == -10)
                        {
                            //clip_name = "ak - 72";
                            clip_height_full = 120.47f; //For space between 50, ak - 39
                            fieldBar_prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/pergola/ak - 40-72-40", typeof(GameObject));
                        }

                        int NumOfRafafa = Convert.ToInt32(System.Math.Floor((field_height - clip_spare_part_height) / clip_height_full));
                        float h_Bar_offset = (field_height + clip_spare_part_height) - ((NumOfRafafa * (clip_height_full - clip_spare_part_height)));

                        //HORIZONTALS ALWAYS MIDDLE ALIGNED.
                        h_Bar_offset = h_Bar_offset / 2;

                        if (h_Bar_offset <= 0)
                            h_Bar_offset = space_btw_rafafa;

                        FrameDividersForL frameDividersForL_dividertoe = new FrameDividersForL(hit_BtoD.distance, framePartSettings, verticalPartSettings);
                        InnerFieldDividers innerFieldDividers_for_Divider_to_E = new InnerFieldDividers(frameDividersForL_dividertoe.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                        accessory_placement = framePartSettings._part_depth;
                        float X_pos_offset_for_DivF = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition.x;
                        float Z_pos_offset_for_DivF = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition.z;

                        for (int i = 1; i <= region1_fields_count; i++)
                        {
                            GameObject FieldGroup_GO = new GameObject("Field_Group_" + (i).ToString("D4"));
                            FieldGroup_GO.transform.parent = Field_Parent_Section2.transform;

                            //Accessories
                            GameObject Accessories_parent = new GameObject("Accessories_" + (i).ToString("D4"));
                            Accessories_parent.transform.parent = FieldGroup_GO.transform;

                            for (int j = 1; j <= 2; j++)
                            {
                                GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                GameObject vertical_40x40_Bar = Instantiate(vertical_40x40_Bar_Prefab, Accessories_parent.transform);

                                switch (j)
                                {
                                    case 1: //left bottom
                                        Accessory_GO.transform.localRotation = Quaternion.Euler(-90, -90, 90);
                                        Accessory_GO.transform.localPosition = new Vector3(-accessory_placement - innerFieldDividers_for_Divider_to_E.each_inner_field_width, 0, framePartSettings._part_depth);
                                        // Accessory_GO.transform.localRotation = Quaternion.Euler(-90, 90, -90);
                                        //Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                        Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counterL++.ToString("D4");
                                        vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                        vertical_40x40_Bar.transform.localRotation = Quaternion.Euler(-90, 0, 180);
                                        vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counterBar++.ToString("D4");
                                        vertical_40x40_Bar.transform.Translate((verticalPartAccessory._part_width + L_accessory_thickess) * Vector3.up, Space.World);
                                        vertical_40x40_Bar.transform.Translate(-(verticalPartAccessory._part_width) * Vector3.forward, Space.World);

                                        // vertical_40x40_Bar.transform.Translate(verticalPartAccessory._part_depth * Vector3.forward);

                                        //vertical_40x40_Bar.transform.Translate((assembly_tolerance / 2) * Vector3.forward, Space.World);
                                        vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                        break;
                                    case 2: //right bottom
                                        Accessory_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                                        Accessory_GO.transform.localPosition = new Vector3(-accessory_placement, 0, framePartSettings._part_depth);

                                        //  Accessory_GO.transform.localRotation = Quaternion.Euler(-90, -90, 90);
                                        //Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers_for_Divider_to_F.each_inner_field_width - assembly_tolerance);
                                        Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counterL++.ToString("D4");


                                        vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                        vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counterBar++.ToString("D4");
                                        vertical_40x40_Bar.transform.Translate(-(verticalPartSettings._part_depth) * Vector3.right, Space.World);
                                        // vertical_40x40_Bar.transform.Translate(-((assembly_tolerance / 2) )* Vector3.right, Space.World);
                                        //vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward, Space.World);
                                        vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                        accessory_naming_counter++;
                                        break;
                                }
                                Accessory_GO.transform.localScale = new Vector3(1, -hit_DividertoE.distance - assembly_tolerance, 1);
                                Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = L_accessory_type;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                                characteristics_script.part_group = "Frame";

                                vertical_40x40_Bar.transform.Translate((verticalPartAccessory._part_width + L_accessory_thickess) * Vector3.up);
                                vertical_40x40_Bar.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                                vertical_40x40_Bar.transform.localScale = new Vector3(1, -hit_AtoE.distance - assembly_tolerance, 1);
                                vertical_40x40_Bar.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                characteristics_script = vertical_40x40_Bar.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = vertical_40x40_bar;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                                characteristics_script.part_group = "Field_1";


                                if (!Accessories_name.Contains(vertical_40x40_Bar.name))
                                    Accessories_name.Add(vertical_40x40_Bar.name);

                                if (!Accessories_name.Contains(Accessory_GO.name))
                                    Accessories_name.Add(Accessory_GO.name);

                                //accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_F.each_inner_field_width);
                            }

                            accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_E.each_inner_field_width);
                            if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                            {
                                no_fields = false;

                                //Fields
                                GameObject Fields_GO = new GameObject("Fields_" + (i + region1_fields_count + region2_fields_count).ToString("D4"));
                                Fields_GO.transform.parent = FieldGroup_GO.transform;
                                for (int j = 0; j < NumOfRafafa; j++)
                                {
                                    GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                    Field_GO.GetComponent<Follow>().follow_child = Field_GO.transform.Find("Locator_1");
                                    Field_GO.GetComponent<Follow>().update_location_and_rotation();

                                    Field_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Clip").position.x,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_TopInside_for_Clip").position.y,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_TopInside_for_Clip").position.z);

                                    Field_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, 0, 0);
                                    Field_GO.GetComponent<Follow>().move_parent_relative_toChild();
                                    Field_GO.transform.GetChild(0).localScale = new Vector3(-innerFieldDividers_for_Divider_to_E.each_inner_field_width + (assembly_tolerance / 2) + (2 * L_accessory_thickess), 1, 1);
                                    if (space_btw_rafafa == -10)
                                    {
                                        Field_GO.transform.GetChild(1).localScale = new Vector3(innerFieldDividers_for_Divider_to_E.each_inner_field_width - (assembly_tolerance / 2) - (2 * L_accessory_thickess), 1, 1);
                                        Field_GO.transform.GetChild(3).Translate((innerFieldDividers_for_Divider_to_E.each_inner_field_width - (L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(2).Translate((L_accessory_thickess) * Vector3.right);
                                    }
                                    else
                                    {
                                        Field_GO.transform.GetChild(2).Translate((-innerFieldDividers_for_Divider_to_E.each_inner_field_width + (L_accessory_thickess)) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(1).Translate(-(L_accessory_thickess + 40) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(2).GetChild(0).gameObject.AddComponent<BoxCollider>(); //for clips having spacing -10, box collider already added in prefab
                                    }

                                    Field_GO.transform.GetChild(0).GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    Field_GO.transform.GetChild(1).GetChild(0).gameObject.AddComponent<BoxCollider>();

                                    Field_GO.transform.Translate(-(clip_height_full - clip_spare_part_height) * j * Vector3.up);

                                    Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");


                                    // characteristics for Fields
                                    foreach (Transform t in Field_GO.transform)
                                    {
                                        characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                        characteristics_script.part_group = "Field_1";
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        if (t.name.Contains("40"))
                                        {
                                            characteristics_script.part_name_id = horizontal_type;
                                            characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                        }
                                        else if (t.name.Contains("39"))
                                        {
                                            characteristics_script.part_name_id = "ak - 39";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else if (t.name.Contains("72"))
                                        {
                                            characteristics_script.part_name_id = "ak - 72";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else if (t.name.Contains("76"))
                                        {
                                            characteristics_script.part_name_id = "ak - 76";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                    }
                                }

                                Fields_GO.transform.Translate(h_Bar_offset * Vector3.forward);
                                Fields_GO.transform.Translate(-(innerFieldDividers_for_Divider_to_E.each_inner_field_width + verticalPartSettings._part_depth) * (i - 1) * Vector3.right);
                            }
                            else no_fields = true;

                            // FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                            // FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.right);
                        }



                        field_height = scale_c - (2 * framePartSettings._part_depth) - assembly_tolerance;

                        clip_height_full = 90.47f;
                        clip_spare_part_height = 3.5f;
                        if (space_btw_rafafa == 20)
                        {
                            //clip_name = "ak - 76";
                            clip_height_full = 90.47f;
                            fieldBar_prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/pergola/ak - 40-76-76", typeof(GameObject));
                        }
                        else if (space_btw_rafafa == 50)
                        {
                            //clip_name = "ak - 39";
                            clip_height_full = 120.47f; //For space between 50, ak - 39
                            fieldBar_prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/pergola/ak - 40-39", typeof(GameObject));
                        }
                        else if (space_btw_rafafa == -10)
                        {
                            //clip_name = "ak - 72";
                            clip_height_full = 120.47f; //For space between 50, ak - 39
                            fieldBar_prefab = (GameObject)Resources.Load($"prefabs/Clips_and_bars/pergola/ak - 40-72-40", typeof(GameObject));
                        }

                        NumOfRafafa = Convert.ToInt32(System.Math.Floor((field_height - clip_spare_part_height) / clip_height_full));
                        h_Bar_offset = (field_height + clip_spare_part_height) - ((NumOfRafafa * (clip_height_full - clip_spare_part_height)));

                        //HORIZONTALS ALWAYS MIDDLE ALIGNED.
                        h_Bar_offset = h_Bar_offset / 2;

                        if (h_Bar_offset <= 0)
                            h_Bar_offset = space_btw_rafafa;
                        FrameDividersForL frameDividersForL_dividertC = new FrameDividersForL(hit_DividertoC.distance, framePartSettings, verticalPartSettings);
                        InnerFieldDividers innerFieldDividers_for_Divider_to_C = new InnerFieldDividers(frameDividersForL_dividertC.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                        accessory_placement = framePartSettings._part_depth + hit_DividertoE.distance + verticalPartSettings._part_depth;
                        for (int i = 1; i <= region2_fields_count; i++)
                        {
                            GameObject FieldGroup_GO = new GameObject("Field_Group_" + (i + region1_fields_count).ToString("D4"));
                            FieldGroup_GO.transform.parent = Field_Parent_Section1.transform;

                            //Accessories
                            GameObject Accessories_parent = new GameObject("Accessories_" + (i + region1_fields_count).ToString("D4"));
                            Accessories_parent.transform.parent = FieldGroup_GO.transform;

                            for (int j = 1; j <= 2; j++)
                            {
                                GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                GameObject vertical_40x40_Bar = Instantiate(vertical_40x40_Bar_Prefab, Accessories_parent.transform);

                                switch (j)
                                {
                                    case 1: //left bottom
                                        Accessory_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                                        Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                        Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counterL++.ToString("D4");

                                        vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                        vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counterBar++.ToString("D4");
                                        vertical_40x40_Bar.transform.Translate(verticalPartAccessory._part_depth * Vector3.forward);
                                        // vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.right, Space.World);
                                        // vertical_40x40_Bar.transform.Translate((assembly_tolerance / 2) * Vector3.forward, Space.World);
                                        vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                        break;
                                    case 2: //right bottom
                                        Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                        Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers_for_Divider_to_C.each_inner_field_width);
                                        Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counterL++.ToString("D4");

                                        vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                        vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counterBar++.ToString("D4");
                                        // vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.right, Space.World);
                                        // vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward, Space.World);
                                        vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                        accessory_naming_counter++;
                                        break;
                                }
                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = L_accessory_type;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                                characteristics_script.part_group = "Frame";

                                vertical_40x40_Bar.transform.Translate((verticalPartAccessory._part_depth + L_accessory_thickess) * Vector3.up);
                                vertical_40x40_Bar.transform.localRotation = Quaternion.Euler(-90, 90, 0);
                                vertical_40x40_Bar.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                vertical_40x40_Bar.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                characteristics_script = vertical_40x40_Bar.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = vertical_40x40_bar;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                                characteristics_script.part_group = "Field_2";

                                if (!Accessories_name.Contains(vertical_40x40_Bar.name))
                                    Accessories_name.Add(vertical_40x40_Bar.name);

                                if (!Accessories_name.Contains(Accessory_GO.name))
                                    Accessories_name.Add(Accessory_GO.name);
                            }
                            accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_C.each_inner_field_width);

                            if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                            {
                                no_fields = false;
                                //Fields
                                GameObject Fields_GO = new GameObject("Fields_" + (i + region1_fields_count).ToString("D4"));
                                Fields_GO.transform.parent = FieldGroup_GO.transform;
                                //Fields_GO.transform.Translate((rafafa_placement) * Vector3.forward);
                                for (int j = 0; j < NumOfRafafa; j++)
                                {
                                    GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                    Field_GO.GetComponent<Follow>().follow_child = Field_GO.transform.Find("Locator_1");
                                    Field_GO.GetComponent<Follow>().update_location_and_rotation();
                                    Field_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Clip").position.x,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_TopInside_for_Clip").position.y,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_TopInside_for_Clip").position.z);
                                    Field_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, -90, 0);
                                    Field_GO.GetComponent<Follow>().move_parent_relative_toChild();
                                    Field_GO.transform.GetChild(0).localScale = new Vector3(innerFieldDividers_for_Divider_to_C.each_inner_field_width - (assembly_tolerance / 2) - (2 * L_accessory_thickess), 1, 1);
                                    if (space_btw_rafafa == -10)
                                    {
                                        Field_GO.transform.GetChild(1).localScale = new Vector3(innerFieldDividers_for_Divider_to_C.each_inner_field_width - (2 * L_accessory_thickess), 1, 1);
                                        Field_GO.transform.GetChild(3).Translate((innerFieldDividers_for_Divider_to_C.each_inner_field_width - (L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(2).Translate(L_accessory_thickess * Vector3.right);
                                    }
                                    else
                                    {
                                        Field_GO.transform.GetChild(2).Translate((innerFieldDividers_for_Divider_to_C.each_inner_field_width - (L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(1).Translate(L_accessory_thickess * Vector3.right);
                                        Field_GO.transform.GetChild(2).GetChild(0).gameObject.AddComponent<BoxCollider>(); //for clips having spacing -10, box collider already added in prefab
                                    }

                                    Field_GO.transform.GetChild(0).GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    Field_GO.transform.GetChild(1).GetChild(0).gameObject.AddComponent<BoxCollider>();

                                    Field_GO.transform.Translate(-(clip_height_full - clip_spare_part_height) * j * Vector3.up);
                                    Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");


                                    // characteristics for Fields
                                    foreach (Transform t in Field_GO.transform)
                                    {
                                        characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                        characteristics_script.part_group = "Field_2";
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        if (t.name.Contains("40"))
                                        {
                                            characteristics_script.part_name_id = horizontal_type;
                                            characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                        }
                                        else if (t.name.Contains("39"))
                                        {
                                            characteristics_script.part_name_id = "ak - 39";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else if (t.name.Contains("72"))
                                        {
                                            characteristics_script.part_name_id = "ak - 72";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else if (t.name.Contains("76"))
                                        {
                                            characteristics_script.part_name_id = "ak - 76";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                    }
                                }

                                Fields_GO.transform.Translate(-h_Bar_offset * Vector3.right);
                                Fields_GO.transform.Translate((hit_DividertoE.distance + verticalPartSettings._part_depth) * Vector3.forward);
                                Fields_GO.transform.Translate((innerFieldDividers_for_Divider_to_C.each_inner_field_width + verticalPartSettings._part_depth) * (i - 1) * Vector3.forward);
                            }
                            else no_fields = true;

                            //FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                            //FieldGroup_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                        }



                        field_height = scale_f - (2 * framePartSettings._part_depth) - assembly_tolerance;
                        NumOfRafafa = Convert.ToInt32(System.Math.Floor((field_height - clip_spare_part_height) / clip_height_full));
                        h_Bar_offset = (field_height + clip_spare_part_height) - ((NumOfRafafa * (clip_height_full - clip_spare_part_height)));
                        h_Bar_offset = h_Bar_offset / 2;
                        if (h_Bar_offset <= 0)
                            h_Bar_offset = space_btw_rafafa;
                        FrameDividersForL frameDividersForL_dividertF = new FrameDividersForL(hit_DividertoF.distance, framePartSettings, verticalPartSettings);
                        InnerFieldDividers innerFieldDividers_for_Divider_to_F = new InnerFieldDividers(frameDividersForL_dividertF.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                        accessory_placement = verticalPartSettings._part_depth;
                        for (int i = 1; i <= region3_fields_count; i++)
                        {
                            GameObject FieldGroup_GO = new GameObject("Field_Group_" + (i + region1_fields_count + region2_fields_count).ToString("D4"));
                            FieldGroup_GO.transform.parent = Field_Parent_Section2.transform;

                            //Accessories
                            GameObject Accessories_parent = new GameObject("Accessories_" + (i + region1_fields_count + region2_fields_count).ToString("D4"));
                            Accessories_parent.transform.parent = FieldGroup_GO.transform;

                            // float X_pos_offset_for_DivF = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section2").Find("FrameDivider_Parent_1").Find("FrameDivider_1").transform.localPosition.x;
                            //  float Z_pos_offset_for_DivF = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section2").Find("FrameDivider_Parent_1").Find("FrameDivider_1").transform.localPosition.z;

                            for (int j = 1; j <= 2; j++)
                            {
                                GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                GameObject vertical_40x40_Bar = Instantiate(vertical_40x40_Bar_Prefab, Accessories_parent.transform);

                                switch (j)
                                {
                                    case 1: //left bottom
                                        Accessory_GO.transform.localRotation = Quaternion.Euler(-90, -90, 90);
                                        Accessory_GO.transform.localPosition = new Vector3(X_pos_offset_for_DivF - accessory_placement - innerFieldDividers_for_Divider_to_F.each_inner_field_width, 0, Z_pos_offset_for_DivF);
                                        // Accessory_GO.transform.localRotation = Quaternion.Euler(-90, 90, -90);
                                        //Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                        Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counterL++.ToString("D4");
                                        vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                        vertical_40x40_Bar.transform.localRotation = Quaternion.Euler(-90, 0, 180);
                                        vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counterBar++.ToString("D4");
                                        vertical_40x40_Bar.transform.Translate((verticalPartAccessory._part_width + L_accessory_thickess) * Vector3.up, Space.World);
                                        vertical_40x40_Bar.transform.Translate(-(verticalPartAccessory._part_width) * Vector3.forward, Space.World);

                                        // vertical_40x40_Bar.transform.Translate(verticalPartAccessory._part_depth * Vector3.forward);

                                        //vertical_40x40_Bar.transform.Translate((assembly_tolerance / 2) * Vector3.forward, Space.World);
                                        vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                        break;
                                    case 2: //right bottom
                                        Accessory_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                                        Accessory_GO.transform.localPosition = new Vector3(X_pos_offset_for_DivF - accessory_placement, 0, Z_pos_offset_for_DivF);

                                        //  Accessory_GO.transform.localRotation = Quaternion.Euler(-90, -90, 90);
                                        //Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers_for_Divider_to_F.each_inner_field_width - assembly_tolerance);
                                        Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counterL++.ToString("D4");


                                        vertical_40x40_Bar.transform.position = Accessory_GO.transform.position;
                                        vertical_40x40_Bar.name = vertical_40x40_bar + "_" + accessory_naming_counterBar++.ToString("D4");
                                        vertical_40x40_Bar.transform.Translate(-(verticalPartSettings._part_depth) * Vector3.right, Space.World);
                                        // vertical_40x40_Bar.transform.Translate(-((assembly_tolerance / 2) )* Vector3.right, Space.World);
                                        //vertical_40x40_Bar.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward, Space.World);
                                        vertical_40x40_Bar.transform.Translate(3 * Vector3.up, Space.World); //so that it do not conflict with clip
                                        accessory_naming_counter++;
                                        break;
                                }
                                Accessory_GO.transform.localScale = new Vector3(1, hit_AtoE.distance - assembly_tolerance, 1);
                                Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = L_accessory_type;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                                characteristics_script.part_group = "Frame";

                                vertical_40x40_Bar.transform.Translate((verticalPartAccessory._part_width + L_accessory_thickess) * Vector3.up);
                                vertical_40x40_Bar.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                                vertical_40x40_Bar.transform.localScale = new Vector3(1, hit_AtoE.distance - assembly_tolerance, 1);
                                vertical_40x40_Bar.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                characteristics_script = vertical_40x40_Bar.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = vertical_40x40_bar;
                                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                                characteristics_script.part_group = "Field_3";


                                if (!Accessories_name.Contains(vertical_40x40_Bar.name))
                                    Accessories_name.Add(vertical_40x40_Bar.name);

                                if (!Accessories_name.Contains(Accessory_GO.name))
                                    Accessories_name.Add(Accessory_GO.name);

                                //accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_F.each_inner_field_width);
                            }

                            accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_F.each_inner_field_width);
                            if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                            {
                                no_fields = false;

                                //Fields
                                GameObject Fields_GO = new GameObject("Fields_" + (i + region1_fields_count + region2_fields_count).ToString("D4"));
                                Fields_GO.transform.parent = FieldGroup_GO.transform;
                                for (int j = 0; j < NumOfRafafa; j++)
                                {
                                    GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                    Field_GO.GetComponent<Follow>().follow_child = Field_GO.transform.Find("Locator_1");
                                    Field_GO.GetComponent<Follow>().update_location_and_rotation();



                                    Field_GO.transform.Find("Locator_1").position = new Vector3(FrameDividers_Parent.transform.GetChild(1).GetChild(0).GetChild(0).transform.position.x,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_TopInside_for_Clip").position.y,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_TopInside_for_Clip").position.z);
                                    Field_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, 0, 0);
                                    Field_GO.GetComponent<Follow>().move_parent_relative_toChild();
                                    Field_GO.transform.GetChild(0).localScale = new Vector3(innerFieldDividers_for_Divider_to_F.each_inner_field_width - (assembly_tolerance / 2) - (2 * L_accessory_thickess), 1, 1);
                                    if (space_btw_rafafa == -10)
                                    {
                                        Field_GO.transform.GetChild(1).localScale = new Vector3(innerFieldDividers_for_Divider_to_F.each_inner_field_width - (assembly_tolerance / 2) - (2 * L_accessory_thickess), 1, 1);
                                        Field_GO.transform.GetChild(3).Translate((innerFieldDividers_for_Divider_to_F.each_inner_field_width - (L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(2).Translate((L_accessory_thickess) * Vector3.right);
                                    }
                                    else
                                    {
                                        Field_GO.transform.GetChild(2).Translate((innerFieldDividers_for_Divider_to_F.each_inner_field_width - (L_accessory_thickess) - 40) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(1).Translate((L_accessory_thickess) * Vector3.right);//40 is clip width
                                        Field_GO.transform.GetChild(2).GetChild(0).gameObject.AddComponent<BoxCollider>(); //for clips having spacing -10, box collider already added in prefab
                                    }

                                    Field_GO.transform.GetChild(0).GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    Field_GO.transform.GetChild(1).GetChild(0).gameObject.AddComponent<BoxCollider>();

                                    Field_GO.transform.Translate(-(clip_height_full - clip_spare_part_height) * j * Vector3.up);

                                    Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");


                                    // characteristics for Fields
                                    foreach (Transform t in Field_GO.transform)
                                    {
                                        characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                        characteristics_script.part_group = "Field_3";
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        if (t.name.Contains("40"))
                                        {
                                            characteristics_script.part_name_id = horizontal_type;
                                            characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                        }
                                        else if (t.name.Contains("39"))
                                        {
                                            characteristics_script.part_name_id = "ak - 39";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else if (t.name.Contains("72"))
                                        {
                                            characteristics_script.part_name_id = "ak - 72";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                        else if (t.name.Contains("76"))
                                        {
                                            characteristics_script.part_name_id = "ak - 76";
                                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                                        }
                                    }
                                }

                                Fields_GO.transform.Translate(h_Bar_offset * Vector3.forward);
                                Fields_GO.transform.Translate(-(innerFieldDividers_for_Divider_to_F.each_inner_field_width + verticalPartSettings._part_depth) * i * Vector3.right);
                            }
                            else no_fields = true;

                            // FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                            // FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.right);
                        }


                    }
                }
                else
                {
                    if (dsElement_pergola_header.Tables[0].Rows[0]["rafafa_placement_type"].ToString() == "type_2")
                    {
                        float field_height = 0;
                        int NumOfRafafa = 0;
                        float h_Bar_offset = 0;
                        if (!String.IsNullOrEmpty(horizontal_type))
                        {
                            field_height = hit_BtoD.distance - assembly_tolerance;
                            NumOfRafafa = Convert.ToInt32(System.Math.Floor((field_height + space_btw_rafafa) / (hPartSetting._part_height + space_btw_rafafa)));

                            //HORIZONTALS ALWAYS MIDDLE ALIGNED.
                            h_Bar_offset = field_height - ((NumOfRafafa * hPartSetting._part_height) + (space_btw_rafafa * (NumOfRafafa - 1)));
                            h_Bar_offset = h_Bar_offset / 2;

                            if (h_Bar_offset <= 0)
                                h_Bar_offset = space_btw_rafafa;
                        }
                        float rafafa_placement = 0;

                        FrameDividersForL frameDividersForL_dividertoe = new FrameDividersForL(hit_DividertoE.distance, framePartSettings, verticalPartSettings);
                        InnerFieldDividers innerFieldDividers_for_Divider_to_E = new InnerFieldDividers(frameDividersForL_dividertoe.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                        for (int i = 1; i <= region1_fields_count; i++)
                        {
                            GameObject FieldGroup_GO = new GameObject("Field_Group_" + i.ToString("D4"));
                            FieldGroup_GO.transform.parent = Field_Parent_Section1.transform;

                            //Accessories
                            GameObject Accessories_parent = new GameObject("Accessories_" + i.ToString("D4"));
                            Accessories_parent.transform.parent = FieldGroup_GO.transform;
                            if (place_u_accessory)
                            {
                                if (!String.IsNullOrEmpty(horizontal_type))
                                {
                                    for (int j = 1; j <= 2; j++)// L shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, -L_accessory_thickess, accessory_placement - assembly_tolerance / 2);
                                                Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, -L_accessory_thickess, accessory_placement + L_accessory_thickess + innerFieldDividers_for_Divider_to_E.each_inner_field_width - (assembly_tolerance / 2) - L_accessory_thickess);
                                                Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }
                                        Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = L_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        characteristics_script.part_group = "Frame";
                                    }

                                    for (int j = 1; j <= 2; j++) // U shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(U_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                                float U_accessory_height = Accessory_GO.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.z;
                                                Accessory_GO.transform.Translate(-U_accessory_height * Accessory_GO.transform.right);
                                                Accessory_GO.name = "U_Accessory_Left_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers_for_Divider_to_E.each_inner_field_width - assembly_tolerance);
                                                Accessory_GO.name = "U_Accessory_Right_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }

                                        Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = U_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        characteristics_script.part_group = "Field_1";
                                    }
                                }
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(horizontal_type))
                                {
                                    for (int j = 1; j <= 4; j++)// L shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                                Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward);
                                                Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");

                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers_for_Divider_to_E.each_inner_field_width - assembly_tolerance);
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                                Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                                                Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");

                                                break;
                                            case 3: //left top                                           
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, hPartSetting._part_depth + (2 * L_accessory_thickess), accessory_placement); //Accessory thickness is multiplied twice for bottom and top.so that it do not intersect with field
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                                Accessory_GO.name = "L_Accessory_topLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 4: //right top                                            
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(180, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, hPartSetting._part_depth + (2 * L_accessory_thickess), accessory_placement + innerFieldDividers_for_Divider_to_E.each_inner_field_width - assembly_tolerance);//Accessory thickness is multiplied twice for bottom and top.so that it do not intersect with field
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                                Accessory_GO.name = "L_Accessory_topRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = L_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        if (j == 1 || j == 2)
                                            characteristics_script.part_group = "Frame";
                                        else characteristics_script.part_group = "Field_1";
                                    }
                                }
                            }
                            accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_E.each_inner_field_width);

                            foreach (Transform t in Accessories_parent.transform)
                            {
                                if (!Accessories_name.Contains(t.name))
                                    Accessories_name.Add(t.name);
                            }

                            if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                            {
                                no_fields = false;
                                //Fields
                                GameObject Fields_GO = new GameObject("Fields_" + i.ToString("D4"));
                                Fields_GO.transform.parent = FieldGroup_GO.transform;
                                Fields_GO.transform.Translate(rafafa_placement * Vector3.forward);
                                float individual_field_placement = h_Bar_offset + framePartSettings._part_depth + hPartSetting._part_height;
                                for (int j = 0; j < NumOfRafafa; j++)
                                {
                                    GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                    Field_GO.transform.localRotation = Quaternion.Euler(-90, -90, 0);
                                    if (place_u_accessory)
                                    {
                                        Field_GO.transform.localPosition = new Vector3(-individual_field_placement, U_accessory_thickess, framePartSettings._part_depth + U_accessory_thickess);
                                        Field_GO.transform.localScale = new Vector3(innerFieldDividers_for_Divider_to_E.each_inner_field_width - assembly_tolerance - (2 * U_accessory_thickess), 1, 1);
                                    }
                                    else
                                    {
                                        Field_GO.transform.localPosition = new Vector3(-individual_field_placement, L_accessory_thickess, framePartSettings._part_depth + L_accessory_thickess);
                                        Field_GO.transform.localScale = new Vector3(innerFieldDividers_for_Divider_to_E.each_inner_field_width - assembly_tolerance - (2 * L_accessory_thickess), 1, 1);
                                    }

                                    Field_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");
                                    individual_field_placement += space_btw_rafafa + hPartSetting._part_height;
                                }
                                rafafa_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_E.each_inner_field_width);

                                // characteristics for Fields
                                foreach (Transform t in Fields_GO.transform)
                                {
                                    characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                    characteristics_script.part_name_id = horizontal_type;//frameBar_prefab.name;
                                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                    characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                    characteristics_script.part_group = "Field_1";
                                }
                            }
                            else no_fields = true;

                            FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                            FieldGroup_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                        }


                        FrameDividersForL frameDividersForL_dividertC = new FrameDividersForL(hit_DividertoC.distance, framePartSettings, verticalPartSettings);
                        InnerFieldDividers innerFieldDividers_for_Divider_to_C = new InnerFieldDividers(frameDividersForL_dividertC.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                        for (int i = 1; i <= region2_fields_count; i++)
                        {


                            GameObject FieldGroup_GO = new GameObject("Field_Group_" + (i + region1_fields_count).ToString("D4"));
                            FieldGroup_GO.transform.parent = Field_Parent_Section1.transform;

                            //Accessories
                            GameObject Accessories_parent = new GameObject("Accessories_" + (i + region1_fields_count).ToString("D4"));
                            Accessories_parent.transform.parent = FieldGroup_GO.transform;
                            if (place_u_accessory)
                            {
                                if (!String.IsNullOrEmpty(horizontal_type))
                                {
                                    for (int j = 1; j <= 2; j++)// L shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, -L_accessory_thickess, accessory_placement - assembly_tolerance / 2);
                                                Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, -L_accessory_thickess, accessory_placement + L_accessory_thickess + innerFieldDividers_for_Divider_to_C.each_inner_field_width - assembly_tolerance / 2 - L_accessory_thickess);
                                                Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }
                                        Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = L_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        characteristics_script.part_group = "Frame";
                                    }


                                    for (int j = 1; j <= 2; j++) // U shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(U_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                                float U_accessory_height = Accessory_GO.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.z;
                                                Accessory_GO.transform.Translate(-U_accessory_height * Accessory_GO.transform.right);
                                                Accessory_GO.name = "U_Accessory_Left_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers_for_Divider_to_C.each_inner_field_width - assembly_tolerance);
                                                Accessory_GO.name = "U_Accessory_Right_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }

                                        Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = U_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        characteristics_script.part_group = "Field_2";
                                    }
                                }
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(horizontal_type))
                                {
                                    for (int j = 1; j <= 4; j++)// L shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement);
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                                Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward);
                                                Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + innerFieldDividers_for_Divider_to_C.each_inner_field_width - assembly_tolerance);
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                                Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                                                Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 3: //left top                                           
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, hPartSetting._part_depth + (2 * L_accessory_thickess), accessory_placement); //Accessory thickness is multiplied twice for bottom and top.so that it do not intersect with field
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                                Accessory_GO.name = "L_Accessory_topLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 4: //right top                                            
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(180, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, hPartSetting._part_depth + (2 * L_accessory_thickess), accessory_placement + innerFieldDividers_for_Divider_to_C.each_inner_field_width - assembly_tolerance);//Accessory thickness is multiplied twice for bottom and top.so that it do not intersect with field
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                                Accessory_GO.name = "L_Accessory_topRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = L_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        if (j == 1 || j == 2)
                                            characteristics_script.part_group = "Frame";
                                        else characteristics_script.part_group = "Field_2";
                                    }
                                }
                            }
                            accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_C.each_inner_field_width);

                            foreach (Transform t in Accessories_parent.transform)
                            {
                                if (!Accessories_name.Contains(t.name))
                                    Accessories_name.Add(t.name);
                            }

                            if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                            {
                                no_fields = false;
                                //Fields
                                GameObject Fields_GO = new GameObject("Fields_" + (i + region1_fields_count).ToString("D4"));
                                Fields_GO.transform.parent = FieldGroup_GO.transform;
                                Fields_GO.transform.Translate(rafafa_placement * Vector3.forward);
                                float individual_field_placement = h_Bar_offset + framePartSettings._part_depth + hPartSetting._part_height;
                                for (int j = 0; j < NumOfRafafa; j++)
                                {
                                    GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                    Field_GO.transform.localRotation = Quaternion.Euler(-90, -90, 0);
                                    if (place_u_accessory)
                                    {
                                        Field_GO.transform.localPosition = new Vector3(-individual_field_placement, U_accessory_thickess, framePartSettings._part_depth + U_accessory_thickess);
                                        Field_GO.transform.localScale = new Vector3(innerFieldDividers_for_Divider_to_C.each_inner_field_width - assembly_tolerance - (2 * U_accessory_thickess), 1, 1);
                                    }
                                    else
                                    {
                                        Field_GO.transform.localPosition = new Vector3(-individual_field_placement, L_accessory_thickess, framePartSettings._part_depth + L_accessory_thickess);
                                        Field_GO.transform.localScale = new Vector3(innerFieldDividers_for_Divider_to_C.each_inner_field_width - assembly_tolerance - (2 * L_accessory_thickess), 1, 1);
                                    }

                                    Field_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");
                                    individual_field_placement += space_btw_rafafa + hPartSetting._part_height;
                                }
                                rafafa_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_C.each_inner_field_width);

                                // characteristics for Fields
                                foreach (Transform t in Fields_GO.transform)
                                {
                                    characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                    characteristics_script.part_name_id = horizontal_type;//frameBar_prefab.name;
                                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                    characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                    characteristics_script.part_group = "Field_2";
                                }
                            }
                            else no_fields = true;

                            FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                            FieldGroup_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                        }

                        field_height = 0;
                        NumOfRafafa = 0;
                        h_Bar_offset = 0;
                        if (!String.IsNullOrEmpty(horizontal_type))
                        {
                            field_height = hit_AtoE.distance - assembly_tolerance;
                            NumOfRafafa = Convert.ToInt32(System.Math.Floor((field_height + space_btw_rafafa) / (hPartSetting._part_height + space_btw_rafafa)));

                            //HORIZONTALS ALWAYS MIDDLE ALIGNED.
                            h_Bar_offset = field_height - ((NumOfRafafa * hPartSetting._part_height) + (space_btw_rafafa * (NumOfRafafa - 1)));
                            h_Bar_offset = h_Bar_offset / 2;

                            if (h_Bar_offset <= 0)
                                h_Bar_offset = space_btw_rafafa;
                        }
                        rafafa_placement = 0;
                        accessory_placement = 0;
                        FrameDividersForL frameDividersForL_dividertF = new FrameDividersForL(hit_DividertoF.distance, framePartSettings, verticalPartSettings);
                        InnerFieldDividers innerFieldDividers_for_Divider_to_F = new InnerFieldDividers(frameDividersForL_dividertF.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                        for (int i = 1; i <= region3_fields_count; i++)
                        {

                            GameObject FieldGroup_GO = new GameObject("Field_Group_" + (i + region1_fields_count + region2_fields_count).ToString("D4"));
                            FieldGroup_GO.transform.parent = Field_Parent_Section2.transform;

                            //Accessories
                            GameObject Accessories_parent = new GameObject("Accessories_" + (i + region1_fields_count + region2_fields_count).ToString("D4"));
                            Accessories_parent.transform.parent = FieldGroup_GO.transform;
                            if (place_u_accessory)
                            {
                                if (!String.IsNullOrEmpty(horizontal_type))
                                {
                                    for (int j = 1; j <= 2; j++)// L shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-innerFieldDividers_for_Divider_to_F.each_inner_field_width - verticalPartSettings._part_depth - accessory_placement - assembly_tolerance / 2, -L_accessory_thickess, -FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section2").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition.z + framePartSettings._part_depth + assembly_tolerance / 2);
                                                Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-accessory_placement - verticalPartSettings._part_depth - assembly_tolerance / 2, -L_accessory_thickess, -assembly_tolerance / 2);
                                                Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }
                                        Accessory_GO.transform.localScale = new Vector3(1, hit_AtoE.distance - assembly_tolerance, 1);
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = L_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        characteristics_script.part_group = "Frame";
                                    }

                                    for (int j = 1; j <= 2; j++) // U shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(U_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, 0, 0);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-(innerFieldDividers_for_Divider_to_F.each_inner_field_width) - accessory_placement - U_accessory_thickess + assembly_tolerance / 2, hPartSetting._part_depth + (U_accessory_thickess * 2), -FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section2").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition.z + framePartSettings._part_depth + assembly_tolerance / 2);
                                                // Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement );
                                                float U_accessory_height = Accessory_GO.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.z;
                                                Accessory_GO.transform.Translate(-U_accessory_height * Accessory_GO.transform.right);
                                                Accessory_GO.name = "U_Accessory_Left_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, -90, 90);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-verticalPartSettings._part_depth - assembly_tolerance - accessory_placement, hPartSetting._part_depth + (U_accessory_thickess * 2), -assembly_tolerance / 2);
                                                // Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + each_inner_field_width_fieldDividertoC - assembly_tolerance );
                                                Accessory_GO.name = "U_Accessory_Right_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }

                                        Accessory_GO.transform.localScale = new Vector3(1, hit_AtoE.distance - assembly_tolerance, 1);
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = U_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        characteristics_script.part_group = "Field_3";
                                    }
                                }
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(horizontal_type))
                                {
                                    for (int j = 1; j <= 4; j++)// L shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);

                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-(verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_F.each_inner_field_width) - accessory_placement, 0, 0);
                                                Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                                                Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-verticalPartSettings._part_depth - accessory_placement - assembly_tolerance, 0, 0);
                                                Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward);
                                                Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 3: //left top
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(180, -90, 90);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-(verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_F.each_inner_field_width) - accessory_placement, hPartSetting._part_depth + (2 * L_accessory_thickess), 0);
                                                Accessory_GO.name = "L_Accessory_topLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 4: //right top
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, -90, 90);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-verticalPartSettings._part_depth - accessory_placement - assembly_tolerance, hPartSetting._part_depth + (2 * L_accessory_thickess), 0);
                                                Accessory_GO.name = "L_Accessory_topRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }
                                        Accessory_GO.transform.localScale = new Vector3(1, hit_AtoE.distance - assembly_tolerance, 1);
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = L_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        if (j == 1 || j == 2)
                                            characteristics_script.part_group = "Frame";
                                        else characteristics_script.part_group = "Field_3";
                                    }
                                }
                            }
                            accessory_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_F.each_inner_field_width);
                            Accessories_parent.transform.Translate(assembly_tolerance * -Vector3.forward);
                            foreach (Transform t in Accessories_parent.transform)
                            {
                                if (!Accessories_name.Contains(t.name))
                                    Accessories_name.Add(t.name);
                            }

                            if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                            {
                                no_fields = false;
                                //Fields
                                GameObject Fields_GO = new GameObject("Fields_" + (i + region1_fields_count + region2_fields_count).ToString("D4"));
                                Fields_GO.transform.parent = FieldGroup_GO.transform;

                                if (i == 1)
                                    rafafa_placement = hit_BtoD.distance + framePartSettings._part_depth;// + verticalPartSettings._part_width;

                                rafafa_placement += (verticalPartSettings._part_depth + innerFieldDividers_for_Divider_to_F.each_inner_field_width);

                                Fields_GO.transform.Translate(rafafa_placement * -Vector3.right);
                                float individual_field_placement = h_Bar_offset + framePartSettings._part_depth;
                                for (int j = 0; j < NumOfRafafa; j++)
                                {
                                    GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                    Field_GO.transform.localRotation = Quaternion.Euler(90, 0, 0);
                                    if (place_u_accessory)
                                    {
                                        Field_GO.transform.localPosition = new Vector3(U_accessory_thickess, U_accessory_thickess + hPartSetting._part_depth, individual_field_placement);
                                        //Field_GO.transform.localPosition = new Vector3(-individual_field_placement, L_accessory_thickess + U_accessory_thickess, framePartSettings._part_depth + L_accessory_thickess + U_accessory_thickess);
                                        Field_GO.transform.localScale = new Vector3(innerFieldDividers_for_Divider_to_F.each_inner_field_width - assembly_tolerance - (2 * U_accessory_thickess), 1, 1);
                                    }
                                    else
                                    {
                                        //Field_GO.transform.localPosition = new Vector3(framePartSettings._part_depth + L_accessory_thickess ,- individual_field_placement, L_accessory_thickess );
                                        Field_GO.transform.localPosition = new Vector3(L_accessory_thickess, L_accessory_thickess + hPartSetting._part_depth, individual_field_placement);// framePartSettings._part_depth + L_accessory_thickess);
                                        Field_GO.transform.localScale = new Vector3(innerFieldDividers_for_Divider_to_F.each_inner_field_width - assembly_tolerance - (2 * L_accessory_thickess), 1, 1);
                                    }

                                    Field_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");
                                    individual_field_placement += space_btw_rafafa + hPartSetting._part_height;
                                }


                                //characteristics for Fields
                                foreach (Transform t in Fields_GO.transform)
                                {
                                    characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                    characteristics_script.part_name_id = horizontal_type;//frameBar_prefab.name;
                                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                    characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                    characteristics_script.part_group = "Field_3";
                                }
                            }
                            else no_fields = true;

                            FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                            FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.right);
                        }
                    }

                    else if (dsElement_pergola_header.Tables[0].Rows[0]["rafafa_placement_type"].ToString() == "type_3")
                    {
                        float field_height = 0;
                        int NumOfRafafa = 0;
                        float h_Bar_offset = 0;

                        FrameDividersForL frameDividersForL_dividertE = new FrameDividersForL(hit_BtoD.distance, framePartSettings, verticalPartSettings);
                        InnerFieldDividers DividertoE = new InnerFieldDividers(frameDividersForL_dividertE.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                        int total_number_of_fields_DividertoD = DividertoE.numberOfInnerDividerPoles;
                        int type3_region1_fields_count = frameDividersForL_dividertE.numbefOfFields * DividertoE.numbefOfInnerFields;

                        if (!String.IsNullOrEmpty(horizontal_type))
                        {
                            field_height = hit_DividertoE.distance - assembly_tolerance;
                            NumOfRafafa = Convert.ToInt32(System.Math.Floor((field_height + space_btw_rafafa) / (hPartSetting._part_height + space_btw_rafafa)));

                            //HORIZONTALS ALWAYS MIDDLE ALIGNED.
                            h_Bar_offset = field_height - ((NumOfRafafa * hPartSetting._part_height) + (space_btw_rafafa * (NumOfRafafa - 1)));
                            h_Bar_offset = h_Bar_offset / 2;

                            if (h_Bar_offset <= 0)
                                h_Bar_offset = space_btw_rafafa;
                        }

                        float rafafa_placement = 0;
                        for (int i = 1; i <= type3_region1_fields_count; i++)
                        {
                            GameObject FieldGroup_GO = new GameObject("Field_Group_" + i.ToString("D4"));
                            FieldGroup_GO.transform.parent = Field_Parent_Section2.transform;

                            //Accessories
                            GameObject Accessories_parent = new GameObject("Accessories_" + i.ToString("D4"));
                            Accessories_parent.transform.parent = FieldGroup_GO.transform;
                            if (place_u_accessory)
                            {
                                if (!String.IsNullOrEmpty(horizontal_type))
                                {

                                    for (int j = 1; j <= 2; j++)// L shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-accessory_placement - DividertoE.each_inner_field_width + assembly_tolerance / 2, -L_accessory_thickess, framePartSettings._part_depth);
                                                Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_DividertoE.distance - assembly_tolerance, 1);
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-accessory_placement + assembly_tolerance / 2, -L_accessory_thickess, framePartSettings._part_depth);
                                                Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");
                                                Accessory_GO.transform.localScale = new Vector3(1, -hit_DividertoE.distance - assembly_tolerance, 1);
                                                break;
                                        }
                                        // Accessory_GO.transform.localScale = new Vector3(1, hit_DividertoE.distance - assembly_tolerance, 1);
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = L_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        characteristics_script.part_group = "Frame";
                                    }

                                    for (int j = 1; j <= 2; j++) // U shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(U_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, 90, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-accessory_placement - DividertoE.each_inner_field_width + assembly_tolerance, hPartSetting._part_depth + (2 * U_accessory_thickess), framePartSettings._part_depth);
                                                Accessory_GO.name = "U_Accessory_Left_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(-90, 90, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-accessory_placement, 0, framePartSettings._part_depth);
                                                Accessory_GO.name = "U_Accessory_Right_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }

                                        Accessory_GO.transform.localScale = new Vector3(1, hit_DividertoE.distance - assembly_tolerance, 1);
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = U_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        characteristics_script.part_group = "Field_1";
                                    }
                                }
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(horizontal_type))
                                {
                                    for (int j = 1; j <= 4; j++)// L shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(-90, 90, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-accessory_placement, 0, framePartSettings._part_depth);
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_DividertoE.distance - assembly_tolerance, 1);
                                                Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                                                Accessory_GO.name = "L_Accessory_rightBottom_" + accessory_naming_counter++.ToString("D4"); // Right bottom
                                                break;
                                            case 2: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-accessory_placement - DividertoE.each_inner_field_width + assembly_tolerance / 2, 0, framePartSettings._part_depth);
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_DividertoE.distance - assembly_tolerance, 1);
                                                Accessory_GO.name = "L_Accessory_leftBottom_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 3: //right top
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(180, 90, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-accessory_placement, hPartSetting._part_depth + (2 * L_accessory_thickess), framePartSettings._part_depth); //Accessory thickness is multiplied twice for bottom and top.so that it do not intersect with field
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_DividertoE.distance - assembly_tolerance, 1);
                                                Accessory_GO.name = "L_Accessory_rightTop_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 4: //left top
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, 90, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-accessory_placement - DividertoE.each_inner_field_width + assembly_tolerance / 2, hPartSetting._part_depth + (2 * L_accessory_thickess), framePartSettings._part_depth);
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_DividertoE.distance - assembly_tolerance, 1);
                                                Accessory_GO.transform.Translate((assembly_tolerance / 2) * Vector3.right);
                                                Accessory_GO.name = "L_Accessory_leftTop_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = L_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        if (j == 1 || j == 2)
                                            characteristics_script.part_group = "Frame";
                                        else characteristics_script.part_group = "Field_1";
                                    }
                                }
                            }
                            accessory_placement += (verticalPartSettings._part_depth + DividertoE.each_inner_field_width);

                            foreach (Transform t in Accessories_parent.transform)
                            {
                                if (!Accessories_name.Contains(t.name))
                                    Accessories_name.Add(t.name);
                            }

                            if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                            {
                                no_fields = false;
                                //Fields
                                GameObject Fields_GO = new GameObject("Fields_" + i.ToString("D4"));
                                Fields_GO.transform.parent = FieldGroup_GO.transform;
                                Fields_GO.transform.Translate(-rafafa_placement * Vector3.right);
                                float individual_field_placement = h_Bar_offset + framePartSettings._part_depth + hPartSetting._part_height;// + assembly_tolerance;//check this
                                for (int j = 0; j < NumOfRafafa; j++)
                                {
                                    GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                    Field_GO.transform.localRotation = Quaternion.Euler(90, 180, 0);
                                    if (place_u_accessory)
                                    {
                                        Field_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth - U_accessory_thickess, U_accessory_thickess + hPartSetting._part_depth, individual_field_placement);
                                        Field_GO.transform.localScale = new Vector3(DividertoE.each_inner_field_width - assembly_tolerance - (2 * U_accessory_thickess), 1, 1);
                                    }
                                    else
                                    {
                                        Field_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth - L_accessory_thickess, L_accessory_thickess + hPartSetting._part_depth, individual_field_placement);
                                        Field_GO.transform.localScale = new Vector3(DividertoE.each_inner_field_width - assembly_tolerance - (2 * L_accessory_thickess), 1, 1);
                                    }

                                    Field_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");
                                    individual_field_placement += space_btw_rafafa + hPartSetting._part_height;
                                }
                                rafafa_placement += (verticalPartSettings._part_depth + DividertoE.each_inner_field_width);

                                // characteristics for Fields
                                foreach (Transform t in Fields_GO.transform)
                                {
                                    characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                    characteristics_script.part_name_id = horizontal_type;//frameBar_prefab.name;
                                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                    characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                    characteristics_script.part_group = "Field_1";

                                }
                            }
                            else no_fields = true;

                            FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                            FieldGroup_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                        }  //first section divider to E

                        FrameDividersForL frameDividersForL_dividertC = new FrameDividersForL(hit_DividertoC.distance, framePartSettings, verticalPartSettings);
                        InnerFieldDividers dividertoC = new InnerFieldDividers(frameDividersForL_dividertC.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                        int type3_region2_fields_count = frameDividersForL_dividertC.numbefOfFields * dividertoC.numbefOfInnerFields;

                        if (!String.IsNullOrEmpty(horizontal_type))
                        {
                            field_height = hit_BtoD.distance - assembly_tolerance;
                            NumOfRafafa = Convert.ToInt32(System.Math.Floor((field_height + space_btw_rafafa) / (hPartSetting._part_height + space_btw_rafafa)));

                            //HORIZONTALS ALWAYS MIDDLE ALIGNED.
                            h_Bar_offset = field_height - ((NumOfRafafa * hPartSetting._part_height) + (space_btw_rafafa * (NumOfRafafa - 1)));
                            h_Bar_offset = h_Bar_offset / 2;

                            if (h_Bar_offset <= 0)
                                h_Bar_offset = space_btw_rafafa;
                        }
                        rafafa_placement = 0;

                        accessory_placement = verticalPartSettings._part_depth;

                        for (int i = 1; i <= type3_region2_fields_count; i++)//sec 2 divider to c
                        {
                            GameObject FieldGroup_GO = new GameObject("Field_Group_" + (i + type3_region1_fields_count + type3_region2_fields_count).ToString("D4"));
                            FieldGroup_GO.transform.parent = Field_Parent_Section1.transform;

                            //Accessories
                            GameObject Accessories_parent = new GameObject("Accessories_" + (i + type3_region1_fields_count + type3_region2_fields_count).ToString("D4"));
                            Accessories_parent.transform.parent = FieldGroup_GO.transform;

                            float offset_z = vertical_bar_sideB.transform.position.z;

                            if (place_u_accessory)
                            {
                                if (!String.IsNullOrEmpty(horizontal_type))
                                {
                                    for (int j = 1; j <= 2; j++)// L shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, -L_accessory_thickess, offset_z + accessory_placement - assembly_tolerance / 2);
                                                Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, -L_accessory_thickess, offset_z + accessory_placement + L_accessory_thickess + dividertoC.each_inner_field_width - assembly_tolerance / 2 - L_accessory_thickess);
                                                Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }
                                        Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = L_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        characteristics_script.part_group = "Frame";
                                    }
                                    for (int j = 1; j <= 2; j++) // U shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(U_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, 0, 90);
                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + offset_z);
                                                float U_accessory_height = Accessory_GO.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.z;
                                                Accessory_GO.transform.Translate(-U_accessory_height * Accessory_GO.transform.right);
                                                Accessory_GO.name = "U_Accessory_Left_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);

                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + offset_z + dividertoC.each_inner_field_width - assembly_tolerance);
                                                Accessory_GO.name = "U_Accessory_Right_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }

                                        Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = U_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        characteristics_script.part_group = "Field_2";

                                    }
                                }
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(horizontal_type))
                                {
                                    for (int j = 1; j <= 4; j++)// L shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);

                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + offset_z);
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                                Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward);
                                                Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(270, 0, 90);

                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + offset_z + dividertoC.each_inner_field_width - assembly_tolerance);
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                                Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                                                Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 3: //left top                                           
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, 0, 90);

                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, hPartSetting._part_depth + (2 * L_accessory_thickess), accessory_placement + offset_z); //Accessory thickness is multiplied twice for bottom and top.so that it do not intersect with field
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                                Accessory_GO.name = "L_Accessory_topLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 4: //right top                                            
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(180, 0, 90);

                                                Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, hPartSetting._part_depth + (2 * L_accessory_thickess), accessory_placement + offset_z + dividertoC.each_inner_field_width - assembly_tolerance);//Accessory thickness is multiplied twice for bottom and top.so that it do not intersect with field
                                                Accessory_GO.transform.localScale = new Vector3(1, hit_BtoD.distance - assembly_tolerance, 1);
                                                Accessory_GO.name = "L_Accessory_topRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = L_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();

                                        if (j == 1 || j == 2)
                                            characteristics_script.part_group = "Frame";
                                        else characteristics_script.part_group = "Field_2";
                                    }
                                }
                            }
                            accessory_placement += (verticalPartSettings._part_depth + dividertoC.each_inner_field_width);  // each_inner_field_width_fieldDividertoC);

                            foreach (Transform t in Accessories_parent.transform)
                            {
                                if (!Accessories_name.Contains(t.name))
                                    Accessories_name.Add(t.name);
                            }

                            if (!String.IsNullOrEmpty(horizontal_type)) //checking for hollow pergola
                            {
                                no_fields = false;
                                //Fields
                                GameObject Fields_GO = new GameObject("Fields_" + (i + type3_region1_fields_count + type3_region2_fields_count).ToString("D4"));
                                Fields_GO.transform.parent = FieldGroup_GO.transform;
                                if (i == 1)
                                    rafafa_placement = hit_DividertoE.distance + verticalPartSettings._part_depth;//TODO: fix here and in type2 also

                                // rafafa_placement += (verticalPartSettings._part_depth + each_inner_field_width_fieldDividertoF);

                                Fields_GO.transform.Translate(rafafa_placement * Vector3.forward);
                                float individual_field_placement = h_Bar_offset + framePartSettings._part_depth + hPartSetting._part_height;
                                for (int j = 0; j < NumOfRafafa; j++)
                                {
                                    GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                    Field_GO.transform.localRotation = Quaternion.Euler(-90, -90, 0);
                                    if (place_u_accessory)
                                    {
                                        Field_GO.transform.localPosition = new Vector3(-individual_field_placement, U_accessory_thickess, framePartSettings._part_depth + U_accessory_thickess);
                                        Field_GO.transform.localScale = new Vector3(dividertoC.each_inner_field_width - assembly_tolerance - (2 * U_accessory_thickess), 1, 1);
                                    }
                                    else
                                    {
                                        Field_GO.transform.localPosition = new Vector3(-individual_field_placement, L_accessory_thickess, framePartSettings._part_depth + L_accessory_thickess);
                                        Field_GO.transform.localScale = new Vector3(dividertoC.each_inner_field_width - assembly_tolerance - (2 * L_accessory_thickess), 1, 1);
                                    }

                                    Field_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");
                                    individual_field_placement += space_btw_rafafa + hPartSetting._part_height;
                                }
                                rafafa_placement += (verticalPartSettings._part_depth + dividertoC.each_inner_field_width);



                                // characteristics for Fields
                                foreach (Transform t in Fields_GO.transform)
                                {
                                    characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                    characteristics_script.part_name_id = horizontal_type;//frameBar_prefab.name;
                                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                    characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                    characteristics_script.part_group = "Field_2";

                                }
                            }
                            else no_fields = true;

                            FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                            FieldGroup_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                        } //section 2 divider to c


                        if (!String.IsNullOrEmpty(horizontal_type))
                        {
                            field_height = hit_AtoE.distance - assembly_tolerance;
                            NumOfRafafa = Convert.ToInt32(System.Math.Floor((field_height + space_btw_rafafa) / (hPartSetting._part_height + space_btw_rafafa)));

                            //HORIZONTALS ALWAYS MIDDLE ALIGNED.
                            h_Bar_offset = field_height - ((NumOfRafafa * hPartSetting._part_height) + (space_btw_rafafa * (NumOfRafafa - 1)));
                            h_Bar_offset = h_Bar_offset / 2;

                            if (h_Bar_offset <= 0)
                                h_Bar_offset = space_btw_rafafa;
                        }
                        rafafa_placement = 0;
                        accessory_placement = 0;

                        FrameDividersForL frameDividersForL_dividertF = new FrameDividersForL(hit_DividertoF.distance, framePartSettings, verticalPartSettings);
                        InnerFieldDividers dividertoF = new InnerFieldDividers(frameDividersForL_dividertF.each_field_width, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                        int type3_region3_fields_count = frameDividersForL_dividertF.numbefOfFields * dividertoF.numbefOfInnerFields;


                        int dividertoF_total_no_of_fields = dividertoF.numberOfInnerDividerPoles;
                        for (int i = 1; i <= type3_region3_fields_count; i++)
                        {
                            GameObject FieldGroup_GO = new GameObject("Field_Group_" + (i + type3_region1_fields_count).ToString("D4"));
                            FieldGroup_GO.transform.parent = Field_Parent_Section2.transform;

                            //Accessories
                            GameObject Accessories_parent = new GameObject("Accessories_" + (i + type3_region1_fields_count).ToString("D4"));
                            Accessories_parent.transform.parent = FieldGroup_GO.transform;
                            if (place_u_accessory)
                            {
                                if (!String.IsNullOrEmpty(horizontal_type))
                                {
                                    for (int j = 1; j <= 2; j++)// L shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-dividertoF.each_inner_field_width - verticalPartSettings._part_depth - accessory_placement - assembly_tolerance / 2, -L_accessory_thickess, -FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section2").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition.z + framePartSettings._part_depth + assembly_tolerance / 2);
                                                Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-accessory_placement - verticalPartSettings._part_depth - assembly_tolerance / 2, -L_accessory_thickess, -assembly_tolerance / 2);
                                                Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }
                                        Accessory_GO.transform.localScale = new Vector3(1, hit_AtoE.distance - assembly_tolerance, 1);
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = L_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        characteristics_script.part_group = "Frame";
                                    }

                                    for (int j = 1; j <= 2; j++) // U shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(U_accessoryBar_prefab, Accessories_parent.transform);
                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, 0, 0);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-(each_inner_field_width_fieldDividertoF) - accessory_placement - U_accessory_thickess + assembly_tolerance / 2, hPartSetting._part_depth + (U_accessory_thickess * 2), -FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section2").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition.z + framePartSettings._part_depth + assembly_tolerance / 2);
                                                // Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement );
                                                float U_accessory_height = Accessory_GO.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.z;
                                                Accessory_GO.transform.Translate(-U_accessory_height * Accessory_GO.transform.right);
                                                Accessory_GO.name = "U_Accessory_Left_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, -90, 90);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-verticalPartSettings._part_depth - accessory_placement - assembly_tolerance, hPartSetting._part_depth + (U_accessory_thickess * 2), -assembly_tolerance / 2);
                                                // Accessory_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, accessory_placement + each_inner_field_width_fieldDividertoC - assembly_tolerance );
                                                Accessory_GO.name = "U_Accessory_Right_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }

                                        Accessory_GO.transform.localScale = new Vector3(1, hit_AtoE.distance - assembly_tolerance, 1);//Todo: fix here
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = U_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        characteristics_script.part_group = "Field_3";

                                    }
                                }
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(horizontal_type))
                                {
                                    for (int j = 1; j <= 4; j++)// L shape accessory
                                    {
                                        GameObject Accessory_GO = Instantiate(L_accessoryBar_prefab, Accessories_parent.transform);

                                        switch (j)
                                        {
                                            case 1: //left bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-(verticalPartSettings._part_depth + each_inner_field_width_fieldDividertoF) - accessory_placement, 0, 0);
                                                Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.right);
                                                Accessory_GO.name = "L_Accessory_bottomLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 2: //right bottom
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-verticalPartSettings._part_depth - accessory_placement - assembly_tolerance, 0, 0);
                                                Accessory_GO.transform.Translate(-(assembly_tolerance / 2) * Vector3.forward);
                                                Accessory_GO.name = "L_Accessory_bottomRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 3: //left top
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(180, -90, 90);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-(verticalPartSettings._part_depth + each_inner_field_width_fieldDividertoF) - accessory_placement, hPartSetting._part_depth + (2 * L_accessory_thickess), 0);
                                                Accessory_GO.name = "L_Accessory_topLeft_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                            case 4: //right top
                                                Accessory_GO.transform.localRotation = Quaternion.Euler(90, -90, 90);
                                                Accessory_GO.transform.localPosition = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section_0002").Find("FrameDivider_Parent_0001").Find("FrameDivider_0001").transform.localPosition;
                                                Accessory_GO.transform.localPosition += new Vector3(-verticalPartSettings._part_depth - accessory_placement - assembly_tolerance, hPartSetting._part_depth + (2 * L_accessory_thickess), 0);
                                                Accessory_GO.name = "L_Accessory_topRight_" + accessory_naming_counter++.ToString("D4");
                                                break;
                                        }
                                        Accessory_GO.transform.localScale = new Vector3(1, hit_AtoE.distance - assembly_tolerance, 1); //Todo: fix here
                                        Accessory_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                        characteristics_script = Accessory_GO.AddComponent<Characteristics>();
                                        characteristics_script.part_name_id = L_accessory_type;
                                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                                        if (j == 1 || j == 2)
                                            characteristics_script.part_group = "Frame";
                                        else characteristics_script.part_group = "Field_3";

                                    }
                                }
                            }
                            accessory_placement += verticalPartSettings._part_depth + dividertoF.each_inner_field_width;
                            Accessories_parent.transform.Translate(assembly_tolerance * -Vector3.forward);
                            foreach (Transform t in Accessories_parent.transform)
                            {
                                if (!Accessories_name.Contains(t.name))
                                    Accessories_name.Add(t.name);
                            }

                            if (!String.IsNullOrEmpty(horizontal_type))//checking for hollow pergola
                            {
                                no_fields = false;
                                //Fields
                                GameObject Fields_GO = new GameObject("Fields_" + (i + type3_region1_fields_count).ToString("D4"));
                                Fields_GO.transform.parent = FieldGroup_GO.transform;

                                if (i == 1)
                                    rafafa_placement = hit_BtoD.distance + framePartSettings._part_depth;//TODO: fix here and in type2 also

                                rafafa_placement += (verticalPartSettings._part_depth + each_inner_field_width_fieldDividertoF);

                                Fields_GO.transform.Translate(rafafa_placement * -Vector3.right);
                                float individual_field_placement = h_Bar_offset + framePartSettings._part_depth;// + hPartSetting._part_height;
                                for (int j = 0; j < NumOfRafafa; j++)
                                {
                                    GameObject Field_GO = Instantiate(fieldBar_prefab, Fields_GO.transform);
                                    //Field_GO.transform.localRotation = Quaternion.Euler(90, 180, 0);
                                    Field_GO.transform.localRotation = Quaternion.Euler(90, 0, 0);
                                    //Field_GO.transform.RotateAround(Field_GO.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center, Vector3.right, 90);
                                    if (place_u_accessory)
                                    {
                                        Field_GO.transform.localPosition = new Vector3(U_accessory_thickess, U_accessory_thickess + hPartSetting._part_depth, individual_field_placement);
                                        //Field_GO.transform.localPosition = new Vector3(-individual_field_placement, L_accessory_thickess + U_accessory_thickess, framePartSettings._part_depth + L_accessory_thickess + U_accessory_thickess);
                                        Field_GO.transform.localScale = new Vector3(each_inner_field_width_fieldDividertoF - assembly_tolerance - (2 * U_accessory_thickess), 1, 1);
                                    }
                                    else
                                    {
                                        //Field_GO.transform.localPosition = new Vector3(framePartSettings._part_depth + L_accessory_thickess ,- individual_field_placement, L_accessory_thickess );
                                        Field_GO.transform.localPosition = new Vector3(L_accessory_thickess, L_accessory_thickess + hPartSetting._part_depth, individual_field_placement);// framePartSettings._part_depth + L_accessory_thickess);
                                        Field_GO.transform.localScale = new Vector3(each_inner_field_width_fieldDividertoF - assembly_tolerance - (2 * L_accessory_thickess), 1, 1);
                                    }

                                    Field_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                                    Field_GO.name = "Field_" + field_naming_counter++.ToString("D4");
                                    individual_field_placement += space_btw_rafafa + hPartSetting._part_height;
                                }


                                //characteristics for Fields
                                foreach (Transform t in Fields_GO.transform)
                                {
                                    characteristics_script = t.gameObject.AddComponent<Characteristics>();
                                    characteristics_script.part_name_id = horizontal_type;//frameBar_prefab.name;
                                    characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                                    characteristics_script.part_type = part_type_enum.horizontal.ToString();
                                    characteristics_script.part_group = "Field_3";
                                    //this is for hbar, L,U Accessory, Divider + Clamp+screw

                                }
                            }
                            else no_fields = true;


                            FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.forward);
                            FieldGroup_GO.transform.Translate((assembly_tolerance / 2) * Vector3.right);
                        }  //section 3 divider to F


                    }

                }

                #endregion

                try
                {
                    await UnityMainThreadDispatcher.DispatchAsync(() => step_cut(frame_type, divider_type));
                    #region step_Cut
                    //Frame_Parent = GameObject.Find("Frame_Parent");
                    //foreach (Transform frames in Frame_Parent.transform)
                    //{
                    //    GameObject mesh_ch = frames.GetChild(0).gameObject;

                    //    mesh_ch.gameObject.layer = frame_layer;


                    //}

                    //foreach (Transform Div_ch in Divider_Parent.transform)
                    //{
                    //    foreach (Transform frm_field_ch in Div_ch)
                    //    {


                    //        foreach (Transform divs_par in frm_field_ch)
                    //        {
                    //            GameObject div = null;
                    //            if (divs_par.childCount > 0)
                    //                div = divs_par.GetChild(0).gameObject;
                    //            if (div != null)
                    //            {

                    //                if (crown_names.Contains(frame_type))
                    //                {
                    //                    float val = div.transform.localScale.y, step_cut_width = 0;
                    //                    bool step_cut = false;
                    //                    Vector3 dir = div.transform.up;
                    //                    Bounds bound_div_bar = div.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    //                    float max_depth = Mathf.Max(bound_div_bar.size.x, bound_div_bar.size.y, bound_div_bar.size.z);
                    //                    if (max_depth > 80)
                    //                    {
                    //                        //continuation
                    //                        GameObject type_q_pf = (GameObject)Resources.Load($"prefabs/{divider_type}_type1", typeof(GameObject));
                    //                        if (type_q_pf != null)
                    //                        {
                    //                            Bounds t1_bounds;//= type_q_pf.GetComponentInChildren<MeshFilter>().mesh.bounds;
                    //                            GameObject loc1 = div.transform.Find("Locator_1").gameObject;
                    //                            GameObject loc2 = div.transform.Find("Locator_2").gameObject;
                    //                            RaycastHit hit_frameRfield;

                    //                            //If frame is hit we can place the cut part of the divider for crown
                    //                            if (Physics.Raycast(div.transform.TransformPoint(bound_div_bar.center), (-div.transform.up), out hit_frameRfield, Mathf.Infinity))//, 1 << frame_layer.value))
                    //                            {
                    //                                Debug.DrawRay(div.transform.TransformPoint(bound_div_bar.center), -div.transform.up * 10000, Color.blue, 20f);
                    //                                if (hit_frameRfield.transform.parent != null)
                    //                                    if (hit_frameRfield.transform.parent.name.ToLower().Contains("frame") && !hit_frameRfield.transform.parent.name.ToLower().Contains("divider"))
                    //                                    {
                    //                                        GameObject t1 = GameObject.Instantiate(type_q_pf);
                    //                                        Follow follow = t1.AddComponent<Follow>();
                    //                                        t1.transform.Find("Locator").position = loc1.transform.position;
                    //                                        t1.transform.Find("Locator").rotation = loc1.transform.rotation;
                    //                                        follow.follow_child = t1.transform.Find("Locator");
                    //                                        t1.name = type_q_pf.name + "_div_1";
                    //                                        // follow.update_location_and_rotation();
                    //                                        follow.move_parent_relative_toChild();
                    //                                        //t1.transform.position=  div.transform.Find("Locator_1").transform.position;
                    //                                        t1.transform.parent = div.transform;

                    //                                        //rotate_around_center_rotateAround(t2, Vector3.up, 180);
                    //                                        t1_bounds = t1.GetComponentInChildren<MeshFilter>().mesh.bounds;
                    //                                        //step cut characteristics
                    //                                        step_cut = true;
                    //                                        step_cut_width += t1_bounds.size.y;

                    //                                    }
                    //                            }


                    //                            RaycastHit hit_frameRfield_2;
                    //                            //If frame is hit we can place the cut part of the divider for crown
                    //                            if (Physics.Raycast(div.transform.TransformPoint(bound_div_bar.center), (div.transform.up), out hit_frameRfield_2, Mathf.Infinity))//, 1 << frame_layer.value))
                    //                            {
                    //                                Debug.DrawRay(div.transform.TransformPoint(bound_div_bar.center), div.transform.up * 10000, Color.red, 20f);
                    //                                if (hit_frameRfield_2.transform.parent != null)
                    //                                    if (hit_frameRfield_2.transform.parent.name.ToLower().Contains("frame") && !hit_frameRfield_2.transform.parent.name.ToLower().Contains("divider"))
                    //                                    {

                    //                                        GameObject t2 = GameObject.Instantiate(type_q_pf);
                    //                                        Follow follow_2 = t2.AddComponent<Follow>();
                    //                                        t2.transform.Find("Locator").position = loc1.transform.position;
                    //                                        t2.transform.Find("Locator").rotation = loc1.transform.rotation;
                    //                                        follow_2.follow_child = t2.transform.Find("Locator");
                    //                                        follow_2.move_parent_relative_toChild();
                    //                                        t2.name = type_q_pf.name + "_div_2";
                    //                                        //rotate_around_center_rotateAround(t2, Vector3.up, 180);
                    //                                        //Bounds t2_bounds = t2.GetComponentInChildren<MeshFilter>().mesh.bounds;
                    //                                        t1_bounds = t2.GetComponentInChildren<MeshFilter>().mesh.bounds;

                    //                                        t2.transform.Translate(div.transform.InverseTransformDirection(-div.transform.up) * (Mathf.Abs(val) + t1_bounds.size.y));
                    //                                        //follow.update_location_and_rotation();
                    //                                        //t2.transform.position=  div.transform.Find("Locator_2").transform.position;
                    //                                        t2.transform.parent = div.transform;
                    //                                        //step cut characteristics
                    //                                        step_cut = true;
                    //                                        step_cut_width += t1_bounds.size.y;
                    //                                    }
                    //                            }

                    //                            Characteristics chrs = div.gameObject.GetComponent<Characteristics>();


                    //                            if (chrs == null)
                    //                            {

                    //                                chrs = div.gameObject.AddComponent<Characteristics>();

                    //                            }
                    //                            chrs.part_type = part_type_enum.vertical.ToString();
                    //                            chrs.part_name_id = divider_type;
                    //                            chrs.part_unique_id = Guid.NewGuid().ToString();
                    //                            if (step_cut == true)
                    //                            {
                    //                                chrs.step_cut = step_cut;

                    //                                chrs.step_cut_width = step_cut_width;
                    //                            }
                    //                        }

                    //                    }

                    //                }

                    //                var mc = div.gameObject.AddComponent<MeshCombiner>();
                    //                mc.CreateMultiMaterialMesh = true;
                    //                mc.DestroyCombinedChildren = true;
                    //                mc.CombineMeshes(false);
                    //                mc.transform.gameObject.AddComponent<BoxCollider>();
                    //                mc.gameObject.layer = divider_layer;

                    //                Probuilderize_gameObject(div.transform);
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                }
                catch (Exception divider_part_add)
                {

                    print("Divider Parts :" + divider_part_add);
                }

                try
                {
                    frame_C_clamp_groove_Alignment_();
                    #region Groove Alignment
                    //GameObject first_170000048_Front_0 = GameObject.Find("170000048_Front_0");

                    //if (first_170000048_Front_0 != null)
                    //{
                    //    GameObject first_170000048_Locator_groove = first_170000048_Front_0.transform.Find("Locator_groove").gameObject;



                    //    GameObject FrameD;// GameObject frame_C;
                    //    if (GameObject.Find("FrameD_0"))
                    //    {
                    //        FrameD = GameObject.Find("FrameD_0");
                    //    }
                    //    else
                    //    {
                    //        FrameD = GameObject.Find("FrameD");
                    //    }
                    //    GameObject FrameD_locator_groove = FrameD.transform.Find("Locator_groove").gameObject;

                    //    if (FrameD_locator_groove != null)
                    //    {
                    //        float dist = FrameD_locator_groove.transform.position.y - first_170000048_Locator_groove.transform.position.y;

                    //        print("Distance between Grooves : " + dist);

                    //        Pergola_Model = GameObject.Find("Pergola_Model");
                    //        foreach (Transform ch in Pergola_Model.transform)
                    //        {
                    //            if (!(ch.name.Contains("Frames_Parent")))//||ch.name.Contains("Wall_Parent")||ch.name.Contains("SupportBars_Parent")
                    //            {
                    //                print(ch.name + ": pos =" + ch.position + "before");
                    //                ch.transform.Translate(Vector3.up * dist, Space.World);
                    //                // ch.transform.position = ch.transform.position + new Vector3(0, dist, 0);
                    //                print(ch.name + ": pos =" + ch.position + "after");
                    //            }
                    //        }
                    //    }


                    //}

                    #endregion
                }
                catch (Exception move_dividers_groove)
                {

                    //throw;
                    print("Divider Parts function call" + move_dividers_groove);
                }


                try
                {

                    #region Support Bars 

                    #region Support bar for divider section 1

                    SupportBars_Parent = new GameObject("SupportBars_Parent");
                    SupportBars_Parent.transform.parent = Pergola_Model.transform;

                    int supportbar_components_naming_counter = 1;
                    if (support_line_placement == "full")
                        supportBarLengths = new SupportBarLengths(scale_b);
                    else supportBarLengths = new SupportBarLengths(manual_support_bar_distance);

                    SupportBars_Parent_Section2 = new GameObject("SupportBars_Parent_Section_0001");
                    SupportBars_Parent_Section2.transform.parent = SupportBars_Parent.transform;
                    wall_on_side_C = wall_sides.Contains("C");

                    for (int i = 1; i <= Number_of_dividers_in_section_2 + 2; i++)
                    {
                        GameObject SupportBar_Parent = new GameObject("SupportBar_Parent_" + i.ToString("D4"));
                        SupportBar_Parent.transform.parent = SupportBars_Parent_Section2.transform;

                        //Clamps on Frame
                        GameObject Clamp_onFrame_GO = null;
                        if (support_line_placement == "full")
                        {
                            if (i == Number_of_dividers_in_section_2 + 2)
                            {
                                Clamp_onFrame_GO = Instantiate(l_clamp_onFrame_full_Prefab, SupportBar_Parent.transform);//l_clamp_onFrame_full_Prefab
                                characteristics_script = Clamp_onFrame_GO.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = "ALL.00.04_TwoThird";
                            }
                            else
                            {
                                Clamp_onFrame_GO = Instantiate(T_clamp_onFramePrefab, SupportBar_Parent.transform);
                                characteristics_script = Clamp_onFrame_GO.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = t_clamp_onFrame;
                            }
                        }
                        else if (support_line_placement == "manual" || support_line_placement == "two_third")
                        {
                            Clamp_onFrame_GO = Instantiate(l_clamp_onFrame_twoThird_Prefab, SupportBar_Parent.transform);
                            characteristics_script = Clamp_onFrame_GO.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = "ALL.00.04_TwoThird";
                        }

                        // Clamp_onFrame_GO.transform.parent = SupportBar_Parent.transform;

                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";



                        if (i == 1)//first clamp hard coded on the frame 
                        {
                            if (support_line_placement == "full")
                                Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(0, 0, -90);
                            else
                                Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, 0, -90);

                            Clamp_onFrame_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_Top").position.x,
                                                                                                FrameDividers_Parent.transform.GetChild(0).GetChild(0).GetChild(0).transform.Find("Locator_Top").position.y,
                                                                                                FrameDividers_Parent.transform.GetChild(0).GetChild(0).GetChild(0).transform.Find("Locator_Top").position.z);
                            Clamp_onFrame_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            if (support_line_placement == "manual" || support_line_placement == "two_third")
                            {
                                Clamp_onFrame_GO.transform.Translate(-(scale_c - manual_support_bar_distance) * Vector3.up);//-scale_c * (1 - (2f / 3f))
                            }
                        }
                        else if (i == Number_of_dividers_in_section_2 + 2) //last clamp
                        {
                            Clamp_onFrame_GO.transform.GetComponent<Follow>().follow_child = Clamp_onFrame_GO.transform.Find("Locator_2").transform;
                            Clamp_onFrame_GO.transform.GetComponent<Follow>().update_location_and_rotation();
                            Clamp_onFrame_GO.transform.Find("Locator_2").localRotation = Quaternion.Euler(-90, 0, -90);
                            Clamp_onFrame_GO.transform.Find("Locator_2").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_Top").position.x,
                                                                                              vertical_bar_sideD.transform.Find("Locator_Top").position.y,
                                                                                              vertical_bar_sideC.transform.Find("Locator_Top").position.z);
                            Clamp_onFrame_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();

                            if (support_line_placement == "manual" || support_line_placement == "two_third")
                            {
                                Clamp_onFrame_GO.transform.Translate(-(scale_c - manual_support_bar_distance) * Vector3.up);
                            }
                        }
                        else
                        {
                            if (support_line_placement == "manual" || support_line_placement == "two_third")
                                Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, 0, 90);
                            else
                                Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(0, 0, -90);

                            if (dsElement_pergola_header.Tables[0].Rows[0]["rafafa_placement_type"].ToString() == "type_2")
                            {
                                Clamp_onFrame_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_Top").position.x,
                                           FrameDividers_Parent.transform.GetChild(0).GetChild(supportBarCount_in_Region1 + 1).GetChild(0).transform.Find("Locator_Top").position.y,
                                           FrameDividers_Parent.transform.GetChild(0).GetChild(supportBarCount_in_Region1 + 1).GetChild(0).transform.Find("Locator_Top").position.z);
                            }
                            else
                            {
                                Clamp_onFrame_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_Top").position.x,
                                           FrameDividers_Parent.transform.GetChild(0).GetChild(i - 1).GetChild(0).transform.Find("Locator_Top").position.y,
                                           FrameDividers_Parent.transform.GetChild(0).GetChild(i - 1).GetChild(0).transform.Find("Locator_Top").position.z);
                            }



                            Clamp_onFrame_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();

                            if (support_line_placement == "manual" || support_line_placement == "two_third")
                                Clamp_onFrame_GO.transform.Translate((scale_c - manual_support_bar_distance) * Vector3.up);//scale_c * (1 - (2f / 3f))
                            else
                            {
                                characteristics_script.part_name_id = t_clamp_onFrame;
                            }
                        }



                        Clamp_onFrame_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        Clamp_onFrame_GO.name = "Clamp_onFrame_" + supportbar_components_naming_counter.ToString("D4");
                        GameObject Clamp_onWall_GO = null;
                        if (i == 1)
                        {
                            Clamp_onWall_GO = Instantiate(l_clamp_onWallPrefab_for_l, FieldDividers_Parent.transform);
                            Clamp_onWall_GO.transform.parent = SupportBar_Parent.transform;
                            Clamp_onWall_GO.transform.localPosition = new Vector3(0, Clamp_onFrame_GO.transform.localPosition.y, Clamp_onFrame_GO.transform.localPosition.z);
                            //if (i == 1)
                            Clamp_onWall_GO.transform.Translate((-scale_c) * Vector3.right);
                            Clamp_onWall_GO.transform.Translate(5 * Vector3.right);//adjustment from clamp corner 

                            Clamp_onWall_GO.transform.Translate(supportBarLengths.supportWall_length * Vector3.up);
                            Clamp_onWall_GO.transform.localRotation = Quaternion.Euler(270, -90, 0);
                            Clamp_onWall_GO.transform.localPosition = new Vector3(Clamp_onWall_GO.transform.position.x, Clamp_onWall_GO.transform.position.y, vertical_bar_sideA.transform.position.z - 5);//the -5 is adjustment for the clamp 
                            Clamp_onWall_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            Clamp_onWall_GO.name = "Clamp_onWall_" + supportbar_components_naming_counter.ToString("D4");
                            characteristics_script = Clamp_onWall_GO.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = l_clamp_onWall;//frameBar_prefab.name;
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_group = "Frame";
                        }
                        else
                        {
                            Clamp_onWall_GO = Instantiate(l_clamp_onWallPrefab, FieldDividers_Parent.transform);
                            Clamp_onWall_GO.transform.parent = SupportBar_Parent.transform;
                            Clamp_onWall_GO.transform.localPosition = new Vector3(0, Clamp_onFrame_GO.transform.localPosition.y, Clamp_onFrame_GO.transform.localPosition.z);
                            //if (i == 1)
                            Clamp_onWall_GO.transform.Translate((-scale_c) * Vector3.right);


                            Clamp_onWall_GO.transform.Translate(supportBarLengths.supportWall_length * Vector3.up);
                            Clamp_onWall_GO.transform.localRotation = Quaternion.Euler(270, -90, 0);
                            Clamp_onWall_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            Clamp_onWall_GO.name = "Clamp_onWall_" + supportbar_components_naming_counter.ToString("D4");
                            characteristics_script = Clamp_onWall_GO.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = l_clamp_onWall;//frameBar_prefab.name;
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_group = "Frame";
                        }


                        GameObject Flat_Plate_GO = null;
                        if (i == 1)
                        {
                            GameObject Flat_Plate_prefab = (GameObject)Resources.Load($"prefabs/Flat plate", typeof(GameObject));
                            Flat_Plate_GO = Instantiate(Flat_Plate_prefab);
                            Flat_Plate_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            Flat_Plate_GO.transform.parent = SupportBar_Parent.transform;
                            Flat_Plate_GO.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            // Flat_Plate_GO.transform.GetChild(1).transform.position = ConnectingBar_PartA_GO.transform.GetChild(2).position;
                            Flat_Plate_GO.transform.GetChild(2).transform.position = Clamp_onWall_GO.transform.GetChild(5).position;
                            Flat_Plate_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            Flat_Plate_GO.name = "Flat_Plate_GO" + supportbar_components_naming_counter.ToString("D4");

                        }

                        //Telescope
                        GameObject telescope_GO = Instantiate(telescope_prefab, FieldDividers_Parent.transform);
                        telescope_GO.transform.parent = SupportBar_Parent.transform;
                        telescope_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        telescope_GO.name = "Telescope_" + supportbar_components_naming_counter.ToString("D4");
                        // telescope_GO.transform.GetChild(0).GetChild(0).transform.up = Clamp_onFrame_GO.transform.up;
                        telescope_GO.transform.GetChild(0).GetChild(0).position = Clamp_onFrame_GO.transform.GetChild(1).position;
                        if (i == 1)
                            telescope_GO.transform.GetChild(0).GetChild(0).LookAt(Flat_Plate_GO.transform.GetChild(2));//this alligns forward vector towards target, we need UP vector to be aligned
                        else
                            telescope_GO.transform.GetChild(0).GetChild(0).LookAt(Clamp_onWall_GO.transform.GetChild(1)); //this alligns forward vector towards target, we need UP vector to be aligned

                        telescope_GO.transform.GetChild(0).GetChild(0).RotateAround(telescope_GO.transform.GetChild(0).GetChild(0).position, telescope_GO.transform.GetChild(0).GetChild(0).right, 90);//aligning up vector towards target
                        telescope_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();

                        characteristics_script = telescope_GO.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = telescope;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";




                        //SupportBar_PartA
                        GameObject ConnectingBar_PartA_prefab = (GameObject)Resources.Load($"prefabs/supportBart_PartA", typeof(GameObject));
                        GameObject ConnectingBar_PartA_GO = Instantiate(ConnectingBar_PartA_prefab);
                        ConnectingBar_PartA_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        ConnectingBar_PartA_GO.transform.parent = SupportBar_Parent.transform;
                        if (i == 1)
                        {
                            ConnectingBar_PartA_GO.transform.GetChild(2).position = Flat_Plate_GO.transform.GetChild(1).position;
                            Flat_Plate_GO.transform.Translate(Vector3.forward * 5);//moving to the connecting bar part A thickness 
                        }
                        else
                        {
                            ConnectingBar_PartA_GO.transform.GetChild(2).position = Clamp_onWall_GO.transform.GetChild(1).position;
                        }
                        ConnectingBar_PartA_GO.transform.GetChild(2).LookAt(Clamp_onFrame_GO.transform.GetChild(1));
                        ConnectingBar_PartA_GO.transform.GetChild(2).RotateAround(ConnectingBar_PartA_GO.transform.GetChild(2).position, ConnectingBar_PartA_GO.transform.GetChild(2).right, 90);
                        ConnectingBar_PartA_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();
                        ConnectingBar_PartA_GO.name = "ConnectingBar_PartA_" + supportbar_components_naming_counter.ToString("D4");
                        ConnectingBar_PartA_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        characteristics_script = ConnectingBar_PartA_GO.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = "supportBart_PartA";
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.none.ToString();
                        characteristics_script.part_group = "none";


                        //Suppport Bar
                        GameObject ConnectingBar_prefab = (GameObject)Resources.Load($"prefabs/supportBar", typeof(GameObject));
                        GameObject ConnectingBar_GO = Instantiate(ConnectingBar_prefab);
                        ConnectingBar_GO.transform.GetChild(0).gameObject.AddComponent<CapsuleCollider>();
                        ConnectingBar_GO.transform.parent = SupportBar_Parent.transform;
                        ConnectingBar_GO.transform.GetChild(1).position = ConnectingBar_PartA_GO.transform.GetChild(1).position;
                        ConnectingBar_GO.transform.GetChild(1).LookAt(Clamp_onFrame_GO.transform.GetChild(1));
                        ConnectingBar_GO.transform.GetChild(1).RotateAround(ConnectingBar_GO.transform.GetChild(1).position, ConnectingBar_GO.transform.GetChild(1).right, 90);
                        ConnectingBar_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();
                        Vector3 telescope_pos = telescope_GO.transform.Find("Locater_top").position;
                        Vector3 connecting_bar_pos = ConnectingBar_PartA_GO.transform.Find("Cube_supportBart_PartA_edge").position;
                        float dist = Vector3.Distance(connecting_bar_pos, telescope_pos);
                        //if(i==1)
                        //ConnectingBar_GO.transform.localScale = new Vector3(1, dist-90, 1);//-90 is flat plate length 
                        //else
                        ConnectingBar_GO.transform.localScale = new Vector3(1, dist, 1);
                        //ConnectingBar_GO.transform.localScale = new Vector3(1, supportBarLengths.supportBar_length - 89, 1); //removing 89mm for PartA supportBar
                        ConnectingBar_GO.name = "ConnectingBar_" + supportbar_components_naming_counter.ToString("D4");

                        characteristics_script = ConnectingBar_GO.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = "supportBar";
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                        characteristics_script.part_group = "Frame";



                        supportbar_components_naming_counter++;

                    }
                    #endregion

                    #region support bar for divider Section 2

                    if (support_line_placement == "full")
                        supportBarLengths = new SupportBarLengths(scale_b);
                    else supportBarLengths = new SupportBarLengths(manual_support_bar_distance);

                    SupportBars_Parent_Section3 = new GameObject("SupportBars_Parent_Section_0002");
                    SupportBars_Parent_Section3.transform.parent = SupportBars_Parent.transform;
                    wall_on_side_C = wall_sides.Contains("C");

                    for (int i = 1; i <= Number_of_dividers_in_section_3 + 2; i++)
                    {
                        GameObject SupportBar_Parent = new GameObject("SupportBar_Parent_" + supportbar_components_naming_counter.ToString("D4"));
                        SupportBar_Parent.transform.parent = SupportBars_Parent_Section3.transform;

                        //Clamps on Frame
                        GameObject Clamp_onFrame_GO = null;
                        if (support_line_placement == "full")
                        {
                            if (i == Number_of_dividers_in_section_3 + 2)
                            {
                                Clamp_onFrame_GO = Instantiate(l_clamp_onFrame_full_Prefab, SupportBar_Parent.transform);
                                characteristics_script = Clamp_onFrame_GO.AddComponent<Characteristics>();
                                characteristics_script.part_name_id = "ALL.00.04_TwoThird";
                            }
                            else
                                Clamp_onFrame_GO = Instantiate(T_clamp_onFramePrefab, SupportBar_Parent.transform);
                            characteristics_script = Clamp_onFrame_GO.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = t_clamp_onFrame;

                        }
                        else if (support_line_placement == "manual" || support_line_placement == "two_third")
                        {
                            Clamp_onFrame_GO = Instantiate(l_clamp_onFrame_twoThird_Prefab, SupportBar_Parent.transform);
                            characteristics_script = Clamp_onFrame_GO.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = "ALL.00.04_TwoThird";
                        }
                        // Clamp_onFrame_GO.transform.parent = SupportBar_Parent.transform;

                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";


                        if (i == 1)//first clamp hard coded on the frame 
                        {
                            if (support_line_placement == "full")
                                Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(0, 90, -90);
                            else
                                Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, 90, -90);

                            Clamp_onFrame_GO.transform.Find("Locator_1").position = new Vector3(FrameDividers_Parent.transform.GetChild(1).GetChild(0).GetChild(0).transform.Find("Locator_Top").position.x,
                                                                                                 FrameDividers_Parent.transform.GetChild(1).GetChild(0).GetChild(0).transform.Find("Locator_Top").position.y,
                                                                                                  vertical_bar_sideE.transform.Find("Locator_Top").position.z);
                            Clamp_onFrame_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            if (support_line_placement == "manual" || support_line_placement == "two_third")
                            {
                                Clamp_onFrame_GO.transform.Translate(-(scale_f - manual_support_bar_distance) * Vector3.up);//scale_f * (1 - (2f / 3f))
                            }
                        }
                        else if (i == Number_of_dividers_in_section_3 + 2) //last clamp
                        {
                            Clamp_onFrame_GO.transform.GetComponent<Follow>().follow_child = Clamp_onFrame_GO.transform.Find("Locator_1").transform;
                            Clamp_onFrame_GO.transform.GetComponent<Follow>().update_location_and_rotation();
                            Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, 0, -180);
                            Clamp_onFrame_GO.transform.Find("Locator_1").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_Top").position.x,
                                                                                              vertical_bar_sideE.transform.Find("Locator_Top").position.y,
                                                                                              vertical_bar_sideE.transform.Find("Locator_Top").position.z);
                            Clamp_onFrame_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();

                            if (support_line_placement == "manual" || support_line_placement == "two_third")
                            {
                                Clamp_onFrame_GO.transform.Translate((scale_f - manual_support_bar_distance) * Vector3.up);//scale_f * (1 - (2f / 3f)) 
                            }
                        }
                        else
                        {
                            if (support_line_placement == "full")
                                Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(0, 90, -90);
                            else
                                Clamp_onFrame_GO.transform.Find("Locator_1").localRotation = Quaternion.Euler(-90, 0, 0);

                            if (dsElement_pergola_header.Tables[0].Rows[0]["rafafa_placement_type"].ToString() == "type_3")
                            {
                                Clamp_onFrame_GO.transform.Find("Locator_1").position = new Vector3(FrameDividers_Parent.transform.GetChild(1).GetChild(supportBarCount_in_Region2 + i - 2).GetChild(0).transform.Find("Locator_Top").position.x,
                                                                                                         FrameDividers_Parent.transform.GetChild(1).GetChild(supportBarCount_in_Region2 + i - 2).GetChild(0).transform.Find("Locator_Top").position.y,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_Top").position.z);
                            }
                            else
                            {
                                Clamp_onFrame_GO.transform.Find("Locator_1").position = new Vector3(FrameDividers_Parent.transform.GetChild(1).GetChild(i - 1).GetChild(0).transform.Find("Locator_Top").position.x,
                                                                                                    FrameDividers_Parent.transform.GetChild(1).GetChild(i - 1).GetChild(0).transform.Find("Locator_Top").position.y,
                                                                                                    vertical_bar_sideE.transform.Find("Locator_Top").position.z);

                            }



                            Clamp_onFrame_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();

                            if (support_line_placement == "manual" || support_line_placement == "two_third")
                                Clamp_onFrame_GO.transform.Translate(-(scale_f - manual_support_bar_distance) * Vector3.up);//scale_f * (1 - (2f / 3f))
                            else
                            {
                                characteristics_script.part_name_id = t_clamp_onFrame;
                            }
                        }


                        Clamp_onFrame_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        Clamp_onFrame_GO.name = "Clamp_onFrame_" + supportbar_components_naming_counter.ToString("D4");
                        GameObject Clamp_onWall_GO = null;
                        if (i == 1)
                        {
                            Clamp_onWall_GO = Instantiate(l_clamp_onWallPrefab_for_l, FieldDividers_Parent.transform);
                            Clamp_onWall_GO.transform.parent = SupportBar_Parent.transform;
                            Clamp_onWall_GO.transform.localPosition = new Vector3(Clamp_onFrame_GO.transform.localPosition.x, Clamp_onFrame_GO.transform.localPosition.y, 0);
                            //Clamp_onWall_GO.transform.Translate((-scale_f + framePartSettings._part_depth) * Vector3.right);

                            if (support_line_placement == "full")
                                Clamp_onWall_GO.transform.Translate((-scale_f) * -Vector3.forward, Space.Self);
                            else
                                //    Clamp_onWall_GO.transform.Translate(-(scale_f * 0.666667f) * -Vector3.forward,Space.Self);
                                Clamp_onWall_GO.transform.Translate((-scale_f) * -Vector3.forward, Space.Self);

                            Clamp_onWall_GO.transform.Translate(supportBarLengths.supportWall_length * Vector3.up);
                            Clamp_onWall_GO.transform.localRotation = Quaternion.Euler(270, 0, 0);
                            // Clamp_onWall_GO.transform.up = Clamp_onFrame_GO.transform.up;
                            Clamp_onWall_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            Clamp_onWall_GO.name = "Clamp_onWall_" + supportbar_components_naming_counter.ToString("D4");
                            characteristics_script = Clamp_onWall_GO.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = l_clamp_onWall;//frameBar_prefab.name;
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_group = "Frame";
                        }
                        else
                        {
                            Clamp_onWall_GO = Instantiate(l_clamp_onWallPrefab, FieldDividers_Parent.transform);
                            Clamp_onWall_GO.transform.parent = SupportBar_Parent.transform;
                            Clamp_onWall_GO.transform.localPosition = new Vector3(Clamp_onFrame_GO.transform.localPosition.x, Clamp_onFrame_GO.transform.localPosition.y, 0);
                            //Clamp_onWall_GO.transform.Translate((-scale_f + framePartSettings._part_depth) * Vector3.right);

                            if (support_line_placement == "full")
                                Clamp_onWall_GO.transform.Translate((-scale_f) * -Vector3.forward, Space.Self);
                            else
                                //    Clamp_onWall_GO.transform.Translate(-(scale_f * 0.666667f) * -Vector3.forward,Space.Self);
                                Clamp_onWall_GO.transform.Translate((-scale_f) * -Vector3.forward, Space.Self);

                            Clamp_onWall_GO.transform.Translate(supportBarLengths.supportWall_length * Vector3.up);
                            Clamp_onWall_GO.transform.localRotation = Quaternion.Euler(270, 0, 0);
                            // Clamp_onWall_GO.transform.up = Clamp_onFrame_GO.transform.up;
                            Clamp_onWall_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            Clamp_onWall_GO.name = "Clamp_onWall_" + supportbar_components_naming_counter.ToString("D4");
                            characteristics_script = Clamp_onWall_GO.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = l_clamp_onWall;//frameBar_prefab.name;
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_group = "Frame";
                        }

                        // GameObject Clamp_onWall_GO = Instantiate(l_clamp_onWallPrefab_for_l, FieldDividers_Parent.transform);

                        //Clamps on Wall

                        GameObject Flat_Plate_GO = null;
                        if (i == 1)
                        {
                            GameObject Flat_Plate_prefab = (GameObject)Resources.Load($"prefabs/Flat plate", typeof(GameObject));
                            Flat_Plate_GO = Instantiate(Flat_Plate_prefab);
                            Flat_Plate_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                            Flat_Plate_GO.transform.parent = SupportBar_Parent.transform;
                            Flat_Plate_GO.transform.localRotation = Quaternion.Euler(0, -90, -180);
                            Flat_Plate_GO.transform.GetChild(2).transform.position = SupportBars_Parent_Section2.transform.GetChild(0).GetChild(1).GetChild(6).transform.position;
                            Flat_Plate_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            Flat_Plate_GO.name = "Flat_Plate_GO" + supportbar_components_naming_counter.ToString("D4");


                        }

                        //Telescope
                        GameObject telescope_GO = Instantiate(telescope_prefab, FieldDividers_Parent.transform);
                        telescope_GO.transform.parent = SupportBar_Parent.transform;
                        telescope_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        telescope_GO.name = "Telescope_" + supportbar_components_naming_counter.ToString("D4");

                        telescope_GO.transform.GetChild(0).GetChild(0).position = Clamp_onFrame_GO.transform.GetChild(1).position;
                        if (i == 1)
                            telescope_GO.transform.GetChild(0).GetChild(0).LookAt(Flat_Plate_GO.transform.GetChild(2)); //this alligns forward vector towards target, we need UP vector to be aligned
                        else
                            telescope_GO.transform.GetChild(0).GetChild(0).LookAt(Clamp_onWall_GO.transform.GetChild(1));

                        telescope_GO.transform.GetChild(0).GetChild(0).RotateAround(telescope_GO.transform.GetChild(0).GetChild(0).position, telescope_GO.transform.GetChild(0).GetChild(0).right, 90);//aligning up vector towards target
                        telescope_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();

                        characteristics_script = telescope_GO.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = telescope;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";



                        //SupportBar_PartA
                        GameObject ConnectingBar_PartA_prefab = (GameObject)Resources.Load($"prefabs/supportBart_PartA", typeof(GameObject));
                        GameObject ConnectingBar_PartA_GO = Instantiate(ConnectingBar_PartA_prefab);
                        ConnectingBar_PartA_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        ConnectingBar_PartA_GO.transform.parent = SupportBar_Parent.transform;
                        if (i == 1)
                        {
                            ConnectingBar_PartA_GO.transform.GetChild(2).position = Flat_Plate_GO.transform.GetChild(1).position;
                            Flat_Plate_GO.transform.Translate(Vector3.forward * 5);//moving to the connecting bar part A thickness 
                        }
                        else
                        {
                            ConnectingBar_PartA_GO.transform.GetChild(2).position = Clamp_onWall_GO.transform.GetChild(1).position;
                        }
                        ConnectingBar_PartA_GO.transform.GetChild(2).LookAt(Clamp_onFrame_GO.transform.GetChild(1));
                        ConnectingBar_PartA_GO.transform.GetChild(2).RotateAround(ConnectingBar_PartA_GO.transform.GetChild(2).position, ConnectingBar_PartA_GO.transform.GetChild(2).right, 90);
                        ConnectingBar_PartA_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();
                        ConnectingBar_PartA_GO.name = "ConnectingBar_PartA_" + supportbar_components_naming_counter.ToString("D4");
                        ConnectingBar_PartA_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        characteristics_script = ConnectingBar_PartA_GO.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = "supportBart_PartA";
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.none.ToString();
                        characteristics_script.part_group = "none";


                        //Suppport Bar
                        GameObject ConnectingBar_prefab = (GameObject)Resources.Load($"prefabs/supportBar", typeof(GameObject));
                        GameObject ConnectingBar_GO = Instantiate(ConnectingBar_prefab);
                        ConnectingBar_GO.transform.GetChild(0).gameObject.AddComponent<CapsuleCollider>();
                        ConnectingBar_GO.transform.parent = SupportBar_Parent.transform;
                        ConnectingBar_GO.transform.GetChild(1).position = ConnectingBar_PartA_GO.transform.GetChild(1).position;
                        ConnectingBar_GO.transform.GetChild(1).LookAt(Clamp_onFrame_GO.transform.GetChild(1));
                        ConnectingBar_GO.transform.GetChild(1).RotateAround(ConnectingBar_GO.transform.GetChild(1).position, ConnectingBar_GO.transform.GetChild(1).right, 90);
                        ConnectingBar_GO.transform.GetComponent<Follow>().move_parent_relative_toChild();
                        Vector3 telescope_pos = telescope_GO.transform.Find("Locater_top").position;
                        Vector3 connecting_bar_pos = ConnectingBar_PartA_GO.transform.Find("Cube_supportBart_PartA_edge").position;
                        float dist = Vector3.Distance(connecting_bar_pos, telescope_pos);
                        ConnectingBar_GO.transform.localScale = new Vector3(1, dist, 1);
                        ConnectingBar_GO.name = "ConnectingBar_" + supportbar_components_naming_counter.ToString("D4");

                        characteristics_script = ConnectingBar_GO.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = "supportBar";
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.vertical.ToString();
                        characteristics_script.part_group = "Frame";

                        supportbar_components_naming_counter++;



                        if (i == 1)
                            Destroy(Clamp_onWall_GO);
                    }

                    #endregion

                    if (side_c_wall == 1)
                    {
                        GameObject SupportBars_Parent_Section1 = SupportBars_Parent.transform.Find("SupportBars_Parent_Section1").gameObject;
                        #region child_count logic
                        //if (SupportBars_Parent_Section1 != null)
                        //{
                        //    int ch_count = SupportBars_Parent_Section1.transform.childCount;

                        //    if(SupportBars_Parent_Section1.transform.GetChild(ch_count - 1).gameObject!=null)
                        //    DestroyImmediate(SupportBars_Parent_Section1.transform.GetChild(ch_count - 1).gameObject);//GetChild(SupportBars_Parent.transform.childCount - 1).gameObject) ;
                        //}
                        #endregion
                        DestroyImmediate(GetClosest_SupportBar(SupportBars_Parent_Section1.transform, GameObject.Find("FrameC").transform));
                        GameObject L_Accesory_nearWall_GO = Instantiate(ak_79_L_accessory_prefab, Frames_Parent.transform.GetChild(1));
                        L_Accesory_nearWall_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        L_Accesory_nearWall_GO.transform.localScale = new Vector3(1, 1, -scale_a);
                        L_Accesory_nearWall_GO.transform.localPosition = new Vector3(-(scale_c + 2), -2, scale_f);
                        L_Accesory_nearWall_GO.transform.localRotation = Quaternion.Euler(0, -90, 0);
                        L_Accesory_nearWall_GO.name = ak_79 + "_sideA";
                    }

                    if (side_f_wall == 1)
                    {

                        GameObject SupportBars_Parent_Section2 = SupportBars_Parent.transform.Find("SupportBars_Parent_Section2").gameObject;
                        #region child_count logic
                        //if (SupportBars_Parent_Section2 != null)
                        //{
                        //    int ch_count = SupportBars_Parent_Section2.transform.childCount;

                        //    if (SupportBars_Parent_Section2.transform.GetChild(ch_count - 1).gameObject != null)
                        //        DestroyImmediate(SupportBars_Parent_Section2.transform.GetChild(ch_count - 1).gameObject);//GetChild(SupportBars_Parent.transform.childCount - 1).gameObject) ;
                        //}

                        #endregion
                        DestroyImmediate(GetClosest_SupportBar(SupportBars_Parent_Section2.transform, GameObject.Find("FrameF").transform));
                        //DestroyImmediate(SupportBars_Parent.transform.GetChild(SupportBars_Parent.transform.childCount - 1).gameObject);
                        GameObject L_Accesory_nearWall_GO = Instantiate(ak_79_L_accessory_prefab, Frames_Parent.transform.GetChild(1));
                        L_Accesory_nearWall_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        L_Accesory_nearWall_GO.transform.localScale = new Vector3(1, 1, -scale_c);
                        L_Accesory_nearWall_GO.transform.localRotation = Quaternion.Euler(0, -90, 0);
                        L_Accesory_nearWall_GO.transform.Translate(Vector3.forward * scale_d, Space.World);
                        L_Accesory_nearWall_GO.transform.Translate(Vector3.right * 2);
                        L_Accesory_nearWall_GO.transform.Translate(-Vector3.up * 2);
                        L_Accesory_nearWall_GO.name = ak_79 + "_sideC";
                    }

                    if (side_a_wall == 1 && side_b_wall == 1 && side_c_wall == 1 && side_d_wall == 1 && side_e_wall == 1 && side_f_wall == 1)
                    {
                        foreach (Transform supp_par in SupportBars_Parent.transform)
                        {
                            foreach (Transform supp_sec in supp_par)
                            {
                                DestroyImmediate(supp_sec.gameObject);
                            }
                        }
                    }
                    #endregion

                }
                catch (Exception ex)
                {

                    print("Support bar section: " + ex);
                }


            }


            else if (Convert.ToInt32(dsElement_pergola_header.Tables[0].Rows[0]["element_shape_id"]) == 2)//U Shape
            {
                U_type = true;

                float scale_a = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[0]["side_value"]); //side A
                float scale_b = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[1]["side_value"]); //side B
                float scale_c = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[2]["side_value"]); //side C
                float scale_d = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[3]["side_value"]); //side D
                float scale_e = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[4]["side_value"]); //side E
                float scale_f = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[5]["side_value"]); //side F
                float scale_g = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[6]["side_value"]); //side G
                float scale_h = Convert.ToSingle(dsElement_pergola_details.Tables[0].Rows[7]["side_value"]); //side H

                int side_a_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[0]["is_fixed_to_wall"]);
                int side_b_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[1]["is_fixed_to_wall"]);
                int side_c_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[2]["is_fixed_to_wall"]);
                int side_d_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[3]["is_fixed_to_wall"]);
                int side_e_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[4]["is_fixed_to_wall"]);
                int side_f_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[5]["is_fixed_to_wall"]);
                int side_g_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[6]["is_fixed_to_wall"]);
                int side_h_wall = Convert.ToInt32(dsElement_pergola_details.Tables[0].Rows[7]["is_fixed_to_wall"]);

                if (side_a_wall == 1)
                    wall_sides.Add("A");

                if (side_b_wall == 1)
                    wall_sides.Add("B");

                if (side_c_wall == 1)
                    wall_sides.Add("C");

                if (side_d_wall == 1)
                    wall_sides.Add("D");

                if (side_e_wall == 1)
                    wall_sides.Add("E");

                if (side_f_wall == 1)
                    wall_sides.Add("F");

                if (side_g_wall == 1)
                    wall_sides.Add("G");

                if (side_h_wall == 1)
                    wall_sides.Add("H");

                //assigning the total length of the frame to Global variables to be used in other scripts
                frame_A_length = scale_a;
                frame_B_length = scale_b;
                frame_C_length = scale_c;
                frame_D_length = scale_d;
                frame_E_length = scale_e;
                frame_F_length = scale_f;
                frame_G_length = scale_g;
                frame_H_length = scale_h;

                FrameDividers frameDividers = new FrameDividers(scale_c, framePartSettings, verticalPartSettings,MAX_SUPPORTBAR_DIVIDER_DISTANCE);

                #region Instantiation and positioning Frame
                GameObject vertical_bar_sideC = null, vertical_bar_sideB = null, vertical_bar_sideA = null, vertical_bar_sideD = null, vertical_bar_sideE = null, vertical_bar_sideF = null, vertical_bar_sideG = null, vertical_bar_sideH = null;
                Characteristics characteristics_script = null;

                Frame_Parent = new GameObject("Frame_Parent");
                Frame_Parent.transform.parent = Frames_Parent.transform;


                vertical_bar_sideD = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideD.transform.localPosition = new Vector3(0, 0, scale_d);
                vertical_bar_sideD.transform.localRotation = Quaternion.Euler(0, -90, 90);
                vertical_bar_sideD.transform.localScale = new Vector3(1, scale_d, 1);
                vertical_bar_sideD.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideD.name = "FrameD";
                characteristics_script = vertical_bar_sideD.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, 135";
                characteristics_script.right_end_cut_angle = "270, 0, -135";
                characteristics_script.icon_filename = "frame45-45.jpg";
                Vector3 cube_left_pos = vertical_bar_sideD.transform.Find("Cube_left").transform.position;
                Vector3 cube_right_pos = vertical_bar_sideD.transform.Find("Cube_right").transform.position;
                characteristics_script.cube_left_pos = cube_left_pos.x + "," + cube_left_pos.y + "," + cube_left_pos.z;
                characteristics_script.cube_right_pos = cube_right_pos.x + "," + cube_right_pos.y + "," + cube_right_pos.z;
                sliceGO_script.Slice_two_sides(vertical_bar_sideD, new Vector3(270, 0, 135), new Vector3(270, 0, -135));
                vertical_bar_sideD = Frame_Parent.transform.Find("FrameD").gameObject; //because original will be destroyed after slicing


                vertical_bar_sideC = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideC.transform.localPosition = new Vector3(-scale_c, 0, scale_d); //to offset
                vertical_bar_sideC.transform.localRotation = Quaternion.Euler(0, 180, 90);
                vertical_bar_sideC.transform.localScale = new Vector3(1, scale_c, 1);
                vertical_bar_sideC.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideC.name = "FrameC";
                characteristics_script = vertical_bar_sideC.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, -135";
                characteristics_script.right_end_cut_angle = "270, 0, 135";
                characteristics_script.icon_filename = "frameRight-4545.jpg";
                cube_left_pos = vertical_bar_sideC.transform.Find("Cube_left").transform.position;
                cube_right_pos = vertical_bar_sideC.transform.Find("Cube_right").transform.position;
                characteristics_script.cube_left_pos = cube_left_pos.x + "," + cube_left_pos.y + "," + cube_left_pos.z;
                characteristics_script.cube_right_pos = cube_right_pos.x + "," + cube_right_pos.y + "," + cube_right_pos.z;
                sliceGO_script.Slice_two_sides(vertical_bar_sideC, new Vector3(270, 0, -135), new Vector3(270, 0, 135));
                vertical_bar_sideC = Frame_Parent.transform.Find("FrameC").gameObject; //because original will be destroyed after slicing


                vertical_bar_sideB = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideB.transform.localPosition = new Vector3(-scale_c, 0, (scale_d - scale_b)); //to offset
                vertical_bar_sideB.transform.localRotation = Quaternion.Euler(0, -270, 90);
                vertical_bar_sideB.transform.localScale = new Vector3(1, scale_b + framePartSettings._part_depth, 1); //user feeds outside line, we need to add depth to get inside line
                vertical_bar_sideB.transform.Translate(-Vector3.forward * framePartSettings._part_depth, Space.World);
                vertical_bar_sideB.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideB.name = "FrameB";
                characteristics_script = vertical_bar_sideB.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, -135";
                characteristics_script.right_end_cut_angle = "270, 0, -135";
                characteristics_script.icon_filename = "frame4545.jpg";
                vertical_bar_sideB.transform.Find("Cube_left").Translate(Vector3.forward * framePartSettings._part_depth, Space.World);
                cube_left_pos = vertical_bar_sideB.transform.Find("Cube_left").transform.position;
                cube_right_pos = vertical_bar_sideB.transform.Find("Cube_right").transform.position;
                characteristics_script.cube_left_pos = cube_left_pos.x + "," + cube_left_pos.y + "," + cube_left_pos.z;
                characteristics_script.cube_right_pos = cube_right_pos.x + "," + cube_right_pos.y + "," + cube_right_pos.z;
                sliceGO_script.Slice_two_sides(vertical_bar_sideB, new Vector3(270, 0, -135), new Vector3(270, 0, -135));
                vertical_bar_sideB = Frame_Parent.transform.Find("FrameB").gameObject; //because original will be destroyed after slicing

                vertical_bar_sideA = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideA.transform.localPosition = new Vector3(-scale_e + scale_g, 0, scale_f - scale_b + framePartSettings._part_depth);
                vertical_bar_sideA.transform.localRotation = Quaternion.Euler(0, 180, 90);
                vertical_bar_sideA.transform.localScale = new Vector3(1, scale_a + 2 * framePartSettings._part_depth, 1);
                vertical_bar_sideA.transform.Translate(-Vector3.forward * framePartSettings._part_depth, Space.World);
                vertical_bar_sideA.transform.Translate(-Vector3.right * framePartSettings._part_depth, Space.World);
                vertical_bar_sideA.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideA.name = "FrameA";
                characteristics_script = vertical_bar_sideA.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, 135";
                characteristics_script.right_end_cut_angle = "270, 0, -135";
                characteristics_script.icon_filename = "frameLeft-4545.jpg";
                vertical_bar_sideA.transform.Find("Cube_right").Translate(-Vector3.forward * framePartSettings._part_depth, Space.World);
                vertical_bar_sideA.transform.Find("Cube_left").Translate(-Vector3.forward * framePartSettings._part_depth, Space.World);
                cube_left_pos = vertical_bar_sideA.transform.Find("Cube_left").transform.position;
                cube_right_pos = vertical_bar_sideA.transform.Find("Cube_right").transform.position;
                characteristics_script.cube_left_pos = cube_left_pos.x + "," + cube_left_pos.y + "," + cube_left_pos.z;
                characteristics_script.cube_right_pos = cube_right_pos.x + "," + cube_right_pos.y + "," + cube_right_pos.z;
                sliceGO_script.Slice_two_sides(vertical_bar_sideA, new Vector3(270, 0, 135), new Vector3(270, 0, -135));
                //sliceGO_script.Slice_one_side(vertical_bar_sideA, new Vector3(270, 0, 135), vertical_bar_sideA.transform.Find("Cube_left").position+new Vector3(0,0,-framePartSettings._part_depth));
                vertical_bar_sideA = Frame_Parent.transform.Find("FrameA").gameObject; //because original will be destroyed after slicing

                vertical_bar_sideF = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideF.transform.localPosition = new Vector3(-scale_e + framePartSettings._part_depth, 0, 0);
                vertical_bar_sideF.transform.localRotation = Quaternion.Euler(180, -90, -90);
                vertical_bar_sideF.transform.localScale = new Vector3(1, scale_f, 1);
                vertical_bar_sideF.transform.Translate(-Vector3.right * framePartSettings._part_depth, Space.World);
                vertical_bar_sideF.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideF.name = "FrameF";
                characteristics_script = vertical_bar_sideF.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, 135";
                characteristics_script.right_end_cut_angle = "270, 0, -135";
                characteristics_script.icon_filename = "frame-4545.jpg";
                cube_left_pos = vertical_bar_sideF.transform.Find("Cube_left").transform.position;
                cube_right_pos = vertical_bar_sideF.transform.Find("Cube_right").transform.position;
                characteristics_script.cube_left_pos = cube_left_pos.x + "," + cube_left_pos.y + "," + cube_left_pos.z;
                characteristics_script.cube_right_pos = cube_right_pos.x + "," + cube_right_pos.y + "," + cube_right_pos.z;
                sliceGO_script.Slice_two_sides(vertical_bar_sideF, new Vector3(270, 0, 135), new Vector3(270, 0, -135));
                vertical_bar_sideF = Frame_Parent.transform.Find("FrameF").gameObject; //because original will be destroyed after slicing

                vertical_bar_sideE = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideE.transform.localPosition = new Vector3(0, 0, 0);
                vertical_bar_sideE.transform.localRotation = Quaternion.Euler(0, 0, 90);
                vertical_bar_sideE.transform.localScale = new Vector3(1, scale_e, 1);
                vertical_bar_sideE.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideE.name = "FrameE";
                characteristics_script = vertical_bar_sideE.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, -135";
                characteristics_script.right_end_cut_angle = "270, 0, 135";
                characteristics_script.icon_filename = "frameLeft45-45.jpg";
                cube_left_pos = vertical_bar_sideE.transform.Find("Cube_left").transform.position;
                cube_right_pos = vertical_bar_sideE.transform.Find("Cube_right").transform.position;
                characteristics_script.cube_left_pos = cube_left_pos.x + "," + cube_left_pos.y + "," + cube_left_pos.z;
                characteristics_script.cube_right_pos = cube_right_pos.x + "," + cube_right_pos.y + "," + cube_right_pos.z;
                sliceGO_script.Slice_two_sides(vertical_bar_sideE, new Vector3(270, 0, -135), new Vector3(270, 0, 135));
                vertical_bar_sideE = Frame_Parent.transform.Find("FrameE").gameObject; //because original will be destroyed after slicing

                vertical_bar_sideG = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideG.transform.localPosition = new Vector3(-scale_e, 0, scale_f);
                vertical_bar_sideG.transform.localRotation = Quaternion.Euler(0, 180, 90);
                vertical_bar_sideG.transform.localScale = new Vector3(1, scale_g, 1);
                vertical_bar_sideG.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideG.name = "FrameG";
                characteristics_script = vertical_bar_sideG.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, -135";
                characteristics_script.right_end_cut_angle = "270, 0, 135";
                characteristics_script.icon_filename = "frameLeft45-45.jpg";
                cube_left_pos = vertical_bar_sideG.transform.Find("Cube_left").transform.position;
                cube_right_pos = vertical_bar_sideG.transform.Find("Cube_right").transform.position;
                characteristics_script.cube_left_pos = cube_left_pos.x + "," + cube_left_pos.y + "," + cube_left_pos.z;
                characteristics_script.cube_right_pos = cube_right_pos.x + "," + cube_right_pos.y + "," + cube_right_pos.z;
                sliceGO_script.Slice_two_sides(vertical_bar_sideG, new Vector3(270, 0, -135), new Vector3(270, 0, 135));
                vertical_bar_sideG = Frame_Parent.transform.Find("FrameG").gameObject; //because original will be destroyed after slicing

                vertical_bar_sideH = Instantiate(frameBar_prefab, Frame_Parent.transform);
                vertical_bar_sideH.transform.localPosition = new Vector3(-scale_c - scale_a, 0, (scale_f - scale_h)); //to offset
                vertical_bar_sideH.transform.localRotation = Quaternion.Euler(0, -90, 90);
                vertical_bar_sideH.transform.localScale = new Vector3(1, scale_h + framePartSettings._part_depth, 1); //user feeds outside line, we need to add depth to get inside line
                //vertical_bar_sideH.transform.Translate(-Vector3.forward * framePartSettings._part_depth, Space.World);
                vertical_bar_sideH.transform.Translate(Vector3.forward * scale_h, Space.World);
                vertical_bar_sideH.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                vertical_bar_sideH.name = "FrameH";
                characteristics_script = vertical_bar_sideH.AddComponent<Characteristics>();
                characteristics_script.part_name_id = frame_type;
                characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                characteristics_script.part_type = part_type_enum.frame.ToString();
                characteristics_script.part_group = "Frame";
                characteristics_script.left_end_cut_angle = "270, 0, 135";
                characteristics_script.right_end_cut_angle = "270, 0, 135";
                characteristics_script.icon_filename = "frame4545.jpg";
                cube_left_pos = vertical_bar_sideH.transform.Find("Cube_left").transform.position;
                cube_right_pos = vertical_bar_sideH.transform.Find("Cube_right").transform.position;
                characteristics_script.cube_left_pos = cube_left_pos.x + "," + cube_left_pos.y + "," + cube_left_pos.z;
                characteristics_script.cube_right_pos = cube_right_pos.x + "," + cube_right_pos.y + "," + cube_right_pos.z;
                vertical_bar_sideH.transform.Find("Cube_right").Translate(-Vector3.right * framePartSettings._part_depth, Space.World);
                sliceGO_script.Slice_two_sides(vertical_bar_sideH, new Vector3(270, 0, 135), new Vector3(270, 0, 135));
                vertical_bar_sideH = Frame_Parent.transform.Find("FrameB").gameObject; //because original will be destroyed after slicing

                GameObject Accesories_Parent = new GameObject("Accesories_Parent");
                Accesories_Parent.transform.parent = Frames_Parent.transform;

                int inside_frame_accessories_count = 0;
                if (frame_type == "ak - 288") //80x80
                {
                    List<GameObject> accessories_together_spider_ak288 = new List<GameObject>();

                    GameObject Spider_crown_Left_Front_ = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -90);
                    Spider_crown_Left_Front_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_Left_Front_.name = spider_accessory + "_" + inside_frame_accessories_count++;
                    accessories_together_spider_ak288.Add(Spider_crown_Left_Front_);

                    GameObject Spider_crown_right_front = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_right_front.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right_front.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 180);
                    Spider_crown_right_front.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_front.name = spider_accessory + "_" + inside_frame_accessories_count++;
                    accessories_together_spider_ak288.Add(Spider_crown_right_front);

                    GameObject Spider_crown_right_top_ = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_right_top_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right_top_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 90);
                    Spider_crown_right_top_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_top_.name = spider_accessory + "_" + inside_frame_accessories_count++;
                    accessories_together_spider_ak288.Add(Spider_crown_right_top_);



                    GameObject Spider_crown_middle_rear = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_middle_rear.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_middle_rear.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_middle_rear.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_middle_rear.name = spider_accessory + "_" + inside_frame_accessories_count++;
                    accessories_together_spider_ak288.Add(Spider_crown_middle_rear);

                    GameObject Spider_crown_left_rear = Instantiate(Spider_accessory_prefab, Accesories_Parent.transform);
                    Spider_crown_left_rear.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_left_rear.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_left_rear.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_left_rear.name = spider_accessory + "_" + inside_frame_accessories_count++;
                    accessories_together_spider_ak288.Add(Spider_crown_left_rear);

                    GameObject Spider_crown_middle_bottom = Instantiate(Spider_accessory_v1_prefab, Accesories_Parent.transform);
                    Spider_crown_middle_bottom.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_middle_bottom.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -180);
                    Spider_crown_middle_bottom.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_middle_bottom.name = spider_accessory + "_" + inside_frame_accessories_count++;
                    accessories_together_spider_ak288.Add(Spider_crown_middle_bottom);

                    foreach (GameObject accessory in accessories_together_spider_ak288)
                    {
                        characteristics_script = accessory.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = spider_accessory;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";
                    }

                }
                else if (frame_type == "ak - 31a new" || frame_type == "32306") //Crown 150, 200
                {
                    List<GameObject> accessories_together = new List<GameObject>();
                    List<GameObject> accessories_together_spider = new List<GameObject>();
                    float Bottom_Rubber_Offset = -3.5f;
                    float l_angle_width = 25f;


                    GameObject Right_rear_Bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    float L_Shape_Rubber_height = Right_rear_Bottom.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.size.z;

                    GameObject left_Front_Bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    left_Front_Bottom.transform.localPosition = new Vector3(0, L_Shape_Rubber_height, 0); //to offset
                    left_Front_Bottom.transform.localRotation = Quaternion.Euler(90, 0, 90);
                    left_Front_Bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    left_Front_Bottom.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++;
                    left_Front_Bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(left_Front_Bottom);

                    GameObject left_Front_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    left_Front_top.transform.localPosition = new Vector3(0, framePartSettings._part_width, 0); //to offset
                    left_Front_top.transform.localRotation = Quaternion.Euler(90, 0, 90);
                    left_Front_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    left_Front_top.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++;
                    accessories_together.Add(left_Front_top);


                    GameObject right_Front_bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    right_Front_bottom.transform.localPosition = new Vector3(0, L_Shape_Rubber_height, scale_e); //to offset
                    right_Front_bottom.transform.localRotation = Quaternion.Euler(90, 0, -180);
                    right_Front_bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    right_Front_bottom.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++;
                    right_Front_bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(right_Front_bottom);

                    GameObject Right_Front_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Right_Front_top.transform.localPosition = new Vector3(0, framePartSettings._part_width, scale_e); //to offset
                    Right_Front_top.transform.localRotation = Quaternion.Euler(90, 0, -180);
                    Right_Front_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Right_Front_top.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++;
                    accessories_together.Add(Right_Front_top);

                    GameObject right_rear_bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    right_rear_bottom.transform.localPosition = new Vector3(-scale_f, L_Shape_Rubber_height, scale_e); //to offset
                    right_rear_bottom.transform.localRotation = Quaternion.Euler(90, 0, -90);
                    right_rear_bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    right_rear_bottom.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++;
                    right_rear_bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(right_rear_bottom);

                    GameObject Right_rear_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Right_rear_top.transform.localPosition = new Vector3(-scale_f, framePartSettings._part_width, scale_e); //to offset
                    Right_rear_top.transform.localRotation = Quaternion.Euler(90, 0, -90);
                    Right_rear_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Right_rear_top.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++;
                    accessories_together.Add(Right_rear_top);

                    GameObject middle_front_bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    middle_front_bottom.transform.localPosition = new Vector3(-scale_f + l_angle_width, L_Shape_Rubber_height, scale_a - l_angle_width); //to offset
                    middle_front_bottom.transform.localRotation = Quaternion.Euler(90, 0, 90);
                    middle_front_bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    middle_front_bottom.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++;
                    middle_front_bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(middle_front_bottom);

                    GameObject Middle_front_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Middle_front_top.transform.localPosition = new Vector3(-scale_f + l_angle_width, framePartSettings._part_width, scale_a - l_angle_width); //to offset
                    Middle_front_top.transform.localRotation = Quaternion.Euler(90, 0, 90);
                    Middle_front_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Middle_front_top.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++;
                    accessories_together.Add(Middle_front_top);


                    GameObject middle_rear_bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    middle_rear_bottom.transform.localPosition = new Vector3(-scale_e, L_Shape_Rubber_height, scale_a); //to offset
                    middle_rear_bottom.transform.localRotation = Quaternion.Euler(90, 0, -90);
                    middle_rear_bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    middle_rear_bottom.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++;
                    middle_rear_bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(middle_rear_bottom);

                    GameObject Middle_rear_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    Middle_rear_top.transform.localPosition = new Vector3(-scale_e, framePartSettings._part_width, scale_a); //to offset
                    Middle_rear_top.transform.localRotation = Quaternion.Euler(90, 0, -90);
                    Middle_rear_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    Middle_rear_top.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++;
                    accessories_together.Add(Middle_rear_top);

                    GameObject left_rear_bottom = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    left_rear_bottom.transform.localPosition = new Vector3(-scale_e, L_Shape_Rubber_height, 0); //to offset
                    left_rear_bottom.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    left_rear_bottom.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    left_rear_bottom.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++;
                    left_rear_bottom.transform.Translate(Bottom_Rubber_Offset * Vector3.forward);
                    accessories_together.Add(left_rear_bottom);

                    GameObject left_rear_top = Instantiate(Rubber_Angle_prefab, Accesories_Parent.transform);
                    left_rear_top.transform.localPosition = new Vector3(-scale_e, framePartSettings._part_width, 0); //to offset
                    left_rear_top.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    left_rear_top.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    left_rear_top.name = L_Rubber_accessory + "_" + inside_frame_accessories_count++;
                    accessories_together.Add(left_rear_top);


                    foreach (GameObject accessory in accessories_together)
                    {
                        characteristics_script = accessory.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = L_Rubber_accessory;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";
                    }


                    GameObject Spider_crown_Left_Front_ = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -90);
                    Spider_crown_Left_Front_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_Left_Front_.name = spidertocrown + "_" + inside_frame_accessories_count++;
                    accessories_together_spider.Add(Spider_crown_Left_Front_);



                    GameObject Spider_crown_right_front = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_right_front.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right_front.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 180);
                    Spider_crown_right_front.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_front.name = spidertocrown + "_" + inside_frame_accessories_count++;
                    accessories_together_spider.Add(Spider_crown_right_front);

                    GameObject Spider_crown_right_top_ = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_right_top_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right_top_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 90);
                    Spider_crown_right_top_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_top_.name = spidertocrown + "_" + inside_frame_accessories_count++;
                    accessories_together_spider.Add(Spider_crown_right_top_);



                    GameObject Spider_crown_middle_rear = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_middle_rear.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_middle_rear.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_middle_rear.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_middle_rear.name = spidertocrown + "_" + inside_frame_accessories_count++;
                    accessories_together_spider.Add(Spider_crown_middle_rear);

                    GameObject Spider_crown_left_rear = Instantiate(Spider_to_the_crown_prefab, Accesories_Parent.transform);
                    Spider_crown_left_rear.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_left_rear.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_left_rear.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_left_rear.name = spidertocrown + "_" + inside_frame_accessories_count++;
                    accessories_together_spider.Add(Spider_crown_left_rear);


                    GameObject Spider_crown_middle_bottom = Instantiate(Spider_to_the_crown_v1_prefab, Accesories_Parent.transform);
                    Spider_crown_middle_bottom.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_middle_bottom.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -180);
                    //Spider_crown_middle_bottom.transform.GetChild(0).rotation =  Quaternion.Euler(0, 0, -180);
                    //Spider_crown_middle_bottom.transform.GetChild(2).GetChild(3).rotation = Quaternion.Euler(180, 0, 90);
                    //Spider_crown_middle_bottom.transform.GetChild(4).GetChild(5).rotation = Quaternion.Euler(0, 0, 180);
                    Spider_crown_middle_bottom.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_middle_bottom.name = spidertocrown + "_" + inside_frame_accessories_count++;
                    accessories_together_spider.Add(Spider_crown_middle_bottom);

                    foreach (GameObject accessory in accessories_together_spider)
                    {
                        characteristics_script = accessory.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = spidertocrown;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";
                    }
                }
                else if (frame_type == "ak - 279") //80x40
                {
                    List<GameObject> accessories_together_ak279 = new List<GameObject>();

                    GameObject Spider_crown_Left_Front_ = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_Left_Front_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -90);
                    Spider_crown_Left_Front_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_Left_Front_.name = spider + "_" + inside_frame_accessories_count++;
                    accessories_together_ak279.Add(Spider_crown_Left_Front_);

                    GameObject Spider_crown_right_front = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_crown_right_front.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideE.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right_front.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 180);
                    Spider_crown_right_front.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_front.name = spider + "_" + inside_frame_accessories_count++;
                    accessories_together_ak279.Add(Spider_crown_right_front);

                    GameObject Spider_crown_right_top_ = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_crown_right_top_.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideD.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_right_top_.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 90);
                    Spider_crown_right_top_.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_right_top_.name = spider + "_" + inside_frame_accessories_count++;
                    accessories_together_ak279.Add(Spider_crown_right_top_);



                    GameObject Spider_crown_middle_rear = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_crown_middle_rear.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideC.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_middle_rear.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_middle_rear.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_middle_rear.name = spider + "_" + inside_frame_accessories_count++;
                    accessories_together_ak279.Add(Spider_crown_middle_rear);

                    GameObject Spider_crown_left_rear = Instantiate(spider_prefab, Accesories_Parent.transform);
                    Spider_crown_left_rear.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideF.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_left_rear.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, 0);
                    Spider_crown_left_rear.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_left_rear.name = spider + "_" + inside_frame_accessories_count++;
                    accessories_together_ak279.Add(Spider_crown_left_rear);

                    GameObject Spider_crown_middle_bottom = Instantiate(spider_v1_prefab, Accesories_Parent.transform);
                    Spider_crown_middle_bottom.transform.Find("Locator_top").position = new Vector3(vertical_bar_sideB.transform.Find("Locator_TopInside_for_Spider").position.x,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.y,
                                                                                         vertical_bar_sideA.transform.Find("Locator_TopInside_for_Spider").position.z);
                    Spider_crown_middle_bottom.transform.Find("Locator_top").localRotation = Quaternion.Euler(-90, 0, -180);
                    Spider_crown_middle_bottom.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    Spider_crown_middle_bottom.name = spider + "_" + inside_frame_accessories_count++;
                    accessories_together_ak279.Add(Spider_crown_middle_bottom);

                    foreach (GameObject accessory in accessories_together_ak279)
                    {
                        characteristics_script = accessory.AddComponent<Characteristics>();
                        characteristics_script.part_name_id = spider;
                        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                        characteristics_script.part_type = part_type_enum.accessory.ToString();
                        characteristics_script.part_group = "Frame";
                    }
                }

                foreach (Transform accs in Accesories_Parent.transform)
                {
                    if (!Accessories_name.Contains(accs.name))
                        Accessories_name.Add(accs.name);
                }
                #endregion

                RaycastHit hit_BtoD = new RaycastHit();
                RaycastHit hit_AtoE = new RaycastHit();
                RaycastHit hit_HtoF = new RaycastHit();
                RaycastHit hit_GtoDivSec3 = new RaycastHit();
                RaycastHit hit_CtoDivSec3 = new RaycastHit();
                RaycastHit hit_FtoDivSec3 = new RaycastHit();
                RaycastHit hit_DtoDivSec3 = new RaycastHit();
                RaycastHit hit_divtoDivSec3 = new RaycastHit();

                int total_number_of_fields_DividertoE = 0;
                int total_number_of_fields_DividertoC = 0;
                int total_number_of_fields_DividertoF = 0;

                float each_inner_field_width_fieldDividertoF = 0;
                float each_inner_field_width_fieldDividertoC = 0;
                float each_inner_field_width_fieldDividertoE = 0;

                if (dsElement_pergola_header.Tables[0].Rows[0]["rafafa_placement_type"].ToString() == "type_2")
                {
                    #region Divider

                    Divider_Parent = new GameObject("Divider_Parent");
                    Divider_Parent.transform.parent = Pergola_Model.transform;

                    FrameDividers_Parent = new GameObject("FrameDividers_Parent");
                    FrameDividers_Parent.transform.parent = Divider_Parent.transform;

                    FrameDividers_Parent_Section1 = new GameObject("FrameDividers_Parent_Section1");
                    FrameDividers_Parent_Section1.transform.parent = FrameDividers_Parent.transform;

                    FrameDividers_Parent_Section2 = new GameObject("FrameDividers_Parent_Section2");
                    FrameDividers_Parent_Section2.transform.parent = FrameDividers_Parent.transform;

                    FrameDividers_Parent_Section3 = new GameObject("FrameDividers_Parent_Section3");
                    FrameDividers_Parent_Section3.transform.parent = FrameDividers_Parent.transform;

                    FrameDividers_Parent_Section4 = new GameObject("FrameDividers_Parent_Section4");
                    FrameDividers_Parent_Section4.transform.parent = FrameDividers_Parent.transform;


                    #region hit_BtoD Raycast
                    GameObject frame_B = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameB")) //TODO: here check if frame is sliced, then get the first or last sliced part accordingly (FrameB_0 or FrameB_n)
                            frame_B = frame.gameObject;
                    }
                    Bounds frame_B_bound = Calculate_b(frame_B.transform);
                    Vector3 global_center_Frame_B = Pergola_Model.transform.TransformPoint(frame_B_bound.center);
                    Vector3 ray_cast_dir = Pergola_Model.transform.TransformDirection(Pergola_Model.transform.right);
                    Vector3 top_point_frame_B = global_center_Frame_B + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, frame_B_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_B, ray_cast_dir, out hit_BtoD, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_B, ray_cast_dir * hit_BtoD.distance, Color.yellow);
                        Debug.Log($"Did Hit & Distance={hit_BtoD.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_B, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("Did not Hit");
                    }
                    #endregion

                    #region hit_AtoE Raycast
                    GameObject frame_A = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameA"))
                            frame_A = frame.gameObject;
                    }
                    Bounds frame_A_bound = Calculate_b(frame_A.transform);
                    Vector3 global_center_Frame_A = Pergola_Model.transform.TransformPoint(frame_A_bound.center);
                    ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.forward);
                    Vector3 top_point_frame_A = global_center_Frame_A + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, frame_A_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_A, ray_cast_dir, out hit_AtoE, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_A, ray_cast_dir * hit_AtoE.distance, Color.yellow);
                        Debug.Log($"Did Hit & Distance={hit_BtoD.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_A, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("Did not Hit");
                    }
                    #endregion

                    #region hit_HtoF Raycast
                    GameObject frame_H = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameH"))
                            frame_H = frame.gameObject;
                    }
                    Bounds frame_H_bound = Calculate_b(frame_H.transform);
                    Vector3 global_center_Frame_H = Pergola_Model.transform.TransformPoint(frame_H_bound.center);
                    ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.right); //negetive right= left
                    Vector3 top_point_frame_H = global_center_Frame_H + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, frame_H_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_H, ray_cast_dir, out hit_HtoF, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_H, ray_cast_dir * hit_HtoF.distance, Color.yellow);
                        Debug.Log($"Did Hit H TO F & Distance={hit_HtoF.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_H, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("Did not Hit h to f");
                    }

                    //Frame Divider Code
                    float dividerPoleDistance = frameDividers.each_field_width + framePartSettings._part_depth;
                    int clamp_naming_counter = 0;

                    Debug.Log("number of dividers" + frameDividers.numberOfDividerPoles);

                    #endregion

                    for (int i = 0; i < 2; i++)
                    {

                        // GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + i);
                        // frameDivider_GO_parent.transform.parent = FrameDividers_Parent.transform;

                        GameObject frameDivider_GO = Instantiate(DividerBar_prefab, FrameDividers_Parent_Section1.transform);
                        frameDivider_GO.transform.localPosition = vertical_bar_sideB.transform.localPosition; //Todo add(offset) frame B or D width to each field width
                        if (i == 0)
                        {
                            frameDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            frameDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                            frameDivider_GO.transform.Translate(Vector3.right * (hit_BtoD.distance + framePartSettings._part_depth), Space.World);
                        }
                        else if (i == 1)
                        {
                            frameDivider_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                            frameDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                            frameDivider_GO.transform.Translate(Vector3.right * framePartSettings._part_depth, Space.World);
                        }

                        frameDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        frameDivider_GO.name = "FrameDivider_" + i;
                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, FrameDividers_Parent_Section1.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = frameDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = frameDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, FrameDividers_Parent_Section2.transform);
                            C_Clamp_ForDivider_2.transform.Translate(frameDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }




                        if (i == 0)
                        {
                            frameDivider_GO.transform.parent = FrameDividers_Parent_Section1.transform;
                        }
                        else if (i == 1)
                        {
                            frameDivider_GO.transform.parent = FrameDividers_Parent_Section2.transform;
                        }

                    }

                    for (int i = 0; i < 2; i++)
                    {

                        // GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + i);
                        // frameDivider_GO_parent.transform.parent = FrameDividers_Parent.transform;

                        GameObject frameDivider_GO = Instantiate(DividerBar_prefab, FrameDividers_Parent_Section1.transform);
                        frameDivider_GO.transform.localPosition = vertical_bar_sideA.transform.localPosition; //Todo add(offset) frame B or D width to each field width
                        if (i == 0)
                        {
                            frameDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            frameDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                            frameDivider_GO.transform.Translate(-Vector3.forward * framePartSettings._part_depth, Space.World);
                        }
                        else if (i == 1)
                        {
                            frameDivider_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                            frameDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                            frameDivider_GO.transform.Translate(-Vector3.forward * framePartSettings._part_depth, Space.World);
                            frameDivider_GO.transform.Translate(Vector3.right * (framePartSettings._part_depth / 2), Space.World);
                        }


                        frameDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        frameDivider_GO.name = "FrameDivider_" + i;
                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, FrameDividers_Parent_Section3.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = frameDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = frameDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, FrameDividers_Parent_Section4.transform);
                            C_Clamp_ForDivider_2.transform.Translate(frameDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }




                        if (i == 0)
                        {
                            frameDivider_GO.transform.parent = FrameDividers_Parent_Section3.transform;
                        }
                        else if (i == 1)
                        {
                            frameDivider_GO.transform.parent = FrameDividers_Parent_Section4.transform;
                        }

                    }


                    #region hit_GtoDivSec3 Raycast
                    GameObject frame_G = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameG")) //TODO: here check if frame is sliced, then get the first or last sliced part accordingly (FrameB_0 or FrameB_n)
                            frame_G = frame.gameObject;
                    }
                    Bounds frame_G_bound = Calculate_b(frame_G.transform);
                    Vector3 global_center_Frame_G = Pergola_Model.transform.TransformPoint(frame_G_bound.center);
                    Vector3 ray_cast_dir_G = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.forward);
                    Vector3 top_point_frame_G = global_center_Frame_G + ray_cast_dir_G * Mathf.Abs(Vector3.Dot(ray_cast_dir_G, frame_G_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_G - new Vector3(0, 40, 0), ray_cast_dir_G, out hit_GtoDivSec3, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_G - new Vector3(0, 40, 0), ray_cast_dir_G * hit_GtoDivSec3.distance, Color.black, Mathf.Infinity);
                        Debug.Log($"Did Hit & Distance from G to Divider ={hit_GtoDivSec3.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_G, ray_cast_dir_G * 10000, Color.white, Mathf.Infinity);
                        Debug.Log("Did not Hit");
                    }
                    #endregion

                    //G TO DIV3
                    FrameDividers frameDividers_GtoDivSec3 = new FrameDividers(hit_GtoDivSec3.distance, framePartSettings, verticalPartSettings,MAX_SUPPORTBAR_DIVIDER_DISTANCE);
                    float dividerPoleDistanceGtoDivSec3 = frameDividers.each_field_width + framePartSettings._part_depth;
                    int clamp_naming_counterGtoDivSec3 = 0;

                    for (int i = 0; i < frameDividers.numberOfDividerPoles; i++)
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + i);
                        frameDivider_GO_parent.transform.parent = FrameDividers_Parent.transform;

                        GameObject frameDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        frameDivider_GO.transform.localPosition = new Vector3((frame_G.transform.position.x + framePartSettings._part_depth), 0, (frame_G.transform.position.z - dividerPoleDistanceGtoDivSec3));
                        frameDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        frameDivider_GO.transform.localScale = new Vector3(1, -hit_HtoF.distance, 1);
                        frameDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        frameDivider_GO.name = "FrameDivider_" + i;
                        dividerPoleDistanceGtoDivSec3 += (frameDividers.each_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = frameDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counterGtoDivSec3++;


                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2;

                            C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").Translate(hit_HtoF.distance * Vector3.right);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counterGtoDivSec3++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }

                    #region hit_CtoDivSec3 Raycast
                    GameObject frame_C = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameC")) //TODO: here check if frame is sliced, then get the first or last sliced part accordingly (FrameB_0 or FrameB_n)
                            frame_C = frame.gameObject;
                    }
                    Bounds frame_C_bound = Calculate_b(frame_C.transform);
                    Vector3 global_center_Frame_C = Pergola_Model.transform.TransformPoint(frame_C_bound.center);
                    Vector3 ray_cast_dir_C = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.forward);
                    Vector3 top_point_frame_C = global_center_Frame_C + ray_cast_dir_C * Mathf.Abs(Vector3.Dot(ray_cast_dir_C, frame_C_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_C - new Vector3(0, 40, 0), ray_cast_dir_C, out hit_CtoDivSec3, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_C - new Vector3(0, 40, 0), ray_cast_dir_C * hit_CtoDivSec3.distance, Color.black, Mathf.Infinity);
                        Debug.Log($"Did Hit & Distance from G to Divider ={hit_CtoDivSec3.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_C, ray_cast_dir_C * 10000, Color.white, Mathf.Infinity);
                        Debug.Log("Did not Hit");
                    }
                    #endregion
                    //CtoDivSec3
                    FrameDividers frameDividers_CtoDivSec3 = new FrameDividers(hit_CtoDivSec3.distance, framePartSettings, verticalPartSettings,MAX_SUPPORTBAR_DIVIDER_DISTANCE);
                    float dividerPoleDistanceCtoDivSec3 = frameDividers.each_field_width + framePartSettings._part_depth;
                    int clamp_naming_counterCtoDivSec3 = 0;

                    for (int i = 0; i < frameDividers.numberOfDividerPoles; i++)
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + i);
                        frameDivider_GO_parent.transform.parent = FrameDividers_Parent.transform;

                        GameObject frameDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        frameDivider_GO.transform.localPosition = new Vector3((frame_C.transform.position.x + framePartSettings._part_depth), 0, (frame_C.transform.position.z - dividerPoleDistanceCtoDivSec3));
                        frameDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        frameDivider_GO.transform.localScale = new Vector3(1, -hit_BtoD.distance, 1);
                        frameDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        frameDivider_GO.name = "FrameDivider_" + i;
                        dividerPoleDistanceCtoDivSec3 += (frameDividers.each_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = frameDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counterCtoDivSec3++;


                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2;

                            C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").Translate(hit_BtoD.distance * Vector3.right);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counterCtoDivSec3++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }

                    #region hit_FtoDivSec3 Raycast
                    GameObject frame_F = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameF")) //TODO: here check if frame is sliced, then get the first or last sliced part accordingly (FrameB_0 or FrameB_n)
                            frame_F = frame.gameObject;
                    }
                    Bounds frame_F_bound = Calculate_b(frame_F.transform);
                    Vector3 global_center_Frame_F = Pergola_Model.transform.TransformPoint(frame_F_bound.center);
                    Vector3 ray_cast_dir_F = Pergola_Model.transform.TransformDirection(Pergola_Model.transform.right);
                    Vector3 top_point_frame_F = global_center_Frame_F + ray_cast_dir_F * Mathf.Abs(Vector3.Dot(ray_cast_dir_F, frame_F_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_F - new Vector3(0, 40, 0), ray_cast_dir_F, out hit_FtoDivSec3, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_F - new Vector3(0, 40, 0), ray_cast_dir_F * hit_FtoDivSec3.distance, Color.black, Mathf.Infinity);
                        Debug.Log($"Did Hit & Distance from F to Divider ={hit_FtoDivSec3.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_F, ray_cast_dir_F * 10000, Color.white, Mathf.Infinity);
                        Debug.Log("Did not Hit ftoDivSec3");
                    }
                    #endregion


                    //f
                    FrameDividers frameDividers_FtoDivSec3 = new FrameDividers(hit_FtoDivSec3.distance, framePartSettings, verticalPartSettings,MAX_SUPPORTBAR_DIVIDER_DISTANCE);
                    float dividerPoleDistanceFtoDivSec3 = frameDividers.each_field_width + framePartSettings._part_depth;
                    int clamp_naming_counterdivtoDivSec3 = 0;

                    for (int i = 0; i < frameDividers.numberOfDividerPoles; i++)
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + i);
                        frameDivider_GO_parent.transform.parent = FrameDividers_Parent.transform;

                        GameObject frameDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        frameDivider_GO.transform.localPosition = new Vector3((frame_F.transform.position.x) + dividerPoleDistanceFtoDivSec3, 0, framePartSettings._part_depth);
                        frameDivider_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                        frameDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                        frameDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        frameDivider_GO.name = "FrameDivider_" + i;
                        dividerPoleDistanceFtoDivSec3 += (frameDividers.each_field_width + framePartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = frameDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counterdivtoDivSec3++;


                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2;

                            C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").Translate(hit_AtoE.distance * Vector3.right);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counterdivtoDivSec3++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }



                    #region hit_DtoDivSec3 Raycast
                    GameObject frame_D = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameD")) //TODO: here check if frame is sliced, then get the first or last sliced part accordingly (FrameB_0 or FrameB_n)
                            frame_D = frame.gameObject;
                    }
                    Bounds frame_D_bound = Calculate_b(frame_D.transform);
                    Vector3 global_center_Frame_D = Pergola_Model.transform.TransformPoint(frame_D_bound.center);
                    Vector3 ray_cast_dir_D = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.right);
                    Vector3 top_point_frame_D = global_center_Frame_D + ray_cast_dir_D * Mathf.Abs(Vector3.Dot(ray_cast_dir_D, frame_D_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_D - new Vector3(0, 40, 0), ray_cast_dir_D, out hit_DtoDivSec3, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_D - new Vector3(0, 40, 0), ray_cast_dir_D * hit_DtoDivSec3.distance, Color.black, Mathf.Infinity);
                        Debug.Log($"Did Hit & Distance from D to Divider ={hit_DtoDivSec3.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_D, ray_cast_dir_D * 10000, Color.white, Mathf.Infinity);
                        Debug.Log("Did not Hit DtoDivSec3");
                    }
                    #endregion
                    FrameDividers frameDividers_DtoDivSec3 = new FrameDividers(hit_DtoDivSec3.distance, framePartSettings, verticalPartSettings,MAX_SUPPORTBAR_DIVIDER_DISTANCE);
                    float dividerPoleDistanceDoDivSec3 = frameDividers.each_field_width + framePartSettings._part_depth;
                    int clamp_naming_counterDtoDivSec3 = 0;

                    for (int i = 0; i < frameDividers.numberOfDividerPoles; i++)
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + i);
                        frameDivider_GO_parent.transform.parent = FrameDividers_Parent.transform;

                        GameObject frameDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        frameDivider_GO.transform.localPosition = new Vector3((frame_B.transform.position.x) + dividerPoleDistanceDoDivSec3, 0, framePartSettings._part_depth);
                        frameDivider_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                        frameDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                        frameDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        frameDivider_GO.name = "FrameDivider_" + i;
                        dividerPoleDistanceDoDivSec3 += (frameDividers.each_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = frameDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counterDtoDivSec3++;


                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2;

                            C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").Translate(hit_AtoE.distance * Vector3.right);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counterDtoDivSec3++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }



                    #region hit_divtoDivSec3 Raycast
                    var DivRaycast = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section2").Find("FrameDivider_1").gameObject;

                    Bounds frame_div_bound = Calculate_b(DivRaycast.transform);
                    Vector3 global_center_Frame_div = Pergola_Model.transform.TransformPoint(frame_div_bound.center);
                    Vector3 ray_cast_dir_div = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.right);
                    Vector3 top_point_frame_div = global_center_Frame_div + ray_cast_dir_div * Mathf.Abs(Vector3.Dot(ray_cast_dir_div, frame_div_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_div - new Vector3(0, 40, 0), ray_cast_dir_div, out hit_divtoDivSec3, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_div - new Vector3(0, 40, 0), ray_cast_dir_div * hit_divtoDivSec3.distance, Color.red, Mathf.Infinity);
                        Debug.Log($"Did Hit & Distance from divider to Divider ={hit_divtoDivSec3.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_div, ray_cast_dir_div * 10000, Color.white, Mathf.Infinity);
                        Debug.Log("Did not Hit divider to divider ");
                    }
                    #endregion

                    FrameDividers frameDividers_DIVtoDivSec3 = new FrameDividers(hit_divtoDivSec3.distance, framePartSettings, verticalPartSettings,MAX_SUPPORTBAR_DIVIDER_DISTANCE);
                    float dividerPoleDistanceDivtoDivSec3 = frameDividers.each_field_width + framePartSettings._part_depth;
                    int clamp_naming_counterFtoDivSec3 = 0;

                    for (int i = 0; i < frameDividers.numberOfDividerPoles; i++)
                    {
                        GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + i);
                        frameDivider_GO_parent.transform.parent = FrameDividers_Parent.transform;

                        GameObject frameDivider_GO = Instantiate(DividerBar_prefab, frameDivider_GO_parent.transform);
                        frameDivider_GO.transform.localPosition = new Vector3((frame_A.transform.position.x) + dividerPoleDistanceDivtoDivSec3, 0, framePartSettings._part_depth);
                        frameDivider_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                        frameDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                        frameDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        frameDivider_GO.name = "FrameDivider_" + i;
                        dividerPoleDistanceDivtoDivSec3 += (frameDividers.each_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = frameDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counterFtoDivSec3++;


                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2;

                            C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, frameDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").Translate(hit_AtoE.distance * Vector3.right);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counterFtoDivSec3++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }

                    // characteristics
                    foreach (Transform t in FrameDividers_Parent.transform)
                    {
                        foreach (Transform child_t in t)
                        {
                            characteristics_script = child_t.gameObject.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = clampfordivider;
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_group = "Frame";
                            if (child_t.name.Contains("FrameDivider"))
                            {
                                characteristics_script.part_name_id = divider_type;
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                            }
                        }
                    }


                    //Field Divider Code
                    //FieldDividers_Parent = new GameObject("FieldDividers_Parent");
                    //FieldDividers_Parent.transform.parent = Divider_Parent.transform;

                    //FieldDividers_Parent_Section1 = new GameObject("FieldDividers_Parent_Section1");
                    //FieldDividers_Parent_Section1.transform.parent = FieldDividers_Parent.transform;

                    //FieldDividers_Parent_Section2 = new GameObject("FieldDividers_Parent_Section2");
                    //FieldDividers_Parent_Section2.transform.parent = FieldDividers_Parent.transform;
                    //int divider_naming_counter = 0;


                    //RaycastHit hit_DividertoE;
                    //GameObject FrameDivider_0 = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section1").Find("FrameDivider_0").gameObject;
                    //Bounds FrameDivider_0_bound = Calculate_b(FrameDivider_0.transform);
                    //Vector3 global_center_FrameDivider_0 = Pergola_Model.transform.TransformPoint(FrameDivider_0_bound.center);
                    //ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.forward);
                    //Vector3 top_point_FrameDivider_0 = global_center_FrameDivider_0 + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, FrameDivider_0_bound.size)) / 2;
                    //if (Physics.Raycast(top_point_FrameDivider_0, ray_cast_dir, out hit_DividertoE, Mathf.Infinity))
                    //{
                    //    Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * hit_DividertoE.distance, Color.yellow);
                    //    Debug.Log($"hit_DividertoE  Did Hit & Distance={hit_DividertoE.distance}");
                    //}
                    //else
                    //{
                    //    Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * 10000, Color.white, Mathf.Infinity);
                    //    Debug.Log("hit_DividertoE  Did not Hit");
                    //}
                    //InnerFieldDividers innerFieldDividers = new InnerFieldDividers(hit_DividertoE.distance, verticalPartSettings);
                    //float inner_dividerPoleDistance = framePartSettings._part_depth + innerFieldDividers.each_inner_field_width;
                    //for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                    //{
                    //    GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + divider_naming_counter);
                    //    fieldDivider_GO_parent.transform.parent = FieldDividers_Parent_Section1.transform;

                    //    GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);
                    //    fieldDivider_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, inner_dividerPoleDistance); //Todo add(offset) frame B or D width to each field width
                    //    fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    //    fieldDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                    //    fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    //    fieldDivider_GO.name = "FieldDivider_" + divider_naming_counter++;
                    //    inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                    //    if (divider_type == "ak - 279")
                    //    {
                    //        GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                    //        C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                    //        C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                    //        C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    //        C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++;

                    //        if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                    //            Accessories_name.Add(C_Clamp_ForDivider.name);

                    //        GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                    //        C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                    //        C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                    //        C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    //        C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++;

                    //        if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                    //            Accessories_name.Add((C_Clamp_ForDivider_2.name));
                    //    }
                    //}
                    //total_number_of_fields_DividertoE = innerFieldDividers.numbefOfInnerFields;
                    //each_inner_field_width_fieldDividertoE = innerFieldDividers.each_inner_field_width;


                    //RaycastHit hit_DividertoC;
                    //ray_cast_dir = Pergola_Model.transform.TransformDirection(Pergola_Model.transform.forward);
                    //top_point_FrameDivider_0 = global_center_FrameDivider_0 + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, FrameDivider_0_bound.size)) / 2;
                    //if (Physics.Raycast(top_point_FrameDivider_0, ray_cast_dir, out hit_DividertoC, Mathf.Infinity))
                    //{
                    //    Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * hit_DividertoC.distance, Color.yellow);
                    //    Debug.Log($"hit_DividertoC  Did Hit & Distance={hit_DividertoC.distance}");
                    //}
                    //else
                    //{
                    //    Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * 10000, Color.white, 10);
                    //    Debug.Log("hit_DividertoC  Did not Hit");
                    //}
                    //innerFieldDividers = new InnerFieldDividers(hit_DividertoC.distance, verticalPartSettings);
                    //inner_dividerPoleDistance = vertical_bar_sideB.transform.localPosition.z + innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth;
                    //for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                    //{
                    //    GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + divider_naming_counter);
                    //    fieldDivider_GO_parent.transform.parent = FieldDividers_Parent_Section1.transform;

                    //    GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);
                    //    fieldDivider_GO.transform.localPosition = new Vector3(-framePartSettings._part_depth, 0, inner_dividerPoleDistance); //Todo add(offset) frame B or D width to each field width
                    //    fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    //    fieldDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                    //    fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    //    fieldDivider_GO.name = "FieldDivider_" + divider_naming_counter++;
                    //    inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                    //    if (divider_type == "ak - 279")
                    //    {
                    //        GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                    //        C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                    //        C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                    //        C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    //        C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++;

                    //        if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                    //            Accessories_name.Add(C_Clamp_ForDivider.name);

                    //        GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                    //        C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                    //        C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                    //        C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    //        C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++;

                    //        if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                    //            Accessories_name.Add((C_Clamp_ForDivider_2.name));
                    //    }
                    //}
                    //total_number_of_fields_DividertoC = innerFieldDividers.numbefOfInnerFields;
                    //each_inner_field_width_fieldDividertoC = innerFieldDividers.each_inner_field_width;


                    //RaycastHit hit_DividertoF;
                    //GameObject FrameDivider_1 = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section2").Find("FrameDivider_1").gameObject;
                    //Bounds FrameDivider_1_bound = Calculate_b(FrameDivider_1.transform);
                    //Vector3 global_center_FrameDivider_1 = Pergola_Model.transform.TransformPoint(FrameDivider_1_bound.center);
                    //ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.right);
                    //Vector3 top_point_FrameDivider_1 = global_center_FrameDivider_1 + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, FrameDivider_1_bound.size)) / 2;
                    //if (Physics.Raycast(top_point_FrameDivider_1, ray_cast_dir, out hit_DividertoF, Mathf.Infinity))
                    //{
                    //    Debug.DrawRay(top_point_FrameDivider_1, ray_cast_dir * hit_DividertoF.distance, Color.yellow);
                    //    Debug.Log($"hit_DividertoF  Did Hit & Distance={hit_DividertoF.distance}");
                    //}
                    //else
                    //{
                    //    Debug.DrawRay(top_point_FrameDivider_1, ray_cast_dir * 10000, Color.white, 10);
                    //    Debug.Log("hit_DividertoF  Did not Hit");
                    //}
                    //innerFieldDividers = new InnerFieldDividers(hit_DividertoF.distance, verticalPartSettings);




                    //inner_dividerPoleDistance = -(FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section2").Find("FrameDivider_1").transform.localPosition.x - innerFieldDividers.each_inner_field_width - verticalPartSettings._part_depth);
                    //for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                    //{
                    //    GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + divider_naming_counter);
                    //    fieldDivider_GO_parent.transform.parent = FieldDividers_Parent_Section2.transform;

                    //    GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);
                    //    fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                    //    fieldDivider_GO.transform.localPosition = new Vector3(-inner_dividerPoleDistance, 0, framePartSettings._part_depth);
                    //    fieldDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                    //    fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    //    fieldDivider_GO.transform.Translate(-Vector3.right * verticalPartSettings._part_depth, Space.World);
                    //    fieldDivider_GO.name = "FieldDivider_" + divider_naming_counter++;
                    //    inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                    //    if (divider_type == "ak - 279")
                    //    {
                    //        GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                    //        C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                    //        C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                    //        C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    //        C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++;

                    //        if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                    //            Accessories_name.Add(C_Clamp_ForDivider.name);

                    //        GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                    //        C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                    //        C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                    //        C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                    //        C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++;

                    //        if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                    //            Accessories_name.Add((C_Clamp_ForDivider_2.name));
                    //    }
                    //}
                    //total_number_of_fields_DividertoF = innerFieldDividers.numbefOfInnerFields;
                    //each_inner_field_width_fieldDividertoF = innerFieldDividers.each_inner_field_width;

                    //// characteristics
                    //foreach (Transform t in FieldDividers_Parent.transform)
                    //{
                    //    foreach (Transform child_t in t)
                    //    {
                    //        characteristics_script = child_t.gameObject.AddComponent<Characteristics>();
                    //        characteristics_script.part_name_id = clampfordivider;
                    //        characteristics_script.part_type = part_type_enum.accessory.ToString();
                    //        characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                    //        characteristics_script.part_group = "Frame";
                    //        if (child_t.name.Contains("FieldDivider"))
                    //        {
                    //            characteristics_script.part_name_id = divider_type;
                    //            characteristics_script.part_type = part_type_enum.vertical.ToString();
                    //        }
                    //    }
                    //}

                    #endregion
                }
                else if (dsElement_pergola_header.Tables[0].Rows[0]["rafafa_placement_type"].ToString() == "type_3")
                {
                    #region Divider Inverted 

                    Divider_Parent = new GameObject("Divider_Parent");
                    Divider_Parent.transform.parent = Pergola_Model.transform;

                    FrameDividers_Parent = new GameObject("FrameDividers_Parent");
                    FrameDividers_Parent.transform.parent = Divider_Parent.transform;

                    FrameDividers_Parent_Section1 = new GameObject("FrameDividers_Parent_Section1");
                    FrameDividers_Parent_Section1.transform.parent = FrameDividers_Parent.transform;

                    FrameDividers_Parent_Section2 = new GameObject("FrameDividers_Parent_Section2");
                    FrameDividers_Parent_Section2.transform.parent = FrameDividers_Parent.transform;

                    //RaycastHit hit_BtoD;
                    GameObject frame_B = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameB")) //TODO: here check if frame is sliced, then get the first or last sliced part accordingly (FrameB_0 or FrameB_n)
                            frame_B = frame.gameObject;
                    }
                    Bounds frame_B_bound = Calculate_b(frame_B.transform);
                    Vector3 global_center_Frame_B = Pergola_Model.transform.TransformPoint(frame_B_bound.center);
                    Vector3 ray_cast_dir = Pergola_Model.transform.TransformDirection(Pergola_Model.transform.right);
                    Vector3 top_point_frame_B = global_center_Frame_B + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, frame_B_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_B, ray_cast_dir, out hit_BtoD, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_B, ray_cast_dir * hit_BtoD.distance, Color.yellow);
                        Debug.Log($"Did Hit & Distance={hit_BtoD.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_B, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("Did not Hit");
                    }

                    //RaycastHit hit_AtoE;
                    GameObject frame_A = null;
                    foreach (Transform frame in Frame_Parent.transform)
                    {
                        if (frame.name.Contains("FrameA"))
                            frame_A = frame.gameObject;
                    }
                    Bounds frame_A_bound = Calculate_b(frame_A.transform);
                    Vector3 global_center_Frame_A = Pergola_Model.transform.TransformPoint(frame_A_bound.center);
                    ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.forward);
                    Vector3 top_point_frame_A = global_center_Frame_A + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, frame_A_bound.size)) / 2;

                    if (Physics.Raycast(top_point_frame_A, ray_cast_dir, out hit_AtoE, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_frame_A, ray_cast_dir * hit_AtoE.distance, Color.yellow);
                        Debug.Log($"Did Hit & Distance={hit_BtoD.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_frame_A, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("Did not Hit");
                    }


                    //Frame Divider Code
                    float dividerPoleDistance = frameDividers.each_field_width + framePartSettings._part_depth;
                    int clamp_naming_counter = 0;

                    for (int i = 0; i < 2; i++)
                    {
                        // GameObject frameDivider_GO_parent = new GameObject("FrameDivider_Parent_" + i);
                        // frameDivider_GO_parent.transform.parent = FrameDividers_Parent.transform;

                        GameObject frameDivider_GO = Instantiate(DividerBar_prefab, FrameDividers_Parent_Section1.transform);
                        frameDivider_GO.transform.localPosition = vertical_bar_sideB.transform.localPosition; //Todo add(offset) frame B or D width to each field width
                        if (i == 0)
                        {
                            frameDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            frameDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                            frameDivider_GO.transform.Translate(Vector3.right * (hit_BtoD.distance + framePartSettings._part_depth), Space.World);
                        }
                        else if (i == 1)
                        {
                            frameDivider_GO.transform.localRotation = Quaternion.Euler(0, -90, 90);
                            frameDivider_GO.transform.localScale = new Vector3(1, hit_AtoE.distance, 1);
                            frameDivider_GO.transform.Translate(Vector3.right * framePartSettings._part_depth, Space.World);
                        }

                        frameDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        frameDivider_GO.name = "FrameDivider_" + i;
                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, FrameDividers_Parent_Section1.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = frameDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = frameDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, FrameDividers_Parent_Section2.transform);
                            C_Clamp_ForDivider_2.transform.Translate(frameDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }

                        if (i == 0)
                        {
                            frameDivider_GO.transform.parent = FrameDividers_Parent_Section1.transform;
                        }
                        else if (i == 1)
                        {
                            frameDivider_GO.transform.parent = FrameDividers_Parent_Section2.transform;
                        }

                    }

                    // characteristics
                    foreach (Transform t in FrameDividers_Parent.transform)
                    {
                        foreach (Transform child_t in t)
                        {
                            characteristics_script = child_t.gameObject.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = clampfordivider;
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_group = "Frame";
                            if (child_t.name.Contains("FrameDivider"))
                            {
                                characteristics_script.part_name_id = divider_type;
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                            }
                        }
                    }


                    //Field Divider Code
                    FieldDividers_Parent = new GameObject("FieldDividers_Parent");
                    FieldDividers_Parent.transform.parent = Divider_Parent.transform;

                    FieldDividers_Parent_Section1 = new GameObject("FieldDividers_Parent_Section1");
                    FieldDividers_Parent_Section1.transform.parent = FieldDividers_Parent.transform;

                    FieldDividers_Parent_Section2 = new GameObject("FieldDividers_Parent_Section2");
                    FieldDividers_Parent_Section2.transform.parent = FieldDividers_Parent.transform;

                    int divider_naming_counter = 0;


                    RaycastHit hit_DividertoE;
                    GameObject FrameDivider_0 = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section1").Find("FrameDivider_0").gameObject;
                    Bounds FrameDivider_0_bound = Calculate_b(FrameDivider_0.transform);
                    Vector3 global_center_FrameDivider_0 = Pergola_Model.transform.TransformPoint(FrameDivider_0_bound.center);
                    ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.forward);
                    Vector3 top_point_FrameDivider_0 = global_center_FrameDivider_0 + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, FrameDivider_0_bound.size)) / 2;
                    if (Physics.Raycast(top_point_FrameDivider_0, ray_cast_dir, out hit_DividertoE, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * hit_DividertoE.distance, Color.yellow);
                        Debug.Log($"hit_DividertoE  Did Hit & Distance={hit_DividertoE.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * 10000, Color.white, Mathf.Infinity);
                        Debug.Log("hit_DividertoE  Did not Hit");
                    }
                    InnerFieldDividers innerFieldDividers = new InnerFieldDividers(hit_DividertoE.distance, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                    float inner_dividerPoleDistance = framePartSettings._part_depth + innerFieldDividers.each_inner_field_width;
                    for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                    {
                        GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + divider_naming_counter);
                        fieldDivider_GO_parent.transform.parent = FieldDividers_Parent_Section1.transform;

                        GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);
                        fieldDivider_GO.transform.localPosition = new Vector3(-inner_dividerPoleDistance, 0, framePartSettings._part_depth); //Todo add(offset) frame B or D width to each field width
                        fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                        fieldDivider_GO.transform.localScale = new Vector3(1, hit_BtoD.distance, 1);
                        fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        fieldDivider_GO.name = "FieldDivider_" + divider_naming_counter++;
                        inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }
                    total_number_of_fields_DividertoE = innerFieldDividers.numbefOfInnerFields;
                    each_inner_field_width_fieldDividertoE = innerFieldDividers.each_inner_field_width;


                    RaycastHit hit_DividertoC;
                    ray_cast_dir = Pergola_Model.transform.TransformDirection(Pergola_Model.transform.forward);
                    top_point_FrameDivider_0 = global_center_FrameDivider_0 + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, FrameDivider_0_bound.size)) / 2;
                    if (Physics.Raycast(top_point_FrameDivider_0, ray_cast_dir, out hit_DividertoC, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * hit_DividertoC.distance, Color.yellow);
                        Debug.Log($"hit_DividertoC  Did Hit & Distance={hit_DividertoC.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_FrameDivider_0, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("hit_DividertoC  Did not Hit");
                    }
                    innerFieldDividers = new InnerFieldDividers(hit_DividertoC.distance, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                    float Offser = vertical_bar_sideB.transform.localPosition.z + framePartSettings._part_depth;
                    inner_dividerPoleDistance = framePartSettings._part_depth + innerFieldDividers.each_inner_field_width;
                    for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                    {
                        GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + divider_naming_counter);
                        fieldDivider_GO_parent.transform.parent = FieldDividers_Parent_Section1.transform;

                        GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);
                        fieldDivider_GO.transform.localPosition = new Vector3(-inner_dividerPoleDistance, 0, Offser); //Todo add(offset) frame B or D width to each field width
                        fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 90, 90);
                        fieldDivider_GO.transform.localScale = new Vector3(1, hit_DividertoC.distance, 1);
                        fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        fieldDivider_GO.name = "FieldDivider_" + divider_naming_counter++;
                        inner_dividerPoleDistance += (innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth);

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }
                    total_number_of_fields_DividertoC = innerFieldDividers.numbefOfInnerFields;
                    each_inner_field_width_fieldDividertoC = innerFieldDividers.each_inner_field_width;


                    RaycastHit hit_DividertoF;
                    GameObject FrameDivider_1 = FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section2").Find("FrameDivider_1").gameObject;
                    Bounds FrameDivider_1_bound = Calculate_b(FrameDivider_1.transform);
                    Vector3 global_center_FrameDivider_1 = Pergola_Model.transform.TransformPoint(FrameDivider_1_bound.center);
                    ray_cast_dir = Pergola_Model.transform.TransformDirection(-Pergola_Model.transform.right);
                    Vector3 top_point_FrameDivider_1 = global_center_FrameDivider_1 + ray_cast_dir * Mathf.Abs(Vector3.Dot(ray_cast_dir, FrameDivider_1_bound.size)) / 2;
                    if (Physics.Raycast(top_point_FrameDivider_1, ray_cast_dir, out hit_DividertoF, Mathf.Infinity))
                    {
                        Debug.DrawRay(top_point_FrameDivider_1, ray_cast_dir * hit_DividertoF.distance, Color.yellow);
                        Debug.Log($"hit_DividertoF  Did Hit & Distance={hit_DividertoF.distance}");
                    }
                    else
                    {
                        Debug.DrawRay(top_point_FrameDivider_1, ray_cast_dir * 10000, Color.white, 10);
                        Debug.Log("hit_DividertoF  Did not Hit");
                    }



                    innerFieldDividers = new InnerFieldDividers(hit_DividertoF.distance, verticalPartSettings, MAX_FIELD_DIVIDER_DISTANCE);
                    float offset = -(FrameDividers_Parent.transform.Find("FrameDividers_Parent_Section2").Find("FrameDivider_1").transform.localPosition.x);
                    inner_dividerPoleDistance = innerFieldDividers.each_inner_field_width + framePartSettings._part_depth;
                    for (int j = 0; j < innerFieldDividers.numberOfInnerDividerPoles; j++)
                    {
                        GameObject fieldDivider_GO_parent = new GameObject("FieldDivider_Parent_" + divider_naming_counter);
                        fieldDivider_GO_parent.transform.parent = FieldDividers_Parent_Section2.transform;

                        GameObject fieldDivider_GO = Instantiate(DividerBar_prefab, fieldDivider_GO_parent.transform);
                        fieldDivider_GO.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        fieldDivider_GO.transform.localPosition = new Vector3(-offset, 0, inner_dividerPoleDistance);
                        fieldDivider_GO.transform.localScale = new Vector3(1, hit_DividertoF.distance, 1);
                        fieldDivider_GO.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                        fieldDivider_GO.transform.Translate(-Vector3.right * verticalPartSettings._part_depth, Space.World);
                        fieldDivider_GO.name = "FieldDivider_" + divider_naming_counter++;
                        inner_dividerPoleDistance += innerFieldDividers.each_inner_field_width + verticalPartSettings._part_depth;

                        if (divider_type == "ak - 279")
                        {
                            GameObject C_Clamp_ForDivider = Instantiate(c_clamp_fordivider_prefab, fieldDivider_GO_parent.transform);
                            C_Clamp_ForDivider.transform.Find("Locator_top").position = fieldDivider_GO.transform.Find("Locator_TopInside_for_cClamp").position;
                            C_Clamp_ForDivider.transform.Find("Locator_top").transform.right = fieldDivider_GO.transform.up;
                            C_Clamp_ForDivider.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider.name = clampfordivider + "_Front_" + clamp_naming_counter++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider.name))
                                Accessories_name.Add(C_Clamp_ForDivider.name);

                            GameObject C_Clamp_ForDivider_2 = Instantiate(C_Clamp_ForDivider, fieldDivider_GO_parent.transform);
                            C_Clamp_ForDivider_2.transform.Translate(fieldDivider_GO.transform.localScale.y * Vector3.right, Space.Self);
                            C_Clamp_ForDivider_2.transform.Find("Locator_top").localRotation = Quaternion.Euler(0, 180, 0);
                            C_Clamp_ForDivider_2.transform.GetComponent<Follow>().move_parent_relative_toChild();
                            C_Clamp_ForDivider_2.name = clampfordivider + "_Rear_" + clamp_naming_counter++;

                            if (!Accessories_name.Contains(C_Clamp_ForDivider_2.name))
                                Accessories_name.Add((C_Clamp_ForDivider_2.name));
                        }
                    }
                    total_number_of_fields_DividertoF = innerFieldDividers.numbefOfInnerFields;
                    each_inner_field_width_fieldDividertoF = innerFieldDividers.each_inner_field_width;

                    // characteristics
                    foreach (Transform t in FieldDividers_Parent.transform)
                    {
                        foreach (Transform child_t in t)
                        {
                            characteristics_script = child_t.gameObject.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = clampfordivider;
                            characteristics_script.part_type = part_type_enum.accessory.ToString();
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_group = "Frame";
                            if (child_t.name.Contains("FieldDivider"))
                            {
                                characteristics_script.part_name_id = divider_type;
                                characteristics_script.part_type = part_type_enum.vertical.ToString();
                            }
                        }
                    }

                    #endregion
                }


                try
                {
                    step_cut(frame_type, divider_type);
                }
                catch (Exception Ex)
                {

                    Debug.Log("step cut U type:"+Ex);
                }
            }

            #region Combining Faces of sliced Frames

            foreach (Transform t in Frame_Parent.transform)
            {
                var mc = t.gameObject.AddComponent<MeshCombiner>();
                mc.CreateMultiMaterialMesh = true;
                mc.DestroyCombinedChildren = true;
                mc.CombineMeshes(false);
                mc.transform.gameObject.AddComponent<BoxCollider>();

                Probuilderize_gameObject(t);
            }

            #endregion

            //if (System.Reflection.Assembly.GetExecutingAssembly().Location.StartsWith("C:\\Unity"))
            //    apply_grayScale_material_for_pergola();
            //else
            //    apply_material_for_pergola();



            // try
            // {

            //     //Here we apply textures to frame and fields
            //     apply_Textures_RAL(rafafa_color_texture, frame_color_texture);
            //     ////Apply texture to other Parts
            //     apply_material_for_pergola();
            //     Debug.Log("applying materials");
            // }
            // catch (Exception ex_mat)
            // {

            //     Debug.Log("While Applying Materials :" + ex_mat);
            // }


            //if (wall_sides.Contains("C") && !wall_sides.Contains("A"))
            //{
            //    //arrange_support_bars_for_wall_c();
            //}

            Save_to_db_PergolaManagement(model_type);

            try
            {
                await UnityMainThreadDispatcher.DispatchAsync(() => camera_resizing_for_Pergola());
            }
            catch (Exception ex)
            {
                print("camera_resizing_for_Pergola :" + ex);
            }

            return 5;
        }

        public async void step_cut(string frame_type,string divider_type)
        {

            try
            {
                  GameObject  Frame_Parent = GameObject.Find("Frame_Parent");
                    foreach (Transform frames in Frame_Parent.transform)
                    {
                        GameObject mesh_ch = frames.GetChild(0).gameObject;

                        mesh_ch.gameObject.layer = frame_layer;
                    }
                if(I_type)
                {
                    #region Step_cut

                    foreach (Transform Div_ch in Divider_Parent.transform)
                    {
                        foreach (Transform frm_field_ch in Div_ch)
                        {


                            foreach (Transform divs_par in frm_field_ch)
                            {
                                GameObject div = null;
                                //if (divs_par.childCount > 0)
                                div = divs_par.gameObject;//.GetChild(0)
                                if (div != null)
                                {

                                    if (crown_names.Contains(frame_type))
                                    {
                                        float val = div.transform.localScale.y, step_cut_width = 0;
                                        bool step_cut = false;
                                        Vector3 dir = div.transform.up;
                                        Bounds bound_div_bar = div.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                        float max_depth = Mathf.Max(bound_div_bar.size.x, bound_div_bar.size.y, bound_div_bar.size.z);
                                        if (max_depth > 80)
                                        {
                                            //continuation
                                            GameObject type_q_pf = (GameObject)Resources.Load($"prefabs/{divider_type}_type1", typeof(GameObject));
                                            if (type_q_pf != null)
                                            {
                                                Bounds t1_bounds;//= type_q_pf.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                                GameObject loc1 = div.transform.Find("Locator_1").gameObject;
                                                GameObject loc2 = div.transform.Find("Locator_2").gameObject;
                                                RaycastHit hit_frameRfield;

                                                //If frame is hit we can place the cut part of the divider for crown
                                                if (Physics.Raycast(div.transform.TransformPoint(bound_div_bar.center), (-div.transform.up), out hit_frameRfield, Mathf.Infinity))//, 1 << frame_layer.value))
                                                {
                                                    Debug.DrawRay(div.transform.TransformPoint(bound_div_bar.center), -div.transform.up * 10000, Color.blue, 20f);
                                                    if (hit_frameRfield.transform.parent != null)
                                                        if (hit_frameRfield.transform.parent.name.ToLower().Contains("frame") && !hit_frameRfield.transform.parent.name.ToLower().Contains("divider"))
                                                        {
                                                            GameObject t1 = GameObject.Instantiate(type_q_pf);
                                                            Follow follow = t1.AddComponent<Follow>();
                                                            t1.transform.Find("Locator").position = loc1.transform.position;
                                                            t1.transform.Find("Locator").rotation = loc1.transform.rotation;
                                                            follow.follow_child = t1.transform.Find("Locator");
                                                            t1.name = type_q_pf.name + "_div_1";
                                                            // follow.update_location_and_rotation();
                                                            follow.move_parent_relative_toChild();
                                                            //t1.transform.position=  div.transform.Find("Locator_1").transform.position;
                                                            t1.transform.parent = div.transform;

                                                            //rotate_around_center_rotateAround(t2, Vector3.up, 180);
                                                            t1_bounds = t1.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                                            //step cut characteristics
                                                            step_cut = true;
                                                            step_cut_width += t1_bounds.size.y;

                                                        }
                                                }


                                                RaycastHit hit_frameRfield_2;
                                                //If frame is hit we can place the cut part of the divider for crown
                                                if (Physics.Raycast(div.transform.TransformPoint(bound_div_bar.center), (div.transform.up), out hit_frameRfield_2, Mathf.Infinity))//, 1 << frame_layer.value))
                                                {
                                                    Debug.DrawRay(div.transform.TransformPoint(bound_div_bar.center), div.transform.up * 10000, Color.red, 20f);
                                                    if (hit_frameRfield_2.transform.parent != null)
                                                        if (hit_frameRfield_2.transform.parent.name.ToLower().Contains("frame") && !hit_frameRfield_2.transform.parent.name.ToLower().Contains("divider"))
                                                        {

                                                            GameObject t2 = GameObject.Instantiate(type_q_pf);
                                                            Follow follow_2 = t2.AddComponent<Follow>();
                                                            t2.transform.Find("Locator").position = loc1.transform.position;
                                                            t2.transform.Find("Locator").rotation = loc1.transform.rotation;
                                                            follow_2.follow_child = t2.transform.Find("Locator");
                                                            follow_2.move_parent_relative_toChild();
                                                            t2.name = type_q_pf.name + "_div_2";
                                                            //rotate_around_center_rotateAround(t2, Vector3.up, 180);
                                                            //Bounds t2_bounds = t2.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                                            t1_bounds = t2.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                                            t2.transform.Translate(div.transform.InverseTransformDirection(-div.transform.up) * (Mathf.Abs(val) + t1_bounds.size.y));
                                                            //follow.update_location_and_rotation();
                                                            //t2.transform.position=  div.transform.Find("Locator_2").transform.position;
                                                            t2.transform.parent = div.transform;
                                                            //step cut characteristics
                                                            step_cut = true;
                                                            step_cut_width += t1_bounds.size.y;
                                                        }
                                                }

                                                Characteristics chrs = div.gameObject.GetComponent<Characteristics>();


                                                if (chrs == null)
                                                {

                                                    chrs = div.gameObject.AddComponent<Characteristics>();

                                                }
                                                chrs.part_type = part_type_enum.vertical.ToString();
                                                chrs.part_name_id = divider_type;
                                                chrs.part_unique_id = Guid.NewGuid().ToString();
                                                if (step_cut == true)
                                                {
                                                    chrs.step_cut = step_cut;

                                                    chrs.step_cut_width = step_cut_width;
                                                }
                                            }

                                        }

                                    }

                                    var mc = div.gameObject.AddComponent<MeshCombiner>();
                                    mc.CreateMultiMaterialMesh = true;
                                    mc.DestroyCombinedChildren = true;
                                    mc.CombineMeshes(false);
                                    mc.transform.gameObject.AddComponent<BoxCollider>();
                                    mc.gameObject.layer = divider_layer;
                                    Probuilderize_gameObject(mc.transform);
                                }
                            }
                        }
                    }
                    #endregion

                }

                else if (L_type )
                {
                    //step_cut(frame_type, divider_type);
                    #region step_Cut
                    //GameObject Frame_Parent = GameObject.Find("Frame_Parent");
                    //foreach (Transform frames in Frame_Parent.transform)
                    //{
                    //    GameObject mesh_ch = frames.GetChild(0).gameObject;

                    //    mesh_ch.gameObject.layer = frame_layer;


                    //}

                    foreach (Transform Div_ch in Divider_Parent.transform)
                    {
                        foreach (Transform frm_field_ch in Div_ch)
                        {


                            foreach (Transform divs_par in frm_field_ch)
                            {
                                GameObject div = null;
                                //if (divs_par.childCount > 0) 
                                //    div = divs_par.GetChild(0).gameObject;

                                foreach(Transform divs in divs_par)
                                {
                                    if(divs.name.Contains("Divider"))
                                    {
                                        div = divs.gameObject;
                                    }
                                }
                                if (div != null)
                                {

                                    if (crown_names.Contains(frame_type))
                                    {
                                        float val = div.transform.localScale.y, step_cut_width = 0;
                                        bool step_cut = false;
                                        Vector3 dir = div.transform.up;
                                        Bounds bound_div_bar = div.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                        float max_depth = Mathf.Max(bound_div_bar.size.x, bound_div_bar.size.y, bound_div_bar.size.z);
                                        if (max_depth > 80)
                                        {
                                            //continuation
                                            GameObject type_q_pf = (GameObject)Resources.Load($"prefabs/{divider_type}_type1", typeof(GameObject));
                                            if (type_q_pf != null)
                                            {
                                                Bounds t1_bounds;//= type_q_pf.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                                GameObject loc1 = div.transform.Find("Locator_1").gameObject;
                                                GameObject loc2 = div.transform.Find("Locator_2").gameObject;
                                                RaycastHit hit_frameRfield;

                                                //If frame is hit we can place the cut part of the divider for crown
                                                if (Physics.Raycast(div.transform.TransformPoint(bound_div_bar.center), (-div.transform.up), out hit_frameRfield, Mathf.Infinity))//, 1 << frame_layer.value))
                                                {
                                                    Debug.DrawRay(div.transform.TransformPoint(bound_div_bar.center), -div.transform.up * 10000, Color.blue, 20f);
                                                    if (hit_frameRfield.transform.parent != null)
                                                        if (hit_frameRfield.transform.parent.name.ToLower().Contains("frame") && !hit_frameRfield.transform.parent.name.ToLower().Contains("divider"))
                                                        {
                                                            GameObject t1 = GameObject.Instantiate(type_q_pf);
                                                            Follow follow = t1.AddComponent<Follow>();
                                                            t1.transform.Find("Locator").position = loc1.transform.position;
                                                            t1.transform.Find("Locator").rotation = loc1.transform.rotation;
                                                            follow.follow_child = t1.transform.Find("Locator");
                                                            t1.name = type_q_pf.name + "_div_1";
                                                            // follow.update_location_and_rotation();
                                                            follow.move_parent_relative_toChild();
                                                            //t1.transform.position=  div.transform.Find("Locator_1").transform.position;
                                                            t1.transform.parent = div.transform;

                                                            //rotate_around_center_rotateAround(t2, Vector3.up, 180);
                                                            t1_bounds = t1.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                                            //step cut characteristics
                                                            step_cut = true;
                                                            step_cut_width += t1_bounds.size.y;

                                                        }
                                                }


                                                RaycastHit hit_frameRfield_2;
                                                //If frame is hit we can place the cut part of the divider for crown
                                                if (Physics.Raycast(div.transform.TransformPoint(bound_div_bar.center), (div.transform.up), out hit_frameRfield_2, Mathf.Infinity))//, 1 << frame_layer.value))
                                                {
                                                    Debug.DrawRay(div.transform.TransformPoint(bound_div_bar.center), div.transform.up * 10000, Color.red, 20f);
                                                    if (hit_frameRfield_2.transform.parent != null)
                                                        if (hit_frameRfield_2.transform.parent.name.ToLower().Contains("frame") && !hit_frameRfield_2.transform.parent.name.ToLower().Contains("divider"))
                                                        {

                                                            GameObject t2 = GameObject.Instantiate(type_q_pf);
                                                            Follow follow_2 = t2.AddComponent<Follow>();
                                                            t2.transform.Find("Locator").position = loc1.transform.position;
                                                            t2.transform.Find("Locator").rotation = loc1.transform.rotation;
                                                            follow_2.follow_child = t2.transform.Find("Locator");
                                                            follow_2.move_parent_relative_toChild();
                                                            t2.name = type_q_pf.name + "_div_2";
                                                            //rotate_around_center_rotateAround(t2, Vector3.up, 180);
                                                            //Bounds t2_bounds = t2.GetComponentInChildren<MeshFilter>().mesh.bounds;
                                                            t1_bounds = t2.GetComponentInChildren<MeshFilter>().mesh.bounds;

                                                            t2.transform.Translate(div.transform.InverseTransformDirection(-div.transform.up) * (Mathf.Abs(val) + t1_bounds.size.y));
                                                            //follow.update_location_and_rotation();
                                                            //t2.transform.position=  div.transform.Find("Locator_2").transform.position;
                                                            t2.transform.parent = div.transform;
                                                            //step cut characteristics
                                                            step_cut = true;
                                                            step_cut_width += t1_bounds.size.y;
                                                        }
                                                }

                                                Characteristics chrs = div.gameObject.GetComponent<Characteristics>();


                                                if (chrs == null)
                                                {

                                                    chrs = div.gameObject.AddComponent<Characteristics>();

                                                }
                                                chrs.part_type = part_type_enum.vertical.ToString();
                                                chrs.part_name_id = divider_type;
                                                chrs.part_unique_id = Guid.NewGuid().ToString();
                                                if (step_cut == true)
                                                {
                                                    chrs.step_cut = step_cut;

                                                    chrs.step_cut_width = step_cut_width;
                                                }
                                            }

                                        }

                                    }

                                    var mc = div.gameObject.AddComponent<MeshCombiner>();
                                    mc.CreateMultiMaterialMesh = true;
                                    mc.DestroyCombinedChildren = true;
                                    mc.CombineMeshes(false);
                                    mc.transform.gameObject.AddComponent<BoxCollider>();
                                    mc.gameObject.layer = divider_layer;

                                    Probuilderize_gameObject(mc.transform);
                                }
                            }
                        }
                    }
                    #endregion
                }
                
            }
            catch (Exception divider_part_add)
            {

                print("Divider Parts :" + divider_part_add);
            }
        }

        public async void frame_C_clamp_groove_Alignment_()
        {
            if(I_type)
            {
                #region Groove Alignment
                GameObject first_170000048_Front_0 = GameObject.Find("170000048_Front_0");

                if (first_170000048_Front_0 != null)
                {
                    GameObject first_170000048_Locator_groove = first_170000048_Front_0.transform.Find("Locator_groove").gameObject;




                    GameObject FrameC;// GameObject frame_C;
                    if (GameObject.Find("FrameC_0"))
                    {
                        FrameC = GameObject.Find("FrameC_0");
                    }
                    else
                    {
                        FrameC = GameObject.Find("FrameC");
                    }


                    GameObject FrameC_locator_groove = FrameC.transform.Find("Locator_groove").gameObject;

                    if (FrameC_locator_groove != null)
                    {
                        float dist = FrameC_locator_groove.transform.position.y - first_170000048_Locator_groove.transform.position.y;

                        print("Distance between Grooves : " + dist);

                        Pergola_Model = GameObject.Find("Pergola_Model");
                        foreach (Transform ch in Pergola_Model.transform)
                        {
                            if (!(ch.name.Contains("Frames_Parent")))//||ch.name.Contains("Wall_Parent")||ch.name.Contains("SupportBars_Parent")
                            {
                                print(ch.name + ": pos =" + ch.position + "before");
                                ch.transform.Translate(Vector3.up * dist, Space.World);
                                // ch.transform.position = ch.transform.position + new Vector3(0, dist, 0);
                                print(ch.name + ": pos =" + ch.position + "after");
                            }
                        }
                    }


                }
                #endregion

            }
            else if (L_type)
            {
                #region Groove Alignment
                GameObject first_170000048_Front_0 = GameObject.Find("170000048_Front_0");

                if (first_170000048_Front_0 != null)
                {
                    GameObject first_170000048_Locator_groove = first_170000048_Front_0.transform.Find("Locator_groove").gameObject;



                    GameObject FrameD;// GameObject frame_C;
                    if (GameObject.Find("FrameD_0"))
                    {
                        FrameD = GameObject.Find("FrameD_0");
                    }
                    else
                    {
                        FrameD = GameObject.Find("FrameD");
                    }
                    GameObject FrameD_locator_groove = FrameD.transform.Find("Locator_groove").gameObject;

                    if (FrameD_locator_groove != null)
                    {
                        float dist = FrameD_locator_groove.transform.position.y - first_170000048_Locator_groove.transform.position.y;

                        print("Distance between Grooves : " + dist);

                        Pergola_Model = GameObject.Find("Pergola_Model");
                        foreach (Transform ch in Pergola_Model.transform)
                        {
                            if (!(ch.name.Contains("Frames_Parent")))//||ch.name.Contains("Wall_Parent")||ch.name.Contains("SupportBars_Parent")
                            {
                                print(ch.name + ": pos =" + ch.position + "before");
                                ch.transform.Translate(Vector3.up * dist, Space.World);
                                // ch.transform.position = ch.transform.position + new Vector3(0, dist, 0);
                                print(ch.name + ": pos =" + ch.position + "after");
                            }
                        }
                    }


                }

                #endregion

            }
        }
        //**************** Give global position to the function It gives local pos wrt Pergola_Model*********//
        public static Vector3 give_local_pos_wrt_Pergola_Model(Vector3 position)
        {
            GameObject Pergola_Model = GameObject.Find("Pergola_Model");

            if (Pergola_Model != null)
            {
                return Pergola_Model.transform.InverseTransformPoint(position);
            }

            else
            {
                return Vector3.zero;
            }
        }

        public static Dictionary<string, float> support_bar_name_dist_list;

        //this function could be used if we need closest support bar in given direction
        public static GameObject support_bar_nearest(GameObject support_bar_parent,GameObject wall,Vector3 dir)
        {
            support_bar_name_dist_list = new Dictionary<string, float>();
            foreach (Transform sb in support_bar_parent.transform)
            {
                GameObject clamp_on_frame=null;

                foreach (Transform ch in sb)
                {
                    if (ch.name.Contains("Clamp_onFrame_"))
                    {
                        clamp_on_frame = ch.gameObject;
                    }
                }
                if (clamp_on_frame != null)
                {
                    //Vector3 clamp_pos_wrt_Pergola_model = give_local_pos_wrt_Pergola_Model(clamp_on_frame.transform.position);


                    float single_axis_value = Vector3.Dot(dir, clamp_on_frame.transform.position);

                    support_bar_name_dist_list.Add(sb.name, single_axis_value);
                } 
            }

            GameObject nearest_support_bar = null;

            var list = support_bar_name_dist_list.Values.ToList();

            float wall_single_axis = Vector3.Dot(wall.transform.position, dir);

            float closest_val= list.OrderBy(x => Mathf.Abs(wall_single_axis - x)).First();


            var myKey = support_bar_name_dist_list.FirstOrDefault(x => x.Value == closest_val).Key;

            nearest_support_bar = GameObject.Find(myKey);
            return nearest_support_bar;
        }

   
        GameObject GetClosest_SupportBar(Transform support_bar_parent,Transform wall)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = wall.transform.position;// transform.position;
            foreach (Transform potentialTarget in support_bar_parent.transform)
            {
                GameObject clamp_on_frame = null;

                foreach (Transform ch in potentialTarget)
                {
                    if (ch.name.Contains("Clamp_onFrame_"))
                    {
                        clamp_on_frame = ch.gameObject;
                    }
                }

                Vector3 directionToTarget = clamp_on_frame.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }

            return bestTarget.gameObject;
        }
        //**************** Give local posoition wrt Pergola_Model to the function It gives global_Position*********//
        public static Vector3 give_global_pos_wrt_Pergola_Model(Vector3 position)
        {
            GameObject Pergola_Model = GameObject.Find("Pergola_Model");

            if (Pergola_Model != null)
            {

                return Pergola_Model.transform.TransformPoint(position);
            }

            else
            {
                return Vector3.zero;
            }
        }

        public void arrange_support_bars_for_wall_c_m1()
        {
            List<Transform> support_abr_parents = new List<Transform>();

            List<Transform> clamps_on_wall = new List<Transform>();
            if (SupportBars_Parent != null)
            {
                foreach (Transform supp_bar_Par in SupportBars_Parent.transform)
                {
                    support_abr_parents.Add(supp_bar_Par);

                    //Transform clamp_on_wall = null;
                    //foreach (Transform child in supp_bar_Par)
                    //{
                    //    if (child.name.Contains("Clamp_onWall"))
                    //    {
                    //        clamp_on_wall = child;
                    //        //rotate_around_center_rotateAround(clamp_on_wall.gameObject, Pergola_Model.transform.up, 90);
                    //        //rotate_around_center(clamp_on_frame.gameObject, new Vector3(0, 90, 0));

                    //        clamps_on_wall.Add(child);
                    //    }
                    //}
                    Follow follow_script = null;
                    if (supp_bar_Par.gameObject.GetComponent<Follow>() == null)
                    {
                        follow_script = supp_bar_Par.gameObject.AddComponent<Follow>();
                    }
                    else
                    {
                        follow_script = supp_bar_Par.gameObject.GetComponent<Follow>();
                    }

                    string clamp_name = "Clamp_onWall_" + supp_bar_Par.name.Substring(supp_bar_Par.name.Length - 1);
                    Transform clamp_on_wall_transf = supp_bar_Par.transform.Find(clamp_name);
                    if (clamp_on_wall_transf != null)
                    {
                        follow_script.follow_child = clamp_on_wall_transf;
                        follow_script.update_location_and_rotation();
                        //follow_script.move_parent_relative_toChild();
                    }

                    if (clamp_on_wall_transf != null)
                        clamps_on_wall.Add(clamp_on_wall_transf);

                }


                foreach (Transform supp_bar_Par in support_abr_parents)
                {
                    //Follow follow_script = null;
                    //if (supp_bar_Par.gameObject.GetComponent<Follow>() == null)
                    //{
                    //    follow_script = supp_bar_Par.gameObject.AddComponent<Follow>();
                    //}
                    //else
                    //{
                    //    follow_script = supp_bar_Par.gameObject.GetComponent<Follow>();
                    //}

                    //string clamp_name = "Clamp_onWall_" + supp_bar_Par.name.Substring(supp_bar_Par.name.Length - 1);
                    //Transform clamp_on_wall_transf = supp_bar_Par.transform.Find(clamp_name);
                    //if (clamp_on_wall_transf != null)
                    //{
                    //    follow_script.follow_child = clamp_on_wall_transf;
                    //    //follow_script.update_location_and_rotation();
                    //    //follow_script.move_parent_relative_toChild();
                    //}

                    rotate_around_center(supp_bar_Par.gameObject, new Vector3(0, 180, 0));
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

                Bounds bound_c = frame_C.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                Vector3 global_point_of_frameC_tip = frame_C.transform.TransformPoint(bound_c.center + frame_C.transform.InverseTransformDirection(Pergola_Model.transform.right) * Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_c.size)) / 2);


                Debug.DrawRay(global_point_of_frameC_tip, Pergola_Model.transform.right * 1000, Color.red, 20);



                Vector3 center_of_frameC_wrt_Pergola_Model = give_global_pos_wrt_Pergola_Model(global_point_of_frameC_tip);

                foreach (Transform clamp_on_wall in clamps_on_wall)
                {
                    Bounds clamp_bound = clamp_on_wall.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds;

                    Vector3 global_point_of_clamp_wall_tip = clamp_on_wall.transform.TransformPoint(clamp_bound.center + clamp_on_wall.transform.InverseTransformDirection(Pergola_Model.transform.right) * Mathf.Abs(Vector3.Dot(clamp_on_wall.transform.InverseTransformDirection(Pergola_Model.transform.right), clamp_bound.size)) / 2);


                    Vector3 center_of_calmp_wall_wrt_Pergola_Model = give_global_pos_wrt_Pergola_Model(global_point_of_clamp_wall_tip);

                    //Vector3 along_frameC_tip = new Vector3(center_of_calmp_wall_wrt_Pergola_Model.x, center_of_frameC_wrt_Pergola_Model.y, center_of_frameC_wrt_Pergola_Model.z);

                    float dist = center_of_frameC_wrt_Pergola_Model.x - center_of_calmp_wall_wrt_Pergola_Model.x;//Vector3.Distance(center_of_frameC_wrt_Pergola_Model, along_frameC_tip);

                    clamp_on_wall.transform.Translate(Pergola_Model.transform.right * dist, Space.World);
                }

                foreach (Transform supp_bar_Par in support_abr_parents)
                {
                    Follow follow_script = null;
                    if (supp_bar_Par.gameObject.GetComponent<Follow>() == null)
                    {
                        follow_script = supp_bar_Par.gameObject.AddComponent<Follow>();
                    }
                    else
                    {
                        follow_script = supp_bar_Par.gameObject.GetComponent<Follow>();
                    }

                    //string clamp_name = "Clamp_onWall_" + supp_bar_Par.name.Substring(supp_bar_Par.name.Length - 1);
                    //Transform clamp_on_wall_transf = supp_bar_Par.transform.Find(clamp_name);
                    //if (clamp_on_wall_transf != null)
                    //{
                    //follow_script.follow_child = clamp_on_wall_transf;

                    if (follow_script.follow_child != null)
                    {

                        follow_script.move_parent_relative_toChild();
                    }//}
                    //foreach (Transform child in supp_bar_Par)
                    //{
                    //    if (child.name.Contains("Clamp_onWall"))
                    //    {
                    //        clamp_on_wall = child;

                    //        follow_script.follow_child=clamp_on_wall

                    //    }
                    //}
                }


                //RaycastHit hit;
                //if(Physics.Raycast(global_center_of_frameC,Pergola_Model.transform.right, out hit,Mathf.Infinity))
                //{


                //}

            }
        }
        public void arrange_support_bars_for_wall_c()
        {
            List<Transform> support_abr_parents = new List<Transform>();

            List<Transform> clamps_on_wall = new List<Transform>();
            if (SupportBars_Parent != null)
            {
                foreach (Transform supp_bar_Par in SupportBars_Parent.transform)
                {
                    support_abr_parents.Add(supp_bar_Par);

                    //Transform clamp_on_wall = null;
                    //foreach (Transform child in supp_bar_Par)
                    //{
                    //    if (child.name.Contains("Clamp_onWall"))
                    //    {
                    //        clamp_on_wall = child;
                    //        //rotate_around_center_rotateAround(clamp_on_wall.gameObject, Pergola_Model.transform.up, 90);
                    //        //rotate_around_center(clamp_on_frame.gameObject, new Vector3(0, 90, 0));

                    //        clamps_on_wall.Add(child);
                    //    }
                    //}
                    Follow follow_script = null;
                    if (supp_bar_Par.gameObject.GetComponent<Follow>() == null)
                    {
                        follow_script = supp_bar_Par.gameObject.AddComponent<Follow>();
                    }
                    else
                    {
                        follow_script = supp_bar_Par.gameObject.GetComponent<Follow>();
                    }

                    string clamp_name = "Clamp_onWall_" + supp_bar_Par.name.Substring(supp_bar_Par.name.Length - 1);
                    Transform clamp_on_wall_transf = supp_bar_Par.transform.Find(clamp_name);
                    if (clamp_on_wall_transf != null)
                    {
                        follow_script.follow_child = clamp_on_wall_transf;
                        follow_script.update_location_and_rotation();
                        //follow_script.move_parent_relative_toChild();
                    }

                    if (clamp_on_wall_transf != null)
                        clamps_on_wall.Add(clamp_on_wall_transf);

                }


                foreach (Transform supp_bar_Par in support_abr_parents)
                {
                    //Follow follow_script = null;
                    //if (supp_bar_Par.gameObject.GetComponent<Follow>() == null)
                    //{
                    //    follow_script = supp_bar_Par.gameObject.AddComponent<Follow>();
                    //}
                    //else
                    //{
                    //    follow_script = supp_bar_Par.gameObject.GetComponent<Follow>();
                    //}

                    //string clamp_name = "Clamp_onWall_" + supp_bar_Par.name.Substring(supp_bar_Par.name.Length - 1);
                    //Transform clamp_on_wall_transf = supp_bar_Par.transform.Find(clamp_name);
                    //if (clamp_on_wall_transf != null)
                    //{
                    //    follow_script.follow_child = clamp_on_wall_transf;
                    //    //follow_script.update_location_and_rotation();
                    //    //follow_script.move_parent_relative_toChild();
                    //}

                    rotate_around_center(supp_bar_Par.gameObject, new Vector3(0, 180, 0));
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

                Bounds bound_c = frame_C.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

                Vector3 global_point_of_frameC_tip = frame_C.transform.TransformPoint(bound_c.center + frame_C.transform.InverseTransformDirection(Pergola_Model.transform.right) * Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_c.size)) / 2);


                Debug.DrawRay(global_point_of_frameC_tip, Pergola_Model.transform.right * 1000, Color.red, 20);



                Vector3 center_of_frameC_wrt_Pergola_Model = give_global_pos_wrt_Pergola_Model(global_point_of_frameC_tip);

                foreach (Transform clamp_on_wall in clamps_on_wall)
                {
                    Bounds clamp_bound = clamp_on_wall.transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds;

                    Vector3 global_point_of_clamp_wall_tip = clamp_on_wall.transform.TransformPoint(clamp_bound.center + clamp_on_wall.transform.InverseTransformDirection(Pergola_Model.transform.right) * Mathf.Abs(Vector3.Dot(clamp_on_wall.transform.InverseTransformDirection(Pergola_Model.transform.right), clamp_bound.size)) / 2);


                    Vector3 center_of_calmp_wall_wrt_Pergola_Model = give_global_pos_wrt_Pergola_Model(global_point_of_clamp_wall_tip);

                    //Vector3 along_frameC_tip = new Vector3(center_of_calmp_wall_wrt_Pergola_Model.x, center_of_frameC_wrt_Pergola_Model.y, center_of_frameC_wrt_Pergola_Model.z);

                    float dist = center_of_frameC_wrt_Pergola_Model.x - center_of_calmp_wall_wrt_Pergola_Model.x;//Vector3.Distance(center_of_frameC_wrt_Pergola_Model, along_frameC_tip);

                    clamp_on_wall.transform.Translate(Pergola_Model.transform.right * dist, Space.World);
                }

                foreach (Transform supp_bar_Par in support_abr_parents)
                {
                    Follow follow_script = null;
                    if (supp_bar_Par.gameObject.GetComponent<Follow>() == null)
                    {
                        follow_script = supp_bar_Par.gameObject.AddComponent<Follow>();
                    }
                    else
                    {
                        follow_script = supp_bar_Par.gameObject.GetComponent<Follow>();
                    }

                    //string clamp_name = "Clamp_onWall_" + supp_bar_Par.name.Substring(supp_bar_Par.name.Length - 1);
                    //Transform clamp_on_wall_transf = supp_bar_Par.transform.Find(clamp_name);
                    //if (clamp_on_wall_transf != null)
                    //{
                    //follow_script.follow_child = clamp_on_wall_transf;

                    if (follow_script.follow_child != null)
                    {

                        follow_script.move_parent_relative_toChild();
                    }//}
                    //foreach (Transform child in supp_bar_Par)
                    //{
                    //    if (child.name.Contains("Clamp_onWall"))
                    //    {
                    //        clamp_on_wall = child;

                    //        follow_script.follow_child=clamp_on_wall

                    //    }
                    //}
                }


                //RaycastHit hit;
                //if(Physics.Raycast(global_center_of_frameC,Pergola_Model.transform.right, out hit,Mathf.Infinity))
                //{


                //}

            }
        }
        private void FixedUpdate()
        {
            //GameObject frame_C;
            //if (GameObject.Find("FrameC_0"))
            //{
            //    frame_C = GameObject.Find("FrameC_0");
            //}
            //else
            //{
            //    frame_C = GameObject.Find("FrameC");
            //}
            //if (frame_C != null&& Pergola_Model!=null)
            //{
            //    Bounds bound_c = frame_C.transform.GetComponentInChildren<MeshFilter>().mesh.bounds;

            //    Vector3 global_center_of_frameC = frame_C.transform.TransformPoint(bound_c.center + frame_C.transform.InverseTransformDirection(Pergola_Model.transform.right) * Mathf.Abs(Vector3.Dot(frame_C.transform.InverseTransformDirection(Pergola_Model.transform.right), bound_c.size)) / 2);


            //    Debug.DrawRay(global_center_of_frameC, Pergola_Model.transform.right * 1000, Color.red, 20);
            //}
        }
        public static void rotate_around_center_rotateAround(GameObject go, Vector3 axis_of_rot, float angle_of_rot)
        {
            GameObject dummy_Parent;

            GameObject actual_Parent = null;
            if (go.transform.parent != null)
            {
                actual_Parent = go.transform.parent.gameObject;
            }


            go.transform.parent = null;
            Bounds bound_go = Calculate_b(go.transform);


            dummy_Parent = new GameObject("dummy_Parent_" + go.name);

            dummy_Parent.transform.position = bound_go.center;

            go.transform.parent = dummy_Parent.transform;

            dummy_Parent.transform.RotateAround(dummy_Parent.transform.position, axis_of_rot, angle_of_rot);
            if (actual_Parent != null)
            {
                go.transform.parent = actual_Parent.transform;
            }
            else
            {
                go.transform.parent = null;
            }

            DestroyImmediate(dummy_Parent);

        }
        public static void Probuilderize_gameObject(Transform t)
        {
            if (t.gameObject.GetComponent<MeshFilter>() != null)
            {
                //IF NULL CREATE NEW INSTANCE
                if (pb_list == null)
                {
                    pb_list = new List<ProBuilderMesh>();
                }
                #region ProBuilderizing Frames
                MeshImportSettings settings = new MeshImportSettings()
                {
                    quads = true,
                    smoothing = true,
                    smoothingAngle = 1.0f
                };
                MeshFilter mf = t.gameObject.GetComponent<MeshFilter>();
                Mesh sourceMesh = mf.sharedMesh;
                Material[] sourceMaterials = t.gameObject.GetComponent<MeshRenderer>()?.sharedMaterials;

                try
                {
                    if (t.gameObject.GetComponent<ProBuilderMesh>() == null)
                    {
                        ProBuilderMesh destination = t.gameObject.AddComponent<ProBuilderMesh>();
                        var meshImporter = new MeshImporter(sourceMesh, sourceMaterials, destination);
                        meshImporter.Import(settings);

                        destination.ToMesh();
                        destination.Refresh();
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Failed ProBuilderizing: " + t.name + "\n" + e.ToString());
                }

                if (t.gameObject.GetComponent<ProBuilderMesh>() == null)
                    t.gameObject.AddComponent<ProBuilderMesh>();

                #endregion
            }

        }

        public class pergola_info
        {
            public string project_unique_id
            {
                get;
                set;
            }
            public string building_unique_id
            {
                get;
                set;
            }
            public string element_unique_id
            {
                get;
                set;
            }
            public string output_folder_path
            {
                get;
                set;
            }

            public string serverAddress { get; set; }
        }

        public async Task<Response> data_base_linking(Request request)
        {
            Response response = new Response();
            Debug.Log($"Application executing on thread {Thread.CurrentThread.ManagedThreadId}");
            var body = request.GetPOSTData();
            string json = body;

            //deserializing json to get ID's

            louvers_info item = JsonConvert.DeserializeObject<louvers_info>(json);

            project_unique_id = item.project_unique_id;
            building_unique_id = item.building_unique_id;
            element_unique_id = item.element_unique_id;


            Task t = new Task(async () =>
            {
                Debug.Log($"Task executing on thread {Thread.CurrentThread.ManagedThreadId}");
                string responseData = "test";

                string connetionString = "";
                SqlCommand sqlCmd = null;
                SqlDataAdapter adapter = new SqlDataAdapter();

                //building measure sql-params
                string sql_building_measures = "";
                SqlCommand sqlCmd_get_building_mesurements = null;
                DataSet dsMeasurements = new DataSet();

                //project_elements sql-params
                string sql_project_elements = "";
                SqlCommand sqlCmd_get_project_elements = null;

                DataSet ds_project_elements = new DataSet();


                connetionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=modeling;User ID=sa;Password=Password@1234 ";
                //sql = $"select COUNT(*) as row_count from dbo.tbl_model  where project_unique_id = '1234'";

                //we are taking height and width of the model from "tbl_building_mesures" DB by taking 1 st top row from table
                sql_building_measures = $"select top (1) * from  tbl_building_mesures where proj_id = {item.project_unique_id} and building_id={item.building_unique_id} and  element_id={item.element_unique_id}";

                //we are taking element details from DB
                sql_project_elements = $"select top(1) * from  tbl_project_elements where proj_id = {item.project_unique_id} and building_id={item.building_unique_id} and  element_id={item.element_unique_id}";

                //we assign the connecting string to the variable cnn. The variable cnn, which is of type SqlConnection is used to establish the connection to the database.
                sqlCnn = new SqlConnection(connetionString);

                sqlCnn.Open();//to open the connection
                //sqlCmd = new SqlCommand(sql, sqlCnn);

                try
                {
                    sqlCmd_get_building_mesurements = new SqlCommand(sql_building_measures, sqlCnn);
                    adapter.SelectCommand = sqlCmd_get_building_mesurements;
                    adapter.Fill(dsMeasurements);


                    sqlCmd_get_project_elements = new SqlCommand(sql_project_elements, sqlCnn);
                    adapter.SelectCommand = sqlCmd_get_project_elements;
                    adapter.Fill(ds_project_elements);

                    if (dsMeasurements.Tables.Count > 0 && ds_project_elements.Tables.Count > 0)
                    {
                        Task<int> result = await UnityMainThreadDispatcher.DispatchAsync(() => generate_model(dsMeasurements, ds_project_elements));
                        Debug.Log(result.Result);
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

                responseData = unique_id + ".obj";//".fbx";
                response.SetHTTPStatusCode((int)HttpStatusCode.OK);

                response.SetContent(responseData);
                response.SetMimeType(Response.MIME_CONTENT_TYPE_TEXT);

            });
            //wait untill the above action to complete
            t.RunSynchronously();
            //t.Wait();
            await t;
            return response;
            /*file directory where obj's are stored*/
        }

        public class louvers_info
        {
            public string project_unique_id
            {
                get;
                set;
            }
            public string building_unique_id
            {
                get;
                set;
            }
            public string element_unique_id
            {
                get;
                set;
            }

            public string output_folder_path
            {
                get;
                set;
            }

        }
        private async void generate_model_fromPoints()
        {
            string connetionString = null;
            SqlCommand sqlCmd = null;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            int i = 0;
            string sql = null;

            connetionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=modeling;User ID=sa;Password=Password@1234 ";
            sql = $"select * from dbo.louvers_points where  project_unique_id={project_unique_id} and building_unique_id={building_unique_id} and element_unique_id={element_unique_id} ORDER BY unity_heirarchy_level asc";
            sqlCnn = new SqlConnection(connetionString);
            sqlCnn.Open();

            if (GameObject.Find("Parent") != null)
                DestroyImmediate(GameObject.Find("Parent"));

            if (GameObject.Find("Garnd_Parent") != null)
                DestroyImmediate(GameObject.Find("Garnd_Parent"));

            if (GameObject.Find("VerticalBar_Parent") != null)
                DestroyImmediate(GameObject.Find("VerticalBar_Parent"));

            if (GameObject.Find("HorizontalBar_Parent") != null)
                DestroyImmediate(GameObject.Find("HorizontalBar_Parent"));

            if (GameObject.Find("Accessory_Parent") != null)
                DestroyImmediate(GameObject.Find("Accessory_Parent"));


            sqlCmd = new SqlCommand(sql, sqlCnn);
            SqlDataReader reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                GenerateGameObjects(reader);
            }

            try
            {

                apply_material();
                bool from_points = true;
                //here we place resize the camera view port to match the model dimension
                await UnityMainThreadDispatcher.DispatchAsync(() => camera_resizing(from_points));
            }
            catch (Exception ex)
            {
                Debug.Log("GenerateGameObjects :" + ex);
            }
            adapter.Dispose();
            sqlCmd.Dispose();
            sqlCnn.Close();
        }

        private async void GenerateGameObjects(SqlDataReader reader)
        {

            if (Convert.ToInt16(reader["unity_heirarchy_level"]) == 0)
            {
                //.Trim(); used to remove spaces before and after string
                String GO_name = reader["unity_heirarchy_name"].ToString().Trim();
                //Grand_Parent
                if (GameObject.Find(GO_name) == null)
                {
                    //heirarchy level 0 consist of empty game object
                    GameObject GO = new GameObject(GO_name);
                    GO.transform.position = new Vector3(Convert.ToSingle(reader["pos_x"]), Convert.ToSingle(reader["pos_y"]), Convert.ToSingle(reader["pos_z"]));
                    GO.transform.rotation = Quaternion.Euler(Convert.ToSingle(reader["rot_x"]), Convert.ToSingle(reader["rot_y"]), Convert.ToSingle(reader["rot_z"]));
                    GO.transform.localScale = new Vector3(Convert.ToSingle(reader["scale_x"]), Convert.ToSingle(reader["scale_y"]), Convert.ToSingle(reader["scale_z"]));
                    GO.SetActive(true);
                }
            }
            else if (Convert.ToInt16(reader["unity_heirarchy_level"]) == 1)
            {
                //.Trim(); used to remove spaces before and after strinf
                String GO_name = reader["unity_heirarchy_name"].ToString().Trim();

                string[] GO_name_array = GO_name.Split('/');

                //heirarchy level 1 consist of empty game object

                string part_name_id = reader["part_name_id"].ToString();

                GameObject GO = new GameObject(GO_name_array[1]);
                GO.transform.parent = GameObject.Find(GO_name_array[0]).transform;
                GO.transform.position = new Vector3(Convert.ToSingle(reader["pos_x"]), Convert.ToSingle(reader["pos_y"]), Convert.ToSingle(reader["pos_z"]));
                GO.transform.rotation = Quaternion.Euler(Convert.ToSingle(reader["rot_x"]), Convert.ToSingle(reader["rot_y"]), Convert.ToSingle(reader["rot_z"]));
                GO.transform.localScale = new Vector3(Convert.ToSingle(reader["scale_x"]), Convert.ToSingle(reader["scale_y"]), Convert.ToSingle(reader["scale_z"]));
                GO.SetActive(true);


            }
            else if (Convert.ToInt16(reader["unity_heirarchy_level"]) == 2)
            {


                String GO_name = reader["unity_heirarchy_name"].ToString().Trim();
                string[] GO_name_array = GO_name.Split('/');
                string part_name_id = reader["part_name_id"].ToString();//it is one of the name in Prefabs
                Bar_Prefab = (GameObject)Resources.Load($"prefabs/{part_name_id}", typeof(GameObject));
                if (Bar_Prefab != null)
                {
                    GameObject GO = Instantiate(Bar_Prefab);
                    GO.name = GO_name_array[2];
                    GO.transform.parent = GameObject.Find(GO_name_array[1]).transform;
                    GO.transform.localPosition = new Vector3(Convert.ToSingle(reader["pos_x"]), Convert.ToSingle(reader["pos_y"]), Convert.ToSingle(reader["pos_z"]));
                    GO.transform.localRotation = Quaternion.Euler(Convert.ToSingle(reader["rot_x"]), Convert.ToSingle(reader["rot_y"]), Convert.ToSingle(reader["rot_z"]));
                    GO.transform.localScale = new Vector3(Convert.ToSingle(reader["scale_x"]), Convert.ToSingle(reader["scale_y"]), Convert.ToSingle(reader["scale_z"]));

                }

            }
            else if (Convert.ToInt16(reader["unity_heirarchy_level"]) == 3)
            {
                String GO_name = reader["unity_heirarchy_name"].ToString().Trim();
                string[] GO_name_array = GO_name.Split('/');
                Bar_Prefab = (GameObject)Resources.Load($"prefabs/{GO_name_array[3]}", typeof(GameObject));
                if (Bar_Prefab != null)
                {
                    GameObject GO = Instantiate(Bar_Prefab);
                    GO.name = GO_name_array[3];
                    GO.transform.parent = GameObject.Find(GO_name_array[2]).transform;
                    GO.transform.localPosition = new Vector3(Convert.ToSingle(reader["pos_x"]), Convert.ToSingle(reader["pos_y"]), Convert.ToSingle(reader["pos_z"]));
                    GO.transform.localRotation = Quaternion.Euler(Convert.ToSingle(reader["rot_x"]), Convert.ToSingle(reader["rot_y"]), Convert.ToSingle(reader["rot_z"]));
                    GO.transform.localScale = new Vector3(Convert.ToSingle(reader["scale_x"]), Convert.ToSingle(reader["scale_y"]), Convert.ToSingle(reader["scale_z"]));

                }

            }


        }

        private async void GenerateGameObjects1(SqlDataReader reader)
        {

            if (Convert.ToInt16(reader["unity_heirarchy_level"]) == 0)
            {
                String GO_name = reader["unity_heirarchy_name"].ToString().Trim();
                if (GameObject.Find(GO_name) == null)
                {
                    GameObject GO = new GameObject(GO_name);
                    GO.transform.position = new Vector3(Convert.ToSingle(reader["pos_x"]), Convert.ToSingle(reader["pos_y"]), Convert.ToSingle(reader["pos_z"]));
                    GO.transform.rotation = Quaternion.Euler(Convert.ToSingle(reader["rot_x"]), Convert.ToSingle(reader["rot_y"]), Convert.ToSingle(reader["rot_z"]));
                    GO.transform.localScale = new Vector3(Convert.ToSingle(reader["scale_x"]), Convert.ToSingle(reader["scale_y"]), Convert.ToSingle(reader["scale_z"]));
                    GO.SetActive(true);
                }
            }
            else if (Convert.ToInt16(reader["unity_heirarchy_level"]) == 1)
            {
                String GO_name = reader["unity_heirarchy_name"].ToString().Trim();
                string[] GO_name_array = GO_name.Split('/');

                string[] GO_name_actual = GO_name_array[1].Split('_');
                Bar_Prefab = (GameObject)Resources.Load($"prefabs/{GO_name_actual[0]}", typeof(GameObject));
                if (Bar_Prefab != null || GameObject.Find(GO_name_actual[0]) == null)
                {
                    GameObject GO = Instantiate(Bar_Prefab);
                    //putting back the old name before splitting
                    GO.name = GO_name_array[1];
                    GO.transform.parent = GameObject.Find(GO_name_array[0]).transform;
                    GO.transform.localPosition = new Vector3(Convert.ToSingle(reader["pos_x"]), Convert.ToSingle(reader["pos_y"]), Convert.ToSingle(reader["pos_z"]));
                    GO.transform.localRotation = Quaternion.Euler(Convert.ToSingle(reader["rot_x"]), Convert.ToSingle(reader["rot_y"]), Convert.ToSingle(reader["rot_z"]));
                    GO.transform.localScale = new Vector3(Convert.ToSingle(reader["scale_x"]), Convert.ToSingle(reader["scale_y"]), Convert.ToSingle(reader["scale_z"]));
                }
            }
            else if (Convert.ToInt16(reader["unity_heirarchy_level"]) == 2)
            {
                String GO_name = reader["unity_heirarchy_name"].ToString().Trim();
                string[] GO_name_array = GO_name.Split('/');
                Bar_Prefab = (GameObject)Resources.Load($"prefabs/{GO_name_array[2]}", typeof(GameObject));
                if (Bar_Prefab != null)
                {
                    GameObject GO = Instantiate(Bar_Prefab);
                    GO.name = GO_name_array[2];
                    GO.transform.parent = GameObject.Find(GO_name_array[1]).transform;
                    GO.transform.localPosition = new Vector3(Convert.ToSingle(reader["pos_x"]), Convert.ToSingle(reader["pos_y"]), Convert.ToSingle(reader["pos_z"]));
                    GO.transform.localRotation = Quaternion.Euler(Convert.ToSingle(reader["rot_x"]), Convert.ToSingle(reader["rot_y"]), Convert.ToSingle(reader["rot_z"]));
                    GO.transform.localScale = new Vector3(Convert.ToSingle(reader["scale_x"]), Convert.ToSingle(reader["scale_y"]), Convert.ToSingle(reader["scale_z"]));

                }
            }
            else if (Convert.ToInt16(reader["unity_heirarchy_level"]) == 3)
            {
                String GO_name = reader["unity_heirarchy_name"].ToString().Trim();
                string[] GO_name_array = GO_name.Split('/');
                Bar_Prefab = (GameObject)Resources.Load($"prefabs/{GO_name_array[3]}", typeof(GameObject));
                if (Bar_Prefab != null)
                {
                    GameObject GO = Instantiate(Bar_Prefab);
                    GO.name = GO_name_array[3];
                    GO.transform.parent = GameObject.Find(GO_name_array[2]).transform;
                    GO.transform.localPosition = new Vector3(Convert.ToSingle(reader["pos_x"]), Convert.ToSingle(reader["pos_y"]), Convert.ToSingle(reader["pos_z"]));
                    GO.transform.localRotation = Quaternion.Euler(Convert.ToSingle(reader["rot_x"]), Convert.ToSingle(reader["rot_y"]), Convert.ToSingle(reader["rot_z"]));
                    GO.transform.localScale = new Vector3(Convert.ToSingle(reader["scale_x"]), Convert.ToSingle(reader["scale_y"]), Convert.ToSingle(reader["scale_z"]));

                }
            }

            try
            {
                //await UnityMainThreadDispatcher.DispatchAsync(() => GP());
            }
            catch (Exception ex)
            {
                Debug.Log("GenerateGameObjects :" + ex);
            }
        }
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

                #region characteristics
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
            if (Clips.Contains(horizontal_part_name))

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


                //float clip_height = Mathf.Ceil((clip_height_full - clip_spare_part_height));
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
                if (combined_Clips_names.Contains(Bar_Prefab.name))
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


            apply_material();

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
            await UnityMainThreadDispatcher.DispatchAsync(() => camera_resizing());

            try
            {

                Save_to_db_PergolaManagement();
            }
            catch (Exception ex)
            {
                Debug.Log("while saving to DB: " + ex);
            }

            return 5;
        }
        public void apply_Textures_RAL(string rafafa_texture_name = "", string frame_texture_name = "")
        {
            if (GameObject.Find("FrameDividers_Parent") != null)
                FrameDividers_Parent = GameObject.Find("FrameDividers_Parent");


            if (GameObject.Find("Field_Parent") != null)
                Field_Parent = GameObject.Find("Field_Parent");

            //if(GameObject.Find("Frames_Parent")!=null)
            //{
                Frames_Parent = GameObject.Find("Frames_Parent");
            //}

            FieldDividers_Parent = GameObject.Find("FieldDividers_Parent");
            //if(string.IsNullOrEmpty( rafafa_texture_name))
            //{
            //    //Appplying a default texture
            //    rafafa_texture_name = "cherry textured 82";
            //}

            if (Frames_Parent != null)
                foreach (Transform frame in Frames_Parent.transform)
                {
                    //adding Layer Mask
                    foreach (Transform frames in frame)
                    {
                        if (frames.gameObject.GetComponent<Renderer>() != null)
                        {
                            //frames.gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;
                            var mats = frames.GetComponentInChildren<Renderer>().materials;

                            if (!string.IsNullOrEmpty(frame_texture_name))
                            {
                                if (textures.Keys.Contains(frame_texture_name))
                                {
                                    foreach (Material m in mats)
                                    {
                                        m.shader  = Shader.Find("Standard");
                                        m.mainTexture = Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[frame_texture_name]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;
                                    }
                                }
                                else if (ral_colors.Keys.Contains(frame_texture_name))
                                {
                                    foreach (Material m in mats)
                                    {
                                        m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                        Color color;

                                        if (ColorUtility.TryParseHtmlString(ral_colors[frame_texture_name], out color))
                                        { m.color = color; }




                                    }

                                }
                            }
                            else
                            {
                                foreach (Material m in mats)
                                {
                                    m.shader = Shader.Find("Standard");
                                    m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                    Color color;

                                    if (ColorUtility.TryParseHtmlString("#8E8A8A", out color))
                                    { m.color = color; }




                                }
                            }
                        }
                    }
                }

            if (Field_Parent != null)
                foreach (Transform fieldGroup in Field_Parent.transform)
                {
                    if (I_type)
                    {
                        for (int i = 0; i < fieldGroup.childCount; i++)
                        {
                            if (fieldGroup.GetChild(i).name.Contains("Fields"))
                            {
                                foreach (Transform hBar in fieldGroup.GetChild(i).transform)
                                {

                                    var mats = hBar.GetComponentInChildren<Renderer>().materials;
                                    if (!string.IsNullOrEmpty(rafafa_texture_name))
                                    {
                                        if (textures.Keys.Contains(rafafa_texture_name))
                                        {
                                            foreach (Material m in mats)
                                            {
                                                m.shader = Shader.Find("Standard");
                                                m.mainTexture = Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[rafafa_texture_name]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;
                                            }

                                            if (space_btw_rafafa == -10)
                                            {
                                                mats = hBar.GetChild(1).GetComponentInChildren<Renderer>().materials;
                                                foreach (Material m in mats)
                                                {
                                                    m.shader = Shader.Find("Standard");
                                                    m.mainTexture = Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[rafafa_texture_name]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;
                                                }
                                                //hBar.GetChild(1).GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;
                                            }//if (hBar.GetComponentInChildren<MeshRenderer>() != null)
                                             //    hBar.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;

                                            //if (space_btw_rafafa == -10)
                                            //    hBar.GetChild(1).GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;
                                        }
                                        else
                                        {
                                            foreach (Material m in mats)
                                            {
                                                m.shader = Shader.Find("Standard");
                                                m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                                Color color;
                                                if (ral_colors.Keys.Contains(rafafa_texture_name))
                                                {
                                                    if (ColorUtility.TryParseHtmlString(ral_colors[rafafa_texture_name], out color))
                                                    { m.color = color; }
                                                }
                                                //else
                                                //{
                                                //    if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                                //    { m.color = color; }


                                                //}
                                            }

                                            if (space_btw_rafafa == -10)
                                            {
                                                mats = hBar.GetChild(1).GetComponentInChildren<Renderer>().materials;
                                                foreach (Material m in mats)
                                                {

                                                    m.shader = Shader.Find("Standard");
                                                    m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                                    Color color;

                                                    if (ral_colors.Keys.Contains(rafafa_texture_name))
                                                    {
                                                        if (ColorUtility.TryParseHtmlString(ral_colors[rafafa_texture_name], out color))
                                                        { m.color = color; }
                                                    }
                                                    //else
                                                    //{
                                                    //    if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                                    //    { m.color = color; }

                                                    //}
                                                }
                                                //hBar.GetChild(1).GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (Material m in mats)
                                        {
                                            m.shader = Shader.Find("Standard");
                                            m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                            Color color;
                                            //if (ral_colors.Keys.Contains(rafafa_texture_name))
                                            //{
                                            //    if (ColorUtility.TryParseHtmlString(ral_colors[rafafa_texture_name], out color))
                                            //    { m.color = color; }
                                            //}
                                            ////else
                                            //{
                                            if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                            { m.color = color; }


                                            //}
                                        }

                                        if (space_btw_rafafa == -10)
                                        {
                                            mats = hBar.GetChild(1).GetComponentInChildren<Renderer>().materials;
                                            foreach (Material m in mats)
                                            {
                                                m.shader = Shader.Find("Standard");
                                                m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                                Color color;

                                                //if (ral_colors.Keys.Contains(rafafa_texture_name))
                                                //{
                                                //    if (ColorUtility.TryParseHtmlString(ral_colors[rafafa_texture_name], out color))
                                                //    { m.color = color; }
                                                //}
                                                //else
                                                //{
                                                if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                                { m.color = color; }

                                                //}
                                            }
                                            //hBar.GetChild(1).GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if(L_type)
                    {
                        Transform fs = fieldGroup;
                        //Transform[] g = sub_child.GetComponentsInChildren<Transform>();
                        foreach (Transform f_g in fs)
                        {
                            foreach (Transform acs_R_field in f_g)
                            {

                                if (acs_R_field.name.Contains("Fields_"))
                                {
                                    foreach (Transform hBar in acs_R_field)
                                    {
                                        var mats = hBar.GetComponentInChildren<Renderer>().materials;
                                        if (!string.IsNullOrEmpty(rafafa_texture_name))
                                        {
                                            if (textures.Keys.Contains(rafafa_texture_name))
                                            {
                                                foreach (Material m in mats)
                                                {
                                                    m.shader = Shader.Find("Standard");
                                                    m.mainTexture = Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[rafafa_texture_name]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;
                                                }

                                                if (space_btw_rafafa == -10)
                                                {
                                                    mats = hBar.GetChild(1).GetComponentInChildren<Renderer>().materials;
                                                    foreach (Material m in mats)
                                                    {
                                                        m.shader = Shader.Find("Standard");
                                                        m.mainTexture = Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[rafafa_texture_name]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;
                                                    }
                                                    //hBar.GetChild(1).GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;
                                                }//if (hBar.GetComponentInChildren<MeshRenderer>() != null)
                                                 //    hBar.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;

                                                //if (space_btw_rafafa == -10)
                                                //    hBar.GetChild(1).GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;
                                            }
                                            else
                                            {
                                                foreach (Material m in mats)
                                                {
                                                    m.shader = Shader.Find("Standard");
                                                    m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                                    Color color;
                                                    if (ral_colors.Keys.Contains(rafafa_texture_name))
                                                    {
                                                        if (ColorUtility.TryParseHtmlString(ral_colors[rafafa_texture_name], out color))
                                                        { m.color = color; }
                                                    }
                                                    //else
                                                    //{
                                                    //    if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                                    //    { m.color = color; }


                                                    //}
                                                }

                                                if (space_btw_rafafa == -10)
                                                {
                                                    mats = hBar.GetChild(1).GetComponentInChildren<Renderer>().materials;
                                                    foreach (Material m in mats)
                                                    {
                                                        m.shader = Shader.Find("Standard");
                                                        m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                                        Color color;

                                                        if (ral_colors.Keys.Contains(rafafa_texture_name))
                                                        {
                                                            if (ColorUtility.TryParseHtmlString(ral_colors[rafafa_texture_name], out color))
                                                            { m.color = color; }
                                                        }
                                                        //else
                                                        //{
                                                        //    if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                                        //    { m.color = color; }

                                                        //}
                                                    }
                                                    //hBar.GetChild(1).GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (Material m in mats)
                                            {
                                                m.shader = Shader.Find("Standard");
                                                m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                                Color color;
                                                //if (ral_colors.Keys.Contains(rafafa_texture_name))
                                                //{
                                                //    if (ColorUtility.TryParseHtmlString(ral_colors[rafafa_texture_name], out color))
                                                //    { m.color = color; }
                                                //}
                                                ////else
                                                //{
                                                if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                                { m.color = color; }


                                                //}
                                            }

                                            if (space_btw_rafafa == -10)
                                            {
                                                mats = hBar.GetChild(1).GetComponentInChildren<Renderer>().materials;
                                                foreach (Material m in mats)
                                                {
                                                    m.shader = Shader.Find("Standard");
                                                    m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                                    Color color;

                                                    //if (ral_colors.Keys.Contains(rafafa_texture_name))
                                                    //{
                                                    //    if (ColorUtility.TryParseHtmlString(ral_colors[rafafa_texture_name], out color))
                                                    //    { m.color = color; }
                                                    //}
                                                    //else
                                                    //{
                                                    if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                                    { m.color = color; }

                                                    //}
                                                }
                                                //hBar.GetChild(1).GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;
                                            }
                                        }
                                    }
                                }
                                else if (acs_R_field.name.Contains("Accessories_"))
                                {
                                    foreach (Transform accessory in acs_R_field)
                                    {

                                        if (accessory.GetComponentInChildren<MeshRenderer>() != null)
                                        {   MeshRenderer mr= accessory.GetComponentInChildren<MeshRenderer>();

                               
                                            mr.material = Resources.Load("Materials/LAcessories_Material", typeof(Material)) as Material;
                                            mr.material.shader = Shader.Find("Standard");
                                            //mr.material.color = "#39FF14";
                                            Color color;
                                            if(ColorUtility.TryParseHtmlString("#39FF14", out color))
                                        { mr.material.color = color; }
                                        }

                                        //Hiding Top L_Accessory 
                                        //if (accessories.name.Contains("L_Accessory_top") || acs_R_field.name.Contains("ak - 109a"))
                                        //    accessories.gameObject.SetActive(false);
                                        //else
                                        //    accessories.gameObject.SetActive(true);
                                    }
                                }
                            }
                        }
                    }
                }

            if (FrameDividers_Parent != null)
                foreach (Transform frame_dividerBar in FrameDividers_Parent.transform)
                {
                    if (I_type)
                    {
                        //Adding Layer Mask
                        //field_dividerBar.transform.GetChild(0).GetChild(0)
                        if (frame_dividerBar.transform.GetChild(0) != null)
                            frame_dividerBar.transform.GetChild(0).gameObject.layer = divider_layer;

                        //if (frame_dividerBar.GetComponentInChildren<MeshRenderer>() != null)
                        //    frame_dividerBar.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/field_frame_Divider_Material", typeof(Material)) as Material;

                        var mats = frame_dividerBar.GetComponentInChildren<Renderer>().materials;


                        //foreach (Material m in mats)
                        //{
                        //    m.mainTexture = null;
                        //    m.shader = Shader.Find("Unlit/Color");
                        //    Color color;

                        //    if (ColorUtility.TryParseHtmlString("#A8A8A8", out color))
                        //    { m.color = color; }



                        //}

                        if (!string.IsNullOrEmpty(frame_texture_name))
                        {
                            if (textures.Keys.Contains(frame_texture_name))
                            {
                                foreach (Material m in mats)
                                {
                                    m.shader = Shader.Find("Standard");
                                    m.mainTexture = Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[frame_texture_name]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;
                                }
                            }
                            else if (ral_colors.Keys.Contains(frame_texture_name))
                            {
                                foreach (Material m in mats)
                                {
                                    m.shader = Shader.Find("Standard");
                                    m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                    Color color;

                                    if (ColorUtility.TryParseHtmlString(ral_colors[frame_texture_name], out color))
                                    { m.color = color; }




                                }

                            }
                        }
                        else
                        {
                            foreach (Material m in mats)
                            {
                                m.shader = Shader.Find("Standard");
                                m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                Color color;

                                if (ColorUtility.TryParseHtmlString("#8E8A8A", out color))
                                { m.color = color; }




                            }
                        }
                    }
                    else if (L_type)
                    {

                        Transform FrameDividers_Parent_Section = frame_dividerBar;

                        foreach (Transform frm_div in FrameDividers_Parent_Section.transform)
                        {

                            if (frm_div.GetChild(0) != null)
                            {
                                if (frm_div.GetChild(0).childCount > 1)
                                {
                                    var mats = frm_div.GetChild(0).GetComponentInChildren<Renderer>().materials;


                                    if (!string.IsNullOrEmpty(frame_texture_name))
                                    {
                                        if (textures.Keys.Contains(frame_texture_name))
                                        {
                                            foreach (Material m in mats)
                                            {
                                                m.shader = Shader.Find("Standard");
                                                m.mainTexture = Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[frame_texture_name]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;
                                            }
                                        }
                                        else if (ral_colors.Keys.Contains(frame_texture_name))
                                        {
                                            foreach (Material m in mats)
                                            {
                                                m.shader = Shader.Find("Standard");
                                                m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                                Color color;

                                                if (ColorUtility.TryParseHtmlString(ral_colors[frame_texture_name], out color))
                                                { m.color = color; }




                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (Material m in mats)
                                        {
                                            m.shader = Shader.Find("Standard");
                                            m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                            Color color;

                                            if (ColorUtility.TryParseHtmlString("#8E8A8A", out color))
                                            { m.color = color; }




                                        }
                                    }


                                    if (frm_div.GetChild(0).GetChild(0) != null)
                                        frm_div.GetChild(0).GetChild(0).gameObject.layer = divider_layer;
                                }
                            }
                        }



                    }

                }//FieldDividers_Parent


            if (FieldDividers_Parent != null)
                foreach (Transform field_dividerBar in FieldDividers_Parent.transform)
                {
                    if (I_type)
                    {
                        //Adding Layer Mask
                        //field_dividerBar.transform.GetChild(0).GetChild(0)
                        if (field_dividerBar.transform.GetChild(0) != null)
                            field_dividerBar.transform.GetChild(0).gameObject.layer = divider_layer;

                        //if (frame_dividerBar.GetComponentInChildren<MeshRenderer>() != null)
                        //    frame_dividerBar.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/field_frame_Divider_Material", typeof(Material)) as Material;

                        var mats = field_dividerBar.GetComponentInChildren<Renderer>().materials;


                        //foreach (Material m in mats)
                        //{
                        //    m.mainTexture = null;
                        //    m.shader = Shader.Find("Unlit/Color");
                        //    Color color;

                        //    if (ColorUtility.TryParseHtmlString("#A8A8A8", out color))
                        //    { m.color = color; }



                        //}

                        if (!string.IsNullOrEmpty(frame_texture_name))
                        {
                            if (textures.Keys.Contains(frame_texture_name))
                            {
                                foreach (Material m in mats)
                                {
                                    m.shader = Shader.Find("Standard");
                                    m.shader = Shader.Find("Standard");
                                    m.mainTexture = Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[frame_texture_name]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;
                                }
                            }
                            else if (ral_colors.Keys.Contains(frame_texture_name))
                            {
                                foreach (Material m in mats)
                                {
                                    m.shader = Shader.Find("Standard");
                                    m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                    Color color;

                                    if (ColorUtility.TryParseHtmlString(ral_colors[frame_texture_name], out color))
                                    { m.color = color; }




                                }

                            }
                        }
                        else
                        {
                            foreach (Material m in mats)
                            {
                                m.shader = Shader.Find("Standard");
                                m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                Color color;

                                if (ColorUtility.TryParseHtmlString("#8E8A8A", out color))
                                { m.color = color; }




                            }
                        }
                    }
                    else if (L_type)
                    {

                        Transform FieldDividers_Parent_Section = field_dividerBar;

                        foreach (Transform frm_div in FieldDividers_Parent_Section.transform)
                        {

                            if (frm_div.GetChild(0) != null)
                            {
                                if (frm_div.GetChild(0).childCount > 1)
                                {
                                    var mats = frm_div.GetChild(0).GetComponentInChildren<Renderer>().materials;


                                    if (!string.IsNullOrEmpty(frame_texture_name))
                                    {
                                        if (textures.Keys.Contains(frame_texture_name))
                                        {
                                            foreach (Material m in mats)
                                            {
                                                m.shader = Shader.Find("Standard");
                                                m.shader = Shader.Find("Standard");
                                                m.mainTexture = Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[frame_texture_name]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;
                                            }
                                        }
                                        else if (ral_colors.Keys.Contains(frame_texture_name))
                                        {
                                            foreach (Material m in mats)
                                            {
                                                m.shader = Shader.Find("Standard");
                                                m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                                Color color;

                                                if (ColorUtility.TryParseHtmlString(ral_colors[frame_texture_name], out color))
                                                { m.color = color; }




                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (Material m in mats)
                                        {
                                            m.shader = Shader.Find("Standard");
                                            m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                            Color color;

                                            if (ColorUtility.TryParseHtmlString("#8E8A8A", out color))
                                            { m.color = color; }




                                        }
                                    }


                                    if (frm_div.GetChild(0).GetChild(0) != null)
                                        frm_div.GetChild(0).GetChild(0).gameObject.layer = divider_layer;
                                }
                            }
                        }



                    }

                }
            //}
            //else if (ral_colors.Keys.Contains(name_))
            //{

            //}
        }
        public static void apply_material_for_pergola()
        {


            if (GameObject.Find("Frames_Parent") != null)
                Frames_Parent = GameObject.Find("Frames_Parent");

            if (GameObject.Find("Pergola_Model") != null)
                Pergola_Model = GameObject.Find("Pergola_Model");

            if (GameObject.Find("Divider_Parent") != null)
                Divider_Parent = GameObject.Find("Divider_Parent");


            if (GameObject.Find("FrameDividers_Parent") != null)
                FrameDividers_Parent = GameObject.Find("FrameDividers_Parent");

            if (GameObject.Find("FieldDividers_Parent") != null)
                FieldDividers_Parent = GameObject.Find("FieldDividers_Parent");

            if (GameObject.Find("Field_Parent") != null)
                Field_Parent = GameObject.Find("Field_Parent");

            if (GameObject.Find("SupportBars_Parent") != null)
                SupportBars_Parent = GameObject.Find("SupportBars_Parent");

            if (FrameDividers_Parent != null)
                foreach (Transform frame_dividerBar in FrameDividers_Parent.transform)
                {
                    //Adding Layer Mask
                    if (frame_dividerBar.transform.GetChild(0) != null)
                        frame_dividerBar.transform.GetChild(0).gameObject.layer = divider_layer;

                    var mats = frame_dividerBar.GetComponentInChildren<Renderer>().materials;


                    foreach (Material m in mats)
                    {
                        m.mainTexture = null;

                        Color color;

                        if (ColorUtility.TryParseHtmlString("#8EC63F", out color))
                        { m.color = color; }



                    }

                }

            if (Frames_Parent != null)
                foreach (Transform frame in Frames_Parent.transform)
                {
                    //adding Layer Mask
                    foreach (Transform frames in frame)
                        frames.gameObject.layer = frame_layer;

                    ////adding Layer Mask
                    //foreach (Transform frames in frame)
                    //{
                    //    if (frames.gameObject.GetComponent<Renderer>() != null)
                    //    {

                    //        var mats = frames.GetComponentInChildren<Renderer>().materials;

                    //        foreach (Material m in mats)
                    //        {
                    //            m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                    //            Color color;

                    //            if (ColorUtility.TryParseHtmlString("#A8A8A8", out color))
                    //            { m.color = color; }




                    //        }
                    //    }
                    //}
                }

            if (FieldDividers_Parent != null)
                foreach (Transform field_dividerBar in FieldDividers_Parent.transform)
                {
                    if (I_type)
                    {
                        //if (field_dividerBar.GetComponentInChildren<MeshRenderer>() != null)
                        //    field_dividerBar.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.67f, 0.62f, 0.62f);
                        var mats = field_dividerBar.GetComponentInChildren<Renderer>().materials;


                        foreach (Material m in mats)
                        {
                            m.mainTexture = null;

                            Color color;

                            if (ColorUtility.TryParseHtmlString("#8EC63F", out color))
                            { m.color = color; }



                        }


                        //Adding Layer Mask
                        if (field_dividerBar.transform.GetChild(0) != null)
                            field_dividerBar.transform.GetChild(0).gameObject.layer = divider_layer;
                    }
                    else if(L_type)
                    {

                        Transform FieldDividers_Parent_Section = field_dividerBar;
                            if (FieldDividers_Parent_Section.name.Contains("FieldDividers_Parent_Section1"))
                            {
                            foreach (Transform field_div in FieldDividers_Parent_Section.transform)
                            {
                                if (field_div.GetChild(0) != null)
                                    field_div.GetChild(0).gameObject.layer = divider_layer;
                            }


                            }
                    }
                }
            if (Field_Parent != null)
                foreach (Transform fieldGroup in Field_Parent.transform)
                {
                    for (int i = 0; i < fieldGroup.childCount; i++)
                    {
                        if (fieldGroup.GetChild(i).name.Contains("Accessories"))
                        {
                            foreach (Transform accessory in fieldGroup.GetChild(i).transform)
                            {
                                if (accessory.GetComponentInChildren<MeshRenderer>() != null)
                                    accessory.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/LAcessories_Material 1", typeof(Material)) as Material;
                            }
                        }
                        //else if (fieldGroup.GetChild(i).name.Contains("Fields"))
                        //{
                        //    foreach (Transform hBar in fieldGroup.GetChild(i).transform)
                        //    {
                        //        if (hBar.GetComponentInChildren<MeshRenderer>() != null)
                        //            hBar.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;

                        //        if (space_btw_rafafa == -10)
                        //            hBar.GetChild(1).GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;

                        //    }
                        //}
                    }
                }

            if (SupportBars_Parent != null)
                foreach (Transform supportBar_Parent in SupportBars_Parent.transform)
                {
                    for (int i = 0; i < supportBar_Parent.childCount; i++)
                    {
                        if (supportBar_Parent.GetChild(i).name.Contains("Clamp"))
                        {
                            if (supportBar_Parent.GetChild(i).GetChild(0).GetComponent<MeshRenderer>() != null)
                                supportBar_Parent.GetChild(i).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
                        }
                    }
                }
        }

        public static void apply_grayScale_material_for_pergola()
        {

            if (GameObject.Find("Frames_Parent") != null)
                Frames_Parent = GameObject.Find("Frames_Parent");

            if (GameObject.Find("Pergola_Model") != null)
                Pergola_Model = GameObject.Find("Pergola_Model");

            if (GameObject.Find("Divider_Parent") != null)
                Divider_Parent = GameObject.Find("Divider_Parent");


            if (GameObject.Find("FrameDividers_Parent") != null)
                FrameDividers_Parent = GameObject.Find("FrameDividers_Parent");

            if (GameObject.Find("FieldDividers_Parent") != null)
                FieldDividers_Parent = GameObject.Find("FieldDividers_Parent");

            if (GameObject.Find("Field_Parent") != null)
                Field_Parent = GameObject.Find("Field_Parent");

            if (GameObject.Find("SupportBars_Parent") != null)
                SupportBars_Parent = GameObject.Find("SupportBars_Parent");

            if (FrameDividers_Parent != null)
                foreach (Transform frame_dividerBar in FrameDividers_Parent.transform)
                {
                    if (I_type)
                    {
                        //frame_dividerBar.transform.GetChild(0).GetChild(0) 
                        //Adding Layer Mask
                        if (frame_dividerBar.transform.GetChild(0) != null)
                            frame_dividerBar.transform.GetChild(0).gameObject.layer = divider_layer;

                        //if (frame_dividerBar.GetComponentInChildren<MeshRenderer>() != null)
                        //    frame_dividerBar.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/field_frame_Divider_Material", typeof(Material)) as Material;

                        var mats = frame_dividerBar.GetComponentInChildren<Renderer>().materials;


                        foreach (Material m in mats)
                        {
                            m.mainTexture = null;
                            m.shader = Shader.Find("Unlit/Color");
                            Color color;

                            if (ColorUtility.TryParseHtmlString("#A8A8A8", out color))
                            { m.color = color; }



                        }
                    }
                    else if (L_type)
                    {

                        Transform FrameDividers_Parent_Section = frame_dividerBar;

                        foreach (Transform frm_div in FrameDividers_Parent_Section.transform)
                        {

                            if (frm_div.GetChild(0) != null)
                            {
                                //if (frm_div.GetChild(0).childCount>1)
                                //{
                                //frm_div.GetChild(0).GetChild(0).GetComponentInChildren<Renderer>()
                                var mats = frm_div.GetChild(0).GetComponent<Renderer>().materials;


                                    foreach (Material m in mats)
                                    {
                                        m.mainTexture = null;
                                        m.shader = Shader.Find("Unlit/Color");
                                        Color color;

                                        if (ColorUtility.TryParseHtmlString("#A8A8A8", out color))
                                        { m.color = color; }



                                    }

                                    if (frm_div.GetChild(0).GetChild(0) != null)
                                        frm_div.GetChild(0).GetChild(0).gameObject.layer = divider_layer;
                                //}
                            }
                        }


                        
                    }

                }

            //if(Frames_Parent!=null)
            //foreach (Transform frame in Frames_Parent.transform)
            //{
            //    //adding Layer Mask
            //    if (frame.name == "Frame_Parent")
            //        foreach (Transform frames in frame)
            //        {
            //            frames.gameObject.layer = frame_layer;

            //            frames.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/Frame_Material", typeof(Material)) as Material;
            //        }
            //}


            if (Frames_Parent != null)
                foreach (Transform frame in Frames_Parent.transform)
                {
                    //adding Layer Mask
                    foreach (Transform frames in frame)
                        frames.gameObject.layer = frame_layer;

                    //adding Layer Mask
                    foreach (Transform frames in frame)
                    {
                        if (frames.gameObject.GetComponent<Renderer>() != null)
                        {

                            var mats = frames.GetComponentInChildren<Renderer>().materials;

                            foreach (Material m in mats)
                            {
                                m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                m.shader = Shader.Find("Unlit/Color");
                                Color color;

                                if (ColorUtility.TryParseHtmlString("#A8A8A8", out color))
                                { m.color = color; }





                            }
                        }
                    }
                }

            if (FieldDividers_Parent != null)
                foreach (Transform field_dividerBar in FieldDividers_Parent.transform)
                {
                    if (I_type)
                    {
                        //if (field_dividerBar.GetComponentInChildren<MeshRenderer>() != null)
                        //    field_dividerBar.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/field_frame_Divider_Material", typeof(Material)) as Material;

                        //Adding Layer Mask
                        //field_dividerBar.transform.GetChild(0).GetChild(0)
                        if (field_dividerBar.transform.GetChild(0) != null)
                            field_dividerBar.transform.GetChild(0).gameObject.layer = divider_layer;

                        var mats = field_dividerBar.GetComponentInChildren<Renderer>().materials;

                        foreach (Material m in mats)
                        {
                            m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                            m.shader = Shader.Find("Unlit/Color");
                            Color color;

                            if (ColorUtility.TryParseHtmlString("#A8A8A8", out color))
                            { m.color = color; }





                        }
                    }
                    else if (L_type)
                    {

                        Transform FieldDividers_Parent_Section = field_dividerBar;



                        foreach (Transform field_div in FieldDividers_Parent_Section.transform)
                        {
                            if (field_div.GetChild(0) != null)
                            {
                                //if (field_div.GetChild(0).childCount>1)
                                //{
                                var mats = field_div.GetChild(0).GetComponent<Renderer>().materials;


                                foreach (Material m in mats)
                                {
                                    m.mainTexture = null;
                                    m.shader = Shader.Find("Unlit/Color");
                                    Color color;

                                    if (ColorUtility.TryParseHtmlString("#A8A8A8", out color))
                                    { m.color = color; }



                                }

                                if (field_div.GetChild(0).GetChild(0) != null)
                                    field_div.GetChild(0).GetChild(0).gameObject.layer = divider_layer;
                                //}
                            }
                        }


                     
                    }
                }

            if (Field_Parent != null)
                foreach (Transform fieldGroup in Field_Parent.transform)
                {
                    if (I_type)
                    {
                        if (fieldGroup.childCount > 0)
                        {

                            foreach (Transform accessory in fieldGroup.GetChild(0).transform)
                            {
                                if (accessory.GetComponentInChildren<MeshRenderer>() != null)
                                    accessory.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/LAcessories_Material", typeof(Material)) as Material;
                            }
                        }
                        if (fieldGroup.childCount > 1)
                        {

                            foreach (Transform hBar in fieldGroup.GetChild(1).transform)
                            {
                                //if (hBar.GetComponentInChildren<MeshRenderer>() != null)
                                //    hBar.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material", typeof(Material)) as Material;

                                //if (DB_script.space_btw_rafafa == -10)
                                //{
                                //    hBar.GetChild(1).GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material", typeof(Material)) as Material;

                                //    foreach (Transform ch in hBar)
                                //    {
                                //        if (ch.name.Contains("ak - 72"))
                                //            if (ch.GetComponentInChildren<MeshRenderer>() != null)
                                //                ch.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/Clip_Material", typeof(Material)) as Material;
                                //    }
                                //}
                                var mats = hBar.GetComponentInChildren<Renderer>().materials;
                                foreach (Material m in mats)
                                {
                                    m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                    m.shader = Shader.Find("Unlit/Color");

                                    Color color;

                                    if (ColorUtility.TryParseHtmlString("#69686A", out color))
                                    { m.color = color; }

                                    //else
                                    //{
                                    //    if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                    //    { m.color = color; }


                                    //}
                                }

                                if (space_btw_rafafa == -10)
                                {
                                    mats = hBar.GetChild(1).GetComponentInChildren<Renderer>().materials;
                                    foreach (Material m in mats)
                                    {
                                        m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                        m.shader = Shader.Find("Unlit/Color");

                                        Color color;


                                        if (ColorUtility.TryParseHtmlString("#69686A", out color))
                                        { m.color = color; }

                                        //else
                                        //{
                                        //    if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                        //    { m.color = color; }

                                        //}
                                    }
                                    //hBar.GetChild(1).GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;
                                }
                                //}
                            }
                        }
                    }
                    else if (L_type)
                    {


                      
                            //if (!sub_child.transform.name.Contains("Field_Parent_Section2"))
                            //{
                            //    sub_child.gameObject.SetActive(false);
                            //}
                            //else
                            //{
                              Transform fs=fieldGroup;
                                //Transform[] g = sub_child.GetComponentsInChildren<Transform>();
                                foreach (Transform f_g in fs)
                                {
                                foreach (Transform acs_R_field in f_g)
                                {

                                    if (acs_R_field.name.Contains("Fields_"))
                                    {
                                        foreach(Transform hBar in acs_R_field )
                                        {
                                            var mats = hBar.GetComponentInChildren<Renderer>().materials;
                                            foreach (Material m in mats)
                                            {
                                                m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                                m.shader = Shader.Find("Unlit/Color");

                                                Color color;

                                                if (ColorUtility.TryParseHtmlString("#69686A", out color))
                                                { m.color = color; }

                                                //else
                                                //{
                                                //    if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                                //    { m.color = color; }


                                                //}
                                            }

                                            if (space_btw_rafafa == -10)
                                            {
                                                mats = hBar.GetChild(1).GetComponentInChildren<Renderer>().materials;
                                                foreach (Material m in mats)
                                                {
                                                    m.mainTexture = null;// Resources.Load<Texture>($"Materials/{ral_colors[name_]}");//SetTexture($"{name_}",Resources.Load<Texture>($"Textures/Exalco_wood_color_book/{textures[name_]}")) ;

                                                    m.shader = Shader.Find("Unlit/Color");

                                                    Color color;


                                                    if (ColorUtility.TryParseHtmlString("#69686A", out color))
                                                    { m.color = color; }

                                                    //else
                                                    //{
                                                    //    if (ColorUtility.TryParseHtmlString("#FF9B75", out color))
                                                    //    { m.color = color; }

                                                    //}
                                                }
                                                //hBar.GetChild(1).GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/horizontal_Material 1", typeof(Material)) as Material;
                                            }
                                        }
                                    }
                                    else if(acs_R_field.name.Contains("Accessories_"))
                                    {
                                        foreach (Transform accessory in acs_R_field)
                                        {

                                            if (accessory.GetComponentInChildren<MeshRenderer>() != null)
                                                accessory.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/LAcessories_Material", typeof(Material)) as Material;
                                            //Hiding Top L_Accessory 
                                            //if (accessories.name.Contains("L_Accessory_top") || acs_R_field.name.Contains("ak - 109a"))
                                            //    accessories.gameObject.SetActive(false);
                                            //else
                                            //    accessories.gameObject.SetActive(true);
                                        }
                                    }
                                }
                                }
                            //}
                        
                    }
                }

            if (SupportBars_Parent != null)
                foreach (Transform supportBar_Parent in SupportBars_Parent.transform)
                {
                    if (supportBar_Parent.transform.childCount>0)
                    if (supportBar_Parent.GetChild(0).GetChild(0).GetComponent<MeshRenderer>() != null)
                        supportBar_Parent.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;

                    if (supportBar_Parent.childCount > 1)
                        if (supportBar_Parent.GetChild(1).GetChild(0).GetComponent<MeshRenderer>() != null)
                            supportBar_Parent.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
                }
        }
        public static void apply_material()
        {
            #region Applying materails to Game objects
            if (VerticalBar_Parent != null)
                foreach (Transform v_Bar in VerticalBar_Parent.transform)
                {
                    if (v_Bar.GetComponentInChildren<MeshRenderer>() != null)
                        v_Bar.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/Vertical_material", typeof(Material)) as Material;
                }

            if (HorizontalBar_Parent != null)
                foreach (Transform h_bar in HorizontalBar_Parent.transform)
                {
                    if (h_bar.GetComponentInChildren<MeshRenderer>() != null)
                        h_bar.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/Horizontal_material", typeof(Material)) as Material;
                }

            if (Accessory_Parent != null)
                foreach (Transform accs in Accessory_Parent.transform)
                {
                    if (accs.GetComponentInChildren<MeshRenderer>() != null)
                        accs.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/Accessory_material", typeof(Material)) as Material;
                }
            #endregion
        }
        public static async void place_clips(float space)
        {

            //ADD CLIPS
            string clip_name = "";

            float clip_height_full = 0;
            if (space == -10)
            {
                clip_name = "ak - 72";
                clip_height_full = 120.47f; //For space between 50, ak - 39
            }
            else if (space == 50)
            {
                clip_name = "ak - 39";
                clip_height_full = 120.47f; //For space between 50, ak - 39
            }
            else if (space == 20)
            {
                clip_name = "ak - 76";
                clip_height_full = 90.47f; // For space between 20, ak - 76
            }
            else
            {
                throw new Exception($"Invalid rafafa spacing. Valid spacing is -10, 20, 50.");
            }


            //KeyValuePair<string, float> kv = new KeyValuePair<string, float>();

            //kv = new KeyValuePair<string,float>(clip_name, clip_height_full);




        }

        public static void rotate_around_center(GameObject go, Vector3 three_axis_angle_rotation_abt_center)
        {
            //creating a dummy parent
            GameObject dummy_Parent;

            //saving actual parent
            GameObject actual_Parent = null;
            if (go.transform.parent != null)
            {
                actual_Parent = go.transform.parent.gameObject;
            }


            go.transform.parent = null;
            Bounds bound_go = Calculate_b(go.transform);


            dummy_Parent = new GameObject("dummy_Parent_" + go.name);

            //placing dummy parent at center of game object
            dummy_Parent.transform.position = bound_go.center;

            //making the dummy parent as parent oft go
            go.transform.parent = dummy_Parent.transform;

            //rotating around the center
            dummy_Parent.transform.Rotate(three_axis_angle_rotation_abt_center);

            //reparenting to actual parent
            if (actual_Parent != null)
            {
                go.transform.parent = actual_Parent.transform;
            }

            //destroy dummy parent
            DestroyImmediate(dummy_Parent);

        }
        public static async void camera_resizing_for_Pergola(bool from_DB_points = false)
        {
            if (GameObject.Find("Pergola_Model") != null)
            {
                #region orthographic_camera size,near_clip,far_clip region setting
                Pergola_Model = GameObject.Find("Pergola_Model");

                Bounds combined_bounds = new Bounds();
                //foreach (Transform sub_parent in Pergola_Model.transform)
                //{
                //    Bounds b = Calculate_b(sub_parent);

                //    combined_bounds.Encapsulate(b);
                //}
                combined_bounds = Calculate_b(Pergola_Model.transform);

                // foreach (Transform sub_parent in Pergola_Model.transform)
                // {
                //     //if the Model was generated for the first time then we place the HorizontalBar_Parent & VerticalBar_Parent at the combined center
                //     sub_parent.transform.position = new Vector3(-combined_bounds.center.x, -combined_bounds.center.y, -combined_bounds.center.z);//to place parent at the center 
                // }
                Pergola_Model.transform.position = new Vector3(-combined_bounds.center.x, -combined_bounds.center.y, -combined_bounds.center.z);//to place parent at the center 
                
                // Pergola_Model.transform.position=Pergola_Model.transform.TransformPoint(Pergola_Model.transform.GetComponentInChildren<MeshFilter>().mesh.bounds.center);
                //Attaching rotateonmouse script to new Game object
                if (Pergola_Model.gameObject.GetComponent<rotateonmouse>() == null)
                {
                    rotateonmouse RotateOnMouseScript = Pergola_Model.gameObject.AddComponent<rotateonmouse>();

                    ////setting target as Grand_parent
                    RotateOnMouseScript.target = Pergola_Model.transform;
                }

                if (Pergola_Model.gameObject.GetComponent<PanAndZoom>() == null)
                {
                    //Attaching pan and zoom script
                    PanAndZoom PanAndZoomScript = Pergola_Model.gameObject.AddComponent<PanAndZoom>();
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


                Camera.main.transform.GetComponent<Camera>().nearClipPlane = -max_axis * 100f;
                Camera.main.transform.GetComponent<Camera>().farClipPlane = max_axis * 100f;

                wall_Script_ = GameObject.Find("Directional Light").GetComponent<Wall_Script>();


                //foreach (string sides in wall_sides)
                //    wall_Script_.build_wall_on_sides(sides);
                foreach (string sides in wall_sides)
                    wall_Script_.build_wall_on_sides(sides, "Wall_Cube_Texture");

                if (Wall_Script.Wall_Parent != null)
                    foreach (Transform wall in Wall_Script.Wall_Parent.transform)
                    {
                        if (wall.gameObject.GetComponent<ProBuilderMesh>() != null)
                        {
                            if (!pb_list.Contains(wall.gameObject.GetComponent<ProBuilderMesh>()))
                                pb_list.Add(wall.gameObject.GetComponent<ProBuilderMesh>());
                        }
                    }


                #endregion

                Vector3 prev_rot = Pergola_Model.transform.eulerAngles;


                Pergola_Model.transform.rotation = Quaternion.Euler(0, -90, -90);

                #region Exporting region

                //string path = file_Dir;

                //Directory.CreateDirectory(path);//creating directory
                //string filePath_obj = System.IO.Path.Combine(path, "OBJ");//, unique_id);

                //Directory.CreateDirectory(filePath_obj);
                //filePath_obj = System.IO.Path.Combine(filePath_obj, unique_id);
                //string filePath_obj_zip = System.IO.Path.Combine(path, "OBJ.zip");
                //ExportObj.path = filePath_obj;//assigning path for exporting USING PROBUILDER

                ////settings for OBJ exporting from Pro builder

                //ObjOptions obOpt = new ObjOptions()
                //{

                //    handedness = ObjOptions.Handedness.Right,
                //    textureOffsetScale = false,
                //    copyTextures = true

                //};

                ////ExportObj.ExportWithFileDialog(pb_list, true, true, obOpt); //from probuilder

                ////To export as FBX file

                //FbxExportSettings fx = new FbxExportSettings()
                //{
                //    embedTextures = true,
                //    embedShaderProperty = true,

                //};

                //string filePath_fbx = System.IO.Path.Combine(path, "FBX");

                //Directory.CreateDirectory(filePath_fbx);

                //filePath_fbx = System.IO.Path.Combine(filePath_fbx, $"{unique_id}.fbx");
                //string filePath_fbx_zip = System.IO.Path.Combine(path, "FBX.zip");
                ////FbxExporter.Export(filePath_fbx + "\\" + unique_id + ".fbx", fx, Pergola_Model.transform);

                //************Here Gray material was applid***********************//
                apply_grayScale_material_for_pergola();
                //////UNCOMMENT FOR fbx

                ////FbxExporter.Export(filePath_fbx, fx, Pergola_Model.transform);


                ////ObjExporter.DoExport(true, Pergola_Model, file_Dir, unique_id + "_ObjExorter");//DoExport(bool makesubmeshes) function called from ObjExporter class 
                #endregion

                Pergola_Model.transform.eulerAngles = prev_rot;

                //if (System.Reflection.Assembly.GetExecutingAssembly().Location.StartsWith("C:\\Unity"))
                //{


                //foreach(Transform  wall in Wall_Script.Wall_Parent.transform)
                //{
                //    DestroyImmediate(wall.gameObject);
                //}

                foreach (string sides in wall_sides)
                    wall_Script_.build_wall_on_sides(sides);

                Reset_Script.GetACopyof_Go(Pergola_Model);
                print("region 1,2 3 count :" + DB_script.region1_fields_count + "," + DB_script.region2_fields_count + "," + DB_script.region3_fields_count);
                arrows_Measure_script.Take_Screen_Shot();
                //}

            }
        }

        public static void export_model()
        {
            try
            {
                Pergola_Model = GameObject.Find("Pergola_Model");
                GameObject Dire_Light = GameObject.Find("Directional Light");
                try
                {


                    //Here we apply textures to frame and fields
                   Dire_Light.GetComponent<DB_script>().apply_Textures_RAL(rafafa_color_texture, frame_color_texture);
                    ////Apply texture to other Parts
                    apply_material_for_pergola();
                    Debug.Log("applying materials");
                }
                catch (Exception ex_mat)
                {

                    Debug.Log("While Applying Materials :" + ex_mat);
                }


                foreach (string sides in wall_sides)
                    wall_Script_.build_wall_on_sides(sides, "Wall_Cube_Texture");

                #region Exporting region
                pb_list_updation();

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
                if (File.Exists($@"{Path.Combine(file_Dir, "OBJ-" + project_unique_id + "" + building_unique_id + "" + element_unique_id + ".zip")}"))
                {
                    File.Delete($@"{Path.Combine(file_Dir, "OBJ-" + project_unique_id + "" + building_unique_id + "" + element_unique_id + ".zip")}");
                }

                ZipFile.CreateFromDirectory(Path.Combine(file_Dir, "OBJ"), Path.Combine(file_Dir, "OBJ-" + project_unique_id + "" + building_unique_id + "" + element_unique_id + ".zip"));
               
                
                //To export as FBX file

                //    FbxExportSettings fx = new FbxExportSettings()
                //{
                //    embedTextures = true,
                //    embedShaderProperty = true,

                //};

                //string filePath_fbx = System.IO.Path.Combine(path, "FBX");

                //Directory.CreateDirectory(filePath_fbx);

                //filePath_fbx = System.IO.Path.Combine(filePath_fbx, $"{unique_id}.fbx");
                //string filePath_fbx_zip = System.IO.Path.Combine(path, "FBX.zip");
                //    //FbxExporter.Export(filePath_fbx + "\\" + unique_id + ".fbx", fx, Pergola_Model.transform);

                //    ////UNCOMMENT FOR fbx

                //    FbxExporter.Export(filePath_fbx, fx, Pergola_Model.transform);


                //ObjExporter.DoExport(true, Pergola_Model, file_Dir, unique_id + "_ObjExorter");//DoExport(bool makesubmeshes) function called from ObjExporter class 
                #endregion

            }
            catch (Exception ex)
            {

                print("While EXporting Model : "+ex);
            }
            finally
            {
                modelGenInProgress = false;
            }
        }

        public static void pb_list_updation()
        {
            if (pb_list == null)
            {
                pb_list = new List<ProBuilderMesh>();
            }
            else
            {
                pb_list.Clear();
            }

            Pergola_Model = GameObject.Find("Pergola_Model");

            //***************To create new List and get all the chidren ************************//
            bool create_new_list = true;
            List<GameObject> go_list= Arrows_Measure.get_All_children(Pergola_Model.transform,create_new_list);

            foreach(GameObject go in go_list)
            if (go.gameObject.GetComponent<ProBuilderMesh>() != null)
            {
                if (!pb_list.Contains(go.gameObject.GetComponent<ProBuilderMesh>()))
                    pb_list.Add(go.gameObject.GetComponent<ProBuilderMesh>());


            }
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

                Camera.main.transform.GetComponent<Camera>().nearClipPlane = -max_axis * 1.2f;

                Camera.main.transform.GetComponent<Camera>().farClipPlane = max_axis * 1.2f;

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

        private void Save_to_db_PergolaManagement(string model_type="pergola_model")
        {
            string db = "alukal";

            if (model_type== "pergola_model")
            {
                db = "alukal";
            }
            else
            {
                db = "Architect";
            }
            string query = "";//= "Insert Into dbo.louvers_points (unity_heirarchy_name,unity_heirarchy_level,pos_x,pos_y,pos_z,rot_x,rot_y,rot_z,scale_x,scale_y,scale_z,part_name_id) Values (@unity_heirarchy_name,@unity_heirarchy_level,@pos_x,@pos_y,@pos_z,@rot_x,@rot_y,@rot_z,@scale_x,@scale_y,@scale_z,@part_name_id)";
            //connectionString = $@"Data Source=212.29.201.154,3701\SQLEXPRESS;Initial Catalog={db};User ID=sa;Password=Password@1234";
            connectionString = $@"Data Source=localhost\SQLEXPRESS;Initial Catalog={db};User ID=sa;Password=Password@1234";
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
                //try
                //{
                //    //Deleting old entries from DB
                //    command.CommandText = $"DELETE FROM tbl_element_pergola_points WHERE project_unique_id = '{project_unique_id}' AND building_unique_id = '{building_unique_id}'  AND element_unique_id ='{element_unique_id}'";
                //    command.ExecuteNonQuery();
                //}
                //catch (Exception ex)
                //{
                //    Debug.Log("Deleting table entries:" + ex);
                //}
                if (pb_list == null)
                    pb_list = new List<ProBuilderMesh>();
                foreach (GameObject go in allObjects)
                {
                    if (!NotAllowed_in_db.Contains(go.name))
                    {
                        Characteristics characteristics_script = null ;
                        if (go.GetComponent<Characteristics>() == null)
                        {


                            characteristics_script = go.AddComponent<Characteristics>();
                            characteristics_script.part_name_id = go.name;
                            characteristics_script.part_unique_id = Guid.NewGuid().ToString();
                            characteristics_script.part_type = part_type_enum.none.ToString();
                        }
                        else
                        {
                            //if more than one chars script is attached we go through each and find if null value is not added
                            Characteristics[] chars = go.GetComponents<Characteristics>();
                            foreach (Characteristics ch in chars)
                            {


                                if (!string.IsNullOrEmpty(ch.part_type) )
                                {
                                    characteristics_script = ch;
                                }
                            }
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

                        if(characteristics_script.step_cut==true)
                        {
                            scale_y += characteristics_script.step_cut_width;
                        }

                        part_name_id = characteristics_script.part_name_id;
                        section_name = Characteristics.section_name;
                        part_unique_id = characteristics_script.part_unique_id;
                        part_type = characteristics_script.part_type;
                        part_group = characteristics_script.part_group;
                        left_end_cut_angle = characteristics_script.left_end_cut_angle;
                        right_end_cut_angle = characteristics_script.right_end_cut_angle;
                        icon_filename = characteristics_script.icon_filename;
                        instantiate = characteristics_script.instantiate;


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
                            query = $"Insert Into dbo.tbl_element_pergola_points (unity_heirarchy_name,unity_heirarchy_level,pos_x,pos_y,pos_z,rot_x,rot_y,rot_z,scale_x,scale_y,scale_z,part_name_id,project_unique_id,building_unique_id,element_unique_id,section_name,part_unique_id,part_type,part_group,left_end_cut_angle,right_end_cut_angle,icon_filename,instantiate) Values ('{unity_heirarchy_name}',{unity_heirarchy_level},{pos_x},{pos_y},{pos_z},{rot_x},{rot_y},{rot_z},{scale_x},{scale_y},{scale_z},'{part_name_id}','{project_unique_id}','{building_unique_id}','{element_unique_id}','{section_name}','{part_unique_id}', '{part_type}','{part_group} ','{left_end_cut_angle}','{right_end_cut_angle}','{icon_filename}','{instantiate}')";
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

        public static GameObject horizontal_bar;
        void Update()
        {
            //if (ak109a == null)
            //    ak109a = GameObject.Find("0");
            //if (ak109a != null)
            //    Debug.DrawRay(ak109a.transform.GetChild(0).TransformPoint(ak109a.transform.GetChild(0).transform.GetComponent<MeshFilter>().mesh.bounds.center), -ak109a.transform.up * 1000, Color.green);

        }

        bool forloopdone_1 = false;
        bool forloopdone_2 = false;

    }

}