using UnityEngine;

namespace Pooling
{
    public abstract class PoolObject : MonoBehaviour
    {
        protected virtual void Despawn()
        {
            gameObject.SetActive(false);
            transform.localPosition = Vector3.zero;
        }

        public virtual void OnSpawn()
        {
            
        }
    }
}