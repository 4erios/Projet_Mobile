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

    //private float fade = 0f;
    private bool isDissolving;

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

    public IEnumerator Dissolve(float fade)
    {
        Debug.Log("Dissolve ?" + fade);
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        fade += Time.fixedDeltaTime;

        if (fade>=1)
        {
            fade = 1;
            spriteNoir.material.SetFloat("_Fade", fade);
            fond.material.SetFloat("_Fade", fade);
            titre.material.SetFloat("_Fade", fade);
        }
        else
        {
            spriteNoir.material.SetFloat("_Fade", fade);
            fond.material.SetFloat("_Fade", fade);
            titre.material.SetFloat("_Fade", fade);
            StartCoroutine(Dissolve(fade));
        }
    }


}
