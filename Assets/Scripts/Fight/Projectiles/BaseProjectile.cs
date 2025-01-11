using Pooling;
using UniRx;
using UnityEngine;
using Zenject;

namespace Fight.Projectiles
{
    public abstract class BaseProjectile : PoolObject
    {
        protected UnitColliderService unitColliderService;
        
        [SerializeField] protected Collider collider;
        
        public ReactiveCommand<UnitColliderService.CollisionInfo> OnHit = new ReactiveCommand<UnitColliderService.CollisionInfo>();

        [Inject]
        public void Construct(UnitColliderService unitColliderService)
        {
            this.unitColliderService = unitColliderService;
        }
    }
}