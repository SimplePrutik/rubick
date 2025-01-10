using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Abilities
{
    public class AbilityService : IDisposable
    {
        private List<IDisposable> abilityDisposables = new List<IDisposable>();
        
        public IDisposable InitAbility(Ability ability)
        {
            var abilityDisposable = Observable
                .EveryUpdate()
                .ObserveOnMainThread()
                .Subscribe(_ =>
                {
                    if (Input.GetKey(ability.UseButton))
                    {
                        ability.CheckAndUse();
                    }
                });
            return abilityDisposable;
        }

        public void RemoveAbility(IDisposable disposable)
        {
            disposable?.Dispose();
            abilityDisposables.Remove(disposable);
        }
        
        public void Dispose()
        {
            abilityDisposables.ForEach(abilityDisposable => abilityDisposable?.Dispose());
            abilityDisposables.Clear();
        }
    }
}