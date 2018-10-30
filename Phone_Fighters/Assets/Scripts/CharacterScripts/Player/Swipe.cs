using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;

public class Swipe : MonoBehaviour {
    
    bool fingerOnScreen = false;
    Vector2 startTouch, swipeDelta;
    [SerializeField]
    Transform startMarker, endMarker = null;
    [SerializeField]
    int minSwipe = 50;
    float x, y, time = 0;
    InputManager player;
    bool canSwipe = true;
    public bool CanSwipe { get { return canSwipe; } set { canSwipe = value; } }

    public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
    public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input
    CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

    public Vector2 SwipeDelta { get { return swipeDelta; } }

    void Start()
    {
        Reset();
        if (GameManager.Instance.IsPvp)
        {
            player = NetworkManager.Instance.MyPlayer.GetComponent<InputManager>();
        }
        else
        {
          player = Player.Instance.GetComponentInParent<InputManager>();
        }
    }

    void Update()
    {
        if (canSwipe)
        {
            if (Input.touches.Length > 0) // User is pressing (Mobile)
            {
                MobileControls();
            }
            else                          // User is pressing (PC)
            {
                PcControls();
            }
        }
    }

    void OnEnable()
    {
        CreateVirtualAxes();
    }

    void Reset()
    {
        UpdateVirtualAxes(startTouch);
        fingerOnScreen = false;
        startTouch = swipeDelta = Vector2.zero;
        startMarker.gameObject.SetActive(false);
        endMarker.gameObject.SetActive(false);
        time = 0;
    }

    void MobileControls()
    {
        swipeDelta = Vector2.zero;
        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            if (Input.touches[0].phase == TouchPhase.Began)            // User starts pressing
            {
                startMarker.gameObject.SetActive(true);
                endMarker.gameObject.SetActive(true);
                startMarker.position = endMarker.position = Input.touches[0].position;
                fingerOnScreen = true;
                startTouch = Input.touches[0].position;
                time += Time.deltaTime;
            }
        }

        if (Input.touches[0].phase == TouchPhase.Ended && fingerOnScreen ||
            Input.touches[0].phase == TouchPhase.Canceled && fingerOnScreen)             // User releases finger
        {
            if (time <= player.PressTimeToDash)
            {
                player.Dash();
            }

            Reset();
        }

        if (fingerOnScreen)                                            // User is continuing to press (moving or idle)
        {
            if (Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            swipeDelta += startTouch;
            UpdateVirtualAxes(swipeDelta);
            endMarker.position = Input.touches[0].position;
            time += Time.deltaTime;
        }
    }

    void PcControls()
    {
        swipeDelta = Vector2.zero;
        if (!EventSystem.current.IsPointerOverGameObject()) // pointer is not over GUI
        {
            if (Input.GetMouseButtonDown(0))                            // User starts pressing
            {
                startMarker.gameObject.SetActive(true);
                endMarker.gameObject.SetActive(true);
                startMarker.position = endMarker.position = Input.mousePosition;
                fingerOnScreen = true;
                startTouch = Input.mousePosition;
                time += Time.deltaTime;
            }            
        }
        else { }                                              // pointer is not over GUI

        if (Input.GetMouseButtonUp(0) && fingerOnScreen)            // User releases finger
        {
            if (time <= player.PressTimeToDash)
            {
                player.Dash();
            }

            Reset();
        }

        if (fingerOnScreen)                                         // User is continuing to press (moving or idle)
        {
            if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
            swipeDelta += startTouch;
            UpdateVirtualAxes(swipeDelta);
            endMarker.position = Input.mousePosition;
            time += Time.deltaTime;
        }
    }

    void CreateVirtualAxes()
    {
        // create new axes
        m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
        m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
    }

    void UpdateVirtualAxes(Vector2 value)
    {
        var delta = startTouch - value;
        delta.y = -delta.y;
        delta /= minSwipe;
        m_HorizontalVirtualAxis.Update(-delta.x);
        m_VerticalVirtualAxis.Update(delta.y);
    }

    void OnDisable()
    {
        // remove the joystick from the cross platform input
        m_HorizontalVirtualAxis.Remove();
        m_VerticalVirtualAxis.Remove();
    }
}