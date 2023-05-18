using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class WordPerMinuts
{
    Coroutine WPMCoroutine;

    public void Init(int? Duration)
    {
        WPMCoroutine = Analytics.self.StartCoroutine(TickesWPM());
        if (Duration != null) WordPerMinute.Duration = (int)Duration;
    }
    ~WordPerMinuts()
    {
        // Debug.Log("WPM");
        Analytics.self.StopCoroutine(WPMCoroutine);
    }

    IEnumerator TickesWPM()
    {

        // yield return new WaitForSeconds(60);
        while (true)
        {
            WordPerMinute.ADD = WordPerMinute.Count;
            WordPerMinute.Count = 0;
            // Debug.Log("WPM: " + WordPerMinute.WPM);
            // Debug.Log("MAX_WPM: " + WordPerMinute.MAX_WPM);
            // Debug.Log("MIN_WPM: " + WordPerMinute.MIN_WPM);
            yield return new WaitForSeconds(60);
        }

    }
}
[Serializable]
public class WordPerMinute : MonoBehaviour
{
    public static int Duration = 10; // Store only 10 minutes of WPM

    private static Dictionary<DateTime, int> WPM_LIST
    {
        get { return JsonConvert.DeserializeObject<Dictionary<DateTime, int>>(PlayerPrefs.GetString("WPM_LIST", "{}")); }
        set { PlayerPrefs.SetString("WPM_LIST", JsonConvert.SerializeObject(value, Formatting.Indented)); }
    }
    public static int Count = 0;
    public static int ADD
    {
        set
        {
            Dictionary<DateTime, int> list = WPM_LIST;
            list.Add(DateTime.Now, value);
            if (list.Count > Duration) list.Skip(1);
            WPM_LIST = list;
        }
    }
    public static float WPM
    {
        get
        {
            double average = WPM_LIST.Select(f => f.Value).DefaultIfEmpty().Average();
            return (float)average;
        }
    }
    public static float MAX_WPM
    {
        get
        {
            int max = WPM_LIST.Select(f => f.Value).DefaultIfEmpty().Max();
            return max;
        }
    }
    public static float MIN_WPM
    {
        get
        {
            int min = WPM_LIST.Select(f => f.Value).DefaultIfEmpty().Min();
            return min;
        }
    }
    public static int Length
    {
        get
        {
            return WPM_LIST.Count;
        }
    }
    public static string Json
    {
        get { return JsonConvert.SerializeObject(WPM_LIST, Formatting.Indented); }
    }


}
