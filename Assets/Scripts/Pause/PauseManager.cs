
public class PauseManager : IGameEndSubscriber, IGamePauseSubscriber, IGameRestartSubscriber, IGameStartSubscriber
{
    private bool _isGameOver;
    
    public bool IsPaused { get; private set; } = true;

    public PauseManager() => EventBus.Subscribe(this);
    ~PauseManager() => EventBus.Unsubscribe(this);

    void IGameRestartSubscriber.OnGameRestart()
    {
        _isGameOver = false;
        IsPaused = false;
    }
    void IGamePauseSubscriber.OnPausePressed()
    {
        if (_isGameOver) return;
        IsPaused = !IsPaused;
    }
    void IGameEndSubscriber.OnGameEnd() => IsPaused = true;
    void IGameStartSubscriber.OnGameStart() => IsPaused = false;
}