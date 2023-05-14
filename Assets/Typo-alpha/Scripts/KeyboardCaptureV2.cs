using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class KeyboardCaptureV2 : MonoBehaviour
{
    public static KeyboardCaptureV2 self;
    private TouchScreenKeyboard keyboard;
    public BoxCollider2D Collider;
    public BoxCollider2D left, right;
    private Camera _camera;
    public UnityEvent<string> registeredKeys;
    private void Awake()
    {
        if (self != null) throw new UnityException("Too many keyboards");
        self = this;
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }
    private void Start()
    {
        // Collider = GetComponent<BoxCollider2D>();
        SetColliderSize();
        // Open the on-screen keyboard on mobile devices
#if UNITY_EDITOR || UNITY_STANDALONE
        keyboard = null;
#else
        TouchScreenKeyboard.hideInput = true;
        OpenKeyboard();
#endif
    }
    public void SetColliderSize()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = _camera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;
        Collider.size = new Vector2(cameraWidth, cameraHeight / 3);
        Collider.offset = new Vector2(0, -cameraHeight / 3);
        left.size = new Vector2(cameraWidth / 5, cameraHeight);
        left.offset = new Vector2(-cameraWidth / 2 - left.size.x / 2, 0);
        right.size = new Vector2(cameraWidth / 5, cameraHeight);
        right.offset = new Vector2(+cameraWidth / 2 + right.size.x / 2, 0);
    }
    private void OnGUI()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (TouchScreenKeyboard.visible)
        {
            Vector2 keyboardSize = TouchScreenKeyboard.area.size;
            Collider.size = new Vector2(Screen.width, keyboardSize.y);
            Collider.offset = new Vector2(0, keyboardSize.y / 2 - Screen.height / 2);
        }
#endif

    }
    public void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
        if (!keyboard.active) keyboard.active = true;



    }
    private void Update()
    {

        // Check if the keyboard is active on mobile devices
#if !UNITY_EDITOR && !UNITY_STANDALONE
        if (keyboard != null && keyboard.active)
        {
            // Check for mobile input
            if (keyboard.text.Length > 0)
            {
                // Handle the input
                registeredKeys.Invoke(keyboard.text);

                // Clear the keyboard
                keyboard.text = "";
            }
        }
        else{
            OpenKeyboard();
        }
#endif

        // Check for PC input
        if (Input.anyKeyDown)
        {
            string input = Input.inputString;

            if (!string.IsNullOrEmpty(input))
            {
                registeredKeys.Invoke(input);
            }
        }
    }
}
