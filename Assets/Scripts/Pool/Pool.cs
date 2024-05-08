using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Pool
{
    public class Pool<T> : MonoBehaviour where T : PoolObject
    {
        private T poolObjectPrefab;
        private readonly Vector3 POOL_POSITION = new Vector3(10000f, 10000f, 10000f);
        private List<T> poolObjects;
        
        public Pool(T poolObject, float capacity)
        {
            poolObjectPrefab = poolObject;
            transform.position = POOL_POSITION;
            for (int i = 0; i < capacity; ++i)
            {
                var _poolObject = Instantiate(poolObject, transform);
                _poolObject.gameObject.SetActive(false);
                poolObjects.Add(_poolObject);
            }
        }

        public T Spawn(Vector3 position)
        {
            var poolObject = poolObjects.Find(obj => !obj.gameObject.activeSelf);
            if (poolObject == null)
            {
                poolObject = Instantiate(poolObjectPrefab, transform);
                poolObjects.Add(poolObject);
            }
            poolObject.transform.position = position;
            poolObject.gameObject.SetActive(true);
            return poolObject;
        }
    }
}