using System.Collections;
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

    public static void SaveGameState()
    {
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
    }

    #endregion
}
