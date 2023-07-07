using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MotionController : MonoBehaviour
{
    [SerializeField]
    protected float _dashForceAmount = 400;

    [SerializeField]
    protected float _jumpForceAmount = 400;

    [SerializeField]
    protected float _moveForceAmount = 4;

    protected Rigidbody _rigidBody;

    float _jumpCooldown = 1;
    [SerializeField]
    float _dashCooldown = 3;

    float _jumpCooldownTimer;
    float _dashCooldownTimer;

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
            case MotionType.Dash:
                if (_dashCooldownTimer < 0)
                {
                    if (math.all(newCommand.Direction == float3.zero))
                    { 
                        newCommand.Direction = math.normalize(_rigidBody.velocity);
                    }
                    _command = newCommand;
                }
                break;
            case MotionType.Jump:
                if (_jumpCooldownTimer < 0 &&
                    Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, 1))
                {
                    _command = newCommand;
                }
                break;
            case MotionType.Walk:
                if (_command.Motion == MotionType.Nihil)
                {
                    _command = newCommand;
                    return;
                }
                
                if (_command.Motion == MotionType.Dash)
                { 
                    float3 dir = new float3(newCommand.Direction.x, 0, newCommand.Direction.z);
                    _command.Direction = math.normalizesafe(dir);
                    return;
                }
                break;
        }
    }

    protected virtual void Update()
    {
        _dashCooldownTimer -= Time.deltaTime;
        _jumpCooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        switch (_command.Motion)
        {
            case MotionType.Dash:
                _rigidBody.AddForce(_dashForceAmount * _command.Direction);
                _dashCooldownTimer = _dashCooldown;
                OnDash?.Invoke();
                break;
            case MotionType.Jump:
                _rigidBody.AddForce(_jumpForceAmount * _command.Direction);
                _jumpCooldownTimer = _jumpCooldown;
                OnJump?.Invoke();
                break;
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