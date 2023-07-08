using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Assertions;

public class TileMapController : MonoBehaviour
{
    [SerializeField]
    Tilemap logicTilemap;

    [SerializeField]
    Grid grid;

    [SerializeField]
    Vector2Int dims;

    Vector3Int player;
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
        return player;
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

        var newPlayer = player + moveCommand.Direction;
        if (!IsWalkable(newPlayer))
        {
            return;
        }

        player = newPlayer;
        var newPos = grid.GetCellCenterWorld(player);
        // var newPos = logicTilemap.GetTransformMatrix(new Vector3Int(player.x, player.y)).GetPosition();
        PlayerDelegatesContainer.NewMoveDestination(newPos);
    }

    bool IsWalkable(Vector3Int pos)
    {
        var tile = logicTilemap.GetTile(pos);
        if (tile.name == "black")
        {
            return false;
        }

        return true;
    }
}