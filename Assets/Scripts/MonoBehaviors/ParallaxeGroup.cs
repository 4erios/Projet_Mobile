using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxeGroup : MonoBehaviour
{
    public List<Parallaxe> items;

    private void OnEnable()
    {
        StopAllCoroutines();
        x = 0;
        y = 0;
        StartCoroutine(ChangePosition());
    }

    float x, y;
    IEnumerator ChangePosition()
    {
        x += Random.Range(-1f, 1f);
        y += Random.Range(-1.5f, 1.5f);
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
        Debug.Log(x + y);
        foreach (Parallaxe item in items)
        {
            item.Move(new Vector2(x, y));
        }
        yield return new WaitForSeconds(Random.Range(1, 3));
        StartCoroutine(ChangePosition());
    }
}
