using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Pooling
{
    public class Pool<T> where T : PoolObject
    {
        private readonly Vector3 POOL_POSITION = new Vector3(10000f, 10000f, 10000f);
        private readonly string PREFAB_PATH;
        private List<T> poolObjects = new List<T>();
        private Transform rootObject;
        private DiContainer container;
        
        public Pool(int capacity, Transform rootObject, string prefabPath, DiContainer container)
        {
            this.rootObject = rootObject;
            this.container = container;
            PREFAB_PATH = prefabPath;
            rootObject.position = POOL_POSITION;
            var prefab = Resources.Load<T>(prefabPath);
            
            for (int i = 0; i < capacity; ++i)
            {
                var _poolObject = (T)container.InstantiatePrefabForComponent(typeof(T), prefab, rootObject, Array.Empty<object>());
                _poolObject.gameObject.SetActive(false);
                _poolObject.transform.SetParent(rootObject);
                poolObjects.Add(_poolObject);
            }
        }

        public T SpawnTransform(Vector3 position)
        {
            var poolObject = SpawnObject();
            poolObject.transform.position = position;
            return poolObject;
        }

        public T SpawnRectTransform(Vector2 position)
        {
            var poolObject = SpawnObject();
            poolObject.GetComponent<RectTransform>().anchoredPosition = position;
            return poolObject;
        }

        private T SpawnObject()
        {
            var poolObject = poolObjects.Find(obj => !obj.gameObject.activeSelf);
            if (poolObject == null)
            {
                var prefab = Resources.Load<T>(PREFAB_PATH);
                poolObject = (T)container.InstantiatePrefabForComponent(typeof(T), prefab, rootObject, Array.Empty<object>());
                poolObject.transform.SetParent(rootObject);
                poolObjects.Add(poolObject);
            }
            poolObject.gameObject.SetActive(true);
            poolObject.OnSpawn();
            return poolObject;
        }
    }
}