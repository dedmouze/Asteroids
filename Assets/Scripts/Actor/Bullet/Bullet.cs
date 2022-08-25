using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public sealed class Bullet : Actor<Bullet>
{
    private SpriteRenderer _bulletRenderer;

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

        EventBus.RaiseEvent<IBulletActionSubscriber>(s => s.OnBulletAction(position, BulletType));
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        EventBus.RaiseEvent<IBulletActionSubscriber>(s => s.OnBulletAction(transform.position, BulletType));
        Reclaim(ActorObject);
    }
}