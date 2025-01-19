using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Entities
{
    public abstract class BaseEnemy : MonoBehaviour, IDamagable, IEntity
    {
        public enum EnemyState
        {
            Idle,
            Attack,
            Dead
        }

        protected int Id;
        protected float health = 20f;
        protected int reward = 1;
        
        public ReactiveCommand OnDead = new ReactiveCommand();

        public virtual void TakeDamage(float value, int sourceId)
        {
            health -= value;
            if (health <= 0f)
                Die();
        }

        //make own class for reward
        public virtual int GetReward() => reward;

        protected virtual void Die()
        {
            OnDead.Execute();
            gameObject.SetActive(false);
        }

        public int GetId()
        {
            throw new System.NotImplementedException();
        }
    }
}