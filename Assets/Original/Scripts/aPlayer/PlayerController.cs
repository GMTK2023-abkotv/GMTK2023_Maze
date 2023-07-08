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
        PlayerDelegatesContainer.EventPlayerDead += OnDeath;
    }

    void OnDeath()
    { 
        PlayerDelegatesContainer.EventMoveCommand -= OnMoveCommand;
        PlayerDelegatesContainer.EventPlayerDead -= OnDeath;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventPlayerDead?.Invoke();
        PlayerDelegatesContainer.EventPlayerAlive -= OnAlive;
        PlayerDelegatesContainer.GetTransform -= GetTransform;
    }

    Transform GetTransform()
    {
        return transform;
    }
}