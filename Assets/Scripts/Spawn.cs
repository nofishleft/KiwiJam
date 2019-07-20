using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns kiwis. Attack this to tiles that kiwis spawn on or exit from
/// </summary>
public class Spawn : Tile
{
    public int Color;

    public float SpawnDelay;
    private float _time;

    private void Update()
    {
        _time += Time.deltaTime;

        if (_time > SpawnDelay)
        {
            _time -= SpawnDelay;
            //Create();
        }
    }

    public void Create(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, this.transform);
        Kiwi kiwi = obj.GetComponent<Kiwi>();
        kiwi.parentTile = this;
    }
}