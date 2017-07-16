using System;

namespace NeonRattie.Utility
{
    public interface IUpdater
    {
        Action<float> UpdateAction 
        {
            get;
            set;
        }
        
        void Update(float deltaTime);
    }
}