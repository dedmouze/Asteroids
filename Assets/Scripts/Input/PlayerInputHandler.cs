using System;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private const string HorizontalAxisName = "Horizontal";
    private const string VerticalAxisName = "Vertical";

    private Camera _mainCamera;
    private KeyCode PauseKey;

    public event Action AcceleratePressed;
    public event Action SlowdownPressed;
    public event Action<Vector2> RotationProduced;
    public event Action FirePressed;

    public event Action PausePressed;

    private void Awake()
    {
        _mainCamera = Camera.main;
        
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        PauseKey = KeyCode.Escape;
#else
        PauseKey = KeyCode.P;
#endif
    }

    private void Update()
    {
        if (Game.Instance.PauseManager.IsGameOver) return;

        if(Input.GetKeyDown(PauseKey)) PausePressed?.Invoke();

        if (Game.Instance.PauseManager.IsPaused) return;
        
        switch (Game.Instance.ControlType)
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
}