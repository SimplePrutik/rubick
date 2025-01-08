using System;
using Extentions;
using ScriptableObjects;
using UniRx;
using UnityEngine;
using Zenject;

public class MovementService : IDisposable
{
    private Vector3 movingVelocity;
    private Vector2 horizontalMovingVelocity;
    private float verticalMovingVelocity;
    private Vector3 acceleration;
    private Vector3 appliedAcceleration;
    private Vector3 movementVector;

    private TpvCameraController tpvCameraController;
    private FpvCameraController fpvCameraController;
    private PhysicsSettings physicsSettings;
    private PlayerStats playerStats;
    private UnitColliderService unitColliderService;
    private Transform playerTransform;

    private readonly CompositeDisposable generalDisposable = new CompositeDisposable();
    
    public float VerticalMovingVelocity
    {
        get => verticalMovingVelocity;
        set => verticalMovingVelocity = value;
    }
    
    [Inject]
    public void Construct(
        TpvCameraController tpvCameraController,
        FpvCameraController fpvCameraController,
        PhysicsSettings physicsSettings,
        PlayerStats playerStats,
        UnitColliderService unitColliderService)
    {
        this.tpvCameraController = tpvCameraController;
        this.fpvCameraController = fpvCameraController;
        this.physicsSettings = physicsSettings;
        this.playerStats = playerStats;
        this.unitColliderService = unitColliderService;
    }

    public void Init(CapsuleCollider bodyCollider, Transform playerTransform)
    {
        this.playerTransform = playerTransform;
        
        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                horizontalMovingVelocity = new Vector2(0f, 0f); 
                if (!tpvCameraController.IsLocked || !fpvCameraController.IsLocked)
                {
                    Walk();
                    Rotate();
                }

                if (unitColliderService.IsLanded)
                {
                    acceleration = acceleration.ChangeY(Mathf.Max(acceleration.y,0f));
                    verticalMovingVelocity = Mathf.Max(verticalMovingVelocity, 0f);
                }
                else
                {
                    acceleration = Vector3.down * physicsSettings.GravityStrength;
                }
                acceleration += appliedAcceleration;

                horizontalMovingVelocity += acceleration.ConvertToHorizontalMovement() * Time.deltaTime;
                verticalMovingVelocity += acceleration.y * Time.deltaTime;
                movingVelocity = Extensions.ConvertToVolumetricMovement(horizontalMovingVelocity, verticalMovingVelocity);
                playerTransform.position += unitColliderService.CollideAndSlideCapsule(
                    bodyCollider,
                    movingVelocity * Time.deltaTime,
                    playerTransform.position,
                    0);
                
                appliedAcceleration = Vector3.zero;
            })
            .AddTo(generalDisposable);

        acceleration = Vector3.down * physicsSettings.GravityStrength;
        appliedAcceleration = Vector3.zero;
    }

    public void SetVerticalAcceleration(float value)
    {
        appliedAcceleration = appliedAcceleration.ChangeY(value);
    }
    
    private void Walk()
    {
        var direction = Vector2.zero;
        if (Input.GetKey(ButtonSettings.MoveForward))
        {
            direction += playerTransform.forward.ConvertToHorizontalMovement();
        }
        if (Input.GetKey(ButtonSettings.MoveBack))
        {
            direction -= playerTransform.forward.ConvertToHorizontalMovement();
        }
        if (Input.GetKey(ButtonSettings.MoveRight))
        {
            direction += playerTransform.right.ConvertToHorizontalMovement();
        }
        if (Input.GetKey(ButtonSettings.MoveLeft))
        {
            direction -= playerTransform.right.ConvertToHorizontalMovement();
        }
        horizontalMovingVelocity += direction.normalized * playerStats.MoveSpeed;
    }
    
    private void Rotate()
    {
        var currentRotation = playerTransform.localRotation.eulerAngles.y;
        playerTransform.rotation = Quaternion.Euler(0, 
            currentRotation + Input.GetAxis("Mouse X") * Time.deltaTime * playerStats.RotationSpeed, 0);
    }

    public void Dispose()
    {
        generalDisposable?.Dispose();
    }
}