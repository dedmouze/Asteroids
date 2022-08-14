using System;
using UnityEngine;

public sealed class Bullet : Actor<Bullet>
{
    private SpriteRenderer _bulletRenderer;

    public event Action<Vector2, BulletType> BulletShot;
    public event Action<Vector2, BulletType> BulletBlown;

    public BulletType BulletType { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _bulletRenderer = GetComponent<SpriteRenderer>();
        ActorObject = this;
    }
    
    public void Init(BulletType type, Vector2 position, Vector2 direction, BulletConfigSO config, BulletFactory factory)
    {
        Speed = config.Speed;
        BulletType = type;
        Direction = direction;
        Factory = factory;
        _bulletRenderer.color = config.Color;

        Quaternion rotation = Quaternion.AngleAxis(-Vector2.SignedAngle(direction, Vector2.up), Vector3.forward);
        transform.SetPositionAndRotation(position, rotation);
        
        BulletShot?.Invoke(position, type);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        BulletBlown?.Invoke(transform.position, BulletType);
        Reclaim(ActorObject);
    }
}