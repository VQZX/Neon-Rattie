using NeonRattie.Rat.RateStates;
using UnityEngine;

namespace NeonRattie.Rat
{
    [RequireComponent( typeof(RatAnimator))]
    public class RatController : MonoBehaviour
    {
        public RatAnimator RatAnimator { get; protected set; }

        //other rat effects...


        private RatStateMachine ratStateMachine = new RatStateMachine();

        //states
        
        private void Init()
        {
            //initialise state machine
        }
    }
}
