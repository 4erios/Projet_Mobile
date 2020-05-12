using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Representation
{
    public float valeur, seuilHaut, seuilBas, maxBas, maxHaut;
    public List<Event> eventsPhase2;
    public List<string> journalPhrasesUp, journalPhrasesDown;
    bool firstState = true;

    public float AddPoint(float value, MonstreEventManager manag) // Ajoute/Enlève les points de la Représentation. Renvoie un Bool qui dit s'il y a besoin du Journal ou pas
    {
        float ancientValue = valeur;
        valeur += value;

        if(valeur < maxBas)
        {
            valeur = maxBas;
        }
        else if(valeur > maxHaut)
        {
            valeur = maxHaut;
        }

        if(ancientValue < valeur && valeur > seuilHaut && journalPhrasesUp.Count>0 && firstState)
        {
            firstState = false;
            MonstreEventManager.AddEvent(eventsPhase2);
            manag.AddRepresentationForJournal(journalPhrasesUp[Random.Range(0,journalPhrasesUp.Count)]);
            return 1;
        }
        else if(ancientValue > valeur && valeur < seuilBas && journalPhrasesDown.Count > 0 && !firstState)
        {
            firstState = true;
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
    public EndEvent goodEnding, badEnding;

    public Communaute communauteEnnemie;
    public float coef;

    public string GetHighestRepresentation(out float nb)
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
