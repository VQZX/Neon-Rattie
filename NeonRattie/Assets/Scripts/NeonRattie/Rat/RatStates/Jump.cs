using Flusk.Utility;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Jump : RatState, IActionState
    {
        private float stateTime;

        public override RatActionStates State 
        { 
            get {return RatActionStates.Jump;}
            protected set { }
        }

        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayJump();
            stateTime = 0;
            GetGroundData();
        }

        public override void Tick()
        {
            base.Tick();
            JumpCalculation();
            rat.Walk(rat.WalkDirection);
            stateTime += Time.deltaTime;
            int length = rat.JumpArc.length;
            bool passed = rat.JumpArc[length - 1].time <= stateTime;
            if ( passed )
            {
                StateMachine.ChangeState(RatActionStates.Idle);
            }
        }

        private void JumpCalculation()
        {
            float jumpMultiplier = rat.JumpArc.Evaluate(stateTime);
            Vector3 force = (rat.JumpForce * -rat.Gravity.normalized * jumpMultiplier);
            rat.TryMove(rat.GetGroundData().point + force);
        }
    }
}
