﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartesManager : MonoBehaviour
{
    public static List<EmotionMonstre> cards;

    private List<MonoCartes> cartesJoueur = new List<MonoCartes>();

    [Header("Pour le GD")]
    [SerializeField]
    private List<EmotionMonstre> cartesBases;
    [SerializeField]
    private List<EmotionMonstre> cartesObligatoire;

    [Header("Pour la Prog (Pas touche)")]
    public Camera cam;
    [SerializeField]
    private float distanceBetweenCards;
    [SerializeField]
    private Vector2 positionCentrale;
    [SerializeField]
    private GameObject prefabCarte;
    [SerializeField]
    private Transform zoneCarte;
    [SerializeField]
    private RectTransform dialogueTransf;
    public bool canPlayCards;

    private Vector2 touchStartPos = Vector2.zero;
    private bool isTouch, movingCard, movingList;
    private int mainCard;
    private Vector2 currentPosition, lastTouchPosition;

    public Dictionary<EmotionMonstre, int> removedCard = new Dictionary<EmotionMonstre, int>();

    private Vector2 screenSize;

    private Vector3 pos;

    [SerializeField]
    private GameObject fleche;

    private float showFleche;

    public GameObject greyCard;
    public AnimationCarteJouee feedbackCard;

   /* [SerializeField]
    private GameObject feedbackCardPlayed;
    [SerializeField]
    private SpriteRenderer feebackMonstre, feedbackTitre;*/

    private void Awake()
    {
        cards = new List<EmotionMonstre>();
        string[] cardName = SaveLoadSystem.LoadCards();
        foreach(string name in cardName)
        {
            foreach(EmotionMonstre em in cartesBases)
            {
                if(em.name == name)
                {
                    AddCard(em);
                    break;
                }
            }
        }
        //Mettre les RemovedCards
    }

    private void Start()
    {
        /*while(cards.Count < 3)
        {
            EmotionMonstre cart = cartesBases[Random.Range(0, cartesBases.Count)];
            if(!cards.Contains(cart))
            {
                AddCard(cart);
            }
        }*/
        int rndCard = Random.Range(0, 6);
        foreach(EmotionMonstre emot in cartesObligatoire)
        {
            AddCard(emot);
        }
        for(int i = 0; i< 3; i++)
        {
            EmotionMonstre cart = cartesBases[(i+rndCard)% cartesBases.Count];
            if (!cards.Contains(cart))
            {
                AddCard(cart);
            }
            else
            {
                i--;
                rndCard++;
            }
        }
        /*screenSize = new Vector2(660, Screen.height);
        screenSize = cam.ScreenToWorldPoint(screenSize);*/
        ChangeAnimatorState(true);
    }

    private void Update()
    {
        if (canPlayCards)
        {
            showFleche += Time.deltaTime;
            if (showFleche >= 3f && !fleche.activeSelf)
            {
                fleche.SetActive(true);
            }
        }
        if(Input.touchCount > 0 && canPlayCards)
        {

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = cam.ScreenToWorldPoint(touch.position);
                RaycastHit2D hitinfo = Physics2D.Raycast(touchStartPos, cam.transform.forward);
                if(hitinfo.collider != null && hitinfo.collider.name == "ZoneCarte")
                {
                    isTouch = true;
                    ChangeAnimatorState(false);
                }
                touchStartPos = touch.position;
            }

            if(isTouch && !(movingCard || movingList))
            {
                if (fleche.activeSelf)
                {
                    fleche.GetComponent<Animator>().Play("FlècheTutoDepop");
                }
                showFleche = 0;
                if (Mathf.Abs(touch.position.x - touchStartPos.x) > 100) //Fait bouger la liste de carte
                {
                    movingList = true;
                }
                else if(Mathf.Abs(touch.position.y - touchStartPos.y) > 70 && touch.position.x - positionCentrale.x < 360 + distanceBetweenCards / 2 && touch.position.x - positionCentrale.x > 360 - distanceBetweenCards / 2 && !removedCard.ContainsKey(cards[mainCard])) //Fait bouger la carte principale
                {
                    movingCard = true;
                    cartesJoueur[mainCard].colid.enabled = true;
                }
                else if(touch.phase == TouchPhase.Ended)
                {
                    ChangeAnimatorState(true);
                    isTouch = false;
                }
            }
            else if (movingList)
            {
                DeplacementCartes(touch.position.x - lastTouchPosition.x);
            }
            else if(movingCard)
            {
                pos = cam.ScreenToWorldPoint(new Vector3(0, touch.position.y, 0));
                pos = new Vector3(0, pos.y, 0);
                if(pos.y < -1.5f && pos.y > -7.4f)
                {
                    cartesJoueur[mainCard].transform.position = new Vector3(0, pos.y, 0);
                    pos = cartesJoueur[mainCard].transform.position;
                }
                if ((pos.y < dialogueTransf.position.y + 13 && pos.y > dialogueTransf.position.y - 4 / 2))
                {
                    if (touch.phase == TouchPhase.Ended && isTouch)
                    {
                        ChangeAnimatorState(true);
                       /* feedbackCardPlayed.SetActive(true);
                        feebackMonstre.sprite = cards[mainCard].sprite;
                        feedbackTitre.sprite = cards[mainCard].titre;*/
                        dialogueTransf.parent.GetComponent<MonoDialogue>().GetCard(cartesJoueur[mainCard].emotion);
                        feedbackCard.DissolveNew(1, cartesJoueur[mainCard].spriteNoir.sprite, cartesJoueur[mainCard].fond.sprite, cartesJoueur[mainCard].titre.sprite);
                    }
                }
            }

            lastTouchPosition = touch.position;

            if (touch.phase == TouchPhase.Ended && isTouch)
            {
                ChangeAnimatorState(true);
                isTouch = false;
                movingCard = false;
                movingList = false;
                ResetCardPosition();
            }
        }
        if(isTouch && !canPlayCards)
        {
            isTouch = false;
        }

        if(Input.GetMouseButtonDown(0) && canPlayCards)
        {
            touchStartPos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitinfo = Physics2D.Raycast(touchStartPos, cam.transform.forward);
            if (hitinfo.collider != null && hitinfo.collider.name == "ZoneCarte")
            {
                if (fleche.activeSelf)
                {
                    fleche.GetComponent<Animator>().Play("FlècheTutoDepop");
                }
                showFleche = 0;
                ChangeAnimatorState(false);
                isTouch = true;
            }
            touchStartPos = Input.mousePosition;
        }
        else if(Input.GetMouseButton(0))
        {
            if (isTouch && !(movingCard || movingList))
            {
                if (Mathf.Abs(Input.mousePosition.x - touchStartPos.x) > 100) //Fait bouger la liste de carte
                {
                    movingList = true;
                }
                else if (Mathf.Abs(Input.mousePosition.y - touchStartPos.y) > 70 && !removedCard.ContainsKey(cards[mainCard])) //Fait bouger la carte principale
                {
                    movingCard = true;
                    cartesJoueur[mainCard].colid.enabled = true;
                }
                else if(Input.GetMouseButtonUp(0))
                {
                    ChangeAnimatorState(true);
                    isTouch = false;
                }
            }
            else if (movingList)
            {
                DeplacementCartes(Input.mousePosition.x - lastTouchPosition.x);
            }
            else if (movingCard)
            {
                /*cartesJoueur[mainCard].transform.position = cam.ScreenToWorldPoint(new Vector3(0, Input.mousePosition.y, 0));
                cartesJoueur[mainCard].transform.position = new Vector3(0, cartesJoueur[mainCard].transform.position.y, 0);
                pos = cartesJoueur[mainCard].transform.position;*/
                pos = cam.ScreenToWorldPoint(new Vector3(0, Input.mousePosition.y, 0));
                pos = new Vector3(0, pos.y, 0);
                if (pos.y < -1.5f && pos.y > -7.4f)
                {
                    cartesJoueur[mainCard].transform.position = new Vector3(0, pos.y, 0);
                    pos = cartesJoueur[mainCard].transform.position;
                }
                /*if ((pos.x < dialogueTransf.position.x + 9 && pos.x > dialogueTransf.position.x - 9) && (pos.y < dialogueTransf.position.y + 20 && pos.y > dialogueTransf.position.y - 4 / 2))
                {
                    Debug.Log("Test Card placement");
                    if (Input.GetMouseButtonUp(0) && isTouch)
                    {
                        Debug.Log("Test Here");
                        dialogueTransf.parent.GetComponent<MonoDialogue>().GetCard(cartesJoueur[mainCard].emotion);
                    }
                }*/
            }

            lastTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && isTouch)
        {
            ChangeAnimatorState(true);
            Vector3 pos = cartesJoueur[mainCard].transform.position;
            if ((pos.y < dialogueTransf.position.y + 13 && pos.y > dialogueTransf.position.y - 4 / 2))
            {
                /* feedbackCardPlayed.SetActive(true);
                        feebackMonstre.sprite = cards[mainCard].sprite;
                        feedbackTitre.sprite = cards[mainCard].titre;*/
                dialogueTransf.parent.GetComponent<MonoDialogue>().GetCard(cartesJoueur[mainCard].emotion);
                feedbackCard.DissolveNew(1, cartesJoueur[mainCard].spriteNoir.sprite, cartesJoueur[mainCard].fond.sprite, cartesJoueur[mainCard].titre.sprite);
            }
            isTouch = false;
            movingCard = false;
            movingList = false;
            ResetCardPosition();
        }
    }

    public void AddCard(EmotionMonstre cardToAdd)
    {
        if(!cards.Contains(cardToAdd))
        {
            cards.Add(cardToAdd);
            MonoCartes newCard = Instantiate(prefabCarte, Vector3.zero, Quaternion.identity, zoneCarte).GetComponent<MonoCartes>();
            StartCoroutine(newCard.Dissolve(0f));
            newCard.transform.localPosition = Vector3.zero;
            newCard.emotion = cardToAdd;
            cartesJoueur.Add(newCard);
            mainCard = cartesJoueur.Count - 1;
            ResetCardPosition();
            //Animation de carte débloquée
        }
    }

    public void RemoveCard(EmotionMonstre cardToRemove)
    {
        if (cards.Contains(cardToRemove) && !removedCard.ContainsKey(cardToRemove))
        {
            removedCard.Add(cardToRemove, 2);

            //Animation de carte bloquée

            foreach (MonoCartes cart in cartesJoueur)
            {
                if (cart.emotion == cardToRemove)
                {
                    cart.blockedCardAnimator.Play("AnimDeblocageCarte");
                    //Animation
                }
            }
            /*
            cards.Remove(cardToRemove);
            MonoCartes monoCardToRemove = default;
            foreach(MonoCartes cart in cartesJoueur)
            {
                if(cart.emotion == cardToRemove)
                {
                    monoCardToRemove = cart;
                }
            }
            cartesJoueur.Remove(monoCardToRemove);
            */
            //ResetCardPosition();
        }
    }

    public void EndRemoved(EmotionMonstre card)
    {
        removedCard.Remove(card);

        foreach (MonoCartes cart in cartesJoueur)
        {
            if (cart.emotion == card)
            {
                Debug.Log("Deblock");
                cart.blockedCardAnimator.Play("AnimBlocageCarte");
            }
        }
    }

    void ResetCardPosition()
    {
        cartesJoueur[mainCard].transform.localPosition = new Vector3(positionCentrale.x, positionCentrale.y, cartesJoueur[mainCard].transform.localPosition.z);
        currentPosition = positionCentrale;
        cartesJoueur[mainCard].colid.enabled = false;
        for (int i = 1; i < cartesJoueur.Count; i++)
        {
            //if (i != mainCard) // Voir pour modifier ça, histoire de toujours avec le même ordre
            {
                float newX = currentPosition.x + distanceBetweenCards;
                if (newX > distanceBetweenCards * cartesJoueur.Count / 2)
                {
                    newX -= distanceBetweenCards * cartesJoueur.Count;
                }
                currentPosition = new Vector2(newX, currentPosition.y);
                cartesJoueur[(mainCard+i)% cartesJoueur.Count].transform.localPosition = currentPosition;
                cartesJoueur[(mainCard + i) % cartesJoueur.Count].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            cartesJoueur[mainCard].transform.localScale = new Vector3(1f, 1f, 1f);
            cartesJoueur[i].colid.enabled = false;
        }
        DeplacementCartes(0);
    }

    void DeplacementCartes(float speed)
    {
        for(int i = 0; i < cartesJoueur.Count; i++)
        {
            cartesJoueur[i].transform.localPosition += new Vector3(1, 0, 0) * speed;
            if(Vector2.Distance(cartesJoueur[i].transform.localPosition, positionCentrale) < distanceBetweenCards/2)
            {
                cartesJoueur[i].transform.localScale = (Vector3.one*0.9f) * (1 - (0.3f * (Vector2.Distance(cartesJoueur[i].transform.localPosition, positionCentrale) / (distanceBetweenCards / 2))));
                cartesJoueur[i].ChangeLayerLevel(20);
                mainCard = i;
            }
            else
            {
                if(Vector2.Distance(cartesJoueur[i].transform.localPosition, positionCentrale) <= 3*distanceBetweenCards / 2)
                {
                    cartesJoueur[i].ChangeLayerLevel(15);
                }
                else if(Vector2.Distance(cartesJoueur[i].transform.localPosition, positionCentrale) <= 5 * distanceBetweenCards / 2)
                {
                    cartesJoueur[i].ChangeLayerLevel(10);
                }
                else
                {
                    cartesJoueur[i].ChangeLayerLevel(7);
                }
                cartesJoueur[i].transform.localScale = Vector3.one * 0.7f;

            }
            currentPosition = cartesJoueur[i].transform.localPosition;
            float newX = currentPosition.x;
            if (newX > distanceBetweenCards * cartesJoueur.Count / 2)
            {
                newX -= distanceBetweenCards * cartesJoueur.Count;
            }
            else if (newX < -distanceBetweenCards * cartesJoueur.Count / 2)
            {
                newX += distanceBetweenCards * cartesJoueur.Count;
            }
            currentPosition = new Vector2(newX, currentPosition.y);
            cartesJoueur[i].transform.localPosition = currentPosition;
        }
    }

    void ChangeAnimatorState(bool state)
    {
        cartesJoueur[mainCard].spriteNoir.GetComponent<Animator>().enabled = state;
    }

    public void SaveCards()
    {
        SaveLoadSystem.SaveCards(cards);
        SaveLoadSystem.SaveRemovedCards(removedCard);
    }
}
