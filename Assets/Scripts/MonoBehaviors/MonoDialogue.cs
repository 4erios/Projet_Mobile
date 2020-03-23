using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoDialogue : MonoBehaviour
{
    [SerializeField]
    private Text zoneText;
    [SerializeField]
    private MonstreEventManager manager;
    [SerializeField]
    private CartesManager cardManager;

    public void ShowDialogue(string dial)
    {
        zoneText.text = dial;
    }

    public void GetCard(EmotionMonstre carte)
    {
        cardManager.canPlayCards = false;
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
                cardManager.canPlayCards = false;
            }
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                NextDialogue();
            }
        }
    }

    public void NextDialogue()
    {
        if(manager.needTap)
        {
            manager.EndEvent();
            manager.needTap = false;
        }
    }
}
