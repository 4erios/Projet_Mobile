﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateQuest : MonoBehaviour
{
    private List<Quest> quetes = new List<Quest>();
    private List<Quest> disabledQuests = new List<Quest>();
    [SerializeField]
    private List<Quest> pool = new List<Quest>();
    [SerializeField]
    private Quest beginQuest;

    [SerializeField]
    private List<Text> titre = new List<Text>(), description = new List<Text>();
    [SerializeField]
    private List<GameObject> validationQuest;
    [SerializeField]
    private Text monnaie;

    private List<Quest> questListe = new List<Quest>();

    private void Start()
    {
        //Vérification de la date, Load en cas de nécessité, Récupération de quêtes déjà effectuées, Mise à jour du Pool de Quête
        List<Quest> usedQuest = SaveLoadSystem.GetUsedQuestList();
        if(usedQuest.Count < pool.Count-2)
        {
            foreach (Quest q in usedQuest)
            {
                pool.Remove(q);
            }
        }
        else
        {
            usedQuest = new List<Quest>();
            SaveLoadSystem.ResetQuest();
        }
        Debug.Log("Test ??");
        Debug.Log(System.DateTime.Now.Date == new System.DateTime(2020, 6, 8));

        if (System.DateTime.Now.Day != PlayerPrefs.GetInt("Day"))
        {
            PlayerPrefs.SetInt("Day", System.DateTime.Now.Day);
            if (quetes.Count <= 0 && pool.Count > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Quest q = pool[Random.Range(0, pool.Count)];
                    quetes.Add(q);
                    pool.Remove(q);
                    usedQuest.Add(q);
                }
            }
            questListe = quetes;
            SaveLoadSystem.SaveAllUsedQuest(usedQuest);
            SaveLoadSystem.SaveAllQuest(quetes);
        }
        else
        {
            questListe = SaveLoadSystem.GetQuestList();
        }
        if (System.DateTime.Now.Date == new System.DateTime(2020, 6, 8) || System.DateTime.Now.Date == new System.DateTime(2020, 6, 9) || System.DateTime.Now.Date == new System.DateTime(2020, 6, 10) || System.DateTime.Now.Date == new System.DateTime(2020, 6, 11) || System.DateTime.Now.Date == new System.DateTime(2020, 6, 12))
        {
            Debug.Log(questListe.Count);
            questListe[0] = beginQuest;
        }
        SaveLoadSystem.SaveAllQuest(questListe);
        MenuNavigation.questEvent.AddListener(AffichageMenu);
        MenuNavigation.achatPersoEvent.AddListener(AffichageArgent);
    }

    private void AffichageMenu()
    {
        for (int i = 0; i < 3; i++)
        {
            titre[i].text = questListe[i].titre;
            description[i].text = questListe[i].texteExplicatif;
            validationQuest[i].SetActive(questListe[i].isValid);
            //Rajouter si la quête a été validé ou non
        }
        AffichageArgent();
    }

    private void AffichageArgent()
    {
        monnaie.text = BanqueJoueur.currentMonnaie.ToString();
    }
}
