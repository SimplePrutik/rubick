using System;
using UnityEngine;
using Zenject;

namespace UI.Reticle
{
    public class ReticleFactory : IFactory<Type, BaseReticle>
    {
        private readonly DiContainer container;
        private ScreensService screensService;
        private Transform uiRoot => screensService.CurrentScreen.transform;

        public ReticleFactory(
            DiContainer container,
            ScreensService screensService)
        {
            this.container = container;
            this.screensService = screensService;
        }

        public BaseReticle Create(Type reticleType)
        {
            if (uiRoot == null)
                return null;
            var prefab = Resources.Load($"UI/Reticles/{reticleType.Name}");
        
            return (BaseReticle)container.InstantiatePrefabForComponent(
                reticleType, prefab, uiRoot, Array.Empty<object>());
        }
    }
}