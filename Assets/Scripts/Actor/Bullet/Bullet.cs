using UnityEngine;

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
    
    public void Init(BulletType type, Vector2 position, Vector2 direction, float speed, Color color, BulletFactory factory)
    {
        Speed = speed;
        BulletType = type;
        Direction = direction;
        Factory = factory;
        _bulletRenderer.color = color;

        Quaternion rotation = Quaternion.AngleAxis(-Vector2.SignedAngle(direction, Vector2.up), Vector3.forward);
        transform.SetPositionAndRotation(position, rotation);
    }
}