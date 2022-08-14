using System;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private ShipConfigSO _shipConfig;

    private BoxCollider2D _shipCollider;
    private SpriteRenderer _shipRenderer;
    private ShipMovement _ship;
    private Timer _respawnTimer;
    private Timer _immortalityTimer;
    private float _time;

    private int _lifeCount;
    
    public event Action<Vector3> ShipBlown;
    
    private void Awake()
    {
        _respawnTimer = new Timer(_shipConfig.SpawnCooldown, RespawnShip, false);
        _immortalityTimer = new Timer(_shipConfig.ImmortalityTime, DisableImmortality, false);
        
        _ship = GetComponentInChildren<ShipMovement>();
        _shipCollider = _ship.GetComponent<BoxCollider2D>();
        _shipRenderer = _ship.GetComponent<SpriteRenderer>();
        _lifeCount = _shipConfig.LifeCount;
        
        ActivateImmortality();
    }

    private void OnEnable() => _ship.ShipBlown += OnShipBlown;
    private void OnDisable() => _ship.ShipBlown -= OnShipBlown;

    private void Update()
    {
        if (Game.Instance.PauseManager.IsPaused) return;
        
        if (!_shipCollider.enabled)
        {
            _immortalityTimer.Start();
            _shipRenderer.color = new Color(1f, 1f, 1f, Mathf.Cos(2 * Mathf.PI * _time * _shipConfig.BlinkFrequance));
            _time += Time.deltaTime;
        }
        
        _respawnTimer.Tick(Time.deltaTime);
        _immortalityTimer.Tick(Time.deltaTime);
    }
    
    private void ActivateImmortality() => _shipCollider.enabled = false;

    private void DisableImmortality()
    {
        _shipCollider.enabled = true;
        _time = 0;
    }

    private void RespawnShip()
    {
        _ship.gameObject.SetActive(true);
        ActivateImmortality();
    }

    private void OnShipBlown()
    {
        _ship.gameObject.SetActive(false);
        
        ShipBlown?.Invoke(_ship.transform.position);
        
        _ship.transform.position = Vector3.zero;
        _ship.transform.rotation = Quaternion.identity;
        
        _lifeCount -= 1;
        if (_lifeCount == 0) Game.Instance.PauseManager.GameOver();
        
        _respawnTimer.Start();
    }
}