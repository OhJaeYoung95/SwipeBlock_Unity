using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button play;
    public Button option;

    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
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
        EditorApplication.isPlaying = false;
#else
        SoundManager.Instance.PlayAllButtonClickSound();
        Application.Quit();
#endif    
    }
}
