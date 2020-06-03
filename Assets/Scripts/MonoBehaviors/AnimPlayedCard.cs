using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlayedCard : MonoBehaviour
{
    [SerializeField]
    private MonstreEventManager manag;
    public void EndAnim()
    {
        if(manag != null)
        {
            manag.EndCinematique();
        }
        gameObject.SetActive(false);
    }
}
