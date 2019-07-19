using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableArea : MonoBehaviour
{
    public PathType TypeOfPath;
    //public int PathOrientation;
    
    public EntryPoints[] Entries;

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

            return list;
        } else if (TypeOfPath == PathType.CORNER)
        {
            list.Add(VectorFromEntryPoint(Entries[0]));
            list.Add(Vector3.zero);
            list.Add(VectorFromEntryPoint(Entries[1]));

            return list;
        }

        return null;
    }

    public virtual List<Vector3> GetPath(Vector3 startingPosition)
    {
        return null;
    }

    private void Rotate()
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
                default:
                    break;
            }
        }
    }

    private Vector3 VectorFromEntryPoint(EntryPoints point)
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
            default:
                return Vector3.zero;
        }
    }
}

public enum EntryPoints
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}
