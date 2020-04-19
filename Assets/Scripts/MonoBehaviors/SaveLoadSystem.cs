﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSystem : MonoBehaviour
{
    //https://www.youtube.com/watch?v=SNwPq01yHds

    static string GetPath(string name, string extension = ".txt") => Path.Combine(Application.persistentDataPath, name + extension);

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

    #region GameState
    public static void ResetGameSate()
    {
        DeleteFileInDirectory(GetPath("GameState"));
    }

    public static void CreateDirectory()
    {
        Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "GameState"));
    }

    public static void SaveTextDoc(string docName, string toSave)
    {
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "GameState")))
        {
            Debug.Log("Direct Exist ?");
            CreateDirectory();
        }
        Debug.Log("Path : " + GetPath(docName));
        File.WriteAllText(GetPath(docName), toSave);
    }

    #region GameState
    public static void SaveGameState(Event actualEvent, int panique, int blessures, List<Event> pool)
    {
        string fileContent = "";
        fileContent += actualEvent.name + "\n";
        fileContent += panique + "\n";
        fileContent += blessures + "\n";
        foreach(Event evt in pool)
        {
            fileContent += evt.name + ";";
        }
        SaveTextDoc("GameState/GameActualState", fileContent);
    }
    public static bool LoadGameState(out string ActualEvent, out int panique, out int blessure, out List<string> poolEvent)
    {
        ActualEvent = "";
        panique = 0;
        blessure = 0;
        poolEvent = new List<string>();
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
                commus[i].agressivite.valeur = int.Parse(buffer[0]);
                commus[i].jalousie.valeur = int.Parse(buffer[0]);
                commus[i].desir.valeur = int.Parse(buffer[0]);
                commus[i].acceptation.valeur = int.Parse(buffer[0]);
                commus[i].pitie.valeur = int.Parse(buffer[0]);
            }
        }
    }
    #endregion

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