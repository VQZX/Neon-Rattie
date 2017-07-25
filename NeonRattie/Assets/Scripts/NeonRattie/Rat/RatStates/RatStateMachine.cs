using System;
using Flusk.Utility;

//aliases
using RatBrain = NeonRattie.Rat.RatController;

namespace NeonRattie.Rat.RatStates
{
    public class RatStateMachine : KeyStateMachine<RatActionStates, RatState>
    {
        protected RatBrain ratBrain;

        /// <summary>
        /// An event for when the rat state changes
        /// </summary>
        public Action<RatActionStates, RatActionStates> stateChanged;

        public void Init(RatBrain rat)
        {
            ratBrain = rat;
        }

        public override void ChangeState(IState state)
        {
            var previousState = ((RatState) CurrentState).State;
            var nextState = ((RatState) state).State;
            base.ChangeState(state);
            if (stateChanged != null)
            {
                stateChanged(previousState, nextState);
            }
        }
    }
}
