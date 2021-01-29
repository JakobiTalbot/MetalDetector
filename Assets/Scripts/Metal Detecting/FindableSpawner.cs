using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class FindableSpawner : MonoBehaviour
{
     [SerializeField] int numberOfObjects;
     [SerializeField] Transform _prefab;
     [SerializeField] List<Vector3> _objectPoints;
     
     private LayerMask layer;

     private void Start()
     {
          layer = LayerMask.GetMask("Terrain");
          RandomVector2();
     }

     void RandomVector2()
     {
          for (int i = 0; i < numberOfObjects; i++) 
          {
               int x = Random.Range(0, 200);
               int z = Random.Range(0, 200);

               var pointPos = new Vector3(x, transform.position.y, z);
               RaycastHit hit;
               if (Physics.Raycast(pointPos, Vector3.down, out hit, Mathf.Infinity, layer))
               {
                    pointPos.y -= hit.distance;
                   _objectPoints.Add(pointPos);
                   Instantiate(_prefab, pointPos, Quaternion.identity , transform); 
               }
          }
     }
}
