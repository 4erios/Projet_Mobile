using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EndingEvent", menuName = "Create Ending Event")]
public class EndEvent : ScriptableObject
{
    public Sprite imageFin;
    [TextArea(3, 5)]
    public string texteFin;
    public Success success;
    public AncientGame historic;
}
