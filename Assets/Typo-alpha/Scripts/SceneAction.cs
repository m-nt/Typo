using System;
using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneAction : MonoBehaviour
{
    public GameObject ThisAction, NextAction;
    public Animator SceneAnimator;
    public UnityEvent InitActions, EventActions;

    public void GoNextAction()
    {
        NextAction?.SetActive(true);
        ThisAction.SetActive(false);
        EventActions.Invoke();
    }

    void Start()
    {
        // if (!GlobalValues.Is_first_time) LoadSceneAction("Menu V1");
        // if (Name.Length <= 0) Actions.Invoke();
    }
    void OnEnable()
    {
        InitActions.Invoke();
    }

    public void EmptyAction()
    {
        GoNextAction();
    }
    public void HiOutAction()
    {
        SceneAnimator.SetTrigger("out");
    }
    public void LanguageAction(int language)
    {
        GlobalValues.Language = (Language)language;
        SceneAnimator.SetInteger("lang", language);
        // GoNextAction();

    }
    public void DisableFirstTimeAction()
    {
        GlobalValues.Is_first_time = true;
    }
    public void LoadSceneAction(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void LoadSceneActionIfNotFirstTime(string name)
    {
        if (!GlobalValues.Is_first_time) { GoNextAction(); return; }
        LoadSceneAction(name);
    }
}
