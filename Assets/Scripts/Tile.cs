﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this to tiles
/// </summary>
public class Tile : MonoBehaviour
{
    public PathType TypeOfPath;
    //public int PathOrientation;
    
    public EntryPoints[] Entries;

    public string childSpriteName = "DummySprite";
    [HideInInspector]
    public Transform dummySprite;

    private void Start()
    {
        //_ = Path;
        dummySprite = transform.Find(childSpriteName);

        //Alternatively below, but only workds for the specified type, may not work if we bring in the 3d pothole
        //dummySprite = GetComponentInChildren<SpriteRenderer>().transform;
    }

    public List<Vector3> Path
    {
        get => (_path != null) ? _path : (_path = CreatePath());
    }
    private List<Vector3> _path;

    public List<Vector3> CreatePath()
    {
        List<Vector3> list = new List<Vector3>();

        if (TypeOfPath == PathType.STRAIGHT)
        {
            list.Add(VectorFromEntryPoint(Entries[0]));
            list.Add(VectorFromEntryPoint(Entries[1]));

            for (int i = 1; i < list.Count; ++i)
            {
                Debug.DrawLine(transform.TransformPoint(list[i - 1]), transform.TransformPoint(list[i]), Color.red, 10f, false);
            }

            return list;
        } else if (TypeOfPath == PathType.CORNER)
        {
            list.Add(VectorFromEntryPoint(Entries[0]));
            list.Add(Vector3.zero);
            list.Add(VectorFromEntryPoint(Entries[1]));

            for (int i = 1; i < list.Count; ++i)
            {
                Debug.DrawLine(transform.TransformPoint(list[i - 1]), transform.TransformPoint(list[i]), Color.red, 10f, false);
            }

            return list;
        } else if (TypeOfPath == PathType.DEAD_END)
        {
            list.Add(VectorFromEntryPoint(Entries[0]));
            list.Add(VectorFromEntryPoint(Entries[1]));

            for (int i = 1; i < list.Count; ++i)
            {
                Debug.DrawLine(transform.TransformPoint(list[i - 1]), transform.TransformPoint(list[i]), Color.red, 10f, false);
            }

            return list;
        }

        return null;
    }

    public List<Vector3> CreatePath(EntryPoints point)
    {
        List<Vector3> list = new List<Vector3>();

        if (TypeOfPath == PathType.STRAIGHT)
        {
            if (point == Entries[0])
            {
                list.Add(VectorFromEntryPoint(Entries[0]));
                list.Add(VectorFromEntryPoint(Entries[1]));
            }
            else
            {
                list.Add(VectorFromEntryPoint(Entries[1]));
                list.Add(VectorFromEntryPoint(Entries[0]));
            }

            for (int i = 1; i < list.Count; ++i)
            {
                Debug.DrawLine(transform.TransformPoint(list[i - 1]), transform.TransformPoint(list[i]), Color.red, 10f, false);
            }

            return list;
        }
        else if (TypeOfPath == PathType.CORNER)
        {
            if (point == Entries[0])
            {
                list.Add(VectorFromEntryPoint(Entries[0]));
                list.Add(Vector3.zero);
                list.Add(VectorFromEntryPoint(Entries[1]));
            }
            else
            {
                list.Add(VectorFromEntryPoint(Entries[1]));
                list.Add(Vector3.zero);
                list.Add(VectorFromEntryPoint(Entries[0]));
            }
            for (int i = 1; i < list.Count; ++i)
            {
                Debug.DrawLine(transform.TransformPoint(list[i - 1]), transform.TransformPoint(list[i]), Color.red, 10f, false);
            }

            return list;
        }
        else if (TypeOfPath == PathType.DEAD_END)
        {
            if (point == Entries[0])
            {
                list.Add(VectorFromEntryPoint(Entries[0]));
                list.Add(VectorFromEntryPoint(Entries[1]));
            }
            else
            {
                list.Add(VectorFromEntryPoint(Entries[1]));
                list.Add(VectorFromEntryPoint(Entries[0]));
            }

            for (int i = 1; i < list.Count; ++i)
            {
                Debug.DrawLine(transform.TransformPoint(list[i - 1]), transform.TransformPoint(list[i]), Color.red, 10f, false);
            }

            return list;
        }

        return null;
    }

    public void RegeneratePath()
    {
        _path = CreatePath();
    }

    public EntryPoints RotatedEntryPoint(EntryPoints point)
    {
        switch (point)
        {
            case EntryPoints.UP:
                return EntryPoints.RIGHT;
            case EntryPoints.RIGHT:
                return EntryPoints.DOWN;
            case EntryPoints.DOWN:
                return EntryPoints.LEFT;
            case EntryPoints.LEFT:
                return EntryPoints.UP;
            case EntryPoints.CENTER:
                return point;
            default:
                return point;
        }
    }

