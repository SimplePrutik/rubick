using ScriptableObjects;
using UnityEngine;
using Zenject;

public class GroundGravityController
{
    private readonly PhysicsSettings physicsSettings;
    private readonly UnitColliderService unitColliderService;
    private readonly Transform bodyTransform;
    private readonly CapsuleCollider bodyCollider;

    private float currentGravityForce;
    
    public float ExteriorForce { get; set; }
    
    public GroundGravityController(
        PhysicsSettings physicsSettings,
        UnitColliderService unitColliderService,
        Transform bodyTransform, 
        CapsuleCollider bodyCollider)
    {
        this.physicsSettings = physicsSettings;
        this.unitColliderService = unitColliderService;
        this.bodyTransform = bodyTransform;
        this.bodyCollider = bodyCollider;
    }

    public Vector3 GetForce()
    {
        if (unitColliderService.IsLanded(bodyCollider, bodyTransform))
        {
            currentGravityForce = 0f;
        }
        else
        {
            currentGravityForce += physicsSettings.GravityStrength;
        }

        currentGravityForce += ExteriorForce;
        ExteriorForce = 0f;
        return Vector3.down * currentGravityForce;
    }
}