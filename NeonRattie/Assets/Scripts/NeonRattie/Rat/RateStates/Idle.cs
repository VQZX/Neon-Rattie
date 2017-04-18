using Flusk.Utility;

namespace NeonRattie.Rat.RateStates
{
    public class Idle : RatState
    {
        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayIdle();
        }


    }
}
