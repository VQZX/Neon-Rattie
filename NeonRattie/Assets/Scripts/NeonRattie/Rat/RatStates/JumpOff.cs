using System.Collections.Generic;
using Flusk.Extensions;
using Flusk.Utility;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    //TODO: LOTS OF SIMILARITIES WITH Climb.cs
    public class JumpOff : RatState, IActionState
    {
       
        private const int RAT_LENGTHS_AHEAD = 3;
        private const float NEGLIGIBLE_DISTANCE = 0.0000001f;
        private Vector3 forwardPoint;
        private float distance;
        private float height;
        private Vector3 initialPoint;
        private Vector3 flatDirection;
        private Vector4 goal;
        
        public override RatActionStates State 
        { 
            get {return RatActionStates.JumpOff;}
            protected set { }
        }

        private readonly Queue<Vector3> arcPositions = new Queue<Vector3>(100);
        private Vector3[] drawPositions;
        
        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            float size = rat.RatCollider.bounds.extents.y;
            Vector3 frontPoint = rat.RatCollider.bounds.ClosestPoint(rat.transform.position + rat.LocalForward);
            Vector3 depth = frontPoint -
                            rat.RatCollider.bounds.ClosestPoint(rat.transform.position - rat.LocalForward);
            distance = depth.magnitude * RAT_LENGTHS_AHEAD;
            Vector3 hitPoint = frontPoint + distance * rat.LocalForward;
            Ray ray = new Ray(hitPoint, Vector3.down);
            RaycastHit hit;
            initialPoint = rat.transform.position;
            if (Physics.Raycast(ray, out hit))
            {
                forwardPoint = hit.point + Vector3.up * size;
                flatDirection = (forwardPoint - initialPoint).normalized;
                flatDirection.y = 0;
                height = Mathf.Abs(forwardPoint.y - initialPoint.y);
                goal = forwardPoint;
            }
            else
            {
                rat.StateMachine.ChangeState(previousState);
                return;
            }
            CalculatePositions();
            rat.AddDrawGizmos(DrawGizmos);
        }

        public override void Tick()
        {
            base.Tick();
            if (!rat.TryMove(arcPositions.Dequeue()))
            {
                rat.ChangeState(RatActionStates.Idle);
            }
            if (arcPositions.Count > 0 )
            {
                return;
            }
            rat.ChangeState(RatActionStates.Idle);
        }

        public override void Exit(IState state)
        {
            base.Exit(state);
            rat.NullifyJumpBox();
        }
        
        private void CalculatePositions()
        {
            var reachedTarget = false;
            var upCurve = rat.JumpOffCurve.VerticalMotion;
            var forwardCurve = rat.JumpOffCurve.ForwardMotion;
            var maxtime = Mathf.Min(forwardCurve.GetFinalTime(), upCurve.GetFinalTime());
            float slerpTime = 0;
            while (!reachedTarget)
            {
                var upValue = GetUpValue(slerpTime, upCurve, height);
                var forwardValue = GetForwardValue(slerpTime, forwardCurve, flatDirection, distance);
                var nextPoint = initialPoint + (upValue + forwardValue);
                arcPositions.Enqueue(nextPoint);
                slerpTime += Time.deltaTime;
                var difference = Vector3.Distance(nextPoint, forwardPoint);
                reachedTarget = difference < NEGLIGIBLE_DISTANCE || (maxtime > 0 && slerpTime > maxtime);
            }
            //arcPositions.Enqueue(goal);
            drawPositions = arcPositions.ToArray();
        }
        
        private void DrawGizmos ()
        {
            int capacity = drawPositions.Length;
            Gizmos.DrawSphere(goal, 0.3f);
            for (int i = 0; i < capacity; i++)
            {
                Gizmos.DrawSphere(drawPositions[i], 0.1f);
            }
        }
    }
}