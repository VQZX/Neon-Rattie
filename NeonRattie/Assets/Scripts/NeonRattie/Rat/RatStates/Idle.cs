using Assets.Scripts.Flusk.Utility;
using Flusk.Controls;
using Flusk.Management;
using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Idle : RatState
    {

        public bool hasMovedMouse = false;
        private float resetTime = 10;
        private Timer searchTime;

        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayIdle();
            PlayerControls.Instance.Walk += OnWalkPressed;
            PlayerControls.Instance.Jump += OnJump;
        }

        public override void Tick()
        {
            base.Tick();
            FallTowards();
            RatRotate();
            
            var playerControls = PlayerControls.Instance;
            var keyboardControls = KeyboardControls.Instance;

            if (playerControls.CheckKey(playerControls.Forward))
            {
                rat.ChangeState(RatActionStates.Walk);
                Debug.Log("[IDLE] Change To walk");
                return;
            }
            
            if (MouseManager.Instance == null)
            {
                return;
            }
            if (!(MouseManager.Instance.Delta.magnitude > 0))
            {
                return;
            }
            rat.RatAnimator.PlaySearchingIdle();
            searchTime = new Timer(resetTime, UndoSearch);
        }

        private void UndoSearch ()
        {
            searchTime = null;
            rat.RatAnimator.PlayIdle();
        }

        public override void Exit(IState previousState)
        {
            PlayerControls.Instance.Walk -= OnWalkPressed;
            PlayerControls.Instance.Jump -= OnJump;

        }

        private void OnWalkPressed(float axisValue)
        {
            if (StateMachine != null)
            {
                StateMachine.ChangeState(RatActionStates.Walk);
            }
        }
    }
}
