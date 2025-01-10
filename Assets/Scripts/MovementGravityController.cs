using System;
using Extentions;
using ScriptableObjects;
using UniRx;
using UnityEngine;
using Zenject;

public class MovementGravityController : IDisposable
{
    private PhysicsSettings physicsSettings;
    private UnitColliderService unitColliderService;
    protected Transform bodyTransform;
    protected CapsuleCollider bodyCollider;
    protected Vector2 horizontalMovingVelocity;
    protected float verticalMovingVelocity;
    protected Vector3 acceleration;

    private readonly CompositeDisposable generalDisposable = new CompositeDisposable();
    
    public MovementGravityController(
        PhysicsSettings physicsSettings,
        UnitColliderService unitColliderService,
        CapsuleCollider bodyCollider, 
        Transform bodyTransform)
    {
        this.physicsSettings = physicsSettings;
        this.unitColliderService = unitColliderService;
        this.bodyTransform = bodyTransform;
        this.bodyCollider = bodyCollider;
        
        Observable
            .EveryUpdate()
            .ObserveOnMainThread()
            .Subscribe(_ =>
            {
                UpdateCheck();
                MoveBody();
            })
            .AddTo(generalDisposable);
        
        acceleration = Vector3.down * physicsSettings.GravityStrength;
    }

    protected virtual void UpdateCheck()
    {
        if (unitColliderService.IsLanded(bodyCollider, bodyTransform))
        {
            acceleration = acceleration.ChangeY(Mathf.Max(acceleration.y,0f));
            verticalMovingVelocity = Mathf.Max(verticalMovingVelocity, 0f);
        }
        else
        {
            acceleration = Vector3.down * physicsSettings.GravityStrength;
        }
    }

    private void MoveBody()
    {
        verticalMovingVelocity += acceleration.y * Time.deltaTime;
        horizontalMovingVelocity += acceleration.ConvertToHorizontalMovement() * Time.deltaTime;
        var movingVelocity = Extensions.ConvertToVolumetricMovement(horizontalMovingVelocity, verticalMovingVelocity);
        
        bodyTransform.position += unitColliderService.CollideAndSlideCapsule(
            bodyCollider,
            movingVelocity * Time.deltaTime,
            bodyTransform.position,
            0);
        
    }

    public void Dispose()
    {
        generalDisposable?.Dispose();
    }
}