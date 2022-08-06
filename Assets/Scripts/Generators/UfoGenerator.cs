using UnityEngine;

[RequireComponent(typeof(UfoFactory))]
public sealed class UfoGenerator : EnemyGenerator
{
    private UfoFactory _ufoFactory;
    
    protected override void Init()
    {
        _ufoFactory = GetComponent<UfoFactory>();
        Timer = new Timer(SpawnCooldown.RandomValueInRange, SpawnUfo);
    }

    private void SpawnUfo()
    {
        Vector2 position = GetRandomPositionOutsideScreen();
        Vector2 direction = GetStraightDirection(position);

        _ufoFactory.Create(position, direction);
        Timer.SetNewTime(SpawnCooldown.RandomValueInRange);
    }
}