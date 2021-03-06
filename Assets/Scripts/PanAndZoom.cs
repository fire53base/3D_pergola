using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//https://github.com/GibsS/unity-pan-and-zoom

/// <summary> A modular and easily customisable Unity MonoBehaviour for handling swipe and pinch motions on mobile. </summary>
public class PanAndZoom : MonoBehaviour
{
    /// <summary> Called as soon as the player touches the screen. The argument is the screen position. </summary>
    public event Action<Vector2> onStartTouch;
    /// <summary> Called as soon as the player stops touching the screen. The argument is the screen position. </summary>
    public event Action<Vector2> onEndTouch;
    /// <summary> Called if the player completed a quick tap motion. The argument is the screen position. </summary>
    public event Action<Vector2> onTap;
    /// <summary> Called if the player swiped the screen. The argument is the screen movement delta. </summary>
    public event Action<Vector2> onSwipe;
    /// <summary> Called if the player pinched the screen. The arguments are the distance between the fingers before and after. </summary>
    public event Action<float, float> onPinch;

    [Header("Tap")]
    [Tooltip("The maximum movement for a touch motion to be treated as a tap")]
    public float maxDistanceForTap = 40;
    [Tooltip("The maximum duration for a touch motion to be treated as a tap")]
    public float maxDurationForTap = 0.4f;

    [Header("Desktop debug")]
    [Tooltip("Use the mouse on desktop?")]
    public bool useMouse = true;
    [Tooltip("The simulated pinch speed using the scroll wheel")]
    public float mouseScrollSpeed = 2;

    [Header("Camera control")]
    [Tooltip("Does the script control camera movement?")]
    public bool controlCamera = true;
    [Tooltip("The controlled camera, ignored of controlCamera=false")]
    public Camera cam;

    [Header("UI")]
    [Tooltip("Are touch motions listened to if they are over UI elements?")]
    public bool ignoreUI = false;

    [Header("Bounds")]
    [Tooltip("Is the camera bound to an area?")]
    public bool useBounds;

    public float boundMinX = -150;
    public float boundMaxX = 150;
    public float boundMinY = -150;
    public float boundMaxY = 150;

    Vector2 touch0StartPosition;
    Vector2 touch0LastPosition;
    float touch0StartTime;

    bool cameraControlEnabled = true;

    bool canUseMouse;

    /// <summary> Has the player at least one finger on the screen? </summary>
    public bool isTouching { get; private set; }

    /// <summary> The point of contact if it exists in Screen space. </summary>
    public Vector2 touchPosition { get { return touch0LastPosition; } }

    void Start()
    {
        canUseMouse = Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer && Input.mousePresent;
    }

    void Update()
    {

        if (useMouse && canUseMouse)
        {
            UpdateWithMouse();
        }
        else
        {
            UpdateWithTouch();
        }
    }

    void LateUpdate()
    {
        CameraInBounds();
    }

