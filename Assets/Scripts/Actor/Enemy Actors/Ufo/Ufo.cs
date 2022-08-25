using UnityEngine;

public sealed class Ufo : Enemy<Ufo>
{
    private BulletFactory _bulletFactory;
    private ShipMovement _player;
    private Timer _shootTimer;
    private Transform _gunPoint;
    private Vector2 _position;
    private float _sinusoidFrequency;
    
    protected override void Awake()
    {
        base.Awake();
        
        _bulletFactory = FindObjectOfType<BulletFactory>();
        _player = FindObjectOfType<ShipMovement>();

        _shootTimer = new Timer(0f, Shoot, false, true);
        _gunPoint = transform.GetChild(0);
        ActorObject = this;
    }
    
    public void Init(Vector2 position, Vector2 direction, UfoConfigSO config, UfoFactory factory)
    {
        Direction = direction;
        Speed = config.Speed.RandomValueInRange;
        Factory = factory;
        Score = config.Score;
        _position = position;
        _shootTimer.SetNewTime(config.FireCooldown);
        Timers.Start(_shootTimer);
        _sinusoidFrequency = config.SinusoidFrequency;
        
        transform.position = position;
    }

    protected override void Update()
    {
        if (GameSession.Instance.PauseManager.IsPaused) return;

        _position += Direction * Speed * Time.deltaTime;

        Move(Direction.x == 0 ? Vector2.right : Vector2.up);
        
        if(BeyondBounds()) Reclaim(this);
    }

    private void Shoot()
    {
        if (!gameObject.activeSelf) return;
        Timers.Start(_shootTimer);
        
        Vector2 shootDirection = (_player.transform.position - transform.position).normalized;
        _gunPoint.localPosition = shootDirection * 2f;
        _bulletFactory.Create(BulletType.Ufo, _gunPoint.position, shootDirection);
    }

    private void Move(Vector2 direction) => transform.position = _position + direction * Mathf.Sin(Time.time * _sinusoidFrequency);
}