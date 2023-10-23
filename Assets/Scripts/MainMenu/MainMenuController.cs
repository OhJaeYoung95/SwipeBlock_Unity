using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject soundManager;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider seSlider;
    [SerializeField]
    private Toggle masterMuteToggle;
    [SerializeField]
    private Toggle bgmMuteToggle;
    [SerializeField]
    private Toggle seMuteToggle;

    private void Awake()
    {
        if (SoundManager.Instance == null)
            Instantiate(soundManager);

        transform.GetChild(0).gameObject.SetActive(false);
        masterSlider.onValueChanged.AddListener(SoundManager.Instance.SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBGMVolume);
        seSlider.onValueChanged.AddListener(SoundManager.Instance.SetSEVolume);

        masterMuteToggle.onValueChanged.AddListener(SoundManager.Instance.OnOffMasterVolume);
        bgmMuteToggle.onValueChanged.AddListener(SoundManager.Instance.OnOffBGMVolume);
        seMuteToggle.onValueChanged.AddListener(SoundManager.Instance.OnOffSEVolume);

        masterSlider.value = GameData.MasterVolume;
        bgmSlider.value = GameData.BGMVolune;
        seSlider.value = GameData.SEVolume;

        masterMuteToggle.isOn = GameData.IsOffMasterMute;
        bgmMuteToggle.isOn = GameData.IsOffBGMMute;
        seMuteToggle.isOn = GameData.IsOffSEMute;

    }

    public void OnClickPlayButton()
    {
        SoundManager.Instance.PlayAllButtonClickSound();
        SceneManager.LoadScene((int)Scene.Select);
    }

    public void OnClickOptionButton()
    {
        SoundManager.Instance.PlayAllButtonClickSound();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnClikQuitButton()
    {
#if UNITY_EDITOR
        SoundManager.Instance.PlayAllButtonClickSound();
        GameData.SaveGameData();
        EditorApplication.isPlaying = false;
#else
        SoundManager.Instance.PlayAllButtonClickSound();
        GameData.SaveGameData();
        Application.Quit();
#endif    
    }
}
