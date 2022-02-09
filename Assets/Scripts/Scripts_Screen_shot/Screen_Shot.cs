using RESTfulHTTPServer.src.invoker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using static Arrows_Measure;

public class Screen_Shot : MonoBehaviour
{
    int resWidth = 2550;//it is not used in the code but the code is reset to desired value in (region screen) shot resolution part 
    int resHeight = 3300;
    Bounds targetBounds;

    int x_user_width = 1, y__user_width = 1200;

    Arrows_Measure arrows_Measure_script;

    Arrows_Measure_louvers arrows_Measure_Louvers_script;
    static int c = 0;

    DB_script dB_Script_obj;
    // Start is called before the first frame update
    void Start()
    {
        arrows_Measure_script = GameObject.Find("Directional Light").GetComponent<Arrows_Measure>();

        arrows_Measure_Louvers_script= GameObject.Find("Directional Light").GetComponent<Arrows_Measure_louvers>();

        dB_Script_obj = GameObject.Find("Directional Light").GetComponent<DB_script>();
    }
    //public enum Views { _TOP = 0, _FIELD = 1, _SIDE_FIELD = 2, _FRONT = 3 }
    public enum Views { _TOP, _FRONT, _FIELD, _SIDE_FIELD, _B_B, _SIDE_FIELD_SECTION_1, _SIDE_FIELD_SECTION_2, _SIDE_FIELD_SECTION_3, _FIELD_SECTION_1, _FIELD_SECTION_2, _FIELD_SECTION_3 }
 

    public static int sc_shot_count = 0;
    

