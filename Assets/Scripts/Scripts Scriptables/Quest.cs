using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Create Quest")]
public class Quest : ScriptableObject
{
    public Sprite icone;
    public string titre;
    public string texteExplicatif;

    public Personnage killedPerso;
    public EndEvent endingWanted;
    public EmotionMonstre cardWanted;

    public bool isValid;

    public bool ValidateQuest()
    {
        isValid = true;
        AvancementSuccess.AddQuestAccomplished();
        return false;
    }
}
