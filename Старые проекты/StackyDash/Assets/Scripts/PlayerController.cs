using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }
    void Update()
    {
        _playerMovement.MovePlayer();
    }
}
