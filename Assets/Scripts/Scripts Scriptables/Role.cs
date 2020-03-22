using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Reactions { interet, affection, compassion, degout, peur, haine }

[System.Serializable]
public struct RoleReaction
{
    public Emotions emot;
    public int doesDamage;
}

[System.Serializable]
public class PresetRole
{
    
    public Reactions ReactionDepart;
    public RoleReaction interet;
    public RoleReaction affection;
    public RoleReaction compassion;
    public RoleReaction degout;
    public RoleReaction peur;
    public RoleReaction haine;

    public Reactions GetReact(Emotions emot, out int damage)
    {
        
        if (emot == interet.emot)
        {
            damage = interet.doesDamage;
            return Reactions.interet;
        }
        else if (emot == affection.emot)
        {
            damage = affection.doesDamage;
            return Reactions.affection;
        }
        else if (emot == compassion.emot)
        {
            damage = compassion.doesDamage;
            return Reactions.compassion;
        }
        else if (emot == degout.emot)
        {
            damage = degout.doesDamage;
            return Reactions.degout;
        }
        else if (emot == peur.emot)
        {
            damage = peur.doesDamage;
            return Reactions.peur;
        }
        else
        {
            damage = haine.doesDamage;
            return Reactions.haine;
        }
    }
}

[CreateAssetMenu(fileName ="Role", menuName ="Create Role")]
public class Role : ScriptableObject
{
    public List<PresetRole> presets;
    
}
