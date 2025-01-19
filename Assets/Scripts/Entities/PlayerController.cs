using System;
using System.Collections.Generic;
using Abilities;
using Extentions;
using Movement;
using ScriptableObjects;
using UniRx;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class PlayerController : MonoBehaviour, IEntity
    {
        private UnitMovementController unitMovementController;
        private GroundGravityController groundGravityController;
        
        [SerializeField] private GameObject tpvCameraPointer;
        [SerializeField] private CapsuleCollider bodyCollider;

        private List<Ability> innateAbilites = new List<Ability>();

        [Inject]
        public void Construct(
            TpvCameraController tpvCameraController,
            FpvCameraController fpvCameraController,
            CameraService cameraService,
            AbilityFactory abilityFactory,
            PlayerStats playerStats, 
            PhysicsSettings physicsSettings, 
            PlayerMovementService playerMovementService,
            UnitColliderService unitColliderService,
            AbilityService abilityService,
            DiContainer container)
        {
            tpvCameraController.Init(transform, tpvCameraPointer);
            fpvCameraController.Init(transform);

            cameraService.SetActiveCamera<FpvCameraController>();

            unitMovementController = new UnitMovementController(unitColliderService);
            groundGravityController = new GroundGravityController(physicsSettings, unitColliderService, transform, bodyCollider);

            PrepareInnateAbilities(abilityFactory, abilityService);
            PrepareMovementController(groundGravityController, playerMovementService);
        }

        private void PrepareInnateAbilities(AbilityFactory abilityFactory, AbilityService abilityService)
        {
            var jetPack = abilityFactory.GetAbility<AbilityJetPack>();
            ((AbilityJetPack)jetPack).Prepare(unitMovementController, groundGravityController);
            innateAbilites.Add(jetPack);
            abilityService.EnableAbility(jetPack);
            // innateAbilites.Add(abilityFactory.GetAbility<AbilityViewChange>());
        }

        private void PrepareMovementController(GroundGravityController groundGravityController, PlayerMovementService playerMovementService)
        {
            Observable
                .EveryUpdate()
                .ObserveOnMainThread()
                .Subscribe(_ =>
                {
                    unitMovementController.AddMovementForce(playerMovementService.GetForce(transform));
                    unitMovementController.AddMovementForce(groundGravityController.GetForce());
                    transform.position += unitMovementController.DeltaMovement(bodyCollider, transform.position);
                    transform.localRotation = playerMovementService.GetRotation(transform);
                })
                .AddTo(this);
        }

        public int GetId()
        {
            throw new System.NotImplementedException();
        }
    }
}
