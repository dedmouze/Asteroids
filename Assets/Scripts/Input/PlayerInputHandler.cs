using System;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private const string HorizontalAxisName = "Horizontal";
    private const string VerticalAxisName = "Vertical";

    private UIMainMenu _controlSettings;
    private ControlType _currentControlType = ControlType.OnlyKeyboard;
    
    private Camera _mainCamera;
    
    public event Action AcceleratePressed;
    public event Action SlowdownPressed;
    public event Action<Vector2> RotationProduced;
    public event Action FirePressed;

    public event Action PausePressed;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _controlSettings = FindObjectOfType<UIMainMenu>();
    }

    private void OnEnable() => _controlSettings.ControlTypeChanged += OnControlChanged;
    private void OnDisable() => _controlSettings.ControlTypeChanged -= OnControlChanged;

    private void Update()
    {
        if (PauseManager.Instance.IsGameOver) return;
        
        if(Input.GetKeyDown(KeyCode.Escape)) PausePressed?.Invoke();

        if (PauseManager.Instance.IsPaused) return;
        
        switch (_currentControlType)
        {
            case ControlType.OnlyKeyboard: KeyboardInput();
                return;
            case ControlType.KeyboardWithMouse: KeyboardMouseInput();
                return;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    private void KeyboardInput()
    {
        if (Input.GetAxisRaw(VerticalAxisName) > 0) AcceleratePressed?.Invoke();

        if (Input.GetAxisRaw(VerticalAxisName) < 0) SlowdownPressed?.Invoke();

        if (Input.GetKey(KeyCode.Space)) FirePressed?.Invoke();
        
        RotationProduced?.Invoke(Input.GetAxisRaw(HorizontalAxisName) * Vector2.one);
    }
    private void KeyboardMouseInput()
    {
        if (Input.GetAxisRaw(VerticalAxisName) > 0 || Input.GetMouseButton(1)) AcceleratePressed?.Invoke();

        if (Input.GetAxisRaw(VerticalAxisName) < 0 || Input.GetMouseButton(2)) SlowdownPressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) FirePressed?.Invoke();

        RotationProduced?.Invoke(_mainCamera.ScreenToWorldPoint(Input.mousePosition));
    }
    
    private void OnControlChanged(ControlType type) => _currentControlType = type;
}
