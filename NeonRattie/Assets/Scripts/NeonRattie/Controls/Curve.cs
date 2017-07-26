using System.Collections.Generic;
using UnityEngine;

namespace NeonRattie.Controls
{
    public class Curve
    {
        public Vector3 this [int index]
        {
            get { return points[index]; }
            set { points[index] = value; }
        }

        private readonly List<Vector3> points;

        public int CurrentIndex { get; private set; }
        public bool CurrentIndexValid
        {
            get
            {
                return CurrentIndex < points.Count;
            }
        }


        public Curve ()
        {
            CurrentIndex = 0;
            points = new List<Vector3>();
        }

        public Vector3 CurrentPoint ()
        {
            return points[CurrentIndex++];
        }

        public void Add (Vector3 point)
        {
            points.Add(point);
        }

    }
}
