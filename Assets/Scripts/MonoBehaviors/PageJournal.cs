﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageJournal : MonoBehaviour
{
    [SerializeField]
    private Text txt;

    [SerializeField]
    private List<float> angles = new List<float>();

    public void ShowJournal(string t)
    {
        transform.localScale = Vector3.one;
        txt.text = t;
        if(angles.Count>0)
        {
            transform.eulerAngles = new Vector3(0, 0, angles[Random.Range(0, angles.Count)]);
        }
    }
}