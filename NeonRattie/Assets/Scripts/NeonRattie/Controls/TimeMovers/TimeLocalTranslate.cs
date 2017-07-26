using UnityEngine;

namespace NeonRattie.Controls.TimeMovers
{
    public class TimeLocalTranslate : TimeMover<Vector3>
    {
        public override void Tick(float deltaTime)
        {
            if (TimeLog >= 1)
            {
                return;
            }
            Vector3 current = Manipulation.localPosition;
            Vector3 next = Vector3.Lerp(current, Data, TimeLog);
            Manipulation.localPosition = next;
            TimeLog += deltaTime * Speed;
        }

        public TimeLocalTranslate(Transform transform, float speed) : base(transform, speed)
        {
        }
    }
}