using System;
using Flusk.DataHelp;
using UnityEngine;

namespace NeonRattie.Audio
{
    [RequireComponent(typeof(AudioSource), typeof(Rigidbody))]
    public class RigidbodyAudio : MonoBehaviour
    {
        public enum Type
        {
            Volume,
            Clip,
            Pitch
        }

        [SerializeField]
        protected Type type;

        [SerializeField]
        protected AudioClip[] clips;
        
        /// <summary>
        /// for calculating average velocity on collision
        /// </summary>
        [SerializeField]
        protected int sampleAmounts = 10;

        [Header("Used to map velocity value onto Type setting"), SerializeField]
        protected AnimationCurve velocityMapping;

        [SerializeField]
        protected float maxVelocity = 100;
        
        private AudioSource source;
        private Rigidbody body;

        private Vector3 averageVelocity;
        private Vector3 sumVelocity;

        private int currentSample;

        protected virtual void Awake()
        {
            source = GetComponent<AudioSource>();
            body = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            if (currentSample == sampleAmounts)
            {
                averageVelocity = sumVelocity / sampleAmounts;
                currentSample = 0;
                sumVelocity = Vector3.zero;
            }
            sumVelocity += body.velocity;
        }
        
        protected virtual void OnCollisionEnter(Collision other)
        {
            switch (type)
            {
                case Type.Volume:
                    Volume();
                    break;
                case Type.Clip:
                    Clip();
                    break;
                case Type.Pitch:
                    Pitch();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            source.Play();
        }

        private void Volume()
        {
            source.volume = MapVelocity();
            source.Play();
        }

        private void Clip()
        {
            int index = (int) MapVelocity();
            AudioClip clip = clips[index];
            source.clip = clip;
            source.Play();
        }

        private void Pitch()
        {
            source.pitch = MapVelocity();
            source.Play();
        }

        private float MapVelocity()
        {
            float speed = averageVelocity.magnitude;
            if (type == Type.Pitch || type == Type.Volume)
            {
                float animationMin = 0;
                float animationMax = velocityMapping.GetMax();
                float speedMin = 0;
                return speed.Map(speedMin, maxVelocity, animationMin, animationMax);
            }
            return (int) speed.Map(0, maxVelocity, 0, clips.Length - 1);
        }
    }
}