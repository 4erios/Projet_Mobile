using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoCartes : MonoBehaviour
{
    public EmotionMonstre emotion;
    public Collider2D colid;

    private void Start()
    {
        GetComponent<Image>().sprite = emotion.sprite;
    }
}
