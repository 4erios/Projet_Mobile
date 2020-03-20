using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Representation
{
    public int valeur, seuilHaut, seuilBas;
    public List<Event> eventsPhase2;
    public List<string> journalPhrasesUp, journalPhrasesDown;

    public int AddPoint(int value, MonstreEventManager manag) // Ajoute/Enlève les points de la Représentation. Renvoie un Bool qui dit s'il y a besoin du Journal ou pas
    {
        int ancientValue = valeur;
        valeur += value;

        if(ancientValue < valeur && valeur > seuilHaut)
        {
            MonstreEventManager.AddEvent(eventsPhase2);
            manag.AddRepresentationForJournal(journalPhrasesUp[Random.Range(0,journalPhrasesUp.Count)]);
            return 1;
        }
        else if(ancientValue > valeur && valeur < seuilBas)
        {
            MonstreEventManager.RemoveEvent(eventsPhase2);
            manag.AddRepresentationForJournal(journalPhrasesDown[Random.Range(0, journalPhrasesDown.Count)]);
            return -1;
        }
        return 0;
    }
}

[CreateAssetMenu(fileName = "Communaute", menuName = "Create Communauté")]
public class Communaute : ScriptableObject
{
    public Representation repulsion, agressivite, jalousie, desir, acceptation, pitie;
    public Event goodEnding, badEnding;

    public string GetHighestRepresentation(out int nb)
    {
        nb = pitie.valeur;
        string toReturn = "Pitie";
        if(acceptation.valeur >= nb)
        {
            nb = acceptation.valeur;
            toReturn = "Acceptation";
        }
        if (desir.valeur >= nb)
        {
            nb = desir.valeur;
            toReturn = "Desir";
        }
        if (jalousie.valeur >= nb)
        {
            nb = jalousie.valeur;
            toReturn = "Jalousie";
        }
        if (agressivite.valeur >= nb)
        {
            nb = agressivite.valeur;
            toReturn = "Agressivite";
        }
        if (repulsion.valeur >= nb)
        {
            nb = repulsion.valeur;
            toReturn = "Repulsion";
        }
        return toReturn;
    }
}
