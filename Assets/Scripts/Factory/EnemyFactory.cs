using System;

public abstract class EnemyFactory<T> : Factory<T> where T : Enemy<T>
{
    public event Action<T> EnemyBlown;
    public event Action<int> DestroyedByPlayer;
    
    public override void Reclaim(T enemy)
    {
        EnemyBlown?.Invoke(enemy);
        base.Reclaim(enemy);
    }

    protected void OnDestroyedByPlayer(int score) => DestroyedByPlayer?.Invoke(score);
}