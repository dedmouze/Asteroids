using UnityEngine;

public sealed class Asteroid : Enemy<Asteroid>
{
    private SpriteRenderer _asteroidRenderer;
    private BoxCollider2D _asteroidCollider;

    public AsteroidType AsteroidType { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        
        _asteroidCollider = GetComponent<BoxCollider2D>();
        _asteroidRenderer = GetComponent<SpriteRenderer>();
        ActorObject = this;
    }
    
    public void Init(AsteroidType type, Vector2 position, Quaternion rotation, Vector2 direction, AsteroidConfigSO config, AsteroidFactory factory)
    {
        AsteroidType = type;
        Direction = direction;
        Speed = config.Speed.RandomValueInRange;
        Score = config.Score;
        Factory = factory;
        _asteroidRenderer.sprite = config.Sprite;
        _asteroidCollider.size = config.ColliderSize;

        transform.SetPositionAndRotation(position, rotation);
    }
}