using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShipAnimation : MonoBehaviour
{
    private readonly int ACCELERATE = Animator.StringToHash("Accelerate");
    
    private Animator _shipAnimator;
    private PlayerInputHandler _playerInput;

    private void Awake()
    {
        _playerInput = GetComponentInParent<PlayerInputHandler>();
        _shipAnimator = GetComponent<Animator>();
    }

    private void OnEnable() => _playerInput.AcceleratePressed += OnAcceleratePressed;
    private void OnDisable() => _playerInput.AcceleratePressed -= OnAcceleratePressed;

    private void Update()
    {
        if (PauseManager.Instance.IsPaused)
        {
            _shipAnimator.enabled = false;
            return;
        }
        _shipAnimator.enabled = true;
    }
    
    private void OnAcceleratePressed() => _shipAnimator.SetTrigger(ACCELERATE);
}