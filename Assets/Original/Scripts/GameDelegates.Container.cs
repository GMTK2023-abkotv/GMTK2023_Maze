using System;
using System.Collections.Generic;

using Unity.Mathematics;
using UnityEngine;

public static class GameDelegatesContainer
{
    public static Action Start;
    public static Action Lose;
    public static Action Win;

    public static Action<Vector3Int, Transform> PlayerSpawn;
    public static Func<Vector3Int> GetPlayerPos;
    public static Func<Vector3Int> GetTreausreChestPos;

    public static Action EnemySteppedOnPlayer;

    public static Action CoinTake;
    public static Func<Vector3Int> GetExit;
    public static Action MapChange;
    
    public static Action TimeStep;
    public static Func<int> GetCurrentTime;

    public static Func<Vector3Int, Vector3> GetCellWorldPos;

    public static Func<List<List<bool>>> GetMaze;
    public static Func<Grid> GetGrid;
    public static Func<int> GetGridOffset;
}