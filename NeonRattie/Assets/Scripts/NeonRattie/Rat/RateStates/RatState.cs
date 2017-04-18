using Flusk.Utility;
using RatBrain = NeonRattie.Rat.RatController;

namespace NeonRattie.Rat.RateStates
{
    public class RatState : IState
    {
        protected RatBrain rat;

        public void Init(RatBrain rat)
        {
            this.rat = rat;
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
    }
}
