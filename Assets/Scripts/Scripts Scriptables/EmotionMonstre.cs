using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotions { gronder, attaquer, yeuxDoux, pleurer, caresser, fuir, none}
[CreateAssetMenu(fileName = "Emotion", menuName = "Create Emotion")]
public class EmotionMonstre : ScriptableObject
{
    public Emotions emotion;
    public Sprite sprite;
    public List<Role> bonusRole;
    public List<Role> malusRole;
}
