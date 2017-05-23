using UnityEngine;

namespace NeonRattie.Controls
{
    public interface IMovable
    {
        void Move(Vector3 position);

        bool TryMove(Vector3 position);
    }
}
