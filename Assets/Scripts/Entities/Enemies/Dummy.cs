using DG.Tweening;
using Movement;
using ScriptableObjects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class Dummy : BaseEnemy
    {
        private UnitMovementController unitMovementController;
        private GroundGravityController groundGravityController;
        
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color hitColor;
        [SerializeField] private Collider enemyDetectionCollider;

        private Tween onTakingDamage;

        private EnemyState state;
        private Transform target;
        private CapsuleCollider bodyCollider;
        
        [Inject]
        public void Construct(
            UnitColliderService unitColliderService,
            PhysicsSettings physicsSettings)
        {
            bodyCollider = GetComponent<CapsuleCollider>();
            
            unitMovementController = new UnitMovementController(unitColliderService);
            groundGravityController = new GroundGravityController(physicsSettings, unitColliderService, transform, bodyCollider);

            Observable
                .EveryUpdate()
                .ObserveOnMainThread()
                .Subscribe(_ =>
                {
                    unitMovementController.AddMovementForce(groundGravityController.GetForce());
                    transform.position += unitMovementController.DeltaMovement(bodyCollider, transform.position);
                })
                .AddTo(this);
            
            meshRenderer.material = new Material(meshRenderer.material) {color = normalColor};
        }

        public override void TakeDamage(float value, int sourceId)
        {
            onTakingDamage?.Kill();
            meshRenderer.material.color = hitColor;
            onTakingDamage = meshRenderer.material.DOColor(normalColor, 0.5f);
            
            base.TakeDamage(value, sourceId);
        }

        protected override void Die()
        {
            onTakingDamage?.Kill();
            base.Die();
        }

        private void Update()
        {
            switch (state)
            {
                case EnemyState.Idle:
                    break;
                case EnemyState.Attack:
                    break;
                case EnemyState.Dead:
                    break;
            }
        }
    }
}