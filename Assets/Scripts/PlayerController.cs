using System;
using Abilities;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject tpvCameraPointer;
    [SerializeField] private CapsuleCollider bodyCollider;

    [Inject]
    public void Construct(
        TpvCameraController tpvCameraController,
        FpvCameraController fpvCameraController,
        CameraService cameraService,
        MovementService movementService,
        UnitColliderService unitColliderService,
        AbilityService abilityService,
        IFactory<Type, Ability> abilityFactory)
    {
        tpvCameraController.Init(transform, tpvCameraPointer);
        fpvCameraController.Init(transform);

        cameraService.SetActiveCamera<TpvCameraController>();
        
        unitColliderService.Init(bodyCollider, transform);
        movementService.Init(transform);
        
        abilityService.InitAbility(abilityFactory.Create(typeof(AbilityJump)));
        abilityService.InitAbility(abilityFactory.Create(typeof(AbilityViewChange)));
    }
}
