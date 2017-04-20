using Flusk.Utility;
using NeonRattie.Controls;

namespace NeonRattie.Rat.RatStates
{
    public class WalkBack : RatState
    {
        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayWalk();
            (PlayerControls.Instance as PlayerControls).UnReverse += UnReverse;
        }

        public void Tick()
        {
            base.Tick();
            rat.WalkBackward();
        }

        public override void Exit(IState state)
        {
            base.Exit(state);
            rat.RatAnimator.ExitWalk();
            (PlayerControls.Instance as PlayerControls).UnReverse -= UnReverse;
        }

        private void UnReverse(float x)
        {
            StateMachine.ChangeState(RatActionStates.Idle);
        }
    }
}
