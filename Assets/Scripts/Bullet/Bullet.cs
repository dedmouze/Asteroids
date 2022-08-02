using UnityEngine;

public sealed class Bullet : Actor
{
    [SerializeField] private float _bulletSpeed = 10f;
    
    private Vector2 _forwardDirection;
    private SpriteRenderer _bulletRenderer;
    private BulletFactory _bulletFactory;
    
    public BulletType BulletType { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _bulletRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Init(BulletType type, Vector2 position, Vector2 direction, Color color, BulletFactory factory)
    {
        BulletType = type;
        _bulletFactory = factory;
        _forwardDirection = direction;
        _bulletRenderer.color = color;

        Quaternion rotation = Quaternion.AngleAxis(-Vector2.SignedAngle(direction, Vector2.up), Vector3.forward);
        transform.SetPositionAndRotation(position, rotation);
    }

    private void Update()
    {
        if (PauseManager.Instance.IsPaused) return;
        
        if(BeyondBounds()) ReclaimBullet();
        
        transform.position += (Vector3)(_bulletSpeed * _forwardDirection * Time.deltaTime);
    }

    private void ReclaimBullet() => _bulletFactory.Reclaim(this);

    private void OnTriggerEnter2D(Collider2D other) => ReclaimBullet();
}