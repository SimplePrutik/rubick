using System;
using Abilities;
using Fight.Projectiles;
using Pool;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject tpvCameraPointer;
    [SerializeField] private CapsuleCollider bodyCollider;
    [SerializeField] private Arrow arrowPrefab;

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
        var threeArrow = abilityFactory.Create(typeof(AbilityThreeArrow));
        abilityService.InitAbility(threeArrow);
        
        var pool = new Pool<Arrow>(arrowPrefab, 30, new GameObject().transform);
        threeArrow.Prepare(pool);
    }
}
