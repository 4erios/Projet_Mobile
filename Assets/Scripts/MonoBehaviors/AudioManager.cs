using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMix;

    public AudioSource source;
    public AudioClip music;
    public List<AudioClip> sfxs;
    public float soundLevel;

    [SerializeField]
    private SettingsManager sett;

    // Start is called before the first frame update
    void Start()
    {
        source.clip = music;
        source.Play();

        sett.GetMusicSlider().onValueChanged.AddListener(ChangeValueMusic);
        ChangeValueMusic(sett.GetMusicSlider().value);
        sett.GetSfxSlider().onValueChanged.AddListener(ChangeValueSfx);
        ChangeValueSfx(sett.GetSfxSlider().value);
    }

    private void ChangeValueMusic(float value)
    {
        audioMix.SetFloat("MusicVolume", value*45-50);
    }

    private void ChangeValueSfx(float value)
    {
        audioMix.SetFloat("SfxVolume", value * 50-30);
    }
}
