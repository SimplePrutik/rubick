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
        
        public ReactiveCommand<Vector3> OnHit = new ReactiveCommand<Vector3>();
        public CompositeDisposable ProjectileDisposable { get; private set; }

        [Inject]
        public void Construct(UnitColliderService unitColliderService)
        {
            this.unitColliderService = unitColliderService;
        }

        public override void OnSpawn()
        {
            ProjectileDisposable = new CompositeDisposable();
        }

        protected override void Despawn()
        {
            base.Despawn();
            ProjectileDisposable.Dispose();
        }
    }
}