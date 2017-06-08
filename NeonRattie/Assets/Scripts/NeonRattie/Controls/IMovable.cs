using UnityEngine;

namespace NeonRattie.Controls
{
    public interface IMovable
    {
        bool TryMove(Vector3 position);
    }
}
