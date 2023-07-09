using UnityEngine;

public enum TileType
{ 
    Ground,
    Wall,
    Obstacle,
    Enemy
}

public class PlayerController : MotionController
{
    bool _isActivated;

    protected override void Awake()
    {
        base.Awake();
        GameDelegatesContainer.Start += OnStart;
        GameDelegatesContainer.End += OnEnd;
    }

    void OnDestroy()
    {
        GameDelegatesContainer.Start -= OnStart;
        GameDelegatesContainer.End -= OnEnd;
    }

    void OnStart()
    {
        PlayerDelegatesContainer.IsMoving += IsMoving;
        PlayerDelegatesContainer.NewMoveDestination += OnNewMove;
    }

    void OnEnd()
    {
        PlayerDelegatesContainer.IsMoving -= IsMoving;
        PlayerDelegatesContainer.NewMoveDestination -= OnNewMove;
    }

    void OnNewMove(Vector2 position)
    {
        start = transform.position;
        end = position;
        lerp = 0;
    }

    Transform GetTransform()
    {
        return transform;
    }
}