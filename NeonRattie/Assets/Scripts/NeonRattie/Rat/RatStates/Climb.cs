using System.Collections.Generic;
using Flusk.Extensions;
using Flusk.Utility;
using NeonRattie.Controls;
using NeonRattie.Objects;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    //TODO: LOTS OF SIMILARITIES WITH JumpOff.cs
    public class Climb : RatState, IActionState
    {
        private readonly float negligibleDistance = 0.1f;
        
        private Curve curve;
        private CurveMotion<RatController> curveMotion;
        private float slerpTime;
        private JumpBox jumpBox;
        private Vector3 goal;
        private Vector3 flatGoal;
        private Vector3 direction;
        private float magnitude;
        private float boxHeight;
        private Vector3 initialPoint;

        private Vector3[] drawPositions;

        private readonly Queue<Vector3> arcPositions = new Queue<Vector3>(100);

        public override RatActionStates State
        {
            get { return RatActionStates.Climb;}
            protected set { }
        }

        public override void Enter (IState previousState )
        {
            Debug.Log("[CLIMB] Enter()");
            slerpTime = 0;
            base.Enter(previousState);
            rat.RatAnimator.PlayClimb();
            GetGroundData();
            if ( rat.JumpBox == null )
            {
                rat.StateMachine.ChangeState(RatActionStates.Idle);
                return;
            }
            CalculateClimbData();
            CalculatePositions();
            rat.AddDrawGizmos(DrawGizmos);
        }

        public override void Tick()
        {
            base.Tick();
            rat.TryMove(arcPositions.Dequeue());
            if (arcPositions.Count > 0)
            {
                return;
            }
            rat.ChangeState(RatActionStates.Idle);
        }

        public override void Exit(IState state)
        {
            base.Exit(state);
            rat.RemoveDrawGizmos(DrawGizmos);
        }

        private void DrawGizmos ()
        {
            int capacity = drawPositions.Length;
            for (int i = 0; i < capacity; i++)
            {
                Gizmos.DrawSphere(drawPositions[i], 0.1f);
            }
        }

        private void CalculateClimbData()
        {
            jumpBox = rat.JumpBox;
            goal = jumpBox.GetJumpPoint(rat);
            direction = (goal - rat.transform.position).normalized;
            
            direction.y = 0;
            var towards = (goal - rat.transform.position);
            boxHeight = towards.y;
            towards.y = 0;
            magnitude = towards.magnitude;
            initialPoint = rat.transform.position;
        }

        private Vector3 GetUpValue(float deltaTime)
        {
            Vector3 globalUp = Vector3.up;
            float ypoint = rat.ClimbUpCurve.Evaluate(deltaTime);
            return globalUp * ypoint * boxHeight;
        }

        private Vector3 GetForwardValue(float deltaTime)
        {
            float nextStage = rat.ForwardMotion.Evaluate(deltaTime);
            return direction * nextStage * magnitude;
        }

        private void CalculatePositions()
        {
            bool reachedTarget = false;
            while (!reachedTarget)
            {
                float maxtime = Mathf.Min(rat.ForwardMotion.GetFinalTime(), rat.ClimbUpCurve.GetFinalTime());
                var upValue = GetUpValue(slerpTime);
                var forwardValue = GetForwardValue(slerpTime);
                var nextPoint = initialPoint + (upValue + forwardValue);
                arcPositions.Enqueue(nextPoint);
                slerpTime += Time.deltaTime;
                var difference = Vector3.Distance(nextPoint, goal);
                reachedTarget = difference < negligibleDistance || (maxtime > 0 && slerpTime > maxtime);
            }
            arcPositions.Enqueue(goal);
            drawPositions = arcPositions.ToArray();
        }
    }
}
