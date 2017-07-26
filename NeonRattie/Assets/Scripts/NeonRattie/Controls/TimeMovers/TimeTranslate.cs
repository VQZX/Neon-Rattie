using UnityEngine;

namespace NeonRattie.Controls.TimeMovers
{
    public class TimeTranslate : TimeMover<Vector3>
    {
        public override void Tick(float deltaTime)
        {
            if (TimeLog >= 1)
            {
                return;
            }
            Vector3 current = Manipulation.position;
            Vector3 next = Vector3.Lerp(current, Data, TimeLog);
            Manipulation.position = next;
            TimeLog += deltaTime * Speed;
        }

        public TimeTranslate(Transform transform, float speed) : base(transform, speed)
        {
        }
    }
}