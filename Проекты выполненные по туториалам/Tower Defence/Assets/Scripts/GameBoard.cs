using UnityEngine;
using System.Collections.Generic;
public class GameBoard : MonoBehaviour
{
    [SerializeField] private Transform _ground;
    [SerializeField] private GameTile _tilePrefab;
    [SerializeField] private Texture2D _gridTexture;
    private readonly Queue<GameTile> _searchFrontier = new Queue<GameTile>();
    private readonly List<GameTile> _spawnPoints = new List<GameTile>();
    private readonly List<GameTileContent> _updatingContent = new List<GameTileContent>();
    private GameTileContentFactory _contentFactory;
    private GameTile[] _tiles;
    private Vector2Int _size;
    private bool _showPaths, _showGrid;
    
    public bool ShowPaths
    {
        get => _showPaths;
        set
        {
            _showPaths = value;
            if(_showPaths)
            {
                foreach (var tile in _tiles)
                {
                    tile.ShowPath();
                }
            }
            else
            {
                foreach (var tile in _tiles)
                {
                    tile.HidePath();
                }
            }
        }
    }
    public bool ShowGrid
    {
        get => _showGrid;
        set
        {
            _showGrid = value;
            Material mat = _ground.GetComponent<MeshRenderer>().material;
            if (_showGrid)
            {
                mat.mainTexture = _gridTexture;
                mat.SetTextureScale("_MainTex", _size);
            }
            else
            {
                mat.mainTexture = null;
            }
        }
    }
    public int SpawnPointCount => _spawnPoints.Count;

    public void Initialize(Vector2Int size, GameTileContentFactory contentFactory)
    {
        _size = size;
        _contentFactory = contentFactory;
        _ground.localScale = new Vector3(size.x, size.y, 1f);

        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
        _tiles = new GameTile[size.x * size.y];
        for (int y = 0, i = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = _tiles[i] = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);
                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]);
                }

                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, _tiles[i - size.x]);
                }
                tile.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }

                tile.Content = contentFactory.Get(GameTileContentType.Empty);
            }
        }
        ToggleDestination(_tiles[_tiles.Length / 2]);
        ToggleSpawnPoint(_tiles[0]);
    }
    public void GameUpdate()
    {
        for (int i = 0, n = _updatingContent.Count; i < n; i++)
        {
            _updatingContent[i].GameUpdate();
        }
    }
    public GameTile GetTile(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1))
        {
            int x = (int)(hit.point.x + _size.x * 0.5f);
            int y = (int)(hit.point.z + _size.y * 0.5f);
            if (x >= 0 && x < _size.x && y >= 0 && y < _size.y) 
            {
                return _tiles[x + y * _size.x];
            }
        }
        return null;
    }
    public void ToggleDestination (GameTile tile) 
    {
        if (tile.Content.Type == GameTileContentType.Destination) 
        {
            if (tile.Content.Type == GameTileContentType.Destination) 
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                if (!FindPaths()) 
                {
                    tile.Content = _contentFactory.Get(GameTileContentType.Destination);
                    FindPaths();
                }
            }
        }
        else if (tile.Content.Type == GameTileContentType.Empty) 
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Destination);
            FindPaths();
        }
    }
    public void ToggleWall(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
        else if (tile.Content.Type == GameTileContentType.Empty) 
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Wall);
            if (!FindPaths()) 
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
    }
    public void ToggleTower(GameTile tile, TowerType towerType)
    {
        if (tile.Content.Type == GameTileContentType.Tower)
        {
            _updatingContent.Remove(tile.Content);
            if(((Tower) tile.Content).TowerType == towerType)
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
            else
            {
                tile.Content = _contentFactory.Get(towerType);
                _updatingContent.Add(tile.Content);
            }
        }
        else if (tile.Content.Type == GameTileContentType.Empty) 
        {
            tile.Content = _contentFactory.Get(towerType);
            if (FindPaths())
            {
                _updatingContent.Add(tile.Content);
            }
            else 
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
        else if (tile.Content.Type == GameTileContentType.Wall)
        {
            _updatingContent.Add(tile.Content);
            tile.Content = _contentFactory.Get(towerType);
        }
    }
    public void ToggleSpawnPoint(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.SpawnPoint)
        {
            if(_spawnPoints.Count > 1)
            {
                _spawnPoints.Remove(tile);
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            }
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.SpawnPoint);
            _spawnPoints.Add(tile);
        }
    }
    public GameTile GetSpawnPoint(int index) => _spawnPoints[index];

    private bool FindPaths()
    {
        foreach (var tile in _tiles)
        {
            if (tile.Content.Type == GameTileContentType.Destination)
            {
                tile.BecomeDestination();
                _searchFrontier.Enqueue(tile);
            }
            else
            {
                tile.ClearPath();
            }
        }
        if (_searchFrontier.Count == 0)
            return false;
        while(_searchFrontier.Count > 0)
        {
            GameTile tile = _searchFrontier.Dequeue();
            if(tile != null)
            {
                if(tile.IsAlternative)
                {
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                }
            }
        }
        foreach (var tile in _tiles)
        {
            if (!tile.HasPath)
                return false;
        }
        if(ShowPaths)
        {
            foreach (var tile in _tiles)
            {
                tile.ShowPath();
            }
        }
        return true;
    }
}
