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

    [SerializeField]
    private SettingsManager sett;

    private float musix, sfx;

    // Start is called before the first frame update
    void Start()
    {
        source.clip = music;
        source.Play();
        SaveLoadSystem.LoadSound(out musix, out sfx);
        sett.GetMusicSlider().onValueChanged.AddListener(ChangeValueMusic);
        ChangeValueMusic(musix);
        sett.GetMusicSlider().value = musix;
        sett.GetSfxSlider().onValueChanged.AddListener(ChangeValueSfx);
        ChangeValueSfx(sfx);
        sett.GetSfxSlider().value = sfx;
    }

    private void ChangeValueMusic(float value)
    {
        audioMix.SetFloat("MusicVolume", value*45-50);
        musix = value;
        SaveLoadSystem.SaveSound(musix, sfx);
    }

    private void ChangeValueSfx(float value)
    {
        audioMix.SetFloat("SfxVolume", value * 40-30);
        sfx = value;
        SaveLoadSystem.SaveSound(musix, sfx);
    }
}
