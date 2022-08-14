using System;
using UnityEngine;

public class Enemy<T> : Actor<T>
{
    protected int Score;
    
    public Action<T> Blown;
    public Action<int> DestroyedByPlayer;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Bullet bullet))
        {
            if (bullet.BulletType == BulletType.Player) DestroyedByPlayer?.Invoke(Score);
        }

        Blown?.Invoke(ActorObject);
        Reclaim(ActorObject);
    }
}