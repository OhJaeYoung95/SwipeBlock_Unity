using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionOption : MonoBehaviour
{
    FullScreenMode screenMode;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullScreenBtn;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;

    // Start is called before the first frame update
    void Start()
    {
        InitUI();
    }
     
    void InitUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.value == 60f)
                resolutions.Add(Screen.resolutions[i]);
        }
        //resolutions.AddRange(Screen.resolutions);
        Resolution customResolution = new Resolution();
        customResolution.width = 1080;
        customResolution.height = 1920;
        resolutions.Add(customResolution);

        resolutionDropdown.options.Clear();

        int optionNum = 0;

        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = $"{item.width} x {item.height} {item.refreshRateRatio.value}hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value = optionNum;
            optionNum++;
        }

        resolutionDropdown.RefreshShownValue();

        fullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        SoundManager.Instance.PlayShopItemToggleSound();
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OkBtnClick()
    {
        SoundManager.Instance.PlayAllButtonClickSound();
        //Screen.SetResolution(resolutions[resolutionNum].width,
        //    resolutions[resolutionNum].height,
        //    screenMode);
        GameData.SaveGameData();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}