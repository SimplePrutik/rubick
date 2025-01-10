using Fight.Projectiles;
using Pooling;
using UI.Reticle;
using Zenject;

namespace Abilities
{
    public class AbilityThreeArrow : Ability
    {
        private ReticleService reticleService;
        private Pool<Arrow> arrowPool;
        private CooldownTimer cooldownTimer;
        public float ProjectileSpeed { get; private set; } = 0.5f;
        
        private float projectilePathLength => ProjectileSpeed * PROJECTILE_TTL;

        private const float PROJECTILE_TTL = 5f;

        [Inject]
        public void Construct(ReticleService reticleService)
        {
            this.reticleService = reticleService;

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
                arrow.Launch(finishPoint, PROJECTILE_TTL, ProjectileSpeed);
            }
        }

        public override void Prepare(params object[] args)
        {
            base.Prepare();
            arrowPool = (Pool<Arrow>) args[0];
        }
    }
}