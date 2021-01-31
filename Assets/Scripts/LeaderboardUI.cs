using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI entryNameText = null;
    [SerializeField] private TextMeshProUGUI entryScoreText = null;

    public void Initialise(Score scoreboardEntryData)
    {
        entryNameText.text = scoreboardEntryData.playerName;
        entryScoreText.text = scoreboardEntryData.time.ToString();
    }
}
