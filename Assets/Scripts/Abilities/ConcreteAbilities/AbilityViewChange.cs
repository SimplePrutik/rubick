using Zenject;

namespace Abilities
{
    public class AbilityViewChange : Ability
    {
        private CameraService cameraService;
        private CooldownTimer cooldownTimer;

        [Inject]
        public void Construct(CameraService cameraService)
        {
            this.cameraService = cameraService;
            
            cooldownTimer = new CooldownTimer(2f);
            
            conditions.Add(cooldownTimer.IsOffCooldown);
            UseButton = ButtonSettings.ChangeView;
        }
        public override void Use()
        {
            base.Use();
            cooldownTimer.Start();
            cameraService.RunPlayerCameraTransition();
        }
    }
}