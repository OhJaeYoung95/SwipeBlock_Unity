using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum BgmID
{
    Intro,
    Title,
    Game
}

public enum SoundEffectID
{
    Merge,
    Bomb,
    ItemClick,
    ScoreUp,
    ShopIconClick,
    ArrowButtonClick,
    PlayButtonClick,
    ReturnButtonClick,
    BuyButtonClick,
    FailedBuy,
    ShopItemToggle,
    GameOver,
    AllButtonClick,
    PopupOpen,
    PopupClose
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public List<AudioSource> bgmAudio;
    public List<AudioSource> effectsAudio;

    public AudioMixer audioMixer;

    private float masterVolume;
    private float bgmVolume;
    private float seVolume;

    private bool isOffMasterMute = false;
    private bool isOffBgmMute = false;
    private bool isOffSeMute = false;


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

    // BGM
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

    // SoundEffect
    public void PlayMergeSound()
    {
        effectsAudio[(int)SoundEffectID.Merge].Stop();
        effectsAudio[(int)SoundEffectID.Merge].Play();
    }
    public void PlayBombSound()
    {
        effectsAudio[(int)SoundEffectID.Bomb].Stop();
        effectsAudio[(int)SoundEffectID.Bomb].Play();
    }
    public void PlayItemClickSound()
    {
        effectsAudio[(int)SoundEffectID.ItemClick].Stop();
        effectsAudio[(int)SoundEffectID.ItemClick].Play();
    }
    public void PlayScoreUpSound()
    {
        effectsAudio[(int)SoundEffectID.ScoreUp].Stop();
        effectsAudio[(int)SoundEffectID.ScoreUp].Play();
    }
    public void PlayShopIconClickSound()
    {
        effectsAudio[(int)SoundEffectID.ScoreUp].Stop();
        effectsAudio[(int)SoundEffectID.ScoreUp].Play();
    }
    public void PlayArrowButtonClickSound()
    {
        effectsAudio[(int)SoundEffectID.ArrowButtonClick].Stop();
        effectsAudio[(int)SoundEffectID.ArrowButtonClick].Play();
    }
    public void PlayPlayButtonClickSound()
    {
        effectsAudio[(int)SoundEffectID.PlayButtonClick].Stop();
        effectsAudio[(int)SoundEffectID.PlayButtonClick].Play();
    }
    public void PlayReturnButtonClickSound()
    {
        effectsAudio[(int)SoundEffectID.ReturnButtonClick].Stop();
        effectsAudio[(int)SoundEffectID.ReturnButtonClick].Play();
    }
    public void PlayBuyButtonClickSound()
    {
        effectsAudio[(int)SoundEffectID.BuyButtonClick].Stop();
        effectsAudio[(int)SoundEffectID.BuyButtonClick].Play();
    }
    public void PlayFailedBuySound()
    {
        effectsAudio[(int)SoundEffectID.FailedBuy].Stop();
        effectsAudio[(int)SoundEffectID.FailedBuy].Play();
    }
    public void PlayShopItemToggleSound()
    {
        effectsAudio[(int)SoundEffectID.ShopItemToggle].Stop();
        effectsAudio[(int)SoundEffectID.ShopItemToggle].Play();
    }
    public void PlayGameOverSound()
    {
        effectsAudio[(int)SoundEffectID.GameOver].Stop();
        effectsAudio[(int)SoundEffectID.GameOver].Play();
    }
    public void PlayAllButtonClickSound()
    {
        effectsAudio[(int)SoundEffectID.AllButtonClick].Stop();
        effectsAudio[(int)SoundEffectID.AllButtonClick].Play();
    }
    public void PlaynPopupOpenSound()
    {
        effectsAudio[(int)SoundEffectID.PopupOpen].Stop();
        effectsAudio[(int)SoundEffectID.PopupOpen].Play();
    }
    public void PlayPopupCloseSound()
    {
        effectsAudio[(int)SoundEffectID.PopupClose].Stop();
        effectsAudio[(int)SoundEffectID.PopupClose].Play();
    }

    public void OnOffMasterVolume(bool value)
    {
        isOffMasterMute = value;
        audioMixer.SetFloat("Master", -80);
        if (!isOffMasterMute)
            SetMasterVolume(masterVolume);
    }
    public void OnOffBGMVolume(bool value)
    {
        isOffBgmMute = value;
        audioMixer.SetFloat("BGM", -80);
        if(!isOffBgmMute)
            SetBGMVolume(bgmVolume);
    }
    public void OnOffSEVolume(bool value)
    {
        isOffSeMute = value;
        audioMixer.SetFloat("SE", -80);
        if(!isOffSeMute)
            SetSEVolume(seVolume);
    }
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        if (isOffMasterMute)
            return;

        if (volume == -40f)
        {
            masterVolume = -80;
            audioMixer.SetFloat("Master", -80);
        }
        else
        {
            audioMixer.SetFloat("Master", volume);
        }

    }
    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        if (isOffBgmMute)
            return;

        if (volume == -40f)
        {
            bgmVolume = -80;
            audioMixer.SetFloat("BGM", -80);
        }
        else
        {
            audioMixer.SetFloat("BGM", volume);
        }
    }
    public void SetSEVolume(float volume)
    {
        seVolume = volume;
        if (isOffSeMute)
            return;

        if (volume == -40f)
        {
            seVolume = -80;
            audioMixer.SetFloat("SE", -80);
        }
        else
        {
            audioMixer.SetFloat("SE", volume);
        }
    }
}