using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public float BestScore { get; set; }
    public float CurrentScore { get; set; }
    [SerializeField]
    private float baseScore = 10f;
    [SerializeField]
    private float chainMergeScore = 20f;
    [SerializeField]
    private float comboScore = 30f;
    [SerializeField]
    private float compareScore = 40f;

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
        UIManager.Instance.UpdateBestScoreUI(BestScore);
        IsScoreIncreaseByPattern = false;
        IsScoreIncreaseByItem = false;
        CurrentScore = 0f;
    }

    public void UpdateBestScore()
    {
        BestScore = CurrentScore > BestScore ? CurrentScore : BestScore;
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
        Debug.Log("Combo");
        return ApplyScore(comboScore);
    }

    public float AddScoreByComparePattern()
    {
        Debug.Log("Compare");
        return ApplyScore(compareScore);
    }

    public float ApplyScore(float score)
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
