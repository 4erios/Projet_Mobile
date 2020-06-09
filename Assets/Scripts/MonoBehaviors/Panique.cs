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
            return "PEOPLE ARE SCARED, THE MONSTER IS HERE";
        }
        else if (value >= 0.8f * maxValue)
        {
            return "KEEP CALM, SOME PEOPLES ARE HUNTING THE MONSTER";
        }
        else if (value >= 0.5f * maxValue)
        {
            return "A CREATURE IN THE CITY ? SOME PEOPLE CLAIM THEY SAW IT";
        }
        else
        {
            return "THERE ARE SOME RUMORS ABOUT A CREATURE IN THE CITY";
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
