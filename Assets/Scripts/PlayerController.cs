using System.Collections.Generic;
using Abilities;
using ScriptableObjects;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject tpvCameraPointer;
    [SerializeField] private CapsuleCollider bodyCollider;

    private List<Ability> abilites;

    [Inject]
    public void Construct(
        TpvCameraController tpvCameraController,
        FpvCameraController fpvCameraController,
        CameraService cameraService,
        MovementService movementService,
        UnitColliderService unitColliderService,
        AbilityService abilityService,
        AbilityJump abilityJump,
        AbilityViewChange abilityViewChange)
    {
        tpvCameraController.Init(transform, tpvCameraPointer);
        fpvCameraController.Init(transform);

        cameraService.SetActiveCamera<TpvCameraController>();
        
        unitColliderService.Init(bodyCollider, transform);
        movementService.Init(transform);
        
        abilityService.InitAbility(abilityJump);
        abilityService.InitAbility(abilityViewChange);
    }
}
