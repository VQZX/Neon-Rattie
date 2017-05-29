using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Climb : RatState, IActionState
    {
        private Curve curve;
        private CurveMotion<RatController> curveMotion;

        private float maxRayLength = 10;
        private float slerpTime = 0;

        public override void Enter (IState previousState )
        {
            Debug.Log("[CLIMB] Enter()");
            base.Enter(previousState);
            rat.RatAnimator.PlayClimb();
            GetGroundData();
            if ( rat.JumpBox == null )
            {
                rat.StateMachine.ChangeState(RatActionStates.Idle);
                return;
            }
            PlayerControls.Instance.Unwalk += OnUnWalk;
            rat.AddDrawGizmos(DrawGizmos);
        }

        public override void Tick ()
        {
            base.Tick();
            Orientate();
            rat.WalkForward(rat.LocalForward);
            FallTowards();
        }

        public override void Exit(IState nextState)
        {
            base.Exit(nextState);
            //rat.RemoveDrawGizmos(DrawGizmos);
            PlayerControls.Instance.Unwalk -= OnUnWalk;
            
        }

        private void OnUnWalk(float x)
        {
            StateMachine.ChangeState(RatActionStates.Idle);
        }

        private void Orientate ()
        {
            RaycastHit forward;
            RaycastHit down;
            bool hasForward = Physics.Raycast(rat.transform.position, rat.LocalForward, out forward, maxRayLength );
            bool hasDown = Physics.Raycast(rat.transform.position, -rat.transform.up, out down, maxRayLength);
            Quaternion current = rat.transform.rotation;
            Quaternion next = current;
            Debug.LogFormat("hasForward: {0} -- hasDown: {1}", hasForward, hasDown);
            if ( hasForward && hasDown )
            {
                Vector3 normal = (forward.normal + down.normal + Vector3.down) / 3;
                next = rat.OrientateByGroundNormal(-normal);
            }
            else if ( hasDown )
            {
                next = rat.OrientateByGroundNormal(-down.normal);
            }
            else
            {
                rat.StateMachine.ChangeState(RatActionStates.Idle);
                return;
            }

            rat.transform.rotation = next;
        }
        

        private void DrawGizmos ()
        {
            Gizmos.DrawLine(rat.transform.position, rat.LocalForward * 10);
            Gizmos.DrawLine(rat.transform.position, -rat.transform.up * 10);
        }
    }
}
