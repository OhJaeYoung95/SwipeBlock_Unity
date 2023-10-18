using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum BgmID
{
    Intro,
    Title,
    Game
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public List<AudioSource> bgmAudio;
    public List<AudioSource> effectsAudio;

    public AudioMixer masterMixer;
    public Slider audioSlider;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        OnIntroBGM();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OffAllBGM()
    {
        bgmAudio[(int)BgmID.Intro].Stop();
        bgmAudio[(int)BgmID.Title].Stop();
        bgmAudio[(int)BgmID.Game].Stop();
    }

    public void OnIntroBGM()
    {
        OffAllBGM();
        bgmAudio[(int)BgmID.Intro].Play();
        bgmAudio[(int)BgmID.Title].Play();
    }

    public void OnTitleBGM()
    {
        OffAllBGM();
        bgmAudio[(int)BgmID.Title].Play();
    }

    public void OnGameBGM()
    {
        OffAllBGM();
        bgmAudio[(int)BgmID.Game].Play();
    }


    public void SetAudioVolume()
    {
        float sound = audioSlider.value;

        if (sound == -40f)
            masterMixer.SetFloat("BGM", -80);
        else
            masterMixer.SetFloat("BGM", sound);
    }

    public void OnOffAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

    public void SetEffectsVolume(float volume)
    {

    }
    public void SetMusicVolume(float volume)
    {
        //bgmAudio.volume = volume;
    }
}
