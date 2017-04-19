using UnityEngine;

namespace NeonRattie.Shared
{
    //some place for all shared behaviour
    public abstract class NeonRattieBehaviour : MonoBehaviour
    {
        public abstract void Destroy();

        public abstract void Initialise();

    }
}
