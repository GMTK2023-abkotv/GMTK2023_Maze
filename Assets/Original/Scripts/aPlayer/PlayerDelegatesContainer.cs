using System;
using UnityEngine;

public static class PlayerDelegatesContainer
{
    public static Func<Transform> GetTransform;

    public static Action<MoveCommand> EventMoveCommand;

    public static Action EventMove; // for particles, visuals, sounds
    public static Action EventJump;
    public static Action EventDash;

    public static Action EventPlayerAlive;
    public static Action EventPlayerDead;
    public static Action EventPlayerCapture;
}