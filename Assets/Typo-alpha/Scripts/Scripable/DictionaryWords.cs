using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DictionaryWords", menuName = "DictionaryWords", order = 1)]
public class DictionaryWords : ScriptableObject
{
    public string language;
    public string filePath;
    public int count;
    public int keys;
    private JsonTypes dict = new();
    public DictionaryType words = new();

    public void Status()
    {
        Debug.Log("Dictionary Lenght: " + count);
        Debug.Log("Dictionary Keys: " + words.list.Length);
    }
    public void LoadFiles()
    {
        count = 0;
        keys = 0;
        words = new();
        TextAsset dictionaryData_text = Resources.Load<TextAsset>(filePath);

        dict.data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(dictionaryData_text.text);

        count = dict.data.Count;

        Debug.Log("Loading dictionary...");
        if (dict.data != null)
        {
            foreach (KeyValuePair<string, object> entry in dict.data)
            {
                words.Value = entry.Key;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }

            Debug.Log("Loaded dictionary.");
        }
        keys = words.list.Length;
    }
}
[Serializable]
public class DictionaryType
{
    public Dict[] list = new Dict[0];

    public string[] Keys
    {
        get
        {
            string[] keys = new string[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                keys[i] = list[i].key;
            }
            return keys;
        }
    }
    public string Key
    {
        set
        {
            Append(value);
        }
    }
    public string[] Values(string key)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].key == key) return list[i].value;
        }
        return null;
    }
    public string Value
    {
        set
        {
            string key = value[..1];
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].key == key)
                {
                    Append(value, ref list[i].value);
                    return;
                }
            }
            this.Key = key;
            this.Value = value;
        }
    }
    public bool HasKey(string key)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].key == key) return true;
        }
        return false;
    }
    public string RandomValue
    {
        get
        {
            int i = UnityEngine.Random.Range(0, list.Length);
            int j = UnityEngine.Random.Range(0, list[i].value.Length);
            return list[i].value[j];
        }
    }
    void Append(string value, ref string[] array)
    {
        int index = array.Length;
        Array.Resize(ref array, array.Length + 1);
        array.SetValue(value, index);
    }
    void Append(string key)
    {
        int index = list.Length;
        Array.Resize(ref list, list.Length + 1);
        list[index] = new Dict(key);
    }

}
[Serializable]
public class Dict
{
    public string key;
    public string[] value;

    public Dict(string key)
    {
        this.key = key;
        this.value = new string[0];
    }

}
[Serializable]
public class JsonTypes
{
    public Dictionary<string, object> data = new();
}