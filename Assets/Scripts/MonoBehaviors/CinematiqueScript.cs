using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematiqueScript : MonoBehaviour
{
    private bool canEnd;

    [SerializeField]
    private MonstreEventManager manag;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (!canEnd)
                {
                    canEnd = true;
                }
                else
                {
                    EndAnim();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(!canEnd)
            {
                canEnd = true;
            }
            else
            {
                EndAnim();
            }
        }
    }

    public void EndAnim()
    {
        if (manag != null)
        {
            manag.EndCinematique();
        }
        gameObject.SetActive(false);
    }
}
