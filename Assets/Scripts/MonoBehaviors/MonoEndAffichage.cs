using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoEndAffichage : MonoBehaviour
{
    public GameObject showEnd;
    public Image fond;
    public Text text;
    public bool canEnd;
    [SerializeField]
    private MenuScript menus;
    public Animator anim;

    private float timeBeforeClic = 1.5f;

    private void Update()
    {
        if (timeBeforeClic <= 0)
        {
            if (Input.touchCount > 0 && canEnd)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    menus.LoadScene(0);
                    canEnd = false;
                }
            }

            if (Input.GetMouseButtonDown(0) && canEnd)
            {
                menus.LoadScene(0);
                canEnd = false;
            }
        }
        else
        {
            timeBeforeClic -= Time.fixedDeltaTime;
        }
    }
}
