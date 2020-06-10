using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoJournal : MonoBehaviour
{
    [SerializeField]
    private Text titre;
    [SerializeField]
    private MonstreEventManager manag;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private GameObject animationTransition;
    [SerializeField]
    private GameObject pageJournal;
    private List<GameObject> pages = new List<GameObject>();
    private bool endJournal, stopFlick;
    private Vector2 beganTapPosition;

    [SerializeField]
    private AudioClip feedbackJournal;

    private void Update()
    {
        /*if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                beganTapPosition = touch.position;
                //gameObject.SetActive(false);

            }

            if(touch.phase == TouchPhase.Ended)
            {
                Debug.Log("Test touch journal");
                if(Mathf.Abs(touch.position.y-beganTapPosition.y) <= 200 && touch.position.x - beganTapPosition.x < -100)
                {
                    Debug.Log("Test journal Flick");
                    manag.RemoveFromJournal();
                    if (endJournal)
                    {
                        Debug.Log("Test end journal");
                        manag.AudioFeedback(feedbackJournal);
                        Close();
                    }
                    manag.EndEvent();
                }
            }
        }*/

        if(Input.GetMouseButtonDown(0))
        {
            beganTapPosition = Input.mousePosition;
            //gameObject.SetActive(false);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (Mathf.Abs(Input.mousePosition.y - beganTapPosition.y) <= 200 && Input.mousePosition.x - beganTapPosition.x < -100 && !stopFlick)
            {
                manag.RemoveFromJournal();
                if (endJournal)
                {
                    //manag.AudioFeedback(feedbackJournal);
                    stopFlick = true;
                    Close();
                }
                manag.EndEvent();
            }
        }
    }

    public void ShowText(string text, bool doesEnd, Sprite commu, string interview)
    {
        if(!animationTransition.activeSelf)
        {
            animationTransition.SetActive(true);
            animationTransition.GetComponent<Animator>().Play("journalTransitionClean_anim", -1, 0);
        }
        endJournal = doesEnd;
        manag.HideCards();
        anim.SetBool("EndJournal", false);
        //Mettre l'apparition du Journal
        //titre.text = text;
        Instantiate(pageJournal, gameObject.transform, true).GetComponent<PageJournal>().ShowJournal(text, commu, interview);
        manag.AudioFeedback(feedbackJournal);
    }

    public void Close()
    {
        animationTransition.GetComponent<Animator>().enabled = true;
        //anim.SetBool("EndJournal", true);
    }

    public void Disapear()
    {
        stopFlick = false;
        manag.ShowCards();
        gameObject.SetActive(false);
        foreach(GameObject go in pages)
        {
            Destroy(go);
        }
    }
}
