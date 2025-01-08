using System;
using UniRx;
using UnityEngine;

namespace Abilities
{
    public class CooldownTimer : IDisposable
    {
        private float cooldownValue;

        private float remainingTime;

        private IDisposable timerDisposable;

        public ReactiveProperty<bool> IsOffCooldown = new ReactiveProperty<bool>();

        public CooldownTimer(float cooldownValue)
        {
            this.cooldownValue = cooldownValue;
            IsOffCooldown.Value = true;
            
            timerDisposable = Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    if (!IsOffCooldown.Value)
                    {
                        if (remainingTime <= 0)
                            IsOffCooldown.Value = true;
                        else
                            remainingTime -= Time.deltaTime;
                    }
                });
        }

        public void ResetCooldown(float partialValue = 1.0f)
        {
            remainingTime *= 1f - partialValue;
        }

        public void Start()
        {
            IsOffCooldown.Value = false;
            remainingTime = cooldownValue;
        }

        public void Dispose()
        {
            timerDisposable.Dispose();
        }
    }
}