    void UpdateWithMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ignoreUI || !IsPointerOverUIObject())
            {
                touch0StartPosition = Input.mousePosition;
                touch0StartTime = Time.time;
                touch0LastPosition = touch0StartPosition;

                isTouching = true;

                if (onStartTouch != null) onStartTouch(Input.mousePosition);
            }
        }

        if (Input.GetMouseButton(0) && isTouching)// &&!SupportBar_Movement.isMouseDrag)
        {
            Vector2 move = (Vector2)Input.mousePosition - touch0LastPosition;
            touch0LastPosition = Input.mousePosition;

            if (move != Vector2.zero)
            {
                OnSwipe(move);
            }
        }

        if (Input.GetMouseButtonUp(0) && isTouching)
        {
            if (Time.time - touch0StartTime <= maxDurationForTap
               && Vector2.Distance(Input.mousePosition, touch0StartPosition) <= maxDistanceForTap)
            {
                OnClick(Input.mousePosition);
            }

            if (onEndTouch != null) onEndTouch(Input.mousePosition);
            isTouching = false;
            cameraControlEnabled = true;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            OnPinch(1, Input.mouseScrollDelta.y < 0 ? (1 / mouseScrollSpeed) : mouseScrollSpeed, Vector2.right);
        }
    }

    void UpdateWithTouch()
    {
        int touchCount = Input.touches.Length;

        if (touchCount == 1)
        {
            Touch touch = Input.touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    {
                        if (ignoreUI || !IsPointerOverUIObject())
                        {
                            touch0StartPosition = touch.position;
                            touch0StartTime = Time.time;
                            touch0LastPosition = touch0StartPosition;

                            isTouching = true;

                            if (onStartTouch != null) onStartTouch(touch0StartPosition);
                        }

                        break;
                    }
                case TouchPhase.Moved:
                    {
                        touch0LastPosition = touch.position;

                        if (touch.deltaPosition != Vector2.zero && isTouching)
                        {
                            OnSwipe(touch.deltaPosition);
                        }
                        break;
                    }
                case TouchPhase.Ended:
                    {
                        if (Time.time - touch0StartTime <= maxDurationForTap
                            && Vector2.Distance(touch.position, touch0StartPosition) <= maxDistanceForTap
                            && isTouching)
                        {
                            OnClick(touch.position);
                        }

                        if (onEndTouch != null) onEndTouch(touch.position);
                        isTouching = false;
                        cameraControlEnabled = true;
                        break;
                    }
                case TouchPhase.Stationary:
                case TouchPhase.Canceled:
                    break;
            }
        }
        else if (touchCount == 2)
        {
            Touch touch0 = Input.touches[0];
            Touch touch1 = Input.touches[1];

            if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended) return;

            isTouching = true;

            float previousDistance = Vector2.Distance(touch0.position - touch0.deltaPosition, touch1.position - touch1.deltaPosition);

            float currentDistance = Vector2.Distance(touch0.position, touch1.position);

            if (previousDistance != currentDistance)
            {
                OnPinch(previousDistance, currentDistance, (touch1.position - touch0.position).normalized);
            }
        }
        else
        {
            if (isTouching)
            {
                if (onEndTouch != null) onEndTouch(touch0LastPosition);
                isTouching = false;
            }

            cameraControlEnabled = true;
        }
    }

    void OnClick(Vector2 position)
    {
        if (onTap != null && (ignoreUI || !IsPointerOverUIObject()))
        {
            onTap(position);
        }
    }
    void OnSwipe(Vector2 deltaPosition)
    {
        if (onSwipe != null)
        {
            onSwipe(deltaPosition);
        }

        if (controlCamera && cameraControlEnabled)
        {
            if (cam == null) cam = Camera.main;

            cam.transform.position -= (cam.ScreenToWorldPoint(deltaPosition) - cam.ScreenToWorldPoint(Vector2.zero));
        }
    }
    void OnPinch(float oldDistance, float newDistance, Vector2 touchDelta)
    {
        if (onPinch != null)
        {
            onPinch(oldDistance, newDistance);
        }

        if (controlCamera && cameraControlEnabled)
        {
            if (cam == null) cam = Camera.main;

            if (cam.orthographic)
            {
                cam.orthographicSize = Mathf.Max(0.001f, cam.orthographicSize * oldDistance / newDistance);
            }
            else
            {
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView * oldDistance / newDistance, 0.1f, 179.9f);
            }
        }
    }

    /// <summary> Checks if the the current input is over canvas UI </summary>
    public bool IsPointerOverUIObject()
    {
        if (EventSystem.current == null) return false;
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    /// <summary> Cancels camera movement for the current motion. Resets to use camera at the end of the touch motion.</summary>
    public void CancelCamera()
    {
        cameraControlEnabled = false;
    }

    void CameraInBounds()
    {
        if (controlCamera && useBounds && cam != null && cam.orthographic)
        {

            if (transform.GetComponentsInChildren<MeshFilter>().Length > 0)
            {
                Bounds targetBounds = Calculate_b(transform);

                boundMaxX = Mathf.Max(targetBounds.max.y, targetBounds.max.x) * 109;
                boundMaxY = Mathf.Max(targetBounds.max.y, targetBounds.max.x) * 100;

                boundMinX = -Mathf.Max(targetBounds.max.y, targetBounds.max.x) * 100;
                boundMinY = -Mathf.Max(targetBounds.max.y, targetBounds.max.x) * 100;
            }


            cam.orthographicSize = Mathf.Min(cam.orthographicSize, ((boundMaxY - boundMinY) / 2) - 0.001f);
            cam.orthographicSize = Mathf.Min(cam.orthographicSize, (Screen.height * (boundMaxX - boundMinX) / (2 * Screen.width)) - 0.001f);

        }
        //if (cam.orthographicSize > max_cam_size || lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)
        //{
            //max_cam_size = cam.orthographicSize;
            //lastScreenWidth = Screen.width;
            //lastScreenHeight = Screen.height;

            //previous_cameraPosition = cam.transform.position;
            //previous_size = cam.orthographicSize;

            //Camera.main.transform.position = new Vector3(0, 0, -1); //frustrum planes need to update from center of screen to edges is screen
            //cam.orthographicSize = 1000000;//10000

            //cam.orthographicSize = Mathf.Min(cam.orthographicSize, ((boundMaxY - boundMinY) / 2) - 0.001f);
            //cam.orthographicSize = Mathf.Min(cam.orthographicSize, (Screen.height * (boundMaxX - boundMinX) / (2 * Screen.width)) - 0.001f);
            //GetComponent<cam_planes>().update_cam_planes_pos();

        //    cam.orthographicSize = previous_size;
        //    cam.transform.position = previous_cameraPosition;
        //    Debug.Log("Updated Frustrum for new max camera size");
        //}
    }

    public static Bounds Calculate_b(Transform TheObject)//To calculate bound of all the bars 
    {
        var renderers = TheObject.GetComponentsInChildren<MeshFilter>();
        Bounds combinedBounds = renderers[0].mesh.bounds;
        for (int i = 0; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].mesh.bounds);
        }
        return combinedBounds;
    }
}
