using Flusk.Utility;

namespace NeonRattie.Rat.RatStates
{
    public class Climb : RatState, IActionState
    {
        public override void Enter (IState previousState )
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayClimb();
            GetGroundData();
            if ( rat.JumpBox == null )
            {
                rat.StateMachine.ChangeState(RatActionStates.Jump);
            }
        }

        public override void Tick ()
        {
            base.Tick();
        }

        public override void Exit(IState nextState)
        {
            base.Exit(nextState);
        }
    }
}
