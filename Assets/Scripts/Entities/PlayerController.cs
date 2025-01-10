using Abilities;
using Fight.Projectiles;
using Pooling;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject tpvCameraPointer;
        [SerializeField] private CapsuleCollider bodyCollider;

        [Inject]
        public void Construct(
            TpvCameraController tpvCameraController,
            FpvCameraController fpvCameraController,
            CameraService cameraService,
            AbilityService abilityService,
            AbilityFactory abilityFactory,
            PhysicsSettings physicsSettings,
            UnitColliderService unitColliderService,
            PlayerStats playerStats,
            DiContainer container)
        {
            tpvCameraController.Init(transform, tpvCameraPointer);
            fpvCameraController.Init(transform);

            cameraService.SetActiveCamera<FpvCameraController>();
            
            var playerMovementController = new PlayerMovementController(tpvCameraController, 
                fpvCameraController, physicsSettings, playerStats, unitColliderService, bodyCollider, transform);

            var jetPack = abilityFactory.Create(typeof(AbilityJetPack));
            ((AbilityJetPack) jetPack).Init(playerMovementController);
            abilityService.InitAbility(jetPack);
            abilityService.InitAbility(abilityFactory.Create(typeof(AbilityViewChange)));
            var threeArrow = abilityFactory.Create(typeof(AbilityThreeArrow));
            abilityService.InitAbility(threeArrow);
        
            var pool = new Pool<Arrow>(30, new GameObject().transform, $"Prefabs/3D/Projectiles/Arrow", container);
            threeArrow.Prepare(pool);
        }
    }
}
