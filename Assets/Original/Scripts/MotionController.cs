using System;
using UnityEngine;


public class MotionController : MonoBehaviour
{
    [SerializeField]
    float _moveForceAmount;

    Rigidbody2D _rigidBody;

    MoveCommand _command;

    bool isWalking;

    public Action OnStartWalk;
    public Action OnStopWalk;
    public Action OnJump;
    public Action OnDash;

    protected virtual void Awake()
    {
        TryGetComponent(out _rigidBody);
    }

    protected void OnMoveCommand(MoveCommand newCommand)
    {
        switch (newCommand.Motion)
        {
            case MotionType.Walk:
                _command = newCommand;
                break;
        }
    }

    protected virtual void Update()
    {
    }

    void FixedUpdate()
    {
        switch (_command.Motion)
        {
            case MotionType.Walk:
                _rigidBody.AddForce(_moveForceAmount * _command.Direction);
                if (!isWalking)
                {
                    isWalking = true;
                    OnStartWalk?.Invoke();
                }
                break;
            case MotionType.Nihil:
                if (isWalking)
                {
                   isWalking = false;
                   OnStopWalk?.Invoke();
                }
                break;
        }
        _command.Motion = MotionType.Nihil;
    }
}