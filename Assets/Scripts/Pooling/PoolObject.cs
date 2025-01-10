using UnityEngine;

namespace Pooling
{
    public class PoolObject : MonoBehaviour
    {
        protected virtual void Despawn()
        {
            gameObject.SetActive(false);
            transform.localPosition = Vector3.zero;
        }
    }
}