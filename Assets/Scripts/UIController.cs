using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private Text _scoreText;

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
        BallController.onWin += updateScore;
        BallController.onFail += gameOverScreen;
    }

    public void RestartGame()
    {
        onRestart();
        _score = 0;
        _gameOverPanel.SetActive(false);
        _scoreText.text = "0";
    }
    
    private void OnDisable()
    {
        BallController.onWin -= updateScore;
        BallController.onFail -= gameOverScreen;
    }

    private void updateScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }

    private void gameOverScreen()
    {
        int bestScore = int.Parse(_fileManager.LoadString("highscore.txt"));
        Text[] textArray = _gameOverPanel.GetComponentsInChildren<Text>();
        textArray[1].text = "Score: " + _score;
        textArray[2].text = "Best: " + bestScore;
        _gameOverPanel.SetActive(true);

        if (_score > bestScore)
        {
            _fileManager.SaveString("highscore.txt", _score.ToString());
        }
    }
}
