using System;

public class Timer
{
    private readonly Action _onEnd;
    private readonly bool _autoStart;
    private float _accumulatedTime;
    private float _time;
    private int _startFactor;
    public bool IsEnd;
    
    public Timer(float time, Action onEnd = null, bool autoStart = true)
    {
        if (time < 0) throw new ArgumentOutOfRangeException();

        _onEnd = onEnd;
        _time = time;
        _autoStart = autoStart;
        
        if(autoStart) Start();
        
        if(onEnd == null) _accumulatedTime = time;
    }

    public void Start() => _startFactor = 1;

    public void Tick(float deltaTime)
    {
        if (PauseManager.Instance.IsPaused) return;
        
        IsEnd = _accumulatedTime >= _time;
        
        if (IsEnd)
        {
            if (_onEnd == null) return;
            _onEnd.Invoke(); 
            ResetTimer();
        }
        else _accumulatedTime += deltaTime * _startFactor;
    }

    public void ResetTimer()
    {
        _accumulatedTime -= _time;
        if (!_autoStart) _startFactor = 0;
    }

    public void SetNewTime(float time)
    {
        if (time < 0) throw new ArgumentOutOfRangeException();

        _time = time;
        _accumulatedTime = 0;
        IsEnd = false;
    }
}