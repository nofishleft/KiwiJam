using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static int[] AvailableColors;
    public int[] Colors;

    public Vector3 CameraPosition;
    public bool Orthographic;
    public Rect bounds;

    // Start is called before the first frame update
    void Start()
    {
        AvailableColors = Colors;
        Camera cam = Camera.main;
        Transform t = cam.transform;
        t.position = CameraPosition;
        t.rotation = Quaternion.identity;

        cam.orthographic = Orthographic;
        cam.orthographicSize = (bounds.width > bounds.height) ? bounds.width/2 : bounds.height/2;
    }

}
