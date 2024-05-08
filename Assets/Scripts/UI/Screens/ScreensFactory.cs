using System;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ScreensFactory : IFactory<Type, BaseScreen>
    {
        private readonly DiContainer container;
        private readonly Transform uiRoot;

        public ScreensFactory(DiContainer container)
        {
            this.container = container;
        
            var uiRootGameObject = new GameObject("UI Root");
            GameObject.DontDestroyOnLoad(uiRootGameObject);
            uiRoot = uiRootGameObject.transform;
        }

        public BaseScreen Create(Type screenType)
        {
            var prefab = Resources.Load($"UI/Screens/{screenType.Name}");
        
            return (BaseScreen)container.InstantiatePrefabForComponent(
                screenType, prefab, uiRoot, Array.Empty<object>());
        }
    }
}