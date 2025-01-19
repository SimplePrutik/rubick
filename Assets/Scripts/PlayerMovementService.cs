using Extentions;
using ScriptableObjects;
using UnityEngine;
using Zenject;

public class PlayerMovementService
{
    private TpvCameraController tpvCameraController;
    private FpvCameraController fpvCameraController;
    private PlayerStats playerStats;
    
    private Vector3 appliedAcceleration;
    
    [Inject]
    public void Construct(
        TpvCameraController tpvCameraController,
        FpvCameraController fpvCameraController,
        PlayerStats playerStats)
    {
        this.tpvCameraController = tpvCameraController;
        this.fpvCameraController = fpvCameraController;
        this.playerStats = playerStats;
    }
    
    private Vector3 Walk(Transform bodyTransform)
    {
        var direction = Vector3.zero;
        if (Input.GetKey(ButtonSettings.MoveForward))
        {
            direction += bodyTransform.forward;
        }
        if (Input.GetKey(ButtonSettings.MoveBack))
        {
            direction -= bodyTransform.forward;
        }
        if (Input.GetKey(ButtonSettings.MoveRight))
        {
            direction += bodyTransform.right;
        }
        if (Input.GetKey(ButtonSettings.MoveLeft))
        {
            direction -= bodyTransform.right;
        }
        return direction.normalized * playerStats.MoveSpeed;
    }

    public Vector3 GetForce(Transform bodyTransform)
    {
        var velocity = new Vector3(0f, 0f); 
        if (!tpvCameraController.IsLocked || !fpvCameraController.IsLocked)
        {
            velocity += Walk(bodyTransform);
        }
        return velocity;
    }
    
    public Quaternion GetRotation(Transform bodyTransform)
    {
        var currentAngle = bodyTransform.localRotation.eulerAngles.y;
        return Quaternion.Euler(0, currentAngle + Input.GetAxis("Mouse X") * playerStats.RotationSpeed * Time.deltaTime, 0);
    }
}