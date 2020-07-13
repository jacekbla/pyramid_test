using System;
using System.IO;
using UnityEngine;

/// <summary>
/// This class is responsible for storing and updating score and highscore.
/// It stores highscore in persistentDataPath and updates it when called in UIController.
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

    public ScoreManager()
    {
        _highscore = int.Parse(LoadString());
        UIController.onRestart += ResetScore;
    }

    public bool UpdateHighscore()
    {
        if(_score > _highscore)
        {
            _highscore = _score;
            SaveString(_highscore.ToString());
            return true;
        }
        return false;
    }

    private void ResetScore()
    {
        _score = 0;
    }

    private void SaveString(string p_data)
    {
        string path = Path.Combine(Application.persistentDataPath, _HIGHSCORE_FILE_NAME);

        try
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(p_data);
            }
        }
        catch (IOException e)
        {
            Debug.LogException(e);
        }
    }

    private string LoadString()
    {
        string path = Path.Combine(Application.persistentDataPath, _HIGHSCORE_FILE_NAME);
        string result = _DEFAULT_LOAD_RESULT;

        try
        {
            if (!File.Exists(path))
            {
                return result;
            }

            using (StreamReader reader = new StreamReader(path))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        catch (IOException e)
        {
            Debug.LogException(e);
            return result;
        }
    }
}

