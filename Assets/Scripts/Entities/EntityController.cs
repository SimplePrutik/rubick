using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class EntityController
    {
        private EnemyFactory enemyFactory;
        private RewardService rewardService;

        private Dictionary<BaseEnemy, IDisposable> enemies = new Dictionary<BaseEnemy, IDisposable>();
        
        [Inject]
        public void Construct(
            EnemyFactory enemyFactory,
            RewardService rewardService)
        {
            this.rewardService = rewardService;
            this.enemyFactory = enemyFactory;
        }

        public BaseEnemy SpawnEnemy<T>() where T : BaseEnemy
        {
            var enemy = enemyFactory.Create(typeof(T));
            enemies[enemy] = enemy.OnDead
                .Subscribe(_ =>
                {
                    rewardService.GiveReward(enemy.GetReward());
                    enemies[enemy].Dispose();
                    enemies.Remove(enemy);
                });
            return enemy;
        }

        public void SetRoot(Transform root)
        {
            enemyFactory.SetRoot(root);
        }
    }
}