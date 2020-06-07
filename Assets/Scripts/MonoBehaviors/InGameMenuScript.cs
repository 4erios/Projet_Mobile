using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject settings, cartes, journal;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(settings.activeSelf)
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
            }
        }
    }

    public void OpenSettings()
    {
        settings.SetActive(true);
        if (!journal.activeSelf)
        {
            HideCards();
        }
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
        if (!journal.activeSelf)
        {
            ShowCards();
        }
    }

    void HideCards()
    {
        cartes.SetActive(false);
    }

    void ShowCards()
    {
        cartes.SetActive(true);
    }
}
