using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class MetalDetector : MonoBehaviour
{

    [Header("Hum")]
    [MinMaxSlider(0.0f, 100.0f)]
    public Vector2 detectionRange;
    public AnimationCurve volumeCurve;
    public AnimationCurve pitchCurve;
    public float maxParticleEmission = 50f;
    public AnimationCurve particleEmissionCurve;
    public float maxParticleSpeed = 5f;
    public AnimationCurve particleSpeedCurve;

    [Header("WeeWoo")]
    public float weeWooSpeed = 1.0f;
    public float weeWooAmplitude = 1.0f;

    // this is the good number
    public float sampleProg = 50.29f;

    List<FindableContainer> findablesInRange = new List<FindableContainer>();
    AudioSource humSource;
    [SerializeField]
    ParticleSystem particles;
    [SerializeField]
    GameObject gigaParticles;

    void Start()
    {
        humSource = GetComponent<AudioSource>();
        //MakeNewSound();
    }

    void OnEnable()
    {
        // play animation
        findablesInRange.Clear();
        SetParticleValues(0.0f, 0.0f);
        gigaParticles.SetActive(false);
    }

    void MakeNewSound()
    {
        const int sampleCount = 44100 * 60;
        AudioClip newSineWave = AudioClip.Create("my_hum", sampleCount, 1, 44100, false);

        float[] sineData = new float[sampleCount];

        for(int i = 0; i < sampleCount; ++i)
        {
            float prog = (i / (float)sampleCount) * sampleProg;

            sineData[i] = Mathf.Sin(prog);

            if (i == sampleCount - 1)
                sineData[i] = 0;
        }

        newSineWave.SetData(sineData, 0);
        humSource.clip = newSineWave;
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

            float emissionRate = particleEmissionCurve.Evaluate(percentage) * maxParticleEmission;
            float speed = particleSpeedCurve.Evaluate(percentage) * maxParticleSpeed;
            SetParticleValues(emissionRate, speed);

            if (distance <= detectionRange.x)
            {
                pitch += Mathf.Sin(Time.time * weeWooSpeed) * weeWooAmplitude;
                gigaParticles.SetActive(true);
            }
            else if (gigaParticles.activeSelf)
            {
                gigaParticles.SetActive(false);
            }

            humSource.volume = volume;
            humSource.pitch = pitch;
        }
        else if(humSource.isPlaying)
        {
            humSource.Pause();
            SetParticleValues(0.0f, 0.0f);
        }
    }

    void SetParticleValues(float emissionRate, float startSpeed)
    {
        var emission = particles.emission;
        emission.rateOverTime = emissionRate;
        var main = particles.main;
        main.startSpeed = startSpeed;
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
                if(findablesInRange[i] == null)
                {
                    findablesInRange.RemoveAt(i);
                    i -= 1;
                    continue;
                }

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