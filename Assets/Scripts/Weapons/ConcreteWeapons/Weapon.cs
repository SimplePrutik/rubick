using System.Collections.Generic;
using Abilities;
using UnityEngine;

namespace Weapons.ConcreteWeapons
{
    public abstract class Weapon : MonoBehaviour
    {
        protected Vector3 positionInHand;
        protected Vector3 rotationInHand;
        protected List<Ability> abilities;

        [SerializeField] protected List<GameObject> fireSourcePoints;
        
        
    }
}