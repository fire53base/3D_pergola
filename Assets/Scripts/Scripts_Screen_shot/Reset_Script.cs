using RESTfulHTTPServer.src.invoker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_Script : MonoBehaviour
{
    public static GameObject Pergola_Model_duplicate;
    public static void GetACopyof_Go(GameObject model)
    {

        GameObject duplicate_model_parent;

        if (GameObject.Find("Controller") != null)
        {
            duplicate_model_parent = GameObject.Find("Controller");
        }
        else
        {
            duplicate_model_parent = new GameObject("Controller");
        }
        //checking if there is already a duplicate pergola model
        if (duplicate_model_parent.transform.Find("Pergola_Model_duplicate") != null)
        {
            DestroyImmediate(duplicate_model_parent.transform.Find("Pergola_Model_duplicate").gameObject);
        }
        Pergola_Model_duplicate = Instantiate(model, duplicate_model_parent.transform);



        Pergola_Model_duplicate.name = "Pergola_Model_duplicate";

        //Pergola_Model_duplicate.transform.parent = parent.transform.parent;

        Pergola_Model_duplicate.SetActive(false);

    }

    public static void Reset_Model()
    {
        string model_name = "Pergola_Model";

        if (GameObject.Find(model_name) != null)
        {
            //if (DB_script.I_type)
            //{
                if (GameObject.Find("Controller") != null && GameObject.Find("Controller").transform.Find("Pergola_Model_duplicate") != null)
                {
                    GameObject parent_of_replicate = GameObject.Find("Controller");
                    GameObject parent_replicated = parent_of_replicate.transform.Find("Pergola_Model_duplicate").gameObject;
                    GameObject parent_new_replicated = Instantiate(parent_replicated, parent_of_replicate.transform);
                    GameObject Pergola_Model = GameObject.Find(model_name);

                    //Destroying old_model
                    if (Pergola_Model != null)
                        DestroyImmediate(Pergola_Model);

                    //Assigning duplicate model its name
                    parent_new_replicated.name = "Pergola_Model_duplicate";//Parent_Duplicate

                    parent_replicated.name = model_name;

                    parent_replicated.transform.parent = null;


                    parent_replicated.SetActive(true);

                    parent_new_replicated.SetActive(false);



                }

            //}
            //else if (DB_script.L_type)
            //{
            //    if (GameObject.Find("Controller") != null && GameObject.Find("Controller").transform.Find("Pergola_Model_duplicate") != null)
            //    {
            //        GameObject parent_of_replicate = GameObject.Find("Controller");
            //        GameObject parent_replicated = parent_of_replicate.transform.Find("Pergola_Model_duplicate").gameObject;
            //        GameObject parent_new_replicated = Instantiate(parent_replicated, parent_of_replicate.transform);
            //        GameObject Pergola_Model = GameObject.Find(model_name);

            //        //Destroying old_model
            //        if (Pergola_Model != null)
            //            DestroyImmediate(Pergola_Model);

            //        //Assigning duplicate model its name
            //        parent_new_replicated.name = "Pergola_Model_duplicate";//Parent_Duplicate

            //        parent_replicated.name = model_name;

            //        parent_replicated.transform.parent = null;


            //        parent_replicated.SetActive(true);

            //        parent_new_replicated.SetActive(false);



            //    }


            //}
        }

    }
}
