using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NeonRattie.Rat
{

    /// <summary>
    /// Class specifically for bridging to rat animator
    /// helps for update floats, string etc
    /// as well as playing animations through triggers, bools
    /// and directly through names
    /// </summary>
    [RequireComponent(typeof (Animator))]
    public class RatAnimator : MonoBehaviour
    {
        public Animator Animator { get; private set; }

        public void PlayIdle()
        {
            
        }

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
        }
    }
}
