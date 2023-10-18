using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveDataVC = SaveDataV1;

public class StageController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stageImages;
    [SerializeField]
    private int currentIndex = 0;

    [SerializeField]
    private GameObject selectUICanvas;
    [SerializeField]
    private GameObject shopUICanvas;
    [SerializeField]
    private GameObject characterUICanvas;

    [SerializeField]
    private ShopController shopController;

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private TextMeshProUGUI shopGold;

    [SerializeField]
    private TextMeshProUGUI charGold;


    private void Awake()
    {
        InitScene();
    }
    private void InitScene()
    {
        DisableAllUI();
        selectUICanvas.SetActive(true);
        DisplayGold();
    }

    private void DisableAllStageImage()
    {
        foreach (var image in stageImages)
        {
            image.SetActive(false);
        }
    }

    private void UpdateStageImage(int index)
    {
        DisableAllStageImage();
        stageImages[index].SetActive(true);
    }
    private void DisableAllUI()
    {
        selectUICanvas.SetActive(false);
        shopUICanvas.SetActive(false);
        characterUICanvas.SetActive(false);
    }

    public void DisplayGold()
    {
        int goldAmount = GameData.Gold;
        shopGold.text = $"{goldAmount} Gold";
        charGold.text = $"{goldAmount} Gold";
    }
    
    public void OnClickPlayButton()
    {
        Time.timeScale = 1f;

        GameData.CurrentStage = currentIndex;
        SoundManager.Instance.OnGameBGM();
        SceneManager.LoadScene((int)Scene.Game);
    }
    public void OnClickShopButton()
    {
        DisableAllUI();
        shopUICanvas.SetActive(true);
        scrollRect.normalizedPosition = Vector3.one;
    }
    public void OnClickCharacterButton()
    {
        DisableAllUI();
        characterUICanvas.SetActive(true);
    }

    public void OnClickReturnButton()
    {
        DisableAllUI();
        selectUICanvas.SetActive(true);

    }
    public void OnClickLeftButton()
    {
        currentIndex = (currentIndex - 1 + stageImages.Length) % stageImages.Length;
        UpdateStageImage(currentIndex);
    }
    public void OnClickRightButton()
    {
        currentIndex = (currentIndex + 1) % stageImages.Length;
        UpdateStageImage(currentIndex);
    }
}
