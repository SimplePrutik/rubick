using System;
using System.Collections.Generic;
using Zenject;

namespace Abilities
{
    public class AbilityFactory
    {
        private DiContainer container;

        private Dictionary<Type, Ability> abilityCache = new Dictionary<Type, Ability>();

        [Inject]
        public void Constuct(DiContainer container)
        {
            this.container = container;
        }

        public Ability Create(Type abilityType)
        {
            return (Ability) container.Instantiate(abilityType);
        }

        public Ability GetAbility<T>()
        {
            var abilityType = typeof(T);
            if (abilityCache.ContainsKey(abilityType))
                return abilityCache[abilityType];
            var ability = Create(abilityType);
            abilityCache[abilityType] = ability;
            return ability;
        }
    }
}