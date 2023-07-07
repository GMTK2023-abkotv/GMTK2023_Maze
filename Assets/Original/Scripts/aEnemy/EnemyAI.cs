using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public class EnemyAI : MotionController
{
    [SerializeField]
    Collider _death;

    Transform _player;
    Vector3 _lastPlayerPos;
    protected override void Awake()
    {
        base.Awake();
        _death.enabled = false;
    }

    void Start()
    {
        _player = PlayerDelegatesContainer.GetTransform();
        _lastPlayerPos = _player.position;
    }

    protected override void Update()
    {
        base.Update();

        Vector3 dir = math.normalize(_lastPlayerPos - transform.position);
        MoveCommand moveCommand = new() { Motion = MotionType.Walk, Direction = dir };
        OnMoveCommand(moveCommand);
        _lastPlayerPos = _player.position;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == Layers.Player)
        {
            _moveForceAmount += 300;
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(0.5f);
        _death.enabled = true;
        Debug.Log("enable death");
    }
}