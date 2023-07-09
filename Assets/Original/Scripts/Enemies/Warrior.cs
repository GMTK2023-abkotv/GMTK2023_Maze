
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;
using Unity.Mathematics;

public class Warrior : MotionController
{
    PathFinding pathFinding;

    Animator animator;
    int Animation_idle;
    int Animation_move;
    int Animation_attack;
    int Animation_die;
    int Animation_hit;

    int moveIndex;
    int2 currentMove;
    bool isWithTreasure;

    [SerializeField]
    int stepsBeforeSkip = 3;

    [SerializeField]
    int proximitySound = 4;

    [SerializeField]
    Sprite DeadSprite;

    int currentStepsBeforeSkip;

    public void Initialize(int2 pos)
    {
        animator = GetComponent<Animator>();

        Animation_idle = Animator.StringToHash("Player_idle");
        Animation_move = Animator.StringToHash("Player_move");
        Animation_attack = Animator.StringToHash("Player_attack");
        Animation_hit = Animator.StringToHash("Player_hit");
        Animation_die = Animator.StringToHash("Player_die");

        pathFinding = new();
        pathFinding.SetPosition(pos);

        var playerPos = GameDelegatesContainer.GetTreausreChestPos();
        moveIndex = 0;
        pathFinding.RecalculatePath(new int2(playerPos.x, playerPos.y));
        currentMove = pos;

        GameDelegatesContainer.TimeStep += OnTimeStep;
        GameDelegatesContainer.MapChange += OnMapChange;
        GameDelegatesContainer.GetEnemyPos += GetEnemyPos;
        GameDelegatesContainer.EnemySteppedOnPlayer += OnStepOnPlayer;
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
        GameDelegatesContainer.GetEnemyPos -= GetEnemyPos;
        GameDelegatesContainer.EnemySteppedOnPlayer -= OnStepOnPlayer;
    }

    int2 GetEnemyPos()
    {
        return currentMove;
    }

    void OnStepOnPlayer()
    {
        Debug.Log("Onsametile");
        GameDelegatesContainer.EnemySteppedOnPlayer += OnStepOnPlayer;
        if (animator) animator.Play(Animation_die);
        // animator.SetBool("isDead", true);
        // GetComponent<SpriteRenderer>().sprite = DeadSprite;
        // We can change the Sprite to the Dead Sprite Manually here..

        GameDelegatesContainer.TimeStep -= OnTimeStep;
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
        if (isWithTreasure && currentStepsBeforeSkip >= stepsBeforeSkip)
        {
            // if (animator) animator.Play(Animation_hit); TODO: This is the wrong 
            currentStepsBeforeSkip = 0;
            return;
        }

        bool hasMove = pathFinding.GetMove(moveIndex, ref currentMove);

        bool nearTreasure = math.abs(currentMove.x - pathFinding.targetPos.x) <= 2 
            && (pathFinding.targetPos.y - currentMove.y >= -1 
                && pathFinding.targetPos.y - currentMove.y <= 2);
        if (nearTreasure && !isWithTreasure)
        {
            isWithTreasure = true;
            currentStepsBeforeSkip = 0;
            // GetComponent<SpriteRenderer>().color = Color.magenta;
            GameDelegatesContainer.CoinTake?.Invoke();
            var exit = GameDelegatesContainer.GetExit();
            Debug.Log("the exit is " + exit);
            RecalculatePath(exit);
            return;
        }

        if (!hasMove && isWithTreasure)
        {
            GameDelegatesContainer.Lose();
            GameDelegatesContainer.TimeStep -= OnTimeStep;
            return;
        }

        var playerPos = GameDelegatesContainer.GetPlayerPos();
        if (Mathf.Abs(playerPos.x - currentMove.x) < proximitySound 
            && Mathf.Abs(playerPos.y - currentMove.y) < proximitySound)
        {
            GameDelegatesContainer.CloseToHero();
        }
        else
        {
            GameDelegatesContainer.FarFromHero();
        }

        if (currentMove.x == playerPos.x && currentMove.y == playerPos.y)
        {
            GameDelegatesContainer.EnemySteppedOnPlayer();
            return;
        }

        moveIndex++;
        currentStepsBeforeSkip++;
        pathFinding.SetPosition(currentMove);
        Vector3Int move = new Vector3Int(currentMove.x, currentMove.y, 0);
        var pos = GameDelegatesContainer.GetCellWorldPos(move);
        start = transform.position;
        end = pos;
        lerp = 0;

        if (animator) animator.Play(Animation_move);
    }
}