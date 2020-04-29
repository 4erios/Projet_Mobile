using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Succes", menuName = "Create Success")]
public class Success : ScriptableObject
{
    public Sprite icone;
    public string titre;
    public string texteExplicatif;

    public bool ValidateSuccess()
    {
        return SaveLoadSystem.ValidateSuccess(this);
    }
}
