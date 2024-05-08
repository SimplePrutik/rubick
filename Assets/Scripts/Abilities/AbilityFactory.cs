using System;
using Zenject;

namespace Abilities
{
    public class AbilityFactory : IFactory<Type, Ability>
    {
        private readonly DiContainer container;

        public AbilityFactory(DiContainer container)
        {
            this.container = container;
        }

        public Ability Create(Type abilityType)
        {
            return (Ability) container.Instantiate(abilityType);
        }
    }
}