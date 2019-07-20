﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private InputState _state = InputState.Nothing;

    private MoveableTile _tile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Left");
            Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 20, Kiwi.TileFinderMask);
            Debug.Log(v);
            Debug.Log(hit.collider);
            if (hit.collider != null)
            {
                MoveableTile tile = hit.collider.gameObject.GetComponent<MoveableTile>();
                if (tile == null) return;
                switch (_state)
                {
                    case InputState.Nothing:
                        this._tile = tile;
                        _state = InputState.Undefined_Swap;
                        break;
                    case InputState.Click_Swap:
                        Swap(_tile, tile);
                        break;
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
                if (tile == null) return;
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
                        }
                        break;
                    default:
                        _state = InputState.Nothing;
                        break;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
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
        Vector3 pos = b.transform.position;
        b.transform.position = a.transform.position;
        a.transform.position = pos;
    }

    public void Rotate(Tile a)
    {
        a.Rotate();
    }

    private enum InputState
    {
        Nothing,
        Click_Swap,
        Undefined_Swap
    }
}