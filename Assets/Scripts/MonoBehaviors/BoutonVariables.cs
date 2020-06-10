using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonVariables : MonoBehaviour
{
    public Sprite spr;
    public int newScene = -1;
    public int newMenu = -1;

    [SerializeField]
    private AudioClip feedbackBouton;
    [SerializeField]
    private AudioSource source;

    public void PlayFeedback()
    {
        //source.PlayOneShot(feedbackBouton);
    }
}
