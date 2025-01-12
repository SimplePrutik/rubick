using System;
using DG.Tweening;
using ScriptableObjects;
using UniRx;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class Dummy : BaseEnemy
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color hitColor;

        private Tween onTakingDamage;
        
        private MovementGravityController movementGravityController;
        
        [Inject]
        public void Construct(
            UnitColliderService unitColliderService,
            PhysicsSettings physicsSettings)
        {
            var collider = GetComponent<CapsuleCollider>();
            movementGravityController = new MovementGravityController(physicsSettings, unitColliderService, collider, transform);
            
            meshRenderer.material = new Material(meshRenderer.material);
            meshRenderer.material.color = normalColor;
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
    }
}