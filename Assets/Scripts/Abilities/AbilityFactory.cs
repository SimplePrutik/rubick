using System;
using Zenject;

namespace Abilities
{
    public class AbilityFactory
    {
        private DiContainer container;

        [Inject]
        public void Constuct(DiContainer container)
        {
            this.container = container;
        }

        public Ability Create(Type abilityType)
        {
            return (Ability) container.Instantiate(abilityType);
        }
    }
}