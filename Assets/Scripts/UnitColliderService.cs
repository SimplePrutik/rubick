using System;
using Map;
using UniRx;
using UnityEngine;

public class UnitColliderService : IDisposable
{
    private CompositeDisposable triggerColliderDisposable = new CompositeDisposable();
    private IDisposable onGroundedDisposable;

    private const int MAX_COLLIDE_BOUNCES = 5;
    private const float SKIN_WIDTH = 0.005f;
    
    public ReactiveProperty<bool> IsLanded = new ReactiveProperty<bool>();
    public void InitGroundCheck(CapsuleCollider bodyCollider, Transform bodyTransform)
    {
        onGroundedDisposable?.Dispose();
        onGroundedDisposable = Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                if (Physics.Raycast(bodyTransform.position + Vector3.down * (bodyCollider.height / 2f), Vector3.down, 0.1f))
                {
                    if (!IsLanded.Value)
                        IsLanded.Value = true;
                    return;
                }
                if (IsLanded.Value)
                    IsLanded.Value = false;
            });
    }

    public Vector3 CollideAndSlideCapsule(CapsuleCollider bodyCollider, Vector3 velocity, Vector3 position, int depth)
    {
        if (depth >= MAX_COLLIDE_BOUNCES)
            return Vector3.zero;

        var distance = velocity.magnitude + SKIN_WIDTH;
        var distanceToSphere = bodyCollider.height / 2f - bodyCollider.radius;
        
        var p1 = position + Vector3.down * distanceToSphere;
        var p2 = position + Vector3.up * distanceToSphere;
        if (Physics.CapsuleCast(p1, p2, bodyCollider.radius, velocity.normalized, out var hit, distance))
        {
            var snapToSurface = velocity.normalized * (hit.distance - SKIN_WIDTH);
            var leftOver = velocity - snapToSurface;
            
            if (snapToSurface.magnitude <= SKIN_WIDTH)
                snapToSurface = Vector3.zero;

            var magnitude = leftOver.magnitude;
            leftOver = Vector3.ProjectOnPlane(leftOver, hit.normal).normalized;
            leftOver *= magnitude;

            return snapToSurface + CollideAndSlideCapsule(bodyCollider, leftOver, position + snapToSurface, depth + 1);
        }
        
        return velocity;
    }
    
    public (Vector3 velocity, bool isCollided) CollideAndStuck(BoxCollider bodyCollider, Vector3 velocity, Vector3 position, float depthOfStuck)
    {
        var distance = velocity.magnitude;
        var halfExtents = bodyCollider.bounds.extents;
        var shiftedPosition = position - velocity.normalized * depthOfStuck;
        
        DrawBoxCastBox(shiftedPosition, halfExtents, velocity.normalized, distance, Color.cyan);
        if (Physics.BoxCast(shiftedPosition, halfExtents, velocity.normalized, out var hit, Quaternion.identity, distance)
            && hit.transform.GetComponent<EnvironmentObject>() != null)
        {
            var snapToSurface = velocity.normalized * hit.distance;
            return (snapToSurface, true);
        }
        
        return (velocity, false);
    }

    void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Vector3 direction, float distance, Color color)
    {
        Vector3 forward = direction * distance;
        Vector3[] points = new Vector3[8];
        points[0] = origin + forward + new Vector3(halfExtents.x, halfExtents.y, halfExtents.z);
        points[1] = origin + forward + new Vector3(-halfExtents.x, halfExtents.y, halfExtents.z);
        points[2] = origin + forward + new Vector3(halfExtents.x, -halfExtents.y, halfExtents.z);
        points[3] = origin + forward + new Vector3(-halfExtents.x, -halfExtents.y, halfExtents.z);
        points[4] = origin + forward + new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
        points[5] = origin + forward + new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
        points[6] = origin + forward + new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);
        points[7] = origin + forward + new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
        for (int i = 0; i < 4; i++)
        {
            Debug.DrawLine(points[i], points[(i + 1) % 4], color);
            Debug.DrawLine(points[i + 4], points[(i + 1) % 4 + 4], color);
            Debug.DrawLine(points[i], points[i + 4], color);
        }

        Debug.DrawLine(origin, origin + forward, color);
    }

    public void Dispose()
    {
        triggerColliderDisposable?.Dispose();
    }
}