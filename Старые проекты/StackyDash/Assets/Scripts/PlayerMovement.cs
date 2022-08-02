using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private MotionCheck _motionCheck;
    private bool _isMoving;
    private Vector3 _direction;

    void Start()
    {
        _motionCheck = FindObjectOfType<MotionCheck>();
    }

    public void MovePlayer()
    {
        if (!_motionCheck.CanMove(_direction))
        {
            _isMoving = false;
        }
        else
        {
            _isMoving = true;
            transform.Translate(_direction * Time.deltaTime * _speed);
        }
        
    }

    public void SetDirection(Vector3 direction)
    {
        if (_isMoving)
        {
            return;
        }
        _direction = direction;
    }
}

