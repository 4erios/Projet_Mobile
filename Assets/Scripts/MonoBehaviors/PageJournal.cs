using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageJournal : MonoBehaviour
{
    [SerializeField]
    private Text txt, interv;

    [SerializeField]
    private Image image, commuSprite;

    [SerializeField]
    private List<float> angles = new List<float>();

    [SerializeField]
    private List<Sprite> journaux;

    [SerializeField]
    private Sprite spriteVide;

    public void ShowJournal(string t, Sprite commu, string interview)
    {
        transform.localScale = Vector3.one;
        txt.text = t;
        if (commu != null)
        {
            commuSprite.sprite = commu;
        }
        else
        {
            commuSprite.sprite = spriteVide;
        }
        interv.text = interview;
        if(angles.Count>0)
        {
            transform.eulerAngles = new Vector3(0, 0, angles[Random.Range(0, angles.Count)]);
        }

        image.sprite = journaux[Random.Range(0, 4)];
    }
}
