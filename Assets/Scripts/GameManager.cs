using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsMove { get; set; } = false;
    public bool IsGameOver { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        Instance.IsGameOver = false;
        UIManager.Instance.UpdateBestScoreUI(ScoreManager.Instance.BestScore);
        UIManager.Instance.UpdateScoreUI(ScoreManager.Instance.CurrentScore);
    }

    public void GameOver()
    {
        IsGameOver = true;
        IsMove = false;
        Time.timeScale = 0f;
        StopAllCoroutinesOfSingleTon();
        UIManager.Instance.GameOver();
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        UIManager.Instance.gameTimer = UIManager.Instance.gameDuration;
        BlockManager.Instance.ClearBoard();
        ScoreManager.Instance.Init();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StopAllCoroutinesOfSingleTon()
    {
        BlockManager.Instance.StopAllCoroutines();
        InputManager.Instance.StopAllCoroutines();
    }
}
