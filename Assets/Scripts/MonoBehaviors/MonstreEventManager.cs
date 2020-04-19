using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonstreEventManager : MonoBehaviour
{
    [Header("Pour le GD")]
    [SerializeField]
    private List<Event> eventsDepart;
    [SerializeField]
    private List<Communaute> commus;
    [SerializeField]
    private List<GameObject> fonds;

    [Header("Pour la Prog - Pas Touche")]
    [SerializeField]
    private MonoPersonnage monoPerso;
    [SerializeField]
    private MonoDialogue monoDial;
    [SerializeField]
    private MonoJournal monoJourn;
    [SerializeField]
    private CartesManager cardManager;
    [SerializeField]
    private MonoEndAffichage monoEnd;

    private static List<Event> eventsPool;
    public List<Event> futurEvents;

    private Event actualEvent;
    [HideInInspector]
    public bool needTap;

    private List<string> representationForJournal = new List<string>();

    public int monstreHp = 3;
    private int nbDeadPeople;
    [SerializeField]
    private List<Sprite> blessuresSprites;
    [SerializeField]
    private Image affichageBlessure;
    [SerializeField]
    private EndEvent finCarnage;

    [SerializeField]
    private List<Event> allEvents;

    private int saveCount;

    void Awake()
    {
        //Load Communautés
        SaveLoadSystem.LoadCommunauteScore(commus);

        //Load Game State
        string eventName = "";
        List<string> pool = new List<string>();
        int paniqueToAdd = 0;
        int damage = 0;
        bool didLoad = SaveLoadSystem.LoadGameState(out eventName, out paniqueToAdd, out damage, out pool);

        futurEvents = new List<Event>();
        if (didLoad)
        {
            Panique.AddPanic(paniqueToAdd);
            GetDamage(monstreHp-damage);

            foreach (string name in pool)
            {
                foreach (Event evt in allEvents)
                {
                    if (evt.name == name)
                    {
                        eventsDepart.Add(evt);
                    }
                    if(evt.name == eventName)
                    {
                        futurEvents.Add(evt);
                    }
                    break;
                }
            }
        }
        eventsPool = eventsDepart;
    }

    private void Start()
    {
        int start = 0;
        if(futurEvents.Count>0 && futurEvents[0]!=null)
        {
            start = 1;
        }
        for(int i = start; i < 5; i++) //Voir pour rajouter les Règles de Choix des Events en début de Partie
        {
            Event newEvent = eventsPool[Random.Range(0, eventsPool.Count)];
            while (futurEvents.Contains(newEvent))
            {
                newEvent = eventsPool[Random.Range(0, eventsPool.Count)];
            }
            futurEvents.Add(newEvent);
        }
        StartNextEvent(futurEvents[0]);
    }

    private void Update() //Tests rapide pour PC
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Test-1");
            EndEvent();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetResponse(CartesManager.cards[0]);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetResponse(CartesManager.cards[1]);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetResponse(CartesManager.cards[2]);
        }
    }

    public static void AddEvent(List<Event> eventToAdd)
    {
        foreach(Event evt in eventToAdd)
        {
            if (!eventsPool.Contains(evt))
            {
                eventsPool.Add(evt);
            }
        }
    }

    public static void RemoveEvent(List<Event> eventToRemove)
    {
        foreach (Event evt in eventToRemove)
        {
            if (eventsPool.Contains(evt))
            {
                eventsPool.Remove(evt);
            }
        }
    }

    public void StartNextEvent(Event newEvent)
    {
        if (actualEvent == null)
        {
            List<EmotionMonstre> emotionsToAdd = new List<EmotionMonstre>();
            List<EmotionMonstre> keys = new List<EmotionMonstre>();
            foreach(KeyValuePair<EmotionMonstre,int> dict in cardManager.removedCard)  //Problème de modification d'un valeur du dictionaire, il aime pas
            {
                Debug.Log("Test retire");
                keys.Add(dict.Key);
            }
            for (int i = 0; i < cardManager.removedCard.Count; i++)
            {
                cardManager.removedCard[keys[i]] -= 1;
                if (cardManager.removedCard[keys[i]] <= 0)
                {
                    emotionsToAdd.Add(keys[i]);
                }
            }
            foreach (EmotionMonstre emot in emotionsToAdd)
            {
                cardManager.removedCard.Remove(emot);
                cardManager.AddCard(emot);
            }

            //Save de l'Etat actuel du jeu
            actualEvent = newEvent;
            StartEvent(actualEvent);
        }
    }

    void StartEvent(Event eventToStart)
    {
        foreach (GameObject gm in fonds)
        {
            gm.SetActive(false);
        }
        switch (eventToStart.lieux)
        {
            case Lieux.ruelle:
                fonds[0].SetActive(true);
                break;
            case Lieux.pontDeSeine:
                fonds[1].SetActive(true);
                break;
        }
        ChangeSpritePerso(eventToStart.personnage.role.presets[eventToStart.preset].ReactionDepart);
        monoDial.ShowAccroche(eventToStart.accroche, eventToStart.dialogue.dialogueDepart);
        //monoDial.ShowDialogue(eventToStart.dialogue.dialogueDepart);
        //cardManager.canPlayCards = true; //Modifier la position, pour le mettre à la fin de l'afficage du dialogue

        //Mettre le côté Animation des Cartes
        //Augmentation du Score de Panique au fil de la Partie
    }

    public void GetResponse(EmotionMonstre reponse)
    {
        PresetRole rolePreset = actualEvent.personnage.role.presets[actualEvent.preset];
        int damage = 0;
        Reactions emot = rolePreset.GetReact(reponse.emotion, out damage);
        if (reponse.bonusRole.Contains(actualEvent.personnage.role))
        {
            //Réponse Bonus
            ChangeSpritePerso(emot);
            monoDial.ShowDialogue(actualEvent.dialogue.reponseBonus);
            ChangeScoreCommu(actualEvent.personnage.communaute, emot, 1);
        }
        else if (reponse.malusRole.Contains(actualEvent.personnage.role))
        {
            //Réponse Malus
            ChangeSpritePerso(emot);
            monoDial.ShowDialogue(actualEvent.dialogue.reponseMalus);
            ChangeScoreCommu(actualEvent.personnage.communaute, emot, -1);
            //Panique
            Panique.AddPanic(10);
        }
        else
        {
            ChangeSpritePerso(emot);
            ReponseChoice(emot);
            ChangeScoreCommu(actualEvent.personnage.communaute, emot, 0);
            switch (emot)
            {
                case Reactions.degout:
                    Panique.AddPanic(3);
                    break;
                case Reactions.peur:
                    Panique.AddPanic(6);
                    break;
                case Reactions.haine:
                    Panique.AddPanic(3);
                    break;
            }
        }
        needTap = true;
        if(reponse.emotion == actualEvent.killPersonnage)
        {
            Debug.Log("Kill with : " + reponse.emotion);
            KillPersonnage();
        }
        GetDamage(damage);

        if (actualEvent.effetBonus == reponse)
        {
            cardManager.AddCard(actualEvent.carteBonus);
        }
        else if (actualEvent.effetMalus == reponse)
        {
            cardManager.RemoveCard(actualEvent.carteMalus);
        }
    }

    void KillPersonnage()
    {
        actualEvent.personnage.Die();
        nbDeadPeople++;
    }

    private void GetDamage(int dmg)
    {
        //Animation de prise de blessure
        monstreHp -= dmg;
        affichageBlessure.sprite = blessuresSprites[monstreHp];
    }
    
    public void AddRepresentationForJournal(string phrase)
    {
        representationForJournal.Add(phrase);
    }

    public void EndEvent()
    {
        if (actualEvent.eventSuivant != null)
        {
            StartNextEvent(actualEvent.eventSuivant);
        }
        else if (monstreHp <= 0) //Voir pour d'autres manières de finir le jeu
        {
            StartEndEvent(actualEvent.personnage.communaute.badEnding);
        }
        else if(nbDeadPeople>5)
        {
            StartEndEvent(finCarnage);
        }
        else if(commus[0].acceptation.valeur > 10 || commus[0].desir.valeur > 10 || commus[0].pitie.valeur > 10)
        {
            StartEndEvent(commus[0].goodEnding);
        }
        /*else if (commus[1].acceptation.valeur > 10 || commus[1].desir.valeur > 10 || commus[1].pitie.valeur > 10)
        {
            StartEndEvent(commus[1].goodEnding);
        }
        else if (commus[2].acceptation.valeur > 10 || commus[2].desir.valeur > 10 || commus[2].pitie.valeur > 10)
        {
            StartEndEvent(commus[2].goodEnding);
        }
        else if (commus[3].acceptation.valeur > 10 || commus[3].desir.valeur > 10 || commus[3].pitie.valeur > 10)
        {
            StartEndEvent(commus[3].goodEnding);
        }*/
        else if (representationForJournal.Count > 0)
        {
            monoJourn.gameObject.SetActive(true);
            monoJourn.ShowText(representationForJournal[0]);
            representationForJournal.RemoveAt(0);
        }
        else
        {
            futurEvents.Remove(actualEvent);
            actualEvent = null;
            EventChoice();
            StartNextEvent(futurEvents[0]);
        }

        saveCount++;
        if(saveCount >= 3)
        {
            saveCount = 0;
            SaveLoadSystem.SaveGameState(actualEvent, Panique.value, monstreHp, eventsPool);
            SaveLoadSystem.SaveCommunauteScore(commus);
            cardManager.SaveCards();
        }
    }

    public void EndGameEvent()
    {
        if (actualEvent.eventSuivant != null)
        {
            StartNextEvent(actualEvent.eventSuivant);
        }
        else
        {
            actualEvent = null;
            
        }
    }

    void EventChoice()
    {
        //Règles de Choix d'un événement
        futurEvents.Add(futurEvents[3]);
        int count = 0;
        while (futurEvents[4] == futurEvents[3] && count < 100)
        {
            count++;
            Event newEvent = eventsPool[Random.Range(0, eventsPool.Count)];
            List<int> compteCommu = new List<int>();
            for(int k = 0; k < 4; k++)
            {
                compteCommu.Add(0);
            }
            if (futurEvents.Contains(newEvent)) // Vérification de Doublon
            {
                goto continueWhile;
            }
            if(!newEvent.personnage.isAlive) //Vérifie si le personnage est en vie
            {
                goto continueWhile;
            }
            #region Communautés
            if (count < 75)
            {
                for (int i = 0; i < 5; i++)
                {
                    switch (futurEvents[i].personnage.communaute.name)
                    {
                        case "Dirigeants":
                            compteCommu[0]++;
                            break;
                        case "Habitants":
                            compteCommu[1]++;
                            break;
                        case "Mendiants":
                            compteCommu[2]++;
                            break;
                        case "Représentants de l'ordre":
                            compteCommu[3]++;
                            break;
                    }
                }
            }

            switch (newEvent.personnage.communaute.name) //A remodifier après la Slice
            {
                case "Dirigeants":
                    if (compteCommu[0] > 4)
                    {
                        goto continueWhile;
                    }
                    break;
                case "Habitants":
                    if (compteCommu[1] > 4)
                    {
                        goto continueWhile;
                    }
                    break;
                case "Mendiants":
                    if (compteCommu[2] > 4)
                    {
                        goto continueWhile;
                    }
                    break;
                case "Représentants de l'ordre":
                    if (compteCommu[3] > 4)
                    {
                        goto continueWhile;
                    }
                    break;
            }
            #endregion
            for (int i = 0; i < 4; i++) // Vérification d'un Event spécial présent
            {
                if(futurEvents[i].eventSuivant != null)
                {
                    goto continueWhile;
                }
            }
            futurEvents[4] = newEvent;
            continueWhile:;
        }
    }

    void StartEndEvent(EndEvent eventToEnd)
    {
        SaveLoadSystem.ResetGameSate();
        monoEnd.showEnd.SetActive(true);
        Debug.Log(eventToEnd);
        if (eventToEnd.imageFin != null)
        {
            monoEnd.fond.sprite = eventToEnd.imageFin;
        }
        monoEnd.text.text = eventToEnd.texteFin;
        monoEnd.canEnd = true;
    }

    void ReponseChoice(Reactions react)
    {
        switch (react)
        {
            case Reactions.interet:
                monoDial.ShowDialogue(actualEvent.dialogue.reponseInteret);
                break;
            case Reactions.affection:
                monoDial.ShowDialogue(actualEvent.dialogue.reponseAffection);
                break;
            case Reactions.compassion:
                monoDial.ShowDialogue(actualEvent.dialogue.reponseCompassion);
                break;
            case Reactions.degout:
                monoDial.ShowDialogue(actualEvent.dialogue.reponseDegout);
                break;
            case Reactions.peur:
                monoDial.ShowDialogue(actualEvent.dialogue.reponsePeur);
                break;
            case Reactions.haine:
                monoDial.ShowDialogue(actualEvent.dialogue.reponseHaine);
                break;
        }
    }

    void ChangeSpritePerso(Reactions react)
    {
        switch (react)
        {
            case Reactions.interet:
                monoPerso.ChangeSprite(actualEvent.personnage.spriteReaction[0]);
                break;
            case Reactions.affection:
                monoPerso.ChangeSprite(actualEvent.personnage.spriteReaction[1]);
                break;
            case Reactions.compassion:
                monoPerso.ChangeSprite(actualEvent.personnage.spriteReaction[2]);
                break;
            case Reactions.degout:
                monoPerso.ChangeSprite(actualEvent.personnage.spriteReaction[3]);
                break;
            case Reactions.peur:
                monoPerso.ChangeSprite(actualEvent.personnage.spriteReaction[4]);
                break;
            case Reactions.haine:
                monoPerso.ChangeSprite(actualEvent.personnage.spriteReaction[5]);
                break;
        }
    }

    void ChangeScoreCommu(Communaute commuPerso, Reactions react, int bonus)
    {
        
        switch (react)
        {
            case Reactions.interet:
                if (bonus >= 0)
                {
                    commuPerso.jalousie.AddPoint(1,this);
                    commuPerso.desir.AddPoint(1, this);
                }
                if(bonus <= 0)
                {
                    commuPerso.repulsion.AddPoint(-1, this);
                    commuPerso.pitie.AddPoint(-1, this);
                }
                break;
            case Reactions.affection:
                if (bonus >= 0)
                {
                    commuPerso.acceptation.AddPoint(1, this);
                    commuPerso.desir.AddPoint(1, this);
                }
                if (bonus <= 0)
                {
                    commuPerso.repulsion.AddPoint(-1, this);
                    commuPerso.agressivite.AddPoint(-1, this);
                }
                break;
            case Reactions.compassion:
                if (bonus >= 0)
                {
                    commuPerso.acceptation.AddPoint(1, this);
                    commuPerso.pitie.AddPoint(1, this);
                }
                if (bonus <= 0)
                {
                    commuPerso.jalousie.AddPoint(-1, this);
                    commuPerso.agressivite.AddPoint(-1, this);
                }
                break;
            case Reactions.degout:
                if (bonus >= 0)
                {
                    commuPerso.repulsion.AddPoint(1, this);
                    commuPerso.pitie.AddPoint(1, this);
                }
                if (bonus <= 0)
                {
                    commuPerso.jalousie.AddPoint(-1, this);
                    commuPerso.desir.AddPoint(-1, this);
                }
                break;
            case Reactions.peur:
                if (bonus >= 0)
                {
                    commuPerso.repulsion.AddPoint(1, this);
                    commuPerso.agressivite.AddPoint(1, this);
                }
                if (bonus <= 0)
                {
                    commuPerso.acceptation.AddPoint(-1, this);
                    commuPerso.desir.AddPoint(-1, this);
                }
                break;
            case Reactions.haine:
                if (bonus >= 0)
                {
                    commuPerso.jalousie.AddPoint(1, this);
                    commuPerso.agressivite.AddPoint(1, this);
                }
                if (bonus <= 0)
                {
                    commuPerso.acceptation.AddPoint(-1, this);
                    commuPerso.pitie.AddPoint(-1, this);
                }
                break;
        }
    }
}
