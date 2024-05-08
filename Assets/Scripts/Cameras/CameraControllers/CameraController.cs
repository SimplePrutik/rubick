using ScriptableObjects;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Camera))]
public abstract class CameraController : MonoBehaviour
{
    private CameraService cameraService;
    protected CameraSettings cameraSettings;
    
    public Camera Camera { get; private set; }
    public bool IsLocked { get; set; }
    public bool IsEnabled => gameObject.activeSelf;
    public void SetEnable(bool value) => gameObject.SetActive(value);

    [Inject]
    public void Construct(
        CameraService cameraService,
        CameraSettings cameraSettings)
    {
        this.cameraService = cameraService;
        this.cameraSettings = cameraSettings;

        Camera = GetComponent<Camera>();
        
        cameraService.AddCamera(this);
    }

    private void OnDestroy()
    {
        cameraService.RemoveCamera(this);
    }
}