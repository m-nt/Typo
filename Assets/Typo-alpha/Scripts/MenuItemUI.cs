using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RTLTextMeshPro))]
public class MenuItemUI : MonoBehaviour, IKeyboard
{
    public Color Primary, Secondary;
    public UnityEvent actions;
    public string Name;
    private RTLTextMeshPro text;
    private ColorTag Tag;
    private int index;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (KeyboardCaptureV3.self != null)
            KeyboardCaptureV3.self.registeredKeys.AddListener(OnKeyBuiltIn);


        Tag = new ColorTag("#" + ColorUtility.ToHtmlStringRGB(Secondary));
        text = GetComponent<RTLTextMeshPro>();
        text.text = Name;
    }
    public void OnKey(KeyboardType key)
    {
        KeyboardEventHandler(key.Key);
    }
    public void OnKeyBuiltIn(string key)
    {
        KeyboardEventHandler(key);
    }
    void KeyboardEventHandler(string keyType)
    {

        // Check if the characters of the enemy is filled, prevent overflow error
        if (index >= Name.Length) { ResetObject(); return; }
        // Check if the key is BackSpace then reset the state
        if (keyType.ToUpper() == "BACKSPACE" && index > 0) { ResetObject(); return; }
        // Check if the key clicked/touched is the corresponding character of the enemy
        if (Name[index].ToString().ToUpper() != keyType.ToUpper()) return;
        // add the tag to the corresponding character
        text.add_tag(Tag.Tag, Tag.Seperator, index * Tag.Height);
        index++;
        if (index < Name.Length) return;

        // what gonna happens when the object is selected
        actions.Invoke();
    }
    void ResetObject()
    {
        Debug.Log(Name);
        index = 0;
        text.text = Name;
    }
}
