using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public abstract class Ability : IUsable
{
    private float cooldown;
    private float remainingCooldown;
    private List<ReactiveProperty<bool>> conditions;
    
    private IDisposable cooldownTimerDisposable;
    
    public virtual void Use()
    {
        if (AreConditionsMet())
            return;
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
                        return;
                    }
                    remainingCooldown = MathF.Max(0f, remainingCooldown - Time.deltaTime);
                });
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
