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
    [SerializeField] private int currentScore, highScore,pointByLevel;
    [SerializeField] private int nextLevelPoinst;
    [SerializeField] private int level;

    private void Awake()
    {
        level = 1;
        if(Instance == null) Instance = this;

        if (PlayerPrefs.HasKey("highScore")) highScore = PlayerPrefs.GetInt("highScore");
        highScoreText.text = $"High Score: {highScore}";
        scoreText.text = $"Score: {0}";
    }

    public void GetPoins(int poins)
    {

        currentScore += poins;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("highScore", highScore);
            //highScoreText.text = $"High Score: \n{highScore}";
        }
        //scoreText.text = $"Score: {currentScore}";

        if (currentScore >= nextLevelPoinst)
        {
            ChangeLevel();
        }
    }
    /// <summary>
    /// Facil se pasa de nivel,se mejora el spawn  
    /// </summary>
    public void ChangeLevel()
    {
        nextLevelPoinst += pointByLevel;
        level++;
        SpawnEnemy.Instance.NextLevel();
    }
}
