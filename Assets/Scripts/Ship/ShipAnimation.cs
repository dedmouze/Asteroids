using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShipAnimation : MonoBehaviour, IGameRestartSubscriber, IPlayerAccelerationSubscriber
{
    private readonly int _accelerate = Animator.StringToHash("Accelerate");
    private readonly int _resetAccelerate = Animator.StringToHash("Reset Accelerate");
    
    private Animator _shipAnimator;

    private void Awake()
    {
        EventBus.Subscribe(this);
        _shipAnimator = GetComponent<Animator>();
    }
    private void OnDestroy() => EventBus.Unsubscribe(this);

    private void Update()
    {
        if (GameSession.Instance.PauseManager.IsPaused)
        {
            _shipAnimator.enabled = false;
            return;
        }
        _shipAnimator.enabled = true;
    }

    void IGameRestartSubscriber.OnGameRestart() => _shipAnimator.SetTrigger(_resetAccelerate);
    void IPlayerAccelerationSubscriber.OnAcceleratePressed() => _shipAnimator.SetTrigger(_accelerate);
}