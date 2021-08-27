using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _playerSpeed;
    [SerializeField] private Joystick _joystick;
    private CharacterController _controller;

    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
    }


    void Update()
    {
        float horizontalInput = _joystick.Horizontal * _playerSpeed;
        float verticalInput = _joystick.Vertical * _playerSpeed;

        Vector3 _dir = new Vector3(horizontalInput, 0, verticalInput);

        _controller.Move(_dir * Time.deltaTime);
    }
}
