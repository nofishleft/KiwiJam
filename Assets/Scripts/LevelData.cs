using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Store all the levels specific data
/// - Where the camera will go
/// - What colors will be used
/// </summary>
public class LevelData : MonoBehaviour
{
    public static int[] AvailableColors;
    public int[] Colors;

    public Vector3 CameraPosition;
    public Quaternion CameraRotation;
    public bool Orthographic;
    public Rect bounds;

    public static bool Started = false;
    public GameObject HideOnClick;

    public void OnClick()
    {
        Started = true;
        StartCoroutine(nameof(Hide));
    }

    IEnumerator Hide()
    {
        GetComponentInChildren<Button>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        HideOnClick.SetActive(false);
    }

    private void Awake()
    {
        Started = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Started = false;
        AvailableColors = Colors;
        Camera cam = Camera.main;
        Transform t = cam.transform;
        t.position = CameraPosition;
        t.rotation = CameraRotation;

        cam.orthographic = Orthographic;
        cam.orthographicSize = (bounds.width > bounds.height) ? bounds.width/2 : bounds.height/2;
    }

}
