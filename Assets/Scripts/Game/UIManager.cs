using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private Slider hpBar;
    private Image hpBarFadeFrame;
    private TextMeshProUGUI bestScore;
    private TextMeshProUGUI score;

    private GameObject gameOverPanel;
    private GameObject pausePanel;

    private Button puaseButton;
    private Button selectStage;
    private Button restart;
    private Button pauseSelectStage;
    private Button continueButton;
    private Button quitButton;

    private Slider masterSlider;
    private Slider bgmSlider;
    private Slider seSlider;

    private Toggle masterMuteToggle;
    private Toggle bgmMuteToggle;
    private Toggle seMuteToggle;

    private GameObject[] itemSlots = new GameObject[3];

    private ItemTable itemTable;

    [SerializeField]
    private float gameTimerSpeed = 1f;

    private bool isFadeHpBar = false;

    public TextMeshProUGUI gameOverBestScore;
    public TextMeshProUGUI gameOverScore;

    public GameObject foreCanvas;

    public ItemID[] items = new ItemID[3];

    public float gameTimer;
    public float gameDuration = 20f;

    public float stopTimer = 0f;
    public float stopDuration = 0f;

    public float fadeSpeed = 5f;

    public float scoreItemTimer = 0f;
    public float scoreItemDuration = 0f;

    public bool isStopTimer = false;

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
    void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        if (Instance == null)
            return;

        if (isStopTimer)
            stopTimer += Time.deltaTime;
        else
            gameTimer -= Time.deltaTime * gameTimerSpeed;


        if(stopTimer > stopDuration)
        {
            stopTimer = 0f;
            isStopTimer = false;
            ApplyOriginTimerImage();
        }

        float t = gameTimer / gameDuration;
        // 에러 발생
        if (BlockManager.Instance != null)
        {
            if (t <= BlockManager.Instance.obsSpawnTimeRate && !BlockManager.Instance.isSpawnObstacle)
                BlockManager.Instance.isSpawnObstacle = true;
        }

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

        if(ScoreManager.Instance.IsScoreIncreaseByItem)
        {
            scoreItemTimer += Time.deltaTime;
        }

        if(scoreItemTimer > scoreItemDuration)
        {
            scoreItemTimer = 0f;
            ScoreManager.Instance.IsScoreIncreaseByItem = false;
        }

        if (hpBar != null)
            UpdateTimerUI(t);
        if (gameTimer <= 0f && !GameManager.Instance.IsGameOver)
        {
            GameManager.Instance.GameOver();
        }
    }
    private void Init()
    {
        itemTable = DataTableManager.GetTable<ItemTable>();

        int stage = PlayerPrefs.GetInt("CurrentStage", 1);
        switch (stage)
        {
            case 1:
                gameDuration = 60;
                break;
            case 0:
                gameDuration = 180;
                break;
            case 2:
                gameDuration = 180;
                break;
            default:
                gameDuration = 60;
                break;
        }

        for(int i = 0; i < itemSlots.Length; ++i)
        {
            itemSlots[i] = GameObject.FindGameObjectWithTag($"ItemSlot{i + 1}");
            ApplyItemSlotImage(itemSlots[i], (ItemID)GameData.Slots[i]);
        }

        isStopTimer = false;

        gameTimer = gameDuration;

        hpBar = GameObject.FindGameObjectWithTag("HpBar").GetComponent<Slider>();
        hpBar.onValueChanged.AddListener(UpdateTimerUI);
        hpBarFadeFrame = hpBar.transform.GetChild(2).GetComponent<Image>();
        bestScore = GameObject.FindGameObjectWithTag("BestScore").GetComponent<TextMeshProUGUI>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        foreCanvas = GameObject.FindGameObjectWithTag("ForeCanvas");
        puaseButton = GameObject.FindGameObjectWithTag("Pause").GetComponent<Button>();

        gameOverPanel = foreCanvas.transform.GetChild(1).gameObject;
        gameOverPanel.gameObject.SetActive(false);
        pausePanel = foreCanvas.transform.GetChild(2).gameObject;
        pausePanel.gameObject.SetActive(false);

        gameOverBestScore = gameOverPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        gameOverScore = gameOverPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        masterSlider = pausePanel.transform.GetChild(4).GetComponent<Slider>();
        bgmSlider = pausePanel.transform.GetChild(5).GetComponent<Slider>();
        seSlider = pausePanel.transform.GetChild(6).GetComponent<Slider>();

        masterMuteToggle = masterSlider.transform.GetChild(0).GetComponent<Toggle>();
        bgmMuteToggle = bgmSlider.transform.GetChild(0).GetComponent<Toggle>();
        seMuteToggle = seSlider.transform.GetChild(0).GetComponent<Toggle>();

        masterSlider.onValueChanged.AddListener(SoundManager.Instance.SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBGMVolume);
        seSlider.onValueChanged.AddListener(SoundManager.Instance.SetSEVolume);

        masterMuteToggle.onValueChanged.AddListener(SoundManager.Instance.OnOffMasterVolume);
        bgmMuteToggle.onValueChanged.AddListener(SoundManager.Instance.OnOffBGMVolume);
        seMuteToggle.onValueChanged.AddListener(SoundManager.Instance.OnOffSEVolume);

        masterSlider.value = GameData.MasterVolume;
        bgmSlider.value = GameData.BGMVolune;
        seSlider.value = GameData.SEVolume;

        masterMuteToggle.isOn = GameData.IsOffMasterMute;
        bgmMuteToggle.isOn = GameData.IsOffBGMMute;
        seMuteToggle.isOn = GameData.IsOffSEMute;

        if (GameManager.Instance != null)
        {
            puaseButton.onClick.AddListener(GameManager.Instance.Pause);

            selectStage = gameOverPanel.transform.GetChild(3).GetChild(0).GetComponent<Button>();
            selectStage.onClick.AddListener(GameManager.Instance.SelectStage);
            restart = gameOverPanel.transform.GetChild(4).GetChild(0).GetComponent<Button>();
            restart.onClick.AddListener(GameManager.Instance.Restart);

            pauseSelectStage = pausePanel.transform.GetChild(1).GetChild(0).GetComponent<Button>();
            pauseSelectStage.onClick.AddListener(GameManager.Instance.SelectStage);

            continueButton = pausePanel.transform.GetChild(2).GetChild(0).GetComponent<Button>(); ;
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


            quitButton = pausePanel.transform.GetChild(3).GetChild(0).GetComponent<Button>(); ;
            quitButton.onClick.AddListener(GameManager.Instance.Quit);
        }
    }
    private IEnumerator ContinueDelay()
    {
        yield return new WaitForSeconds(0.5f);
        InputManager.Instance.isHover = false;
    }
    private void FadeHpBarFrame()
    {
        float alpha = Mathf.PingPong(Time.time * fadeSpeed, 1f);
        Color newColor = hpBarFadeFrame.color;
        newColor.a = alpha;
        hpBarFadeFrame.color = newColor;
    }
    private void ApplyOriginTimerImage()
    {
        Image timerFillImage = GameObject.FindGameObjectWithTag("TimerFill").GetComponent<Image>();
        timerFillImage.sprite = Resources.Load<Sprite>("Arts/ProgressbarEmpty");
        timerFillImage.color = Color.red;
    }
    private void UpdateTimerUI(float value)
    {
        if (hpBar != null)
            hpBar.value = value;
    }
    private void ApplyItemSlotImage(GameObject slot, ItemID itemID)
    {
        if (itemID == ItemID.None)
            return;
        Image itemSlotImage = slot.transform.GetChild(1).GetComponent<Image>();
        string itemImagePath = itemTable.GetItemInfo(itemID).path;
        itemSlotImage.sprite = Resources.Load<Sprite>($"Arts/{itemImagePath}");
        Color iamgeColor = itemSlotImage.color;
        iamgeColor.a = 255;
        itemSlotImage.color = iamgeColor;
    }
    public void GameOver()
    {
        isStopTimer = false;
        gameOverPanel.gameObject.SetActive(true);
        ScoreManager.Instance.UpdateBestScore();
        gameOverBestScore.text = $"{GameData.BestScore}";
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
    public void IncreaseTimer(float value)
    {
        gameTimer += value;
        Mathf.Clamp(gameTimer, 0f, gameDuration);
    }
    public void ApplyStopTimerImage()
    {
        Image timerFillImage = GameObject.FindGameObjectWithTag("TimerFill").GetComponent<Image>();
        timerFillImage.sprite = Resources.Load<Sprite>("Arts/TimerStop");
        timerFillImage.color = Color.white;
    }
    public void UpdateBestScoreUI(float value)
    {
        bestScore.text = $"BEST SCORE \n {Mathf.RoundToInt(value)}";
    }
    public void UpdateScoreUI(float value)
    {
        score.text = $"SCORE \n {Mathf.RoundToInt(value)}";
    }
}
