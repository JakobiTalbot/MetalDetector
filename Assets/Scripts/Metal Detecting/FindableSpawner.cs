using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class FindableSpawner : MonoBehaviour
{
    [SerializeField] List<Findables> objectsToSpawn;
    [SerializeField] float objectDepth = 1f;

    private LayerMask layer;
    private BoxCollider boxCollider;

    private void Awake()
    {
        layer = LayerMask.GetMask("Terrain");
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        RandomVector2();
    }

    void RandomVector2()
    {
        foreach(Findables findable in objectsToSpawn)
        {
            var pointPos = RandomRange(boxCollider.bounds.min, boxCollider.bounds.max);
            if (Physics.Raycast(pointPos, Vector3.down, out var hit, Mathf.Infinity, layer))
            {
                pointPos.y -= hit.distance + objectDepth;
                var newFindable = Instantiate(findable.prefab, pointPos, Quaternion.identity, transform);
                
                var newContainer = newFindable.gameObject.AddComponent<FindableContainer>();
                newContainer.findable = findable;
                
            }
        }
    }

    Vector3 RandomRange(Vector3 min, Vector3 max)
    {
        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }
}
