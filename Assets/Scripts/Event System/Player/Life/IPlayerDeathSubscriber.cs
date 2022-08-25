using UnityEngine;

public interface IPlayerDeathSubscriber : IGlobalSubscriber
{
    void OnPlayerDeath(Vector2 position);
}

public interface IPlayerDeathParameterlessSubscriber : IGlobalSubscriber
{
    void OnPlayerDeath();
}