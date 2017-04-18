using Flusk.Utility;

//aliases
using RatBrain = NeonRattie.Rat.RatController;

namespace NeonRattie.Rat.RateStates
{
    public class RatStateMachine : StateMachine<RatState>
    {
        protected RatBrain ratBrain;

        //states

        public void Init(RatBrain rat)
        {
            ratBrain = rat;
            AssignStates();
        }

        private void AssignStates()
        {
            if (ratBrain == null)
            {
                return;
            }
            int length = states.Count;
            for (int i = 0; i < length; i++)
            {
                var current = states[i];
                current.Init(ratBrain);
            }
        }
    }
}
