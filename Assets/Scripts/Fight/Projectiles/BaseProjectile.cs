﻿using Pooling;
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

        [Inject]
        public void Construct(UnitColliderService unitColliderService)
        {
            this.unitColliderService = unitColliderService;
        }
    }
}