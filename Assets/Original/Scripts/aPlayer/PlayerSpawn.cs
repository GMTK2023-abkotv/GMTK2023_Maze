using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    [SerializeField]
    Grid grid;

    [SerializeField]
    [Tooltip("Use negative to disable constantSeeding")]
    int constantSeed = -1;

    Unity.Mathematics.Random rnd;

    void Awake()
    {
        GameDelegatesContainer.Start += OnStart;
    }

    void OnDestroy()
    {
        GameDelegatesContainer.Start -= OnStart;
    }

    void OnStart()
    {
        if (constantSeed < 0) rnd = new((uint)Guid.NewGuid().GetHashCode());
        else rnd = new((uint)constantSeed);

        var spawnIndex = rnd.NextInt(0, transform.childCount);
        var index = grid.WorldToCell(transform.GetChild(spawnIndex).position);
        Vector3 pos = grid.GetCellCenterWorld(index);
        var gb = Instantiate(prefab, pos, Quaternion.identity);
        GameDelegatesContainer.PlayerSpawn(index, gb.transform);
    }
}