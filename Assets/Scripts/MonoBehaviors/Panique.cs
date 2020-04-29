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
    private Image paniqueAffichage;
    [SerializeField]
    private List<Sprite> paniqueSprites;
    [SerializeField]
    private float max;

    private void Awake()
    {
        midEvent = middleGameEvent;
        lateEvent = lateGameEvent;
        maxValue = max;
        addPanicEvent.AddListener(AffichagePanique);
        //Mettre à jour l'affichage de la panique
    }

    [ContextMenu("AddPoint")]
    public void AddPointTest()
    {
        AddPanic(5);
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

    void AffichagePanique()
    {
        if(value>= maxValue)
        {
            paniqueAffichage.sprite = paniqueSprites[4];
        }
        else if(value>=0.75* maxValue)
        {
            paniqueAffichage.sprite = paniqueSprites[3];
        }
        else if (value >= 0.5* maxValue)
        {
            paniqueAffichage.sprite = paniqueSprites[2];
        }
        else if (value >= 0.25* maxValue)
        {
            paniqueAffichage.sprite = paniqueSprites[1];
        }
        else if (value >= 0)
        {
            paniqueAffichage.sprite = paniqueSprites[0];
        }
    }
}
