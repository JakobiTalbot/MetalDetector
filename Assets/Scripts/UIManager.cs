using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("References")]
    public PlayerController player;
    public Transform continueButton;


    [Header("UI References")]
    public ToolDisplay toolDisplay;
    [SerializeField]
    TextMeshProUGUI time;
    [SerializeField]
    TextMeshProUGUI progress;
    [SerializeField]
    TextMeshProUGUI objectName;
    [SerializeField]
    TextMeshProUGUI objectDescription;
    [SerializeField]
    GameObject gameOverUI;
    [SerializeField]
    TextMeshProUGUI gameOverTime;
    [Header("Numbers")]
    public int numberOfFindables;
    
    GameObject findableContainer;
    float secondsSinceStart;
    int findablesCollected;

    private void Awake()
    {
        if (instance == null || instance.gameObject == null)
            instance = this;
        progress.text = "0/" + numberOfFindables;

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

    public void DisplayContinueUI(Findables findable, FindableContainer container)
    {
        player.Frozen = true;
        findableContainer = container.gameObject;
        objectName.text = findable.objectName;
        objectDescription.text = findable.description;
        continueButton.gameObject.SetActive(true);
    }
    
    public void DisableContinueUI()
    {
        player.SetTool(Tool.Detector);
        player.Frozen = false;
        // add findable to inventory
        Destroy(findableContainer);
        continueButton.gameObject.SetActive(false);
        IncrementProgress();
    }

    public void IncrementProgress()
    {
        ++findablesCollected;
        progress.text = findablesCollected + "/" + numberOfFindables;

        // check win
        if (findablesCollected >= numberOfFindables)
        {
            Win();
        }
    }

    void Win()
    {
        toolDisplay.gameObject.SetActive(false);
        time.gameObject.SetActive(false);
        progress.gameObject.SetActive(false);
        player.Frozen = true;
        gameOverUI.SetActive(true);
        int minutes = (int)(secondsSinceStart / 60f);
        int seconds = (int)(secondsSinceStart % 60f);
        gameOverTime.text = minutes + ":" + (seconds < 10 ? "0" + seconds : seconds.ToString());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}