using Flusk.Structures;
using NeonRattie.Controls;
using NeonRattie.Rat;
using NeonRattie.Rat.RatStates;
using UnityEngine;

namespace NeonRattie.Viewing
{
    [RequireComponent(typeof(Camera))]
    public class CameraControls : MonoBehaviour
    {
        [SerializeField]
        protected RatController rat;

        [SerializeField]
        protected FollowData followData;

        [SerializeField]
        protected FreeControlData freeControl;

        [SerializeField]
        protected TriggerCallback delayCollider;

        [SerializeField]
        protected LayerMask groundLayer;

        private Vector3 initDirectionToRat;

        private Vector3 speedTest;

        protected virtual void Start()
        {
            if (rat == null)
            {
                rat = SceneManagement.Instance.Rat;
            }
            initDirectionToRat = (rat.transform.position - transform.position).normalized;
        }

        protected virtual void LateUpdate()
        {
            var currentState = rat.StateMachine.CurrentState;
            if (currentState is Idle)
            {
                FreeControl();
            }
            else if ( currentState is IActionState)
            {
                Follow();
            }
            else
            {
                Debug.LogWarning("[CAMERA CONTROLS] RAT IS NOT A STATE FOR THE CAMERA TO DO ANYTHING");
            }
        }

        protected void Follow()
        {
            if (delayCollider.IsInside(rat.RatPosition.transform) )
            {
                Debug.Log("Is inside");
                return;
            }
            else
            {
                Debug.Log("Not inside");
            }
            //TODO: add acceleration to prevent snapping
            transform.LookAt(rat.transform);
            Vector3 current = transform.position;
            Vector3 next = rat.RatPosition.transform.position;
            //translation
            next -= initDirectionToRat * followData.DistanceFromPlayer;
            transform.position = Vector3.Lerp(current, next, Time.deltaTime * followData.PitchRotation);
            CorrectHeightFromGround();
        }

        protected void FreeControl()
        {
            
        }

        private void CorrectHeightFromGround()
        {
            Vector3 pos = transform.position;
            Vector3 ratPos = rat.RatPosition.transform.position;
            pos.y = ratPos.y + followData.HeightAboveAgent;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * followData.PitchRotation);
        }
    }
}
