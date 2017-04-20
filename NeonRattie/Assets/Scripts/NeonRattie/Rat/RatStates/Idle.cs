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
            (PlayerControls.Instance as PlayerControls).Walk += OnWalkPressed;
        }

        public override void Tick()
        {
            base.Tick();
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
                if (pc.CheckKey(pc.Forward))
                {
                    
                }
            }
        }

        public override void Exit(IState previousState)
        {
            (PlayerControls.Instance as PlayerControls).Walk -= OnWalkPressed;
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
