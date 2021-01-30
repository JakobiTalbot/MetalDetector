using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "ScriptableObjects/Findables", order = 1)]
public class Findables : ScriptableObject
{
    public string objectName;
    public string description;
    public Transform prefab;
    public Vector3 showcaseCameraOffset;
}