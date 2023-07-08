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
    KeyCode _interactKey = KeyCode.E;

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        
        bool isWalking = IsWalkCommand(out int2 direction);

        if (isWalking)
        {
            MoveCommand walkCommand = new()
            {
                Direction = new Vector3Int(direction.x, direction.y, 0)
            };
            PlayerDelegatesContainer.EventMoveCommand?.Invoke(walkCommand);
        }

        if (Input.GetKeyUp(_interactKey))
        {
            PlayerDelegatesContainer.EventInteractCommand?.Invoke();
        }

    }

    bool IsWalkCommand(out int2 direction)
    {
        bool isWalking = false;
        direction = int2.zero;
        if (Input.GetKeyUp(_moveLeftKey))
        {
            direction.x = -1;
            isWalking = true;
        }
        else if (Input.GetKeyUp(_moveRightKey))
        {
            direction.x = 1;
            isWalking = true;
        }
        else if (Input.GetKeyUp(_moveDownKey))
        {
            direction.y = -1;
            isWalking = true;
        }
        else if (Input.GetKeyUp(_moveUpKey))
        {
            direction.y = 1;
            isWalking = true;
        }
        return isWalking;
    }
}