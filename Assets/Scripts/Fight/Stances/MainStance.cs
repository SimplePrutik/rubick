using System;
using Abilities;
using UI.Reticle;
using Zenject;

namespace Fight.Stances
{
    public class MainStance
    {
        private Type reticleType;
        private ReticleService reticleService;
        [Inject]
        public void Construct(ReticleService reticleService)
        {
            this.reticleService = reticleService;
        }

        public void Init(Type reticleType)
        {
            this.reticleType = reticleType;
        }
        
        public void TurnOn()
        {
            
        }

        private BaseReticle GetOrCreateReticle<T>() where T : BaseReticle
        {
            return reticleService.GetOrCreateReticle<T>();
        }
    }
}