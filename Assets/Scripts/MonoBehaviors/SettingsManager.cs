using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private Slider musicSlider, sfxSlider;

    public Slider GetMusicSlider() => musicSlider;
    public Slider GetSfxSlider() => sfxSlider;

}
