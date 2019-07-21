#define TwoD

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiwi : MonoBehaviour
{
    public Vector3 MovementDirection;
    public List<Vector3> Path;
    public float SpaceBetweenKiwis;
    public static LayerMask TileFinderMask = 1 << 11;
    public static LayerMask KiwiFinderMask = 1 << 10;
    public Tile parentTile;
    public int next = 1;
    public EntryPoints Exit = EntryPoints.DOWN;
    public bool HasPath = true;
    public float Speed = 0.2f;
    public static Dictionary<int, Material> materials;
    public int Color;
    public float Rage;
    public float RageIncreaseOverTime = 1/10000f;
    public float RageIncreaseWhileStationary = 1/1000f;
    public bool awaiting = false;
    public Transform ImageOrientation;
    public float RageIncreasePotHole = 1 / 100f;
    public float RageIncreaseWrongDestination = 1 / 100f;
    public Spawn SpawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        //transform.parent = parentTile.transform;
    }
    public void SetPath(List<Vector3> src)
    {
        Debug.Log(src.Count);
        Path = new List<Vector3>(src.Count);

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
            foreach (var obj in src)
            {
                Path.Add(obj);
            }
        }

        //Debug.Log(Path.Count);
    }

    private bool FlipPath = false;

    public bool UpdatePath(Vector3 dir)
    {
        //Find next tile
        Vector3 origin = transform.position;
#if TwoD
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, 0.5f, TileFinderMask);
        //foreach (var thing in hits) Debug.Log(thing);
#else
        RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.back, 0.5f, TileFinderMask);
#endif
        foreach (var hit in hits)
            if (hit.collider != null && hit.collider.gameObject != this.gameObject && hit.collider.gameObject != parentTile.gameObject)
            {
#if DEBUG
                //Debug.Log("Found");
#endif
                Tile oldTile = parentTile;

                GameObject obj = hit.collider.gameObject;
                parentTile = obj.GetComponent<Tile>();

                bool contains = false;
                EntryPoints entry = Tile.FlipEntryPoints(Exit);
                foreach (var en in parentTile.Entries) if (en == entry) contains = true;

                if (contains)
                {
                    RaycastHit2D checkHits = Physics2D.BoxCast(transform.position, new Vector2(0.25f, 0.25f), 0, Vector2.zero, 0, KiwiFinderMask);
                    if (checkHits.collider != null && checkHits.collider.gameObject != this.gameObject)
                    {
                        Debug.Log("Returning false");
                        return false;
                    }

                    if (parentTile.Entries[0] == entry)
                    {
                        this.Exit = parentTile.Entries[1];
                    } else if(parentTile.Entries[1] == entry)
                    {
                        this.Exit = parentTile.Entries[0];
                    }

                    //Debug.Log("Contains");
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
                    //Debug.Log(Path[0]);

                    next = 1;

                    //foreach (var o in Path) Debug.Log(o);
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
        //if (!HasPath) Debug.Log(Tile.VectorFromEntryPoint(Exit).normalized);
        if (!HasPath) HasPath = UpdatePath(Tile.VectorFromEntryPoint(Exit).normalized);
        if (!HasPath || awaiting)
        {
            float r = (RageIncreaseOverTime + RageIncreaseWhileStationary) * Time.deltaTime;
            Rage += r;
            RageBar.rage += r;
        }
        if (!HasPath) {
            if (parentTile is Exit)
            {
                Exit ex = (Exit)parentTile;
                if (ex.Color == this.Color)
                {
                    //Despawn
                    Debug.Log("Despawning");
                    RageBar.rage -= Rage;

                    Destroy(gameObject);
                }
                else
                {
                    RageBar.rage += RageIncreaseWrongDestination;

                    SpawnLocation.AddToRespawnQueue(this);
                }
            }
            else if (parentTile is Spawn)
            {
                Spawn spawn = (Spawn)parentTile;
                if (spawn.Color == this.Color)
                {
                    //Despawn
                    Debug.Log("Despawning");
                    RageBar.rage -= Rage;

                    Destroy(gameObject);
                }
                else
                {
                    /*Debug.Log("Turning around");
                    //Turn back
                    //Reverse Path
                    for (int i = 0; i < Path.Count / 2; ++i)
                    {
                        Vector3 vec = Path[i];
                        Path[i] = Path[Path.Count - 1 - i];
                        Path[Path.Count - 1 - i] = vec;
                        next = 1;
                    }

                    //Change Exit
                    foreach (var a in parentTile.Entries) Debug.Log(a);

                    if (this.Exit == parentTile.Entries[0])
                        this.Exit = parentTile.Entries[1];
                    else
                        this.Exit = parentTile.Entries[0];
                    HasPath = true;*/

                    RageBar.rage += RageIncreaseWrongDestination;

                    SpawnLocation.AddToRespawnQueue(this);
                    //Destroy(gameObject);
                }
            }
            else if (parentTile is PotHole)
            {
                PotHole hole = (PotHole)parentTile;

                RageBar.rage += RageIncreasePotHole;

                SpawnLocation.AddToRespawnQueue(this);
                //Destroy(gameObject);
            }
            else
                return;
        }

        if (HasPath)
        {
            float r = RageIncreaseOverTime * Time.deltaTime;
            Rage += r;
            RageBar.rage += r;
        }

        if (Path == null || Path.Count == 0) return;

        //Debug.Log($"Next: {next}, Path Count: {Path.Count}");

        Vector3 to = Path[next] - transform.localPosition;
        Vector3 dir = to.normalized;
        float mag = to.magnitude;

        float distTravelled = Time.deltaTime * Speed;

        while (distTravelled >= mag)
        {
            Transform otherKiwi = Cast(distTravelled - mag);

            if (otherKiwi != null)
            {
                awaiting = true;
                return;
                //transform.localPosition = transform.InverseTransformDirection(otherKiwi.position) - dir * SpaceBetweenKiwis;
                //distTravelled = 0;
                //break;
            } else awaiting = false;

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

        MovementDirection = transform.TransformDirection(dir);
        MovementDirection.z = 0;

        //Update Rotation to point in Movement Direction
        ImageOrientation.rotation = Quaternion.FromToRotation(Vector2.right, MovementDirection);

        if (distTravelled > 0)
        {
            Transform otherKiwi = Cast(distTravelled);

            if (otherKiwi != null)
            {
                awaiting = true;
                return;
                //transform.localPosition = transform.InverseTransformPoint(otherKiwi.position) - dir * SpaceBetweenKiwis;
                //awaiting = otherKiwi;
                //lastPositionOfAwaiting = otherKiwi.position;
            }
            else
            {
                awaiting = false;
                transform.localPosition += dir * distTravelled;
            }
        }
    }

    //Raycast ahead to check if can move
    public Transform Cast(float distance)
    {
        distance += SpaceBetweenKiwis;
        Vector3 origin = transform.position;

        //Debug.Log(MovementDirection);
#if TwoD
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, MovementDirection, distance, KiwiFinderMask);
#else
        RaycastHit[] hits = Physics.RaycastAll(origin, MovementDirection, distance, KiwiFinderMask);
#endif
        foreach (var hit in hits)
            if (hit.collider != null && hit.collider.gameObject != this.gameObject)
            {
                //Debug.Log("Detected Kiwi");
                return hit.collider.transform;
            }

        return null;
    }
}

public enum PathType
{
    CORNER = 0,
    STRAIGHT = 1,
    DEAD_END = 2
}
