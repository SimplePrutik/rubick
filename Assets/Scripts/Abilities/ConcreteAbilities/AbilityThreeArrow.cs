using System;
using System.Linq;
using Fight.Projectiles;
using Pooling;
using UI.Reticle;
using UniRx;
using UnityEngine;
using Zenject;

namespace Abilities
{
    public class AbilityThreeArrow : Ability
    {
        private ReticleService reticleService;
        private DamageIndicatorController damageIndicatorController;
        private Pool<Arrow> arrowPool;
        private CooldownTimer cooldownTimer;
        private IDisposable chargeDisposable;
        
        private float projectileSpeed = 0.5f;
        private float damage = 2f;
        private float chargeTime = 1f;
        private KeyCode CancelButton;
        
        private float projectilePathLength => projectileSpeed * PROJECTILE_TTL;

        private const float PROJECTILE_TTL = 5f;

        public ReactiveProperty<float> ChargeState = new ReactiveProperty<float>();

        [Inject]
        public void Construct(
            ReticleService reticleService,
            DamageIndicatorController damageIndicatorController)
        {
            this.reticleService = reticleService;
            this.damageIndicatorController = damageIndicatorController;

            UseButton = ButtonSettings.MainUse;
            CancelButton = ButtonSettings.SecondaryUse;
            abilityType = AbilityType.ChargeAndHold;
            ChargeState.Value = 0;
            
            cooldownTimer = new CooldownTimer(0.4f);
            
            conditions.Add(cooldownTimer.IsOffCooldown);
        }
        
        public override void Use()
        {
            base.Use();
            chargeDisposable?.Dispose();
            cooldownTimer.Start();
            var shotRays = reticleService.GetAllShotRays();
            foreach (var ray in shotRays)
            {
                var arrow = arrowPool.SpawnTransform(ray.origin);
                var finishPoint = ray.origin + ray.direction.normalized * projectilePathLength;
                arrow.Launch(finishPoint, PROJECTILE_TTL, projectileSpeed, damage);
                arrow.OnHit
                    .Subscribe(collisionPosition =>
                    {
                        damageIndicatorController.SpawnIndicator(damage, collisionPosition);
                    })
                    .AddTo(arrow.ProjectileDisposable);
            }
        }

        public override void Charge()
        {
            base.Charge();
            chargeDisposable = Observable
                .EveryUpdate()
                .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(chargeTime)))
                .Subscribe(_ =>
                {
                    if (!Input.GetKey(UseButton))
                        if (ChargeState.Value >= chargeTime)
                        {
                            Use();
                        }
                        else
                        {
                            Halt();
                        }
                    else if (Input.GetKey(CancelButton))
                    {
                        Halt();
                    }
                    ChargeState.Value += Time.deltaTime / chargeTime;
                });

        }

        public override void Halt()
        {
            base.Halt();
            chargeDisposable?.Dispose();
            ChargeState.Value = 0f;
        }
    }
}