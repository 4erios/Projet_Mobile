using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuScript : EventTrigger
{
    private bool sceneLoading = false;
    private Sprite spr;

    private void Start()
    {
        if (GetComponent<Image>() != null)
        {
            spr = GetComponent<Image>().sprite;
        }
    }

    public void LoadScene(int nbScene)
    {
        if (!sceneLoading)
        {
            sceneLoading = true;
            LoadingScreen.ShowLoadScreen();
            StartCoroutine(LoadGame(nbScene));
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (GetComponent<BoutonVariables>() != null)
        {
            GetComponent<Image>().sprite = GetComponent<BoutonVariables>().spr;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        GetComponent<Image>().sprite = spr;
        if (GetComponent<BoutonVariables>() != null)
        {
            GetComponent<BoutonVariables>().PlayFeedback();
            if (GetComponent<BoutonVariables>().newMenu > 0)
            {
                ChangeMenu(GetComponent<BoutonVariables>().newMenu);
            }
            else if (GetComponent<BoutonVariables>().newScene > 0)
            {
                LoadScene(GetComponent<BoutonVariables>().newScene);
            }
        }
    }

    public void ChangeMenu(int menuId)
    {
        switch (menuId)
        {
            case 1:
                MenuNavigation.mainMenuEvent.Invoke();
                break;
            case 2:
                MenuNavigation.achievementEvent.Invoke();
                break;
            case 3:
                MenuNavigation.settingsEvent.Invoke();
                break;
            case 4:
                MenuNavigation.creditEvent.Invoke();
                break;
            case 5:
                MenuNavigation.successEvent.Invoke();
                break;
            case 6:
                MenuNavigation.questEvent.Invoke();
                break;
            case 7:
                MenuNavigation.shopEvent.Invoke();
                break;
        }
    }


    IEnumerator LoadGame(int nb)
    {
        AsyncOperation asnc = SceneManager.LoadSceneAsync(nb);

        while(!asnc.isDone)
        {
            yield return null;
        }
        sceneLoading = false;
    }
}
