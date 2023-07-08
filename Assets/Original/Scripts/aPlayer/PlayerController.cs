using UnityEngine;

public class PlayerController : MotionController
{
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