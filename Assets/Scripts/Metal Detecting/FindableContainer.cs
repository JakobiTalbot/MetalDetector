using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// just holds a findable
/// </summary>
public class FindableContainer : MonoBehaviour
{
    public Findables findable;

    [Button("Reveal")]
    public void Reveal()
    {
        StartCoroutine(DoReveal());

    }

    IEnumerator DoReveal()
    {
        const float revealSeconds = 2.0f;
        Camera cam = Camera.main;

        Vector3 b1 = transform.position;
        Vector3 b2 = transform.position + Vector3.up * 10.0f;
        Vector3 destPos = Vector3.zero;

        float lerp = 0f;
        while (lerp <= revealSeconds)
        {
            destPos = cam.transform.position + cam.transform.forward * 3.0f + (cam.transform.rotation * findable.showcaseCameraOffset);
            Vector3 b3 = destPos + cam.transform.forward * 10.0f;
            Vector3 b4 = destPos;

            lerp += Time.deltaTime;

            float t = EaseInOut(lerp / revealSeconds);
            transform.position = Util.Bezier(t, b4, b3, b2, b1);
            yield return null;
        }

        transform.position = destPos;
        UIManager.instance.DisplayContinueUI(findable, this);
    }

    float EaseInOut(float t)
    {
        return 1.0f - (1.0f - t) * (1.0f - t);
    }
}
