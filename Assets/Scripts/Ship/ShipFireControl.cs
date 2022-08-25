using UnityEngine;

public class ShipFireControl : MonoBehaviour, IPlayerFireSubscriber
{
    [SerializeField] private ShipConfigSO _shipConfig;
    
    private BulletFactory _bulletFactory;
    private Transform _gunPoint;
    private Timer _shootTimer;
    
    private void Awake()
    {
        _bulletFactory = FindObjectOfType<BulletFactory>();

        _shootTimer = new Timer(_shipConfig.FireCooldown, null, false, true);
        Timers.Start(_shootTimer);
        _shootTimer.AccumulatedTime = _shipConfig.FireCooldown; //таймер игрока сразу готов

        _gunPoint = transform.GetChild(0);
        EventBus.Subscribe(this);
    }
    private void OnDestroy() => EventBus.Unsubscribe(this);
    
    void IPlayerFireSubscriber.OnFirePressed()
    {
        if (!_shootTimer.IsEnd) return;
        Timers.Start(_shootTimer);

        _bulletFactory.Create(BulletType.Player, _gunPoint.position, transform.rotation * Vector3.up);
    }
}