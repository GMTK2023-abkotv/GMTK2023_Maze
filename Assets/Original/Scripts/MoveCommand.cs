using Unity.Mathematics;

public struct MoveCommand
{
    public MotionType Motion;
    public float3 Direction;
}

public enum MotionType
{ 
    Nihil,
    Walk,
    Jump,
    Dash
}