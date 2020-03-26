using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Create Event")]
public class Event : ScriptableObject
{
    public Personnage personnage;
    public Dialogues dialogue;
    public EmotionMonstre effetBonus;
    public EmotionMonstre carteBonus;
    public EmotionMonstre effetMalus;
    public EmotionMonstre carteMalus;
    public int preset;
    public Emotions killPersonnage;
    public Event eventSuivant;
}
