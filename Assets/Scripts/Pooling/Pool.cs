using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class Pool<T> where T : PoolObject
    {
        private readonly Vector3 POOL_POSITION = new Vector3(10000f, 10000f, 10000f);
        private readonly string PREFAB_PATH;
        private List<T> poolObjects = new List<T>();
        private Transform rootObject;
        
        public Pool(float capacity, Transform rootObject, string prefabPath)
        {
            this.rootObject = rootObject;
            PREFAB_PATH = prefabPath;
            rootObject.name = $"{nameof(T)} Pool";
            rootObject.position = POOL_POSITION;
            var prefab = Resources.Load<T>(prefabPath);
            
            for (int i = 0; i < capacity; ++i)
            {
                var _poolObject = GameObject.Instantiate(prefab, rootObject);
                _poolObject.gameObject.SetActive(false);
                _poolObject.transform.SetParent(rootObject);
                poolObjects.Add(_poolObject as T);
            }
        }

        public T Spawn(Vector3 position)
        {
            var poolObject = poolObjects.Find(obj => !obj.gameObject.activeSelf);
            if (poolObject == null)
            {
                var prefab = Resources.Load<T>(PREFAB_PATH);
                poolObject = GameObject.Instantiate(prefab, rootObject);
                poolObject.transform.SetParent(rootObject);
                poolObjects.Add(poolObject);
            }
            poolObject.transform.position = position;
            poolObject.gameObject.SetActive(true);
            return poolObject;
        }
    }
}