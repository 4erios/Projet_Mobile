using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoJournal : MonoBehaviour
{
    [SerializeField]
    private Text titre;
    [SerializeField]
    private MonstreEventManager manag;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                gameObject.SetActive(false);
                manag.EndEvent();
            }
        }
    }

    public void ShowText(string text)
    {
        titre.text = text;
    }

}
