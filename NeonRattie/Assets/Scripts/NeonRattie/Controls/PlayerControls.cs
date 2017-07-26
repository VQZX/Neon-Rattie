using System;
using Flusk.Controls;
using Flusk.Patterns;
using JetBrains.Annotations;
using UnityEngine;

namespace NeonRattie.Controls
{
    //a a class for reading in input data and feeding to the avatar
    //mostly relays keypresses to certain actions
    public class PlayerControls : PersistentSingleton<PlayerControls>
    {
        [SerializeField] protected KeyCode [] walkKey;
        [SerializeField] protected KeyCode reverseKey = KeyCode.S;
        [SerializeField] protected KeyCode runKey;
        [SerializeField] protected KeyCode jumpKey;

        [SerializeField] protected KeyCode pauseKey;
        [SerializeField] protected KeyCode exitKey;

        [Header("Shooosh, only later")]
        [SerializeField] protected KeyCode screenShotKey;

        //some quick usages
        [SerializeField]
        protected KeyCode forward = KeyCode.W;
        public KeyCode Forward { get { return forward; }}

        [SerializeField]
        protected KeyCode back = KeyCode.S;
        public KeyCode Back { get { return back; } }
        
        [SerializeField]
        protected KeyCode left = KeyCode.A;
        public KeyCode Left {get { return left; }}

        [SerializeField] protected KeyCode right = KeyCode.D;
        public KeyCode Right {get { return right; }}

        protected KeyCode jumpUp = KeyCode.Space;
        public KeyCode JumpUp { get { return jumpUp; } }

        //float value to communicate any access amount
        //for "pressurised" speed
        public event Action<float> Walk;
        public event Action<float> Run;
        public event Action<float> Jump;
        public event Action<float> Unwalk;

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
            bool up = Input.GetKeyUp(code);
            Debug.LogFormat("Code: {0} Upstate: {1}", code, up);
            return up;
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
            kc.KeyHit += InvokeUnWalk;   
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
            kc.KeyHit -= InvokeUnWalk;
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
            if (!Contains(data.Code, walkKey) || data.State == KeyState.Up)
            {
                return;
            }
            Invoke(Walk, data.AxisValue);
        }

        private void InvokeUnWalk(KeyData data)
        {
            if (!Contains(data.Code, walkKey) || data.State != KeyState.Up)
            {
                return;
            }
            Invoke(Unwalk, data.AxisValue);
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

        [UsedImplicitly]
        private void InvokePause(KeyData data)
        {
            if (pauseKey != data.Code)
            {
                return;
            }
            Invoke(Pause);
        }

        [UsedImplicitly]
        private void InvokeExit(KeyData data)
        {
            if (exitKey != data.Code)
            {
                return;
            }
            Invoke(Exit);
        }
        

        private static bool Contains(KeyCode code, KeyCode[] codes)
        {
            int length = codes.Length;
            for (int i = 0; i < length; i++)
            {
                if (codes[i] == code)
                {
                    return true;
                }      
            }
            return false;
        }
    }
}
