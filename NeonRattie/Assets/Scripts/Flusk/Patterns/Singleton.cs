using UnityEngine;

namespace Flusk.Patterns
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        protected Singleton<T> instance;

        public T Instance
        {
            get { return (T) instance; }
        }

        protected virtual void Awake ()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
