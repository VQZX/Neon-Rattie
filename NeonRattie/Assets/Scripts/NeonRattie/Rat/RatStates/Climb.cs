using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Climb : RatState, IActionState
    {
        private Curve curve;
        private CurveMotion<RatController> curveMotion;

        public override void Enter (IState previousState )
        {
            Debug.Log("[CLIMB] Enter()");
            base.Enter(previousState);
            rat.RatAnimator.PlayClimb();
            GetGroundData();
            if ( rat.JumpBox == null )
            {
                rat.StateMachine.ChangeState(RatActionStates.Jump);
                return;
            }
            curve = rat.JumpBox.CalculateCurve( rat.GetComponent<Collider>() );
            curveMotion = new CurveMotion<RatController>(curve);
        }

        public override void Tick ()
        {
            base.Tick();
            curveMotion.Tick(rat);
        }

        public override void Exit(IState nextState)
        {
            base.Exit(nextState);
        }
    }
}
