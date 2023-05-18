using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using Unity.Collections;

public class Analytics : MonoBehaviour, IKeyboard
{
    public static Analytics self;
    public int CPS_Duration, WPM_Duration;
    [SerializeField] public CharacterPerSeconds CPS;
    [SerializeField] public WordPerMinuts WPM;

    // private CharPerSecond WPS = new();

    void Awake()
    {
        if (self != null) throw new System.Exception("Only one instance can be created at a time");
        self = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        CPS = new CharacterPerSeconds();
        CPS.Init(CPS_Duration * 60);
        WPM = new WordPerMinuts();
        WPM.Init(WPM_Duration);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnKey(KeyboardType keyType)
    {
        KeyboardEventHandler(keyType.Key);
    }
    public void OnKeyBuiltIn(string keyType)
    {
        KeyboardEventHandler(keyType);
    }
    void KeyboardEventHandler(string keyType)
    {

    }
}


