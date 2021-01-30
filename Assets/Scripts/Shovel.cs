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
    bool isDigging = false;

    AudioSource digAudio;

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
        if (Input.GetMouseButtonDown(0) && !isDigging)
        {
            animator.SetTrigger("dig");
            isDigging = true;
            player.SetFrozen(true);
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
                findable.Reveal();
                return;
            }
        }
    }

    void SetIsDiggingFalse()
    {
        isDigging = false;
        //player.SetFrozen(false);
    }
}