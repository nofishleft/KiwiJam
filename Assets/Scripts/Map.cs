using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Tile[,] map;
    public int width;
    public int height;
    // Start is called before the first frame update
    void Awake()
    {
        map = new Tile[width, height];

        Tile[] tiles = GameObject.FindObjectsOfType<Tile>();

        foreach (var tile in tiles)
        {
            Vector3 pos = tile.transform.position;
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);
            map[x, y] = tile;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
