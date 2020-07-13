using System;
using System.IO;
using UnityEngine;

/// <summary>
/// This class is responsible for storing and updating score and highscore.
/// It stores highscore in file located accesed with persistentDataPath and 
/// updates it when called in UIController.
/// </summary>
class ScoreManager
{
    private const string _HIGHSCORE_FILE_NAME = "hightscore.txt";
    private const string _DEFAULT_LOAD_RESULT = "0";

    private int _score = 0;
    private int _highscore = -1;

    public int Score
    {
        get
        {
            return _score;
        }

        set
        {
            if(value < 0)
            {
                throw new ArgumentOutOfRangeException("Score cannot be negative.");
            }
            _score = value;
        }
    }
    public int Highscore
    {
        get
        {
            return _highscore;
        }
    }

    /// <summary>
    /// Constructor - loads highscore from file to variable and subscribes to onRestart event.
    /// </summary>
    public ScoreManager()
    {
        _highscore = LoadHighscore();
        UIController.onRestart += ResetScore;
    }
    
    /// <summary>
    /// Checks if current score is the new highscore, updates and saves it to file if needed.
    /// </summary>
    /// <returns>bool - true when highscore is updated, false otherwise</returns>
    public bool UpdateHighscore()
    {
        if(_score > _highscore)
        {
            _highscore = _score;
            SaveHighscore(_highscore);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Simple method that resets current score back to 0.
    /// </summary>
    private void ResetScore()
    {
        _score = 0;
    }

    /// <summary>
    /// This method tries to save given value in highscore file.
    /// </summary>
    /// <param name="p_data">int - highscore value to be stored</param>
    private void SaveHighscore(int p_data)
    {
        string path = Path.Combine(Application.persistentDataPath, _HIGHSCORE_FILE_NAME);
        string data = p_data.ToString();

        try
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(data);
            }
        }
        catch (IOException e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// This method tries to load data from highscore file.
    /// </summary>
    /// <returns>int - highscore from file or _DEFAULT_LOAD_RESULT when it could not be loaded</returns>
    private int LoadHighscore()
    {
        string path = Path.Combine(Application.persistentDataPath, _HIGHSCORE_FILE_NAME);
        string result = _DEFAULT_LOAD_RESULT;

        try
        {
            if (!File.Exists(path))
            {
                return int.Parse(result);
            }

            using (StreamReader reader = new StreamReader(path))
            {
                result = reader.ReadToEnd();
            }
            return int.Parse(result);
        }
        catch (IOException e)
        {
            Debug.LogException(e);
            return int.Parse(result);
        }
    }
}

