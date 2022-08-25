using UnityEngine;

public sealed class UIGameOverMenu : UIMenu, IGameEndSubscriber, IGameRestartSubscriber
{
    [SerializeField] private GameObject _gameOverPanel;
    
    private void Awake() => EventBus.Subscribe(this);
    private void OnDestroy() => EventBus.Unsubscribe(this);

    void IGameEndSubscriber.OnGameEnd() => _gameOverPanel.SetActive(true);
    void IGameRestartSubscriber.OnGameRestart() => _gameOverPanel.SetActive(false);
}