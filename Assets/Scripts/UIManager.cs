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
    public FindableSpawner objectSpawner;
    public Shovel shovel;
    
    [Header("UI References")]
    public ToolDisplay toolDisplay;
    public Transform continueButton;
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
    [SerializeField] 
    Findables findable;

    GameObject findableContainer;
    float secondsSinceStart;
    int numberOfFindables;
    int findablesCollected;

    private void Awake()
    {
        if (instance == null || instance.gameObject == null)
            instance = this;

        if(player != null)
        {
            player.OnToolChange += toolDisplay.SetTool;
        }

        if(objectSpawner != null)
        {
            numberOfFindables = objectSpawner.objectsToSpawn.Count;
        }

        progress.text = "0/" + numberOfFindables;
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

    public void DisplayContinueUI(Findables _findable, FindableContainer container)
    {
        player.Frozen = true;
        findableContainer = container.gameObject;
        findable = _findable;
        objectName.text = _findable.objectName;
        objectDescription.text = _findable.description;
        continueButton.gameObject.SetActive(true);
    }
    
    public void DisableContinueUI()
    {
        player.SetTool(Tool.Detector);
        player.Frozen = false;
        shovel.foundFindable = false;

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
        SceneManager.LoadScene(1);
    }
}