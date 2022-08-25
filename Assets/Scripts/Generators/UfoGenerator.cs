using UnityEngine;

[RequireComponent(typeof(UfoFactory))]
public sealed class UfoGenerator : EnemyGenerator, IGameRestartSubscriber
{
    private UfoFactory _ufoFactory;
    
    protected override void Init()
    {
        _ufoFactory = GetComponent<UfoFactory>();
        GenerationTimer = new Timer(SpawnCooldown.RandomValueInRange, SpawnUfo, true);
        EventBus.Subscribe(this);
    }
    private void OnDestroy() => EventBus.Unsubscribe(this);

    private void SpawnUfo()
    {
        Vector2 position = GetRandomPositionOutsideScreen();
        Vector2 direction = GetStraightDirection(position);

        _ufoFactory.Create(position, direction);
        GenerationTimer.SetNewTime(SpawnCooldown.RandomValueInRange);
        SpawnCooldown.Max *= DifficultFactor;
    }

    void IGameRestartSubscriber.OnGameRestart() => SpawnCooldown.Max = StartMaxSpawnCooldown;
}