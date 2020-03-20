using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Create Dialogue")]
public class Dialogues : ScriptableObject
{
    public string dialogueDepart;
    public string reponseInteret, reponseAffection, reponseCompassion, reponseDegout, reponsePeur, reponseHaine, reponseBonus, reponseMalus;

    public Dialogues()
    {
        //Mettre les Choix et Réponses dans des listes
    }
}
