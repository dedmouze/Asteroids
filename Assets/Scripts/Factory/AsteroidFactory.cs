using System;
using UnityEngine;

public sealed class AsteroidFactory : EnemyFactory<Asteroid>
{
    [SerializeField] private AsteroidConfigSO _bigAsteroidConfig, _mediumAsteroidConfig, _smallAsteroidConfig;

    protected override void Awake() => Pool = new AsteroidPool(BaseCapacity, Prefab);

    public void Create(AsteroidType type, Vector2 position, Vector2 direction, Quaternion rotation)
    {
        AsteroidConfigSO config = GetAsteroidConfig(type);
        
        Asteroid asteroid = Pool.Request();

        Subscribe(asteroid);
        asteroid.name = type.ToString();
        
        asteroid.Init(type, position, rotation, direction, config, this);
    }

    private AsteroidConfigSO GetAsteroidConfig(AsteroidType type)
    {
        switch (type)
        {
            case AsteroidType.Big: return _bigAsteroidConfig;
            case AsteroidType.Medium: return _mediumAsteroidConfig;
            case AsteroidType.Small: return _smallAsteroidConfig;
            default: throw new ArgumentOutOfRangeException();
        }
    }
}