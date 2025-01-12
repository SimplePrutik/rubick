using Abilities;
using TMPro;
using UI.Reticle;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameScreen : BaseScreen
    {
        private ReticleService reticleService;
        private DamageIndicatorController damageIndicatorController;

        [SerializeField] private TMP_Text goldValue;
        [SerializeField] private RectTransform indicatorPoolRoot;
        
        [Inject]
        public void Construct(
            ReticleService reticleService,
            DamageIndicatorController damageIndicatorController)
        {
            this.reticleService = reticleService;

            damageIndicatorController.Init(indicatorPoolRoot);
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