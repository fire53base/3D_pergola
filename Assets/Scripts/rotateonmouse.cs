using UnityEngine;
using UnityEngine;
using System.Collections;
using RESTfulHTTPServer.src.invoker;

//[AddComponentMenu("Camera-Control/Mouse drag Orbit with zoom")]
public class rotateonmouse : MonoBehaviour
{
    public Transform target;
    public float distance = 0.0f; //default 5.0f
    public float xSpeed = 20.0f; //default 120.0f
    public float ySpeed = 20.0f; //default 120.0f

    public float yMinLimit = -360f; //default -20.0f
    public float yMaxLimit = 360f; //default 80.0f

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public float smoothTime = 8f;//default 2.0f

    public float rotationYAxis = 0.0f;
    public float rotationXAxis = 0.0f;

    float velocityX = 0.0f;
    float velocityY = 0.0f;

    public static UnityEngine.UI.Button TOP, BOTTOM, FRONT, SIDE, ISOMETRIC;

    //public bool isometric_180 = false;

    public float z_rotataion = 0f;

    public enum rafaffa_Placement_string { type_2, type_3 };
    public enum Views { _TOP, _FRONT, _FIELD, _SIDE_FIELD, _B_B, _SIDE_FIELD_SECTION_1, _SIDE_FIELD_SECTION_2, _SIDE_FIELD_SECTION_3, _FIELD_SECTION_1, _FIELD_SECTION_2, _FIELD_SECTION_3 }
    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationYAxis = angles.y;
        rotationXAxis = angles.x;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        if (GameObject.Find("TOP") != null && GameObject.Find("SIDE") != null && GameObject.Find("FRONT") != null && GameObject.Find("ISOMETRIC") != null)
        {

            TOP = GameObject.Find("TOP").GetComponent<UnityEngine.UI.Button>();

            BOTTOM = GameObject.Find("BOTTOM").GetComponent<UnityEngine.UI.Button>();
            SIDE = GameObject.Find("SIDE").GetComponent<UnityEngine.UI.Button>();
            FRONT = GameObject.Find("FRONT").GetComponent<UnityEngine.UI.Button>();
            ISOMETRIC = GameObject.Find("ISOMETRIC").GetComponent<UnityEngine.UI.Button>();

            //Adding listeners for onClick
            TOP.onClick.AddListener(rotate_TOP);
            BOTTOM.onClick.AddListener(rotate_BOTTOM);

            SIDE.onClick.AddListener(rotate_SIDE);
            FRONT.onClick.AddListener(rotate_FRONT);
            ISOMETRIC.onClick.AddListener(rotate_ISOMETRIC);
        }
    }

    void LateUpdate()
    {
        if (target)
        {
            if (Input.GetMouseButton(1))
            {
                velocityX += xSpeed * Input.GetAxis("Mouse X") * 0.02f;
                velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;

                rotationYAxis += velocityX;
                rotationXAxis -= velocityY;

                rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);

                //*************new Code added**********************//
                //float z_rotataion = 0f;
                //if(isometric_180==true)
                //{
                //    z_rotataion = 180f;
                //    //isometric_180 = false;
                //}
                //else
                //{
                //    z_rotataion = 0f;
                //}
                ////**********************************************///
                Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, z_rotataion);
                Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, z_rotataion);
                Quaternion rotation = toRotation;


                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;

                velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
                velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
            }
        }

    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);

    }

    public void rotate_TOP()
    {
        z_rotataion = 0;
        transform.rotation = Quaternion.Euler(0, 90, -90);
        rotationXAxis = 90;
        rotationYAxis = -90;

        Fit_TO_Screen();
    }

    public void rotate_Raffafa_TOP()
    {
       

        Fit_TO_Screen();
    }

    public void rotate_BOTTOM()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rotationXAxis = 0;
        rotationYAxis = 0;

        Fit_TO_Screen();
    }
    public void rotate_SIDE()
    {
        z_rotataion = -90;
        transform.rotation = Quaternion.Euler(0, 0, -90);
        rotationXAxis = 0;
        rotationYAxis = 0;

        Fit_TO_Screen();

    }

    public void rotate_Raffafa_SIDE()
    {
        z_rotataion = 90;
        transform.rotation = Quaternion.Euler(0, 0, 90);
        rotationXAxis = 0;
        rotationYAxis = 0;

        Fit_TO_Screen();

    }

    public void rotate_FRONT()
    {
        z_rotataion = 0;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        rotationXAxis = 0;
        rotationYAxis = 90;


        Fit_TO_Screen();
    }

    public void rotate_FRONT_B_B()
    {
        z_rotataion = -90;
        transform.rotation = Quaternion.Euler(0, 180, -90);
        rotationXAxis = 0;
        rotationYAxis = 180;


        Fit_TO_Screen();
    }

    public void rotate_ISOMETRIC()
    {
        z_rotataion = -45;
        transform.rotation = Quaternion.Euler(0, 135, -45);
        rotationXAxis = 0;
        rotationYAxis = 135;

        Fit_TO_Screen();

    }

    public void rotate_PergolaModel(string view)
    {
        bool type_2 = DB_script.rafafa_placement_type == rafaffa_Placement_string.type_2.ToString();

        if (view == Views._SIDE_FIELD_SECTION_2.ToString())
        {

            if (type_2)
            {
                z_rotataion = -90;
                transform.rotation = Quaternion.Euler(0, 0, -90);
                rotationXAxis = 0;
                rotationYAxis = 0;
            }
            else
            {
                z_rotataion = -90;
                transform.rotation = Quaternion.Euler(0, 0, -90);
                rotationXAxis = 0;
                rotationYAxis = 0;
            }
        }
        else if (view == Views._FIELD_SECTION_3.ToString())
        {
            if (type_2)
            {
                z_rotataion = -90;
                transform.rotation = Quaternion.Euler(-90, 90, -90);
                rotationXAxis = -90;
                rotationYAxis = -90;
            }
            else
            {
                z_rotataion = 90;
                transform.rotation = Quaternion.Euler(-90, -90, 90);
                rotationXAxis = -90;
                rotationYAxis = -90;
            }

        }
        else if (view == Views._FIELD_SECTION_2.ToString())
        {
            if (type_2)
            {
                //z_rotataion = 90;
                //transform.rotation = Quaternion.Euler(0, 90, 90);
                //rotationXAxis = 0;
                //rotationYAxis = 90;
                z_rotataion = -90;
                transform.rotation = Quaternion.Euler(0, 90, -90);
                rotationXAxis = 0;
                rotationYAxis = 90;
            }
            else
            {
                z_rotataion = 270;
                transform.rotation = Quaternion.Euler(0, 90, 270);
                rotationXAxis = 0;
                rotationYAxis = 90;
            }

        }
        else if (view == Views._SIDE_FIELD_SECTION_3.ToString())
        {
            //if (DB_script.rafafa_placement_type == rafaffa_Placement_string.type_2.ToString())
            //{
                z_rotataion = -90;
                transform.rotation = Quaternion.Euler(-90, 0, -90);
                rotationXAxis = -90;
                rotationYAxis = 0;
            //}
            //else
            //{
            //    z_rotataion = 0;
            //    transform.rotation = Quaternion.Euler( 90, 90, 0);
            //    rotationXAxis = 90;
            //    rotationYAxis = 90;
            //}
        }
        
      
            Fit_TO_Screen();
    }

    public void rotate_FRONT_Louvers()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rotationXAxis = 0;
        rotationYAxis = 0;

        Fit_TO_Screen();
    }

    public void rotate_TOP_Louvers()
    {
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        rotationXAxis = -90;
        rotationYAxis = 0;

        Fit_TO_Screen();
    }

    public void Fit_TO_Screen()
    {
        try
        {

     
        GameObject Pergola_Model;
        if (GameObject.Find("Pergola_Model") != null)
        {
            Pergola_Model = GameObject.Find("Pergola_Model");

            Bounds targetBounds = Calculate_b(Pergola_Model.transform);


            float screenRatio = (float)Screen.width / (float)Screen.height;

            float targetRatio = targetBounds.size.x / targetBounds.size.y;


            if (screenRatio >= targetRatio)
            {
                //adding ( targetBounds.size.y/15) x100 % of frustum height
                Camera.main.orthographicSize = (targetBounds.size.y + targetBounds.size.y * 25 / 100) / 2;
                //else if(targetBounds.size.x > targetBounds.size.y)
                //    Camera.main.main.orthographicSize = targetBounds.size.x / 2;
                //transform.position = new Vector3(targetBounds.center.x, targetBounds.center.y, -1f);
            }
            else
            {
                //float differenceInSize = targetRatio / screenRatio;

                //adding ( targetBounds.size.y/15) x100% of frustum height
                float frustumHeight = (targetBounds.size.y + targetBounds.size.y * 25 / 100) / 2; //* differenceInSize;
                Camera.main.orthographicSize = frustumHeight;
                // distance = frustumHeight * 0.5f / Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
                //transform.position = new Vector3(targetBounds.center.x, targetBounds.center.y, distance);
                //else if (targetBounds.size.x > targetBounds.size.y)
                //    Camera.main.main.orthographicSize = targetBounds.size.x / 2*(1/ differenceInSize);
                //Camera.main.main.orthographicSize = targetBounds.size.y / 2 * differenceInSize;
            }
            Camera.main.transform.position = new Vector3(targetBounds.center.x, targetBounds.center.y, -1f);
        }

        }
        catch (System.Exception ex)
        {

            Debug.Log("Fiiting Model to screen Rotate On Mouse Script: " + ex);
        }
    }
    public static Bounds Calculate_b(Transform TheObject)//To calculate bound of all the bars 
    {
        var renderers = TheObject.GetComponentsInChildren<MeshRenderer>();
        Bounds combinedBounds = new Bounds();
        if (renderers.Length > 0)
        {
            combinedBounds = renderers[0].bounds;
            for (int i = 0; i < renderers.Length; i++)
            {
                combinedBounds.Encapsulate(renderers[i].bounds);
            }
        }
        return combinedBounds;
    }
}
