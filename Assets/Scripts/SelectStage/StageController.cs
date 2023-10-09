using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public Image[] images;
    public Vector2[] pos = new Vector2[3];
    public int currentIndex = 0;

    private void Start()
    {
        for (int i = 0; i < images.Length; i++)
        {
            pos[i] = images[i].transform.localPosition;
        }
        foreach(var value in pos)
        {
            Debug.Log(value);
        }
        UpdateImagePositions();
    }
    public void OnClickPlayButton()
    {
        Time.timeScale = 1f;

        PlayerPrefs.SetInt("CurrentStage", currentIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(3);
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

    private void UpdateImagePositions()
    {
        for (int i = 0; i < images.Length; i++)
        {
            int newIndex = (i + currentIndex) % images.Length;
            images[i].rectTransform.anchoredPosition = pos[newIndex];
        }
    }
}
