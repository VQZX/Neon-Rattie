using System;
using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Walk : RatState, IActionState
    {
        public override RatActionStates State 
        { 
            get {return RatActionStates.Walk;}
            protected set { }
        }
        
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
            if (Math.Abs(rat.WalkDirection.magnitude) < 0.001f)
            {
                rat.ChangeState(RatActionStates.Idle);
            }
            rat.Walk(rat.WalkDirection);
            rat.RotateController.SetLookDirection(rat.WalkDirection, Vector3.up, 0.9f);
            FallTowards();
            if (rat.ClimbValid())
            {
                rat.ChangeState(RatActionStates.Climb);
                return;
            }
            if (rat.JumpOffValid())
            {
                rat.ChangeState(RatActionStates.JumpOff);
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
