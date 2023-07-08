
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

        StartCoroutine(Pathing());
    }

    void Path(int2 gridPos, int2 prev)
    {
        int2 index = gridPos + offset;
        bool outOfBound = index.x < 0 || index.x >= walkable[0].Count || index.y < 0 || index.y >= walkable.Count;
        bool isWalkable = outOfBound || walkable[index.y][index.x];
        bool isVisited = outOfBound || visited[index.y][index.x];
        if (outOfBound || !isWalkable || isVisited)
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

        int2x3 priority = int2x3.zero;
        int2 delta = targetPos - gridPos;
        if (math.abs(delta.x) > math.abs(delta.y))
        {
            priority = delta.x > 0 ? Left(prev) : Right(prev);
        }
        else
        {
            priority = delta.y > 0 ? Bottom(prev) : Top(prev);
        }

        Path(gridPos + priority[0], priority[0]);
        Path(gridPos + priority[1], priority[1]);
        Path(gridPos + priority[2], priority[2]);

        tempMoves.RemoveAt(tempMoves.Count - 1);
    }

    IEnumerator Pathing()
    {
        int2x4 priority = int2x4.zero;
        int2 delta = targetPos - pos;
        if (math.abs(delta.x) < math.abs(delta.y))
        {
            priority = delta.x > 0 ? new int2x4(left, top, bottom, right) : new int2x4(right, bottom, top, left);
        }
        else
        {
            priority = delta.y > 0 ? new int2x4(bottom, left, right, top) : new int2x4(top, right, left, bottom);
        }

        tempMoves.Clear();
        yield return StartCoroutine(PathCoroutine(pos + priority[0], priority[0]));
        yield return StartCoroutine(PathCoroutine(pos + priority[1], priority[1]));
        yield return StartCoroutine(PathCoroutine(pos + priority[2], priority[2]));
        yield return StartCoroutine(PathCoroutine(pos + priority[3], priority[3]));
    }

    IEnumerator PathCoroutine(int2 gridPos, int2 prev)
    {
        Debug.Log(gridPos + " " + prev);
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
            // found = true;
            if (tempMoves.Count < minDistance)
            {
                Debug.Log("m<");
                minDistance = tempMoves.Count;
                CopyMoves();
                // found = true;
                yield break;
            }
        }

        Mark(gridPos, grid);
        yield return new WaitForSeconds(waitTime);

        tempMoves.Add(gridPos);
        visited[index.y][index.x] = true;

        int2x3 priority = int2x3.zero;
        int2 delta = targetPos - gridPos;
        if (math.abs(delta.x) > math.abs(delta.y))
        {
            priority = delta.x > 0 ? Right(prev) : Left(prev);
        }
        else 
        {
            priority = delta.y > 0 ? Top(prev) : Bottom(prev);
        }

        yield return StartCoroutine(PathCoroutine(gridPos + priority[0], priority[0]));
        yield return StartCoroutine(PathCoroutine(gridPos + priority[1], priority[1]));
        yield return StartCoroutine(PathCoroutine(gridPos + priority[2], priority[2]));

        tempMoves.RemoveAt(tempMoves.Count - 1);
    }

    int2x3 Top(int2 prev)
    {
        if (math.all(prev == bottom))
        {
            return new int2x3(left, right, top);
        }
        else if (math.all(prev == left))
        {
            return new int2x3(bottom, right, top);
        }
        else if (math.all(prev == right))
        {
            return new int2x3(bottom, left, top);
        }
        else
        { 
            return new int2x3(bottom, left, right);
        }
    }

    int2x3 Right(int2 prev)
    {
        if (math.all(prev == left))
        {
            return new int2x3(top, bottom, right);
        }
        else if (math.all(prev == top))
        {
            return new int2x3(left, bottom, right);
        }
        else if (math.all(prev == bottom))
        {
            return new int2x3(left, top, right);
        }
        else
        {
            return new int2x3(left, top, bottom);
        }
    }

    int2x3 Bottom(int2 prev)
    {
        if (math.all(prev == top))
        {
            return new int2x3(right, left, bottom);
        }
        else if (math.all(prev == right))
        {
            return new int2x3(top, left, bottom);
        }
        else if (math.all(prev == left))
        {
            return new int2x3(top, right, bottom);
        }
        else
        {
            return new int2x3(top, right, left);
        }
    }
    
    int2x3 Left(int2 prev)
    {
        if (math.all(prev == right))
        {
            return new int2x3(bottom, top, left);
        }
        else if (math.all(prev == bottom))
        {
            return new int2x3(right, top, left);
        }
        else if (math.all(prev == top))
        {
            return new int2x3(right, bottom, left);
        }
        else
        {
            return new int2x3(right, bottom, top);
        }
    }

    int2 bottom = new int2(0, -1);
    int2 left = new int2(-1, 0);
    int2 top = new int2(0, 1);
    int2 right = new int2(1, 0);

    public static void Mark(int2 gridPos, Grid grid)
    {
        Vector3Int index3d = new(gridPos.x, gridPos.y);
        var d = grid.GetCellCenterWorld(index3d);
        d.z = -1;
        Debug.DrawRay(d + Vector3.down * 0.375f, Vector3.up * 0.75f, Color.red, 10f);
        Debug.DrawRay(d + Vector3.left * 0.375f, Vector3.right * 0.75f, Color.red, 10f);
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
        RecalculatePath(new int2(0, 0));
        StartCoroutine(Draw());
    }

    float waitTime = 0.1f;
    IEnumerator Draw()
    {
        for (int i = 0; i < moves.Count; i++)
        {
            Mark(moves[i], grid);
            yield return new WaitForSeconds(waitTime);
        }
    }
}