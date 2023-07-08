using UnityEngine;

public class MotionController : MonoBehaviour
{
    [SerializeField]
    float _moveDelta;

    [SerializeField]
    float _moveSpeed;

    Vector2 start;
    Vector2 end;
    float lerp;

    protected virtual void Awake()
    {
        lerp = 2;
    }

    protected void OnMoveCommand(MoveCommand newCommand)
    {
        Debug.Log("movecommand + " + lerp);
        if (lerp <= 1)
        {
            return;
        }

        switch (newCommand.Motion)
        {
            case MotionType.Walk:
                start = transform.position;
                end = (Vector2)transform.position + _moveDelta * newCommand.Direction;
                lerp = 0;
                Debug.Log($"{start} {end}");
                break;
        }
    }

    void Update()
    {
        if (lerp <= 1)
        {
            lerp += _moveSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, lerp);
        }
    }
}