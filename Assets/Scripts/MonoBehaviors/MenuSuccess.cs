using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSuccess : MonoBehaviour
{
    [SerializeField]
    private List<AncientGame> historic, allHistoric;
    [SerializeField]
    private List<Success> unlockedSuccess, allSuccess; //Créer un Scriptable avec la liste des succès, qu'on pourrait ensuite utiliser pour faire voyager les succès d'une scène à l'autre

    [SerializeField]
    private List<MonoSuccess> historicList, successList, threeSuccess;

    private void Start()
    {
        MenuNavigation.achievementEvent.AddListener(AffichageHistorique);
        MenuNavigation.achievementEvent.AddListener(AffichageDerniersSucces);
        MenuNavigation.achievementEvent.AddListener(AffichageSuccess);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        /* Récupère les Saves si elles existent
         * Crée les 2 listes (3 Parties précédentes ET celle des succès disponible)
         */
    }

    public void AffichageHistorique()
    {
        if(historic.Count>0 && historic[0]!=null)
        {
            for(int i = 0; i < 3; i++)
            {
                historicList[i].ShowHistoric(historic[i]);
            }
        }
    }

    public void AffichageDerniersSucces()
    {
        int j = 0;
        for(int i = unlockedSuccess.Count-1; i > unlockedSuccess.Count-4; i--)
        {
            threeSuccess[j].ShowSuccess(unlockedSuccess[j]);
            j++;
        }
    }

    public void AffichageSuccess()
    {
        if(unlockedSuccess.Count>0 && unlockedSuccess[0]!=null)
        {
            for(int i = 0; i < allSuccess.Count; i++)
            {
                if(unlockedSuccess.Contains(allSuccess[i]))
                {
                    successList[i].gameObject.SetActive(true);
                }
            }
        }
    }
}
