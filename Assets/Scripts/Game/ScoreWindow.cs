using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    private const string SCORE_TEXT = "ScoreText";
    private Text _scoreText;

    private void Awake()
    {
        _scoreText = transform.Find(SCORE_TEXT).GetComponent<Text>();
    }

    private void Update()
    {
        if (Level.GetInstance() != null)
            _scoreText.text = Level.GetInstance().GetPoints().ToString();
    }
}
