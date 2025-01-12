using UnityEngine;

namespace Extentions
{
    public static class Extensions
    {
        public static Vector2 ConvertToHorizontalMovement(this Vector3 volumetricMovement)
        {
            return new Vector2(volumetricMovement.x, volumetricMovement.z);
        }

        public static Vector3 ConvertToVolumetricMovement(Vector2 horizontalMovement, float verticalMovement)
        {
            return new Vector3(horizontalMovement.x, verticalMovement, horizontalMovement.y);
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

        public static Vector2 GetPositionOnScreen(this Camera camera, Vector3 worldPosition, RectTransform screen)
        {
            var relativePos = camera.WorldToViewportPoint(worldPosition);
            return new Vector2(relativePos.x * screen.sizeDelta.x, relativePos.y * screen.sizeDelta.y);
        }
    }
}