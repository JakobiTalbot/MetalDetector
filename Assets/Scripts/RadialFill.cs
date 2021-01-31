using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class RadialFill : MonoBehaviour
{
    [SerializeField]
    Image radialFillCircle;
    [SerializeField]
    float timeToFill = 2f;
    [SerializeField]
    KeyCode buttonToFill;
    [SerializeField]
    UnityEvent whenFilled;
    [SerializeField]
    float timeToWaitBeforeFadingOut = 1f;
    [SerializeField]
    float timeToFadeOut = 1f;

    CanvasGroup canvasGroup;
    bool coroutineRunning = false;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Input.GetKey(buttonToFill))
        {
            if (coroutineRunning)
            {
                StopAllCoroutines();
            }
            canvasGroup.alpha = 1f;

            radialFillCircle.fillAmount += Time.deltaTime / timeToFill;
            if (radialFillCircle.fillAmount >= 1f)
            {
                whenFilled.Invoke();
            }
        }
        else if (radialFillCircle.fillAmount > 0f)
        {
            radialFillCircle.fillAmount -= Time.deltaTime / timeToFill;
        }
        else if (!coroutineRunning && canvasGroup.alpha > 0f)
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        coroutineRunning = true;

        yield return new WaitForSeconds(timeToWaitBeforeFadingOut);

        float lerp = 1f;
        while (lerp > 0f)
        {
            lerp -= Time.deltaTime / timeToFadeOut;
            canvasGroup.alpha = lerp;

            yield return null;
        }

        coroutineRunning = false;
    }
}