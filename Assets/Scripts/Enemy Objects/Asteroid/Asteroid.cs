using System;
using UnityEngine;

public sealed class Asteroid : Actor
{
    [SerializeField] private float _asteroidSpeed;

    private BoxCollider2D _asteroidCollider;
    private SpriteRenderer _asteroidRenderer;
    private AsteroidFactory _asteroidFactory;
    private AsteroidConfigSO _config;
    
    public AsteroidType AsteroidType { get; private set; }
    public Vector2 ForwardDirection { get; private set; }
    
    public event Action<Asteroid, int> AsteroidBlown;

    protected override void Awake()
    {
        base.Awake();
        
        _asteroidCollider = GetComponent<BoxCollider2D>();
        _asteroidRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Init(AsteroidType type, Vector2 position, Quaternion rotation, Vector2 direction, AsteroidConfigSO config, AsteroidFactory factory)
    {
        transform.SetPositionAndRotation(position, rotation);
        ForwardDirection = direction;
        _asteroidFactory = factory;
        AsteroidType = type;
        _config = config;
        
        _asteroidSpeed = config.Speed.RandomValueInRange;
        _asteroidCollider.size = config.ColliderSize;
        _asteroidRenderer.sprite = config.Sprite;
    }
    
    private void Update()
    {
        if (PauseManager.Instance.IsPaused) return;
        
        if(BeyondBounds()) ReclaimAsteroid();
        
        transform.position += (Vector3) (ForwardDirection * _asteroidSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Bullet bullet))
        {
            if (bullet.BulletType == BulletType.Player)
            {
                AsteroidBlown?.Invoke(this, _config.Score);
                ReclaimAsteroid();
                return;
            }
        }
        AsteroidBlown?.Invoke(this, 0);
        ReclaimAsteroid();
    }
    
    private void ReclaimAsteroid() => _asteroidFactory.Reclaim(this);
}