using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoPersonnage : MonoBehaviour
{
    private Image spriteRnd;

    public Personnage personnageActuel;

    private void Start()
    {
        spriteRnd = GetComponent<Image>();
    }

    public void ChangeSprite(Sprite spr)
    {
        spriteRnd.sprite = spr;
    }
}
