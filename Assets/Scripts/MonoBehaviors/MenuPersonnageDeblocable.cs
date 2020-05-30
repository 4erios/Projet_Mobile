using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPersonnageDeblocable : MonoBehaviour
{
    public Personnage perso;

    [SerializeField]
    private Image img;
    [SerializeField]
    private Text nom, cout;

    private void OnEnable()
    {
        img.sprite = perso.spriteReaction[0];
        nom.text = perso.name;
        cout.text = perso.cout.ToString();
    }

    public void UnlockCharacter()
    {
        if(!perso.isUnlocked && perso.cout <= BanqueJoueur.currentMonnaie)
        {
            perso.isUnlocked = true;
            BanqueJoueur.LoseMonney(perso.cout);
            SaveLoadSystem.SaveUnlockedPerso(perso.name);
            MenuNavigation.achatPersoEvent.Invoke();
            //Rajouter ce perso à la liste des persos débloqués
        }
    }
}
