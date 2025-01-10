using System;
using UI;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class EnemyFactory
    {
        private DiContainer container;
        private Transform root;

        [Inject]
        public void Construct(DiContainer container)
        {
            this.container = container;
        }

        public BaseEnemy Create(Type enemyType)
        {
            var prefab = Resources.Load($"Prefabs/3D/Enemies/{enemyType.Name}");
        
            return (BaseEnemy)container.InstantiatePrefabForComponent(enemyType, prefab, root, Array.Empty<object>());
        }

        public void SetRoot(Transform root)
        {
            this.root = root;
        }
    }
}