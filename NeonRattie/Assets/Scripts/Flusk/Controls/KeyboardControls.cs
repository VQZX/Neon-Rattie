using System;
using Flusk.Patterns;
using UnityEngine;

namespace Flusk.Controls
{
    public class KeyboardControls : PersistentSingleton<KeyboardControls>
    {
        [SerializeField] protected KeyCheck[] codes;

        public event Action<KeyData> KeyHit;

        protected virtual void Start()
        {
            var count = codes.Length;
            for (var i = 0; i < count; ++i)
            {
                codes[i].Init();
            }
        }

        protected virtual void Update()
        {
            Check();
        }

        private void Check()
        {
            var count = codes.Length;
            for (var i = 0; i < count; ++i)
            {
                if (!codes[i].Check())
                {
                    continue;
                }
                KeyData data = new KeyData();
                data.Code = codes[i].Code;
                data.AxisValue = 0;
                if (KeyHit != null)
                {
                    KeyHit(data);
                }
            }
        }
    }
}
