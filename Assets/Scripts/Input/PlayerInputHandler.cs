using System;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour, IControlTypeSubscriber, IPlayerLifeStateSubscriber
{
    private const string HorizontalAxisName = "Horizontal";
    private const string VerticalAxisName = "Vertical";

    private Camera _mainCamera;
    private KeyCode _pauseKey;

    private ControlType _controlType;
    private bool _isPlayerDead;

    private void Awake()
    {
        EventBus.Subscribe(this);
        _mainCamera = Camera.main;
        
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        _pauseKey = KeyCode.Escape;
#else
        _pauseKey = KeyCode.P;
#endif
    }
    private void OnDestroy() => EventBus.Unsubscribe(this);

    private void Update()
    {
        if(GameSession.Instance.IsGameStarted && Input.GetKeyDown(_pauseKey)) EventBus.RaiseEvent<IGamePauseSubscriber>(s => s.OnPausePressed());

        if (GameSession.Instance.PauseManager.IsPaused || _isPlayerDead) return;
        
        switch (_controlType)
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
        if (Input.GetAxisRaw(VerticalAxisName) > 0) EventBus.RaiseEvent<IPlayerAccelerationSubscriber>(s => s.OnAcceleratePressed());

        if (Input.GetAxisRaw(VerticalAxisName) < 0) EventBus.RaiseEvent<IPlayerSlowdownSubscriber>(s => s.OnSlowdownPressed());

        if (Input.GetKey(KeyCode.Space)) EventBus.RaiseEvent<IPlayerFireSubscriber>(s => s.OnFirePressed());
        
        Vector2 direction = Input.GetAxisRaw(HorizontalAxisName) * Vector2.one;
        EventBus.RaiseEvent<IPlayerRotationSubscriber>(h => h.OnRotationProduced(direction));
    }
    private void KeyboardMouseInput()
    {
        if (Input.GetAxisRaw(VerticalAxisName) > 0 || Input.GetMouseButton(1)) EventBus.RaiseEvent<IPlayerAccelerationSubscriber>(s => s.OnAcceleratePressed());

        if (Input.GetAxisRaw(VerticalAxisName) < 0 || Input.GetMouseButton(2)) EventBus.RaiseEvent<IPlayerSlowdownSubscriber>(s => s.OnSlowdownPressed());

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) EventBus.RaiseEvent<IPlayerFireSubscriber>(s => s.OnFirePressed());

        Vector2 direction = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        EventBus.RaiseEvent<IPlayerRotationSubscriber>(s => s.OnRotationProduced(direction));
    }

    void IControlTypeSubscriber.OnControlTypeChanged(ControlType type) => _controlType = type;
    void IPlayerLifeStateSubscriber.OnPlayerLifeStateChanged(bool isDead) => _isPlayerDead = isDead;
}