using UnityEngine;

public class PlayerController : MotionController
{
    [SerializeField]
    Transform start;

    bool _isActivated;

    protected override void Awake()
    {
        base.Awake();
        PlayerDelegatesContainer.EventPlayerAlive += OnAlive;
        PlayerDelegatesContainer.GetTransform += GetTransform;
    }

    void OnAlive()
    {
        PlayerDelegatesContainer.EventMoveCommand += OnMoveCommand;

        transform.position = start.position;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventPlayerAlive -= OnAlive;
        PlayerDelegatesContainer.GetTransform -= GetTransform;
    }

    Transform GetTransform()
    {
        return transform;
    }
}