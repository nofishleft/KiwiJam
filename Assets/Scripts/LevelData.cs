using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static int[] AvailableColors;
    public int[] Colors;

    // Start is called before the first frame update
    void Start()
    {
        AvailableColors = Colors;
    }
}
