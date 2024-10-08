using Hover;
using UnityEngine;

public class GameTile : MonoBehaviour
{

    [SerializeField]
    Transform arrow = default;

    GameTile north, east, south, west, nextOnPath;

    int distance;

    GameTileContent content;

    public int TileId{ get; set;}

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

    public Direction PathDirection { get; private set; }

    public Outline outline;
    public Vector3 ExitPoint { get; private set; }

    public bool IsAlternative { get; set; }

    public bool HasPath => distance != int.MaxValue;

    public GameTile NextTileOnPath => nextOnPath;

    public void BecomeDestination()
    {
        distance = 0;
        nextOnPath = null;
        ExitPoint = transform.localPosition;
    }

    public void ClearPath()
    {
        distance = int.MaxValue;
        nextOnPath = null;
    }

    public GameTile GrowPathNorth() => GrowPathTo(north, Direction.South);

    public GameTile GrowPathEast() => GrowPathTo(east, Direction.West);

    public GameTile GrowPathSouth() => GrowPathTo(south, Direction.North);

    public GameTile GrowPathWest() => GrowPathTo(west, Direction.East);

    GameTile GrowPathTo(GameTile neighbor, Direction direction)
    {
        Debug.Assert(HasPath, "No path!");
        if (neighbor == null || neighbor.HasPath)
        {
            return null;
        }
        neighbor.distance = distance + 1;
        neighbor.nextOnPath = this;
        neighbor.ExitPoint =
            neighbor.transform.localPosition + direction.GetHalfVector();
        neighbor.PathDirection = direction;
        return
            neighbor.Content.Type != GameTileContentType.Wall ? neighbor : null;
    }

    public void HidePath()
    {
        arrow.gameObject.SetActive(false);
    }

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

    static Quaternion
        northRotation = Quaternion.Euler(90f, 0f, 0f),
        eastRotation = Quaternion.Euler(90f, 90f, 0f),
        southRotation = Quaternion.Euler(90f, 180f, 0f),
        westRotation = Quaternion.Euler(90f, 270f, 0f);

    public static void MakeEastWestNeighbors(GameTile east, GameTile west)
    {
        Debug.Assert(
            west.east == null && east.west == null, "Redefined neighbors!"
        );
        west.east = east;
        east.west = west;
    }

    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
    {
        Debug.Assert(
            south.north == null && north.south == null, "Redefined neighbors!"
        );
        south.north = north;
        north.south = south;
    }
    public int getDistance()
    {
        return distance;
    }
}

//enum itd
public enum Direction
{
    North, East, South, West
}
public enum DirectionChange
{
    None, TurnRight, TurnLeft, TurnAround
}

public static class DirectionExtensions
{

    static Quaternion[] rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f)
    };

    public static Quaternion GetRotation(this Direction direction)
    {
        return rotations[(int)direction];
    }
    public static DirectionChange GetDirectionChangeTo(
       this Direction current, Direction next
   )
    {
        if (current == next)
        {
            return DirectionChange.None;
        }
        else if (current + 1 == next || current - 3 == next)
        {
            return DirectionChange.TurnRight;
        }
        else if (current - 1 == next || current + 3 == next)
        {
            return DirectionChange.TurnLeft;
        }
        return DirectionChange.TurnAround;
    }
    public static float GetAngle(this Direction direction)
    {
        return (float)direction * 90f;
    }

    static Vector3[] halfVectors = {
        Vector3.forward * 0.5f,
        Vector3.right * 0.5f,
        Vector3.back * 0.5f,
        Vector3.left * 0.5f
    };

	public static Vector3 GetHalfVector(this Direction direction)
    {
        return halfVectors[(int)direction];
    }
}


