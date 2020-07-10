using System.IO;
using UnityEngine;

class FileManager
{
    public void SaveString(string p_fileName, string p_data)
    {
        string path = Application.persistentDataPath + "/" + p_fileName;

        if(!File.Exists(path))
        {
            File.Create(path);
        }

        using (StreamWriter writer = new StreamWriter(path, false))
        {
            writer.Write(p_data);
        }
    }

    public string LoadString(string p_fileName)
    {
        string path = Application.persistentDataPath + "/" + p_fileName;
        string result;
        
        if (!File.Exists(path))
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write("0");
            }
            return "0";
        }
        
        using (StreamReader reader = new StreamReader(path))
        {
            result = reader.ReadToEnd();
        }

        return result;
    }
}

