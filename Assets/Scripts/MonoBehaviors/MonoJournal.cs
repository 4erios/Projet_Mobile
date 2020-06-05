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
    private bool endJournal;
    private Vector2 beganTapPosition;

    [SerializeField]
    private AudioClip feedbackJournal;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                beganTapPosition = touch.position;
                //gameObject.SetActive(false);

            }

            if(touch.phase == TouchPhase.Ended)
            {
                if(Mathf.Abs(touch.position.y-beganTapPosition.y) <= 200 && touch.position.x - beganTapPosition.x < -100)
                {
                    manag.RemoveFromJournal();
                    if (endJournal)
                    {
                        manag.AudioFeedback(feedbackJournal);
                        Close();
                    }
                    manag.EndEvent();
                }
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            beganTapPosition = Input.mousePosition;
            //gameObject.SetActive(false);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (Mathf.Abs(Input.mousePosition.y - beganTapPosition.y) <= 200 && Input.mousePosition.x - beganTapPosition.x < -100)
            {
                manag.RemoveFromJournal();
                if (endJournal)
                {
                    manag.AudioFeedback(feedbackJournal);
                    Close();
                }
                manag.EndEvent();
            }
        }
    }

    public void ShowText(string text, bool doesEnd)
    {
        endJournal = doesEnd;
        manag.HideCards();
        anim.SetBool("EndJournal", false);
        //Mettre l'apparition du Journal
        //titre.text = text;
        Instantiate(pageJournal, gameObject.transform, true).GetComponent<PageJournal>().ShowJournal(text);
        manag.AudioFeedback(feedbackJournal);
    }

    public void Close()
    {
        animationTransition.SetActive(true);
        //anim.SetBool("EndJournal", true);
    }

    public void Disapear()
    {
        manag.ShowCards();
        gameObject.SetActive(false);
        foreach(GameObject go in pages)
        {
            Destroy(go);
        }
    }
}
