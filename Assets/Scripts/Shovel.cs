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
        Physics.Raycast(ray, out RaycastHit hit, LayerMask.GetMask("Terrain"));
        Transform hole = Instantiate(holePrefab, hit.point, Quaternion.identity).transform;
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