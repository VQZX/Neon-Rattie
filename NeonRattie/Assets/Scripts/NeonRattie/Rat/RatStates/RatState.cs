using System;
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
        protected Vector3 groundPosition;

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

        protected void OnJump(float x)
        {
            StateMachine.ChangeState(RatActionStates.Jump);
        }

        protected void GetGroundData ()
        {
            var ground = rat.GetGroundData().transform;
            if (ground == null)
            {
                //TODO: immediately change to another state
                groundPosition = rat.transform.position;
            }
            else
            {
                groundPosition = ground.position;
            }
        }

        protected void FallTowards (Vector3 point)
        {
            rat.TryMove(point);
        }

        protected void FallTowards()
        {
            Vector3 point = rat.transform.position - rat.transform.up;
            bool fallTowards = rat.TryMove(point);
            Debug.LogFormat("FallTowards: {0}", fallTowards);
        }

        protected void FallDown ()
        {
            bool fallTowards = rat.TryMove(rat.LowestPoint - Vector3.down * 0.1f);
            Debug.LogFormat("FallTowards: {0}", fallTowards);
        }
    }
}
