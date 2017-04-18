using System.Collections.Generic;

namespace Flusk.Utility
{
    public class KeyStateMachine<TKey, TState> : StateMachine<TState> where TState : IState
    {
        public Dictionary<TKey, TState> keyStates;
        
        public virtual void AddState(TKey key, TState state)
        {
            keyStates.Add(key, state);
        }
    }
}
