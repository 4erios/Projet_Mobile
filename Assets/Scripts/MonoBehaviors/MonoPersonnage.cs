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

    private float transitionSpeed = 0.06f;

    private void Start()
    {
        spriteRnd = GetComponent<Image>();
        Color newColor = Color.white;
        newColor.a = 0;
        spriteRnd.color = newColor;
    }

    public void ChangeSprite(Sprite spr, string anim, bool needAnim)
    {
        spriteRnd.sprite = spr;
        if (needAnim)
        {
            GetComponent<Animator>().Play(anim, 0, 0.25f);
        }
    }

    public void Apparition()
    {
        StartCoroutine(ShowSprite());
    }

    public void Disparition()
    {
        GetComponent<Animator>().Play("BaseAnimPerso", 0, 0.25f);
        StartCoroutine(HideSprite());
    }

    public void Die()
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().Play("Death", 0, 0.25f);
        
    }

    IEnumerator ShowSprite()
    {
        Color newColor = Color.white;
        newColor.a = 0;
        for(int i = 0; i < 1/transitionSpeed; i++)
        {
            newColor.a += transitionSpeed;
            spriteRnd.color = newColor;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        GetComponent<Animator>().enabled = true;
    }

    IEnumerator HideSprite()
    {
        GetComponent<Animator>().Play("BaseAnimPerso", 0, 0.25f);
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        GetComponent<Animator>().enabled = false;
        Color newColor = spriteRnd.color;
        newColor.a = 1;
        for (int i = 0; i < 1/transitionSpeed; i++)
        {
            if(newColor.a <= 0)
            {
                break;
            }
            newColor.a -= transitionSpeed;
            spriteRnd.color = newColor;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        //GetComponent<RectTransform>().position = new Vector3(-2, GetComponent<RectTransform>().position.y, GetComponent<RectTransform>().position.z);
        manager.EndEvent();
    }
}
