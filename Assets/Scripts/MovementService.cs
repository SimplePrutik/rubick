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
    private Vector3 movementVector;

    private TpvCameraController tpvCameraController;
    private FpvCameraController fpvCameraController;
    private UnitColliderService unitColliderService;
    private PlayerStats playerStats;
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
        UnitColliderService unitColliderService)
    {
        this.tpvCameraController = tpvCameraController;
        this.fpvCameraController = fpvCameraController;
        this.unitColliderService = unitColliderService;
    }

    public void Init(
        PhysicsSettings physicsSettings,
        PlayerStats playerStats,
        Transform playerTransform,
        UnitColliderService unitColliderService)
    {
        this.playerStats = playerStats;
        this.playerTransform = playerTransform;
        this.unitColliderService = unitColliderService;
        
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

                horizontalMovingVelocity += acceleration.ConvertToHorizontalMovement() * Time.deltaTime;
                verticalMovingVelocity += acceleration.y * Time.deltaTime;
                movingVelocity = Extensions.ConvertToVolumetricMovement(horizontalMovingVelocity, verticalMovingVelocity);
                playerTransform.position += movingVelocity * Time.deltaTime;
            })
            .AddTo(generalDisposable);

        
        this.unitColliderService
            .IsLanded
            .Subscribe(value =>
            {
                if (value)
                {
                    acceleration = acceleration.ChangeY(0f);
                    verticalMovingVelocity = 0f;
                }
                else
                {
                    acceleration = Vector3.down * physicsSettings.GravityStrength;
                }
            })
            .AddTo(generalDisposable);
        
        acceleration = Vector3.down * physicsSettings.GravityStrength;
    }
    
    private void Walk()
    {
        if (Input.GetKey(MovementSettings.MoveForward))
        {
            horizontalMovingVelocity += playerTransform.forward.ConvertToHorizontalMovement() * playerStats.MoveSpeed;
        }
        if (Input.GetKey(MovementSettings.MoveBack))
        {
            horizontalMovingVelocity -= playerTransform.forward.ConvertToHorizontalMovement() * playerStats.MoveSpeed;
        }
        if (Input.GetKey(MovementSettings.MoveRight))
        {
            horizontalMovingVelocity += playerTransform.right.ConvertToHorizontalMovement() * playerStats.MoveSpeed;
        }
        if (Input.GetKey(MovementSettings.MoveLeft))
        {
            horizontalMovingVelocity -= playerTransform.right.ConvertToHorizontalMovement() * playerStats.MoveSpeed;
        }
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