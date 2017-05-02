using Flusk.Management;
using Flusk.Utility;
using UnityEngine;
using RatBrain = NeonRattie.Rat.RatController;

namespace NeonRattie.Rat.RatStates
{
    public class RatState : IState
    {
        public RatStateMachine StateMachine { get; set; }

        protected RatBrain rat;

        public void Init(RatBrain rat, RatStateMachine machine)
        {
            this.rat = rat;
            StateMachine = machine;
        }

        public virtual void Enter(IState previousState)
        {
            
        }

        public virtual void Exit(IState nextState)
        {
            
        }

        public virtual void Tick()
        {
            
        }

        public virtual void Tick(RatStateMachine stateMachine)
        {
            
        }

        protected void RatRotate()
        {
            if (MouseManager.Instance == null)
            {
                return;
            }
            var rotationDelta = MouseManager.Instance.Delta;
            if (rotationDelta.magnitude == 0)
            {
                return;
            }
            //TODO: allow for player configuration
            Vector3 euler;
            float angle;
            MouseManager.Instance.GetMotionData(out euler, out angle);
            rat.RotateRat(angle);
        }
    }
}
