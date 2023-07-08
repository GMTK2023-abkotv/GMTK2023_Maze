using System;
using UnityEngine;

public static class PlayerDelegatesContainer
{
    public static Action<MoveCommand> EventMoveCommand;
    public static Func<bool> IsMoving;

    public static Action<Vector2> NewMoveDestination;
    public static Action EventInteractCommand;

    public static Action EventMove; // for particles, visuals, sounds
    public static Action EventJump;
    public static Action EventDash;
}