using UniRx;
using UnityEngine;

public class FpvCameraController : CameraController
{
    public void Init(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = cameraSettings.FPVCameraPosition;
        transform.localRotation = Quaternion.Euler(cameraSettings.FPVCameraRotation);
        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                if (!IsLocked)
                    Rotate();
            })
            .AddTo(this);
    }

    private void Rotate()
    {
        var currentRotation = transform.localRotation.eulerAngles.x;
        transform.localRotation = Quaternion.Euler(currentRotation - Input.GetAxis("Mouse Y"), 0, 0);
    }
    
    private void Update()
    {
        Rotate();
    }
}