using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class WalkBack : RatState, IActionState
    {
        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayReverse();
        }

        public override void Tick()
        {
            base.Tick();
            rat.TankControls();
            rat.WalkBackward();
            RatRotate();
            FallTowards();
        }

        public override void Exit(IState state)
        {
            base.Exit(state);
            rat.RatAnimator.ExitWalk();
        }

        private void UnReverse(float x)
        {
            StateMachine.ChangeState(RatActionStates.Idle);
        }
    }
}
