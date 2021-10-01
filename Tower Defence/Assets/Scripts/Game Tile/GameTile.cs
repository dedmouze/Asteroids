using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField] private Transform _arrow;
    private GameTile _north, _south, _east, _west, _nextOnPath;
    private GameTileContent _content;
    private readonly Quaternion
        _northRotation = Quaternion.Euler(90f, 0f, 0f),
        _eastRotation = Quaternion.Euler(90f, 90f, 0f),
        _southRotation = Quaternion.Euler(90f, 180f, 0f),
        _westRotation = Quaternion.Euler(90f, 270f, 0f);
    private int _distance;

    public bool HasPath => _distance != int.MaxValue;
    public GameTile NextTileOnPath => _nextOnPath;
    public bool IsAlternative { get; set; }
    public Vector3 ExitPoint { get; private set; }
    public Direction PathDirection { get; private set; }
    public GameTileContent Content
    {
        get => _content;
        set
        {
            Debug.Assert(value != null, "Null assigned to content!");
            if (_content != null)
            {
                _content.Recycle();
            }
            _content = value;
            _content.transform.localPosition = transform.localPosition;
        }
    }

    public static void MakeEastWestNeighbors(GameTile east, GameTile west)
    {
        Debug.Assert(west._east == null && east._west == null, "Redefined neighbors!");
        east._west = west;
        west._east = east;
    }
    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
    {
        Debug.Assert(north._south == null && south._north == null, "Redefined neighbors!");
        north._south = south;
        south._north = north;
    }
    public void ClearPath()
    {
        _distance = int.MaxValue;
        _nextOnPath = null;
    }
    public void BecomeDestination()
    {
        _distance = 0;
        _nextOnPath = null;
        ExitPoint = transform.localPosition;
    }
    public GameTile GrowPathNorth () => GrowPathTo(_north, Direction.South);
    public GameTile GrowPathEast () => GrowPathTo(_east, Direction.West);
    public GameTile GrowPathSouth () => GrowPathTo(_south, Direction.North);
    public GameTile GrowPathWest () => GrowPathTo(_west, Direction.East);
    public void ShowPath()
    {
        if(_distance == 0)
        {
            _arrow.gameObject.SetActive(false);
            return;
        }
        _arrow.gameObject.SetActive(true);
        _arrow.localRotation =
            _nextOnPath == _north ? _northRotation :
            _nextOnPath == _south ? _southRotation :
            _nextOnPath == _east ? _eastRotation 
            : _westRotation;
    }
    public void HidePath()
    {
        _arrow.gameObject.SetActive(false);
    }
    
    private GameTile GrowPathTo(GameTile neighbor, Direction direction)
    {
        Debug.Assert(HasPath, "No path!");
        if (neighbor == null || neighbor.HasPath)
            return null;
        neighbor._distance = _distance + 1;
        neighbor._nextOnPath = this;
        neighbor.ExitPoint = neighbor.transform.localPosition + direction.GetHalfVector();
        neighbor.PathDirection = direction;
        return neighbor.Content.BlocksPath ? null : neighbor;
    }
}
