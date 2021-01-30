using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("References")]
    public PlayerController player;

    [Header("UI References")]
    public ToolDisplay toolDisplay;
    [SerializeField]
    TextMeshProUGUI time;
    [SerializeField]
    TextMeshProUGUI progress;

    [Header("Numbers")]
    [SerializeField]
    int numberOfCollectables;

    float secondsSinceStart;
    int numberCollected;

    private void Awake()
    {
        if (instance == null || instance.gameObject == null)
            instance = this;
        progress.text = "0/" + numberOfCollectables;

        if(player != null)
        {
            player.OnToolChange += toolDisplay.SetTool;
        }
    }

    void Start()
    {
        StartCoroutine(Count());
    }

    private void OnDisable()
    {
        if(player != null)
        {
            player.OnToolChange -= toolDisplay.SetTool;
        }
    }

    IEnumerator Count()
    {
        while (true)
        {
            secondsSinceStart += Time.deltaTime;
            int minutes = (int)(secondsSinceStart / 60f);
            int seconds = (int)(secondsSinceStart % 60f);
            time.text = minutes + ":" + (seconds < 10 ? "0" + seconds : seconds.ToString());

            yield return null;
        }
    }

    public void IncrementProgress()
    {
        ++numberCollected;
        progress.text = numberCollected + "/" + numberOfCollectables;
    }
}