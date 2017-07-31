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
        protected float speed = 1;

        [SerializeField]
        protected float maxRotationModifier = 5;

        protected float maxRotation = 10;

        private Vector3 initDirectionToRat;

        private Vector3 speedTest;

        private Vector3 idleForward;

        private Vector3 originalRot;

        protected float lerpTime = 0;

        protected float slerpTime = 0;

        private Collider avoidCollider;

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
            RealignToRat();
            transform.position = Vector3.Lerp(transform.position, SumMotion(), Time.deltaTime * speed);
            AxisRotation();
        }

        private void AxisRotation()
        {
            MouseManager mm;
            if (!MouseManager.TryGetInstance(out mm))
            {
                return;
            }
            Vector3 delta = mm.ExpandedAxis;
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
            Quaternion next = transform.rotation * deltaRotation;
            Vector3 keepZ = next.eulerAngles;
            keepZ.z = transform.eulerAngles.z;
            next.eulerAngles = keepZ;
            transform.rotation = Quaternion.Slerp(transform.rotation, next, Time.deltaTime * 5);
            Vector3 euler = transform.eulerAngles;
            euler.z = 0;
            transform.eulerAngles = euler;
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
            return avg;
        }

        private void RealignToRat()
        {
            Ray currentCameraRay = new Ray(transform.position, transform.forward);
            Vector3 point = currentCameraRay.GetPoint(followData.DistanceFromPlayer);
            Vector3 difference = rat.transform.position - point;
            transform.position += difference;
            transform.position = CorrectHeightFromGround(transform.position);
        }

        private Vector3 SlowLookAtRat (Vector3 pos)
        {
            Vector3 idealPlayerPosition = CalculatePositionByRotation(transform.rotation);
            return idealPlayerPosition;
        }

        private Vector3 CalculatePositionByRotation(Quaternion rotation)
        {
            Vector3 newForward = rotation * Vector3.forward;
            Vector3 newPosition = transform.position + newForward * followData.DistanceFromPlayer;
            return newPosition;
        }

        protected virtual void OnTriggerEnter(Collider otherCollider)
        {
            avoidCollider = otherCollider;
        }

        protected virtual void OnTriggerExit(Collider otherCollider)
        {
            avoidCollider = null;
        }
    }
}
