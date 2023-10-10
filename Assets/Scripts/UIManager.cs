using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private Slider hpBar;
    private Image hpBarFadeFrame;
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

    public float fadeSpeed = 5f;
    private bool isFadeHpBar = false;

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

        float t = gameTimer / gameDuration;
        if (t <= 0.5f && !BlockManager.Instance.isSpawnObstacle)
            BlockManager.Instance.isSpawnObstacle = true;

        if (t <= 0.1f && !isFadeHpBar)
        {
            isFadeHpBar = true;
            hpBarFadeFrame.gameObject.SetActive(true);
        }

        if (t > 0.1f && isFadeHpBar)
        {
            isFadeHpBar = false;
            hpBarFadeFrame.gameObject.SetActive(false);
        }

        if (isFadeHpBar)
            FadeHpBarFrame();



        UpdateTimerUI(t);
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
                gameDuration = 30;
                break;
            case 0:
                gameDuration = 240;
                break;
            case 2:
                gameDuration = 300;
                break;
            default:
                gameDuration = 30;
                break;
        }



        gameTimer = gameDuration;

        hpBar = GameObject.FindGameObjectWithTag("HpBar").GetComponent<Slider>();
        hpBarFadeFrame = hpBar.transform.GetChild(2).GetComponent<Image>();
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

    public void Pause()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        StartCoroutine(ContinueDelay());
    }

    public IEnumerator ContinueDelay()
    {
        yield return new WaitForSeconds(0.5f);
        InputManager.Instance.isHover = false;
    }

    public void FadeHpBarFrame()
    {
        float alpha = Mathf.PingPong(Time.time * fadeSpeed, 1f);
        Color newColor = hpBarFadeFrame.color;
        newColor.a = alpha;
        hpBarFadeFrame.color = newColor;
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
