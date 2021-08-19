using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _playerSpeed;
    private MotionCheck _motionCheck;
    public Vector3 _direction;
    public bool _isMoving = false;

    void Start()
    {
        _motionCheck = FindObjectOfType<MotionCheck>();
    }

    public void MovePlayer()
    {
        if(_motionCheck.CanMove(_direction) && _direction != Vector3.zero)
        {
            transform.Translate(_direction * _playerSpeed * Time.deltaTime);
            _isMoving = true;
        }
        if(!_motionCheck.CanMove(_direction))
            _isMoving = false;
    }
}

