﻿using System;
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
            Debug.Log("Play Idle");  
        }

        public void PlayWalk()
        {
            //probably set bool on animator here
            Debug.Log("Play walk");
        }

        public void ExitWalk()
        {
            //probably reset bool on animator here
            Debug.Log("Exit Walk");
        }

        public void PlayJump()
        {
            Debug.Log("Play Jump");
        }

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
        }
    }
}