using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingStartGame : MonoBehaviour
{
    private void Start()
    {
        SaveLoadSystem.ResetGameSate();
        LoadingScreen.HideLoadScreen();
    }
}
