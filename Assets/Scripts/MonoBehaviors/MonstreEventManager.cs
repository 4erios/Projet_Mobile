using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstreEventManager : MonoBehaviour
{
    [Header("Pour le GD")]
    [SerializeField]
    private List<Event> eventsDepart;
    [SerializeField]
    private List<Communaute> commus;

    [Header("Pour la Prog - Pas Touche")]
    [SerializeField]
    private MonoPersonnage monoPerso;
    [SerializeField]
    private MonoDialogue monoDial;
    [SerializeField]
    private MonoJournal monoJourn;
    [SerializeField]
    private CartesManager cardManager;

    private static List<Event> eventsPool;
    private List<Event> futurEvents;

    private Event actualEvent;
    [HideInInspector]
    public bool needTap;

    private List<string> representationForJournal;

    public int monstreHp = 3;

    void Awake()
    {
        eventsPool = eventsDepart;
    }

    private void Start()
    {
        futurEvents = new List<Event>();
        for(int i = 0; i < 5; i++) //Voir pour rajouter les Règles de Choix des Events en début de Partie
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
        if(Input.GetKeyDown(KeyCode.D))
        {
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
            foreach(KeyValuePair<EmotionMonstre,int> dict in cardManager.RemovedCard)
            {
                cardManager.RemovedCard[dict.Key] -= 1;
                if(cardManager.RemovedCard[dict.Key] <= 0)
                {
                    emotionsToAdd.Add(dict.Key);
                }
            }
            foreach(EmotionMonstre emot in emotionsToAdd)
            {
                cardManager.RemovedCard.Remove(emot);
                cardManager.AddCard(emot);
            }
            //Save de l'Etat actuel du jeu
            actualEvent = newEvent;
            StartEvent(actualEvent);
        }
    }

    void StartEvent(Event eventToStart)
    {
        //Changement du Décor en fond
        ChangeSpritePerso(eventToStart.personnage.role.presets[eventToStart.preset].ReactionDepart);
        monoDial.ShowDialogue(eventToStart.dialogue.dialogueDepart);
        cardManager.canPlayCards = true;
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
            actualEvent.personnage.Die();
        }

        if(actualEvent.effetBonus == reponse)
        {
            cardManager.AddCard(actualEvent.carteBonus);
        }
        else if (actualEvent.effetMalus == reponse)
        {
            cardManager.RemoveCard(actualEvent.carteMalus);
        }
    }

    private void GetDamage(int dmg)
    {
        monstreHp -= dmg;
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
            EndGameEvent();
        }
        else
        {
            futurEvents.Remove(actualEvent);
            actualEvent = null;
            EventChoice();
            if (representationForJournal.Count > 0)
            {
                monoJourn.gameObject.SetActive(true);
                monoJourn.ShowText(representationForJournal[0]);
                representationForJournal.RemoveAt(0);
            }
            else
            {
                StartNextEvent(futurEvents[0]);
            }
        }
    }

    public void EndGameEvent()
    {
        if (actualEvent.eventSuivant != null)
        {
            StartNextEvent(actualEvent.eventSuivant);
        }
        else if(actualEvent.endGame)
        {
            //Ecran de fin du jeu
        }
        else
        {
            actualEvent = null;
            StartNextEvent(EndingEventChoice());
        }
    }

    void EventChoice()
    {
        //Règles de Choix d'un événement
        Debug.Log(futurEvents.Count);
        futurEvents.Add(futurEvents[3]);
        while (futurEvents[4] == futurEvents[3])
        {
            Event newEvent = eventsPool[Random.Range(0, eventsPool.Count)];
            List<int> compteCommu = new List<int>(4);

            if(futurEvents.Contains(newEvent)) // Vérification de Doublon
            {
                goto continueWhile;
            }

            if(!newEvent.personnage.isAlive) //Vérifie si le personnage est en vie
            {
                goto continueWhile;
            }

            #region Communautés
            for(int i = 0; i < 5; i++)
            {
                switch(futurEvents[i].personnage.communaute.name)
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

            switch (newEvent.personnage.communaute.name)
            {
                case "Dirigeants":
                    if (compteCommu[0] > 2)
                    {
                        goto continueWhile;
                    }
                    break;
                case "Habitants":
                    if (compteCommu[1] > 2)
                    {
                        goto continueWhile;
                    }
                    break;
                case "Mendiants":
                    if (compteCommu[2] > 2)
                    {
                        goto continueWhile;
                    }
                    break;
                case "Représentants de l'ordre":
                    if (compteCommu[3] > 2)
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

    Event EndingEventChoice()
    {
        int maxScore = 0;
        Event finalEvent = commus[0].goodEnding;
        for(int i = 0; i < commus.Count; i++) //Pour les Commu, les mettre de la moins prioritaire à la plus prioritaire
        {
            int commuScore = 0;
            string tmpStr = commus[i].GetHighestRepresentation(out commuScore);
            if(commuScore>=maxScore)
            {
                maxScore = commuScore;
                switch(tmpStr)
                {
                    case "Pitie":
                        finalEvent = commus[i].goodEnding;
                        break;
                    case "Acceptation":
                        finalEvent = commus[i].goodEnding;
                        break;
                    case "Desir":
                        finalEvent = commus[i].goodEnding;
                        break;
                    case "Jalousie":
                        finalEvent = commus[i].badEnding;
                        break;
                    case "Agressivite":
                        finalEvent = commus[i].badEnding;
                        break;
                    case "Repulsion":
                        finalEvent = commus[i].badEnding;
                        break;
                }
            }
        }
        return finalEvent;
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
