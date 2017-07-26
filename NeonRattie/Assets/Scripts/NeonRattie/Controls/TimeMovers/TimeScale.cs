using UnityEngine;

namespace NeonRattie.Controls.TimeMovers
{
    public class TimeScale : TimeMover<Vector3>
    {

        public override void Tick(float deltaTime)
        {
            if (TimeLog >= 1)
            {
                return;
            }
            Vector3 current = Manipulation.localScale;
            Vector3 next = Vector3.Lerp(current, Data, TimeLog);
            Manipulation.localScale = next;
            TimeLog += deltaTime * Speed;
        }

        public TimeScale(Transform transform, float speed) : base(transform, speed)
        {
        }
    }
}