using System;
using DG.Tweening;
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
        public void Launch(Vector3 finishPoint, float projectileTtl, float projectileSpeed)
        {
            transform.LookAt(finishPoint);
            velocity = transform.forward * projectileSpeed;
            var stuckDistance = bodyCollider.bounds.extents.z / 5f;

            motionDisposable = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    var collideCheck = unitColliderService.CollideAndStuck(bodyCollider, velocity, transform.position, stuckDistance);
                    transform.position += collideCheck.velocity;
                    if (collideCheck.isCollided)
                        motionDisposable.Dispose();
                });

            ttlDisposable = Observable.Timer(TimeSpan.FromSeconds(projectileTtl))
                .Subscribe(_ =>
                {
                    motionDisposable?.Dispose();
                    ttlDisposable?.Dispose();
                    Despawn();
                });
        }
    }
}