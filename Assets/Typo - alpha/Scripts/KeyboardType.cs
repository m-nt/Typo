using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class KeyboardType : MonoBehaviour
{
    public string Key { get; set; }
    // Start is called before the first frame update
    GameObject child;
    KeyboardType parent;
    void Start()
    {
        if (this.transform.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI tmp))
            Key = tmp.text;
        else if (this.transform.childCount > 0 && this.transform.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI tmp_in_child))
            Key = tmp_in_child.text;
        else Key = this.gameObject.name;


        if (this.transform.parent.parent)
            this.transform.parent.parent.TryGetComponent<KeyboardType>(out parent);


        if (this.transform.childCount <= 0) return;
        child = this.transform.transform.GetChild(0).gameObject;
    }
    public void ActivatedDeactivateChildKeys(bool state)
    {
        if (!child) return;
        child.SetActive(state);
    }
    public void ActivatedDeactivateChildKeysFromChild(bool state)
    {
        if (!parent) return;
        parent.ActivatedDeactivateChildKeys(state);
    }
}
