using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability : IAbility
    {
        protected List<ReactiveProperty<bool>> conditions = new List<ReactiveProperty<bool>>();
        public KeyCode UseButton { get; protected set; }
    
        public virtual void Use()
        {
        }

        public virtual void Halt()
        {
        }

        public virtual void Prepare(params object [] args)
        {
        }

        public void CheckAndUse()
        {
            if (AreConditionsMet())
                Use();
        }

        private bool AreConditionsMet()
        {
            return conditions.All(condition => condition.HasValue && condition.Value);
        }
    }
}
