namespace Flusk.Patterns
{
    public class PersistentSingleton<T> : Singleton<PersistentSingleton<T>>
    {
        protected sealed override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}
