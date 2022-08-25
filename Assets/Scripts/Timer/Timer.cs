using System;

public class Timer : IGameRestartSubscriber
{
    private readonly bool _readyOnStart;
    private float _time;
    
    public readonly Action OnEnd;
    public readonly bool AutoStart;
    public float AccumulatedTime;

    public bool IsEnd => AccumulatedTime >= _time;
        
    public Timer(float time, Action onEnd = null, bool autoStart = false, bool readyOnStart = false)
    {
        _time = time;
        AutoStart = autoStart;
        _readyOnStart = readyOnStart;
        OnEnd = onEnd;
        EventBus.Subscribe(this);
    }
    ~Timer() => EventBus.Unsubscribe(this);

    public void SetNewTime(float time)
    {
        Reset();
        _time = time;
    }
    public void Reset() => AccumulatedTime = 0f;
    
    void IGameRestartSubscriber.OnGameRestart() => AccumulatedTime = _readyOnStart ? _time : 0f;
}