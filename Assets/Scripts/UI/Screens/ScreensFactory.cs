using System;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ScreensFactory
    {
        private DiContainer container;
        private Transform uiRoot;

        [Inject]
        public void Construct(DiContainer container)
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