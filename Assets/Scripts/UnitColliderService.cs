using System;
using Map;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class UnitColliderService : IDisposable
{
    private CompositeDisposable triggerColliderDisposable = new CompositeDisposable();
    private IDisposable onGroundedDisposable;

    private Transform bodyTransform;
    private CapsuleCollider bodyCollider;

    private const int MAX_COLLIDE_BOUNCES = 5;
    private const float SKIN_WIDTH = 0.015f;
    
    public ReactiveProperty<bool> IsLanded = new ReactiveProperty<bool>();
    public void Init(
        CapsuleCollider bodyCollider,
        Transform bodyTransform)
    {
        this.bodyCollider = bodyCollider;
        this.bodyTransform = bodyTransform;

        onGroundedDisposable?.Dispose();
        onGroundedDisposable = Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                if (Physics.SphereCast(
                    bodyTransform.position + Vector3.down * (bodyCollider.height / 2f + bodyCollider.radius),
                    SKIN_WIDTH,
                    Vector3.down,
                    out var _,
                    0.1f))
                {
                    if (!IsLanded.Value)
                        IsLanded.Value = true;
                    return;
                }
                if (IsLanded.Value)
                    IsLanded.Value = false;
            });
    }

    public Vector3 CollideAndSlide(Vector3 velocity, Vector3 position, int depth)
    {
        if (depth >= MAX_COLLIDE_BOUNCES)
            return Vector3.zero;

        var distance = velocity.magnitude + SKIN_WIDTH;

        var height = bodyCollider.height;
        var p1 = position + Vector3.down * height / 2f;
        var p2 = p1 + Vector3.up * height;
        if (Physics.CapsuleCast(p1, p2, bodyCollider.radius, velocity.normalized, out var hit, distance))
        {
            var snapToSurface = velocity.normalized * (hit.distance - SKIN_WIDTH);
            var leftOver = velocity - snapToSurface;
            
            if (snapToSurface.magnitude <= SKIN_WIDTH)
                snapToSurface = Vector3.zero;

            var magnitude = leftOver.magnitude;
            leftOver = Vector3.ProjectOnPlane(leftOver, hit.normal).normalized;
            leftOver *= magnitude;

            return snapToSurface + CollideAndSlide(leftOver, position + snapToSurface, depth + 1);
        }
        
        return velocity;
    }

    public void Dispose()
    {
        triggerColliderDisposable?.Dispose();
    }
}