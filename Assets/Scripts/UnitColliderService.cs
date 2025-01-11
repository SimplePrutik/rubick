using System;
using System.Collections.Generic;
using System.Linq;
using Map;
using UniRx;
using UnityEngine;

public class UnitColliderService : IDisposable
{
    public struct CollisionInfo
    {
        public Transform Hit;
        public Vector3 CollisionPosition;
    }
    
    private CompositeDisposable triggerColliderDisposable = new CompositeDisposable();

    private const int MAX_COLLIDE_BOUNCES = 5;
    private const float SKIN_WIDTH = 0.005f;
    
    public bool IsLanded(CapsuleCollider bodyCollider, Transform bodyTransform)
    {
        return Physics.Raycast(bodyTransform.position + Vector3.down * (bodyCollider.height / 2f), Vector3.down, 0.1f);
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
    
    public Vector3 CollideAndStuck(
        BoxCollider bodyCollider, 
        Vector3 velocity, 
        Vector3 position, 
        float depthOfStuck, 
        List<Type> collisionTypes, 
        out CollisionInfo collisionInfo)
    {
        var distance = velocity.magnitude;
        var halfExtents = bodyCollider.bounds.extents;
        var shiftedPosition = position - velocity.normalized * depthOfStuck;
        
        if (Physics.BoxCast(shiftedPosition, halfExtents, velocity.normalized, out var hit, Quaternion.identity, distance))
        {
            var collisionType = collisionTypes.Find(type => hit.transform.GetComponent(type) != null);
            if (collisionType != null)
            {
                var snapToSurface = velocity.normalized * hit.distance;
                collisionInfo = new CollisionInfo {Hit = hit.transform, CollisionPosition = position + snapToSurface};
                return snapToSurface;
            }
        }

        collisionInfo = new CollisionInfo {Hit = null, CollisionPosition = Vector3.zero};
        return velocity;
    }

    public void Dispose()
    {
        triggerColliderDisposable?.Dispose();
    }
}