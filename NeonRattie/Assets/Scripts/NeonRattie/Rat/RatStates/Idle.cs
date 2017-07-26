using Assets.Scripts.Flusk.Utility;
using Flusk.Management;
using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Idle : RatState
    {

        public bool hasMovedMouse = false;
        private const float RESET_TIME = 10;
        private Timer searchTime;

        public override RatActionStates State 
        { 
            get {return RatActionStates.Idle;}
            protected set { }
        }

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
            StartSearch();
            if (searchTime != null)
            {
                searchTime.Tick(Time.deltaTime);
            }
            
        }

        private void UndoSearch ()
        {
            searchTime = null;
            rat.RatAnimator.PlayIdle();
        }

        private void StartSearch()
        {
            if (searchTime != null)
            {
                return;
            }
            rat.RatAnimator.PlaySearchingIdle();
            searchTime = new Timer(RESET_TIME, UndoSearch);
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
