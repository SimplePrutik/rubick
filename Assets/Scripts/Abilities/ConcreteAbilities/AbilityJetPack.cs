using Movement;
using UnityEngine;
using Zenject;

namespace Abilities
{
    public class AbilityJetPack : Ability
    {
        private UnitMovementController unitMovementController;
        private GroundGravityController groundGravityController;

        [Inject]
        public void Construct()
        {
            UseButton = ButtonSettings.Jump;
            abilityType = AbilityType.Hold;
        }
        
        public override void Use()
        {
            base.Use();
            groundGravityController.ExteriorForce = -0.15f;
        }

        public void Prepare(UnitMovementController unitMovementController, GroundGravityController groundGravityController)
        {
            this.unitMovementController = unitMovementController;
            this.groundGravityController = groundGravityController;
        }
    }
}