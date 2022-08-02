using System;
using UnityEngine;

[RequireComponent(typeof(UfoFactory))]
public sealed class UfoGenerator : Generator
{
    private UfoFactory _ufoFactory;

    public event Action<int> DestroyedByPlayer;
    
    protected override void Init()
    {
        _ufoFactory = GetComponent<UfoFactory>();
        Timer = new Timer(SpawnCooldown.RandomValueInRange, SpawnUfo);
    }

    private void SpawnUfo()
    {
        Vector2 position = GetRandomPositionOutsideScreen();
        Vector2 direction = GetStraightDirection(position);

        _ufoFactory.Create(position, direction, OnUfoBlown);
        Timer.SetNewTime(SpawnCooldown.RandomValueInRange);
    }

    private void OnUfoBlown(int score)
    {
        if (score != 0) DestroyedByPlayer?.Invoke(score);
    }
}