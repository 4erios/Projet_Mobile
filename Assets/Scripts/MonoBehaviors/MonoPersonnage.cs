using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoPersonnage : MonoBehaviour
{
    private Image spriteRnd;

    public Personnage personnageActuel;

    private Sprite sprite;

    [SerializeField]
    private MonstreEventManager manager;

    private void Start()
    {
        spriteRnd = GetComponent<Image>();
    }

    public void ChangeSprite(Sprite spr)
    {
        spriteRnd.sprite = spr;
    }

    public void Apparition()
    {
        StartCoroutine(ShowSprite());
    }

    public void Disparition()
    {
        StartCoroutine(HideSprite());
    }

    IEnumerator ShowSprite()
    {
        Color newColor = Color.white;
        newColor.a = 0;
        for(int i = 0; i < 100; i++)
        {
            newColor.a += 0.06f;
            spriteRnd.color = newColor;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    IEnumerator HideSprite()
    {
        Color newColor = Color.white;
        newColor.a = 1;
        for (int i = 0; i < 100; i++)
        {
            newColor.a -= 0.06f;
            spriteRnd.color = newColor;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        manager.EndEvent();
    }
}
