using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Movement
{
    public class UnitMovementController
    {
        private UnitColliderService unitColliderService;
        
        private List<Vector3> movementForces = new List<Vector3>();

        public UnitMovementController(UnitColliderService unitColliderService)
        {
            this.unitColliderService = unitColliderService;
        }

        public void AddMovementForce(Vector3 value)
        {
            movementForces.Add(value);
        }

        public Vector3 DeltaMovement(CapsuleCollider bodyCollider, Vector3 position)
        {
            var sumForce = Sum(movementForces) * Time.deltaTime;
            movementForces.Clear();
            return unitColliderService.CollideAndSlideCapsule(
                bodyCollider,
                sumForce,
                position,
                0);
        }

        private Vector3 Sum(List<Vector3> content)
        {
            return content.Count == 0 ? Vector3.zero : content.Aggregate((v1, v2) => v1 + v2);
        }
    }
}