    public IEnumerator Texture_Render(string view = "",bool perg_mod=true)
    {
        arrows_Measure_script = GameObject.Find("Directional Light").GetComponent<Arrows_Measure>();
        arrows_Measure_script.Refresh();
        yield return new WaitForSeconds(0.5f);
        //await Task.Delay(100);
        //yield return new WaitForSeconds(0.5f);
        try
        {
            sc_shot_count++;

            string model_name = "Pergola_Model";

            if (perg_mod == true)
                model_name = "Pergola_Model";
            else
                model_name = "Louver_Model";


            if (GameObject.Find(model_name) != null)
            {
                GameObject Parent = GameObject.Find(model_name);






                //*********** Encapsulating the Bound to Take total bounds of the model, considering every child of Pergola Model********************//

                //foreach (Transform tr in Parent.transform)
                //{
                //    Bounds b = DB_script.Calculate_b(tr);
                //    targetBounds.Encapsulate(b);
                //}

                targetBounds = DB_script.Calculate_b(Parent.transform);

                //Giving synchronous delay to the function
                //await Task.Run(new Action(Screenshot_completion_delay));

                #region view setting part
                float screenRatio = (float)Screen.width / (float)Screen.height;
                Vector3 model_y_dir = Parent.transform.TransformPoint(Vector3.up);
                Vector3 model_x_dir = Parent.transform.TransformPoint(Vector3.right);



                //float distance = Vector3.Distance(transform.position, Parent.transform.position);



                float targetRatio = targetBounds.size.x / targetBounds.size.y;


                if (screenRatio >= targetRatio)
                {

                    Camera.main.orthographicSize = targetBounds.size.y / 2;

                }
                else
                {



                    float frustumHeight = targetBounds.size.y / 2; //* differenceInSize;
                    Camera.main.orthographicSize = frustumHeight;
                    //float differenceInSize = targetRatio / screenRatio;
                    //Camera.main.orthographicSize = targetBounds.size.y / 2 * differenceInSize;

                }


                transform.position = new Vector3(targetBounds.center.x, targetBounds.center.y, -1f);
                #endregion

                #region screen shot resolution part
                Vector3 posStart = Camera.main.WorldToScreenPoint(new Vector3(targetBounds.min.x, targetBounds.min.y, targetBounds.min.z));
                Vector3 posEnd = Camera.main.WorldToScreenPoint(new Vector3(targetBounds.max.x, targetBounds.max.y, targetBounds.min.z));

                int widthX = System.Math.Abs((int)(posEnd.x - posStart.x));
                int widthY = System.Math.Abs((int)(posEnd.y - posStart.y));

                //To set user defined width for the Image

                x_user_width = widthX * y__user_width / widthY;

                widthX = x_user_width;
                widthY = y__user_width;

                resWidth = widthX;
                resHeight = widthY;

                print("Screen Size=" + resWidth + "X" + resHeight);
                #endregion


                #region setting maximum resolution supported 16384x16384

                while (resWidth > SystemInfo.maxTextureSize || resHeight > SystemInfo.maxTextureSize)
                {
                    resWidth = (int)(resWidth / 1.1f);//resWidth / 2f
                    resHeight = (int)(resHeight / 1.1f);//resHeight / 2f

                }

                #endregion

                //Destroying the arrow that shoukd not be rendered in Raffafa_Side View
                if (Arrows_Measure.Arrow_Side_Destroy_field_width != null)
                {
                    DestroyImmediate(Arrows_Measure.Arrow_Side_Destroy_field_width);
                }

                //float ratio = 300.0f / 96.0f;
                //resWidth =(int) (resWidth * ratio);
                //resHeight = (int)(resHeight * ratio);
                RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
                Camera.main.targetTexture = rt;
                Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);//ARGB32,RGB24//ARGB gives transparent background
                Camera.main.Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
                Camera.main.targetTexture = null;
                RenderTexture.active = null; // JC: added to avoid errors
                DestroyImmediate(rt);
                //to make it PNG
                //byte[] bytes = screenShot.EncodeToPNG();
                byte[] bytes = screenShot.EncodeToPNG();
                //byte[] bytes = screenShot.GetRawTextureData();

                //Here we set the DPI for Image 
                bytes = B83.Image.PNGTools.ChangePPI(bytes, 300, 300);

                DestroyImmediate(screenShot);
                //string filename = ScreenShotName(resWidth, resHeight, (control.unique_id));//+ shot_type));//added new string field that has unique name (control.unique_id+shot_type)          

                try
                {//saving screen shot

                    //string filePath = DataBase.file_Dir; //System.IO.Path.Combine(Application.streamingAssetsPath, "images");//D:\pergolawebapp\wwwroot//(Application.streamingAssetsPath, "images")

                    System.Random r = new System.Random();
                    int genRand = r.Next(10, 50);


                    bool no_LandU = false;

                    string filePath = DB_script.file_Dir;

                    string path = DB_script.file_Dir;

                    try
                    {
                        //if ((view == "_FIELD" || view == "_SIDE_FIELD") && no_LandU == true)

                        if ((view == Views._FIELD.ToString() || view == Views._SIDE_FIELD.ToString()) && DB_script.no_fields)
                        {

                            Debug.Log("No Field L and U Accessories to save screen shot : " + view);
                        }
                        else
                        {
                            Directory.CreateDirectory(path);//creating directory
                            filePath = System.IO.Path.Combine(path, DB_script.unique_id + view + ".png");
                            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                fs.Write(bytes, 0, bytes.Length);
                            }
                            Debug.Log("Saved Path:" + path);

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The process failed: {0}", e.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in process: {0}", ex);


                }
                finally
                {
                    //checking if its pergola model
                    if (perg_mod == true)
                    {

                        if (DB_script.I_type)
                        {
                            Reset_Script.Reset_Model();
                            if (sc_shot_count > 3)
                            {
                                sc_shot_count = 0;
                                DB_script.completed = true;
                                DB_script.export_model();
                            }
                            //else
                            //{
                                //Reset_Script.Reset_Model();
                            //}
                            if (sc_shot_count == 1)
                            {
                                if (DB_script.no_fields == false)
                                    arrows_Measure_script.arrs(Views._FIELD.ToString());
                                else
                                    sc_shot_count++;

                            }
                            if (sc_shot_count == 2)
                            {
                                if (DB_script.no_fields == false)
                                    arrows_Measure_script.arrs(Views._SIDE_FIELD.ToString());
                                else
                                    sc_shot_count++;
                            }
                            if (sc_shot_count == 3)
                            {
                                arrows_Measure_script.arrs(Views._FRONT.ToString());
                            }
                        }
                        else if(DB_script.L_type)
                        {
                            Reset_Script.Reset_Model();
                            if (sc_shot_count > 8)//8
                            {
                                sc_shot_count = 0;
                                DB_script.completed = true;
                                DB_script.export_model();
                            }
                            //else
                            //{
                                //Reset_Script.Reset_Model();
                            //}
                           if (sc_shot_count == 1)
                            {
                                if (DB_script.no_fields == false)
                                    arrows_Measure_script.arrs(Views._FIELD_SECTION_1.ToString());
                                else
                                    sc_shot_count++;

                            }
                            if (sc_shot_count == 2)
                            {
                                if (DB_script.no_fields == false)
                                    arrows_Measure_script.arrs(Views._SIDE_FIELD_SECTION_1.ToString());
                                else
                                    sc_shot_count++;
                            }
                            if (sc_shot_count == 3)
                            {
                                if (DB_script.no_fields == false)
                                    arrows_Measure_script.arrs(Views._FIELD_SECTION_2.ToString());
                                else
                                    sc_shot_count++;
                            }
                            if (sc_shot_count == 4)
                            {
                                if (DB_script.no_fields == false)
                                    arrows_Measure_script.arrs(Views._SIDE_FIELD_SECTION_2.ToString());
                                else
                                    sc_shot_count++;
                            }
                            if (sc_shot_count == 5)
                            {
                                if (DB_script.no_fields == false)
                                    arrows_Measure_script.arrs(Views._FIELD_SECTION_3.ToString());
                                else
                                    sc_shot_count++;
                            }
                            if (sc_shot_count == 6)
                            {
                                if (DB_script.no_fields == false)
                                    arrows_Measure_script.arrs(Views._SIDE_FIELD_SECTION_3.ToString());
                                else
                                    sc_shot_count++;
                            }
                            if (sc_shot_count == 7)
                            {
                            //    if (DB_script.no_fields == false)
                            //        arrows_Measure_script.arrs(Views._SIDE_FIELD_SECTION_1.ToString());
                            //    else
                            //        sc_shot_count++;
                            //}
                            //if (sc_shot_count == 6)
                            //{
                                //if (DB_script.no_fields == false)
                                    arrows_Measure_script.arrs(Views._FRONT.ToString());
                                //else
                                //    sc_shot_count++;
                            }
                            if (sc_shot_count == 8)
                            {
                                arrows_Measure_script.arrs(Views._B_B.ToString());
                            }
                         
                        }
                    }
                    else
                    {
                        if (sc_shot_count == 1)
                        {

                            arrows_Measure_Louvers_script.arrows_Louvers(Views._TOP.ToString());
                        }
                        else
                        {
                            sc_shot_count = 0;
                            DB_script.completed = true;
                            DB_script.modelGenInProgress = false;
                        }
                    }
                }

            }
        }

        catch (Exception ex)
        {
            Debug.Log("Screenshot Texture region:\n" + ex);
        }
    }
    public static void exp_compl_bool()
    {
        DB_script.completed = true;
        DB_script.export_model();
    }
}
