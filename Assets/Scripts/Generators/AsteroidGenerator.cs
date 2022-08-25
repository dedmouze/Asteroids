using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AsteroidFactory))]
public sealed class AsteroidGenerator : EnemyGenerator, IEnemyDeathSubscriber<Asteroid>, IGameRestartSubscriber
{
    [SerializeField] private FloatRange _asteroidPartsCount;
    
    private readonly int _asteroidTypeLength = Enum.GetNames(typeof(AsteroidType)).Length;
    private AsteroidFactory _asteroidFactory;

    protected override void Init()
    {
        EventBus.Subscribe(this);
        _asteroidFactory = GetComponent<AsteroidFactory>();
        GenerationTimer = new Timer(SpawnCooldown.RandomValueInRange, SpawnAsteroid,true);
    }
    private void OnDestroy() => EventBus.Unsubscribe(this);

    void IEnemyDeathSubscriber<Asteroid>.OnEnemyDeath(Asteroid asteroid)
    {
        switch (asteroid.AsteroidType)
        {
            case AsteroidType.Big: CreateAsteroidParts(asteroid, AsteroidType.Medium);
                return;
            case AsteroidType.Medium: CreateAsteroidParts(asteroid, AsteroidType.Small);
                return;
        }
    }
    
    void IGameRestartSubscriber.OnGameRestart() => SpawnCooldown.Max = StartMaxSpawnCooldown;

    private void SpawnAsteroid()
    {
        int randomAsteroidType = Random.Range(0, _asteroidTypeLength);
        Vector2 randomPositionOutsideScreen = GetRandomPositionOutsideScreen();
        Quaternion randomRotation = GetRandomRotation();
        _asteroidFactory.Create(
            (AsteroidType) randomAsteroidType, randomPositionOutsideScreen,
            GetRandomDirection(randomPositionOutsideScreen), randomRotation
            );
        
        GenerationTimer.SetNewTime(SpawnCooldown.RandomValueInRange);
        SpawnCooldown.Max *= DifficultFactor;
    }
    
    private void CreateAsteroidParts(Asteroid parent, AsteroidType type)
    {
        for (int i = 0; i < _asteroidPartsCount.RandomValueInRange; i++)
        {
            Quaternion rotation = GetRandomRotation();
            Vector2 direction = GetRandomDirection(parent.transform.position);

            _asteroidFactory.Create(type, parent.transform.position, direction, rotation);
        }
    }

    private Quaternion GetRandomRotation() => Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-180f, 180f)));
}