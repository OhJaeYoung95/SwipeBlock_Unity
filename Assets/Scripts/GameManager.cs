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
            Destroy(this);

        IsGameOver = false;
    }

    public void GameOver()
    {
        IsGameOver = true;
        IsMove = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
