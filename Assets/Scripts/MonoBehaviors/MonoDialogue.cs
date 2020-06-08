using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoDialogue : MonoBehaviour
{
    [SerializeField]
    private Text zoneText;
    [SerializeField]
    private Image spriteCommu;
    [SerializeField]
    private GameObject zoneAccroche;
    [SerializeField]
    private Text textAccroche;
    [SerializeField]
    private MonstreEventManager manager;
    [SerializeField]
    private CartesManager cardManager;
    [SerializeField]
    private GameObject animCard;

    private bool isDialShowing;
    private bool isResponseDial;
    private string dial;
    private Communaute commuSprite;
    [SerializeField]
    private int speed;
    [SerializeField]
    private Sprite spriteVide;


    public void ShowAccroche(string txt, string dialog, Communaute commu)
    {
        commuSprite = commu;
        dial = dialog;
        zoneAccroche.SetActive(true);
        textAccroche.text = txt;
    }

    public void ShowDialogue(string dialogue, bool reponseDial, Communaute commu)
    {
        isResponseDial = reponseDial;
        dial = dialogue;
        commuSprite = commu;
        if(commu!=null)
        {
            spriteCommu.sprite = commu.dialCom;
        }
        else
        {
            spriteCommu.sprite = spriteVide;
        }
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

        for (int i = 0; i < dial.Length; i += speed)
        {
            for (int j = 0; j < speed; j++)
            {
                if (i + j < dial.Length)
                {
                    tmpStr += dial[i + j];
                }
            }
            zoneText.text = tmpStr;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        isDialShowing = false;
        if (!isResponseDial)
        {
            cardManager.canPlayCards = true;
            cardManager.greyCard.SetActive(false);
        }
    }

    public void GetCard(EmotionMonstre carte)
    {
        animCard.SetActive(true);
        cardManager.canPlayCards = false;
        cardManager.greyCard.SetActive(true);
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
            Debug.Log("TestInput ?");
            Touch touch = Input.GetTouch(0);
            Vector2 touchStartPos = cardManager.cam.ScreenToWorldPoint(touch.position);
            RaycastHit2D hitinfo = Physics2D.Raycast(touchStartPos, cardManager.cam.transform.forward);
            if (hitinfo.collider == null || hitinfo.collider.tag != "Settings")
            {
                Debug.Log(hitinfo.collider);
                if (touch.phase == TouchPhase.Began)
                {
                    DoOnDown();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchStartPos = cardManager.cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitinfo = Physics2D.Raycast(touchStartPos, cardManager.cam.transform.forward);
            if (hitinfo.collider == null || hitinfo.collider.tag != "Settings")
            {
                DoOnDown();
            }
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
                    cardManager.greyCard.SetActive(false);
                }
            }
            else if (zoneAccroche.activeSelf)
            {
                manager.AccrocheShowed();
                ShowDialogue(dial, false, commuSprite);
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
