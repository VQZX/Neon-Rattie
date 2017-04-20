using System.Collections.Generic;
using UnityEngine;

namespace Flusk.Utility
{
    public class KeyStateMachine<TKey, TState> : StateMachine<TState> where TState : IState
    {
        public Dictionary<TKey, TState> keyStates;

        public KeyStateMachine() : base()
        {
            keyStates = new Dictionary<TKey, TState>();
            keyStates.Clear();
        }
        
        public virtual void AddState(TKey key, TState state)
        {
            keyStates.Add(key, state);
        }

        public virtual void ChangeState(TKey key)
        {
            TState state = keyStates[key];
            base.ChangeState(state);
        }
    }
}
