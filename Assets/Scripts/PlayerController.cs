using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    //take it somewhere else in some global game controller
    [SerializeField] private PhysicsSettings physicsSettings;
    [SerializeField] private GameObject tpvCameraPointer;
    [SerializeField] private CameraSettings cameraSettings;
    [SerializeField] private Collider groundCollider;

    private CameraManager cameraManager;

    private List<Ability> abilites;

    [Inject]
    public void Construct(
        TpvCameraController tpvCameraController,
        FpvCameraController fpvCameraController,
        CameraManager cameraManager,
        MovementService movementService,
        UnitColliderService unitColliderService)
    {
        this.cameraManager = cameraManager;
        
        tpvCameraController.Init(transform, tpvCameraPointer, cameraSettings.TPVCameraPosition, cameraSettings.TPVCameraRotation);
        fpvCameraController.Init(transform, cameraSettings.FPVCameraPosition, cameraSettings.FPVCameraRotation);

        cameraManager.SetActiveCamera<TpvCameraController>();
        
        unitColliderService.Init(groundCollider);
        movementService.Init(physicsSettings, playerStats, transform, unitColliderService);
    }
    
    private void ChangeCamera()
    {
        if (Input.GetKey(KeyCode.F))
        {
            cameraManager.RunPlayerCameraTransition();
        }  
    }
}
