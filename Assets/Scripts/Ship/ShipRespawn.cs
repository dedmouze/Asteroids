using UnityEngine;

public class ShipRespawn : MonoBehaviour, IPlayerDeathParameterlessSubscriber, IGameRestartSubscriber //переписать бы этот класс как-нибудь
{
    [SerializeField] private ShipConfigSO _shipConfig;
    
    private BoxCollider2D _shipCollider;
    private ShipMovement _ship;
    private Timer _respawnTimer;
    private Timer _immortalityTimer;
    
    private void Awake()
    {
        EventBus.Subscribe(this);
        _respawnTimer = new Timer(_shipConfig.SpawnCooldown, RespawnShip);
        _immortalityTimer = new Timer(_shipConfig.ImmortalityTime, DisableImmortality);
        
        _ship = GetComponentInChildren<ShipMovement>();
        _shipCollider = _ship.GetComponent<BoxCollider2D>();
        
        ActivateImmortality();
    }
    private void OnDestroy() => EventBus.Unsubscribe(this);

    private void RespawnShip()
    {
        EventBus.RaiseEvent<IPlayerRespawnSubscriber>(s => s.OnPlayerRespawn());
        _ship.gameObject.SetActive(true);
        ActivateImmortality();
    }
    
    private void ActivateImmortality()
    {
        _shipCollider.enabled = false;
        Timers.Start(_immortalityTimer);
    }

    private void DisableImmortality() => _shipCollider.enabled = true;

    void IPlayerDeathParameterlessSubscriber.OnPlayerDeath()
    {
        _ship.gameObject.SetActive(false);
        
        _ship.transform.position = Vector3.zero;
        _ship.transform.rotation = Quaternion.identity;

        Timers.Start(_respawnTimer);
    }

    void IGameRestartSubscriber.OnGameRestart()
    {
        EventBus.RaiseEvent<IPlayerRespawnSubscriber>(s => s.OnPlayerRespawn());
        _ship.gameObject.SetActive(true);
        Timers.Stop(_immortalityTimer);
        Timers.Stop(_respawnTimer);
        ActivateImmortality();
    }
}