using System;
using Flusk.Management;
using Flusk.Structures;
using NeonRattie.Controls;
using NeonRattie.Rat;
using NeonRattie.Rat.RatStates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NeonRattie.Viewing
{
    [RequireComponent(typeof(Camera))]
    public class CameraControls : MonoBehaviour
    {
        [SerializeField]
        protected RatController rat;

        [SerializeField] protected Range distanceFromRat;


        //maybe use angle limitation instead 
        [SerializeField] protected Range verticalRotation;
        [SerializeField] protected Range horizontalRotation;

        [SerializeField] protected float nonIdleLimiter = 0.5f;

        [SerializeField] protected float rotateSpeed = 10;

        private Vector2 rotationDelta;

        private Quaternion originalRotation;
        private Vector3 originalPosition;
        private Vector3 ratDirection;
        private float flatDistance = 0;

        public void LoadRat()
        {
            rat = (SceneManagement.Instance as SceneManagement).Rat;
        }

        //used if we want make the camera look elsewhere
        public void UnLoadRat()
        {
            rat = null;
        }

        protected virtual void Start()
        {
            originalRotation = transform.localRotation;
            LoadRat();
            ratDirection = (transform.position - rat.transform.position).normalized;
            originalPosition = transform.position;
            CalculateFlat();
        }


        protected virtual void CalculateFlat()
        {
            var y = rat.transform.position.y - originalPosition.y;
            var hyp = distanceFromRat.Median;
            flatDistance = Mathf.Sqrt((y * y) + (hyp * hyp));
        }

        protected virtual void LateUpdate()
        {
            if (rat == null)
            {
                return;
            }

            Translation();
            Rotation();
            Vector3 point = transform.position;
            point.y = originalPosition.y;
            transform.position = point;

            return;
        }

        private void Translation()
        {
            transform.position = rat.transform.position - rat.ForwardDirection * distanceFromRat.Median;
        }

        private void Rotation()
        {
            if (MouseManager.Instance == null)
            {
                return;
            }
            rotationDelta = MouseManager.Instance.Delta;
            if (rat.StateMachine.CurrentState is Idle)
            {
                //allow free look around
                FollowMouse();
                return;
            }
            if (Math.Abs(MouseManager.Instance.Delta.sqrMagnitude) < 0.01f)
            {
                //close to no movement, return to rat
                transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRotation,
                    Time.deltaTime * rotateSpeed);
                return;
            }
            //bias towards rat
            transform.LookAt(rat.transform);
        }

        private void FollowMouse(float speed = 1)
        {
            //transform.LookAt(rat.transform);
            var euler = new Vector3(-rotationDelta.y, rotationDelta.x);

            var current = transform.localRotation;
            var delta = new Quaternion { eulerAngles = euler };
            var next = current * delta;
            transform.localRotation = Quaternion.Slerp(current, next, Time.deltaTime * rotateSpeed * speed);

        }      
    }
}
