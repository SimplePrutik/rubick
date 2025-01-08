using Zenject;

namespace Abilities
{
    public class AbilityJetPack : Ability
    {
        private MovementService movementService;

        [Inject]
        public void Construct(
            UnitColliderService unitColliderService,
            MovementService movementService)
        {
            this.movementService = movementService;

            UseButton = ButtonSettings.Jump;
        }
        public override void Use()
        {
            base.Use();
            movementService.SetVerticalAcceleration(40f);
        }
    }
}