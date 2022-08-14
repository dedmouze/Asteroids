using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public abstract class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _collider;
    
    [SerializeField] protected FloatRange SpawnCooldown;

    private Camera _mainCamera;
    private Vector2 _halfScreenSize;
    private Vector2 _spawnPositionBounds;
    
    protected Timer Timer;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _halfScreenSize = new Vector2(20f, 11.25f); // Игра будет работать только от 1920x1080
        Vector2 extents = _collider.size / 2;
        _spawnPositionBounds = new Vector2(_halfScreenSize.x + extents.x, _halfScreenSize.y + extents.y);
        
        Init();
    }

    private void OnValidate() => Timer?.SetNewTime(SpawnCooldown.RandomValueInRange);

    private void Update() => Timer.Tick(Time.deltaTime);

    protected Vector2 GetRandomPositionOutsideScreen()
    {
        return Random.Range(0, 4) switch
        {
            0 => new Vector2(_spawnPositionBounds.x, Random.Range(-_halfScreenSize.y, _halfScreenSize.y)),
            1 => new Vector2(-_spawnPositionBounds.x, Random.Range(-_halfScreenSize.y, _halfScreenSize.y)),
            2 => new Vector2(Random.Range(-_halfScreenSize.x, _halfScreenSize.x), _spawnPositionBounds.y),
            3 => new Vector2(Random.Range(-_halfScreenSize.x, _halfScreenSize.x), -_spawnPositionBounds.y),
            _ => Vector2.zero
        };
    }
    
    protected Vector2 GetRandomDirection(Vector2 position)
    {
        Vector2 point = Random.insideUnitCircle;
        point = new Vector2(Mathf.Abs(point.x), Mathf.Abs(point.y));
        point = _mainCamera.ViewportToWorldPoint(point);
        
        return (point - position).normalized;
    }

    protected Vector2 GetStraightDirection(Vector2 position)
    {
        if (position.x >= _spawnPositionBounds.x) return Vector2.left;
        if (position.x <= -_spawnPositionBounds.x) return Vector2.right;

        return position.y >= _spawnPositionBounds.y ? Vector2.down : Vector2.up;
    }
    
    protected virtual void Init(){}
}