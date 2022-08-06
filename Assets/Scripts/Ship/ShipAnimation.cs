using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShipAnimation : MonoBehaviour
{
    private readonly int _accelerate = Animator.StringToHash("Accelerate");
    
    private Animator _shipAnimator;
    private PlayerInputHandler _playerInput;
    
    private void Awake()
    {
        _playerInput = GetComponentInParent<PlayerInputHandler>();
        _shipAnimator = GetComponent<Animator>();
    }

    private void OnEnable() => _playerInput.AcceleratePressed += OnAcceleratePressed;
    private void OnDisable() => _playerInput.AcceleratePressed -= OnAcceleratePressed;
    
    private void OnAcceleratePressed() => _shipAnimator.SetTrigger(_accelerate);
}