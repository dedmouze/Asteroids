public interface IPlayerLifeStateSubscriber : IPlayerDeathParameterlessSubscriber, IPlayerRespawnSubscriber, IGlobalSubscriber
{
    void IPlayerDeathParameterlessSubscriber.OnPlayerDeath() => OnPlayerLifeStateChanged(true);
    void IPlayerRespawnSubscriber.OnPlayerRespawn() => OnPlayerLifeStateChanged(false);

    void OnPlayerLifeStateChanged(bool isDead);
}