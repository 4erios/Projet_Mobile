using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Reactions { interet, affection, compassion, degout, peur, haine }
[System.Serializable]
public class PresetRole
{
    public Reactions ReactionDepart;
    public Emotions interet;
    public Emotions affection;
    public Emotions compassion;
    public Emotions degout;
    public Emotions peur;
    public Emotions haine;

    public Reactions GetReact(Emotions emot)
    {
        if (emot == interet)
        {
            return Reactions.interet;
        }
        else if (emot == affection)
        {
            return Reactions.affection;
        }
        else if (emot == compassion)
        {
            return Reactions.compassion;
        }
        else if (emot == degout)
        {
            return Reactions.degout;
        }
        else if (emot == peur)
        {
            return Reactions.peur;
        }
        else
        {
            return Reactions.haine;
        }
    }
}

[CreateAssetMenu(fileName ="Role", menuName ="Create Role")]
public class Role : ScriptableObject
{
    public List<PresetRole> presets;
    
}
