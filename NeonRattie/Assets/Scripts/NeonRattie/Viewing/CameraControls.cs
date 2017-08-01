using System;
using Flusk.Management;
using Flusk.Structures;
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
        protected float rotationSlerpSpeed = 5;

        [SerializeField]
        protected float maxAngleRoation = 100;

        [SerializeField]
        protected float orbitSpeed = 10;

        [SerializeField]
        protected Range xRange;

        [SerializeField]
        protected Range yRange;

        protected float maxRotation = 10;

        private Vector3 initDirectionToRat;

        private Vector3 speedTest;

        private Vector3 idleForward;

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
        }

        protected virtual void Update()
        {   
            AxisRotation();
            RealignToRat();
        }

        private void AxisRotation()
        {
            MouseManager mm;
            if (!MouseManager.TryGetInstance(out mm))
            {
                return;
            }
            Vector3 delta = mm.ExpandedAxis;
            if (delta.magnitude <= float.Epsilon)
            {
                return;
            }
            var axis = Mathf.Abs(delta.y) < Mathf.Abs(delta.x) ? 
                new Vector3(0, Mathf.Clamp(delta.x, xRange.Min, xRange.Max)) : 
                new Vector3(Mathf.Clamp(-delta.y, yRange.Min, yRange.Max), 0);
            float angle = Mathf.Clamp(freeControl.RotationSpeed * delta.magnitude, 0, maxAngleRoation);
            Quaternion deltaRotation = Quaternion.AngleAxis(angle, axis);
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
            transform.rotation = Quaternion.Slerp(transform.rotation, next, Time.deltaTime * rotationSlerpSpeed);
            //ensure no z-rotation
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

        private Vector3 CorrectHeightFromGround(Vector3 pos)
        {
            Vector3 ratPos = rat.RatPosition.transform.position;
            pos.y = ratPos.y + followData.HeightAboveAgent;
            return pos;
        }


        private void RealignToRat()
        {
            Ray currentCameraRay = new Ray(transform.position, transform.forward);
            Vector3 point = currentCameraRay.GetPoint(followData.DistanceFromPlayer);
            Vector3 difference = rat.transform.position - point;
            transform.position = Vector3.Slerp(transform.position, transform.position + difference, Time.deltaTime * orbitSpeed);
            transform.position = CorrectHeightFromGround(transform.position);
        }


        private Vector3 CalculatePositionByRotation(Quaternion rotation)
        {
            Vector3 newForward = rotation * Vector3.forward;
            Vector3 newPosition = transform.position + newForward * followData.DistanceFromPlayer;
            return newPosition;
        }
    }
}
