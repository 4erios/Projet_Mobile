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

    [SerializeField]
    private AudioClip feedbackJournal;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                //gameObject.SetActive(false);
                manag.RemoveFromJournal();
                manag.EndEvent();
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            //gameObject.SetActive(false);
            manag.RemoveFromJournal();
            Debug.Log("Test EndJournal");
            if(endJournal)
            {
                manag.AudioFeedback(feedbackJournal);
                Close();
            }
            manag.EndEvent();
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
