using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuScript : EventTrigger
{
    private bool sceneLoading = false;

    public void LoadScene(int nbScene)
    {
        if (!sceneLoading)
        {
            sceneLoading = true;
            LoadingScreen.ShowLoadScreen();
            StartCoroutine(LoadGame(nbScene));
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
