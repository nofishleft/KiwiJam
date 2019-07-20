using System.Collections;
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

    private void Start()
    {
        //_ = Path;
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

    public void ApplyChangedPathToChildren()
    {
        Quaternion q = Quaternion.Euler(0, 0, -90);
        foreach (var kiwi in GetComponentsInChildren<Kiwi>())
        {
            kiwi.SetPath(Path);
            Transform t = kiwi.transform;

            t.localPosition = q * t.localPosition;

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
        }
    }

    public virtual List<Vector3> GetPath(Vector3 startingPosition)
    {
        return null;
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
    CENTER
}
