using UnityEngine;

public class ShipRespawnBlink : MonoBehaviour, IGameRestartSubscriber, IPlayerRespawnSubscriber
{
    [SerializeField] private ShipConfigSO _shipConfig;

    private SpriteRenderer _shipRenderer;

    private bool _isRespawning;
    private float _time;
    
    private void Awake()
    {
        _shipRenderer = GetComponentInChildren<SpriteRenderer>();
        EventBus.Subscribe(this);
    }
    private void OnDestroy() => EventBus.Unsubscribe(this);

    private void Update()
    {
        if (GameSession.Instance.PauseManager.IsPaused) return;
        
        if(_isRespawning)
        {
            _shipRenderer.color = new Color(1f, 1f, 1f, Mathf.Cos(2 * Mathf.PI * _time * _shipConfig.BlinkFrequency));
            _time += Time.deltaTime;
            if (_time >= _shipConfig.ImmortalityTime)
            {
                _isRespawning = false;
                _time = 0f;
            }
        }
    }

    void IGameRestartSubscriber.OnGameRestart() => _time = 0f;
    void IPlayerRespawnSubscriber.OnPlayerRespawn() => _isRespawning = true;
}