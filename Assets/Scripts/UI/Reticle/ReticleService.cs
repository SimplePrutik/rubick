using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace UI.Reticle
{
    public class ReticleService
    {
        private ReticleFactory reticleFactory;
        private Dictionary<Type, BaseReticle> reticlesCache = new Dictionary<Type, BaseReticle>();

        private CameraService cameraService;

        private BaseReticle currentReticle;
        
        [Inject]
        public void Construct(ReticleFactory reticleFactory, CameraService cameraService)
        {
            this.reticleFactory = reticleFactory;
            this.cameraService = cameraService;
        }

        public BaseReticle GetOrCreateReticle<T>() where T : BaseReticle
        {
            var reticleType = typeof(T);
            if (reticlesCache.ContainsKey(reticleType))
                currentReticle = reticlesCache[reticleType] as T;
            currentReticle = reticleFactory.Create(reticleType);
            reticlesCache[reticleType] = currentReticle;
            return currentReticle;
        }

        public void ClearCache()
        {
            foreach (var reticle in reticlesCache)
            {
                GameObject.Destroy(reticle.Value);
            }
            reticlesCache.Clear();
        }

        public List<Ray> GetAllShotRays()
        {
            return currentReticle.GetPointsOfFire().Select(pof =>
                cameraService.CurrentCamera().Camera.ScreenPointToRay(pof)).ToList();
        }
    }
}