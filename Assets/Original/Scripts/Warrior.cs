
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;
using Unity.Collections;
using Unity.Mathematics;

public class Warrior : MonoBehaviour
{
    const int initialCapacity = 100;

    int movesCount;
    List<int2> moves;
    List<int2> tempMoves;

    List<List<bool>> walkable;
    List<List<bool>> visited;

    int2 pos;
    int minDistance = int.MaxValue;
    int distance = -1;
    int2 targetPos;

    Grid grid;
    int offset;
    int2 dims;

    bool found = false;

    void Awake()
    {
        moves = new(initialCapacity);
        tempMoves = new(initialCapacity);
    }

    void Start()
    {
        walkable = GameDelegatesContainer.GetMaze();
        visited = new(walkable.Count);
        for (int i = 0; i < walkable.Count; i++)
        {
            visited.Add(new(walkable[i].Count));
            for (int j = 0; j < walkable[i].Count; j++)
            {
                visited[i].Add(false);
            }
        }

        grid = GameDelegatesContainer.GetGrid();
        offset = GameDelegatesContainer.GetGridOffset();
        dims = new int2(walkable[0].Count - offset, walkable.Count - offset);
        var index = grid.WorldToCell(transform.position);
        pos = new int2(index.x, index.y);

        DebugMoves();
    }


    void RecalculatePath(int2 playerPos)
    {
        targetPos = playerPos;
        Debug.Log("Searching for " + targetPos);
        ResetVisited();

        minDistance = int.MaxValue;
        tempMoves.Clear();
        StartCoroutine(PathCoroutine(pos + new int2(1, 0), 0));
        // tempMoves.Clear();
        StartCoroutine(PathCoroutine(pos + new int2(-1, 0), 1));
        // tempMoves.Clear();
        StartCoroutine(PathCoroutine(pos + new int2(0, 1), 2));
        // tempMoves.Clear();
        StartCoroutine(PathCoroutine(pos + new int2(0, -1), 3));
    }

    void Path(int2 gridPos)
    {
        int2 index = gridPos + offset;
        if (gridPos.x < -dims.x || gridPos.x >= dims.x || gridPos.y < -dims.y || gridPos.y >= dims.y ||
            !walkable[index.y][index.x] || visited[index.y][index.x])
        {
            return;
        }

        if (math.all(gridPos == targetPos))
        {
            if (tempMoves.Count < minDistance)
            {
                minDistance = tempMoves.Count;
                CopyMoves();
                return;
            }
        }

        tempMoves.Add(gridPos);
        visited[index.y][index.x] = true;

        Path(gridPos + new int2(1, 0));
        Path(gridPos + new int2(-1, 0));
        Path(gridPos + new int2(0, 1));
        Path(gridPos + new int2(0, -1));

        tempMoves.RemoveAt(tempMoves.Count - 1);
    }

    IEnumerator PathCoroutine(int2 gridPos, int prev)
    {
        if (found)
        {
            yield break;
        }

        int2 index = gridPos + offset;
        // Debug.Log(gridPos + " " + index);
        bool outOfBound = index.x < 0 || index.x >= walkable[0].Count  || index.y < 0 || index.y >= walkable.Count;
        bool isWalkable = outOfBound || walkable[index.y][index.x];
        bool isVisited = outOfBound || visited[index.y][index.x];
        if (outOfBound || !isWalkable || isVisited)
        {
            // Debug.Log($"s1 {index} outOfBound {outOfBound} isWalkable {isWalkable} isVisited {isVisited}");
            yield break;
        }

        if (math.all(gridPos == targetPos))
        {
            Debug.Log("m<");
            found = true;
            if (tempMoves.Count < minDistance)
            {
                Debug.Log("m<");
                minDistance = tempMoves.Count;
                CopyMoves();
                found = true;
                yield break;
            }
        }

        Mark(gridPos, grid);
        yield return new WaitForSeconds(waitTime);

        tempMoves.Add(gridPos);
        visited[index.y][index.x] = true;

        if (prev != 1) StartCoroutine(PathCoroutine(gridPos + new int2(1, 0), 0));
        if (prev != 0) StartCoroutine(PathCoroutine(gridPos + new int2(-1, 0), 1));
        if (prev != 3) StartCoroutine(PathCoroutine(gridPos + new int2(0, 1), 2));
        if (prev != 2) StartCoroutine(PathCoroutine(gridPos + new int2(0, -1), 3));

        tempMoves.RemoveAt(tempMoves.Count - 1);
    }

    public static void Mark(int2 gridPos, Grid grid)
    {
        Vector3Int index3d = new(gridPos.x, gridPos.y);
        var d = grid.GetCellCenterWorld(index3d);
        d.z = -1;
        Debug.DrawRay(d + Vector3.down * 0.375f, Vector3.up * 0.75f, Color.red, 100);
        Debug.DrawRay(d + Vector3.left * 0.375f, Vector3.right * 0.75f, Color.red, 100);
    }

    void CopyMoves()
    {
        moves.Clear();
        for (int i = 0; i < tempMoves.Count; i++)
        {
            moves.Add(tempMoves[i]);
        }
    }

    void ResetVisited()
    {
        for (int i = 0; i < visited.Count; i++)
        {
            for (int j = 0; j < visited[i].Count; j++)
            {
                visited[i][j] = false;
            }
        }
    }

    [ContextMenu("DeugMoves")]
    public void DebugMoves()
    {
        // for (int i = 0; i < walkable.Count; i++)
        // {
        //     for (int j = 0; j < walkable[i].Count; j++)
        //     {
        //         if (walkable[i][j])
        //             Mark(new int2(j - offset, i - offset ), grid);
        //     }
        // }
        RecalculatePath(new int2(0, 100));
        StartCoroutine(Draw());

        // Debug.Log($"{walkable[2][11]} {walkable[0][11]} {walkable[1][12]} {walkable[1][10]}");
        // Debug.Log($"{walkable[1][11]} {walkable[0][10]} {walkable[0][12]} {walkable[2][10]} {walkable[2][12]}");
        // Debug.Log($"{walkable[1][13]} {walkable[1][9]} {walkable[1][8]} {walkable[1][14]}");
        // Mark(new int2(0, 1), grid);
        // Mark(new int2(0, 0), grid);
    }

    float waitTime = 0.1f;
    IEnumerator Draw()
    {
        for (int i = 0; i < moves.Count; i++)
        {
            int2 m = moves[i] + offset;
            Vector3Int index = new(m.x, m.y);
            var pos = grid.GetCellCenterWorld(index);  
            Debug.DrawRay(pos, Vector3.back, Color.red, 10);
            Debug.Log("d");
            yield return new WaitForSeconds(waitTime);
        }
    }
}