using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Player _player;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    private void Awake()
    {
        _player = GetComponent<Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (_player.Speed.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        if (_player.Speed.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        _animator.SetFloat("Speed", Mathf.Abs(_player.Speed.x));
    }
}
