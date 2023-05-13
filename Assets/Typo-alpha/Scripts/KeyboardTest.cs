using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RTLTMPro;
public class KeyboardTest : MonoBehaviour
{
    public string Name = "TEST";
    public RTLTextMeshPro tmp;
    private readonly string Tag = "<color=yellow>&SPT&</color>";
    int index = 0;
    readonly int height = 23;
    // Start is called before the first frame update
    void Start()
    {
        KeyboardCapture.self.registeredKeys.AddListener(OnKey);
        tmp = this.transform.GetComponent<RTLTextMeshPro>();
        tmp.text = Name.ToLower();
    }
    public void OnKey(KeyboardType key)
    {
        if (index >= Name.Length) return;
        if (Name[index].ToString().ToUpper() == key.Key)
        {
            tmp.add_tag(Tag, "&SPT&", index * height);
            index++;
            if (index >= Name.Length)
            {
                // what gonna happens to the Enemy
            }
        }

    }

}
