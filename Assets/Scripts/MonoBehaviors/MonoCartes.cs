using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoCartes : MonoBehaviour
{
    public EmotionMonstre emotion;
    public Collider2D colid;
    public SpriteRenderer spritePrincipal, titre;
    public Animator animNoire, animBlanche;

    private void Start()
    {
        spritePrincipal.sprite = emotion.sprite;
        if (emotion.titre != null)
        {
            titre.sprite = emotion.titre;
        }
        if (emotion.animNoire != null)
        {
            animNoire.runtimeAnimatorController = emotion.animNoire;
        }
        if (emotion.animBlanche != null)
        {
            animBlanche.runtimeAnimatorController = emotion.animBlanche;
        }
    }

    public void ChangeLayerLevel(int newLevel)
    {
        spritePrincipal.sortingOrder = newLevel;
    }
}
