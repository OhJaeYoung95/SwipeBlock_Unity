using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveDataVC = SaveDataV1;

public class StageController : MonoBehaviour
{
    [SerializeField]
    private Image[] images;
    [SerializeField]
    private Vector2[] pos = new Vector2[3];
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
        for (int i = 0; i < images.Length; i++)
        {
            pos[i] = images[i].transform.localPosition;
        }
        InitScene();
    }
    private void InitScene()
    {
        DisableAllUI();
        selectUICanvas.SetActive(true);
        DisplayGold();
        UpdateImagePositions();
    }

    private void UpdateImagePositions()
    {
        for (int i = 0; i < images.Length; i++)
        {
            int newIndex = (i + currentIndex) % images.Length;
            images[i].rectTransform.anchoredPosition = pos[newIndex];
        }
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
        PlayerPrefs.SetInt("CurrentStage", currentIndex);
        PlayerPrefs.Save();

        List<ItemID> itemInfos = shopController.GetItemSlotInfo();

        if(itemInfos.Count > 0)
        {
            for (int i = 1; i < itemInfos.Count + 1; ++i)
            {
                int itemId = (int)itemInfos[i - 1];
                PlayerPrefs.SetInt($"ItemSlot{i}", itemId);
            }
        }

        SceneManager.LoadScene(3);
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
        currentIndex = (currentIndex + 1) % images.Length;
        UpdateImagePositions();
    }
    public void OnClickRightButton()
    {
        currentIndex = (currentIndex - 1 + images.Length) % images.Length;
        UpdateImagePositions();
    }
}
