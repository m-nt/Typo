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
        Debug.Log("Dictionary Keys: " + words.Length);
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
                words.ValueByLen = entry.Key;
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif

            Debug.Log("Loaded dictionary.");
        }
        keys = words.Length;
    }
}
[Serializable]
public class DictionaryType
{
    Dict[] list = new Dict[0];
    DictByLength[] list_by_len = new DictByLength[0];

    public int Length
    {
        get
        {
            return list.Length;
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
            AppendList(key);
            Value = value;
        }
    }
    public string ValueByLen
    {
        set
        {
            for (int i = 0; i < list_by_len.Length; i++)
            {
                if (list_by_len[i].len == value.Length)
                {
                    Append(value, ref list_by_len[i].value);
                    return;
                }
            }
            AppendListByLen(value.Length);
            ValueByLen = value;
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
    private string RandomValue
    {
        get
        {
            int i = UnityEngine.Random.Range(0, list.Length);
            int j = UnityEngine.Random.Range(0, list[i].value.Length);
            return list[i].value[j];
        }
    }
    public void RandomValues(int count, ref string[] values)
    {
        values = new string[count];
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = RandomValue;
        }
    }
    public void RandomValues(int count, int len, ref string[] values)
    {
        values = new string[count];
        for (int i = 0; i < values.Length; i++)
        {
            RandomValues(len, out string value);
            values[i] = value;
        }
    }
    private void RandomValues(int worldLength, out string value)
    {
        for (int i = 0; i < list_by_len.Length; i++)
        {
            if (list_by_len[i].len != worldLength) continue;
            int j = UnityEngine.Random.Range(0, list_by_len[i].value.Length);
            value = list_by_len[i].value[j];
            return;
        }
        value = RandomValue;
    }

    void Append(string value, ref string[] array)
    {
        int index = array.Length;
        Array.Resize(ref array, array.Length + 1);
        array.SetValue(value, index);
    }
    void AppendList(string key)
    {
        int index = list.Length;
        Array.Resize(ref list, list.Length + 1);
        list[index] = new Dict(key);
    }
    void AppendListByLen(int len)
    {
        int index = list_by_len.Length;
        Array.Resize(ref list_by_len, list_by_len.Length + 1);
        list_by_len[index] = new DictByLength(len);
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
public class DictByLength
{
    public int len;
    public string[] value;

    public DictByLength(int len)
    {
        this.len = len;
        value = new string[0];
    }
}
[Serializable]
public class JsonTypes
{
    public Dictionary<string, object> data = new();
}