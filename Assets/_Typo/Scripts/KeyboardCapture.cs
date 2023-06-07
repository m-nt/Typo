using UnityEngine;
using UnityEngine.Events;
using System.Text;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class KeyboardCapture : MonoBehaviour
{
    public static KeyboardCapture self;
    private TouchScreenKeyboard keyboard;
    private BoxCollider2D Collider;
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
        if (!Collider) Collider = GetComponent<BoxCollider2D>();
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
                keyboard.text = keyboard.text.Replace(" ", "");
                registeredKeys.Invoke(keyboard.text);
                CharPerSecond.TotalCount++;
                // Clear the keyboard
                keyboard.text = " ";
            }
            else if (keyboard.text.Length < 1)
            {
                registeredKeys.Invoke("BACKSPACE");
                keyboard.text = " ";
            }
        }
        else
        {
            OpenKeyboard();
        }
#endif


        // Check for PC input
        if (Input.anyKeyDown)
        {
            string input = Input.inputString;

            if (!string.IsNullOrEmpty(input))
            {

                // Convert the input string to Hexadecimal representation
                byte[] bytes = Encoding.ASCII.GetBytes(input);
                string hex = BitConverter.ToString(bytes).Replace("-", "");
                // Check if the input hex is the Backspace or delete key
                bool is_delete_or_backspace = hex == "08" || Is_delete_key(input);
                // if (!is_delete_or_backspace) CharPerSecond.TotalCount++;
                registeredKeys.Invoke(is_delete_or_backspace ? "BACKSPACE" : input);
            }
        }
    }

    bool Is_delete_key(string inputKey)
    {
        foreach (char c in inputKey)
        {
            if (c == '\b')
            {
                return true;
            }
        }
        return false;
    }
}
