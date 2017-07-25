using UnityEngine;

namespace NeonRattie.Audio
{
    public class RatAudio : MonoBehaviour
    {
        /// <summary>
        /// List of audio sources attached to this object
        /// </summary>
        [SerializeField]
        protected AudioSource[] audioSources;

        public void Play(AudioClip clip)
        {
            var source = AudioHelper.FindAvailableAudioSource(audioSources);
            source.clip = clip;
            source.Play();
        }
    }
}