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

        private IDisposable ttlDisposable;
        private IDisposable motionDisposable;
        
        private BoxCollider bodyCollider => collider as BoxCollider;
        public void Launch(Vector3 finishPoint, float projectileTtl, float projectileSpeed, float damage)
        {
            transform.LookAt(finishPoint);
            velocity = transform.forward * projectileSpeed;
            var stuckDistance = bodyCollider.bounds.extents.z / 5f;

            motionDisposable = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    var nextVelocity = unitColliderService.CollideAndStuck(
                        bodyCollider, 
                        velocity, 
                        transform.position, 
                        stuckDistance, 
                        new List<Type>{typeof(EnvironmentObject), typeof(BaseEnemy)},
                        out var collisionInfo);
                    transform.position += nextVelocity;
                    
                    if (collisionInfo.Hit == null)
                        return;
                    
                    if (collisionInfo.Hit.GetComponent<EnvironmentObject>() != null)
                    {
                        motionDisposable.Dispose();
                        return;
                    }
                    
                    var enemy = collisionInfo.Hit.GetComponent<BaseEnemy>();
                    if (enemy != null)
                    {
                        OnHit.Execute(collisionInfo);
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