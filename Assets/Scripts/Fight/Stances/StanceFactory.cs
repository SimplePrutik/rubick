using System;
using Zenject;

namespace Fight.Stances
{
    public class StanceFactory : IFactory<Type, BaseStance>
    {
        private readonly DiContainer container;

        public StanceFactory(DiContainer container)
        {
            this.container = container;
        }

        public BaseStance Create(Type stanceType)
        {
            return (BaseStance) container.Instantiate(stanceType);
        }
    }
}