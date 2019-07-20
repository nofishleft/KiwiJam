using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// Allows you to rotate and swap tiles
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class TileController : MonoBehaviour
{
    public float delay;
    public static float timeStatic;
    public static float timeStaticMax;
    public float SwappingCooldown;

    private InputState _state = InputState.Nothing;

    private MoveableTile _tile;

    public AudioClip[] swapClips;
    public AudioClip[] rotateClips;
    private AudioSource audioSource;

    // Start is called before the first frame update

    void OnLevelWasLoaded()
    {
        StopAllCoroutines();
        Debug.Log("Level was loaded");
        StartCoroutine(nameof(WaitForKiwisToSpawn));
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        timeStaticMax = SwappingCooldown;
    }
    private void OnLevelWasLoaded(int level)
    {
        _state = InputState.Nothing;
        _tile = null;
    }

    IEnumerator WaitForKiwisToSpawn()
    {
        Debug.Log("Started Waiting");
        float t = 0;

        while (t < delay) {
            Debug.Log($"Time: {t}");
            yield return null;

            CheckSwapInput();

            CheckRotationInput();

            t += Time.deltaTime;
        }

        StartCoroutine(nameof(StartSwapCooldown));
    }

    IEnumerator StartSwapCooldown()
    {
        while (true)
        {
            //Check input each frame until swap has been performed
            while (!CheckSwapInput()) yield return null;

            timeStatic = 0; 

            //Wait until swap is off cooldown
            while (timeStatic < timeStaticMax)
            {
                yield return null;
                timeStatic += Time.deltaTime;
            }
        }
    }

    public bool CheckSwapInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse Left");
            //Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 20, Kiwi.TileFinderMask);
            //Debug.Log(v);
            //Debug.Log(hit.collider);
            if (hit.collider != null)
            {
                MoveableTile tile = hit.collider.gameObject.GetComponent<MoveableTile>();
                if (tile == null) return false;
                switch (_state)
                {
                    case InputState.Nothing:
                        this._tile = tile;
                        _state = InputState.Undefined_Swap;
                        break;
                    case InputState.Click_Swap:
                        Swap(_tile, tile);
                        return true;
                    default:
                        _state = InputState.Nothing;
                        break;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 20, Kiwi.TileFinderMask);
            if (hit.collider != null)
            {
                MoveableTile tile = hit.collider.gameObject.GetComponent<MoveableTile>();
                if (tile == null) return false;
                switch (_state)
                {
                    case InputState.Undefined_Swap:
                        if (tile == _tile)
                        {
                            _state = InputState.Click_Swap;
                        }
                        else
                        {
                            _state = InputState.Nothing;
                            Swap(_tile, tile);
                            return true;
                        }
                        break;
                    default:
                        _state = InputState.Nothing;
                        break;
                }
            }
        }
        return false;
    }

    public void CheckRotationInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Mouse Right");
            Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 20, Kiwi.TileFinderMask);
            if (hit.collider != null)
            {
                MoveableTile tile = hit.collider.gameObject.GetComponent<MoveableTile>();
                Rotate(tile);
            }
        }
    }

    public void Swap(Tile a, Tile b)
    {
        playSwapClip();
        Vector3 pos = b.transform.position;
        b.transform.position = a.transform.position;
        a.transform.position = pos;
    }

    public void Rotate(Tile a)
    {
        playRotateClip();
        a.Rotate();

    }

    public void playRotateClip()
    {
        if (rotateClips != null && rotateClips.Length > 0)
        {
            AudioClip clip = rotateClips[Random.Range(0, rotateClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    public void playSwapClip()
    {
        if (swapClips != null && swapClips.Length > 0)
        {
            AudioClip clip = swapClips[Random.Range(0, swapClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private enum InputState
    {
        Nothing,
        Click_Swap,
        Undefined_Swap
    }
}
