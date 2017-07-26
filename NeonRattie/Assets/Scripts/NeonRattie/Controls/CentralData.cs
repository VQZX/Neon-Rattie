using Flusk.Patterns;
using NeonRattie.Rat.Data;
using UnityEngine;

namespace NeonRattie.Controls
{
    public class CentralData : PersistentSingleton<CentralData>
    {
        [SerializeField]
        protected JumpData jumpData;
        public JumpData JumpData
        {
            get { return jumpData; }
        }
    }
}
