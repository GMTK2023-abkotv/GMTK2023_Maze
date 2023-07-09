using System;
using System.Collections.Generic;

using Unity.Mathematics;

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Assertions;

public class TileMapController : MonoBehaviour
{
    [SerializeField]
    Tilemap groundTilemap;
    [SerializeField]
    Tilemap fgTilemap;
    [SerializeField]
    Tilemap bgTilemap;

    [SerializeField]
    Grid grid;

    [SerializeField]
    Vector2Int dims;

    [SerializeField]
    Vector3Int treasurePos;

    [SerializeField]
    List<Transform> entries;

    Vector3Int playerPos;
    List<List<bool>> maze;

    int gridOffset;
    List<int2> exits;

    void Awake()
    {
        GameDelegatesContainer.GetPlayerPos += GetPlayerPos;
        GameDelegatesContainer.GetTreausreChestPos += GetTreasureChestPos;
        GameDelegatesContainer.GetCellWorldPos += GetCellWorldPos;
        GameDelegatesContainer.IsWalkable += IsWalkable;

        GameDelegatesContainer.Start += OnStart;
        GameDelegatesContainer.End += OnEnd;

        GameDelegatesContainer.GetMaze += GetMaze;
        GameDelegatesContainer.GetGrid += GetGrid;
        GameDelegatesContainer.GetGridOffset += GetGridOffset;

        GameDelegatesContainer.GetExit += GetExit;

        exits = new(entries.Count);
        foreach (var entry in entries)
        {
            var entryPos = grid.WorldToCell(entry.position);
            int2 exit = new int2(entryPos.x, entryPos.y);
            exits.Add(exit);
        }
    }

    void OnDestroy()
    {
        GameDelegatesContainer.GetPlayerPos -= GetPlayerPos;
        GameDelegatesContainer.GetTreausreChestPos -= GetTreasureChestPos;
        GameDelegatesContainer.GetCellWorldPos -= GetCellWorldPos;
        GameDelegatesContainer.IsWalkable -= IsWalkable;

        GameDelegatesContainer.Start -= OnStart;
        GameDelegatesContainer.End -= OnEnd;

        GameDelegatesContainer.GetMaze -= GetMaze;
        GameDelegatesContainer.GetGrid -= GetGrid;
        GameDelegatesContainer.GetGridOffset -= GetGridOffset;

        GameDelegatesContainer.GetExit -= GetExit;
    }

    Vector3Int GetPlayerPos()
    {
        return playerPos;
    }

    Vector3Int GetTreasureChestPos()
    {
        return new Vector3Int(0, 0, 0);
    }

    void OnStart()
    {
        PlayerDelegatesContainer.EventMoveCommand += OnMoveCommand;
    }

    void OnEnd()
    {
        PlayerDelegatesContainer.EventMoveCommand -= OnMoveCommand;
    }

    Grid GetGrid()
    {
        return grid;
    }

    int GetGridOffset()
    {
        return gridOffset;
    }

    Vector3Int GetExit()
    {
        int2 n = new int2((int)math.sign(-playerPos.x), (int)math.sign(-playerPos.y));
        int p = 0;
        int mp = -1;
        int index = -1;
        for (int i = 0; i < exits.Count; i++)
        { 
            if (math.sign(exits[i].x) == n.x) p++;
            if (math.sign(exits[i].y) == n.y) p++;
            if (p > mp) 
            {
                mp = p;
                index = i;
            }
        }
        return new Vector3Int(exits[index].x, exits[index].y);
    }

    List<List<bool>> GetMaze()
    {
        gridOffset = dims.x / 2;
        Assert.IsTrue(gridOffset == dims.y / 2);

        maze = new(dims.y);
        for (int i = 0; i < dims.y; i++)
        {
            maze.Add(new(dims.x));
            for (int j = 0; j < dims.x; j++)
            {
                Vector3Int pos = new(j - gridOffset, i - gridOffset, 0);
                maze[i].Add(IsWalkable(pos));
            }
        }

        return maze;
    }

    void OnMoveCommand(MoveCommand moveCommand)
    {
        if (PlayerDelegatesContainer.IsMoving())
        {
            return;
        }

        var newPlayerPos = playerPos + moveCommand.Direction;
        TileTypes tileTypeToWalkOn = GetTileType(newPlayerPos); // it actually means newPlayerPos

        Debug.Log(newPlayerPos);
        Debug.Log(tileTypeToWalkOn);

        // Try Moving to the Position Requested Tile.
        var newPos = grid.GetCellCenterWorld(newPlayerPos);
        bool moved = PlayerDelegatesContainer.NewMoveDestination(newPos, tileTypeToWalkOn);
        GameDelegatesContainer.TimeStep();
        if (moved) playerPos = newPlayerPos;
    }

    TileTypes GetTileType(Vector3Int pos)
    {

        // This Layer has the walls
        var tile = bgTilemap.GetTile(pos);
        if (tile) {
            return TileTypes.Wall;
        }

        // these are walkable
        tile = fgTilemap.GetTile(pos);
        if (tile) {
            return TileTypes.Walkable;
        }

        tile = groundTilemap.GetTile(pos);
        if (tile) {
            return TileTypes.Walkable;
        }

        return TileTypes.NotATile;
    }

    Vector3 GetCellWorldPos(Vector3Int gridPos)
    {
        return grid.GetCellCenterWorld(gridPos);
    }

    bool IsWalkable(Vector3Int pos)
    {
        var tile = groundTilemap.GetTile(pos);
        if (tile.name == "black")
        {
            return false;
        }

        return true;
    }

    public static void Mark(int2 gridPos, Grid grid)
    {
        Vector3Int index3d = new(gridPos.x, gridPos.y);
        var d = grid.GetCellCenterWorld(index3d);
        d.z = -1;
        Debug.DrawRay(d + Vector3.down * 0.375f, Vector3.up * 0.75f, Color.red, 10f);
        Debug.DrawRay(d + Vector3.left * 0.375f, Vector3.right * 0.75f, Color.red, 10f);
    }
}