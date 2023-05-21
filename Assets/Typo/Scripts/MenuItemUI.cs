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
        Debug.LogError(KeyboardCapture.self.name);
        KeyboardCapture.self?.registeredKeys.AddListener(OnKeyboardEventHandler);
        Tag = new ColorTag("#" + ColorUtility.ToHtmlStringRGB(Secondary));
        text = GetComponent<RTLTextMeshPro>();
        text.text = Name;
    }
    public void OnKeyboardEventHandler(string key)
    {
        Debug.LogError(key);

        if (!gameObject.activeInHierarchy) return;
        // Check if the characters of the Item is filled, prevent overflow error
        if (index >= Name.Length) { ResetObject(); return; }
        // Check if the key is BackSpace then reset the state
        if (key.ToUpper() == "BACKSPACE" && index > 0) { ResetObject(); return; }
        // Check if the key clicked/touched is the corresponding character of the Item
        if (Name[index].ToString().ToUpper() != key.ToUpper()) return;
        if (index <= 0)
        {
            // if selected Item in Item manager is null asign this gameObject to the selected Item
            MenuManager.self.SelectedItem = MenuManager.self.SelectedItem == null ? gameObject : MenuManager.self.SelectedItem;
        }
        // Check is this Item is selected or not
        if (MenuManager.self.SelectedItem != gameObject) return;
        // add the tag to the corresponding character
        text.add_tag(Tag.Tag, Tag.Seperator, index * Tag.Height);
        index++;
        if (index < Name.Length) return;

        // what gonna happens when the object is selected
        MenuManager.self.SelectedItem = null;
        actions.Invoke();
    }
    void ResetObject()
    {
        MenuManager.self.SelectedItem = null;
        index = 0;
        text.text = Name;
    }
}
