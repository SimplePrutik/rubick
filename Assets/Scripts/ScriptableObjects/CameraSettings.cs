using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "ScriptableObjects/CameraSettings", order = 0)]
    public class CameraSettings : ScriptableObject
    {
        public Vector3 TPVCameraPosition;
        public Vector3 FPVCameraPosition;
        public Vector3 TPVCameraRotation;
        public Vector3 FPVCameraRotation;
    }
}