﻿using UnityEngine;
using Zenject;

namespace Abilities
{
    public class AbilityJump : Ability
    {
        private MovementService movementService;
        private float jumpHeight;

        [Inject]
        public void Construct(
            UnitColliderService unitColliderService,
            MovementService movementService,
            CameraService cameraService,
            PlayerStats playerStats)
        {
            this.movementService = movementService;
            jumpHeight = playerStats.JumpHeight;
            
            conditions.AddRange(new []{unitColliderService.IsLanded, cameraService.IsFPV});
            cooldown = 1f;
            UseButton = KeyCode.Space;
        }
        public override void Use()
        {
            base.Use();
            movementService.VerticalMovingVelocity = jumpHeight;
        }
    }
}