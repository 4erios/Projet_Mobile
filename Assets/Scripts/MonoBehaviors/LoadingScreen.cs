using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject waitingScreen;

    private static GameObject screen;

    private void Start()
    {
        screen = waitingScreen;
    }

    public static void ShowLoadScreen()
    {
        screen.SetActive(true);
    }

    public static void HideLoadScreen()
    {
        screen.SetActive(false);
    }
}
