using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject settings, cartes;

    public void OpenSettings()
    {
        settings.SetActive(true);
        HideCards();
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
        ShowCards();
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
