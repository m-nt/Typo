using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public enum ShateStateV2
{
    PRIMARY,
    SECONDRAY
}

[RequireComponent(typeof(BoxCollider2D))]
public class KeyboardCaptureV2 : MonoBehaviour
{
    public static KeyboardCaptureV2 self;
    public float holdTime = 0.1f;
    public UnityEvent<KeyboardType> registeredKeys;
    public Color primary, secondary, scaled;
    public TMP_FontAsset font;
    KeyboardType holdedKey;
    BoxCollider2D Bound;
    private Camera _camera;
    bool touchStarted = false;
    float touchTime = 0f;

    // Event delegates and events
    public delegate void TouchEvent(Vector2 position);
    public static event TouchEvent OnTouchBegan;
    public static event TouchEvent OnTouchMoved;
    public static event TouchEvent OnTouchEnded;

    void Awake()
    {
        if (self != null) throw new UnityException("Too many keyboards");
        self = this;
        Bound = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
        UpdateCollider();
    }
    private void OnGUI()
    {
        UpdateCollider();
    }
    private void UpdateCollider()
    {
        if (_camera != null && Bound != null)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = _camera.orthographicSize * 2;
            float cameraWidth = cameraHeight * screenAspect;

            transform.localScale = new Vector2(cameraWidth / Bound.size.x, cameraWidth / Bound.size.x);
        }
    }
    void OnEnable()
    {
        OnTouchBegan += HandleTouchBegan;
        OnTouchMoved += HandleTouchMoved;
        OnTouchEnded += HandleTouchEnded;
    }

    void OnDisable()
    {
        OnTouchBegan -= HandleTouchBegan;
        OnTouchMoved -= HandleTouchMoved;
        OnTouchEnded -= HandleTouchEnded;
    }

    void HandleTouchBegan(Vector2 position)
    {
        // Your implementation here
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log("Touch began: " + hit.collider.name);
        }
    }

    void HandleTouchMoved(Vector2 position)
    {
        // Your implementation here
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log("Touch Moved: " + hit.collider.name);
        }
    }

    void HandleTouchEnded(Vector2 position)
    {
        // Your implementation here
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log("Touch Ended: " + hit.collider.name);
        }
    }
    private void Update()
    {
        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                OnTouchBegan?.Invoke(touchPosition);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                OnTouchMoved?.Invoke(touchPosition);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                OnTouchEnded?.Invoke(touchPosition);
            }
        }

        // Handle mouse input
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnTouchBegan?.Invoke(mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnTouchMoved?.Invoke(mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnTouchEnded?.Invoke(mousePosition);
        }
    }
    void xxxUpdate()
    {

        // Click begins on the keyboard
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
    || Input.GetMouseButtonDown(0))
        {
            // Get the click or touch position in screen coordinates
            Vector3 clickPos = Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;
            // Send a raycast from the click or touch position
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(clickPos);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            if (hit.collider != null)
            {
                touchStarted = true;
                touchTime = Time.time;
                hit.collider.TryGetComponent<KeyboardType>(out KeyboardType key);
                key.ChangeColor(ShateState.SECONDRAY);
                key.ActivatedDeactivateScaledKeys(true);
                if (holdedKey) holdedKey.ActivatedDeactivateScaledKeys(false);
                holdedKey = key;
            }
        }
        // Click ends on the keyboard
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended
    || Input.GetMouseButtonUp(0))
        {
            // Get the click or touch position in screen coordinates
            Vector3 clickPos = Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;
            // Send a raycast from the click or touch position
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(clickPos);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            if (hit.collider != null)
            {
                // A collider was hit
                hit.collider.TryGetComponent<KeyboardType>(out KeyboardType key);
                registeredKeys.Invoke(key);
                holdedKey.ActivatedDeactivateChildKeysFromChild(false);
                holdedKey.ActivatedDeactivateChildKeys(false);
                holdedKey.ChangeColor(ShateState.PRIMARY);
                holdedKey.ActivatedDeactivateScaledKeys(false);
                holdedKey = null;
                touchStarted = false;
                touchTime = 0;
            }
            else
            {
                holdedKey.ChangeColor(ShateState.PRIMARY);
                holdedKey.ActivatedDeactivateScaledKeys(false);
            }
        }
        // check if the touch was a hold for child objects
        if (touchStarted && Time.time - touchTime > holdTime)
        {
            if (!holdedKey) return;
            holdedKey.ActivatedDeactivateChildKeys(true);
            touchStarted = false;
        }
    }
}
