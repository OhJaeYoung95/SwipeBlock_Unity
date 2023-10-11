using System.Collections;
using System.Collections.Generic;
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

    public bool IsScoreIncrease { get; set; } = false;

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
    }

    public void Init()
    {
        UpdateBestScore();
        UIManager.Instance.UpdateBestScoreUI(BestScore);
        CurrentScore = 0f;
    }

    public void UpdateBestScore()
    {
        BestScore = CurrentScore > BestScore ? CurrentScore : BestScore;
    }

    public void AddScoreBase()
    {
        if(IsScoreIncrease)
        {
            Debug.Log(baseScore * 1.5f);

            CurrentScore += baseScore * 1.5f;
        }
        else
            CurrentScore += baseScore;
        UIManager.Instance.UpdateScoreUI(CurrentScore);
    }

    public void AddScoreByConnected(int count)
    {
        int factor = count - 2;

        if (IsScoreIncrease)
        {
            Debug.Log(chainMergeScore * factor * 1.5f);

            CurrentScore += chainMergeScore * factor * 1.5f;
        }
        else
            CurrentScore += chainMergeScore * factor;

        UIManager.Instance.UpdateScoreUI(CurrentScore);
    }

    public void AddScoreByCombo()
    {
        Debug.Log("Combo");
        if(IsScoreIncrease)
        {
            Debug.Log(comboScore * 1.5f);

            CurrentScore += comboScore * 1.5f;
        }
        else
            CurrentScore += comboScore;
        UIManager.Instance.UpdateScoreUI(CurrentScore);
    }

    public void AddScoreByComparePattern()
    {
        Debug.Log("Compare");
        if(IsScoreIncrease)
        {
            Debug.Log(compareScore * 1.5f);
            CurrentScore += compareScore * 1.5f;
        }
        else
            CurrentScore += compareScore;
        UIManager.Instance.UpdateScoreUI(CurrentScore);
    }
}
