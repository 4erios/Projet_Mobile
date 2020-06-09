using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Panique : MonoBehaviour
{
    public static float value, maxValue;
    [SerializeField]
    private List<Event> middleGameEvent, lateGameEvent;
    private static List<Event> midEvent, lateEvent;
    private static UnityEvent addPanicEvent = new UnityEvent();
    [SerializeField]
    private float max;

    private void Awake()
    {
        midEvent = middleGameEvent;
        lateEvent = lateGameEvent;
        maxValue = max;
        //Mettre à jour l'affichage de la panique
    }

    [ContextMenu("AddPoint")]
    public void AddPointTest()
    {
        AddPanic(10);
    }

    public static string JournalPanique()
    {
        if (maxValue == 0)
        {
            maxValue = 10;
        }
        if(value >= maxValue)
        {
            return "People are scared, the Monster is here.";
        }
        else if (value >= 0.8f * maxValue)
        {
            return "Keep calm, some peoples are hunting the Monster.";
        }
        else if (value >= 0.5f * maxValue)
        {
            return "A creature in the city ? Some people claim they saw it.";
        }
        else
        {
            return "There are some rumors about a creature in the city.";
        }
    }

    public static void AddPanic(float valueToAdd)
    {
        if(maxValue==0)
        {
            maxValue = 10;
        }
        value += valueToAdd;
        if(value >= 0.8f* maxValue)
        {
            MonstreEventManager.AddEvent(lateEvent);
        }
        else if(value >= 0.5f* maxValue)
        {
            MonstreEventManager.AddEvent(midEvent);
        }
        addPanicEvent.Invoke();
    }
}
