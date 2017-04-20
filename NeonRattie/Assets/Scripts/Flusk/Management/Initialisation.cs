#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Flusk.Management
{
    public class Initialisation
    {
        private const string PATH = "MainPrefab";

        [RuntimeInitializeOnLoadMethod]
        static void StartUp ()
        {
            GameObject mainPrefab = Resources.Load(PATH) as GameObject;
            var mp = mainPrefab.GetComponent<MainPrefab>();
            mp.ForceSet();
            mp.Initialise();
        }
    }
}
#endif
