using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardCapture : MonoBehaviour
{
    public static KeyboardCapture self;
    public float holdTime = 0.1f;
    public UnityEvent<KeyboardType> registeredKeys;
    KeyboardType holdedKey;
    bool touchStarted = false;
    float touchTime = 0f;

    void Awake()
    {
        if (self != null) throw new UnityException("Too many keyboards");
        self = this;
    }

    void Update()
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
                holdedKey = null;
                touchStarted = false;
                touchTime = 0;
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
