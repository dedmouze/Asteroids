using UnityEngine;

public class ShipMovement : MonoBehaviour, IGameRestartSubscriber, IPlayerMovementSubscriber, IControlTypeSubscriber
{
    [SerializeField] private ShipConfigSO _shipConfig;
    
    private Camera _mainCamera;
    private ControlType _controlType;
    
    private Vector2 _velocity;
    private float _rotationVelocity;

    private Vector2 _mouseDirection;
    
    private bool _pauseState => GameSession.Instance.PauseManager.IsPaused;
    
    private void Awake()
    {
        EventBus.Subscribe(this);
        _mainCamera = Camera.main;
    }
    private void OnDestroy() => EventBus.Unsubscribe(this);

    private void Update()
    {
        if (_pauseState) return;
        
        switch (_controlType)
        {
            case ControlType.OnlyKeyboard:
            {
                transform.Rotate(new Vector3(0f, 0f, _rotationVelocity));
                break;
            }
            case ControlType.KeyboardWithMouse:
            {
                var endRotation = Quaternion.AngleAxis(-Vector2.SignedAngle(_mouseDirection, Vector2.up), Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endRotation, _rotationVelocity);
                break;
            }
        }

        transform.position += (Vector3) _velocity * Time.deltaTime;
    }
    private void LateUpdate()
    {
        if (_pauseState) return;
        
        Vector3 newPosition = transform.position;
        Vector3 viewPosition = _mainCamera.WorldToViewportPoint(newPosition);
        
        if (viewPosition.x is > 1 or < 0) newPosition.x = -newPosition.x;
        if (viewPosition.y is > 1 or < 0) newPosition.y = -newPosition.y;
        
        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ResetVelocity();
        EventBus.RaiseEvent<IPlayerDeathSubscriber>(s => s.OnPlayerDeath(transform.position));
        EventBus.RaiseEvent<IPlayerDeathParameterlessSubscriber>(s => s.OnPlayerDeath());
    }

    void IPlayerAccelerationSubscriber.OnAcceleratePressed()
    {
        Vector2 forwardDirection = transform.rotation * Vector3.up;
        
        _velocity += forwardDirection * _shipConfig.Acceleration * Time.deltaTime;
        _velocity = Vector2.ClampMagnitude(_velocity, _shipConfig.MaxVelocity);
    }
    void IPlayerSlowdownSubscriber.OnSlowdownPressed()
    {
        float deceleration = -_shipConfig.Acceleration * Time.deltaTime;
        _velocity += 2 * _velocity.normalized * deceleration / _shipConfig.SecondsToStop;

        if(_velocity.sqrMagnitude < 0.001f) _velocity = Vector2.zero;
    }
    void IPlayerRotationSubscriber.OnRotationProduced(Vector2 direction)
    {
        switch (_controlType)
        {
            case ControlType.OnlyKeyboard:
            {
                _rotationVelocity = -direction.x * _shipConfig.TurnVelocity * Time.deltaTime;
                break;
            }
            case ControlType.KeyboardWithMouse:
            {
                _mouseDirection = (direction - (Vector2)transform.position).normalized;
                _rotationVelocity = _shipConfig.TurnVelocity * Time.deltaTime;
                break;
            }
        }
    }
    
    void IGameRestartSubscriber.OnGameRestart()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        ResetVelocity();
    }

    void IControlTypeSubscriber.OnControlTypeChanged(ControlType type) => _controlType = type;
    
    private void ResetVelocity()
    {
        _velocity = Vector2.zero;
        _rotationVelocity = 0f;
    }
}