using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RTLTMPro;
public class KeyboardTest : MonoBehaviour
{
    public string name = "TEST";
    public RTLTextMeshPro tmp;
    string tag = "<color=yellow>&SPT&</color>";
    int index = 0;
    int height = 23;
    // Start is called before the first frame update
    void Start()
    {
        KeyboardCapture.self.registeredKeys.AddListener(OnKey);
        tmp = this.transform.GetComponent<RTLTextMeshPro>();
        tmp.text = name.ToLower();
    }
    public void OnKey(KeyboardType key)
    {
        if (index >= name.Length) return;
        if (name[index].ToString().ToUpper() == key.Key)
        {
            // Debug.Log(key.Key);
            tmp.add_tag(tag, "&SPT&", index * height);
            // tmp.text = tmp.text.Insert(index * height, add_tag(key.Key));
            index++;
            if (index >= name.Length)
            {
                // what gonna happens to the Enemy
            }
        }

    }
    string add_tag(string value)
    {
        return tag.Replace("&SPT&", value.ToLower());
    }
    // Update is called once per frame
    void Update()
    {

    }
}
