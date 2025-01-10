using System;
using System.Collections.Generic;
using DG.Tweening;
using Entities;
using Map;
using UniRx;
using UnityEngine;

namespace Fight.Projectiles
{
    public class Arrow : BaseProjectile
    {
        private Tween projectileTween;
        private Vector3 velocity;
        private float damage = 0.5f;

        private IDisposable ttlDisposable;
        private IDisposable motionDisposable;
        
        private BoxCollider bodyCollider => collider as BoxCollider;
        public void Launch(Vector3 finishPoint, float projectileTtl, float projectileSpeed)
        {
            transform.LookAt(finishPoint);
            velocity = transform.forward * projectileSpeed;
            var stuckDistance = bodyCollider.bounds.extents.z / 5f;

            motionDisposable = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    var collideCheck = unitColliderService.CollideAndStuck(
                        bodyCollider, velocity, transform.position, stuckDistance, new List<Type>{typeof(EnvironmentObject), typeof(BaseEnemy)});
                    transform.position += collideCheck.velocity;
                    
                    if (collideCheck.hit == null)
                        return;
                    
                    if (collideCheck.hit.GetComponent<EnvironmentObject>() != null)
                    {
                        motionDisposable.Dispose();
                        return;
                    }
                    
                    var enemy = collideCheck.hit.GetComponent<BaseEnemy>();
                    if (enemy != null)
                    {
                        Despawn();
                        enemy.TakeDamage(damage, 0);
                    }
                });

            ttlDisposable = Observable.Timer(TimeSpan.FromSeconds(projectileTtl))
                .Subscribe(_ =>
                {
                    Despawn();
                });
        }

        protected override void Despawn()
        {
            motionDisposable?.Dispose();
            ttlDisposable?.Dispose();
            base.Despawn();
        }
    }
}