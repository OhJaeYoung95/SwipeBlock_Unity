using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject soundManager;
    public static GameManager Instance { get; private set; }
    public bool IsMove { get; set; } = false;
    public bool IsGameOver { get; private set; } = false;
    public bool IsPause { get; set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        if (SoundManager.Instance == null)
            Instantiate(soundManager);

        Instance.IsGameOver = false;
        Instance.IsPause = false;
        Instance.IsMove = false;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateBestScoreUI(GameData.BestScore);
            UIManager.Instance.UpdateScoreUI(ScoreManager.Instance.CurrentScore);
        }
    }

    public void GameOver()
    {
        IsGameOver = true;
        IsMove = false;
        Time.timeScale = 0f;
        StopAllCoroutinesOfSingleTon();
        SoundManager.Instance.PlayGameOverSound();
        BlockManager.Instance.ClearBoard();
        ScoreManager.Instance.ConvertScoreToGold();
        UIManager.Instance.GameOver();
        GameData.SaveGameData();
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        UIManager.Instance.gameTimer = UIManager.Instance.gameDuration;
        SoundManager.Instance.OnGameBGM();
        SoundManager.Instance.PlayAllButtonClickSound();
        StopAllCoroutinesOfSingleTon();
        BlockManager.Instance.ClearBoard();
        ScoreManager.Instance.Init();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SelectStage()
    {
        Time.timeScale = 0f;
        StopAllCoroutinesOfSingleTon();
        SoundManager.Instance.OnTitleBGM();
        SoundManager.Instance.PlayAllButtonClickSound();
        ScoreManager.Instance.CurrentScore = 0;
        BlockManager.Instance.ClearBoard();
        SceneManager.LoadScene((int)Scene.Select);
    }

    public void Pause()
    {
        IsPause = true;
        SoundManager.Instance.PlaynPopupOpenSound();
        UIManager.Instance.Pause();
    }

    public void Continue()
    {
        IsPause = false;
        SoundManager.Instance.PlayPopupCloseSound();
        UIManager.Instance.Continue();
    }

    public void Quit()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        GameData.SaveGameData();
        SoundManager.Instance.PlayAllButtonClickSound();
        EditorApplication.isPlaying = false;
#elif UNITY_ANDROID || UNITY_IOS
        SoundManager.Instance.PlayAllButtonClickSound();
        GameData.SaveGameData();
        Application.Quit();
#endif    
    }

    public void StopAllCoroutinesOfSingleTon()
    {
        BlockManager.Instance.StopAllCoroutines();
        InputManager.Instance.StopAllCoroutines();
    }
}
