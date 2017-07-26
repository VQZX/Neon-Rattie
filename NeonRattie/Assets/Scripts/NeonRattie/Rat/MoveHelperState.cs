using System;

namespace NeonRattie.Rat
{
    [Flags]
    public enum MoveHelperState
    {
        None = 0,
        
        Translate = 1,
        
        Rotation = 2,
        
        Scale = 4
    }
}