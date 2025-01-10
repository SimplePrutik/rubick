using Extentions;
using ScriptableObjects;
using UnityEngine;

public class PlayerMovementController : MovementGravityController
{
    private TpvCameraController tpvCameraController;
    private FpvCameraController fpvCameraController;
    private PlayerStats playerStats;
    
    private Vector3 appliedAcceleration;
    
    public PlayerMovementController(
        TpvCameraController tpvCameraController,
        FpvCameraController fpvCameraController,
        PhysicsSettings physicsSettings, 
        PlayerStats playerStats, 
        UnitColliderService unitColliderService,
        CapsuleCollider bodyCollider, 
        Transform bodyTransform)
        : base(physicsSettings, unitColliderService, bodyCollider, bodyTransform)
    {
        this.tpvCameraController = tpvCameraController;
        this.fpvCameraController = fpvCameraController;
        this.playerStats = playerStats;
    }
    protected override void UpdateCheck()
    {
        horizontalMovingVelocity = new Vector2(0f, 0f); 
        if (!tpvCameraController.IsLocked || !fpvCameraController.IsLocked)
        {
            Walk();
            Rotate();
        }
        
        base.UpdateCheck();
        acceleration += appliedAcceleration;
                
        appliedAcceleration = Vector3.zero;
    }

    public void AddVerticalAcceleration(float value)
    {
        appliedAcceleration += appliedAcceleration.ChangeY(value);
    }
    
    private void Walk()
    {
        var direction = Vector2.zero;
        if (Input.GetKey(ButtonSettings.MoveForward))
        {
            direction += bodyTransform.forward.ConvertToHorizontalMovement();
        }
        if (Input.GetKey(ButtonSettings.MoveBack))
        {
            direction -= bodyTransform.forward.ConvertToHorizontalMovement();
        }
        if (Input.GetKey(ButtonSettings.MoveRight))
        {
            direction += bodyTransform.right.ConvertToHorizontalMovement();
        }
        if (Input.GetKey(ButtonSettings.MoveLeft))
        {
            direction -= bodyTransform.right.ConvertToHorizontalMovement();
        }
        horizontalMovingVelocity += direction.normalized * playerStats.MoveSpeed;
    }
    
    private void Rotate()
    {
        var currentRotation = bodyTransform.localRotation.eulerAngles.y;
        bodyTransform.rotation = Quaternion.Euler(
            0, 
            currentRotation + Input.GetAxis("Mouse X") * Time.deltaTime * playerStats.RotationSpeed, 
            0);
    }
}