using Flusk.Utility;
using UnityEngine;

namespace NeonRattie.Rat
{
    public class RatController : MonoBehaviour
    {
        private StateMachine<RatStates> stateMachine = new StateMachine<RatStates>();

        //states

        private void Init()
        {
            //initialise state machine
        }
    }
}
