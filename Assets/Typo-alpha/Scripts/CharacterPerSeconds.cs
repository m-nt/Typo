using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class CharacterPerSeconds
{
    Coroutine CPSCoroutine;

    public void Init(int? Duration)
    {
        CPSCoroutine = Analytics.self.StartCoroutine(TickesCPS());
        if (Duration != null) WordPerMinute.Duration = (int)Duration;
    }
    ~CharacterPerSeconds()
    {
        // Debug.Log("CPS");
        Analytics.self.StopCoroutine(CPSCoroutine);
    }

    IEnumerator TickesCPS()
    {
        // yield return new WaitForSeconds(1);
        while (true)
        {
            CharPerSecond.ADD = CharPerSecond.Count;
            CharPerSecond.ADD_TOTAL = CharPerSecond.TotalCount;
            CharPerSecond.TotalCount = 0;
            CharPerSecond.Count = 0;
            // Debug.Log("CPS: " + CharPerSecond.CPS);
            // Debug.Log("MAX_CPS: " + CharPerSecond.MAX_CPS);
            // Debug.Log("MISS_HITS: " + CharPerSecond.MISS_HITS);
            // Debug.Log("TOTAL_CHAR: " + CharPerSecond.TOTAL_CHAR);
            // Debug.Log("TOTAL_HIT: " + CharPerSecond.TOTAL_HIT);
            yield return new WaitForSeconds(1);
        }

    }
}

[Serializable]
public class CharPerSecond : MonoBehaviour
{
    public static int Duration = 60 * 10; // Store only 10 minutes of WPM

    private static Dictionary<DateTime, int> CPS_LIST
    {
        get { return JsonConvert.DeserializeObject<Dictionary<DateTime, int>>(PlayerPrefs.GetString("CPS_LIST", "{}")); }
        set { PlayerPrefs.SetString("CPS_LIST", JsonConvert.SerializeObject(value, Formatting.Indented)); }
    }
    private static Dictionary<DateTime, int> TOTAL_CHAR_LIST
    {
        get { return JsonConvert.DeserializeObject<Dictionary<DateTime, int>>(PlayerPrefs.GetString("TOTAL_CHAR_LIST", "{}")); }
        set { PlayerPrefs.SetString("TOTAL_CHAR_LIST", JsonConvert.SerializeObject(value, Formatting.Indented)); }
    }
    public static int Count = 0;
    public static int TotalCount = 0;
    public static int ADD_TOTAL
    {
        set
        {
            Dictionary<DateTime, int> list = TOTAL_CHAR_LIST;
            list.Add(DateTime.Now, value);
            if (list.Count > Duration) list.Skip(1);
            TOTAL_CHAR_LIST = list;
        }
    }
    public static int ADD
    {
        set
        {
            Dictionary<DateTime, int> list = CPS_LIST;
            list.Add(DateTime.Now, value);
            if (list.Count > Duration) list.Skip(1);
            CPS_LIST = list;
        }
    }
    public static float CPS
    {
        get
        {
            double average = CPS_LIST.Select(f => f.Value).DefaultIfEmpty().Average();
            return (float)average;
        }
    }
    public static int MISS_HITS
    {
        get
        {
            return TOTAL_CHAR - TOTAL_HIT;
        }
    }
    public static int TOTAL_HIT
    {
        get
        {
            int chars = CPS_LIST.Select(f => f.Value).DefaultIfEmpty().Sum();
            return chars;
        }
    }
    public static int TOTAL_CHAR
    {
        get
        {
            int chars = TOTAL_CHAR_LIST.Select(f => f.Value).DefaultIfEmpty().Sum();
            return chars;
        }
    }
    public static int ACCURACY
    {
        get
        {
            return (int)(((float)TOTAL_HIT / (float)TOTAL_CHAR) * 100);
        }
    }
    public static float MAX_CPS
    {
        get
        {
            int max = CPS_LIST.Select(f => f.Value).DefaultIfEmpty().Max();
            return max;
        }
    }
    public static float MIN_CPS
    {
        get
        {
            int min = CPS_LIST.Select(f => f.Value).DefaultIfEmpty().Min();
            return min;
        }
    }
    public static int Length
    {
        get
        {
            return CPS_LIST.Count;
        }
    }
    public static string Json
    {
        get { return JsonConvert.SerializeObject(CPS_LIST, Formatting.Indented); }
    }


}
