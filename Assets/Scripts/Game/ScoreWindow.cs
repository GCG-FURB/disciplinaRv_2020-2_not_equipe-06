using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    private const string SCORE_TEXT = "ScoreText";
    private const string HIGHSCORE_TEXT = "HighScoreText";

    private Text _highscoreText;
    private Text _scoreText;

    private void Awake()
    {
        _scoreText = transform.Find(SCORE_TEXT).GetComponent<Text>();
        _highscoreText = transform.Find(HIGHSCORE_TEXT).GetComponent<Text>();
    }

    private void Start()
    {
        _highscoreText.text = $"HIGHSCORE: {Score.GetHighScore()}";
    }

    private void Update()
    {
        if (Level.GetInstance() != null)
            _scoreText.text = Level.GetInstance().GetPoints().ToString();
    }
}
