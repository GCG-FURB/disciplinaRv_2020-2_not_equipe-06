using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    private static GameOverWindow _instance;

    private const string SCORE_TEXT = "ScoreText";
    private const string RETRY_BUTTON = "RetryButton";
    private const string MENU_BUTTON = "MenuButton";
    private Text _scoreText;

    private void Awake()
    {
        _instance = this;
        _scoreText = transform.Find(SCORE_TEXT).GetComponent<Text>();

        transform.Find(RETRY_BUTTON).GetComponent<Button_UI>().ClickFunc = () =>
        {
            Loader.Load(Scene.Game);
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
            _scoreText.text = level.GetPoints().ToString();
    }

    public static GameOverWindow GetInstance() => _instance;

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
