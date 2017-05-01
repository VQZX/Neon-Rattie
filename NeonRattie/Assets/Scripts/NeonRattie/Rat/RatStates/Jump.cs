using Flusk.Utility;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Jump : RatState, IActionState
    {
        private float stateTime = 0;
        private Vector3 groundPosition;

        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayJump();
            stateTime = 0;
            groundPosition = rat.transform.position;
        }

        public override void Tick()
        {
            base.Tick();
            
            JumpCalculation();
            int length = rat.JumpAnimationCurve.length;
            bool passed = rat.JumpAnimationCurve[length - 1].time <= stateTime;
            if ( passed || rat.IsGrounded() )
            {
                StateMachine.ChangeState(RatActionStates.Idle);
            }
        }

        private void JumpCalculation()
        {
            Vector3 force = rat.Gravity;
            float jumpMultiplier = rat.JumpAnimationCurve.Evaluate(stateTime);
            force += (rat.JumpForce * -rat.Gravity.normalized * jumpMultiplier);
            rat.transform.position = groundPosition + force;
            stateTime += Time.deltaTime;
        }
    }
}
