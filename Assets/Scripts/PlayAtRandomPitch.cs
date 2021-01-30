using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAtRandomPitch : MonoBehaviour
{

    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    private void Awake()
    {
        var source = GetComponent<AudioSource>();

        source.pitch = Random.Range(minPitch, maxPitch);
        source.Play();
    }

}
