using UnityEngine;

public class MotionController : MonoBehaviour
{
    [SerializeField]
    float _moveDelta;

    [SerializeField]
    float _moveSpeed;

    protected Vector2 start;
    protected Vector2 end;
    protected float lerp;

    protected virtual void Awake()
    {
        lerp = 2;
    }

    void Update()
    {
        if (IsMoving())
        {
            lerp += _moveSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, lerp);
        }
    }

    protected bool IsMoving()
    {
        return lerp <= 1;
    }
}