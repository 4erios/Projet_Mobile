using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFinJournal : MonoBehaviour
{
    public MonoJournal journal;

    public void EndJournal()
    {
        gameObject.SetActive(false);
        journal.Disapear();
    }
}
