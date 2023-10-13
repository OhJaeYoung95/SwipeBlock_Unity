using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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


    private void Awake()
    {
        for (int i = 0; i < images.Length; i++)
        {
            pos[i] = images[i].transform.localPosition;
        }
        DisableAllUI();
        //shopUICanvas.SetActive(true);
        selectUICanvas.transform.GetChild(0).gameObject.SetActive(true);

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
        //characterUICanvas.SetActive(false);
    }
    public void OnClickPlayButton()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("CurrentStage", currentIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(3);
    }
    public void OnClickShopButton()
    {
        DisableAllUI();
        shopUICanvas.SetActive(true);

    }
    public void OnClickReturnButton()
    {
        DisableAllUI();
        selectUICanvas.SetActive(true);

    }
    public void OnClickCharacterButton()
    {
        DisableAllUI();
        characterUICanvas.SetActive(true);
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
