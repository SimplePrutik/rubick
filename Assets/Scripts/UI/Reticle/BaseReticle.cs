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
            return pointsOfFire.Select(pof => (Vector2)pof.position).ToList();
        }
    }
}