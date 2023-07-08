using UnityEngine;

public class PlayerController : MotionController
{
    bool _isActivated;

    protected override void Awake()
    {
        base.Awake();
        PlayerDelegatesContainer.IsMoving += IsMoving;
        PlayerDelegatesContainer.NewMoveDestination += OnNewMove;
    }

    void OnDestroy()
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

    bool IsMoving()
    {
        return lerp <= 1;
    }
}