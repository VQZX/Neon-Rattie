using Flusk.Patterns;
using NeonRattie.Rat;
using NeonRattie.Viewing;
using UnityEngine;

namespace NeonRattie.Management
{
    public class SceneObjects : Singleton<SceneObjects>
    {
        [SerializeField] protected CameraControls cameraControls;
        public CameraControls CameraControls
        {
            get { return cameraControls; }
        }

        [SerializeField] protected RatController ratController;
        public RatController RatController
        {
            get { return ratController; }
        }
    }
}