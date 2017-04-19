#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Flusk.Management
{

    [InitializeOnLoad]
    public class Initialisation
    {
        static void StartUp ()
        {
            Debug.Log("Up and running");
        }
    }
}
#endif
