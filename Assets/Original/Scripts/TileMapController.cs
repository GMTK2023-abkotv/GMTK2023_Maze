using System;
using System.Collections.Generic;

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

    Vector3Int playerPos;
    List<List<bool>> maze;

    int gridOffset;

    void Awake()
    {
        GameDelegatesContainer.GetPlayerPos += GetPlayerPos;

        GameDelegatesContainer.Start += OnStart;
        GameDelegatesContainer.GetMaze += GetMaze;
        GameDelegatesContainer.GetGrid += GetGrid;
        GameDelegatesContainer.GetGridOffset += GetGridOffset;
    }

    void OnDestroy()
    {
        GameDelegatesContainer.GetPlayerPos -= GetPlayerPos;

        PlayerDelegatesContainer.EventMoveCommand -= OnMoveCommand;
        GameDelegatesContainer.GetMaze -= GetMaze;
        GameDelegatesContainer.GetGrid -= GetGrid;
        GameDelegatesContainer.GetGridOffset -= GetGridOffset;
    }

    Vector3Int GetPlayerPos()
    {
        return playerPos;
    }

    void OnStart()
    {
        PlayerDelegatesContainer.EventMoveCommand += OnMoveCommand;
    }

    Grid GetGrid()
    {
        return grid;
    }

    int GetGridOffset()
    {
        return gridOffset;
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
        TileTypes tileTypeToWalkOn = getTileType(newPlayerPos); // it actually means newPlayerPos

        // Try Moving to the Position Requested Tile.
        var newPos = grid.GetCellCenterWorld(newPlayerPos);
        bool moved = PlayerDelegatesContainer.NewMoveDestination(newPos, tileTypeToWalkOn);
        if (moved) playerPos = newPlayerPos;
    }

    TileTypes getTileType(Vector3Int pos)
    {
        // we need to check multiple tileMaps !
        // TileTypes tileType;


        // to simulate a priority for different TileMaps
        // Ground has lowest Priority, EnemyLayer would have most ig
        // Layers can be "Layered"/"Nested" so we need to check for the tile using a priority manner
        
        // This Layer has the walls
        var tile = bgTilemap.GetTile(pos);
        // if (tile && tile.name == "black") {
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

    bool IsWalkable(Vector3Int pos)
    {
        var tile = groundTilemap.GetTile(pos);
        if (tile.name == "black")
        {
            return false;
        }

        return true;
    }
}