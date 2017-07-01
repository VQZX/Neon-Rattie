using UnityEngine;

namespace NeonRattie.Controls.TimeMovers
{
    public class TimeLocalRotate : TimeMover<Quaternion>
    {
        public TimeLocalRotate(Transform transform, float speed) : base(transform, speed)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (TimeLog >= 1)
            {
                return;
            }
            Quaternion current = Manipulation.localRotation;
            Quaternion next = Quaternion.Slerp(current, Data, TimeLog);
            Manipulation.localRotation = next;
        }
    }
}