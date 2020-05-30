using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSystem : MonoBehaviour
{
    //https://www.youtube.com/watch?v=SNwPq01yHds

    [SerializeField]
    private List<Success> allSuccess;
    [SerializeField]
    private List<AncientGame> allHistoric;
    [SerializeField]
    private List<Quest> allQuests;
    
    public static List<Success> everySuccess = new List<Success>();
    private static List<AncientGame> everyHistoric = new List<AncientGame>();
    private static List<Quest> everyQuests = new List<Quest>();

    private void Awake()
    {
        everySuccess = allSuccess;
        everyHistoric = allHistoric;
        everyQuests = allQuests;
    }

    static string GetPath(string name, string extension = ".txt") => Path.Combine(Application.persistentDataPath, name + extension);

    static string GetDirectory(string name) => Path.Combine(Application.persistentDataPath, name);

    public static void Save(object objToSave, string name)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GetPath(name));
        var json = JsonUtility.ToJson(objToSave);
        bf.Serialize(file, json);
        file.Close();
        Debug.Log("Did Save");
    }

    public static bool Load(object objToLoad, string name)
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(GetPath(name)))
        {
            FileStream file = File.Open(GetPath(name), FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), objToLoad);
            file.Close();
            return true;
        }
        return false;
    }

    public static void DeleteFileInDirectory(string direc)
    {
        if (Directory.Exists(direc))
        {
            string[] files = Directory.GetFiles(direc);
            string[] dirs = Directory.GetDirectories(direc);

            if (dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    DeleteFileInDirectory(dir);
                }
            }

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            Directory.Delete(direc);
        }
    }

    #region Save System
    public static void ResetGameSate()
    {
        if (Directory.Exists(GetDirectory("GameState")))
        {
            DeleteFileInDirectory(GetDirectory("GameState"));
        }
    }

    public static void CreateDirectory()
    {
        Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "GameState"));
    }

    public static void SaveTextDoc(string docName, string toSave)
    {
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "GameState")))
        {
            CreateDirectory();
        }
        File.WriteAllText(GetPath(docName), toSave);
    }

    #region GameState
    public static void SaveGameState(Event actualEvent, float panique, int blessures, List<Event> pool, List<Personnage> persos)
    {
        string fileContent = "";
        fileContent += actualEvent.name + "\n";
        fileContent += panique + "\n";
        fileContent += blessures + "\n";
        foreach(Event evt in pool)
        {
            fileContent += evt.name + ";";
        }
        fileContent += "\n";
        foreach(Personnage perso in persos)
        {
            fileContent += perso.name + ";";
        }
        SaveTextDoc("GameState/GameActualState", fileContent);
    }
    public static bool LoadGameState(out string ActualEvent, out int panique, out int blessure, out List<string> poolEvent, out List<string> personnages)
    {
        ActualEvent = "";
        panique = 0;
        blessure = 0;
        poolEvent = new List<string>();
        personnages = new List<string>();
        if (File.Exists(GetPath("GameState/GameActualState")))
        {
            string[] fileContent = File.ReadAllLines(GetPath("GameState/GameActualState"));

            ActualEvent = fileContent[0];
            panique = int.Parse(fileContent[1]);
            blessure = int.Parse(fileContent[2]);
            string[] buffer = fileContent[3].Split(';');
            foreach(string name in buffer)
            {
                poolEvent.Add(name);
            }
            if (fileContent.Length >= 4)
            {
                buffer = fileContent[4].Split(';');
                foreach (string name in buffer)
                {
                    personnages.Add(name);
                }
            }
            return true;
        }
        else
        {
            Debug.Log("Echec Chargement Game State");
            return false;
        }
    }
    #endregion

    #region Cards
    public static void SaveCards(List<EmotionMonstre> cards)
    {
        string fileContent = "";
        foreach(EmotionMonstre card in cards)
        {
            fileContent += card.name + "\n";
        }
        SaveTextDoc("GameState/Cards", fileContent);
    }
    public static string[] LoadCards()
    {
        if (File.Exists(GetPath("GameState/Cards")))
        {
            string[] fileContent = File.ReadAllLines(GetPath("GameState/Cards"));
            return fileContent;
        }
        else
        {
            Debug.Log("Echec Chargement Game State");
            return new string[0];
        }
    }
    #endregion

    #region Removed Cards
    public static void SaveRemovedCards(Dictionary<EmotionMonstre, int> removedCards)
    {
        string fileContent = "";
        foreach (var card in removedCards)
        {
            fileContent += card.Key.name + ";" + card.Value + "\n";
        }
        SaveTextDoc("GameState/RemovedCards", fileContent);
    }
    public static Dictionary<string,int> LoadRemovedCards()
    {
        if (File.Exists(GetPath("GameState/RemovedCards")))
        {
            Dictionary<string, int> theDict = new Dictionary<string, int>();
            string[] fileContent = File.ReadAllLines(GetPath("GameState/RemovedCards"));
           
            foreach(string line in fileContent)
            {
                string[] buffer = line.Split(';');
                theDict.Add(buffer[0], int.Parse(buffer[1]));
            }
            return theDict;
        }
        else
        {
            Debug.Log("Echec Chargement Game State");
            return new Dictionary<string, int>();
        }
    }
    #endregion

    #region Communautés
    public static void SaveCommunauteScore(List<Communaute> commus)
    {
        string fileContent = "";
        foreach (Communaute com in commus)
        {
            fileContent += com.repulsion.valeur + ";";
            fileContent += com.agressivite.valeur + ";";
            fileContent += com.jalousie.valeur + ";";
            fileContent += com.desir.valeur + ";";
            fileContent += com.acceptation.valeur + ";";
            fileContent += com.pitie.valeur + "\n";
        }
        SaveTextDoc("GameState/Communautes", fileContent);
    }
    public static void LoadCommunauteScore(List<Communaute> commus)
    {
        if (File.Exists(GetPath("GameState/Communautes")))
        {
            string[] fileContent = File.ReadAllLines(GetPath("GameState/Communautes"));

            for(int i =0; i < 4; i++)
            {
                string[] buffer = fileContent[i].Split(';');
                commus[i].repulsion.valeur = int.Parse(buffer[0]);
                if(commus[i].repulsion.valeur >= commus[i].repulsion.seuilHaut)
                {
                    commus[i].repulsion.firstState = false;
                }
                else
                {
                    commus[i].repulsion.firstState = true;
                }

                commus[i].agressivite.valeur = int.Parse(buffer[0]);
                if (commus[i].agressivite.valeur >= commus[i].agressivite.seuilHaut)
                {
                    commus[i].agressivite.firstState = false;
                }
                else
                {
                    commus[i].agressivite.firstState = true;
                }

                commus[i].jalousie.valeur = int.Parse(buffer[0]);
                if (commus[i].jalousie.valeur >= commus[i].jalousie.seuilHaut)
                {
                    commus[i].jalousie.firstState = false;
                }
                else
                {
                    commus[i].jalousie.firstState = true;
                }

                commus[i].desir.valeur = int.Parse(buffer[0]);
                if (commus[i].desir.valeur >= commus[i].desir.seuilHaut)
                {
                    commus[i].desir.firstState = false;
                }
                else
                {
                    commus[i].desir.firstState = true;
                }

                commus[i].acceptation.valeur = int.Parse(buffer[0]);
                if (commus[i].acceptation.valeur >= commus[i].acceptation.seuilHaut)
                {
                    commus[i].acceptation.firstState = false;
                }
                else
                {
                    commus[i].acceptation.firstState = true;
                }

                commus[i].pitie.valeur = int.Parse(buffer[0]);
                if (commus[i].pitie.valeur >= commus[i].pitie.seuilHaut)
                {
                    commus[i].pitie.firstState = false;
                }
                else
                {
                    commus[i].pitie.firstState = true;
                }

            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                commus[i].repulsion.valeur = 0;
                commus[i].repulsion.firstState =true;
                commus[i].agressivite.valeur = 0;
                commus[i].agressivite.firstState = true;
                commus[i].jalousie.valeur = 0;
                commus[i].jalousie.firstState = true;
                commus[i].desir.valeur = 0;
                commus[i].desir.firstState = true;
                commus[i].acceptation.valeur = 0;
                commus[i].acceptation.firstState = true;
                commus[i].pitie.valeur = 0;
                commus[i].pitie.firstState = true;
            }
        }
    }
    #endregion

    public static void SaveHistoric(List<AncientGame> historic)
    {
        string fileContent = "";
        foreach (AncientGame game in historic)
        {
            fileContent += game.titre + "\n";
        }
        SaveTextDoc("Historic", fileContent);
    }
    public static string[] LoadHistoric()
    {
        if (File.Exists(GetPath("Historic")))
        {
            string[] fileContent = File.ReadAllLines(GetPath("Historic"));
            return fileContent;
        }
        else
        {
            Debug.Log("Echec Chargement Historique");
            return new string[0];
        }
    }
    public static List<AncientGame> GetHistoricList()
    {
        string[] historicTitles = LoadHistoric();
        List<AncientGame> historic = new List<AncientGame>();

        foreach (string title in historicTitles)
        {
            for (int i = 0; i < everyHistoric.Count; i++)
            {
                if (everyHistoric[i].titre == title)
                {
                    historic.Add(everyHistoric[i]);
                    break;
                }
            }
        }
        return historic;
    }

    public static bool ValidateSuccess(Success succ)
    {
        List<Success> successList = GetSuccessList();
        if(successList.Contains(succ))
        {
            return false;
        }
        else
        {
            AffichageSuccesInGame.AffichageSucces(succ.titre);
            successList.Add(succ);
            Debug.Log(successList.Count);
            SaveAllSuccess(successList);
            return true;
        }
    }
    public static void SaveAllSuccess(List<Success> successList)
    {
        string fileContent = "";
        foreach (Success sucess in successList)
        {
            fileContent += sucess.titre + "\n";
        }
        SaveTextDoc("Success", fileContent);
    }
    public static string[] LoadSuccess()
    {
        if (File.Exists(GetPath("Success")))
        {
            string[] fileContent = File.ReadAllLines(GetPath("Success"));
            return fileContent;
        }
        else
        {
            Debug.Log("Echec Chargement Succès");
            return new string[0];
        }
    }
    public static List<Success> GetSuccessList()
    {
        string[] succesTitles = LoadSuccess();
        List<Success> unlockedSucc = new List<Success>();

        foreach(string title in succesTitles)
        {
            for(int i = 0; i < everySuccess.Count; i++)
            {
                if(everySuccess[i].titre == title)
                {
                    Debug.Log("Success is Unlocked ?");
                    unlockedSucc.Add(everySuccess[i]);
                    break;
                }
            }
        }
        return unlockedSucc;
    }

    public static bool ValidateQuest(Quest quete)
    {
        List<Quest> queteList = GetQuestList();
        if (queteList.Contains(quete))
        {
            return false;
        }
        else
        {
            queteList.Add(quete);
            SaveAllQuest(queteList);
            return true;
        }
    }
    public static void SaveAllQuest(List<Quest> quetesList)
    {
        string fileContent = "";
        foreach (Quest quete in quetesList)
        {
            fileContent += quete.titre + ";" + quete.isValid + "\n";
        }
        SaveTextDoc("Quest", fileContent);
    }
    public static void SaveAllUsedQuest(List<Quest> quetesList)
    {
        string fileContent = "";
        foreach (Quest quete in quetesList)
        {
            fileContent += quete.titre + "\n";
        }
        SaveTextDoc("UsedQuest", fileContent);
    }
    public static string[] LoadQuest(string path)
    {
        if (File.Exists(GetPath(path)))
        {
            string[] fileContent = File.ReadAllLines(GetPath(path));
            return fileContent;
        }
        else
        {
            Debug.Log("Echec Chargement Quest");
            return new string[0];
        }
    }
    public static List<Quest> GetQuestList()
    {
        string[] questTitles = LoadQuest("Quest");
        List<Quest> unlockedQuest = new List<Quest>();

        foreach (string title in questTitles)
        {
            string[] buffer = title.Split(';');
            for (int i = 0; i < everyQuests.Count; i++)
            {
                if (everyQuests[i].titre == buffer[0])
                {
                    Quest newQuest = everyQuests[i];
                    if (buffer[1] == "True")
                    {
                        newQuest.isValid = true;
                    }
                    unlockedQuest.Add(newQuest);

                    break;
                }
            }
        }
        return unlockedQuest;
    }
    public static List<Quest> GetUsedQuestList()
    {
        string[] questTitles = LoadQuest("UsedQuest");
        List<Quest> unlockedQuest = new List<Quest>();

        foreach (string title in questTitles)
        {
            for (int i = 0; i < everyQuests.Count; i++)
            {
                if (everyQuests[i].titre == title)
                {
                    unlockedQuest.Add(everyQuests[i]);
                    break;
                }
            }
        }
        return unlockedQuest;
    }
    public static void SaveMonney(int montant)
    {
        string fileContent = "";
        fileContent += montant;
        SaveTextDoc("Bank", fileContent);
    }
    public static int LoadMonney()
    {
        if (File.Exists(GetPath("Bank")))
        {
            string[] fileContent = File.ReadAllLines(GetPath("Bank"));
            return int.Parse(fileContent[0]);
        }
        else
        {
            Debug.Log("Echec Chargement Bank");
            return 0;
        }
    }


    public static void SaveSuccessState(int playedCards,int killedCharacter,int questsAccomplished)
    {
        string fileContent = "";
        int currentPlayCard = 0;
        int currentKilledChara = 0;
        int currentQuest = 0;
        LoadSuccessState(out currentPlayCard, out currentKilledChara, out currentQuest);
        playedCards += currentPlayCard;
        killedCharacter += currentKilledChara;
        questsAccomplished += currentQuest;
        fileContent += playedCards + "\n";
        fileContent += killedCharacter + "\n";
        fileContent += questsAccomplished + "\n";

        SaveTextDoc("SuccessState", fileContent);
    }
    public static void LoadSuccessState(out int playedCards,out int killedCharacter,out int questsAccomplished)
    {
        playedCards = 0;
        killedCharacter = 0;
        questsAccomplished = 0;
        if (File.Exists(GetPath("SuccessState")))
        {
            string[] fileContent = File.ReadAllLines(GetPath("SuccessState"));
            playedCards = int.Parse(fileContent[0]);
            killedCharacter = int.Parse(fileContent[1]);
            questsAccomplished = int.Parse(fileContent[2]);
        }
        else
        {
            Debug.Log("Echec Chargement SuccessState");
        }
    }

    public static void SaveUnlockedPerso(string persoName)
    {
        Debug.Log("Path : " + GetPath("UnlockedPerso"));
        File.AppendAllText(GetPath("UnlockedPerso"), persoName + "\n");
    }
    public static string[] LoadUnlockedPerso()
    {
        if (File.Exists(GetPath("UnlockedPerso")))
        {
            string[] fileContent = File.ReadAllLines(GetPath("UnlockedPerso"));
            return fileContent;
        }
        else
        {
            Debug.Log("Echec Chargement UnlockedPerso");
            return new string[0];
        }
    }

    public static void ResetQuest()
    {
        string[] files = Directory.GetFiles(GetDirectory(""));

        foreach (string file in files)
        {
            if (file == GetPath("UsedQuest"))
            {
                //File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
        }
    }

    public void ResetAll()
    {
        string[] files = Directory.GetFiles(GetDirectory(""));

        foreach (string file in files)
        {
            if (file == GetPath("UsedQuest") || file == GetPath("Success") || file == GetPath("SuccessState") || file == GetPath("Bank"))
            {
                //File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
        }
    }

    //Representation repulsion, agressivite, jalousie, desir, acceptation, pitie;

    //Save Event Actuel
    //Save Cartes disponibles
    //Save Panique
    //Save Score de toute les communautés
    //Save Blessure
    //Save Personnages morts

    /* Méthode :
     *  Fichier 1 : EventActuel / Panique / Blessure
     *  Fichier 2 : Cartes Disponible
     *  Fichier 3 : Personnages Morts
     *  Fichier 4 : Les Communautés
     *      Fichier 4.1 : Score de Mendiant
     *      Fichier 4.2 : Score de Habitant
     *      Fichier 4.3 : Score de Ordre
     *      Fichier 4.4 : Score de Dirigeant
     */
    #endregion
}
