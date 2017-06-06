using System;
using UnityEngine;

namespace NeonRattie.Rat.Data
{
    [Serializable]
    public struct AnimationMotion
    {
        public AnimationCurve VerticalMotion;
        public AnimationCurve ForwardMotion;
    }
}