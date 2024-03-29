using UnityEngine;
using GooglePlayGames;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField]
    private float baseScore = 10f;
    [SerializeField]
    private float chainMergeScore = 40f;
    [SerializeField]
    private float comboScore = 60f;
    [SerializeField]
    private float compareScore = 70f;

    public float BestScore { get; set; }
    public float CurrentScore { get; set; }
    public float itemValue = 0;

    public bool IsScoreIncreaseByPattern { get; set; } = false;
    public bool IsScoreIncreaseByItem { get; set; } = false;

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
    public void Init()
    {
        UpdateBestScore();
        UIManager.Instance.UpdateBestScoreUI(GameData.BestScore);
        IsScoreIncreaseByPattern = false;
        IsScoreIncreaseByItem = false;
        CurrentScore = 0f;
    }
    public void UpdateBestScore()
    {
        GameData.BestScore = CurrentScore > GameData.BestScore ? CurrentScore : GameData.BestScore;
        PlayGamesPlatform.Instance.ReportScore((long)GameData.BestScore, "CgkIl-aKn6IbEAIQAQ", (bool success) => { });
    }
    public void ConvertScoreToGold()
    {
        switch(GameData.CurrentStage)
        {
            case 0:
                GameData.Gold += Mathf.RoundToInt(CurrentScore / 100);
                break;
            case 1:
                GameData.Gold += Mathf.RoundToInt(CurrentScore / 10);
                break;
            case 2:
                GameData.Gold += Mathf.RoundToInt(CurrentScore * 2 / 10);
                break;
            default:
                GameData.Gold += Mathf.RoundToInt(CurrentScore / 100);
                break;
        }
    }
    public float AddScoreBase()
    {
        return ApplyScore(baseScore);
    }
    public float AddScoreByConnected(int count)
    {
        int factor = count - 2;
        return ApplyScore(chainMergeScore * factor);
    }
    public float AddScoreByCombo()
    {
        return ApplyScore(comboScore);
    }
    public float AddScoreByComparePattern()
    {
        return ApplyScore(compareScore);
    }
    private float ApplyScore(float score)
    {
        float scoreTextValue = 0f;
        if (IsScoreIncreaseByPattern)
        {
            CurrentScore += score * 1.5f;
            scoreTextValue += score * 1.5f;
        }
        if (IsScoreIncreaseByItem)
        {
            CurrentScore += score * itemValue;
            scoreTextValue += score * itemValue;
        }

        if (!IsScoreIncreaseByPattern && !IsScoreIncreaseByItem)
        {
            CurrentScore += score;
            scoreTextValue += score;
        }
        
        UIManager.Instance.UpdateScoreUI(CurrentScore);
        return scoreTextValue;
    }
}
