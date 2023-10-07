using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsMove { get; set; } = false;
    public bool IsGameOver { get; private set; } = false;
    public float gameTimer;
    public float gameDuration = 2f;



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
    }

    private void Update()
    {
        //gameTimer += Time.deltaTime;
        //if(gameTimer > gameDuration)
        //{
        //    GameOver();
        //}
    }

    public void GameOver()
    {
        IsGameOver = true;
        IsMove = false;
        gameTimer = 0f;
        BlockManager.Instance.ClearBoard();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
