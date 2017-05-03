using Flusk.Management;
using NeonRattie.Controls;
using NeonRattie.Rat;
using NeonRattie.Rat.RatStates;
using UnityEngine;

namespace NeonRattie.Viewing
{
    //TODO: defs requires a FSM
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

        [SerializeField] protected float slerpTimeModifier = 0.1f;

        private Vector3 initDirectionToRat;

        private Vector3 speedTest;

        private Vector3 idleForward;

        private Vector3 originalRot;

        protected float slerpTime;

        protected virtual void Start()
        {
            if (rat == null)
            {
                rat = SceneManagement.Instance.Rat;
            }
            initDirectionToRat = (rat.transform.position - transform.position).normalized;
            originalRot = transform.rotation.eulerAngles;
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
            Debug.Log("Not inside");
            //TODO: add acceleration to prevent snapping
            LookAt();
            AlignWithRat();
            idleForward = transform.forward;
        }

        protected void FreeControl()
        {
            //AlignWithRat();
            //transform.LookAt(rat.RatPosition);
            slerpTime = 0;
            MouseManager mm;
            if (!MouseManager.TryGetInstance(out mm))
            {
                return;
            }
            Vector3 euler;
            float angle;
            mm.GetMotionData(out euler, out angle);

            //the rat takes care of itself horizontally
            //so we only need vertically
            //HACK: we will have to add independant, decoupled horizontal motion

            Vector2 delta = mm.Delta;
            //never do them at the same time!
            Vector3 axis;
            axis = Mathf.Abs(delta.y) < Mathf.Abs(delta.x) ? new Vector3(0, delta.x) : new Vector3(-delta.y, 0);
            if (axis.y == 0)
            {
                Quaternion rot = transform.rotation;
                rot *= Quaternion.AngleAxis(freeControl.RotationSpeed * delta.magnitude, axis);
                if (VerticalValid(rot.eulerAngles))
                {
                    return;
                }
            }
            transform.rotation *= Quaternion.AngleAxis(freeControl.RotationSpeed * delta.magnitude, axis);
            Vector3 currentRot = transform.rotation.eulerAngles;
            currentRot.z = originalRot.z;
            transform.rotation = new Quaternion {eulerAngles = currentRot};
        }

        private bool VerticalValid(Vector3 next)
        {
            float origX = originalRot.x;
            float currentX = next.x;
            if (currentX > 180)
            {
                currentX -= 360;
            }
            float difference = Mathf.Abs(origX - currentX);
            bool lessThan = difference > freeControl.DownMovement;
            bool moreThan = difference > freeControl.UpMovement;
            return lessThan || moreThan;
        }


        private void CorrectHeightFromGround()
        {
            Vector3 pos = transform.position;
            Vector3 ratPos = rat.RatPosition.transform.position;
            pos.y = ratPos.y + followData.HeightAboveAgent;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * followData.PitchRotation);
        }

        private void AlignWithRat()
        {
            CorrectHeightFromGround();
            //the ray that we follow
            Ray lookingRay = new Ray(rat.RatPosition.position, -initDirectionToRat);
            Ray ratRay = new Ray(rat.RatPosition.position, -rat.LocalForward);
            Vector3 look = lookingRay.GetPoint(followData.DistanceFromPlayer);
            Vector3 ratLook = ratRay.GetPoint(followData.DistanceFromPlayer);
            var avg = new Vector3(ratLook.x, look.y, ratLook.z);
            Vector3 current = transform.position;
            transform.position = Vector3.Lerp(current, avg, Time.deltaTime * followData.PitchRotation * 25);
        }

        private void LookAt()
        {
            slerpTime += Time.deltaTime * slerpTimeModifier;
            Vector3 direction = (rat.RatPosition.position - transform.position).normalized;
            Quaternion current = transform.rotation;
            Quaternion next = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(current, next, slerpTime);
        }
    }
}
