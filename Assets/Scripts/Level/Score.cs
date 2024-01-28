using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance { get; set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Next level")]
    private int nextLevelPoinst;
    private int level;
    public int CurrentScore, HighScore;

    private void Awake()
    {
        level = 1;
        if(Instance == null) Instance = this;

        if (PlayerPrefs.HasKey("HighScore")) HighScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = $"High Score: {HighScore}";
        scoreText.text = $"Score: {0}";
    }

    public void GetPoins(int poins)
    {

        CurrentScore += poins;

        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            PlayerPrefs.SetInt("HighScore", HighScore);
            highScoreText.text = $"High Score: \n{HighScore}";
        }
        scoreText.text = $"Score: {CurrentScore}";

        if (CurrentScore >= nextLevelPoinst)
        {
            ChangeLevel();
        }
    }
    /// <summary>
    /// Facil se pasa de nivel,se mejora el spawn  
    /// </summary>
    public void ChangeLevel()
    {
        nextLevelPoinst += 10;
        level++;
        SpawnEnemy.Instance.NextLevel();
    }
}
