namespace Flusk.Patterns
{
    public class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
    {
        protected sealed override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}
