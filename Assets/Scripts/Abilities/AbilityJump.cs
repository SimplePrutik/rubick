using Extentions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Abilities
{
    public class AbilityJump : Ability
    {
        private MovementService movementService;
        private float jumpHeight;

        public AbilityJump(
            ReactiveProperty<bool> isLanded,
            MovementService movementService,
            float jumpHeight)
        {
            this.movementService = movementService;
            this.jumpHeight = jumpHeight;
            
            conditions.Add(isLanded);
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