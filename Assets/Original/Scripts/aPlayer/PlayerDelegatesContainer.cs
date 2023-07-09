using System;
using UnityEngine;

public static class PlayerDelegatesContainer
{
    public static Action<MoveCommand> EventMoveCommand;
    public static Func<bool> IsMoving;

    public static Func<Vector2, TileTypes, bool> NewMoveDestination;
    public static Action EventInteractCommand;

    public static Action EventMove; // for particles, visuals, sounds
    public static Action EventJump;
    public static Action EventDash;
}