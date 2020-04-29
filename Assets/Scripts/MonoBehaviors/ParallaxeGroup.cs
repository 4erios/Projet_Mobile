using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxeGroup : MonoBehaviour
{
    public List<Parallaxe> items;

    private Gyroscope gyro;
    private Vector2 acceleratorStart;
    [SerializeField]
    private Animator anim;

    private void OnEnable()
    {
        /*Color baseColor = Color.white;
        baseColor.a = 0;
        foreach(Parallaxe item in items)
        {
            item.gameObject.GetComponent<SpriteRenderer>().color = baseColor;
        }*/
        acceleratorStart = new Vector2(Input.acceleration.x, Input.acceleration.y);
        StopAllCoroutines();
        x = 0;
        y = 0;
        StartCoroutine(ChangePosition());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void HideDecor()
    {
        gameObject.SetActive(false);
    }

    float x, y;
    IEnumerator ChangePosition()
    {
        x += Input.acceleration.x*0.3f; // Random.Range(-1f, 1f);
        y += (Input.acceleration.y+0.62f)*0.3f; // Random.Range(-1.5f, 1.5f);
        if (x>2)
        {
            x = 2;
        }
        else if (x<-2)
        {
            x = -2;
        }
        if (y > 3)
        {
            y = 3;
        }
        else if (y < -3)
        {
            y = -3;
        }
        foreach (Parallaxe item in items)
        {
            item.Move(new Vector2(x, y));
        }
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(ChangePosition());
    }
}
