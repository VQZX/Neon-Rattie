using Assets.Scripts.Flusk.Utility;
using Flusk.Management;
using Flusk.Utility;
using NeonRattie.Controls;

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
            PlayerControls.Instance.Reverse += OnReversePressed;
        }

        public override void Tick()
        {
            base.Tick();
            rat.TankControls();
            var pc = (PlayerControls.Instance as PlayerControls);
            if (pc != null)
            {
                if (pc.CheckKeyDown(pc.JumpUp))
                {
                    if (rat.ClimbValid())
                    {
                        StateMachine.ChangeState(RatActionStates.Climb);
                        return;
                    }
                    StateMachine.ChangeState(RatActionStates.Jump);
                    return;
                }
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
            PlayerControls.Instance.Reverse -= OnReversePressed;

        }

        private void OnWalkPressed(float axisValue)
        {
            if (StateMachine != null)
            {
                StateMachine.ChangeState(RatActionStates.Walk);
            }
        }

        private void OnReversePressed(float axis)
        {
            StateMachine.ChangeState(RatActionStates.Reverse);
        }
    }
}
