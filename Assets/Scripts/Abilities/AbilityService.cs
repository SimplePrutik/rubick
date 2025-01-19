using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Abilities
{
    public class AbilityService : IDisposable
    {
        private List<IDisposable> abilityDisposables = new List<IDisposable>();
        
        public IDisposable EnableAbility(Ability ability)
        {
            var abilityDisposable = Observable
                .EveryUpdate()
                .ObserveOnMainThread()
                .Subscribe(_ =>
                {
                    if (!ability.AreConditionsMet())
                        return;
                    switch (ability.abilityType)
                    {
                        case Ability.AbilityType.Tap:
                        case Ability.AbilityType.Hold:
                            if (Input.GetKey(ability.UseButton))
                            {
                                ability.Use();
                            }
                            break;
                        case Ability.AbilityType.ChargeAndHold:
                        case Ability.AbilityType.ChargeAndTap:
                            if (Input.GetKey(ability.UseButton))
                            {
                                ability.Charge();
                            }
                            break;
                    }
                });
            return abilityDisposable;
        }

        public void DisableAbility(IDisposable disposable)
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