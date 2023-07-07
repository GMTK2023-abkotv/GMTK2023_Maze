using UnityEngine;

using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Camera _renderingCamera;

    [SerializeField]
    CinemachineVirtualCamera _vcam;

    void Awake()
    {
        PlayerDelegatesContainer.EventPlayerAlive += OnPlayerAlive;
        PlayerDelegatesContainer.EventPlayerDead  += OnPlayerDead;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventPlayerAlive -= OnPlayerAlive;
        PlayerDelegatesContainer.EventPlayerDead  -= OnPlayerDead;
    }

    void Start()
    {

    }

    void OnPlayerAlive()
    { 
        
    }

    void OnPlayerDead()
    { 

    }
}
