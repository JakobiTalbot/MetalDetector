using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDetector : MonoBehaviour
{
    List<FindableContainer> findablesInRange;

    void Start()
    {
        findablesInRange = new List<FindableContainer>();
    }

    void Update()
    {
        if (findablesInRange.Count > 1)
        {
            int closestIndex = 0;
            Vector3 pos = transform.position;
            float lowestDistance = Vector3.Distance(pos, findablesInRange[0].transform.position);
            for (int i = 1; i < findablesInRange.Count; ++i)
            {
                float dist;
                if ((dist = Vector3.Distance(pos, findablesInRange[closestIndex].transform.position)) < lowestDistance)
                {
                    closestIndex = i;
                    lowestDistance = dist;
                }
            }
        }

        // TODO: feedback on distance to findable
    }

    void OnTriggerEnter(Collider other)
    {
        FindableContainer findable;
        if (findable = other.GetComponent<FindableContainer>())
        {
            findablesInRange.Add(findable);
        }
    }

    void OnTriggerExit(Collider other)
    {
        FindableContainer findable;
        if (findable = other.GetComponent<FindableContainer>())
        {
            findablesInRange.Remove(findable);
        }
    }
}