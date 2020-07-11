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
    [Header("UI Elements")]
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private Text _scoreText;

    private const string _HIGHSCORE_FILE_NAME = "hightscore.txt";

    private FileManager _fileManager = new FileManager();
    private int _score = 0;
    
    public delegate void Restart();
    public static event Restart onRestart;

    private void Awake()
    {
        _gameOverPanel.SetActive(false);
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
        _score = 0;
        _gameOverPanel.SetActive(false);
        _scoreText.text = "0";
    }

    private void UpdateScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }

    private void ShowGameOverScreen()
    {
        int bestScore = int.Parse(_fileManager.LoadString(_HIGHSCORE_FILE_NAME));
        Text[] textArray = _gameOverPanel.GetComponentsInChildren<Text>();
        textArray[1].text = "Score: " + _score;
        textArray[2].text = "Best: " + bestScore;
        _gameOverPanel.SetActive(true);

        if (_score > bestScore)
        {
            _fileManager.SaveString(_HIGHSCORE_FILE_NAME, _score.ToString());
        }
    }
}
