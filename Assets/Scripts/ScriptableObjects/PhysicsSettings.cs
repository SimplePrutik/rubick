using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PhysicsSettings", menuName = "ScriptableObjects/PhysicsSettings", order = 0)]
    public class PhysicsSettings : ScriptableObject
    {
        public float GravityStrength;
    }
}