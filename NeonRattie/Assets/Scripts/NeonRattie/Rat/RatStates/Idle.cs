using Flusk.Utility;
using NeonRattie.Controls;

namespace NeonRattie.Rat.RatStates
{
    public class Idle : RatState
    {
        

        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayIdle();
            PlayerControls.Instance.Walk += OnWalkPressed;
        }

        public override void Exit(IState previousState)
        {
            PlayerControls.Instance.Walk -= OnWalkPressed;
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
