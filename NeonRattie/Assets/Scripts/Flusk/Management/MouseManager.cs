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
        public Vector2 DistanceFromOrigin { get; protected set; }

        protected Vector2 previousScreen;

        private static readonly Vector2 ScreenViewCenter = new Vector2(0.5f, 5f);
        private Vector2 origin = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        public void GetMotionData(out Vector3 euler, out float angle)
        {
            var rotationDelta = MouseManager.Instance.Delta;
            euler = new Vector3(-rotationDelta.y, rotationDelta.x);
            angle = Mathf.Atan2(euler.y, euler.x);
        }

        public void GetMotionDataStatic(out Vector3 euler, out float angle)
        {
            Vector2 difference = ViewPosition - ScreenViewCenter;
            euler = new Vector3(-difference.y, difference.x);
            angle = Mathf.Atan2(difference.y, difference.x);
        }

        protected virtual void Start()
        {
            ScreenPosition = Input.mousePosition;
            ViewPosition = Camera.main.ScreenToViewportPoint(ScreenPosition);
        }

        protected virtual void Update()
        {
            Delta = ((Vector2) Input.mousePosition) - ScreenPosition;
            ScreenPosition = Input.mousePosition;
            ViewPosition = Camera.main.ScreenToViewportPoint(ScreenPosition);
            DistanceFromOrigin = ScreenPosition - origin;

        }
    }
}
