using UnityEngine;

namespace Extentions
{
    public static class Extensions
    {
        public static Vector2 ConvertToHorizontalMovement(this Vector3 volumetricMovement)
        {
            return new Vector2(volumetricMovement.x, volumetricMovement.z);
        }

        public static Vector3 ConvertToVolumetricMovement(Vector2 horizontalMovement, float verticalMoevement)
        {
            return new Vector3(horizontalMovement.x, verticalMoevement, horizontalMovement.y);
        }

        public static Vector3 ChangeX(this Vector3 value, float x)
        {
            return new Vector3(x, value.y, value.z);
        }

        public static Vector3 ChangeY(this Vector3 value, float y)
        {
            return new Vector3(value.x, y, value.z);
        }

        public static Vector3 ChangeZ(this Vector3 value, float z)
        {
            return new Vector3(value.x, value.y, z);
        }
    }
}