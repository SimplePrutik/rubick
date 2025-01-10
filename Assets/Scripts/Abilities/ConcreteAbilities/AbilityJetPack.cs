using Zenject;

namespace Abilities
{
    public class AbilityJetPack : Ability
    {
        private PlayerMovementController playerMovementController;

        [Inject]
        public void Construct()
        {
            UseButton = ButtonSettings.Jump;
        }

        public void Init(PlayerMovementController playerMovementController)
        {
            this.playerMovementController = playerMovementController;
        }
        
        public override void Use()
        {
            base.Use();
            playerMovementController.AddVerticalAcceleration(40f);
        }
    }
}