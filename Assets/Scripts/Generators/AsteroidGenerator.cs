using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AsteroidFactory))]
public sealed class AsteroidGenerator : Generator
{
    [SerializeField] private int _asteroidPartsCount = 2;

    private readonly int _asteroidTypeLength = Enum.GetNames(typeof(AsteroidType)).Length;
    private AsteroidFactory _asteroidFactory;

    public event Action<int> DestroyedByPlayer;

    protected override void Init()
    {
        _asteroidFactory = GetComponent<AsteroidFactory>();
        Timer = new Timer(SpawnCooldown.RandomValueInRange, SpawnAsteroid);
    }

    private void SpawnAsteroid()
    {
        int randomAsteroidType = Random.Range(0, _asteroidTypeLength);
        Vector2 randomPositionOutsideScreen = GetRandomPositionOutsideScreen();
        Quaternion randomRotation = GetRandomRotation();
        _asteroidFactory.Create(
            (AsteroidType) randomAsteroidType, randomPositionOutsideScreen,
            GetRandomDirection(randomPositionOutsideScreen), randomRotation, 
            OnAsteroidBlown
            );
        Timer.SetNewTime(SpawnCooldown.RandomValueInRange);
    }

    private void OnAsteroidBlown(Asteroid asteroid, int score)
    {
        if (score != 0) DestroyedByPlayer?.Invoke(score);
        
        switch (asteroid.AsteroidType)
        {
            case AsteroidType.Big: CreateAsteroidParts(asteroid, AsteroidType.Medium);
                return;
            case AsteroidType.Medium: CreateAsteroidParts(asteroid, AsteroidType.Small);
                return;
        }
    }

    private void CreateAsteroidParts(Asteroid parent, AsteroidType type)
    {
        for (int i = 0; i < _asteroidPartsCount; i++)
        {
            Quaternion rotation = GetRandomRotation();
            Vector2 direction = GetRandomDirection(parent.transform.position);

            _asteroidFactory.Create(type, parent.transform.position, direction, rotation, OnAsteroidBlown);
        }
    }
    
    private Quaternion GetRandomRotation() => Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-180f, 180f)));
}

