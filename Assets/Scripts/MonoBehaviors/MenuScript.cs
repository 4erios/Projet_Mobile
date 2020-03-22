using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private bool sceneLoading = false;

    public void LoadScene(int nbScene)
    {
        if (!sceneLoading)
        {
            sceneLoading = true;
            StartCoroutine(LoadGame(nbScene));
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
