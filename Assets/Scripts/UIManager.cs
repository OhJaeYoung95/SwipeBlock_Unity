using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

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
    private GameObject pausePanel;

    private Button puaseButton;

    private Button selectStage;
    private Button restart;

    private Button pauseSelectStage;
    private Button continueButton;
    private Button quitButton;

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
        int stage = PlayerPrefs.GetInt("CurrentStage", 1);
        switch (stage)
        {
            case 1:
                gameDuration = 180;
                break;
            case 0:
                gameDuration = 240;
                break;
            case 2:
                gameDuration = 300;
                break;
            default:
                gameDuration = 180;
                break;
        }



        gameTimer = gameDuration;

        hpBar = GameObject.FindGameObjectWithTag("HpBar").GetComponent<Slider>();
        bestScore = GameObject.FindGameObjectWithTag("BestScore").GetComponent<TextMeshProUGUI>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        puaseButton = GameObject.FindGameObjectWithTag("Pause").GetComponent<Button>();

        gameOverPanel = canvas.transform.GetChild(2).gameObject;
        gameOverPanel.gameObject.SetActive(false);
        pausePanel = canvas.transform.GetChild(3).gameObject;
        pausePanel.gameObject.SetActive(false);

        gameOverBestScore = gameOverPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        gameOverScore = gameOverPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        if (GameManager.Instance != null)
        {
            puaseButton.onClick.AddListener(GameManager.Instance.Pause);

            selectStage = gameOverPanel.transform.GetChild(3).GetComponent<Button>();
            selectStage.onClick.AddListener(GameManager.Instance.SelectStage);
            restart = gameOverPanel.transform.GetChild(4).GetComponent<Button>();
            restart.onClick.AddListener(GameManager.Instance.Restart);

            pauseSelectStage = pausePanel.transform.GetChild(1).GetComponent<Button>();
            pauseSelectStage.onClick.AddListener(GameManager.Instance.SelectStage);

            continueButton = pausePanel.transform.GetChild(2).GetComponent<Button>(); ;
            continueButton.onClick.AddListener(GameManager.Instance.Continue);
            EventTrigger eventTrigger = continueButton.GetComponent<EventTrigger>();
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => { InputManager.Instance.OnButtonHoverEnter(); });
            eventTrigger.triggers.Add(enterEntry);
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { InputManager.Instance.OnButtonHoverExit(); });
            eventTrigger.triggers.Add(exitEntry);


            quitButton = pausePanel.transform.GetChild(3).GetComponent<Button>(); ;
            quitButton.onClick.AddListener(GameManager.Instance.Quit);

        }
    }

    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        ScoreManager.Instance.UpdateBestScore();
        gameOverBestScore.text = $"{ScoreManager.Instance.BestScore}";
        gameOverScore.text = $"{ScoreManager.Instance.CurrentScore}";
    }

    public void Puase()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
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
