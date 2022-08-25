using System.Linq;
using System.Collections.Generic;

public class Timers
{
    private static readonly List<Timer> _timers = new List<Timer>();

    public static void Start(Timer timer)
    {
        timer.Reset();
        _timers.Add(timer);
    }

    public static void Stop(Timer timer) => _timers.Remove(timer);

    public void Tick(float deltaTime)
    {
        foreach (var timer in _timers.ToList())
        {
            timer.AccumulatedTime += deltaTime;

            if (timer.IsEnd)
            {
                timer.OnEnd?.Invoke();

                if (timer.AutoStart) timer.Reset();
                else _timers.Remove(timer);
            }
        }
    }
}