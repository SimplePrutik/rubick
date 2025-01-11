using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Entities
{
    public abstract class BaseEnemy : MonoBehaviour, IDamagable
    {
        protected float health = 20f;
        protected int reward;
        
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
        
    }
}