using System;
using UnityEngine;

public class Ship : MonoBehaviour // я бы переписал бы как нибудь этот класс
{
    [SerializeField] private float _spawnCooldown = 5f;
    [SerializeField] private float _immortalityTime = 3f;
    [SerializeField] private float _blinkFrequance = 2f;
    [SerializeField] private int _lifeCount = 3;

    private BoxCollider2D _shipCollider;
    private SpriteRenderer _shipRenderer;
    private ShipMovement _ship;
    private Timer _respawnTimer;
    private Timer _immortalityTimer;
    private float _time;

    public event Action GameOver;

    private void Awake()
    {
        _respawnTimer = new Timer(_spawnCooldown, RespawnShip, false);
        _immortalityTimer = new Timer(_immortalityTime, DisableImmortality, false);
        
        _ship = GetComponentInChildren<ShipMovement>();
        _shipCollider = _ship.GetComponent<BoxCollider2D>();
        _shipRenderer = _ship.GetComponent<SpriteRenderer>();
        
        ActivateImmortality();
    }

    private void OnEnable() => _ship.ShipBlown += OnShipBlown;
    private void OnDisable() => _ship.ShipBlown -= OnShipBlown;

    private void Update()
    {
        if (PauseManager.Instance.IsPaused) return;
        
        if (!_shipCollider.enabled)
        {
            _immortalityTimer.Start();
            _shipRenderer.color = new Color(1f, 1f, 1f, Mathf.Cos(2 * Mathf.PI * _time * _blinkFrequance));
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
        
        _lifeCount -= 1;
        if (_lifeCount == 0) GameOver?.Invoke();
        
        _respawnTimer.Start();
    }
}