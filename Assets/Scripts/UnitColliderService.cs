using System;
using Map;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class UnitColliderService : IDisposable
{
    public ReactiveCommand IsLanded = new ReactiveCommand();
    public ReactiveCommand IsFlown = new ReactiveCommand();

    private CompositeDisposable triggerColliderDisposable = new CompositeDisposable();
    public void Init(Collider groundCollider)
    {
        groundCollider
            .OnTriggerEnterAsObservable()
            .Subscribe(collider =>
            {
                if (collider.transform.GetComponent<EnvironmentObject>())
                    IsLanded.Execute();
            })
            .AddTo(triggerColliderDisposable);
        
        groundCollider
            .OnTriggerExitAsObservable()
            .Subscribe(collider =>
            {
                if (collider.transform.GetComponent<EnvironmentObject>())
                    IsFlown.Execute();
            })
            .AddTo(triggerColliderDisposable);
    }

    public void Dispose()
    {
        triggerColliderDisposable?.Dispose();
    }
}