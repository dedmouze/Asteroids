using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] private float _spawnSpeed;
    [SerializeField] private EnemyFactory _enemyFactory;
    private readonly GamaBehaviorCollection _enemies = new GamaBehaviorCollection();
    private float _spawnProgress;

    [SerializeField] private WarFactory _warFactory;
    private readonly GamaBehaviorCollection _nonEnemies = new GamaBehaviorCollection();
    private static Game _instance;

    [SerializeField] private GameTileContentFactory _tileContentFactory;
    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private Vector2Int _boardSize;

    private TowerType _selectedTowerType;
    private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    public static Shell SpawnShell()
    {
        Shell shell = _instance._warFactory.Shell;
        _instance._nonEnemies.Add(shell);
        return shell;
    }
    public static Explosion SpawnExplosion () 
    {
        Explosion explosion = _instance._warFactory.Explosion;
        _instance._nonEnemies.Add(explosion);
        return explosion;
    }
    
    private void OnEnable()
    {
        _instance = this;
    }
    private void Awake()
    {
        _gameBoard.Initialize(_boardSize, _tileContentFactory);
    }
    private void OnValidate()
    {
        if (_boardSize.x < 2)
            _boardSize.x = 2;
        if (_boardSize.y < 2)
            _boardSize.y = 2;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleTouch();
        else if(Input.GetMouseButtonDown(1))
            HandleAlternativeTouch();
        if (Input.GetKeyDown(KeyCode.V))
            _gameBoard.ShowPaths = !_gameBoard.ShowPaths;
        if (Input.GetKeyDown(KeyCode.G))
            _gameBoard.ShowGrid = !_gameBoard.ShowGrid;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _selectedTowerType = TowerType.Laser;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            _selectedTowerType = TowerType.Mortar;

        _spawnProgress += Time.deltaTime * _spawnSpeed;
        if (_spawnProgress >= 1f)
        {
            _spawnProgress -= 1f;
            SpawnEnemy();
        }
        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _gameBoard.GameUpdate();
        _nonEnemies.GameUpdate();
    }
    private void HandleTouch()
    {
        GameTile tile = _gameBoard.GetTile(TouchRay);
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            _gameBoard.ToggleTower(tile, _selectedTowerType);
        }
        else 
        {
            _gameBoard.ToggleWall(tile);
        }
    }
    private void HandleAlternativeTouch() 
    {
        GameTile tile = _gameBoard.GetTile(TouchRay);
        if (tile != null) 
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _gameBoard.ToggleDestination(tile);
            }
            else
            {
                _gameBoard.ToggleSpawnPoint(tile);
            }
        }
    }
    private void SpawnEnemy()
    {
        GameTile spawnPoint = _gameBoard.GetSpawnPoint(Random.Range(0, _gameBoard.SpawnPointCount));
        Enemy enemy = _enemyFactory.Get();
        enemy.SpawnOn(spawnPoint);
        _enemies.Add(enemy);
    }
}
