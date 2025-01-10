using Entities;
using UnityEngine;
using Zenject;

namespace Extentions
{
    public class EntityController
    {
        private EnemyFactory enemyFactory;
        
        [Inject]
        public void Construct(EnemyFactory enemyFactory)
        {
            this.enemyFactory = enemyFactory;
        }

        public BaseEnemy SpawnEnemy<T>() where T : BaseEnemy
        {
            return enemyFactory.Create(typeof(T));
        }

        public void SetRoot(Transform root)
        {
            enemyFactory.SetRoot(root);
        }
    }
}