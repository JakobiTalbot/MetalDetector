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
    [SerializeField]
    PlayerController player;

    Animator animator;

    AudioSource digAudio;
    bool foundFindable = false;

    private void Start()
    {
        digAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // play animation??
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !player.Frozen) 
        {
            animator.SetTrigger("dig");
            player.Frozen = true;
        }
    }

    void CreateHole()
    {
        digAudio.Play();

        Ray ray = new Ray(shovelHeadTransform.position + Vector3.up * 5, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Terrain"));
        Transform hole = Instantiate(holePrefab, hit.point, Quaternion.identity).transform;
        hole.up = hit.normal;

        Collider[] colliders = Physics.OverlapSphere(hole.position, digRadius);
        foreach (Collider collider in colliders)
        {
            FindableContainer findable;
            if (findable = collider.GetComponent<FindableContainer>())
            {
                findable.transform.position = hole.position;
                findable.Reveal();
                foundFindable = true;
                return;
            }
        }
    }

    void SetIsDiggingFalse()
    {
        if (!foundFindable)
            player.Frozen = false;
    }
}