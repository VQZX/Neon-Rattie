using System;
using Flusk.Management;
using NeonRattie.Controls;
using NeonRattie.Rat;
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

        [SerializeField]
        protected float translationSpeed = 1;

        [SerializeField]
        protected float maxRotationModifier = 5;

        protected float maxRotation = 10;

        private Vector3 initDirectionToRat;

        private Vector3 speedTest;

        private Vector3 idleForward;

        private Vector3 originalRot;

        protected float lerpTime = 0;

        protected float slerpTime = 0;

        public Vector3 GetFlatRight ()
        {
            Vector3 right = transform.right;
            right.y = 0;
            return right;
        }

        public Vector3 GetFlatForward()
        {
            Vector3 forward = transform.forward;
            forward.y = 0;
            return forward;
        }

        protected virtual void Start()
        {
            if (rat == null)
            {
                rat = SceneManagement.Instance.Rat;
            }
            initDirectionToRat = (rat.transform.position - transform.position).normalized;
            originalRot = transform.rotation.eulerAngles;
        }

        protected virtual void Update()
        {   
            FreeControl(Time.deltaTime);
        }

        protected void FreeControl(float deltaTime)
        {
            transform.position = Vector3.Lerp(transform.position, SumMotion(), translationSpeed);
            Rotation();
        }

        private void Rotation()
        {
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

            Vector2 delta = mm.DistanceFromOrigin;
            //never do them at the same time!
            var axis = Mathf.Abs(delta.y) < Mathf.Abs(delta.x) ? new Vector3(0, delta.x) : new Vector3(-delta.y, 0);
            Quaternion deltaRotation = Quaternion.AngleAxis(freeControl.RotationSpeed * delta.magnitude, axis);
            if (Math.Abs(axis.y) < 0.001f)
            {
                Quaternion rot = transform.rotation;
                rot *= deltaRotation;
                if (VerticalValid(rot))
                {
                    return;
                }
            }
            Quaternion nextRot = deltaRotation;
            Vector3 currentRot = nextRot.eulerAngles;
            currentRot.z = originalRot.z;
            Quaternion next = new Quaternion {eulerAngles = currentRot};
            maxRotation = freeControl.RotationSpeed * delta.magnitude * maxRotationModifier;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, next, maxRotation);
        }
        

        private bool VerticalValid(Quaternion rotation)
        {
            Vector3 newPosition = CalculatePositionByRotation(rotation);
            float heightDifference = (newPosition.y - rat.transform.position.y);
            float sign = Mathf.Sign(newPosition.y - transform.position.y);
            if (sign > 0)
            {
                return heightDifference < freeControl.UpMovement;
            }
            return heightDifference < freeControl.DownMovement;
        }

        private Vector3 SumMotion()
        {
            Vector3 sum = transform.position;
            sum = AlignWithRat(sum);
            sum = SlowLookAtRat(sum);
            sum = CorrectHeightFromGround(sum);
            return sum;
        }
        


        private Vector3 CorrectHeightFromGround(Vector3 pos)
        {
            Vector3 ratPos = rat.RatPosition.transform.position;
            pos.y = ratPos.y + followData.HeightAboveAgent;
            return pos;
        }

        private Vector3 AlignWithRat(Vector3 pos)
        {
            //CorrectHeightFromGround();
            Ray lookingRay = new Ray(rat.RatPosition.position, -initDirectionToRat);
            Ray ratRay = new Ray(rat.RatPosition.position, -rat.LocalForward);
            Vector3 look = lookingRay.GetPoint(followData.DistanceFromPlayer);
            Vector3 ratLook = ratRay.GetPoint(followData.DistanceFromPlayer);
            var avg = new Vector3(ratLook.x, look.y, ratLook.z);
            Vector3 current = pos;
            return Vector3.Lerp(current, avg, Time.deltaTime * 10);
        }

        private Vector3 SlowLookAtRat (Vector3 pos)
        {
            Vector3 idealPlayerPosition = CalculatePositionByRotation(transform.rotation);
            Vector3 difference = (rat.transform.position - idealPlayerPosition);
            pos += difference;
            return pos;
        }

        private Vector3 CalculatePositionByRotation(Quaternion rotation)
        {
            Vector3 newForward = rotation * Vector3.forward;
            Vector3 newPosition = transform.position + newForward * followData.DistanceFromPlayer;
            return newPosition;
        }
    }
}
