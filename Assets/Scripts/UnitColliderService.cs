using System;
using Map;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class UnitColliderService : IDisposable
{
    public ReactiveProperty<bool> IsLanded = new ReactiveProperty<bool>();

    private CompositeDisposable triggerColliderDisposable = new CompositeDisposable();
    public void Init(Collider groundCollider)
    {
        groundCollider
            .OnTriggerEnterAsObservable()
            .Subscribe(collider =>
            {
                if (collider.transform.GetComponent<EnvironmentObject>())
                    IsLanded.Value = true;
            })
            .AddTo(triggerColliderDisposable);
        
        groundCollider
            .OnTriggerExitAsObservable()
            .Subscribe(collider =>
            {
                if (collider.transform.GetComponent<EnvironmentObject>())
                    IsLanded.Value = false;
            })
            .AddTo(triggerColliderDisposable);
    }

    public void Dispose()
    {
        triggerColliderDisposable?.Dispose();
    }
}