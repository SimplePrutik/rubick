using Abilities;
using TMPro;
using UI.Reticle;
using UniRx;
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
            DamageIndicatorController damageIndicatorController,
            PlayerStatsController playerStatsController)
        {
            this.reticleService = reticleService;

            damageIndicatorController.Init(indicatorPoolRoot);

            playerStatsController.GoldCollected
                .Subscribe(value => { goldValue.text = value.ToString(); })
                .AddTo(this);
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