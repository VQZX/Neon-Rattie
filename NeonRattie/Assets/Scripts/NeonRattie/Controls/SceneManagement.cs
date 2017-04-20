using Flusk.Patterns;
using NeonRattie.Rat;

namespace NeonRattie.Controls
{
    public class SceneManagement : PersistentSingleton<SceneManagement>
    {
        public RatController Rat { get; set; }
    }
}
