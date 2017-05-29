using Flusk.Management;
using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Walk : RatState, IActionState
    {
        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayWalk();
            PlayerControls.Instance.Unwalk += OnUnWalk;
            PlayerControls.Instance.Jump += OnJump;
        }

        public override void Tick()
        {
            base.Tick();
            rat.TankControls();
            rat.WalkForward();
            RatRotate();
            if (rat.ClimbValid())
            {
                rat.StateMachine.ChangeState(RatActionStates.Climb);
            }
        }

        public override void Exit (IState nextState)
        {
            PlayerControls.Instance.Unwalk -= OnUnWalk;
            PlayerControls.Instance.Jump -= OnJump;
        }

        private void OnUnWalk(float x)
        {
            StateMachine.ChangeState(RatActionStates.Idle);
        }
    }
}
