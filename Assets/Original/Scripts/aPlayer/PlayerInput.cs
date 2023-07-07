using UnityEngine;
using Unity.Mathematics;

[DefaultExecutionOrder(-1)]
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    KeyCode _moveLeftKey = KeyCode.A;
    [SerializeField]
    KeyCode _moveRightKey = KeyCode.D;
    [SerializeField]
    KeyCode _moveDownKey = KeyCode.S;
    [SerializeField]
    KeyCode _moveUpKey = KeyCode.W;

    [SerializeField]
    KeyCode _jumpKey = KeyCode.Space;
    [SerializeField]
    KeyCode _dashKey = KeyCode.V;

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        
        bool isWalking = IsWalkCommand(out float3 direction);

        if (Input.GetKeyDown(_jumpKey))
        {
            MoveCommand jumpCommand = new() { Motion = MotionType.Jump };
            jumpCommand.Direction = math.up();
            PlayerDelegatesContainer.EventMoveCommand?.Invoke(jumpCommand);
            return;
        }

        if (Input.GetKeyDown(_dashKey))
        {
            MoveCommand dashCommand = new() { Motion = MotionType.Dash };
            if (isWalking) dashCommand.Direction = direction;
            else dashCommand.Direction = float3.zero;
            PlayerDelegatesContainer.EventMoveCommand?.Invoke(dashCommand);
            return;
        }

        if (isWalking)
        {
            MoveCommand walkCommand = new() 
            { 
                Motion = MotionType.Walk,
                Direction = direction
            };
            PlayerDelegatesContainer.EventMoveCommand?.Invoke(walkCommand);
        }

    }

    bool IsWalkCommand(out float3 direction)
    {
        bool isWalking = false;
        direction = float3.zero;
        if (Input.GetKey(_moveLeftKey))
        {
            direction.x = -1;
            isWalking = true;
        }
        else if (Input.GetKey(_moveRightKey))
        {
            direction.x = 1;
            isWalking = true;
        }

        if (Input.GetKey(_moveDownKey))
        {
            direction.z = -1;
            isWalking = true;
        }
        else if (Input.GetKey(_moveUpKey))
        {
            direction.z = 1;
            isWalking = true;
        }
        direction = math.normalizesafe(direction);
        return isWalking;
    }
}