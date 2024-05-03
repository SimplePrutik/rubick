using UnityEngine;
using Zenject;

[RequireComponent(typeof(Camera))]
public abstract class CameraController : MonoBehaviour
{
    private CameraManager cameraManager;
    
    protected Camera Camera;
    public bool IsLocked { get; set; }
    public bool IsEnabled => gameObject.activeInHierarchy;
    public void SetEnable(bool value) => gameObject.SetActive(value);

    [Inject]
    public void Construct(CameraManager cameraManager)
    {
        this.cameraManager = cameraManager;

        Camera = GetComponent<Camera>();
        
        cameraManager.AddCamera(this);
    }

    private void OnDestroy()
    {
        cameraManager.RemoveCamera(this);
    }
}