using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoSuccess : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text, titre;

    public void ShowSuccess(Success succ)
    {
        image.sprite = succ.icone;
        titre.text = succ.titre;
        text.text = succ.texteExplicatif;
    }

    public void ShowValidSuccess()
    {
        image.color = Color.white;
        titre.color = Color.white;
        text.color = Color.white;
    }

    public void ShowHistoric(AncientGame game)
    {
        image.sprite = game.icone;
        titre.text = game.titre;
    }
}
