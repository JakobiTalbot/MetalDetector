using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [SerializeField]
    TextMeshPro startText;
    [SerializeField]
    float startTextFadeTime = 0.5f;
    [SerializeField]
    float startTextStayTime = 4f;
    [SerializeField]
    GameObject stars;
    [SerializeField]
    TextMeshPro titleText;
    [SerializeField]
    float titleTextWaitTime = 3f;
    [SerializeField]
    float titleTextMoveSpeed = -20f;
    [SerializeField]
    float titleTextAccelerationDistance = 20f;
    [SerializeField]
    float titleTextAccelerationRate = 0.05f;
    [SerializeField]
    float titleEndDistance = 60f;
    [SerializeField]
    float titleTextFadeTime = 0.5f;
    [SerializeField]
    AudioSource introMusic;
    [SerializeField]
    Transform crawlText;
    [SerializeField]
    float crawlSpeed = 5f;
    [SerializeField]
    MainMenuManager menuManager;

    void Start()
    {
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        Color currentColour = startText.color;

        // fade in start text
        while (currentColour.a < 1f)
        {
            currentColour.a += Time.deltaTime / startTextFadeTime;
            startText.color = currentColour;

            yield return null;
        }

        yield return new WaitForSeconds(startTextStayTime);

        // fade out start text
        while (currentColour.a > 0f)
        {
            currentColour.a -= Time.deltaTime / startTextFadeTime;
            startText.color = currentColour;

            yield return null;
        }

        startText.gameObject.SetActive(false);

        yield return new WaitForSeconds(titleTextWaitTime);

        titleText.gameObject.SetActive(true);
        stars.SetActive(true);

        introMusic.Play();
        Invoke("LoadLevel", introMusic.clip.length + 2f);
        StartCoroutine(PushBackTitle());
    }

    IEnumerator PushBackTitle()
    {
        bool crawlStarted = false;
        while (titleText.transform.position.z < titleEndDistance)
        {
            if (titleText.transform.position.z > titleTextAccelerationDistance)
            {
                titleTextMoveSpeed *= titleTextAccelerationRate;
                if (!crawlStarted)
                {
                    StartCoroutine(Crawl());
                    crawlStarted = true;
                }
            }

            titleText.transform.position += Vector3.back * titleTextMoveSpeed * Time.deltaTime;

            yield return null;
        }

        Color currentColour = titleText.color;

        // fade out title text (still push back)
        while (currentColour.a > 0f)
        {
            currentColour.a -= Time.deltaTime / startTextFadeTime;
            titleText.color = currentColour;

            titleTextMoveSpeed *= titleTextAccelerationRate;
            titleText.transform.position += Vector3.back * titleTextMoveSpeed * Time.deltaTime;

            yield return null;
        }

        titleText.gameObject.SetActive(false);
    }

    IEnumerator Crawl()
    {
        crawlText.gameObject.SetActive(true);
        while (true)
        {
            crawlText.position += crawlText.up * crawlSpeed * Time.deltaTime;

            yield return null;
        }
    }

    void LoadLevel()
    {
        menuManager.LoadScene(1);
    }
}
