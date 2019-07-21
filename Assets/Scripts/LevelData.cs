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
    public Vector3 CameraRotation;
    public bool Orthographic;
    public Rect bounds;

    public static bool Started = false;
    public GameObject HideOnClick;
    public Image FadeOnClick;
    public Button Button;

    public void OnClick()
    {
        Started = true;
        StartCoroutine(nameof(Hide));
    }

    IEnumerator Hide()
    {
        this.Button.enabled = false;

        float f = 1f;
        while ((f = Mathf.Clamp(f - Time.deltaTime * 2f, 0, 1)) > 0)
        {
            Color c = FadeOnClick.color;
            c.a = f;
            FadeOnClick.color = c;
            yield return null;
        }

        yield return null;

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
        Debug.Log(Camera.main);
        Transform t = cam.transform;
        t.position = CameraPosition;
        t.rotation = Quaternion.Euler(CameraRotation);

        cam.orthographic = Orthographic;
        cam.orthographicSize = (bounds.width > bounds.height) ? bounds.width/2 : bounds.height/2;
    }

}
