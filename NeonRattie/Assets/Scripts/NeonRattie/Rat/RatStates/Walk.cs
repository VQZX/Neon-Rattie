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
            (PlayerControls.Instance as PlayerControls).Unwalk += OnUnWalk;
        }

        public override void Tick()
        {
            base.Tick();
            PlayerControls pc = (PlayerControls.Instance as PlayerControls);
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

        private void OnUnWalk(float x)
        {
            StateMachine.ChangeState(RatActionStates.Idle);
        }
    }
}
