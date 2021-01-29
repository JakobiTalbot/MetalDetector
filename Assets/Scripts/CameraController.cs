using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CameraController : MonoBehaviour
{

    public Transform followObject;
    public Vector3 followOffset;
    public float followSpeed = 10.0f;

    void Update()
    {
        Vector3 difference = transform.position - (followObject.position + followOffset);
        transform.position -= difference * followSpeed * Time.deltaTime;
    }

    [Button("Set Follow Offset")]
    void SetFollowOffset()
    {
        followOffset = transform.position - followObject.position;
    }
}
