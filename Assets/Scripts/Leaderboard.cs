using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine; 


[Serializable]
public class ScoreSaveData
{
    public List<Score> highScores = new List<Score>();
}

[Serializable]
public class Score
{
    public string playerName;
    public float time;

    public void Initialise(string _playerName, float _time)
    {
        playerName = _playerName;
        time = _time;
    }
}

public class Leaderboard : MonoBehaviour
{
    [SerializeField] int maxScores;
    [SerializeField] GameObject scoreboardEntryObject;

    private string savePath => $"{Application.persistentDataPath}/Score.json";

    private void Start()
    {
        ScoreSaveData savedScores = GetSavedScores();
        
        SaveScores(savedScores);
        UpdateUI(savedScores);
    }

    ScoreSaveData GetSavedScores()
    {
        if (!File.Exists(savePath))
        {
            File.Create(savePath).Dispose();
            return new ScoreSaveData();
        }

        using (StreamReader stream = new StreamReader(savePath))
        {
            string json = stream.ReadToEnd();
            return JsonUtility.FromJson<ScoreSaveData>(json);
        }
    }
    
    public void AddEntry(Score scoreboardEntryData)
    {
        ScoreSaveData savedScores = GetSavedScores();

        bool scoreAdded = false;

        for (int i = 0; i < savedScores.highScores.Count; i++)
        {
            if (scoreboardEntryData.time > savedScores.highScores[i].time)
            {
                savedScores.highScores.Insert(i, scoreboardEntryData);
                scoreAdded = true;
                break;
            }
        }

        if (!scoreAdded && savedScores.highScores.Count < maxScores)
        {
            savedScores.highScores.Add(scoreboardEntryData);
        }

        if (savedScores.highScores.Count > maxScores)
        {
            savedScores.highScores.RemoveRange(maxScores, savedScores.highScores.Count - maxScores);
        }

        UpdateUI(savedScores);

        SaveScores(savedScores);
    }

    void SaveScores(ScoreSaveData scoreSaveData)
    {
        using (StreamWriter stream = new StreamWriter(savePath))
        {
            string json = JsonUtility.ToJson(scoreSaveData, true);
            stream.Write(json);
        }
    }
    
    private void UpdateUI(ScoreSaveData savedScores)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Score highscore in savedScores.highScores)
        {
            Instantiate(scoreboardEntryObject, transform).GetComponent<LeaderboardUI>().Initialise(highscore);
        }
    }
}
