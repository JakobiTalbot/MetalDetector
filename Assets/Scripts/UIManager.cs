using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine.Serialization;

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
    [SerializeField]
    TextMeshProUGUI objectName;
    [SerializeField]
    TextMeshProUGUI objectDescription;
    [SerializeField]
    Transform continueButton;

    [Header("Numbers")]
    [SerializeField]
    int numberOfCollectables;
    
    GameObject findableContainer;
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

    public void DisplayContinueUI(Findables findable, FindableContainer container)
    {
        //player.gameObject.SetActive(false);
        findableContainer = container.gameObject;
        objectName.text = findable.objectName;
        objectDescription.text = findable.description;
        continueButton.gameObject.SetActive(true);
    }
    
    public void DisableContinueUI()
    {
        //player.gameObject.SetActive(true);
        // add findable to inventory
        findableContainer.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }

    public void IncrementProgress()
    {
        ++numberCollected;
        progress.text = numberCollected + "/" + numberOfCollectables;
    }
}