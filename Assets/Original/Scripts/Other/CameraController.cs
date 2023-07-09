using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera vm;

    void Awake()
    {
        TryGetComponent(out vm);

        GameDelegatesContainer.PlayerSpawn += OnPlayerSpawn;
    }

    void OnDestroy()
    {
        GameDelegatesContainer.PlayerSpawn -= OnPlayerSpawn;
    }

    void OnPlayerSpawn(Vector3Int gridPos, Transform player)
    {
        vm.Follow = player;
    }
}
