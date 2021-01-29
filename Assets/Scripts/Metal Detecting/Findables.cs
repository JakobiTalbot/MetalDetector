using UnityEngine;

namespace Metal_Detecting
{
    [CreateAssetMenu(fileName = "Object", menuName = "ScriptableObjects/Findables", order = 1)]
    public class Findables : ScriptableObject
    {
        public string _name;
        public string _description;
        public Transform _prefab;
        public Vector3 _position;
    }
}