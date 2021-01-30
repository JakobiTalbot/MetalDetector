using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegManager : MonoBehaviour
{

    public Vector3 LegOffset { get; set; }

    public float moveDistance = 0.1f;
    public float stepRange = 5.0f;

    LegController[] allLegs;
    int lastMovedLeg = 0;

    private void Awake()
    {
        allLegs = GetComponentsInChildren<LegController>();
    }

    private void Update()
    {
        if(!allLegs[lastMovedLeg].IsMoving)
        {
            int newLeg = (lastMovedLeg + 1) % allLegs.Length;

            Vector3 rayOrigin = allLegs[newLeg].transform.position + (LegOffset*stepRange) + (Vector3.up*10.0f);
            if(Physics.Raycast(rayOrigin, Vector3.down, out var hit, float.PositiveInfinity, LayerMask.GetMask("Terrain")))
            {
                if(Vector3.Distance(allLegs[newLeg].footTransform.position, hit.point) >= moveDistance)
                {
                    allLegs[newLeg].MoveTo(hit.point);
                    lastMovedLeg = newLeg;
                }
            }
        }
    }

}
