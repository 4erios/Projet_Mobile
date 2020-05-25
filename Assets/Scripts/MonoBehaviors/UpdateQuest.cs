﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateQuest : MonoBehaviour
{
    private List<Quest> quetes = new List<Quest>();
    private List<Quest> disabledQuests = new List<Quest>();
    [SerializeField]
    private List<Quest> pool = new List<Quest>();


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

        if (quetes.Count<=0 && pool.Count > 0)
        {
            for(int i = 0; i < 3; i++)
            {
                Quest q = pool[Random.Range(0, pool.Count)];
                quetes.Add(q);
                pool.Remove(q);
                usedQuest.Add(q);
            }
        }
        SaveLoadSystem.SaveAllUsedQuest(usedQuest);
        SaveLoadSystem.SaveAllQuest(quetes);
    }

    //Affichage des quêtes ?
}