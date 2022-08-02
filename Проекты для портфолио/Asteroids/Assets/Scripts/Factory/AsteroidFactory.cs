using System;
using UnityEngine;

public class AsteroidFactory : MonoBehaviour
{
    [SerializeField] private int _baseCapacity = 16;
    [SerializeField] private Asteroid _asteroidPrefab;
    [SerializeField] private AsteroidConfigSO _bigAsteroidConfig, _mediumAsteroidConfig, _smallAsteroidConfig;
    
    private IObjectPool<Asteroid> _asteroidPool;
    private Action<Asteroid, int> _onBlown;
    
    private void Awake() => _asteroidPool = new AsteroidPool(_baseCapacity, _asteroidPrefab);

    public Asteroid Create(AsteroidType type, Vector2 position, Vector2 direction, Quaternion rotation, Action<Asteroid, int> onBlown)
    {
        AsteroidConfigSO config = GetAsteroidConfig(type);
        
        Asteroid asteroid = _asteroidPool.Request();
        
        asteroid.name = type.ToString();
        asteroid.AsteroidBlown += onBlown;
        _onBlown = onBlown;

        asteroid.Init(type, position, rotation, direction, config, this);

        return asteroid;
    }
    
    public void Reclaim(Asteroid asteroid)
    {
        asteroid.AsteroidBlown -= _onBlown;
        _asteroidPool.Return(asteroid);
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