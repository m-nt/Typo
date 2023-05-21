using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValues : MonoBehaviour
{
    public static bool Is_first_time
    {
        get { return PlayerPrefs.GetInt("Is_first_time", 0) == 1; }
        set { PlayerPrefs.SetInt("Is_first_time", value ? 1 : 0); }
    }
    public static Language Language
    {
        get { return (Language)PlayerPrefs.GetInt("Language", (int)Language.EN); }
        set { PlayerPrefs.SetInt("Language", (int)value); }
    }
    public static int Score
    {
        get { return PlayerPrefs.GetInt("Score", 0); }
        set { PlayerPrefs.SetInt("Score", value); }
    }

}