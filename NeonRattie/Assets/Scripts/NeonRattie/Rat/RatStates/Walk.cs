using Flusk.Management;
using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

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
            rat.TankControls();
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

            if (MouseManager.Instance == null)
            {
                return;;
            }
            var rotationDelta = MouseManager.Instance.Delta;
            Debug.Log(rotationDelta);
            if (rotationDelta.magnitude == 0)
            {
                return;
            }
            var euler = new Vector3(-rotationDelta.y, rotationDelta.x);
            var angle = Mathf.Atan2(euler.y, euler.x);
            rat.RotateRat(angle);
        }

        private void OnUnWalk(float x)
        {
            StateMachine.ChangeState(RatActionStates.Idle);
        }
    }
}
