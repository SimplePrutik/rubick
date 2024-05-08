using System;
using UniRx;
using UnityEngine;

namespace Pool
{
    public class PoolObject : MonoBehaviour
    {
        private IDisposable despawnDisposable;
        
        public ReactiveCommand OnDespawn = new ReactiveCommand();

        private void OnEnable()
        {
            despawnDisposable = OnDespawn.Subscribe(_ => Despawn());
        }

        private void Despawn()
        {
            gameObject.SetActive(false);
            transform.localPosition = Vector3.zero;
            despawnDisposable?.Dispose();
        }
    }
}