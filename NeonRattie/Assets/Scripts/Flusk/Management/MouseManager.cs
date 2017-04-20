using Flusk.Patterns;
using UnityEngine;

namespace Flusk.Management
{

    /// <summary>
    /// Class for keeping tracking of all the mousey things
    /// </summary>
    public class MouseManager : Singleton<MouseManager>
    {
        public Vector2 Delta { get; protected set; }
        public Vector2 ScreenPosition { get; protected set; }
        public Vector2 ViewPosition { get; protected set; }

        protected Vector2 previousScreen;

        protected virtual void Start()
        {
            ScreenPosition = Input.mousePosition;
            ViewPosition = Camera.main.ScreenToViewportPoint(ScreenPosition);
        }

        protected virtual void Update()
        {
            Delta = ((Vector2)Input.mousePosition) - ScreenPosition;
            ScreenPosition = Input.mousePosition;
            ViewPosition = Camera.main.ScreenToViewportPoint(ScreenPosition);
        }
    }
}
