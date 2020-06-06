using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCarteJouee : MonoBehaviour
{
    public SpriteRenderer spriteNoir, fond, titre;

    public void DissolveNew(float fade, Sprite _spriteNoir, Sprite _fond, Sprite _titre)
    {
        gameObject.SetActive(true);
        spriteNoir.sprite = _spriteNoir;
        fond.sprite = _fond;
        titre.sprite = _titre;
        StartCoroutine(Dissolve(fade));
    }

    private IEnumerator Dissolve(float fade)
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        fade -= Time.fixedDeltaTime*2;

        if (fade <= 0)
        {
            fade = 0;
            spriteNoir.material.SetFloat("_Fade", fade);
            fond.material.SetFloat("_Fade", fade);
            titre.material.SetFloat("_Fade", fade);
            gameObject.SetActive(false);
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
