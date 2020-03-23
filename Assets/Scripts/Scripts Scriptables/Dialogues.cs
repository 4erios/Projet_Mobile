using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Create Dialogue")]
public class Dialogues : ScriptableObject
{
    [TextArea(3,5)]
    public string dialogueDepart;
    [TextArea(3,5)]
    public string reponseInteret, reponseAffection, reponseCompassion, reponseDegout, reponsePeur, reponseHaine, reponseBonus, reponseMalus;

}
