using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    [SerializeField]
    GameObject holePrefab;
    [SerializeField]
    Transform shovelHeadTransform;
    [SerializeField]
    float digRadius = 3f;

    AudioSource digAudio;

    private void Start()
    {
        digAudio = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        // play animation
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Dig();
        }
    }

    void Dig()
    {
        Ray ray = new Ray(shovelHeadTransform.position, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Terrain"));
        Transform hole = Instantiate(holePrefab, hit.point, Quaternion.identity).transform;
        digAudio.Play();
        hole.up = hit.normal;
        Collider[] colliders = Physics.OverlapSphere(hole.position, digRadius);
        foreach (Collider collider in colliders)
        {
            FindableContainer findable;
            if (findable = collider.GetComponent<FindableContainer>())
            {
                StartCoroutine(findable.Reveal());
                return;
            }
        }
    }
}