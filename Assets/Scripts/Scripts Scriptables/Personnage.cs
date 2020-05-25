using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Personnage", menuName = "Create Personnage")]
public class Personnage : ScriptableObject
{
    public Role role;
    public Communaute communaute;
    public List<Sprite> spriteReaction; //Voir si on met une Anim ou une liste de Sprites
    public bool isAlive = true;
    public Event rencontreEvent;
    public Success dyingSuccess;
    public int cout = 10;
    public bool isUnlocked = true;

    public void Die()
    {
        isAlive = false;
    }
}
