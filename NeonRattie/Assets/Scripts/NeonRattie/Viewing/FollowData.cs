using System;

namespace NeonRattie.Viewing
{
    [Serializable]
    public struct FollowData
    {
        public float HeightAboveAgent;
        public float DistanceFromPlayer;
        public float PitchRotation;
    }
}