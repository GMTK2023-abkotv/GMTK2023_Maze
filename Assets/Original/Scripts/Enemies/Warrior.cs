
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;
using Unity.Mathematics;

public class Warrior : MotionController
{
    PathFinding pathFinding;
    int moveIndex;
    int2 currentMove;
    bool isWithTreasure;

    public void Initialize(int2 pos)
    {
        pathFinding = new();
        pathFinding.SetPosition(pos);

        var playerPos = GameDelegatesContainer.GetTreausreChestPos();
        moveIndex = 0;
        pathFinding.RecalculatePath(new int2(playerPos.x, playerPos.y));
        currentMove = pos;

        GameDelegatesContainer.TimeStep += OnTimeStep;
        GameDelegatesContainer.MapChange += OnMapChange;
    }

    public bool IsOn(Vector3Int gridPos)
    {
        if (currentMove.x == gridPos.x && currentMove.y == gridPos.y)
        {
            return true;
        }

        return false;
    }

    void OnDestroy()
    {
        GameDelegatesContainer.TimeStep -= OnTimeStep;
        GameDelegatesContainer.MapChange -= OnMapChange;
    }

    void OnMapChange()
    {
        var targetPos = GameDelegatesContainer.GetTreausreChestPos();
        RecalculatePath(targetPos);
    }

    void RecalculatePath(Vector3Int targetPos)
    {
        moveIndex = 0;
        pathFinding.SetPosition(currentMove);
        pathFinding.RecalculatePath(new int2(targetPos.x, targetPos.y));
    }

    void OnTimeStep()
    {
        bool hasMove = pathFinding.GetMove(moveIndex++, ref currentMove);
        if (!hasMove)
        {
            if (!isWithTreasure)
            { 
                isWithTreasure = true;
                GetComponent<SpriteRenderer>().color = Color.magenta;
                var exit = GameDelegatesContainer.GetExit();
                Debug.Log("the exit is " + exit);
                RecalculatePath(exit);
            }
            else
            {
                GameDelegatesContainer.End();
            }

            return;
        }
        
        pathFinding.SetPosition(currentMove);
        Vector3Int move = new Vector3Int(currentMove.x, currentMove.y, 0);
        var pos = GameDelegatesContainer.GetCellWorldPos(move);
        start = transform.position;
        end = pos;
        lerp = 0;
    }
}