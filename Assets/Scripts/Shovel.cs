using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    [SerializeField]
    GameObject holePrefab;
    [SerializeField]
    Transform shovelHeadTransform;

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
        Physics.Raycast(ray, out RaycastHit hit);

        Instantiate(holePrefab, hit.point, Quaternion.identity).transform.up = hit.normal;
    }
}