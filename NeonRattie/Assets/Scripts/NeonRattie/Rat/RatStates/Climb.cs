using Flusk.Utility;
using NeonRattie.Controls;
using NeonRattie.Objects;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Climb : RatState, IActionState
    {
        private Curve curve;
        private CurveMotion<RatController> curveMotion;

        private float maxRayLength = 10;
        private float slerpTime = 0;
        private JumpBox jumpBox;

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
            jumpBox = rat.JumpBox;
            rat.AddDrawGizmos(DrawGizmos);
        }

        public override void Tick()
        {
            base.Tick();
            RaycastHit forward, down;
            Orientate(out forward, out down);
            rat.WalkForward(rat.LocalForward);
            FallTowards();
            float angle = Vector3.Angle(down.normal, Vector3.up);
            angle = angle > 180 ? (360 - angle) : angle;
            if (CanChangeToIdle(down) )
            {
                rat.ChangeState(RatActionStates.Idle);
            }
        }

        public override void Exit(IState nextState)
        {
            base.Exit(nextState);
        }

        private bool CanChangeToIdle (RaycastHit hit)
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            angle = angle > 180 ? (360 - angle) : angle;
            if ( hit.collider != null )
            {
                return angle < 10 && hit.collider.gameObject == jumpBox.gameObject;
            }
            else
            {
                return false;
            }   
        }

        private void OnUnWalk(float x)
        {
            StateMachine.ChangeState(RatActionStates.Idle);
        }

        private void Orientate (out RaycastHit forward, out RaycastHit down)
        {
            bool hasForward = Physics.Raycast(rat.transform.position, rat.LocalForward, out forward, maxRayLength );
            bool hasDown = Physics.Raycast(rat.transform.position, -rat.transform.up, out down, maxRayLength);
            Quaternion current = rat.transform.rotation;
            Quaternion next = current;
            Debug.LogFormat("hasForward: {0} -- hasDown: {1}", hasForward, hasDown);
            if ( hasForward && hasDown )
            {
                Vector3 normal = (forward.normal + down.normal + Vector3.down);
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
