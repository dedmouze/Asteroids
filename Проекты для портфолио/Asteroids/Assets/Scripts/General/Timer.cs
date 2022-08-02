using System;

public class Timer
{
    private readonly Action _onEnd;
    private float _accumulatedTime;
    private float _time;

    public bool IsEnd;
    
    public Timer(float time, Action onEnd = null)
    {
        if (time < 0) throw new ArgumentOutOfRangeException();

        _onEnd = onEnd;
        _time = time;

        if(onEnd == null) _accumulatedTime = time;
    }

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
        else _accumulatedTime += deltaTime;
    }

    public void ResetTimer() => _accumulatedTime -= _time;

    public void SetNewTime(float time)
    {
        if (time < 0) throw new ArgumentOutOfRangeException();

        _time = time;
        _accumulatedTime = 0;
        IsEnd = false;
    }
}