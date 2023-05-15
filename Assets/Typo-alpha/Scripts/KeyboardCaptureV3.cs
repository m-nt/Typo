using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System;

public class KeyboardCaptureV3 : MonoBehaviour
{
    public static KeyboardCaptureV3 self;
    private TouchScreenKeyboard keyboard;
    public BoxCollider2D Collider;
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
        keyboard.text = " ";


    }
    private void Update()
    {

        // Check if the keyboard is active on mobile devices
#if !UNITY_EDITOR && !UNITY_STANDALONE
        if (keyboard != null && keyboard.active)
        {
            // Check for mobile input
            if (keyboard.text.Length > 1)
            {
                // byte[] bytes = Encoding.ASCII.GetBytes(input);
                // string hex = BitConverter.ToString(bytes).Replace("-", "");
                // registeredKeys.Invoke(hex == "08" ? "BACKSPACE" : input);
                // Handle the input
                bool is_delete = is_delete_key(keyboard.text);
                keyboard.text = keyboard.text.Replace(" ", "");
                Debug.LogError("ANDROID: " + keyboard.text);
                registeredKeys.Invoke(keyboard.text);

                // Clear the keyboard
                keyboard.text = " ";
            }else if(keyboard.text.Length < 1){
                Debug.LogError("BACKSPACE " + keyboard.text);
                registeredKeys.Invoke("BACKSPACE");
                keyboard.text = " ";
            }
        }
        else{
            OpenKeyboard();
        }
#endif

        // Check for BackSpace key
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.LogError("keyCode : " + KeyCode.Backspace);
        }

        // Check for PC input
        if (Input.anyKeyDown)
        {
            string input = Input.inputString;

            if (!string.IsNullOrEmpty(input))
            {

                byte[] bytes = Encoding.ASCII.GetBytes(input);
                string hex = BitConverter.ToString(bytes).Replace("-", "");
                Debug.LogError("UNITY: " + hex);
                bool is_delete_or_backspace = hex == "08" || is_delete_key(input);

                registeredKeys.Invoke(is_delete_or_backspace ? "BACKSPACE" : input);
            }
        }
    }

    bool is_delete_key(string inputKey)
    {
        foreach (char c in inputKey)
        {
            Debug.LogError("INDEVISUAL CHARS: " + c);
            if (c == '\b')
            {
                return true;
            }
        }
        return false;
    }
}
