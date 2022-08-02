using System;
using UnityEngine;

public sealed class Ufo : Actor
{
    [SerializeField] private float _fireCooldown = 0.5f;
    [SerializeField] private Transform _gunPoint;
    [SerializeField] private Color _bulletColor = Color.cyan;
    [SerializeField] private float _sinusoidFrequance = 3f;
    
    private UfoFactory _ufoFactory;
    private Vector2 _direction;
    private Vector2 _position;
    private float _speed;
    private int _score;
    
    private BulletFactory _bulletFactory;
    private ShipMovement _player;
    private Timer _timer;

    public event Action<int> UfoBlown;

    protected override void Awake()
    {
        base.Awake();
        
        _bulletFactory = FindObjectOfType<BulletFactory>();
        _player = FindObjectOfType<ShipMovement>();

        _timer = new Timer(_fireCooldown, Shoot);
    }

    private void OnValidate() => _timer?.SetNewTime(_fireCooldown);

    public void Init(Vector2 position, Vector2 direction, UfoConfig config, UfoFactory factory)
    {
        transform.position = position;
        _ufoFactory = factory;
        _direction = direction;
        _position = position;
        _score = config.Score;
        _speed = config.Speed.RandomValueInRange;
    }

    private void Update()
    {
        if (PauseManager.Instance.IsPaused) return;
        
        if(BeyondBounds()) ReclaimUfo();
        
        _position += _direction * _speed * Time.deltaTime;

        if (_direction.x == 0) transform.position = _position + Vector2.right * Mathf.Sin(Time.time * _sinusoidFrequance);
        else transform.position = _position + Vector2.up * Mathf.Sin(Time.time * _sinusoidFrequance);

        _timer.Tick(Time.deltaTime);
    }

    private void Shoot()
    {
        Vector2 shootDirection = (_player.transform.position - transform.position).normalized;
        _gunPoint.localPosition = shootDirection * 2f;
        _bulletFactory.Create(BulletType.Ufo, _gunPoint.position, shootDirection, _bulletColor);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Bullet bullet))
        {
            if (bullet.BulletType == BulletType.Player)
            {
                UfoBlown?.Invoke(_score);
                ReclaimUfo();
                return;
            }
        }
        
        UfoBlown?.Invoke(0);
        ReclaimUfo();
    }

    private void ReclaimUfo() => _ufoFactory.Reclaim(this);
}

