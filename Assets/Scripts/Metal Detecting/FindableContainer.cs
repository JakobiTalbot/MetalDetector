using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// just holds a findable
/// </summary>
public class FindableContainer : MonoBehaviour
{

    public Findables findable;
    [Tooltip("Reveal")]
    [SerializeField]
    float revealLandDistance = 1f;
    [SerializeField]
    float revealSeconds = 2f;

    public IEnumerator Reveal()
    {
        Vector3 landPosition = Random.insideUnitSphere;
        landPosition = landPosition.normalized * revealLandDistance;
        landPosition += transform.position;
        landPosition.y = 10f;
        Ray ray = new Ray(landPosition, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hit);
        landPosition = hit.point;

        float lerp = 0f;
        Vector3 startPos = transform.position;
        while (lerp < 1f)
        {
            lerp += Time.deltaTime / revealSeconds;
            transform.position = Vector3.Slerp(startPos, landPosition, lerp);
            yield return null;
        }
    }
}