    public Vector3 RotatedVector(Vector3 v)
    {
        if (v.x > 0)
        {
            v.y = -v.x;
            v.x = 0;
        }
        else if (v.x < 0)
        {
            v.y = -v.x;
            v.x = 0;
        }
        else if (v.y > 0)
        {
            v.x = v.y;
            v.y = 0;
        }
        else if (v.y < 0)
        {
            v.x = v.y;
            v.y = 0;
        }

        return v;
    }

    public void ApplyChangedPathToChildren()
    {
        Quaternion q = Quaternion.Euler(0, 0, -90);
        foreach (var kiwi in GetComponentsInChildren<Kiwi>())
        {
            switch (kiwi.Exit)
            {
                case EntryPoints.UP:
                    kiwi.Exit = EntryPoints.RIGHT;
                    break;
                case EntryPoints.RIGHT:
                    kiwi.Exit = EntryPoints.DOWN;
                    break;
                case EntryPoints.DOWN:
                    kiwi.Exit = EntryPoints.LEFT;
                    break;
                case EntryPoints.LEFT:
                    kiwi.Exit = EntryPoints.UP;
                    break;
                case EntryPoints.CENTER:
                    break;
                default:
                    break;
            }

            for (int i = 0; i < kiwi.Path.Count; ++i)
            {
                kiwi.Path[i] = RotatedVector(kiwi.Path[i]);
            }

            /*if (kiwi.Exit == Entries[0])
            {
                kiwi.SetPath(FlipPath(Path));
            }
            else
            {
                kiwi.SetPath(Path);
            }*/
            
            Transform t = kiwi.transform;

            t.localPosition = q * t.localPosition;

            if (!kiwi.HasPath)
            {
                Vector3 v = kiwi.Path[kiwi.Path.Count - 1];
                kiwi.HasPath = kiwi.UpdatePath(v);
            }

            kiwi.MovementDirection = RotatedVector(kiwi.MovementDirection);
            kiwi.ImageOrientation.rotation = Quaternion.FromToRotation(Vector2.right, kiwi.MovementDirection);
        }
    }

    public List<Vector3> FlipPath(List<Vector3> path)
    {
        List<Vector3> p = new List<Vector3>(path.Count);
        foreach (var v in path)
        {
            p.Add(v);
        }

        for (int i = 0; i < p.Count; ++i)
        {
            Vector3 vec = p[i];
            p[i] = p[p.Count - 1 - i];
            p[p.Count - 1 - i] = vec;
        }

        return p;
    }

    public void Rotate()
    {
        for (int i = 0; i < Entries.Length; ++i)
        {
            switch (Entries[i])
            {
                case EntryPoints.UP:
                    Entries[i] = EntryPoints.RIGHT;
                    break;
                case EntryPoints.RIGHT:
                    Entries[i] = EntryPoints.DOWN;
                    break;
                case EntryPoints.DOWN:
                    Entries[i] = EntryPoints.LEFT;
                    break;
                case EntryPoints.LEFT:
                    Entries[i] = EntryPoints.UP;
                    break;
                case EntryPoints.CENTER:
                    break;
                default:
                    break;
            }
        }

        RegeneratePath();
        //Quaternion q = Quaternion.Euler(0, 0, -90);
        //transform.localRotation = transform.localRotation * q;
        ApplyChangedPathToChildren();

        RotateSprite();
    }

    public void RotateSprite()
    {
        dummySprite.Rotate(0, 0, -90);
    }

    public static Vector3 VectorFromEntryPoint(EntryPoints point)
    {
        switch (point)
        {
            case EntryPoints.UP:
                return Vector3.up/2;
            case EntryPoints.RIGHT:
                return Vector3.right/2;
            case EntryPoints.DOWN:
                return Vector3.down/2;
            case EntryPoints.LEFT:
                return Vector3.left/2;
            case EntryPoints.CENTER:
                return Vector3.zero;
            default:
                return Vector3.zero;
        }
    }

    public static EntryPoints FlipEntryPoints(EntryPoints point)
    {
        switch (point)
        {
            case EntryPoints.UP:
                return EntryPoints.DOWN;
            case EntryPoints.RIGHT:
                return EntryPoints.LEFT;
            case EntryPoints.DOWN:
                return EntryPoints.UP;
            case EntryPoints.LEFT:
                return EntryPoints.RIGHT;
            case EntryPoints.CENTER:
                return EntryPoints.CENTER;
            default:
                return EntryPoints.DOWN;
        }
    }
}

public enum EntryPoints
{
    LEFT,
    RIGHT,
    UP,
    DOWN,
    CENTER,
    UNDEFINED
}
