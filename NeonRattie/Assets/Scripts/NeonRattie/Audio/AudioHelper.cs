using System.Collections.Generic;
using UnityEngine;

namespace NeonRattie.Audio
{
    public static class AudioHelper
    {
        
        public static AudioSource FindAvailableAudioSource(List<AudioSource> sources)
        {
            foreach (AudioSource source in sources)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }
            return null;
        }

        public static bool IsClipPlaying(List<AudioSource> sources, AudioClip clip)
        {
            foreach (AudioSource source in sources)
            {
                if (source.clip == clip)
                {
                    return true;
                }
            }
            return false;
        }
    }
}