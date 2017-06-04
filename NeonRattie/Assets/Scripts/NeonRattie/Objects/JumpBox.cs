using Assets.Scripts.Flusk.DataHelp;
using NeonRattie.Controls;
using NeonRattie.Shared;
using UnityEngine;

namespace NeonRattie.Objects
{
    [RequireComponent(typeof(Rigidbody))]
    public class JumpBox : NeonRattieBehaviour
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

        public Curve CalculateCurve (Collider collider)
        {
            Curve curve = new Curve();
            Vector3 firstPoint = collider.transform.position;
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

        public override void Destroy()
        {
            throw new System.NotImplementedException();
        }

        public override void Initialise()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void Awake ()
        {
            material = GetComponent<MeshRenderer>().material;
        }
    }
}
