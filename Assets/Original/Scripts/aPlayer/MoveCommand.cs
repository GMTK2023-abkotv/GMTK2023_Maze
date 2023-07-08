using UnityEngine;

public struct MoveCommand
{
    public MotionType Motion;
    public Vector2 Direction;
}

public enum MotionType
{ 
    Nihil,
    Walk
}