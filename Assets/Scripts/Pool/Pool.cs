using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class Pool<T> where T : PoolObject
    {
        private T poolObjectPrefab;
        private readonly Vector3 POOL_POSITION = new Vector3(10000f, 10000f, 10000f);
        private List<T> poolObjects = new List<T>();
        private Transform rootObject;
        
        public Pool(T poolObject, float capacity, Transform rootObject)
        {
            this.rootObject = rootObject;
            rootObject.name = $"{nameof(T)} Pool";
            poolObjectPrefab = poolObject;
            rootObject.position = POOL_POSITION;
            for (int i = 0; i < capacity; ++i)
            {
                var _poolObject = GameObject.Instantiate(poolObject, rootObject);
                _poolObject.gameObject.SetActive(false);
                poolObjects.Add(_poolObject);
            }
            poolObjectPrefab = poolObject;
        }

        public T Spawn(Vector3 position)
        {
            var poolObject = poolObjects.Find(obj => !obj.gameObject.activeSelf);
            if (poolObject == null)
            {
                poolObject = GameObject.Instantiate(poolObjectPrefab, rootObject);
                poolObjects.Add(poolObject);
            }
            poolObject.transform.position = position;
            poolObject.gameObject.SetActive(true);
            return poolObject;
        }
    }
}