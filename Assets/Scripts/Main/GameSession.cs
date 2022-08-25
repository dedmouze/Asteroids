using UnityEngine;

public class GameSession : MonoBehaviour, INewGameSubscriber
{
    private readonly Timers _allTimers = new Timers();
    
    public PauseManager PauseManager { get; private set; }
    public bool IsGameStarted { get; private set; }
    public static GameSession Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        PauseManager = new PauseManager();
        EventBus.Subscribe(this);
    }
    private void OnDestroy() => EventBus.Unsubscribe(this);
    
    private void Update()
    {
        if (PauseManager.IsPaused) return;
        _allTimers.Tick(Time.deltaTime);
    }

    void INewGameSubscriber.OnNewGame()
    {
        if(!IsGameStarted) EventBus.RaiseEvent<IGameStartSubscriber>(s => s.OnGameStart());
        else EventBus.RaiseEvent<IGameRestartSubscriber>(s => s.OnGameRestart());

        IsGameStarted = true;
    }
}