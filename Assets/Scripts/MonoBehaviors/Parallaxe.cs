using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxe : MonoBehaviour
{
    [SerializeField]
    private float distance;
    [SerializeField]
    private float lerpCoef;
    public Vector2 newPosition;
    private float coef;
    private int ips = 100;
    private Quaternion startGyro, actualGyro;

    // Start is called before the first frame update
    void Start()
    {
        /*if(!Input.gyro.enabled)
        {
            Input.gyro.enabled = true;
        }
        startGyro = Input.gyro.attitude;
        Debug.Log(startGyro);
        actualGyro = startGyro;*/
        if (distance < 1)
        {
            distance = 1;
        }
        coef = 1 / distance;
        GetComponent<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(distance);
        StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        /*actualGyro = Input.gyro.attitude;
        Debug.Log(actualGyro);
        Debug.Log(Input.gyro.rotationRate);*/
        if (Vector2.Distance(transform.localPosition, newPosition) > 0.1f)
        {
            transform.localPosition = new Vector2(Mathf.Lerp(transform.localPosition.x, newPosition.x * coef, lerpCoef), Mathf.Lerp(transform.localPosition.y, newPosition.y * coef, lerpCoef));
        }
        yield return new WaitForSeconds(1 / ips);
        StartCoroutine(Moving());
    }

    public void Move(Vector2 pos)
    {
        StopAllCoroutines();
        newPosition = pos;
        StartCoroutine(Moving());
    }
}
