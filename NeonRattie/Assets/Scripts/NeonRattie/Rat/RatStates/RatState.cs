﻿using System;
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
            Vector2 delta = MouseManager.Instance.Delta;
            float deltaX = delta.x;
            Vector3 axis = Vector3.up;
            if (deltaX > 0)
            {
                deltaX = -deltaX;
                axis = Vector3.down;
            }
            float angle = Mathf.Atan2(-delta.y, deltaX);
            rat.RotateRat(angle, axis);
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
        }

        protected void FallDown ()
        {
            bool fallTowards = rat.TryMove(rat.LowestPoint - Vector3.down * 0.1f);
        }
        
        protected Vector3 GetUpValue(float deltaTime, AnimationCurve curve, float height)
        {
            Vector3 globalUp = Vector3.up;
            float ypoint = curve.Evaluate(deltaTime);
            return globalUp * ypoint * height;
        }

        protected Vector3 GetForwardValue(float deltaTime, AnimationCurve curve, Vector3 direction, float distance)
        {
            float nextStage = curve.Evaluate(deltaTime);
            return direction * nextStage * distance;
        }
    }
}
