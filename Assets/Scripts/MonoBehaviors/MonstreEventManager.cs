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
    [SerializeField]
    private int valeurFinCommu = 6;

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
    private SpriteRenderer affichageBlessure;
    [SerializeField]
    private GameObject tacheSang;
    [SerializeField]
    private EndEvent finCarnage;

    [SerializeField]
    private List<Event> allEvents;
    [SerializeField]
    private List<Personnage> allPersos;

    private int saveCount;

    private List<Personnage> encounteredCharacter = new List<Personnage>();

    #region Load des sauvegardes
    void Awake()
    {
        //Load Communautés
        SaveLoadSystem.LoadCommunauteScore(commus);

        //Load Game State
        string eventName = "";
        List<string> pool = new List<string>();
        List<string> persos = new List<string>();
        int paniqueToAdd = 0;
        int damage = 0;
        bool didLoad = SaveLoadSystem.LoadGameState(out eventName, out paniqueToAdd, out damage, out pool, out persos);

        futurEvents = new List<Event>();
        if (didLoad)
        {
            if (pool.Contains(eventsDepart[0].name))
            {
                Debug.Log("DidLoad");
                Panique.AddPanic(paniqueToAdd);
                GetDamage(monstreHp - damage);

                foreach (string name in pool)
                {
                    foreach (Event evt in allEvents)
                    {
                        if (evt.name == name)
                        {
                            if (!eventsDepart.Contains(evt))
                            {
                                eventsDepart.Add(evt);
                                if (evt.name == eventName)
                                {
                                    futurEvents.Add(evt);
                                    Debug.Log(futurEvents);
                                }
                            }
                            break;
                        }
                    }
                }

                foreach (string name in persos)
                {
                    foreach (Personnage perso in allPersos)
                    {
                        if (perso.name == name)
                        {
                            encounteredCharacter.Add(perso);
                            break;
                        }
                    }
                }
            }
            else
            {
                SaveLoadSystem.ResetGameSate();
            }
        }
        eventsPool = eventsDepart;
    }
    #endregion

    #region Mise en place des Events de début de partie
    private void Start()
    {
        LoadingScreen.HideLoadScreen();
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
    #endregion

    public void ShowCards()
    {
        cardManager.gameObject.SetActive(true);
    }

    public void HideCards()
    {
        cardManager.gameObject.SetActive(false);
    }

    public static void AddEvent(List<Event> eventToAdd) //Ajoute un Event au Pool
    {
        if (eventToAdd.Count > 0 && eventToAdd[0] != null)
        {
            foreach (Event evt in eventToAdd)
            {
                if (!eventsPool.Contains(evt))
                {
                    eventsPool.Add(evt);
                }
            }
        }
    }

    public static void RemoveEvent(List<Event> eventToRemove) //Enlève un Event du Pool
    {
        foreach (Event evt in eventToRemove)
        {
            if (eventsPool.Contains(evt))
            {
                eventsPool.Remove(evt);
            }
        }
    }

    public void StartNextEvent(Event newEvent) //Lance l'Event en entrée de fonction (Fonction publique appelant la fonction privée)
    {
        if (actualEvent == null)
        {
            #region Mise à jour des cartes retirées
            List<EmotionMonstre> emotionsToAdd = new List<EmotionMonstre>();
            List<EmotionMonstre> keys = new List<EmotionMonstre>();
            foreach (KeyValuePair<EmotionMonstre, int> dict in cardManager.removedCard)
            {
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
            #endregion


            actualEvent = newEvent;

            if (!encounteredCharacter.Contains(newEvent.personnage) && newEvent.personnage.rencontreEvent != null)
            {
                encounteredCharacter.Add(newEvent.personnage);
                actualEvent = newEvent.personnage.rencontreEvent;
            }
            StartEvent(actualEvent);
        }
    }

    void StartEvent(Event eventToStart) //Lance le nouvel Event (Fonction privée)
    {
        #region Mise en place du nouveau fond
        foreach (GameObject gm in fonds)
        {
            gm.GetComponent<ParallaxeGroup>().HideDecor();
        }
        switch (eventToStart.lieux)
        {
            //Mettre une animation à la place d'un SetActive
            case Lieux.ruelle:
                fonds[0].SetActive(true);
                break;
            case Lieux.pontDeSeine:
                fonds[1].SetActive(true);
                break;
        }
        #endregion
        monoDial.ShowAccroche(eventToStart.accroche.texte, eventToStart.dialogue.dialogueDepart);

        //Mettre le côté Animation des Cartes
    }

    public void AccrocheShowed()
    {
        ApparitionPerso(actualEvent.personnage.role.presets[actualEvent.preset].ReactionDepart);
    }

    public void GetResponse(EmotionMonstre reponse) //Récupère la carte jouée par le Joueur
    {
        PresetRole rolePreset = actualEvent.personnage.role.presets[actualEvent.preset];
        int damage = 0;
        Reactions emot = rolePreset.GetReact(reponse.emotion, out damage);
        if (reponse.bonusRole.Contains(actualEvent.personnage.role)) //Pour la réponse Bonus
        {
            ChangeSpritePerso(emot);
            monoDial.ShowDialogue(actualEvent.dialogue.reponseBonus, true);
            ChangeScoreCommu(actualEvent.personnage.communaute, emot, 1);
        }
        else if (reponse.malusRole.Contains(actualEvent.personnage.role)) //Pour la réponse Malus
        {
            ChangeSpritePerso(emot);
            monoDial.ShowDialogue(actualEvent.dialogue.reponseMalus, true);
            ChangeScoreCommu(actualEvent.personnage.communaute, emot, -1);
            Panique.AddPanic(2);
        }
        else
        {
            ChangeSpritePerso(emot);
            ReponseChoice(emot);
            ChangeScoreCommu(actualEvent.personnage.communaute, emot, 0);
            switch (emot)
            {
                case Reactions.degout:
                    Panique.AddPanic(1);
                    break;
                case Reactions.peur:
                    Panique.AddPanic(2);
                    break;
                case Reactions.haine:
                    Panique.AddPanic(1);
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

    [ContextMenu("Kill")]
    void KillPersonnage()
    {
        tacheSang.SetActive(true);
        actualEvent.personnage.Die();
        monoPerso.Die();

        nbDeadPeople++;
    }

    private void GetDamage(int dmg)
    {
        //Animation de prise de blessure
        monstreHp -= dmg;
        if (monstreHp > 0)
        {
            if (blessuresSprites.Count > monstreHp - 1 && blessuresSprites[monstreHp - 1] != null)
            {
                affichageBlessure.sprite = blessuresSprites[monstreHp - 1];
            }
        }
    }
    
    public void AddRepresentationForJournal(string phrase)
    {
        representationForJournal.Add(phrase);
    }

    [ContextMenu("TestJournal")]
    void EndEventTestJournel()
    {
        AddRepresentationForJournal("Test Journal 1");
        AddRepresentationForJournal("Test Journal 2");
        AddRepresentationForJournal("Test Journal 3");
        EndEvent();
    }

    public void EndEvent()
    {
        //Animation de fin d'évènement OU est appelée à la fin de l'animation (Uniquement si l'instruction vien de MonoDialogue)
        if (actualEvent.eventSuivant != null)
        {
            StartNextEvent(actualEvent.eventSuivant);
        }
        else if (monstreHp <= 0) //Fin "Bad End"
        {
            StartEndEvent(actualEvent.personnage.communaute.badEnding);
        }
        else if(nbDeadPeople>5) //Fin "Carnage"
        {
            StartEndEvent(finCarnage);
        }
        else if(commus[0].acceptation.valeur >= valeurFinCommu || commus[0].desir.valeur >= valeurFinCommu || commus[0].pitie.valeur >= valeurFinCommu) //Fin "Good End"
        {
            Debug.Log("Commu 1 fin");
            StartEndEvent(commus[0].goodEnding);
        }
        else if (commus[1].acceptation.valeur >= valeurFinCommu || commus[1].desir.valeur >= valeurFinCommu || commus[1].pitie.valeur >= valeurFinCommu)
        {
            Debug.Log("Commu 2 fin");
            StartEndEvent(commus[1].goodEnding);
        }
        else if (commus[2].acceptation.valeur >= valeurFinCommu || commus[2].desir.valeur >= valeurFinCommu || commus[2].pitie.valeur >= valeurFinCommu)
        {
            Debug.Log("Commu 3 fin");
            StartEndEvent(commus[2].goodEnding);
        }
        else if (commus[3].acceptation.valeur >= valeurFinCommu || commus[3].desir.valeur >= valeurFinCommu || commus[3].pitie.valeur >= valeurFinCommu)
        {
            Debug.Log("Commu 4 fin");
            StartEndEvent(commus[3].goodEnding);
        }
        else if (representationForJournal.Count > 0)
        {
            monoJourn.gameObject.SetActive(true);
            monoJourn.ShowText(representationForJournal[0]);
        }
        else
        {
            saveCount++;
            if (saveCount >= 3) //Save de l'état de la partie
            {
                saveCount = 0;
                SaveGameState();
            }
            futurEvents.Remove(actualEvent);
            actualEvent = null;
            EventChoice();
            StartNextEvent(futurEvents[0]);
        }
    }

    public void RemoveFromJournal()
    {
        representationForJournal.RemoveAt(0);
        if(representationForJournal.Count<=0)
        {
            monoJourn.Close();
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
                continue;
            }
            if(!newEvent.personnage.isAlive) //Vérifie si le personnage est en vie
            {
                continue;
            }
            #region Communautés
            if (count < 90)
            {
                for (int j = 0; j < 5; j++)
                {
                    switch (futurEvents[j].personnage.communaute.name)
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
                    if (compteCommu[0] > 2)
                    {
                        continue;
                    }
                    break;
                case "Habitants":
                    if (compteCommu[1] > 2)
                    {
                        continue;
                    }
                    break;
                case "Mendiants":
                    if (compteCommu[2] > 2)
                    {
                        continue;
                    }
                    break;
                case "Représentants de l'ordre":
                    if (compteCommu[3] > 2)
                    {
                        continue;
                    }
                    break;
            }
            #endregion
            int i = 0;
            for (i = 0; i < 4; i++) // Vérification d'un Event spécial présent
            {
                if(futurEvents[i].eventSuivant != null)
                {
                    break;
                }
            }
            if (i >= 4)
            {
                futurEvents[4] = newEvent;
            }
            //continueWhile:;
        }
    }
    [ContextMenu("BadEnd")]
    void TestEndBad()
    {
        monstreHp = -1;
        StartEndEvent(actualEvent.personnage.communaute.badEnding);
    }
    [ContextMenu("GoodEnd")]
    void TestEndGood()
    {
        StartEndEvent(actualEvent.personnage.communaute.goodEnding);
    }

    void StartEndEvent(EndEvent eventToEnd)
    {
        Debug.Log("End Game");
        SaveLoadSystem.ResetGameSate();
        HideCards();
        monoEnd.showEnd.SetActive(true);
        if(monstreHp<=0)
        {
            monoEnd.anim.SetBool("DyingEnd", true);
        }
        else
        {
            monoEnd.anim.SetBool("JustEnd", true);
        }
        if (eventToEnd.success != null)
        {
            eventToEnd.success.ValidateSuccess();
        }
        List<AncientGame> currentHistoric = SaveLoadSystem.GetHistoricList();
        if(currentHistoric.Count>=3)
        {
            currentHistoric.RemoveAt(0);
        }
        currentHistoric.Add(eventToEnd.historic);
        SaveLoadSystem.SaveHistoric(currentHistoric);
        //Mettre un affichage pour les succès
        Debug.Log("Image de fin ? : " + eventToEnd.imageFin);
        if (eventToEnd.imageFin != null)
        {
            Debug.Log("Test image fin");
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
                monoDial.ShowDialogue(actualEvent.dialogue.reponseInteret, true);
                break;
            case Reactions.affection:
                monoDial.ShowDialogue(actualEvent.dialogue.reponseAffection, true);
                break;
            case Reactions.compassion:
                monoDial.ShowDialogue(actualEvent.dialogue.reponseCompassion, true);
                break;
            case Reactions.degout:
                monoDial.ShowDialogue(actualEvent.dialogue.reponseDegout, true);
                break;
            case Reactions.peur:
                monoDial.ShowDialogue(actualEvent.dialogue.reponsePeur, true);
                break;
            case Reactions.haine:
                monoDial.ShowDialogue(actualEvent.dialogue.reponseHaine, true);
                break;
        }
    }

    void ApparitionPerso(Reactions react)
    {
        ChangeSpritePerso(react);
        monoPerso.Apparition();
    }

    public void DisparitionPerso()
    {
        monoPerso.Disparition();
        monoDial.SupprDial();
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
                    commuPerso.communauteEnnemie.jalousie.AddPoint(-commuPerso.coef, this);
                    commuPerso.desir.AddPoint(1, this);
                    commuPerso.communauteEnnemie.desir.AddPoint(-commuPerso.coef, this);
                }
                if(bonus <= 0)
                {
                    commuPerso.repulsion.AddPoint(-1, this);
                    commuPerso.communauteEnnemie.repulsion.AddPoint(commuPerso.coef, this);
                    commuPerso.pitie.AddPoint(-1, this);
                    commuPerso.communauteEnnemie.pitie.AddPoint(commuPerso.coef, this);
                }
                break;
            case Reactions.affection:
                if (bonus >= 0)
                {
                    commuPerso.acceptation.AddPoint(1, this);
                    commuPerso.communauteEnnemie.acceptation.AddPoint(-commuPerso.coef, this);
                    commuPerso.desir.AddPoint(1, this);
                    commuPerso.communauteEnnemie.desir.AddPoint(-commuPerso.coef, this);
                }
                if (bonus <= 0)
                {
                    commuPerso.repulsion.AddPoint(-1, this);
                    commuPerso.communauteEnnemie.repulsion.AddPoint(commuPerso.coef, this);
                    commuPerso.agressivite.AddPoint(-1, this);
                    commuPerso.communauteEnnemie.agressivite.AddPoint(commuPerso.coef, this);
                }
                break;
            case Reactions.compassion:
                if (bonus >= 0)
                {
                    commuPerso.acceptation.AddPoint(1, this);
                    commuPerso.communauteEnnemie.acceptation.AddPoint(-commuPerso.coef, this);
                    commuPerso.pitie.AddPoint(1, this);
                    commuPerso.communauteEnnemie.pitie.AddPoint(-commuPerso.coef, this);
                }
                if (bonus <= 0)
                {
                    commuPerso.jalousie.AddPoint(-1, this);
                    commuPerso.communauteEnnemie.jalousie.AddPoint(commuPerso.coef, this);
                    commuPerso.agressivite.AddPoint(-1, this);
                    commuPerso.communauteEnnemie.agressivite.AddPoint(commuPerso.coef, this);
                }
                break;
            case Reactions.degout:
                if (bonus >= 0)
                {
                    commuPerso.repulsion.AddPoint(1, this);
                    commuPerso.communauteEnnemie.repulsion.AddPoint(-commuPerso.coef, this);
                    commuPerso.pitie.AddPoint(1, this);
                    commuPerso.communauteEnnemie.pitie.AddPoint(-commuPerso.coef, this);
                }
                if (bonus <= 0)
                {
                    commuPerso.jalousie.AddPoint(-1, this);
                    commuPerso.communauteEnnemie.jalousie.AddPoint(commuPerso.coef, this);
                    commuPerso.desir.AddPoint(-1, this);
                    commuPerso.communauteEnnemie.desir.AddPoint(commuPerso.coef, this);
                }
                break;
            case Reactions.peur:
                if (bonus >= 0)
                {
                    commuPerso.repulsion.AddPoint(1, this);
                    commuPerso.communauteEnnemie.repulsion.AddPoint(-commuPerso.coef, this);
                    commuPerso.agressivite.AddPoint(1, this);
                    commuPerso.communauteEnnemie.agressivite.AddPoint(-commuPerso.coef, this);
                }
                if (bonus <= 0)
                {
                    commuPerso.acceptation.AddPoint(-1, this);
                    commuPerso.communauteEnnemie.acceptation.AddPoint(commuPerso.coef, this);
                    commuPerso.desir.AddPoint(-1, this);
                    commuPerso.communauteEnnemie.desir.AddPoint(commuPerso.coef, this);
                }
                break;
            case Reactions.haine:
                if (bonus >= 0)
                {
                    commuPerso.jalousie.AddPoint(1, this);
                    commuPerso.communauteEnnemie.jalousie.AddPoint(-commuPerso.coef, this);
                    commuPerso.agressivite.AddPoint(1, this);
                    commuPerso.communauteEnnemie.agressivite.AddPoint(-commuPerso.coef, this);
                }
                if (bonus <= 0)
                {
                    commuPerso.acceptation.AddPoint(-1, this);
                    commuPerso.communauteEnnemie.acceptation.AddPoint(commuPerso.coef, this);
                    commuPerso.pitie.AddPoint(-1, this);
                    commuPerso.communauteEnnemie.pitie.AddPoint(commuPerso.coef, this);
                }
                break;
        }
    }

    public void SaveGameState()
    {
        SaveLoadSystem.SaveGameState(actualEvent, Panique.value, monstreHp, eventsPool, encounteredCharacter);
        SaveLoadSystem.SaveCommunauteScore(commus);
        cardManager.SaveCards();
    }
}
