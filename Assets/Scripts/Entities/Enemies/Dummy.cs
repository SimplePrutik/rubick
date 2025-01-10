using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class Dummy : BaseEnemy
    {
        private Tween onTakingDamage;
        
        private MovementGravityController movementGravityController;
        
        [Inject]
        public void Construct(
            UnitColliderService unitColliderService,
            PhysicsSettings physicsSettings)
        {
            var collider = GetComponent<CapsuleCollider>();
            movementGravityController = new MovementGravityController(physicsSettings, unitColliderService, collider, transform);
        }

        public override void TakeDamage(float value, int sourceId)
        {
            //make appropriate animation transition
            onTakingDamage?.Kill();
            onTakingDamage = DOTween.Sequence()
                .Append(transform.DOScale(0.5f, 0.3f))
                .Append(transform.DOScale(1f, 0.3f));
            base.TakeDamage(value, sourceId);
        }

        protected override void Die()
        {
            onTakingDamage?.Kill();
            base.Die();
        }
    }
}