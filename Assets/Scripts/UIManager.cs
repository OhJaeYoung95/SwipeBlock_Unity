using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private Slider hpBar;
    private TextMeshProUGUI bestScore;
    private TextMeshProUGUI score;

    public TextMeshProUGUI gameOverBestScore;
    public TextMeshProUGUI gameOverScore;

    private GameObject canvas;
    private GameObject gameOverPanel;

    private Button selectStage;
    private Button restart;

    public float gameTimer;
    public float gameDuration = 20f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer -= Time.deltaTime;
        UpdateTimerUI(gameTimer / gameDuration);
        if (gameTimer <= 0f)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void Init()
    {
        gameTimer = gameDuration;

        hpBar = GameObject.FindGameObjectWithTag("HpBar").GetComponent<Slider>();
        bestScore = GameObject.FindGameObjectWithTag("BestScore").GetComponent<TextMeshProUGUI>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        gameOverPanel = canvas.transform.GetChild(2).gameObject;
        gameOverPanel.gameObject.SetActive(false);
        gameOverBestScore = gameOverPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        gameOverScore = gameOverPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>();


        if (GameManager.Instance != null)
        {
            selectStage = gameOverPanel.transform.GetChild(3).GetComponent<Button>();
            selectStage.onClick.AddListener(GameManager.Instance.Restart);
            restart = gameOverPanel.transform.GetChild(4).GetComponent<Button>();
            restart.onClick.AddListener(GameManager.Instance.Restart);
        }
    }

    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        ScoreManager.Instance.UpdateBestScore();
        gameOverBestScore.text = $"{ScoreManager.Instance.BestScore}";
        gameOverScore.text = $"{ScoreManager.Instance.CurrentScore}";
    }

    public void UpdateTimerUI(float value)
    {
        hpBar.value = value;
    }

    public void UpdateBestScoreUI(int value)
    {
        bestScore.text = $"BEST SCORE \n {value}";
    }
    public void UpdateScoreUI(int value)
    {
        score.text = $"SCORE \n {value}";
    }
}
