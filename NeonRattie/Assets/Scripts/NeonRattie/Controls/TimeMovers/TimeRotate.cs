using UnityEngine;

namespace NeonRattie.Controls.TimeMovers
{
    public class TimeRotate : TimeMover<Quaternion>
    {
        public TimeRotate(Transform transform, float speed) : base(transform, speed)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (TimeLog >= 1)
            {
                return;
            }
            Quaternion current = Manipulation.rotation;
            Quaternion next = Quaternion.Slerp(current, Data, TimeLog);
            Manipulation.rotation = next;
        }
    }
}