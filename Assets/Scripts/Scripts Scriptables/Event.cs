using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Create Event")]
public class Event : ScriptableObject
{
    public Personnage personnage;
    public Dialogues dialogue;
    public EmotionMonstre carteBonus;
    public int preset;
    public Event eventSuivant;
    public bool endGame;
}
