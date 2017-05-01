using UnityEngine;

namespace NeonRattie.Viewing
{
    [RequireComponent( typeof(Collider))]
    public class TriggerCallback : MonoBehaviour
    {
        private new Collider collider;

        public bool IsInside(Transform trans)
        {
            Vector3 position = trans.position;
            return collider.bounds.Contains(position);
        }

        protected virtual void Awake()
        {
            collider = GetComponent<Collider>();
        }
    }
}
