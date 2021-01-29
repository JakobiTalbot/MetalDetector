using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    int numberOfCollectables;
    [SerializeField]
    TextMeshProUGUI time;
    [SerializeField]
    TextMeshProUGUI progress;

    float secondsSinceStart;
    int numberCollected;

    void Start()
    {
        if (!instance.gameObject)
            instance = this;
        StartCoroutine(Count());
        progress.text = "0/" + numberOfCollectables;
    }

    IEnumerator Count()
    {
        while (true)
        {
            secondsSinceStart += Time.deltaTime;
            int minutes = (int)(secondsSinceStart / 60f);
            int seconds = (int)(secondsSinceStart % 60f);
            time.text = minutes + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString());

            yield return null;
        }
    }

    public void IncrementProgress()
    {
        ++numberCollected;
        progress.text = numberCollected + "/" + numberOfCollectables;
    }
}