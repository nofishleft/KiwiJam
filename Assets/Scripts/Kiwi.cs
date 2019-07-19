#define TwoD

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiwi : MonoBehaviour
{
    public List<Vector3> Path;
    public float SpaceBetweenKiwis;
    public static LayerMask TileFinderMask = 1 << 11;
    public static LayerMask KiwiFinderMask = 1 << 10;
    public MoveableArea parentTile;
    private int next = 1;
    public EntryPoints Exit = EntryPoints.DOWN;
    private bool HasPath = true;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = parentTile.transform;
    }

    public void SetPath(List<Vector3> src)
    {
        if (Path == null) Path = new List<Vector3>(src.Count);

        if (FlipPath)
        {
            //Flip the path because we are coming from the other direction
            for (int i = 0; i < Path.Count / 2; ++i)
            {
                Vector3 vec = src[i];
                Path[i] = src[src.Count - 1 - i];
                Path[Path.Count - 1 - i] = vec;
            }
        }
        else
        {
            for (int i = 0; i < Path.Count; ++i)
            {
                Path[i] = src[i];
            }
        }
    }

    private bool FlipPath = false;

    bool UpdatePath(Vector3 dir)
    {
        //Find next tile
        Vector3 origin = transform.position - Vector3.back;
#if TwoD
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, 0.5f, TileFinderMask);
#else
        RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.back, 0.5f, TileFinderMask);
#endif
        foreach (var hit in hits)
            if (hit.collider != null && hit.collider.gameObject != this.gameObject && hit.collider.gameObject != parentTile.gameObject)
            {
#if DEBUG
                Debug.Log("Found");
#endif
                MoveableArea oldTile = parentTile;

                GameObject obj = hit.collider.gameObject;
                parentTile = obj.GetComponent<MoveableArea>();

                bool contains = false;
                EntryPoints entry = MoveableArea.FlipEntryPoints(Exit);
                foreach (var en in parentTile.Entries) if (en == entry) contains = true;

                if (contains)
                {
                    if (parentTile.Entries[0] == entry)
                    {
                        this.Exit = parentTile.Entries[1];
                    } else if(parentTile.Entries[1] == entry)
                    {
                        this.Exit = parentTile.Entries[0];
                    }

                    Debug.Log("Contains");
                    transform.parent = obj.transform;

                    List<Vector3> src = parentTile.Path;//parentTile.GetPath(transform.position);
                    Path = new List<Vector3>(src.Count);

                    for (int i = 0; i < src.Count; ++i)
                    {
                        Path.Add(src[i]);
                    }

                    if ((Path[0] - transform.localPosition).sqrMagnitude > (Path[Path.Count - 1] - transform.localPosition).sqrMagnitude)
                    {
                        FlipPath = true;
                        //Flip the path because we are coming from the other direction
                        for (int i = 0; i < Path.Count / 2; ++i)
                        {
                            Vector3 vec = Path[i];
                            Path[i] = Path[Path.Count - 1 - i];
                            Path[Path.Count - 1 - i] = vec;
                        }
                    }

                    transform.localPosition = Path[0];
                    Debug.Log(Path[0]);

                    next = 1;

                    foreach (var o in Path) Debug.Log(o);
                    return true;
                }
                else
                {
                    parentTile = oldTile;
                }
                return false;
            }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasPath) HasPath = UpdatePath(MoveableArea.VectorFromEntryPoint(Exit).normalized);
        if (!HasPath) return;

        Vector3 to = Path[next] - transform.localPosition;
        Vector3 dir = to.normalized;
        float mag = to.magnitude;

        float distTravelled = Time.deltaTime /* Times by some constant*/;

        while (distTravelled >= mag)
        {
            Transform otherKiwi = Cast(distTravelled - mag);

            if (otherKiwi != null)
            {
                transform.localPosition = transform.InverseTransformPoint(otherKiwi.position) - dir * SpaceBetweenKiwis;
                distTravelled = 0;
                break;
            }

            //distTravelled -= mag;
            distTravelled = 0;
            
            transform.localPosition = Path[next];

            next++;

            if (next >= Path.Count)
            {
                //Next tile
                //Update path
                HasPath = UpdatePath(dir);
                if (!HasPath) return;
            }
            else
            {
                to = Path[next] - transform.localPosition;
                dir = to.normalized;
                mag = to.magnitude;
            }
        }

        if (distTravelled > 0)
        {
            Transform otherKiwi = Cast(distTravelled - mag);

            if (otherKiwi != null)
            {
                transform.localPosition = transform.InverseTransformPoint(otherKiwi.position) - dir * SpaceBetweenKiwis;
            }
            else
            {
                transform.localPosition += dir * distTravelled;
            }
        }
    }

    //Raycast ahead to check if can move
    public Transform Cast(float distance)
    {
        Vector3 origin = transform.position;
        Vector3 dir;
        if (next < Path.Count)
            dir = (Path[next] - Path[next - 1]).normalized;
        else
            dir = (Path[next - 1] - Path[next - 2]).normalized;
#if ThreeD
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, distance, 1 << gameObject.layer);
#else
        RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.back, 2, ~(1 << gameObject.layer));
#endif
        foreach (var hit in hits)
            if (hit.collider != null && hit.collider.gameObject != this.gameObject)
            {
                return hit.collider.transform;
            }

        return null;
    }
}

public enum PathType
{
    CORNER = 0,
    STRAIGHT = 1
}
