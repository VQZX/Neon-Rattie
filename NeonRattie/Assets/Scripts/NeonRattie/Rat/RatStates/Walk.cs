using Flusk.Utility;
using NeonRattie.Controls;

namespace NeonRattie.Rat.RatStates
{
    public class Walk : RatState
    {
        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayWalk();
        }

        public override void Tick()
        {
            base.Tick();
            PlayerControls pc = (PlayerControls.Instance as PlayerControls);
            if (pc.CheckKeyUp(pc.Forward))
            {
                StateMachine.ChangeState(RatActionStates.Idle);
                return;
            }
            if (pc.CheckKey(pc.JumpUp))
            {
                if (rat.ClimbValid())
                {
                    StateMachine.ChangeState(RatActionStates.Climb);
                    return;
                }
                StateMachine.ChangeState(RatActionStates.Jump);
                return;
            }
            //otherwise
            rat.WalkForward();
        }

        public override void Exit(IState state)
        {
            base.Exit(state);
            rat.RatAnimator.ExitWalk();
        }
    }
}
