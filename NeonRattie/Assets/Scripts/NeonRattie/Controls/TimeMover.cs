using UnityEngine;

namespace NeonRattie.Controls
{
    public abstract class TimeMover<TType> 
    {
        public TType Data { get; protected set; }
        
        public float Speed { get; set; }
        
        public float TimeLog { get; protected set; }
        
        public Transform Manipulation { get; protected set; }

        protected TimeMover (Transform transform, float speed)
        {
            TimeLog = 0;
            Speed = Speed;
            Manipulation = transform;
        }
        
        public void UpdateData(TType data)
        {
            Data = data;
        }

        public void Cancel()
        {
            TimeLog = Mathf.Infinity;
        }
        
        public abstract void Tick(float deltaTime);
    }
}