using UnityEngine;
using UnityEngine.UI;

public sealed class UIMainMenu : UIMenu, IGamePauseSubscriber, IGameRestartSubscriber, IGameStartSubscriber
{
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private GameObject _pausePanel;

    private void Awake() => EventBus.Subscribe(this);
    private void OnDestroy() => EventBus.Unsubscribe(this);

    protected override void OnEnable()
    {
        base.OnEnable();
        _continueGameButton.onClick.AddListener(ContinueGame);
    }
    protected override void OnDisable()
    {
        base.OnEnable();
        _continueGameButton.onClick.RemoveListener(ContinueGame);
    }

    private void ContinueGame()
    {
        if (!GameSession.Instance.IsGameStarted) return;
        EventBus.RaiseEvent<IGamePauseSubscriber>(s => s.OnPausePressed());
    }
    
    void IGameRestartSubscriber.OnGameRestart() => _pausePanel.SetActive(false);
    void IGamePauseSubscriber.OnPausePressed() => _pausePanel.SetActive(!_pausePanel.activeSelf);
    void IGameStartSubscriber.OnGameStart()
    {
        _pausePanel.SetActive(false);
        _continueGameButton.GetComponent<Image>().color = Color.white;
    }
}