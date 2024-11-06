using UnityEngine;

namespace Pool
{
    public class PoolObject : MonoBehaviour
    {
        protected void Despawn()
        {
            gameObject.SetActive(false);
            transform.localPosition = Vector3.zero;
        }
    }
}