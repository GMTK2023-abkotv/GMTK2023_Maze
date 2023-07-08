using System;
using System.Collections.Generic;

using UnityEngine;

public static class GameDelegatesContainer
{
    public static Action Start;

    public static Func<Vector3Int> GetPlayerPos;
    
    public static Func<List<List<bool>>> GetMaze;
    public static Func<Grid> GetGrid;
    public static Func<int> GetGridOffset;
}