using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffichageSuccesInGame : MonoBehaviour
{
    [SerializeField]
    private GameObject anim;
    private static GameObject goAnim;

    [SerializeField]
    private Text titre;
    private static Text staticTitre;

    private void Start()
    {
        goAnim = anim;
        staticTitre = titre;
    }

    public static void AffichageSucces(string nom)
    {
        if (goAnim != null)
        {
            goAnim.SetActive(true);
            staticTitre.text = nom;
        }
    }

    public void FinAffichage()
    {
        gameObject.SetActive(false);
    }
}
