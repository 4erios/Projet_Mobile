﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public static UnityEvent mainMenuEvent, achievementEvent, settingsEvent, creditEvent, successEvent, questEvent, shopEvent;
    public static UnityEvent achatPersoEvent = new UnityEvent();

    // Start is called before the first frame update
    void Awake()
    {
        mainMenuEvent = new UnityEvent();
        achievementEvent = new UnityEvent();
        settingsEvent = new UnityEvent();
        creditEvent = new UnityEvent();
        successEvent = new UnityEvent();
        questEvent = new UnityEvent();
        shopEvent = new UnityEvent();
        mainMenuEvent.AddListener(OpenMainMenu);
        achievementEvent.AddListener(OpenAchievementMenu);
        settingsEvent.AddListener(OpenSettingsMenu);
        creditEvent.AddListener(OpenCreditsMenu);
        successEvent.AddListener(OpenSuccessMenu);
        questEvent.AddListener(OpenQuestMenu);
        shopEvent.AddListener(OpenShop);

        if (!PlayerPrefs.HasKey("FirstLoad"))
        {
            PlayerPrefs.SetInt("FirstLoad", 1);
            PlayerPrefs.Save();
            anim.SetBool("EcranTitre", true);
            StartCoroutine(EcranTitreReset());
        }
        PlayerPrefs.DeleteKey("FirstLoad");
    }

    IEnumerator EcranTitreReset()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("EcranTitre", false);
    }

    void ResetAnim()
    {
        anim.SetBool("MainMenu", false);
        anim.SetBool("Achievement", false);
        anim.SetBool("Settings", false);
        anim.SetBool("Credit", false);
        anim.SetBool("Success", false);
        anim.SetBool("Quest", false);
        anim.SetBool("Shop", false);
    }

    void OpenMainMenu()
    {
        ResetAnim();
        anim.SetBool("MainMenu", true);
    }

    void OpenShop()
    {
        ResetAnim();
        anim.SetBool("Shop", true);
    }


    void OpenAchievementMenu()
    {
        ResetAnim();
        anim.SetBool("Achievement", true);
    }

    void OpenSettingsMenu()
    {
        ResetAnim();
        anim.SetBool("Settings", true);
    }

    void OpenCreditsMenu()
    {
        ResetAnim();
        anim.SetBool("Credit", true);
    }

    void OpenSuccessMenu()
    {
        ResetAnim();
        anim.SetBool("Success", true);
    }

    void OpenQuestMenu()
    {
        ResetAnim();
        anim.SetBool("Quest", true);
    }
}
