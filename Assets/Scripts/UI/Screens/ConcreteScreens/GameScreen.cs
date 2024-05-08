using Abilities;
using UI.Reticle;
using Zenject;

namespace UI
{
    public class GameScreen : BaseScreen
    {
        private ReticleService reticleService;
        
        [Inject]
        public void Construct(ReticleService reticleService)
        {
            this.reticleService = reticleService;
        }

        public override void Show()
        {
            base.Show();
            //take reticle from character service
            reticleService.GetOrCreateReticle<ThreePointReticle>();
        }

        public override void Hide()
        {
            base.Hide();
            reticleService.ClearCache();
        }
    }
}