using UnityEngine;
using UnityEngine.Assertions;

public class PlayerInteract : MonoBehaviour
{
    IInteractable _current;

    void Awake()
    {
        PlayerDelegatesContainer.EventInteractCommand += OnInteractCommand;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventInteractCommand -= OnInteractCommand;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.Interactable)
        {
            Assert.IsNull(_current);
            InteractableDelegatesContainer.EventInteractableEnter?.Invoke(_current);
            _current = other.GetComponent<IInteractable>();
            _current.OnBecomeInteractable();
        }
    }

    public void OnInteractCommand()
    {
        if (_current != null)
        {
            _current.Interact();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.Interactable)
        {
            Assert.IsNotNull(_current);
            InteractableDelegatesContainer.EventInteractableExit?.Invoke(_current);
            _current.OnStopInteractable();
            _current = null;
        }
    }
}