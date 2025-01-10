using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Entities
{
    public abstract class BaseEnemy : MonoBehaviour, IDamagable
    {
        protected float health = 10f;
        protected float reward;
        
        public ReactiveCommand OnDead = new ReactiveCommand();

        public virtual void TakeDamage(float value, int sourceId)
        {
            health -= value;
            if (health <= 0f)
                Die();
        }

        protected virtual void Die()
        {
            OnDead.Execute();
            gameObject.SetActive(false);
        }
    }
}