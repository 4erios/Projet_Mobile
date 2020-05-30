using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanqueJoueur : MonoBehaviour
{
    public static int currentMonnaie;

    private void Start()
    {
        currentMonnaie = SaveLoadSystem.LoadMonney();
        //Récupérer la sauvegarde de la Banque du joueur
    }

    public static void WinMonney(int montant)
    {
        currentMonnaie += montant;
        SaveLoadSystem.SaveMonney(currentMonnaie);
    }

    public static void LoseMonney (int montant)
    {
        currentMonnaie -= montant;
        SaveLoadSystem.SaveMonney(currentMonnaie);
    }
}
