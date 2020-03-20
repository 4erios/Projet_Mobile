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
    private Image paniqueSliderDroite,paniqueSliderGauche;

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
        //Mettre à jour l'affichage
        paniqueSliderDroite.fillAmount = value / 100f;
        paniqueSliderGauche.fillAmount = value / 100f;
        Debug.Log("La panique est à : " + paniqueSliderDroite.fillAmount);
    }
}
