using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability
    {
        public enum AbilityType
        {
            Tap,
            ChargeAndTap,
            ChargeAndHold,
            Hold
        }
        
        protected List<ReactiveProperty<bool>> conditions = new List<ReactiveProperty<bool>>();
        public AbilityType abilityType;
        public KeyCode UseButton { get; protected set; }
    
        public virtual void Use()
        {
        }
        
        public virtual void Charge()
        {
        }

        public virtual void Halt()
        {
        }

        public bool AreConditionsMet()
        {
            return conditions.All(condition => condition.HasValue && condition.Value);
        }
    }
}
