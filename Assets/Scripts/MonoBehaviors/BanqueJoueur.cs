using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanqueJoueur : MonoBehaviour
{
    public static int currentMonnaie;

    private void Start()
    {
        currentMonnaie = 100;
        //Récupérer la sauvegarde de la Banque du joueur
    }
}
