using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// just holds a findable
/// </summary>
public class FindableContainer : MonoBehaviour
{
    public Findables findable;

    public void Reveal()
    {
        StartCoroutine(DoReveal());

    }

    IEnumerator DoReveal()
    {
        const float revealSeconds = 1.0f;
        Camera cam = Camera.main;

        Vector3 destPos = cam.transform.position + cam.transform.forward * 3.0f;
        Vector3 b1 = transform.position;
        Vector3 b2 = transform.position + Vector3.up * 10.0f;
        Vector3 b3 = destPos + cam.transform.forward * 10.0f;
        Vector3 b4 = destPos;

        float lerp = 0f;
        while (lerp <= revealSeconds)
        {
            lerp += Time.deltaTime;

            float t = EaseIn(lerp / revealSeconds);
            transform.position = Util.Bezier(t, b4, b3, b2, b1);
            yield return null;
        }

        transform.position = destPos;
        UIManager.instance.DisplayContinueUI(findable, this);
    }

    float EaseIn(float t)
    {
        return 1.0f - Mathf.Cos((t * Mathf.PI) / 2.0f);
    }
}
