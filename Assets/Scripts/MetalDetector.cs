using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class MetalDetector : MonoBehaviour
{

    [MinMaxSlider(0.0f, 100.0f)]
    public Vector2 detectionRange;
    public AnimationCurve volumeCurve;
    public AnimationCurve pitchCurve;

    List<FindableContainer> findablesInRange;
    AudioSource humSource;

    void Start()
    {
        findablesInRange = new List<FindableContainer>();
        humSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GetClosestFindable(out var distance);

        if(distance <= detectionRange.y)
        {
            if(!humSource.isPlaying)
            {
                humSource.Play();
            }

            float percentage = 1.0f - ((distance - detectionRange.x) / (detectionRange.y - detectionRange.x));

            float volume = volumeCurve.Evaluate(percentage);
            float pitch = pitchCurve.Evaluate(percentage);

            humSource.volume = volume;
            humSource.pitch = pitch;
        }
        else if(humSource.isPlaying)
        {
            humSource.Pause();
        }
    }

    FindableContainer GetClosestFindable(out float closestDistance)
    {
        FindableContainer result = null;
        closestDistance = float.PositiveInfinity;

        if(findablesInRange.Count > 0)
        {
            Vector3 pos = transform.position;

            for (int i = 0; i < findablesInRange.Count; ++i)
            {
                float dist;
                if ((dist = Vector3.Distance(pos, findablesInRange[i].transform.position)) < closestDistance)
                {
                    closestDistance = dist;
                    result = findablesInRange[i];
                }
            }
        }

        return result;
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