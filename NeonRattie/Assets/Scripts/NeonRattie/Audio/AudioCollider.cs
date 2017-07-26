using UnityEngine;

namespace NeonRattie.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioCollider : MonoBehaviour
    {
        private AudioSource source;

        protected virtual void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            source.Play();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            source.Play();
        }
    }
}