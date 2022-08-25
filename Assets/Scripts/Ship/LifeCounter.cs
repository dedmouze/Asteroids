using UnityEngine;

public class LifeCounter : MonoBehaviour, IPlayerDeathParameterlessSubscriber, IGameRestartSubscriber
{
    [SerializeField] private ShipConfigSO _shipConfig;

    private int _lifeCount;

    private void Awake()
    {
        EventBus.Subscribe(this);
        _lifeCount = _shipConfig.LifeCount;
    }
    private void OnDestroy() => EventBus.Unsubscribe(this);

    void IPlayerDeathParameterlessSubscriber.OnPlayerDeath()
    {
        _lifeCount -= 1;
        if (_lifeCount == 0) EventBus.RaiseEvent<IGameEndSubscriber>(s => s.OnGameEnd());
    }

    void IGameRestartSubscriber.OnGameRestart() => _lifeCount = _shipConfig.LifeCount;
}