using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemiesSpawn : MonoBehaviour
{
    [SerializeField]
    int spawnTimeSteps;

    [SerializeField]
    int maxEnemiesCount;

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    Grid grid;

    [SerializeField]
    [Tooltip("Use negative to disable constantSeeding")]
    int constantSeed = -1;

    [SerializeField]
    List<int> spawnPattern;

    int lastSpawnTime;
    List<Vector3Int> spawnPoints;

    int spawnPatternIndex;

    int enemiesCount;
    Unity.Mathematics.Random rnd;

    List<Warrior> warriors;

    void Awake()
    {
        if (constantSeed < 0) rnd = new();
        else rnd = new((uint)constantSeed);

        spawnPoints = new(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 pos = transform.GetChild(i).position;
            spawnPoints.Add(grid.WorldToCell(pos));
        }

        warriors = new(maxEnemiesCount);
        spawnPattern = new(5);
        for (int i = 0; i < 5; i++)
        {
            spawnPattern.Add(rnd.NextInt(0, spawnPoints.Count));
        }

        GameDelegatesContainer.TimeStep += OnTimeStep;
    }

    void OnDestroy()
    {
        GameDelegatesContainer.TimeStep -= OnTimeStep;
    }

    void OnTimeStep()
    {
        int currentTime = GameDelegatesContainer.GetCurrentTime();
        if (enemiesCount >= maxEnemiesCount)
        {
            lastSpawnTime = currentTime;
            return;
        }

        if (currentTime - lastSpawnTime > spawnTimeSteps)
        {
            lastSpawnTime = currentTime;
            Spawn();
        }
    }

    void Spawn()
    {
        enemiesCount++;

        int spawnPatternTraversal = 0;
        int spawnIndex = -1;
        while (spawnPatternTraversal < spawnPattern.Count)
        {
            spawnIndex = spawnPattern[spawnPatternIndex++];
            if (spawnPatternIndex >= spawnPattern.Count) spawnPatternIndex = 0;
            Vector3Int index = spawnPoints[spawnIndex];
            if (IsSpawnPointEmpty(index) && GameDelegatesContainer.IsWalkable(index))
            {
                Vector3 pos = grid.GetCellCenterWorld(index);
                var gb = Instantiate(prefab, pos, Quaternion.identity);
                var warrior = gb.GetComponent<Warrior>();
                warrior.Initialize(new int2(index.x, index.y));
                warriors.Add(warrior);
                Debug.Log("Spawn");
                break;
            }
        }
    }

    bool IsSpawnPointEmpty(Vector3Int gridPos)
    {
        for (int i = 0; i < warriors.Count; i++)
        {
            if (warriors[i].IsOn(gridPos))
            {
                return false;
            }
        }

        return true;
    }
}