using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapController : MonoBehaviour
{
    [SerializeField]
    Tilemap logicTilemap;

    [SerializeField]
    Grid grid;

    Vector3Int player;

    void Awake()
    {
        GameDelegatesContainer.Start += OnStart;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventMoveCommand -= OnMoveCommand;
    }

    void OnStart()
    {
        PlayerDelegatesContainer.EventMoveCommand += OnMoveCommand;
    }

    void OnMoveCommand(MoveCommand moveCommand)
    {
        if (PlayerDelegatesContainer.IsMoving())
        {
            return;
        }

        var newPlayer = player + moveCommand.Direction;
        var tile = logicTilemap.GetTile(newPlayer);
        Debug.Log(tile.name);
        if (tile.name == "black")
        {
            return;
        }

        player = newPlayer;
        var newPos = grid.GetCellCenterWorld(player);
        // var newPos = logicTilemap.GetTransformMatrix(new Vector3Int(player.x, player.y)).GetPosition();
        Debug.Log(newPos);
        PlayerDelegatesContainer.NewMoveDestination(newPos);
    }
}