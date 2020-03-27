using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxeGroup : MonoBehaviour
{
    public List<Parallaxe> items;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            foreach(Parallaxe item in items)
            {
                item.Move(new Vector2(4, 3));
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (Parallaxe item in items)
            {
                item.Move(new Vector2(-4, -3));
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (Parallaxe item in items)
            {
                item.Move(new Vector2(-4, 3));
            }
        }
    }
}
