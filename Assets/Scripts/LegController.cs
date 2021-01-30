using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LegController : MonoBehaviour
{

    public int linePoints = 10;
    public Transform footTransform;
    public float stepHeight = 2.0f;
    public float stepSpeed = 1.0f;
    public bool IsMoving { get; private set; } = false;

    Vector3[] midOffsets = new Vector3[2];
    LineRenderer legRenderer;
    FootstepCreator footstepCreator;
    PlayerController attachedPlayer;
    Vector3 footPos;

    private void Awake()
    {
        legRenderer = GetComponent<LineRenderer>();
        legRenderer.positionCount = linePoints;
        legRenderer.useWorldSpace = true;
        footstepCreator = GetComponentInChildren<FootstepCreator>();
        attachedPlayer = GetComponentInParent<PlayerController>();

        footPos = footTransform.position;
    }

    private void Update()
    {
        if (!IsMoving)
            footTransform.position = footPos;

        Vector3 startPos = transform.position;
        Vector3 endPos = footTransform.position;
        footPos = endPos;

        Vector3 dif = startPos - endPos;
        dif.y = 0.0f;
        float dot = -Vector3.Dot(dif, attachedPlayer.transform.forward);
        midOffsets[0] = Vector3.up * Mathf.Pow(dif.sqrMagnitude, 0.01f) * dot * 0.2f;
        midOffsets[1] = Vector3.up * Mathf.Pow(dif.sqrMagnitude, 0.01f) * dot * 0.2f;

        Vector3 mid1 = (startPos + (endPos - startPos) * 0.25f) + midOffsets[0];
        Vector3 mid2 = (startPos + (endPos - startPos) * 0.75f) + midOffsets[1];

        for (int i = 0; i < linePoints; ++i)
        {
            float perc = (float)i / (linePoints - 1);

            Vector3 linePos = Util.Bezier(perc, endPos, mid2, mid1, startPos);
            legRenderer.SetPosition(i, linePos);
        }
    }

    private void LateUpdate()
    {
    }

    public void MoveTo(Vector3 pos)
    {
        StartCoroutine(DoStep(pos));
    }

    IEnumerator DoStep(Vector3 endPos)
    {
        IsMoving = true;
        Vector3 startPos = footTransform.position;
        Vector3 dif = endPos - startPos;
        Vector3 mid1 = (startPos + (endPos - startPos) * 0.25f) + Vector3.up * stepHeight;
        Vector3 mid2 = (startPos + (endPos - startPos) * 0.25f) + Vector3.up * stepHeight;

        float stepTime = stepSpeed;

        footstepCreator.CreateFootstep();

        float t = 0.0f;
        while (t <= stepTime)
        {
            t += Time.deltaTime;

            Vector3 newPos = Util.Bezier(t/stepTime, endPos, mid2, mid1, startPos);
            footTransform.position = newPos;

            yield return null;
        }

        footTransform.position = endPos;
        IsMoving = false;
    }

}
