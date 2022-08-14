using System;

public abstract class EnemyFactory<T> : Factory<T> where T : Enemy<T>
{
    public event Action<T> EnemyBlown;
    public event Action<int> DestroyedByPlayer;

    public override void Reclaim(T enemy)
    {
        Unsubscribe(enemy);
        base.Reclaim(enemy);
    }
    
    protected void Subscribe(T enemy)
    {
        enemy.Blown += OnBlown;
        enemy.DestroyedByPlayer += OnDestroyedByPlayer;
    }

    private void Unsubscribe(T enemy)
    {
        enemy.Blown -= OnBlown;
        enemy.DestroyedByPlayer -= OnDestroyedByPlayer;
    }
    
    private void OnBlown(T enemy) => EnemyBlown?.Invoke(enemy);
    private void OnDestroyedByPlayer(int score) => DestroyedByPlayer?.Invoke(score);
}