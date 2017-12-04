using NeonRattie.Controls.TimeMovers;
using UnityEngine;

namespace NeonRattie.Rat
{
    /// <summary>
    /// A simple class for helping make movement look smoother
    /// attached to the object its moving
    /// </summary>
    public class MoveHelper : MonoBehaviour
    {
        [SerializeField]
        protected float rotateSpeed = 1, translateSpeed = 1, scaleSpeed = 1;
        
        protected TimeLocalRotate rotate;
        protected TimeTranslate translate;
        protected TimeScale scale;
        
        public void Translate(Vector3 point)
        {
            translate.UpdateData(point);    
        }

        public void Rotate(Quaternion rot)
        {
            rotate.UpdateData(rot);
        }

        public void Scale(Vector3 size)
        {
            scale.UpdateData(size);
        }

        private void Awake()
        {
            rotate = new TimeLocalRotate(transform, rotateSpeed);
            translate = new TimeTranslate(transform, translateSpeed);
            scale = new TimeScale(transform, scaleSpeed);
            
            rotate.Cancel();
            translate.Cancel();
            scale.Cancel();
        }

        protected virtual void Update()
        {
            translate.Tick(Time.deltaTime);
            rotate.Tick(Time.deltaTime);
            scale.Tick(Time.deltaTime);
        }


        protected virtual void Translate(float deltaTime)
        {
            translate.Tick(deltaTime);
        }

        protected virtual void Rotate(float deltaTime)
        {
            rotate.Tick(deltaTime);
        }

        protected virtual void Scale(float deltaTime)
        {
            scale.Tick(deltaTime);
        }
    }
}