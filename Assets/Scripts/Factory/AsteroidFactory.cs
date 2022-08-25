using System;
using UnityEngine;

public sealed class AsteroidFactory : Factory<Asteroid>
{
    [SerializeField] private AsteroidConfigSO _bigAsteroidConfig, _mediumAsteroidConfig, _smallAsteroidConfig;

    protected override void Awake() => Pool = new AsteroidPool(BaseCapacity, Prefab);

    public void Create(AsteroidType type, Vector2 position, Vector2 direction, Quaternion rotation)
    {
        AsteroidConfigSO config = GetAsteroidConfig(type);
        
        Asteroid asteroid = Pool.Request();

        asteroid.Init(type, position, rotation, direction, config, this);
    }

    private AsteroidConfigSO GetAsteroidConfig(AsteroidType type)
    {
        return type switch
        {
            AsteroidType.Big => _bigAsteroidConfig,
            AsteroidType.Medium => _mediumAsteroidConfig,
            AsteroidType.Small => _smallAsteroidConfig,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}