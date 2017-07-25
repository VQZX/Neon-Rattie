using System;
using NeonRattie.Rat.RatStates;
using UnityEngine;
using UnityEngine.Events;

namespace NeonRattie.Rat
{
    public class RatEvent : MonoBehaviour
    {

        [SerializeField]
        protected UnityEvent idle;
        
        [SerializeField]
        protected UnityEvent walk;
        
        [SerializeField]
        protected UnityEvent run;
        
        [SerializeField]
        protected UnityEvent jump;
        
        [SerializeField]
        protected UnityEvent jumpOff;
        
        [SerializeField]
        protected UnityEvent climb;
        

        private RatController controller;

        protected virtual void Awake()
        {
            controller = GetComponent<RatController>();
            controller.StateMachine.stateChanged += OnStateChanged;
        }

        public void PlayIdle()
        {
            idle.Invoke();
        }

        public void PlayWalk()
        {
            walk.Invoke();
        }

        public void PlayRun()
        {
            run.Invoke();
        }

        public void PlayJump()
        {
            jump.Invoke();
        }

        public void PlayClimb()
        {
            climb.Invoke();
        }

        public void PlayJumpOff()
        {
            jumpOff.Invoke();
        }
        

        private void OnStateChanged(RatActionStates previous, RatActionStates current)
        {
            switch (current)
            {
                case RatActionStates.Idle:
                    PlayIdle();
                    break;
                case RatActionStates.Walk:
                    PlayWalk();
                    break;
                case RatActionStates.Run:
                    PlayRun();
                    break;
                case RatActionStates.Jump:
                    PlayJump();
                    break;
                case RatActionStates.Climb:
                    PlayClimb();
                    break;
                case RatActionStates.JumpOff:
                    PlayJumpOff();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("current", current, null);
            }
        }
    }
}