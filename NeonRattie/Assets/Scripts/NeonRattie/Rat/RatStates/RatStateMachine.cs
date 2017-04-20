using Flusk.Utility;

//aliases
using RatBrain = NeonRattie.Rat.RatController;

namespace NeonRattie.Rat.RatStates
{
    public class RatStateMachine : KeyStateMachine<RatActionStates, RatState>
    {
        protected RatBrain ratBrain;

        //states

        public void Init(RatBrain rat)
        {
            ratBrain = rat;
        }
    }
}
