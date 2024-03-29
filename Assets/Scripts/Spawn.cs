﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns kiwis. Attack this to tiles that kiwis spawn on or exit from
/// </summary>
public class Spawn : Tile
{
    public int Color;

    public float SpawnDelay;
    public float initDelay;
    public static float initDelayStatic;
    private float _time = 0;

    /*private void Awake()
    {
        initDelayStatic = initDelay;
        _time -= initDelay;
    }*/

    private void Update()
    {
        if (!LevelData.Started) return;

        _time += Time.deltaTime;

        if (_time > SpawnDelay)
        {
            _time -= SpawnDelay;
            ++SpawnQueue;
        }

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0, Vector2.zero, 0f, Kiwi.KiwiFinderMask);
        if (hit.collider == null)
        {
            //Spawn one
            if (RespawnQueue.Count > 0)
            {
                Kiwi kiwi = RespawnQueue.Dequeue();
                Respawn(kiwi);
            }
            else if (SpawnQueue > 0)
            {
                --SpawnQueue;
                Create();
            }
        }
    }

    private int SpawnQueue = 0;
    private Queue<Kiwi> RespawnQueue = new Queue<Kiwi>();

    public void AddToRespawnQueue(Kiwi kiwi)
    {
        kiwi.gameObject.SetActive(false);
        RespawnQueue.Enqueue(kiwi);
    }

    private void Respawn(Kiwi kiwi)
    {
        kiwi.gameObject.SetActive(true);

        //Teleport back to base and reset path
        kiwi.parentTile = this;
        Transform t = kiwi.transform;
        t.parent = transform;
        t.localPosition = Vector2.zero;

        kiwi.SetPath(CreatePath(EntryPoints.CENTER));
    }

    private void Create()
    {
        GameObject prefab = GameManager.ConstructionKiwiPrefab;

        int i = LevelData.AvailableColors[Random.Range(0,LevelData.AvailableColors.Length)];
        //Material mat = GameManager.ConstructionKiwiMaterial;

        GameObject obj = Instantiate(prefab, this.transform);
        Kiwi kiwi = obj.GetComponent<Kiwi>();
        kiwi.parentTile = this;
        kiwi.SpawnLocation = this;

        kiwi.SetPath(CreatePath(EntryPoints.CENTER));
    }
}