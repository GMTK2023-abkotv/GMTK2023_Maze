using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WarriorSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    [SerializeField]
    Grid grid;

    [SerializeField]
    [Tooltip("Use negative to disable constantSeeding")]
    int constantSeed = -1;

    Unity.Mathematics.Random rnd;

    void Start()
    {
        if (constantSeed < 0) rnd = new();
        else rnd = new((uint)constantSeed);

        var spawnIndex = rnd.NextInt(0, transform.childCount);
        var index = grid.WorldToCell(transform.GetChild(spawnIndex).position);
        Vector3 pos = grid.GetCellCenterWorld(index);
        var gb = Instantiate(prefab, pos, Quaternion.identity);
        var warrior = gb.GetComponent<Warrior>();
        warrior.Initialize(new int2(index.x, index.y));
    }
}