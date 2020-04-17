using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxeGroup : MonoBehaviour
{
    public List<Parallaxe> items;

    private Gyroscope gyro;

    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
    }
    void OnGUI()
    {
        if (gyro != null)
        {
            GUILayout.Label("Gyroscope attitude : " + gyro.attitude);
            GUILayout.Label("Gyroscope gravity : " + gyro.gravity);
            GUILayout.Label("Gyroscope rotationRate : " + gyro.rotationRate);
            GUILayout.Label("Gyroscope rotationRateUnbiased : " + gyro.rotationRateUnbiased);
            GUILayout.Label("Gyroscope updateInterval : " + gyro.updateInterval);
            GUILayout.Label("Gyroscope userAcceleration : " + gyro.userAcceleration);
        }
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        x = 0;
        y = 0;
        StartCoroutine(ChangePosition());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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
        foreach (Parallaxe item in items)
        {
            item.Move(new Vector2(x, y));
        }
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(ChangePosition());
    }
}
