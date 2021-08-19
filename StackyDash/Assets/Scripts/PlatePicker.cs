using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatePicker : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Plate"))
        {
            var offset = new Vector3(0, 0.2f, 0);
            other.transform.SetParent(_player.transform);
            other.transform.position = transform.position - offset;
            transform.position = transform.position - offset;
            _player.transform.position = _player.transform.position + offset;
        }
    }
}
