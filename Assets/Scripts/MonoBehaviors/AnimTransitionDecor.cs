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
        #region Mise en place du nouveau fond
        Debug.Log("Journal Count : "+journalCount);
        if (ancienLieu != actualEvent.lieux && journalCount > 0)
        {
            Transition();
        }
        else
        {
            EndTransition();
        }
        #endregion
    }

    void Transition()
    {
        anim.enabled = true;
    }

    public void EndTransition()
    {
        if (ancienLieu != actualEvent.lieux)
        {
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
        ancienLieu = actualEvent.lieux;
        manager.StartEvent();
    }

    public void HideTransition()
    {
        anim.enabled = false;
    }
}
