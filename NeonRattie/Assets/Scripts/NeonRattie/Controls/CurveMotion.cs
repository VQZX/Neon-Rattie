using UnityEngine;

namespace NeonRattie.Controls
{
    public class CurveMotion<TMovable> where TMovable : IMovable
    {
        public Curve Curve { get; private set; }

        public CurveMotion (Curve curve)
        {
            Curve = curve;
        }

        public void Tick (TMovable movable)
        {
            Vector3 point = Curve.CurrentPoint();
            movable.TryMove(point);
        }
    }
}
