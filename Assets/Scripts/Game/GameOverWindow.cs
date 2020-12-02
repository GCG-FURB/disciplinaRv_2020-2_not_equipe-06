using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    private static GameOverWindow _instance;

    private const string SCORE_TEXT = "ScoreText";
    private const string HIGHSCORE_TEXT = "HighScoreText";
    private const string RETRY_BUTTON = "RetryButton";
    private const string MENU_BUTTON = "MenuButton";
    private Text _scoreText;
    private Text _highscoreText;

    private void Awake()
    {
        _instance = this;
        _scoreText = transform.Find(SCORE_TEXT).GetComponent<Text>();
        _highscoreText = transform.Find(HIGHSCORE_TEXT).GetComponent<Text>();

        transform.Find(RETRY_BUTTON).GetComponent<Button_UI>().ClickFunc = () =>
        {
            Level.GetInstance().Restart();
        };

        transform.Find(MENU_BUTTON).GetComponent<Button_UI>().ClickFunc = () =>
        {
            Loader.Load(Scene.Menu);
        };

        Hide();
    }

    private void Update()
    {
        var level = Level.GetInstance();

        if (level != null && level.GetState() == GameState.Dead)
        {
            _scoreText.text = level.GetPoints().ToString();
            _highscoreText.text = $"HIGHSCORE: {Score.GetHighScore()}";
        }
    }

    public static GameOverWindow GetInstance() => _instance;

    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);
}
