using ScriptableObjects;
using UniRx;
using UnityEngine;
using Zenject;

public class TpvCameraController : CameraController
{

    private const float CAMERA_VERTICAL_MOVEMENT_SPEED = 0.1f;
    private const float CAMERA_VERTICAL_MOVEMENT_UPPER_CAP = 5f;
    private const float CAMERA_VERTICAL_MOVEMENT_BOTTOM_CAP = 0.5f;
    public void Init(
        Transform parent,
        GameObject pointer)
    {
        transform.SetParent(parent);
        transform.localPosition = cameraSettings.TPVCameraPosition;
        transform.localRotation = Quaternion.Euler(cameraSettings.TPVCameraRotation);
        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                if (!IsLocked)
                {
                    Rotate();
                    Camera.transform.LookAt(pointer.transform.position);
                }
            })
            .AddTo(this);
    }
    
    private void Rotate()
    {
        var localPos = transform.localPosition;
        var newY = Mathf.Clamp(
            -Input.GetAxis("Mouse Y") * CAMERA_VERTICAL_MOVEMENT_SPEED + localPos.y,
            CAMERA_VERTICAL_MOVEMENT_BOTTOM_CAP,
            CAMERA_VERTICAL_MOVEMENT_UPPER_CAP);
        transform.localPosition = new Vector3(localPos.x, newY, localPos.z);
    }
}