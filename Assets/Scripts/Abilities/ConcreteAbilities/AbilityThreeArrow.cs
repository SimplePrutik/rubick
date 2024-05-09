using DG.Tweening;
using Fight.Projectiles;
using Pool;
using UI.Reticle;
using Zenject;

namespace Abilities
{
    public class AbilityThreeArrow : Ability
    {
        private ReticleService reticleService;
        private Pool<Arrow> arrowPool;
        
        [Inject]
        public void Construct(ReticleService reticleService)
        {
            this.reticleService = reticleService;

            UseButton = MovementSettings.BasicAttack;
            cooldown = 1f;
        }
        
        public override void Use()
        {
            base.Use();
            var shotRays = reticleService.GetAllShotRays();
            foreach (var ray in shotRays)
            {
                var arrow = arrowPool.Spawn(ray.origin);
                var finishPoint = ray.origin + ray.direction.normalized * 5f;
                arrow.transform.LookAt(finishPoint);
                arrow.transform
                    .DOMove(finishPoint, 3f)
                    .OnKill(() => arrow.OnDespawn.Execute());
            }
        }

        public override void Prepare(params object[] args)
        {
            base.Prepare();
            arrowPool = (Pool<Arrow>) args[0];
        }
    }
}