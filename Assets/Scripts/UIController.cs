using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for User Interface control.
/// This includes: game over panel displayed when the ball misses the target
/// and score counter displayed in upper right corner of the screen.
/// It subscribes to events invoked when ball hits the target or misses it, defined 
/// in BallController.
/// </summary>
public class UIController : MonoBehaviour
{
    public static event Action onRestart;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private Text _scoreText;

    private Text _gameoverHighscoreText;
    private Text _gameoverScoreText;

    private const string _INITIAL_SCORE_TEXT = "0";
    private const string _GAMEOVER_SCORE_CONSTANT_TEXT = "Score: ";
    private const string _GAMEOVER_HIGHTSCORE_CONSTANT_TEXT = "Best: ";

    private ScoreManager _scoreManager;

    private void Awake()
    {
        _gameOverPanel.SetActive(false);

        Text[] texts = _gameOverPanel.GetComponentsInChildren<Text>();
        _gameoverScoreText = texts[1];
        _gameoverHighscoreText = texts[2];

        _scoreManager = new ScoreManager();
        _gameoverHighscoreText.text = _GAMEOVER_HIGHTSCORE_CONSTANT_TEXT + _scoreManager.Highscore;

    }

    private void OnEnable()
    {
        BallController.onHit += UpdateScore;
        BallController.onMiss += ShowGameOverScreen;
    }

    private void OnDisable()
    {
        BallController.onHit -= UpdateScore;
        BallController.onMiss -= ShowGameOverScreen;
    }

    public void RestartGame()
    {
        onRestart();
        _gameOverPanel.SetActive(false);
        _scoreText.text = _INITIAL_SCORE_TEXT;
    }

    private void UpdateScore()
    {
        _scoreManager.Score++;
        _scoreText.text = _scoreManager.Score.ToString();
    }

    private void ShowGameOverScreen()
    {
        _gameoverScoreText.text = _GAMEOVER_SCORE_CONSTANT_TEXT + _scoreManager.Score;
        if (_scoreManager.UpdateHighscore())
        {
            _gameoverHighscoreText.text = _GAMEOVER_HIGHTSCORE_CONSTANT_TEXT + _scoreManager.Highscore;
        }

        _gameOverPanel.SetActive(true);
    }
}
