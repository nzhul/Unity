using Assets.Scripts;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField]
    Transform arrow = default;

    GameTile north, east, south, west, nextOnPath;

    int distance;

    static Quaternion
        northRotation = Quaternion.Euler(90f, 0f, 0f),
        eastRotation = Quaternion.Euler(90f, 90f, 0f),
        southRotation = Quaternion.Euler(90f, 180f, 0f),
        westRotation = Quaternion.Euler(90f, 270f, 0f);

    GameTileContent content;

    public bool IsAlternative { get; set; }

    public GameTile NextTileOnPath => nextOnPath;

    public bool HasPath => distance != int.MaxValue;

    public Vector3 ExitPoint { get; private set; }

    public Direction PathDirection { get; private set; }

    public void ShowPath()
    {
        if (distance == 0)
        {
            arrow.gameObject.SetActive(false);
            return;
        }

        arrow.gameObject.SetActive(true);
        arrow.localRotation =
            nextOnPath == north ? northRotation :
            nextOnPath == east ? eastRotation :
            nextOnPath == south ? southRotation :
            westRotation;
    }

    public static void MakeEasyWestNeighbors(GameTile east, GameTile west)
    {
        Debug.Assert(west.east == null && east.west == null, "Redefined neighbors!");
        west.east = east;
        east.west = west;
    }

    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
    {
        Debug.Assert(south.north == null && north.south == null, "Redefined neighbors!");
        south.north = north;
        north.south = south;
    }

    public GameTile GrowPathNorth() => GrowPathTo(north, Direction.South);

    public GameTile GrowPathEast() => GrowPathTo(east, Direction.West);

    public GameTile GrowPathSouth() => GrowPathTo(south, Direction.North);

    public GameTile GrowPathWest() => GrowPathTo(west, Direction.East);

    public void ClearPath()
    {
        distance = int.MaxValue;
        nextOnPath = null;
    }

    public void BecomeDestination()
    {
        distance = 0;
        nextOnPath = null;
        ExitPoint = transform.localPosition;
    }

    public GameTileContent Content
    {
        get => content;
        set
        {
            Debug.Assert(value != null, "Null assigned to content!");

            if (content != null)
            {
                content.Recycle();
            }

            content = value;
            content.transform.localPosition = transform.localPosition;
        }
    }

    public void HidePath()
    {
        arrow.gameObject.SetActive(false);
    }

    private GameTile GrowPathTo(GameTile neighbor, Direction direction)
    {
        Debug.Assert(HasPath, "No path!");

        if (neighbor == null || neighbor.HasPath)
        {
            return null;
        }

        neighbor.distance = distance + 1;
        neighbor.nextOnPath = this;

        neighbor.ExitPoint = neighbor.transform.localPosition + direction.GetHalfVector();

        neighbor.PathDirection = direction;

        return neighbor.Content.BlocksPath ? null : neighbor;
    }
}
