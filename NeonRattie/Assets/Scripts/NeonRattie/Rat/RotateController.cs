using Flusk.Extensions;
using UnityEditor;
using UnityEngine;

namespace NeonRattie.Rat
{
    public class RotateController : MonoBehaviour
    {
        private float angle;
        private Vector3 axis;

        private Quaternion goal;

        private bool isRotating;

        private float slerpTime;
        private float speed;

        public void SetRotation(float toAngle, Vector3 aroundAxis, float newSpeed = 1)
        {
            angle = toAngle;
            axis = aroundAxis;
            goal = Quaternion.AngleAxis(toAngle, aroundAxis);
            speed = newSpeed;
            isRotating = true;
        }

        public void SetLookDirection(Vector3 direction, Vector3 upAxis, float rotateSpeed = 1)
        {
            goal = Quaternion.LookRotation(direction, upAxis);
            speed = rotateSpeed;
            isRotating = true;
        }
        
        protected virtual void Rotate()
        {
            Quaternion current = transform.rotation;
            Quaternion next = Quaternion.Slerp(current, goal, slerpTime);
            transform.rotation = next;
            slerpTime += Time.deltaTime * speed;
            Quaternion difference = next.Difference(goal);
            float change = next.eulerAngles.y - goal.eulerAngles.y;
            if (Mathf.Abs(change) < 0.01f)
            {
                slerpTime = 0;
            }
            
        }

        protected virtual void Update()
        {
            Rotate();
        }
    }
}