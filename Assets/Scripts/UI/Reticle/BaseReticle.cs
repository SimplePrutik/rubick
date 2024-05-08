using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Reticle
{
    public abstract class BaseReticle : MonoBehaviour
    {
        [SerializeField] protected List<RectTransform> pointsOfFire;

        public List<Vector2> GetPointsOfFire()
        {
            // return pointsOfFire.Select(pof => transform.TransformPoint(pof.position).)
            return new List<Vector2>();
        }
    }
}