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
    public static float timeStatic;
    public static float timeStaticMax;
    public float SwappingCooldown;

    private InputState _state = InputState.Nothing;

    private MoveableTile _tile;

    // Start is called before the first frame update

    void OnLevelWasLoaded()
    {
        StopAllCoroutines();
        Debug.Log("Level was loaded");
        StartCoroutine(nameof(WaitForKiwisToSpawn));
        timeStatic = timeStaticMax;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
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

        while (LevelData.Started == false) {
            yield return null;

            CheckSwapInput();

            CheckRotationInput();
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit,20, Kiwi.TileFinderMask))
            {

                MoveableTile tile = hit.collider.gameObject.GetComponent<MoveableTile>();
                if (tile == null) return false;
                switch (_state)
                {
                    case InputState.Nothing:
                        this._tile = tile;
                        _state = InputState.Undefined_Swap;
                        SFXPlayer.PlaySelectSound(transform.position);
                        break;
                    case InputState.Click_Swap:
                        Swap(_tile, tile);
                        _state = InputState.Nothing;
                        _tile = null;
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

            if (Physics.Raycast(ray, out RaycastHit hit, 20, Kiwi.TileFinderMask))
            {
                MoveableTile tile = hit.collider.gameObject.GetComponent<MoveableTile>();
                if (tile == null) return false;
                switch (_state)
                {
                    case InputState.Undefined_Swap:
                        if (tile == _tile)
                        {
                            //SFXPlayer.PlaySelectSound(transform.position);
                            _state = InputState.Click_Swap;
                        }
                        else
                        {
                            _state = InputState.Nothing;
                            Swap(_tile, tile);
                            _tile = null;
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

            if (Physics.Raycast(ray, out RaycastHit hit, 20, Kiwi.TileFinderMask))
            {
                MoveableTile tile = hit.collider.gameObject.GetComponent<MoveableTile>();
                Rotate(tile);
            }
        }
    }

    public void Swap(Tile a, Tile b)
    {
        SFXPlayer.PlaySwapSound(Vector3.Lerp(a.transform.position, b.transform.position, 0.5f));
        Vector3 pos = b.transform.position;
        b.transform.position = a.transform.position;
        a.transform.position = pos;
    }

    public void Rotate(Tile a)
    {
        SFXPlayer.PlayRotateSound(a.transform.position);
        a.Rotate();

    }

    private enum InputState
    {
        Nothing,
        Click_Swap,
        Undefined_Swap
    }
}
