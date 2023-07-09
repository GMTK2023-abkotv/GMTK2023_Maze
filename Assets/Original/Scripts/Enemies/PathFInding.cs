using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PathFinding
{
    const int initialCapacity = 100;

    List<int2> moves;
    

    List<List<bool>> walkable;
    List<List<bool>> visited;

    int2 currentPos; // in grid coordinates
    public int2 targetPos; // in grid coordinates

    bool found = false;

    int2 bottom = new int2(0, -1);
    int2 left = new int2(-1, 0);
    int2 top = new int2(0, 1);
    int2 right = new int2(1, 0);

    int offset;

    public PathFinding()
    {
        moves = new(initialCapacity);

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

        offset = GameDelegatesContainer.GetGridOffset();
    }

    public void SetPosition(int2 newPos)
    {
        currentPos = newPos;
    }

    public bool GetMove(int moveIndex, ref int2 move)
    {
        if (moveIndex < moves.Count)
        {
            move = moves[moveIndex];
            return true;
        }

        return false;
    }

    public void RecalculatePath(int2 playerPos)
    {
        targetPos = playerPos;
        ResetBuffers();

        InitialPath();
    }

    void InitialPath()
    {
        int2x4 priority = int2x4.zero;
        int2 delta = targetPos - currentPos;
        if (math.abs(delta.x) > math.abs(delta.y))
        {
            int2x2 h = delta.y > 0 ? new int2x2(top, bottom) : new int2x2(bottom, top);
            priority = delta.x > 0 ? new int2x4(right, h.c0, h.c1, left) : new int2x4(left, h.c0, h.c1, right);
        }
        else
        {
            int2x2 v = delta.x > 0 ? new int2x2(right, left) : new int2x2(left, right);
            priority = delta.y > 0 ? new int2x4(top, v.c0, v.c1, bottom) : new int2x4(bottom, v.c0, v.c1, top);
        }

        Path(currentPos + priority[0], priority[0]);
        Path(currentPos + priority[1], priority[1]);
        Path(currentPos + priority[2], priority[2]);
        Path(currentPos + priority[3], priority[3]);
    }

    void Path(int2 gridPos, int2 prev)
    {
        if (found)
        {
            return;
        }

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
            moves.Add(gridPos);
            found = true;
            return;
        }

        moves.Add(gridPos);
        visited[index.y][index.x] = true;

        int2x3 priority = int2x3.zero;
        int2 delta = targetPos - gridPos;
        if (math.abs(delta.x) > math.abs(delta.y))
        {
            priority = delta.x > 0 ? ToRight(prev, delta.y) : ToLeft(prev, delta.y);
        }
        else
        {
            priority = delta.y > 0 ? ToTop(prev, delta.x) : ToBottom(prev, delta.x);
        }

        Path(gridPos + priority[0], priority[0]);
        Path(gridPos + priority[1], priority[1]);
        Path(gridPos + priority[2], priority[2]);

        if (!found)
        {
            moves.RemoveAt(moves.Count - 1);
        }
    }

    int2x3 ToTop(int2 prev, int delta)
    {
        int2x2 h = delta > 0 ? new int2x2(right, left) : new int2x2(left, right);
        int2 hp = math.all(prev == left) ? left : right;
        if (math.all(prev == top))
        {
            return new int2x3(top, h.c0, h.c1);
        }
        else if (math.all(prev == bottom))
        {
            return new int2x3(h.c0, h.c1, bottom);
        }
        else
        {
            return new int2x3(top, hp, bottom);
        }
    }

    int2x3 ToBottom(int2 prev, int delta)
    {
        int2x2 h = delta > 0 ? new int2x2(right, left) : new int2x2(left, right);
        int2 hp = math.all(prev == left) ? left : right;
        if (math.all(prev == bottom))
        {
            return new int2x3(bottom, h.c0, h.c1);
        }
        else if (math.all(prev == top))
        {
            return new int2x3(h.c0, h.c1, top);
        }
        else
        {
            return new int2x3(bottom, hp, top);
        }
    }

    int2x3 ToRight(int2 prev, int delta)
    {
        int2x2 v = delta > 0 ? new int2x2(top, bottom) : new int2x2(bottom, top);
        int2 vp = math.all(prev == bottom) ? bottom : top;
        if (math.all(prev == right))
        {
            return new int2x3(right, v.c0, v.c1);
        }
        else if (math.all(prev == left))
        {
            return new int2x3(v.c0, v.c1, left);
        }
        else
        {
            return new int2x3(right, vp, left);
        }
    }

    int2x3 ToLeft(int2 prev, int delta)
    {
        int2x2 v = delta > 0 ? new int2x2(top, bottom) : new int2x2(bottom, top);
        int2 vp = math.all(prev == bottom) ? bottom : top;
        if (math.all(prev == left))
        {
            return new int2x3(left, v.c0, v.c1);
        }
        else if (math.all(prev == right))
        {
            return new int2x3(v.c0, v.c1, right);
        }
        else
        {
            return new int2x3(left, vp, right);
        }
    }

    void ResetBuffers()
    {
        for (int i = 0; i < visited.Count; i++)
        {
            for (int j = 0; j < visited[i].Count; j++)
            {
                visited[i][j] = false;
            }
        }

        moves.Clear();
        found = false;
    }
}

/*
    IEnumerator PathCoroutine(int2 gridPos, int2 prev)
    {
        if (found)
        {
            yield break;
        }

        int2 index = gridPos + offset;
        bool outOfBound = index.x < 0 || index.x >= walkable[0].Count || index.y < 0 || index.y >= walkable.Count;
        bool isWalkable = outOfBound || walkable[index.y][index.x];
        bool isVisited = outOfBound || visited[index.y][index.x];
        if (outOfBound || !isWalkable || isVisited)
        {
            // Debug.Log($"s1 {index} outOfBound {outOfBound} isWalkable {isWalkable} isVisited {isVisited}");
            yield break;
        }

        if (math.all(gridPos == targetPos))
        {
            Debug.Log("found");
            found = true;
            yield break;
        }

        Mark(gridPos, grid);
        yield return new WaitForSeconds(waitTime);

        moves.Add(gridPos);
        visited[index.y][index.x] = true;

        int2x3 priority = int2x3.zero;
        int2 delta = targetPos - gridPos;
        if (math.abs(delta.x) > math.abs(delta.y))
        {
            priority = delta.x > 0 ? ToRight(prev, delta.y) : ToLeft(prev, delta.y);
        }
        else
        {
            priority = delta.y > 0 ? ToTop(prev, delta.x) : ToBottom(prev, delta.x);
        }

        yield return StartCoroutine(PathCoroutine(gridPos + priority[0], priority[0]));
        yield return StartCoroutine(PathCoroutine(gridPos + priority[1], priority[1]));
        yield return StartCoroutine(PathCoroutine(gridPos + priority[2], priority[2]));

        if (!found)
        {
            moves.RemoveAt(moves.Count - 1);
        }
    }

    // Grid grid;
    // int2 dims;

    // grid = GameDelegatesContainer.GetGrid();
    //     offset = GameDelegatesContainer.GetGridOffset();
    //     dims = new int2(walkable[0].Count - offset, walkable.Count - offset);
    // var index = grid.WorldToCell(transform.position);
    // currentPos = new int2(index.x, index.y);

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
*/