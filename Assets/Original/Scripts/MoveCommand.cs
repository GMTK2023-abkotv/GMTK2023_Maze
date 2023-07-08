using Unity.Mathematics;

public struct MoveCommand
{
    public MotionType Motion;
    public float2 Direction;
}

public enum MotionType
{ 
    Nihil,
    Walk,
    Jump,
    Dash
}