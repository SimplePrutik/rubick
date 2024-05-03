using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UI;
using Zenject;

public class CameraManager
{
    private ScreensService screensService;
    
    private readonly List<CameraController> controllers = new List<CameraController>();

    [Inject]
    public void Construct(ScreensService screensService)
    {
        this.screensService = screensService;
    }

    public void AddCamera(CameraController controller)
    {
        controllers.Add(controller);
    }
    
    public void RemoveCamera(CameraController controller)
    {
        controllers.Remove(controller);
    }

    public void SetActiveCamera<T>(bool areOthersDisabled = true) where T : CameraController
    {
        if (areOthersDisabled)
            foreach (var cameraController in controllers)
            {
                cameraController.SetEnable(false);
                cameraController.IsLocked = true;
            }
        var currentController = GetCameraController<T>();
        currentController.SetEnable(true);
        currentController.IsLocked = false;
    }

    public void RunPlayerCameraTransition()
    {
        var tpvCamera = GetCameraController<TpvCameraController>();
        var fpvCamera = GetCameraController<FpvCameraController>();
        if (tpvCamera == null || fpvCamera == null)
            return;
        if (tpvCamera.IsEnabled)
            screensService.Fade(0.5f, () => SetActiveCamera<FpvCameraController>());
        if (fpvCamera.IsEnabled)
            screensService.Fade(0.5f, () => SetActiveCamera<TpvCameraController>());
    }

    private CameraController GetCameraController<T>() where T : CameraController
    {
        return controllers.First(cameraController => cameraController is T);
    }
    
}