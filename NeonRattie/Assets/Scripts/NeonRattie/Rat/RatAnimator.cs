using UnityEngine;

namespace NeonRattie.Rat
{

    /// <summary>
    /// Class specifically for bridging to rat animator
    /// helps for update floats, string etc
    /// as well as playing animations through triggers, bools
    /// and directly through names
    /// </summary>
    public class RatAnimator : MonoBehaviour
    {
        [SerializeField] protected Animator animator;

        public Animator Animator
        {
            get { return animator; }
        }

        public void PlayIdle()
        {
            //Debug.Log("Play Idle");  
        }

        public void PlaySearchingIdle ()
        {
            //Debug.Log("Play Searching Idle");
        }

        public void PlayWalk()
        {
            //probably set bool on animator here
            //Debug.Log("Play Walk");
        }

        public void ExitWalk()
        {
            //probably reset bool on animator here
            //Debug.Log("Exit Walk");
        }

        public void PlayJump()
        {
            //Debug.Log("Play Jump");
        }

        public void PlayClimb ()
        {
            //Debug.Log("Play Climb");
        } 

        public void PlayReverse()
        {
            //Debug.Log("Reverse");
        }
    }
}
