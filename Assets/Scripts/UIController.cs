using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for User Interface control.
/// This includes: game over panel displayed when the ball misses the target and 
/// score counter displayed in upper right corner of the screen.
/// It subscribes to events invoked when ball hits or misses the target, defined 
/// in BallController.
/// It contains definition of event invoked when restart button is clicked.
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

    /// <summary>
    /// Initializes UI elements and class variables.
    /// </summary>
    private void Awake()
    {
        _gameOverPanel.SetActive(false);

        _gameoverScoreText = _gameOverPanel.transform.GetChild(1).GetComponent<Text>();
        _gameoverHighscoreText = _gameOverPanel.transform.GetChild(2).GetComponent<Text>();

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

    /// <summary>
    /// This method is assigned to GameOverRestartButton in Inspector.
    /// </summary>
    public void RestartGame()
    {
        onRestart();
        _gameOverPanel.SetActive(false);
        _scoreText.text = _INITIAL_SCORE_TEXT;
    }

    /// <summary>
    /// Called when the ball hits the target. It increments score stored in ScoreManager 
    /// and updates ScoreText;
    /// </summary>
    private void UpdateScore()
    {
        _scoreManager.Score++;
        _scoreText.text = _scoreManager.Score.ToString();
    }

    /// <summary>
    /// Updates GameOverScoreText with current score and checks if highscore should be updated.
    /// If it does the GameOverHighscore is also updated. Then sets GameOverPanel active.
    /// </summary>
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
