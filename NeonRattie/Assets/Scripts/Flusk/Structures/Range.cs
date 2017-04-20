using System;

namespace Flusk.Structures
{
    [Serializable]
    public struct Range
    {
        public float Min;
        public float Max;

        public float Median
        {
            get { return ((Min + Max) * 0.5f); }
            private set { throw new NotImplementedException(); }
        }
    }
}