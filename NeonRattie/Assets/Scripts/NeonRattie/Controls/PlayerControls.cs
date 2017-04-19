using System;
using System.ComponentModel;
using Flusk.Controls;
using Flusk.Patterns;
using UnityEngine;

namespace NeonRattie.Controls
{
    //a a class for reading in input data and feeding to the avatar
    //mostly relays keypresses to certain actions
    public class PlayerControls : Singleton<PlayerControls>
    {
        [SerializeField] protected KeyCode walkKey;
        [SerializeField] protected KeyCode runKey;
        [SerializeField] protected KeyCode jumpKey;

        [SerializeField] protected KeyCode pauseKey;
        [SerializeField] protected KeyCode exitKey;

        [Header("Shooosh, only later")]
        [SerializeField] protected KeyCode screenShotKey;

        //some quick usages
        protected KeyCode forward = KeyCode.W;
        public KeyCode Forward { get; protected set; }

        protected KeyCode jumpUp = KeyCode.Space;
        public KeyCode JumpUp { get; protected set; }

        //float value to communicate any access amount
        //for "pressurised" speed
        public event Action<float> Walk;
        public event Action<float> Run;
        public event Action<float> Jump;

        public event Action Pause;
        public event Action Exit;

        public bool CheckKeyDown(KeyCode code)
        {
            return Input.GetKeyDown(code);
        }

        public bool CheckKey(KeyCode code)
        {
            return Input.GetKey(code);          
        }

        public bool CheckKeyUp(KeyCode code)
        {
            return Input.GetKeyUp(code);
        }

        protected virtual void Start ()
        {
            var kc = KeyboardControls.Instance;
            if (kc == null)
            {
                return;
            }
            kc.KeyHit += InvokeWalk;
            kc.KeyHit += InvokeRun;
            kc.KeyHit += InvokeJump;
        }

        protected virtual void OnDisable()
        {
            var kc = KeyboardControls.Instance;
            if (kc == null)
            {
                return;
            }
            kc.KeyHit -= InvokeWalk;
            kc.KeyHit -= InvokeRun;
            kc.KeyHit -= InvokeJump;
        }

        private void Invoke(Action<float> action, float value)
        {
            if (action != null)
            {
                action(value);
            }
        }

        private void Invoke(Action action)
        {
            if (action != null)
            {
                action();
            }
        }

        private void InvokeWalk(KeyData data)
        {
            if (walkKey != data.Code)
            {
                return;
            }
            Invoke(Walk, data.AxisValue);
        }

        private void InvokeRun(KeyData data)
        {
            if (runKey != data.Code)
            {
                return;
            }
            Invoke(Run, data.AxisValue);
        }

        private void InvokeJump(KeyData data)
        {
            if (jumpKey != data.Code)
            {
                return;
            }
            Invoke(Jump, data.AxisValue);
        }

        private void InvokePause(KeyData data)
        {
            if (pauseKey != data.Code)
            {
                return;
            }
            Invoke(Pause);
        }

        private void InvokeExit(KeyData data)
        {
            if (exitKey != data.Code)
            {
                return;
            }
            Invoke(Exit);
        }
    }
}
