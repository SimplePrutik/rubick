using Abilities;
using Fight.Projectiles;
using Pooling;
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
        AbilityFactory abilityFactory)
    {
        tpvCameraController.Init(transform, tpvCameraPointer);
        fpvCameraController.Init(transform);

        cameraService.SetActiveCamera<FpvCameraController>();
        
        unitColliderService.InitGroundCheck(bodyCollider, transform);
        movementService.Init(bodyCollider, transform);
        
        abilityService.InitAbility(abilityFactory.Create(typeof(AbilityJump)));
        abilityService.InitAbility(abilityFactory.Create(typeof(AbilityViewChange)));
        var threeArrow = abilityFactory.Create(typeof(AbilityThreeArrow));
        abilityService.InitAbility(threeArrow);
        
        var pool = new Pool<Arrow>(30, new GameObject().transform, $"Prefabs/3D/Projectiles/Arrow");
        threeArrow.Prepare(pool);
    }
}
