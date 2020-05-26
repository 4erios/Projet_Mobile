using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoCartes : MonoBehaviour
{
    public EmotionMonstre emotion;
    public Collider2D colid;
    public SpriteRenderer spriteNoir, fond, titre;
    public Animator animNoire;
    public Animator animPlayedCard;

    private void Start()
    {
        spriteNoir.sprite = emotion.sprite;
        if (emotion.titre != null)
        {
            titre.sprite = emotion.titre;
        }
        if (emotion.animNoire != null)
        {
            animNoire.runtimeAnimatorController = emotion.animNoire;
        }
    }

    public void ChangeLayerLevel(int newLevel)
    {
        spriteNoir.sortingOrder = newLevel;
        fond.sortingOrder = newLevel - 1;
        titre.sortingOrder = newLevel + 2;
    }


}
