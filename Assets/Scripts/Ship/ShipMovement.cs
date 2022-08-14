using System;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private ShipConfigSO _shipConfig;
    
    private PlayerInputHandler _playerInput;
    private Camera _mainCamera;

    private ControlType _currentControlType => Game.Instance.ControlType;
    private bool _pauseState => Game.Instance.PauseManager.IsPaused;
    
    private Vector2 _velocity;
    private float _rotationVelocity;

    private Vector2 _mouseDirection;

    public event Action ShipBlown;

    private void Awake()
    {
        _playerInput = GetComponentInParent<PlayerInputHandler>();
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _playerInput.AcceleratePressed += OnAcceleratePressed;
        _playerInput.SlowdownPressed += OnSlowdownPressed;
        _playerInput.RotationProduced += OnRotationProduced;
    }
    private void OnDisable()
    {
        _playerInput.AcceleratePressed -= OnAcceleratePressed;
        _playerInput.SlowdownPressed -= OnSlowdownPressed;
        _playerInput.RotationProduced -= OnRotationProduced;
    }
    
    private void Update()
    {
        if (_pauseState) return;
        
        switch (_currentControlType)
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
    
    private void OnAcceleratePressed()
    {
        Vector2 forwardDirection = transform.rotation * Vector3.up;
        
        _velocity += forwardDirection * _shipConfig.Acceleration * Time.deltaTime;
        _velocity = Vector2.ClampMagnitude(_velocity, _shipConfig.MaxVelocity);
    }
    
    private void OnSlowdownPressed()
    {
        float deceleration = -_shipConfig.Acceleration * Time.deltaTime;
        _velocity += 2 * _velocity.normalized * deceleration / _shipConfig.SecondsToStop;

        if(_velocity.sqrMagnitude < 0.001f) _velocity = Vector2.zero;
    }

    private void OnRotationProduced(Vector2 direction)
    {
        switch (_currentControlType)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        ShipBlown?.Invoke();
        _velocity = Vector2.zero;
        _rotationVelocity = 0f;
    }
}