using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Panique : MonoBehaviour
{
    public static int value;
    [SerializeField]
    private List<Event> middleGameEvent, lateGameEvent;
    private static List<Event> midEvent, lateEvent;
    private static UnityEvent addPanicEvent = new UnityEvent();
    [SerializeField]
    private Image paniqueAffichage;
    [SerializeField]
    private List<Sprite> paniqueSprites;

    private void Start()
    {
        midEvent = middleGameEvent;
        lateEvent = lateGameEvent;
        addPanicEvent.AddListener(AffichagePanique);
    }

    [ContextMenu("AddPoint")]
    public void AddPointTest()
    {
        AddPanic(5);
    }

    public static void AddPanic(int valueToAdd)
    {
        value += valueToAdd;
        if(value >= 80)
        {
            MonstreEventManager.AddEvent(lateEvent);
        }
        else if(value >= 50)
        {
            MonstreEventManager.AddEvent(midEvent);
        }
        addPanicEvent.Invoke();
    }

    void AffichagePanique()
    {
        if(value>=100)
        {
            paniqueAffichage.sprite = paniqueSprites[4];
        }
        else if(value>=75)
        {
            paniqueAffichage.sprite = paniqueSprites[3];
        }
        else if (value >= 50)
        {
            paniqueAffichage.sprite = paniqueSprites[2];
        }
        else if (value >= 25)
        {
            paniqueAffichage.sprite = paniqueSprites[1];
        }
        else if (value >= 0)
        {
            paniqueAffichage.sprite = paniqueSprites[0];
        }
    }
}
