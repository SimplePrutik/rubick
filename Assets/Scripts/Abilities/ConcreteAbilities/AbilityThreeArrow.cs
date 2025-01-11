using Fight.Projectiles;
using Pooling;
using UI.Reticle;
using UniRx;
using Zenject;

namespace Abilities
{
    public class AbilityThreeArrow : Ability
    {
        private ReticleService reticleService;
        private DamageIndicatorController damageIndicatorController;
        private Pool<Arrow> arrowPool;
        private CooldownTimer cooldownTimer;
        
        private float projectileSpeed = 0.5f;
        private float damage = 2f;
        
        private float projectilePathLength => projectileSpeed * PROJECTILE_TTL;

        private const float PROJECTILE_TTL = 5f;

        [Inject]
        public void Construct(
            ReticleService reticleService,
            DamageIndicatorController damageIndicatorController)
        {
            this.reticleService = reticleService;
            this.damageIndicatorController = damageIndicatorController;

            UseButton = ButtonSettings.BasicAttack;
            
            cooldownTimer = new CooldownTimer(0.4f);
            
            conditions.Add(cooldownTimer.IsOffCooldown);
        }
        
        public override void Use()
        {
            base.Use();
            cooldownTimer.Start();
            var shotRays = reticleService.GetAllShotRays();
            foreach (var ray in shotRays)
            {
                var arrow = arrowPool.Spawn(ray.origin);
                var finishPoint = ray.origin + ray.direction.normalized * projectilePathLength;
                arrow.Launch(finishPoint, PROJECTILE_TTL, projectileSpeed, damage);
                arrow.OnHit
                    .Subscribe(collisionInfo =>
                    {
                        damageIndicatorController.SpawnIndicator(damage, collisionInfo.Hit,
                            collisionInfo.CollisionPosition);
                    })
                    .AddTo(arrow);
            }
        }

        public override void Prepare(params object[] args)
        {
            base.Prepare();
            arrowPool = (Pool<Arrow>) args[0];
        }
    }
}