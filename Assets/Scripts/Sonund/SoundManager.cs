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

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        OnIntroBGM();
        InitSound();
    }

    private void InitSound()
    {
        OnOffMasterVolume(GameData.IsOffMasterMute);
        OnOffBGMVolume(GameData.IsOffBGMMute);
        OnOffSEVolume(GameData.IsOffSEMute);
        SetMasterVolume(GameData.MasterVolume);
        SetBGMVolume(GameData.BGMVolune);
        SetSEVolume(GameData.SEVolume);
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

    // Control
    public void OnOffMasterVolume(bool value)
    {
        GameData.IsOffMasterMute = value;
        audioMixer.SetFloat("Master", -80);
        if (!GameData.IsOffMasterMute)
            SetMasterVolume(GameData.MasterVolume);
    }
    public void OnOffBGMVolume(bool value)
    {
        GameData.IsOffBGMMute = value;
        audioMixer.SetFloat("BGM", -80);
        if(!GameData.IsOffBGMMute)
            SetBGMVolume(GameData.BGMVolune);
    }
    public void OnOffSEVolume(bool value)
    {
        GameData.IsOffSEMute = value;
        audioMixer.SetFloat("SE", -80);
        if(!GameData.IsOffSEMute)
            SetSEVolume(GameData.SEVolume);
    }
    public void SetMasterVolume(float volume)
    {
        GameData.MasterVolume = volume;
        if (GameData.IsOffMasterMute)
            return;

        if (volume == -40f)
        {
            GameData.MasterVolume = -80;
            audioMixer.SetFloat("Master", -80);
        }
        else
        {
            audioMixer.SetFloat("Master", volume);
        }

    }
    public void SetBGMVolume(float volume)
    {
        GameData.BGMVolune = volume;
        if (GameData.IsOffBGMMute)
            return;

        if (volume == -40f)
        {
            GameData.BGMVolune = -80;
            audioMixer.SetFloat("BGM", -80);
        }
        else
        {
            audioMixer.SetFloat("BGM", volume);
        }
    }
    public void SetSEVolume(float volume)
    {
        GameData.SEVolume = volume;
        if (GameData.IsOffSEMute)
            return;

        if (volume == -40f)
        {
            GameData.SEVolume = -80;
            audioMixer.SetFloat("SE", -80);
        }
        else
        {
            audioMixer.SetFloat("SE", volume);
        }
    }
}