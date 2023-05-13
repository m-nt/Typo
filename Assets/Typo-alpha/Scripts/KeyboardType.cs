using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RTLTMPro;


public class KeyboardType : MonoBehaviour
{
    public string Key;
    public RTLTextMeshPro3D text, scaledText;
    public SpriteRenderer Shape, ScaledShape;
    GameObject child;
    KeyboardType parent;
    void Start()
    {
        if (Key == null) throw new ArgumentException("Key is required");
        if (text == null || scaledText == null) throw new ArgumentNullException("text/scaled is required");
        if (Shape == null || ScaledShape == null) throw new ArgumentNullException("text/scaled is required");

        text.text = Key;
        scaledText.text = Key;
        gameObject.name = Key;

        if (KeyboardCapture.self.font != null)
        {
            text.font = KeyboardCapture.self.font;
            scaledText.font = KeyboardCapture.self.font;
        }

        Shape.color = KeyboardCapture.self.primary;
        ScaledShape.color = KeyboardCapture.self.scaled;

        if (transform.parent.parent)
            transform.parent.parent.TryGetComponent<KeyboardType>(out parent);

        if (transform.childCount < 3) return;
        child = transform.transform.GetChild(2).gameObject;
    }
    public bool ActivatedDeactivateChildKeys(bool state)
    {
        if (!child) return false;
        ActivatedDeactivateScaledKeys(false);
        child.SetActive(state);
        return true;
    }
    public void ChangeColor(ShateState state)
    {
        switch (state)
        {
            case ShateState.PRIMARY: Shape.color = KeyboardCapture.self.primary; break;
            case ShateState.SECONDRAY: Shape.color = KeyboardCapture.self.secondary; break;
            default: Shape.color = KeyboardCapture.self.primary; break;
        }
    }
    public void ActivatedDeactivateScaledKeys(bool state)
    {
        if (!scaledText) return;
        scaledText.gameObject.SetActive(state);
    }
    public void ActivatedDeactivateChildKeysFromChild(bool state)
    {
        if (!parent) return;
        parent.ActivatedDeactivateChildKeys(state);
    }
}
