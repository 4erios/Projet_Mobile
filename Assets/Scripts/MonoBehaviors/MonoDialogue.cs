using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoDialogue : MonoBehaviour
{
    [SerializeField]
    private Text zoneText;
    [SerializeField]
    private GameObject zoneAccroche;
    [SerializeField]
    private Text textAccroche;
    [SerializeField]
    private MonstreEventManager manager;
    [SerializeField]
    private CartesManager cardManager;
    private bool isDialShowing;
    private bool isResponseDial;
    private string dial;

    public void ShowAccroche(string txt, string dialog)
    {
        dial = dialog;
        zoneAccroche.SetActive(true);
        textAccroche.text = txt;
    }

    public void ShowDialogue(string dialogue, bool reponseDial)
    {
        isResponseDial = reponseDial;
        dial = dialogue;
        StartCoroutine(AffichageDialogue());
    }

    public void SupprDial()
    {
        zoneText.text = "";
    }

    IEnumerator AffichageDialogue()
    {
        isDialShowing = true;
        string tmpStr = "";
        for(int i = 0; i < dial.Length; i+=3)
        {
            tmpStr += dial[i];
            if (i + 1 < dial.Length)
            {
                tmpStr += dial[i+1];
            }
            if (i + 2 < dial.Length)
            {
                tmpStr += dial[i+2];
            }
            zoneText.text = tmpStr;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        isDialShowing = false;
        if (!isResponseDial)
        {
            cardManager.canPlayCards = true;
        }
    }

    public void GetCard(EmotionMonstre carte)
    {
        cardManager.canPlayCards = false;
        //Mettre les faces caché des cartes
        manager.GetResponse(carte);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (collision.tag == "Card" && touch.phase == TouchPhase.Ended)
            {
                GetCard(collision.GetComponent<MonoCartes>().emotion);
            }
        }

        if (collision.tag == "Card" && Input.GetMouseButtonUp(0))
        {
            GetCard(collision.GetComponent<MonoCartes>().emotion);
        }
    }

    float time = 0;

    private void Update() //En faire un Enumerator, qui n'est appelé qu'après un certains temps après avoir joué la carte  ??
    {
        time += Time.deltaTime;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                DoOnDown();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            DoOnDown();
        }
    }

    void DoOnDown()
    {
        if (time > 0.5f)
        {
            if (isDialShowing)
            {
                StopAllCoroutines();
                zoneText.text = dial;
                isDialShowing = false;
                if (!isResponseDial)
                {
                    cardManager.canPlayCards = true;
                }
            }
        
            if (zoneAccroche.activeSelf)
            {
                manager.AccrocheShowed();
                ShowDialogue(dial, false);
                zoneAccroche.SetActive(false);
            }
            else
            {
                NextDialogue();
            }
            time = 0;
        }
    }

    public void NextDialogue()
    {
        if(manager.needTap)
        {
            //manager.EndEvent();
            manager.DisparitionPerso();
            manager.needTap = false;
        }
    }
}
