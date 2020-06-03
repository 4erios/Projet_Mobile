using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Create Quest")]
public class Quest : ScriptableObject
{
    public string titre;
    public string texteExplicatif;

    public Personnage killedPerso;
    public EndEvent endingWanted;
    public EmotionMonstre cardWanted;

    public bool isValid;
    public int gain;

    [HideInInspector]
    public GameObject validationObj;

    public bool ValidateQuest()
    {
        isValid = true;
        AvancementSuccess.AddQuestAccomplished();
        if(validationObj != null)
        {
            validationObj.SetActive(true);
        }
        BanqueJoueur.WinMonney(gain);
        return false;
    }
}
