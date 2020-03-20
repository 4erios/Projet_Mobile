using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private int gameScene;

    private bool sceneLoading = false;

    public void LaunchGame()
    {
        if (!sceneLoading)
        {
            sceneLoading = true;
            StartCoroutine(LoadGame());
        }
    }

    IEnumerator LoadGame()
    {
        AsyncOperation asnc = SceneManager.LoadSceneAsync(gameScene);

        while(!asnc.isDone)
        {
            yield return null;
        }
        sceneLoading = false;
    }
}
