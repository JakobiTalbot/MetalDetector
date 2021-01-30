using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepCreator : MonoBehaviour
{

    const float zfightAdjust = 0.001f;

    public GameObject footstepPrefab;
    public int maxFootsteps = 10;

    Queue<GameObject> allFootsteps = new Queue<GameObject>();

    /// <summary>
    /// Creates a footstep at the floor underneath this object
    /// </summary>
    public void CreateFootstep()
    {
        CreateFootstep(transform.position + Vector3.up * 2.0f);
    }

    /// <summary>
    /// Creates a footstep at the floor underneath <paramref name="footPosition"/>
    /// </summary>
    /// <param name="footPosition">Position of the "foot"</param>
    public void CreateFootstep(Vector3 footPosition)
    {
        if(Physics.Raycast(footPosition, Vector3.down, out var hit, float.PositiveInfinity, LayerMask.GetMask("Terrain")))
        {
            var newFootstep = Instantiate(footstepPrefab);
            newFootstep.transform.position = hit.point + (hit.normal * zfightAdjust);
            newFootstep.transform.up = hit.normal;
            allFootsteps.Enqueue(newFootstep);
            if (allFootsteps.Count > maxFootsteps)
            {
                StartCoroutine(DeleteFootstep(allFootsteps.Dequeue()));
            }
        }
    }

    IEnumerator DeleteFootstep(GameObject oldFootstep)
    {
        const float shrinkTime = 1.0f;

        float t = 0.0f;
        Vector3 startScale = oldFootstep.transform.localScale;
        while(t < shrinkTime)
        {
            t += Time.deltaTime;

            oldFootstep.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t / shrinkTime);
            yield return null;
        }

        Destroy(oldFootstep);
    }

}
