using System;
using System.Collections.Generic;
using NeonRattie.Rat;
using NeonRattie.Rat.RatStates;
using UnityEngine;

namespace NeonRattie.Audio
{
    public class RatAudio : MonoBehaviour
    {
        [SerializeField]
        protected RatController controller;
        
        /// <summary>
        /// List of audio sources attached to this object
        /// </summary>
        [SerializeField]
        protected List<AudioSource> audioSources;


        public void Play(AudioClip clip)
        {
            var source = AudioHelper.FindAvailableAudioSource(audioSources);
            source.clip = clip;
            source.Play();
        }

        public void Play(AudioClip clip, bool loop)
        {
            var source = AudioHelper.FindAvailableAudioSource(audioSources);
            source.clip = clip;
            source.loop = loop;
            source.Play();
        }

        public void CancelNonLoopingAudio()
        {
            var nonLooping = audioSources.FindAll(a => a.loop );
            foreach (var audioSource in nonLooping)
            {
                audioSource.Stop();
            }
        }

        public void CancelAllAudio()
        {
            foreach (var source in audioSources)
            {
                source.Stop();
            }
        }

        protected virtual void Awake ()
        {
            controller.StateMachine.stateChanged += OnStateChanged;
        }

        public void PlayIdle()
        {
            throw new NotImplementedException();
        }

        public void PlayWalk()
        {
            
        }

        public void PlayRun()
        {
            
        }

        public void PlayJump()
        {
            
        }

        public void PlayClimp()
        {
            
        }

        public void PlayJumpOff()
        {
            
        }
        

        private void OnStateChanged(RatActionStates previous, RatActionStates current)
        {
            switch (current)
            {
                case RatActionStates.Idle:
                    PlayIdle();
                    break;
                case RatActionStates.Walk:
                    PlayWalk();
                    break;
                case RatActionStates.Run:
                    PlayRun();
                    break;
                case RatActionStates.Jump:
                    PlayJump();
                    break;
                case RatActionStates.Climb:
                    PlayClimp();
                    break;
                case RatActionStates.JumpOff:
                    PlayJumpOff();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("current", current, null);
            }
        }
    }
}