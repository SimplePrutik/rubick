using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability : IUsable
    {
        protected float cooldown;
        protected float remainingCooldown;
        protected List<ReactiveProperty<bool>> conditions = new List<ReactiveProperty<bool>>();
    
        private IDisposable cooldownTimerDisposable;
    
        public KeyCode UseButton { get; protected set; }
    
        public virtual void Use()
        {
            remainingCooldown = cooldown;
            cooldownTimerDisposable?.Dispose();
            cooldownTimerDisposable =
                Observable
                    .EveryUpdate()
                    .Subscribe(_ =>
                    {
                        if (remainingCooldown <= 0)
                        {
                            cooldownTimerDisposable.Dispose();
                            cooldownTimerDisposable = null;
                            return;
                        }
                        remainingCooldown = MathF.Max(0f, remainingCooldown - Time.deltaTime);
                    });
        }

        public void CheckAndUse()
        {
            if (AreConditionsMet())
                Use();
        }

        public bool IsOnCooldown(out float remainingCooldown)
        {
            remainingCooldown = this.remainingCooldown;
            return cooldownTimerDisposable != null;
        }

        private bool AreConditionsMet()
        {
            return conditions.All(condition => condition.HasValue && condition.Value) && !IsOnCooldown(out _);
        }
    }
}
