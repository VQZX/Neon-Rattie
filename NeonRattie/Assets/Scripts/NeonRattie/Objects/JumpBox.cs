using NeonRattie.Shared;
using UnityEngine;

namespace NeonRattie.Objects
{
    public class JumpBox : NeonRattieBehaviour
    {
        [SerializeField] protected LayerMask jumpLayer;

        public override void Destroy()
        {
            throw new System.NotImplementedException();
        }

        public override void Initialise()
        {
            throw new System.NotImplementedException();
        }
    }
}
