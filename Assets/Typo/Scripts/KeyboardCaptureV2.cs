using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class KeyboardCaptureV2 : MonoBehaviour
{
    public static KeyboardCaptureV2 self;
    private TouchScreenKeyboard keyboard;
    public UnityEvent<string> registeredKeys;
    private void Awake()
    {
        if (self != null) throw new UnityException("Too many keyboards");
        self = this;
    }
    private void Start()
    {
        // Open the on-screen keyboard on mobile devices
#if UNITY_EDITOR || UNITY_STANDALONE
        keyboard = null;
#else
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
#endif
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
                string input = keyboard.text;
                registeredKeys.Invoke(keyboard.text);

                // Clear the keyboard
                keyboard.text = "";
            }
        }
        else{
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
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
