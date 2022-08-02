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
    private GameObject _ship;
    private Timer _respawnTimer;
    private Timer _immortalityTimer;
    
    private float _time;
    // Вместо добавления события ShipDestroyed в класс ShipMovement, на которое подпишется только этот класс Ship,
    // я просто буду сохранять предыдущее состояние корабля, и если произошла смена с true на false, значит будем отнимать одну жизнь
    private bool _previousShipActiveState = true;

    public event Action LifeDecreased;
    public event Action GameOver;

    private void Awake()
    {
        _respawnTimer = new Timer(_spawnCooldown, RespawnShip);
        _immortalityTimer = new Timer(_immortalityTime, DisableImmortality);
        
        _ship = GetComponentInChildren<ShipMovement>().gameObject;
        _shipCollider = _ship.GetComponent<BoxCollider2D>();
        _shipRenderer = _ship.GetComponent<SpriteRenderer>();
        
        ActivateImmortality();
    }

    private void Update()
    {
        if (PauseManager.Instance.IsPaused) return;

        if (_previousShipActiveState != _ship.activeSelf)
        {
            _lifeCount -= 1;
            LifeDecreased?.Invoke();
        }

        if (_lifeCount == 0)
        {
            GameOver?.Invoke();
            return;
        }

        if(!_ship.activeSelf) _respawnTimer.Tick(Time.deltaTime);

        if (!_shipCollider.enabled)
        {
            _immortalityTimer.Tick(Time.deltaTime);
            _shipRenderer.color = new Color(1f, 1f, 1f, Mathf.Cos(2 * Mathf.PI * _time * _blinkFrequance));
            _time += Time.deltaTime;
        }
        
        _previousShipActiveState = _ship.activeSelf;
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
}