using UnityEngine;

public interface IInteractable
{
    public Vector2 Pos { get; }
    public void OnBecomeInteractable();
    public void OnStopInteractable();
    public void Interact();
}