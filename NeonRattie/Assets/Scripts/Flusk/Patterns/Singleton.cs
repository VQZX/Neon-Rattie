using UnityEngine;

namespace Flusk.Patterns
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; protected set; }

        public bool InstanceExists
        {
            get {return Instance != null; }
        }

        protected virtual void Awake ()
        {
            if (Instance == null)
            {
                Instance = (T) this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
