using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvancementSuccess : MonoBehaviour
{
    public static List<Success> successList;

    private static int playedCards, killedCharacter, questsAccomplished;

    private void Start()
    {
        successList = new List<Success>();
        foreach (Success succ in SaveLoadSystem.everySuccess)
        {
            successList.Add(succ);
        }
        foreach(Success succ in SaveLoadSystem.GetSuccessList())
        {
            if(successList.Contains(succ))
            {
                successList.Remove(succ);
            }
        }

        SaveLoadSystem.LoadSuccessState(out playedCards, out killedCharacter, out questsAccomplished);

        //Faire un autre truc de sauvgearde pour les etats des succès comme ça
    }

    public static void AddEnding(EndEvent ending)
    {
        if (successList.Contains(ending.success))
        {
            ending.success.ValidateSuccess();
        }
    }

    public static void AddUnlockCard(EmotionMonstre unlockedCard)
    {
        if(unlockedCard.name == "Play")
        {
            SearchForSuccess("Kid");
        }
        else if(unlockedCard.name == "Ignore")
        {
            SearchForSuccess("Ghost");
        }
    }

    public static void AddCardPlayed()
    {
        playedCards++;
        SaveLoadSystem.SaveSuccessState(1, 0, 0);
        if (playedCards==120)
        {
            SearchForSuccess("Human");
        }
        else if(playedCards==60)
        {
            SearchForSuccess("Animal");
        }
        else if(playedCards==20)
        {
            SearchForSuccess("Stranger Things");
        }
    }

    public static void AddKilledCharacter(Personnage perso)
    {
        killedCharacter++;
        SaveLoadSystem.SaveSuccessState(0, 1, 0);
        if (killedCharacter==1)
        {
            SearchForSuccess("No Return");
        }
        else if(killedCharacter==50)
        {
            SearchForSuccess("Murder");
        }
        else if(killedCharacter==100)
        {
            SearchForSuccess("Terrorist");
        }
        perso.dyingSuccess.ValidateSuccess();
    }

    public static void AddQuestAccomplished()
    {
        questsAccomplished++;
        SaveLoadSystem.SaveSuccessState(0, 0, 1);
        if (questsAccomplished == 1)
        {
            SearchForSuccess("New City");
        }
        else if (questsAccomplished == 5)
        {
            SearchForSuccess("Altruistic");
        }
        else if (questsAccomplished == 10)
        {
            SearchForSuccess("Adventurer");
        }
        else if (questsAccomplished == 25)
        {
            SearchForSuccess("Mercenary");
        }
    }

    private static void SearchForSuccess(string successName)
    {
        foreach(Success succ in successList)
        {
            if(succ.titre == successName)
            {
                succ.ValidateSuccess();
                //Animation de débloquage de succès
                return;
            }
        }
    }
}
