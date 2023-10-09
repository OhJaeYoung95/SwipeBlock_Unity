using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    public Button play;
    public Button option;

    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnClickPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickOptionButton()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnClikQuitButton()
    {
        // 에디터 상에서 종료
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        // 빌드된 게임 상에서 종료
        Application.Quit();
#endif    
    }
}
