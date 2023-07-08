using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorSwitch : MonoBehaviour, IInteractable
{
    SpriteRenderer _renderer;

    [SerializeField]
    Color _default;
    [SerializeField]
    Color _interactable;
    [SerializeField]
    Color _interact;

    public Vector2 Pos { get { return transform.position; } }

    void Awake()
    {
        TryGetComponent(out _renderer);
        _renderer.color = _default;
    }

    public void OnBecomeInteractable()
    {
        _renderer.color = _interactable;
    }
    
    public void OnStopInteractable()
    {
        _renderer.color = _default;
    }
    
    public void Interact()
    {
        _renderer.color = _interact;
    }
}