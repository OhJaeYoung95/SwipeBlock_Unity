using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource[] effectsAudio;
    public AudioSource musicAudio;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetEffectsVolume(float volume)
    {
        //AudioSource[] bunnyAudios = bunnys.GetComponentsInChildren<AudioSource>();
        //AudioSource[] bearAudios = bears.GetComponentsInChildren<AudioSource>();
        //AudioSource[] elephantAudios = elephants.GetComponentsInChildren<AudioSource>();

        //foreach (var bunnyAudio in bunnyAudios)
        //{
        //    bunnyAudio.volume = volume;
        //}

        //foreach (var bearAudio in bearAudios)
        //{
        //    bearAudio.volume = volume;
        //}

        //foreach (var elephantAudio in elephantAudios)
        //{
        //    elephantAudio.volume = volume;
        //}

        //foreach (var effect in effectsAudio)
        //{
        //    effect.volume = volume;
        //}
    }
    public void SetMusicVolume(float volume)
    {
        musicAudio.volume = volume;
    }
}
