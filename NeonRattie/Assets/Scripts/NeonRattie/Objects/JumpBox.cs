using Flusk.DataHelp;
using NeonRattie.Controls;
using NeonRattie.Rat;
using UnityEngine;

namespace NeonRattie.Objects
{
    [RequireComponent(typeof(Rigidbody))]
    public class JumpBox : MonoBehaviour
    {
        [SerializeField]
        protected LayerMask jumpLayer;

        [SerializeField]
        protected Transform jumpPoint;
        public Transform JumpPoint
        {
            get { return jumpPoint; }
        }

        private Material material;


        public void Select (bool state = true)
        {
            material.color = state ? Color.red : Color.white;
        }

        public Vector3 GetJumpPoint(RatController climber)
        {
            Vector3 output = jumpPoint.position;
            Vector3 normal = -climber.LocalForward.normalized;
            Plane plane = new Plane(normal, jumpPoint.position);
            Ray ray = new Ray(climber.transform.position, -normal);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                output = ray.GetPoint(distance);
                RaycastHit hitInfo;
                Ray downRay = new Ray(output + Vector3.up * 10, Vector3.down);
                if (Physics.Raycast(downRay, out hitInfo))
                {
                    output = hitInfo.point + Vector3.up * climber.Bounds.extents.y;
                }
            }
            return output;
        }

        public Curve CalculateCurve (Collider inComingClimber)
        {
            Curve curve = new Curve();
            Vector3 firstPoint = inComingClimber.transform.position;
            Vector3 lastPoint = jumpPoint.position;
            curve.Add(firstPoint);
            float timing = 0;
            AnimationCurve jump = CentralData.Instance.JumpData.JumpCurve;
            float min = jump.GetMin();
            float max = jump.GetMax();
            float first = jump.keys[0].time;
            float last = jump.keys[jump.length - 1].time;
            Vector3 flatDirection = (lastPoint - firstPoint);
            flatDirection.y = 0;
            float distance = flatDirection.magnitude;
            flatDirection.Normalize();
            while (timing < last)
            {
                timing += Time.deltaTime;
                float eval = jump.Evaluate(timing);
                float x = eval.Map(first, last, 0, distance);
                float y = timing.Map(min, max, firstPoint.y, lastPoint.y);
                Vector3 move = flatDirection * x;
                move.y = y;
                curve.Add(move);
            }
            curve.Add(lastPoint);
            return curve;
        }

        protected virtual void Awake ()
        {
            material = GetComponent<MeshRenderer>().material;
        }
    }
}
