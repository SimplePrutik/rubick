using UniRx;
using UnityEngine;

public class FpvCameraController : CameraController
{
    public void Init(
        Transform parent,
        Vector3 position,
        Vector3 rotation)
    {
        transform.SetParent(parent);
        transform.localPosition = position;
        transform.localRotation = Quaternion.Euler(rotation);
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