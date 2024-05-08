using UI.Reticle;
using Zenject;

namespace Abilities
{
    public class AbilityThreeArrow : Ability
    {
        private ReticleService reticleService;
        
        [Inject]
        public void Construct(ReticleService reticleService)
        {
            this.reticleService = reticleService;

            UseButton = MovementSettings.BasicAttack;
        }
        
        public override void Use()
        {
            base.Use();
            var shotRays = reticleService.GetAllShotRays();

        }
    }
}