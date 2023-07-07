using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField]
    Collider _trigger;

    [SerializeField]
    Collider _collider;

    void Awake()
    { 
        PlayerDelegatesContainer.EventPlayerDead  += OnDead;
        PlayerDelegatesContainer.EventPlayerAlive += OnAlive;
    }
     
    void OnDestroy()
    { 
        PlayerDelegatesContainer.EventPlayerDead  -= OnDead;
        PlayerDelegatesContainer.EventPlayerAlive -= OnAlive;
    }

    void OnDead()
    {
        _trigger.enabled = false;
        _collider.enabled = false;
    }

    void OnAlive()
    {
        _trigger.enabled = true;
        _collider.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.Death)
        {
            PlayerDelegatesContainer.EventPlayerDead();
            return;
        }

        if (other.gameObject.layer == Layers.Holes)
        {
            _collider.enabled = false;
            return;
        }

        if (other.gameObject.layer == Layers.Enemies)
        {
            _collider.enabled = false;
            PlayerDelegatesContainer.EventPlayerCapture();
        }
    }
}