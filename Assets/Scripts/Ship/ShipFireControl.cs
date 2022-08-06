using UnityEngine;

public class ShipFireControl : MonoBehaviour
{
    [SerializeField] private float _fireCooldown = 1f / 3f;
    [SerializeField] private Color _bulletColor = Color.green;
    [SerializeField] private Transform _gunPoint;
    
    private BulletFactory _bulletFactory;
    private PlayerInputHandler _playerInput;
    private Timer _timer;
    
    private void Awake()
    {
        _bulletFactory = FindObjectOfType<BulletFactory>();
        _playerInput = GetComponentInParent<PlayerInputHandler>();
        
        _timer = new Timer(_fireCooldown);
    }

    private void OnValidate() => _timer?.SetNewTime(_fireCooldown);

    private void OnEnable() => _playerInput.FirePressed += OnFirePressed;
    private void OnDisable() => _playerInput.FirePressed -= OnFirePressed;
    
    private void Update() => _timer.Tick(Time.deltaTime);

    private void OnFirePressed()
    {
        if (!_timer.IsEnd) return;
        _timer.ResetTimer();
        
        _bulletFactory.Create(BulletType.Player, _gunPoint.position, transform.rotation * Vector3.up, _bulletColor);
    }
    
}