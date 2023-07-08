using System;

public static class InteractableDelegatesContainer
{
    public static Action<IInteractable> EventInteractableEnter;
    public static Action<IInteractable> EventInteractableExit;
}