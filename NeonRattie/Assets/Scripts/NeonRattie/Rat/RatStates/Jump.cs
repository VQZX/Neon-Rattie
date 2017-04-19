using Flusk.Utility;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Jump : RatState
    {
        private float stateTime = 0;

        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayJump();
            stateTime = 0;
        }

        public override void Tick()
        {
            base.Tick();
            
            JumpCalculation();
            int length = rat.JumpAnimationCurve.length;
            if (rat.JumpAnimationCurve[length - 1].time <= stateTime)
            {
                StateMachine.ChangeState(RatActionStates.Idle);
            }
        }

        private void JumpCalculation()
        {
            Vector3 force = rat.Gravity;
            float jumpMultiplier = rat.JumpAnimationCurve.Evaluate(stateTime);
            force += (rat.JumpForce * rat.Gravity.normalized * jumpMultiplier);
            rat.transform.position += force;
            stateTime += Time.deltaTime;
        }
    }
}
