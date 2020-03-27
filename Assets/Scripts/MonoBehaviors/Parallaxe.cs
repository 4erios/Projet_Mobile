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

    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.z >= 1)
        {
            distance = gameObject.transform.position.z;
        }
        coef = 1 / distance;
        GetComponent<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(distance);
    }

    IEnumerator Moving()
    {
        if (Vector2.Distance(transform.position, newPosition) > 0.1f)
        {
            transform.position = new Vector2(Mathf.Lerp(transform.position.x, newPosition.x * coef, lerpCoef), Mathf.Lerp(transform.position.y, newPosition.y * coef, lerpCoef));
            yield return new WaitForSeconds(1/ips);
            StartCoroutine(Moving());
        }
    }

    public void Move(Vector2 pos)
    {
        StopAllCoroutines();
        newPosition = pos;
        StartCoroutine(Moving());
    }
}
