using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSuccess : MonoBehaviour
{
    [SerializeField]
    private List<AncientGame> historic;
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

        unlockedSuccess = SaveLoadSystem.GetSuccessList();
        historic = SaveLoadSystem.GetHistoricList();
    }

    public void AffichageHistorique()
    {
        if(historic.Count>0 && historic[0]!=null)
        {
            for(int i = 0; i < historic.Count; i++)
            {
                historicList[i].ShowHistoric(historic[i]);
            }
        }
    }

    public void AffichageDerniersSucces()
    {
        int j = 0;

        int forEnd = unlockedSuccess.Count - 4;
        if(forEnd<0)
        {
            forEnd = 0;
        }
        if (unlockedSuccess.Count > 0)
        {
            for (int i = unlockedSuccess.Count - 1; i >= forEnd; i--)
            {
                threeSuccess[j].ShowSuccess(unlockedSuccess[j]);
                j++;
            }
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
                    successList[i].ShowSuccess(allSuccess[i]); ;
                }
            }
        }
    }
}
