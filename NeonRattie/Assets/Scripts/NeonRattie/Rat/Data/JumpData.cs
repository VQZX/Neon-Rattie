using UnityEngine;

namespace NeonRattie.Rat.Data
{

    [CreateAssetMenu(fileName = "JumpData.asset", menuName = "Data/Jump", order = 1)]
    public class JumpData : ScriptableObject
    {
        [SerializeField]
        protected AnimationCurve jumpCurve;

        public AnimationCurve JumpCurve
        {
            get { return jumpCurve; }
        }
    }
}
