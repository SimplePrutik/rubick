using Pooling;
using UnityEngine;
using Zenject;

namespace Fight.Projectiles
{
    public abstract class BaseProjectile : PoolObject
    {
        protected UnitColliderService unitColliderService;
        
        [SerializeField] protected Collider collider;

        [Inject]
        public void Construct(UnitColliderService unitColliderService)
        {
            this.unitColliderService = unitColliderService;
        }
    }
}