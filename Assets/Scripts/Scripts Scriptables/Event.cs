using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Lieux { ruelle, pontDeSeine, parc, cabaret}

[CreateAssetMenu(fileName = "Event", menuName = "Create Event")]
public class Event : ScriptableObject
{
    public Personnage personnage;
    public Accroche accroche;
    public Dialogues dialogue;
    public Lieux lieux;
    public EmotionMonstre effetBonus;
    public EmotionMonstre carteBonus;
    public EmotionMonstre effetMalus;
    public EmotionMonstre carteMalus;
    public int preset;
    public Emotions killPersonnage;
    public Event eventSuivant;
}
