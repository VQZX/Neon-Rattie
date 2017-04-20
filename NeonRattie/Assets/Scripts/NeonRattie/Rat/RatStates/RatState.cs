using Flusk.Utility;
using RatBrain = NeonRattie.Rat.RatController;

namespace NeonRattie.Rat.RatStates
{
    public class RatState : IState
    {
        public RatStateMachine StateMachine { get; set; }

        protected RatBrain rat;

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
    }
}
