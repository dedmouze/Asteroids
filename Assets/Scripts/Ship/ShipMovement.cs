using UnityEngine;

public sealed class ShipMovement : Actor
{
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxVelocity = 10f;
    [SerializeField] private float _turnVelocity = 180f;
    [SerializeField] private float _secondsToStop = 1f;
    
    private PlayerInputHandler _playerInput;
    private UIMainMenu _controlSettings;
    
    private Vector2 _velocity;
    private float _rotationVelocity;

    private ControlType _currentControlType = ControlType.OnlyKeyboard;
    private Vector2 _mouseDirection;
    
    protected override void Awake()
    {
        base.Awake();
        _playerInput = GetComponentInParent<PlayerInputHandler>();
        _controlSettings = FindObjectOfType<UIMainMenu>();
    }

    private void OnEnable()
    {
        _playerInput.AcceleratePressed += OnAcceleratePressed;
        _playerInput.SlowdownPressed += OnSlowdownPressed;
        _playerInput.RotationProduced += OnRotationProduced;
        _controlSettings.ControlTypeChanged += OnControlChanged;
    }
    private void OnDisable()
    {
        _playerInput.AcceleratePressed -= OnAcceleratePressed;
        _playerInput.SlowdownPressed -= OnSlowdownPressed;
        _playerInput.RotationProduced -= OnRotationProduced;
        _controlSettings.ControlTypeChanged -= OnControlChanged;
    }
    
    private void OnControlChanged(ControlType type) => _currentControlType = type;
    
    private void Update()
    {
        if (PauseManager.Instance.IsPaused) return;
        
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
        if (PauseManager.Instance.IsPaused) return;
        ScreenWrap();
    }
    
    private void OnAcceleratePressed()
    {
        Vector2 forwardDirection = transform.rotation * Vector3.up;
        
        _velocity += forwardDirection * _acceleration * Time.deltaTime;
        _velocity = Vector2.ClampMagnitude(_velocity, _maxVelocity);
    }
    
    private void OnSlowdownPressed()
    {
        float deceleration = -_acceleration * Time.deltaTime;
        _velocity += 2 * _velocity.normalized * deceleration / _secondsToStop;

        if(_velocity.sqrMagnitude < 0.001f) _velocity = Vector2.zero;
    }

    private void OnRotationProduced(Vector2 direction)
    {
        switch (_currentControlType)
        {
            case ControlType.OnlyKeyboard:
            {
                _rotationVelocity = -direction.x * _turnVelocity * Time.deltaTime;
                break;
            }
            case ControlType.KeyboardWithMouse:
            {
                _mouseDirection = (direction - (Vector2)transform.position).normalized;
                _rotationVelocity = _turnVelocity * Time.deltaTime;
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);
        _velocity = Vector2.zero;
        _rotationVelocity = 0f;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
