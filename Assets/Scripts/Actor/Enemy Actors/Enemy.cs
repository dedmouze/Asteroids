using UnityEngine;

public class Enemy<T> : Actor<T>
{
    protected int Score;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Bullet bullet))
        {
            if (bullet.BulletType == BulletType.Player) EventBus.RaiseEvent<IEnemyDeadByPlayerSubscriber>(s => s.OnEnemyDeadByPlayer(Score));
        }
        
        EventBus.RaiseEvent<IEnemyDeathSubscriber<T>>(s => s.OnEnemyDeath(ActorObject));
        Reclaim(ActorObject);
    }
}