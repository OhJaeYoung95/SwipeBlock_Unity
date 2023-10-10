using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int BestScore { get; set; }
    public int CurrentScore { get; set; }
    private int baseScore = 10;
    private int chainMergeScore = 20;
    private int comboScore = 30;
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
        CurrentScore = 0;
    }

    public void UpdateBestScore()
    {
        BestScore = CurrentScore > BestScore ? CurrentScore : BestScore;
    }

    public void AddScoreBase()
    {
        CurrentScore += baseScore;
        UIManager.Instance.UpdateScoreUI(CurrentScore);
    }

    public void AddScoreByConnected(int count)
    {
        int factor = count - 2;
        CurrentScore += chainMergeScore * factor;
        UIManager.Instance.UpdateScoreUI(CurrentScore);
    }

    public void AddScoreByCombo()
    {
        Debug.Log("Combo");
        CurrentScore += comboScore;
        UIManager.Instance.UpdateScoreUI(CurrentScore);
    }
}
