using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTransitionDecor : MonoBehaviour
{
    public MonstreEventManager manager;
    [SerializeField]
    private List<GameObject> fonds;
    private Lieux ancienLieu;
    private Event actualEvent;
    [SerializeField]
    private Animator anim;

    public void ChangeDecor(Event newEvent, int journalCount)
    {
        actualEvent = newEvent;
        Debug.Log("Journal cahngement : " + journalCount);
        #region Mise en place du nouveau fond
        if (ancienLieu != actualEvent.lieux && journalCount > 0)
        {
            Transition();
        }
        else
        {
            EndTransition();
        }
        ancienLieu = actualEvent.lieux;
        #endregion
    }

    void Transition()
    {
        anim.enabled = true;
    }

    public void EndTransition()
    {
        manager.StartEvent();
        foreach (GameObject gm in fonds)
        {
            gm.GetComponent<ParallaxeGroup>().HideDecor();
        }
        switch (actualEvent.lieux)
        {
            //Mettre une animation à la place d'un SetActive
            case Lieux.ruelle:
                fonds[0].SetActive(true);
                break;
            case Lieux.pontDeSeine:
                fonds[1].SetActive(true);
                break;
            case Lieux.parc:
                fonds[2].SetActive(true);
                break;
            case Lieux.cabaret:
                fonds[3].SetActive(true);
                break;
        }
    }

    public void HideTransition()
    {
        anim.enabled = false;
    }
}
